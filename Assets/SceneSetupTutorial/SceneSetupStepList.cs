using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DevTools/SceneSetupStepList")]
public class SceneSetupStepList : ScriptableObject
{
    public List<SceneSetupStep> steps;
}