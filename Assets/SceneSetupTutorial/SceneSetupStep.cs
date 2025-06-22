using UnityEngine;

[CreateAssetMenu(menuName = "DevTools/SceneSetupStep")]
public class SceneSetupStep : ScriptableObject
{
    public enum ActionType
    {
        None,
        OpenPath,
        OpenEditorWindow
    }

    public string title;
    [TextArea(3, 10)] public string description;

    public bool isMainBankOnly;
    public bool isEventBankOnly;

    public ActionType actionType = ActionType.None;
    [Tooltip("If OpenPath: relative project path (e.g. \"Assets/Configs/Installers\").\n" +
             "If OpenEditorWindow: full type name (e.g. \"MyNamespace.MyWindow, AssemblyName\").")]
    public string actionParameter;
}