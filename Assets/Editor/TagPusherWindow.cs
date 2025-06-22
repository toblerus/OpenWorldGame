using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;

namespace Assets.Editor
{
    public class TagPusherWindow : EditorWindow
    {
        private string tagVersion = "v1.0.0";
        private string repoPath = "Assets/ReactiveCore"; // Adjust if needed

        [MenuItem("Tools/Push Git Tag")]
        public static void ShowWindow()
        {
            GetWindow<TagPusherWindow>("Push Git Tag");
        }

        void OnGUI()
        {
            GUILayout.Label("Tag Version", EditorStyles.boldLabel);
            tagVersion = EditorGUILayout.TextField("Tag:", tagVersion);

            if (GUILayout.Button("Create and Push Tag"))
            {
                if (string.IsNullOrWhiteSpace(tagVersion))
                {
                    UnityEngine.Debug.LogError("Tag cannot be empty.");
                    return;
                }

                if (!RunGitCommand($"tag {tagVersion}") ||
                    !RunGitCommand($"push origin {tagVersion}"))
                {
                    UnityEngine.Debug.LogError("Failed to create or push tag.");
                }
                else
                {
                    UnityEngine.Debug.Log($"Tag {tagVersion} pushed successfully.");
                }
            }
        }

        private bool RunGitCommand(string arguments)
        {
            try
            {
                string workingDir = Path.Combine(Directory.GetCurrentDirectory(), repoPath);

                var process = new Process();
                process.StartInfo.FileName = "git";
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WorkingDirectory = workingDir;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    UnityEngine.Debug.LogError($"Git error:\n{error}");
                    return false;
                }

                UnityEngine.Debug.Log(output.Trim());
                return true;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"Git command failed: {ex.Message}");
                return false;
            }
        }
    }
}