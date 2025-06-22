using System.Diagnostics;
using System.IO;
using UnityEditor;

namespace Editor
{
    public static class ExportPackage
    {
        [MenuItem("Tools/Export ReactiveCore Package")]
        public static void Build()
        {
            string repoRoot = Path.Combine(Directory.GetCurrentDirectory(), "Assets/ReactiveCore");
            string versionTag = GetGitTag(repoRoot) ?? "preview";

            string exportPath = Path.Combine(Directory.GetCurrentDirectory(), $"ReactiveCore-{versionTag}.unitypackage");

            AssetDatabase.ExportPackage(
                "Assets/ReactiveCore",
                exportPath,
                ExportPackageOptions.Recurse
            );

            UnityEngine.Debug.Log($"Exported package: {exportPath}");
        }

        private static string GetGitTag(string workingDirectory)
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = "git";
                process.StartInfo.Arguments = "describe --tags --abbrev=0";
                process.StartInfo.WorkingDirectory = workingDirectory;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                return string.IsNullOrEmpty(output) ? null : output;
            }
            catch
            {
                return null;
            }
        }
    }
}