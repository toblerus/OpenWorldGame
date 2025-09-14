using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using System.Linq;

public class KenneyPalettePostprocessor : AssetPostprocessor
{
    const string PrefDestKey = "KenneyAssetViewer.Dest";

    Material OnAssignMaterialModel(Material material, Renderer renderer)
    {
        var mat = LoadOrCreatePaletteForModel(assetPath);
        return mat != null ? mat : material;
    }

    void OnPostprocessModel(GameObject g)
    {
        var mat = LoadOrCreatePaletteForModel(assetPath);
        if (mat == null) return;
        var renderers = g.GetComponentsInChildren<Renderer>(true);
        for (var i = 0; i < renderers.Length; i++) renderers[i].sharedMaterial = mat;
    }

    [MenuItem("Tools/Kenney/Repair Selected Models")]
    static void RepairSelected()
    {
        var guids = Selection.assetGUIDs;
        for (var i = 0; i < guids.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (go == null) continue;
            var mat = LoadOrCreatePaletteForModel(path);
            if (mat == null) continue;
            var renderers = go.GetComponentsInChildren<Renderer>(true);
            for (var r = 0; r < renderers.Length; r++) renderers[r].sharedMaterial = mat;
            EditorUtility.SetDirty(go);
        }
        AssetDatabase.SaveAssets();
    }

    static Material LoadOrCreatePaletteForModel(string modelAssetPath)
    {
        var destRoot = EditorPrefs.GetString(PrefDestKey, "Assets/KenneyImported");
        var matFolder = Path.Combine(destRoot, "_Materials").Replace("\\", "/");
        if (!AssetDatabase.IsValidFolder(matFolder))
        {
            var parent = Path.GetDirectoryName(matFolder).Replace("\\", "/");
            if (!AssetDatabase.IsValidFolder(parent)) AssetDatabase.CreateFolder("Assets", Path.GetFileName(parent));
            AssetDatabase.CreateFolder(parent, Path.GetFileName(matFolder));
        }
        var matPath = Path.Combine(matFolder, "KenneyPalette.mat").Replace("\\", "/");
        var mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        var tex = FindPaletteTextureNear(modelAssetPath);
        if (mat == null)
        {
            var shader = GraphicsSettings.currentRenderPipeline == null ? Shader.Find("Unlit/Texture") : Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null) shader = Shader.Find("Unlit/Texture");
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, matPath);
        }
        if (tex != null)
        {
            if (mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap", tex);
            if (mat.HasProperty("_MainTex")) mat.SetTexture("_MainTex", tex);
        }
        return mat;
    }

    static Texture2D FindPaletteTextureNear(string modelAssetPath)
    {
        var dir = Path.GetDirectoryName(modelAssetPath).Replace("\\", "/");
        var parts = dir.Split('/');
        var modelsIndex = System.Array.IndexOf(parts, "Models");
        if (modelsIndex < 0) return null;
        var modelsRoot = string.Join("/", parts.Take(modelsIndex + 1));
        var texDir = Path.Combine(modelsRoot, "Textures").Replace("\\", "/");
        if (!AssetDatabase.IsValidFolder(texDir)) return null;
        var names = new[] { "variation-a", "variation-b", "palette", "color", "colors" };
        for (var i = 0; i < names.Length; i++)
        {
            var hits = AssetDatabase.FindAssets(names[i] + " t:Texture2D", new[] { texDir });
            if (hits != null && hits.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(hits[0]);
                var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (tex != null) return tex;
            }
        }
        var any = AssetDatabase.FindAssets("t:Texture2D", new[] { texDir });
        if (any != null && any.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(any[0]);
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }
        return null;
    }
}
