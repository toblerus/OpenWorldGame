using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public class KenneyAssetViewer : EditorWindow
{
    const string PrefRootKey = "KenneyAssetViewer.Root";
    const string PrefFolderKey = "KenneyAssetViewer.SelectedFolder";
    const string PrefTypesKey = "KenneyAssetViewer.Types";
    const string PrefThumbKey = "KenneyAssetViewer.Thumb";
    const string PrefRecurseKey = "KenneyAssetViewer.Recurse";
    const string PrefDestKey = "KenneyAssetViewer.Dest";

    string rootPath;
    string currentFolder;
    string search;
    string[] typeOptions = new[] { ".fbx", ".obj", ".glb" };
    HashSet<string> typeFilter = new HashSet<string>(new[] { ".fbx", ".obj", ".glb" });
    Vector2 folderScroll;
    Vector2 gridScroll;
    float thumbSize = 112f;
    bool includeSubfolders = true;
    string destRoot = "Assets/KenneyImported";

    readonly Dictionary<string, Texture2D> previewCache = new Dictionary<string, Texture2D>();
    readonly HashSet<string> selected = new HashSet<string>();
    readonly List<Item> items = new List<Item>();
    readonly ConcurrentQueue<Item> pendingItems = new ConcurrentQueue<Item>();

    CancellationTokenSource scanCts;
    bool scanning;
    int scannedCount;
    string scanInfo;
    double nextRepaint;
    string previewBaseForFolder;

    class Item
    {
        public string FilePath;
        public string PreviewPath;
        public Texture2D Preview;
        public string DisplayName;
        public string RelativeFromRoot;
    }

    [MenuItem("Tools/Kenney Asset Viewer")]
    static void OpenWindow()
    {
        var w = GetWindow<KenneyAssetViewer>("Kenney Asset Viewer");
        w.minSize = new Vector2(760, 460);
        w.Show();
    }

    void OnEnable()
    {
        rootPath = EditorPrefs.GetString(PrefRootKey, string.Empty);
        currentFolder = EditorPrefs.GetString(PrefFolderKey, string.Empty);
        var savedTypes = EditorPrefs.GetString(PrefTypesKey, ".fbx;.obj;.glb");
        typeFilter = new HashSet<string>(savedTypes.Split(';').Where(s => !string.IsNullOrEmpty(s)));
        thumbSize = EditorPrefs.GetFloat(PrefThumbKey, 112f);
        includeSubfolders = EditorPrefs.GetBool(PrefRecurseKey, true);
        destRoot = EditorPrefs.GetString(PrefDestKey, "Assets/KenneyImported");
        if (string.IsNullOrEmpty(currentFolder)) currentFolder = rootPath;
        EditorApplication.update += OnEditorUpdate;
        RefreshItems();
    }

    void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
        CancelScan();
        foreach (var kv in previewCache) DestroyImmediate(kv.Value);
        previewCache.Clear();
    }

    void OnEditorUpdate()
    {
        var updated = false;
        while (pendingItems.TryDequeue(out var it))
        {
            items.Add(it);
            updated = true;
        }
        if (updated && EditorApplication.timeSinceStartup > nextRepaint)
        {
            nextRepaint = EditorApplication.timeSinceStartup + 0.05;
            Repaint();
        }
    }

    void OnGUI()
    {
        DrawToolbar();
        if (string.IsNullOrEmpty(rootPath) || !Directory.Exists(rootPath))
        {
            EditorGUILayout.HelpBox("Set Root to the unpacked Kenney folder.", MessageType.Info);
            return;
        }
        EditorGUILayout.BeginHorizontal();
        DrawFolderBrowser();
        DrawAssetGrid();
        EditorGUILayout.EndHorizontal();
        DrawFooter();
    }

    void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        if (GUILayout.Button("Set Root", EditorStyles.toolbarButton))
        {
            var picked = EditorUtility.OpenFolderPanel("Select Kenney Pack Root", string.IsNullOrEmpty(rootPath) ? "" : rootPath, "");
            if (!string.IsNullOrEmpty(picked))
            {
                rootPath = picked;
                currentFolder = rootPath;
                EditorPrefs.SetString(PrefRootKey, rootPath);
                EditorPrefs.SetString(PrefFolderKey, currentFolder);
                RefreshItems();
            }
        }
        EditorGUILayout.LabelField(string.IsNullOrEmpty(rootPath) ? "(no root set)" : Shorten(rootPath), GUILayout.MaxWidth(position.width * 0.5f));
        GUILayout.FlexibleSpace();
        var newThumb = GUILayout.HorizontalSlider(thumbSize, 64f, 192f, GUILayout.Width(160));
        if (!Mathf.Approximately(newThumb, thumbSize))
        {
            thumbSize = newThumb;
            EditorPrefs.SetFloat(PrefThumbKey, thumbSize);
            Repaint();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Current Folder", GUILayout.Width(100));
        EditorGUILayout.SelectableLabel(string.IsNullOrEmpty(currentFolder) ? "(none)" : currentFolder, GUILayout.Height(16));
        if (GUILayout.Button("Browse", GUILayout.Width(90)))
        {
            var picked = EditorUtility.OpenFolderPanel("Select Subfolder", string.IsNullOrEmpty(currentFolder) ? rootPath : currentFolder, "");
            if (!string.IsNullOrEmpty(picked))
            {
                if (!picked.StartsWith(rootPath))
                {
                    EditorUtility.DisplayDialog("Invalid Folder", "Folder must be inside the configured root.", "OK");
                }
                else
                {
                    currentFolder = picked;
                    EditorPrefs.SetString(PrefFolderKey, currentFolder);
                    RefreshItems();
                }
            }
        }
        GUILayout.FlexibleSpace();
        includeSubfolders = EditorGUILayout.ToggleLeft("Include Subfolders", includeSubfolders, GUILayout.Width(150));
        if (GUI.changed) EditorPrefs.SetBool(PrefRecurseKey, includeSubfolders);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Filter", GUILayout.Width(100));
        var newSearch = EditorGUILayout.TextField(search);
        if (newSearch != search)
        {
            search = newSearch;
            RefreshItems();
        }
        GUILayout.FlexibleSpace();
        for (var i = 0; i < typeOptions.Length; i++)
        {
            var ext = typeOptions[i];
            var on = typeFilter.Contains(ext);
            var toggled = GUILayout.Toggle(on, ext.ToUpperInvariant(), "Button", GUILayout.Width(60));
            if (toggled != on)
            {
                if (toggled) typeFilter.Add(ext); else typeFilter.Remove(ext);
                var joined = string.Join(";", typeFilter);
                EditorPrefs.SetString(PrefTypesKey, joined);
                RefreshItems();
            }
        }
        GUILayout.FlexibleSpace();
        if (scanning)
        {
            EditorGUILayout.LabelField(string.IsNullOrEmpty(scanInfo) ? "Scanning..." : scanInfo, GUILayout.Width(260));
            if (GUILayout.Button("Cancel", GUILayout.Width(80))) CancelScan();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(4);
    }

    void DrawFolderBrowser()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(320));
        EditorGUILayout.LabelField("Folders", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Up", GUILayout.Width(48)))
        {
            if (!string.IsNullOrEmpty(currentFolder) && currentFolder != rootPath)
            {
                var parent = Directory.GetParent(currentFolder)?.FullName;
                if (!string.IsNullOrEmpty(parent) && parent.Replace("\\", "/").StartsWith(rootPath.Replace("\\", "/")))
                {
                    currentFolder = parent;
                    EditorPrefs.SetString(PrefFolderKey, currentFolder);
                    RefreshItems();
                }
            }
        }
        DrawBreadcrumbs();
        EditorGUILayout.EndHorizontal();

        folderScroll = EditorGUILayout.BeginScrollView(folderScroll);
        if (Directory.Exists(currentFolder))
        {
            var dirs = SafeGetDirs(currentFolder).OrderBy(d => d, System.StringComparer.OrdinalIgnoreCase);
            foreach (var d in dirs)
            {
                EditorGUILayout.BeginHorizontal();
                var name = Path.GetFileName(d);
                if (GUILayout.Button(name, GUILayout.Height(22)))
                {
                    currentFolder = d;
                    EditorPrefs.SetString(PrefFolderKey, currentFolder);
                    RefreshItems();
                }
                if (GUILayout.Button("Open", GUILayout.Width(60)))
                {
                    currentFolder = d;
                    EditorPrefs.SetString(PrefFolderKey, currentFolder);
                    RefreshItems();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void DrawBreadcrumbs()
    {
        var normRoot = rootPath.Replace("\\", "/").TrimEnd('/');
        var normCurrent = currentFolder.Replace("\\", "/");
        var segments = normCurrent.StartsWith(normRoot) ? normCurrent.Substring(normRoot.Length).TrimStart('/').Split(new[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries) : new string[0];
        if (GUILayout.Button("Root", GUILayout.Height(22))) { currentFolder = rootPath; EditorPrefs.SetString(PrefFolderKey, currentFolder); RefreshItems(); }
        for (var i = 0; i < segments.Length; i++)
        {
            EditorGUILayout.LabelField(">", GUILayout.Width(10));
            var idx = i;
            if (GUILayout.Button(segments[i], GUILayout.Height(22)))
            {
                var path = normRoot;
                for (var s = 0; s <= idx; s++) path = Path.Combine(path, segments[s]);
                currentFolder = path.Replace("\\", "/");
                EditorPrefs.SetString(PrefFolderKey, currentFolder);
                RefreshItems();
            }
        }
    }

    void DrawAssetGrid()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Assets", EditorStyles.boldLabel);
        gridScroll = EditorGUILayout.BeginScrollView(gridScroll);
        var viewWidth = position.width - 340f;
        var cell = thumbSize + 36f;
        var columns = Mathf.Max(1, Mathf.FloorToInt(viewWidth / cell));
        var rows = Mathf.CeilToInt((float)items.Count / columns);
        var idx = 0;
        for (var r = 0; r < rows; r++)
        {
            EditorGUILayout.BeginHorizontal();
            for (var c = 0; c < columns; c++)
            {
                if (idx >= items.Count)
                {
                    GUILayout.FlexibleSpace();
                    continue;
                }
                var it = items[idx++];
                EditorGUILayout.BeginVertical(GUILayout.Width(cell), GUILayout.Height(cell));
                var tex = GetPreview(it);
                var rect = GUILayoutUtility.GetRect(thumbSize, thumbSize, GUILayout.Width(thumbSize), GUILayout.Height(thumbSize));
                if (tex != null) GUI.DrawTexture(rect, tex, ScaleMode.ScaleToFit, true);
                else EditorGUI.HelpBox(rect, "No preview", MessageType.None);
                EditorGUILayout.LabelField(it.DisplayName, GUILayout.Height(16));
                EditorGUILayout.BeginHorizontal();
                var wasSel = selected.Contains(it.FilePath);
                var nowSel = GUILayout.Toggle(wasSel, "Select", GUILayout.Width(60));
                if (nowSel != wasSel)
                {
                    if (nowSel) selected.Add(it.FilePath); else selected.Remove(it.FilePath);
                }
                if (GUILayout.Button("Import", GUILayout.Width(60))) ImportOne(it.FilePath);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void DrawFooter()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Destination", GUILayout.Width(100));
        destRoot = EditorGUILayout.TextField(destRoot);
        if (GUILayout.Button("Open", GUILayout.Width(60)))
        {
            var abs = Path.GetFullPath(destRoot);
            if (Directory.Exists(abs)) EditorUtility.RevealInFinder(abs);
            else EditorUtility.DisplayDialog("Not Found", "Destination folder does not exist yet.", "OK");
        }
        GUILayout.FlexibleSpace();
        var count = selected.Count;
        var label = count == 0 ? "Import None" : $"Import {count} Selected";
        GUI.enabled = count > 0;
        if (GUILayout.Button(label, GUILayout.Width(160)))
        {
            var list = selected.ToList();
            for (var i = 0; i < list.Count; i++) ImportOne(list[i]);
            selected.Clear();
        }
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
        EditorPrefs.SetString(PrefDestKey, destRoot);
    }

    void RefreshItems()
    {
        CancelScan();
        items.Clear();
        selected.Clear();
        previewBaseForFolder = ResolvePreviewBase(currentFolder, rootPath);
        if (string.IsNullOrEmpty(rootPath) || string.IsNullOrEmpty(currentFolder)) return;
        if (!Directory.Exists(currentFolder)) return;
        var filters = typeFilter.ToArray();
        scanning = true;
        scannedCount = 0;
        scanInfo = "";
        scanCts = new CancellationTokenSource();
        var token = scanCts.Token;
        Task.Run(() =>
        {
            try
            {
                var opt = includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                for (var f = 0; f < filters.Length; f++)
                {
                    if (token.IsCancellationRequested) break;
                    var ext = filters[f];
                    foreach (var path in SafeEnumerate(currentFolder, "*" + ext, opt))
                    {
                        if (token.IsCancellationRequested) break;
                        scannedCount++;
                        if (!string.IsNullOrEmpty(search) && !Path.GetFileName(path).ToLowerInvariant().Contains(search.ToLowerInvariant())) continue;
                        var rel = MakeRelative(rootPath, path);
                        var disp = Path.GetFileNameWithoutExtension(path);
                        var prev = GuessPreviewForModel(path, previewBaseForFolder);
                        var it = new Item { FilePath = path, PreviewPath = prev, DisplayName = disp, RelativeFromRoot = rel };
                        pendingItems.Enqueue(it);
                        if (scannedCount % 50 == 0) scanInfo = $"Found {scannedCount}";
                    }
                }
            }
            finally
            {
                scanning = false;
                scanInfo = $"Found {scannedCount}";
            }
        }, token);
    }

    void CancelScan()
    {
        if (scanCts != null)
        {
            scanCts.Cancel();
            scanCts.Dispose();
            scanCts = null;
        }
        scanning = false;
        scanInfo = "";
        while (pendingItems.TryDequeue(out var _)) { }
    }

    Texture2D GetPreview(Item it)
    {
        if (it.Preview != null) return it.Preview;
        if (string.IsNullOrEmpty(it.PreviewPath) || !File.Exists(it.PreviewPath)) return null;
        if (previewCache.TryGetValue(it.PreviewPath, out var texCached)) return texCached;
        var bytes = File.ReadAllBytes(it.PreviewPath);
        var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(bytes);
        previewCache[it.PreviewPath] = tex;
        it.Preview = tex;
        return tex;
    }

    void ImportOne(string srcFile)
    {
        if (string.IsNullOrEmpty(srcFile) || !File.Exists(srcFile)) return;
        var rel = MakeRelative(rootPath, srcFile);
        var dest = Path.Combine(destRoot, rel).Replace("\\", "/");
        var destDir = Path.GetDirectoryName(dest);
        if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
        File.Copy(srcFile, dest, true);
        var ext = Path.GetExtension(srcFile).ToLowerInvariant();
        if (ext == ".obj")
        {
            var mtl = Path.ChangeExtension(srcFile, ".mtl");
            if (File.Exists(mtl))
            {
                var destMtl = Path.Combine(destDir, Path.GetFileName(mtl)).Replace("\\", "/");
                File.Copy(mtl, destMtl, true);
            }
        }
        EnsureTexturesForModel(srcFile);
        AssetDatabase.ImportAsset(dest, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
        AssetDatabase.Refresh();
    }

    void EnsureTexturesForModel(string srcFile)
    {
        var dir = new DirectoryInfo(Path.GetDirectoryName(srcFile));
        var modelsDir = dir;
        while (modelsDir != null && !string.Equals(modelsDir.Name, "Models", System.StringComparison.OrdinalIgnoreCase)) modelsDir = modelsDir.Parent;
        if (modelsDir == null) return;
        var texturesSrc = Path.Combine(modelsDir.FullName, "Textures");
        if (!Directory.Exists(texturesSrc)) return;
        var relTextures = MakeRelative(rootPath, texturesSrc);
        var destTextures = Path.Combine(destRoot, relTextures).Replace("\\", "/");
        DirectoryCopy(texturesSrc, destTextures);
        AssetDatabase.ImportAsset(destTextures, ImportAssetOptions.ForceUpdate);
    }

    void DirectoryCopy(string sourceDir, string destDir)
    {
        if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
        var files = Directory.GetFiles(sourceDir);
        for (var i = 0; i < files.Length; i++)
        {
            var f = files[i];
            var target = Path.Combine(destDir, Path.GetFileName(f));
            File.Copy(f, target, true);
        }
        var dirs = Directory.GetDirectories(sourceDir);
        for (var i = 0; i < dirs.Length; i++)
        {
            var d = dirs[i];
            var subDest = Path.Combine(destDir, Path.GetFileName(d));
            DirectoryCopy(d, subDest);
        }
    }

    static string[] SafeGetDirs(string path)
    {
        try { return Directory.GetDirectories(path); } catch { return new string[0]; }
    }

    static IEnumerable<string> SafeEnumerate(string path, string pattern, SearchOption opt)
    {
        try { return Directory.EnumerateFiles(path, pattern, opt); } catch { return Enumerable.Empty<string>(); }
    }

    static string MakeRelative(string root, string full)
    {
        var r = root.Replace("\\", "/").TrimEnd('/');
        var f = full.Replace("\\", "/");
        if (f.StartsWith(r)) return f.Substring(r.Length).TrimStart('/');
        return Path.GetFileName(full);
    }

    static string Shorten(string p)
    {
        if (string.IsNullOrEmpty(p)) return p;
        var s = p.Replace("\\", "/");
        if (s.Length <= 64) return s;
        var head = s.Substring(0, 24);
        var tail = s.Substring(s.Length - 30);
        return head + "..." + tail;
    }

    static string ResolvePreviewBase(string selected, string root)
    {
        if (string.IsNullOrEmpty(selected)) return null;
        var norm = selected.Replace("\\", "/");
        var parts = norm.Split('/');
        var idxModels = System.Array.IndexOf(parts, "Models");
        if (idxModels >= 0)
        {
            var kitRoot = string.Join("/", parts.Take(idxModels));
            var candidate = Path.Combine(kitRoot, "Previews").Replace("\\", "/");
            if (Directory.Exists(candidate)) return candidate;
        }
        var idx3d = System.Array.IndexOf(parts, "3D assets");
        if (idx3d >= 0)
        {
            var upToKit = string.Join("/", parts.Take(idx3d + 2));
            var candidate = Path.Combine(upToKit, "Previews").Replace("\\", "/");
            if (Directory.Exists(candidate)) return candidate;
        }
        var direct = Path.Combine(selected, "Previews").Replace("\\", "/");
        if (Directory.Exists(direct)) return direct;
        return null;
    }

    static string GuessPreviewForModel(string modelPath, string previewBase)
    {
        if (string.IsNullOrEmpty(previewBase)) return null;
        var name = Path.GetFileNameWithoutExtension(modelPath) + ".png";
        var direct = Path.Combine(previewBase, name).Replace("\\", "/");
        if (File.Exists(direct)) return direct;
        var alt = Path.Combine(previewBase, Path.GetFileName(Path.GetDirectoryName(modelPath)) + ".png").Replace("\\", "/");
        if (File.Exists(alt)) return alt;
        return null;
    }
}
