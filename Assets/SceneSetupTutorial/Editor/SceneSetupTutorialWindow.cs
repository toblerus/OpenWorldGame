using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SceneSetupTutorial.Editor
{
    public class SceneSetupTutorialWindow : EditorWindow
    {
        private SceneSetupStepList _stepList;
        private Vector2           _scroll;
        private int               _currentStep;
        private bool              _isMainBank;

        [MenuItem("Tools/Scene Setup Tutorial")]
        public static void ShowWindow()
        {
            GetWindow<SceneSetupTutorialWindow>("Scene Setup Tutorial");
        }

        private void OnEnable()
        {
            _stepList   = Resources.Load<SceneSetupStepList>("SceneSetupSteps");
            _currentStep = 0;
            _isMainBank  = EditorPrefs.GetBool("SceneSetup_IsMainBank", true);
        }

        private void OnGUI()
        {
            if (_stepList == null)
            {
                EditorGUILayout.HelpBox("SceneSetupSteps asset not found in Resources.", MessageType.Error);
                return;
            }

            EditorGUILayout.LabelField("Scene Setup Tutorial", EditorStyles.boldLabel);
            _isMainBank = EditorGUILayout.Toggle("Main Bank", _isMainBank);
            EditorPrefs.SetBool("SceneSetup_IsMainBank", _isMainBank);
            EditorGUILayout.Space();

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            for (int i = 0; i < _stepList.steps.Count; i++)
            {
                var step = _stepList.steps[i];
                if ((_isMainBank && step.isEventBankOnly) || (!_isMainBank && step.isMainBankOnly))
                    continue;

                bool isActive = i == _currentStep;

                EditorGUILayout.BeginVertical("box");
                // always show title
                EditorGUILayout.LabelField($"Step {i + 1}: {step.title}",
                    isActive ? EditorStyles.boldLabel : EditorStyles.label);

                if (isActive)
                {
                    // unfolded description
                    EditorGUILayout.HelpBox(step.description, MessageType.None);
                    DrawActionButton(step);
                    if (GUILayout.Button("Mark as Done"))
                    {
                        _currentStep = Mathf.Min(_currentStep + 1, _stepList.steps.Count - 1);
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(4);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawActionButton(SceneSetupStep step)
        {
            switch (step.actionType)
            {
                case SceneSetupStep.ActionType.OpenPath:
                    if (!string.IsNullOrEmpty(step.actionParameter) 
                        && GUILayout.Button("Open Path"))
                    {
                        string path = step.actionParameter;
                        if (Directory.Exists(path) || File.Exists(path))
                        {
                            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                            if (obj != null)
                            {
                                EditorUtility.FocusProjectWindow();
                                Selection.activeObject = obj;
                                EditorGUIUtility.PingObject(obj);
                            }
                            else
                            {
                                // fallback: reveal in OS
                                EditorUtility.RevealInFinder(path);
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"Path not found: {path}");
                        }
                    }
                    break;

                case SceneSetupStep.ActionType.OpenEditorWindow:
                    if (!string.IsNullOrEmpty(step.actionParameter) 
                        && GUILayout.Button("Open Editor Window"))
                    {
                        Type windowType = Type.GetType(step.actionParameter);
                        if (windowType == null)
                        {
                            Debug.LogWarning($"Type not found: {step.actionParameter}");
                        }
                        else
                        {
                            EditorWindow.GetWindow(windowType);
                        }
                    }
                    break;

                case SceneSetupStep.ActionType.None:
                default:
                    break;
            }
        }
    }
}
