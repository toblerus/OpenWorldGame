using System.Collections.Generic;
using UnityEngine;

namespace Installation
{
    public class MainInstaller : MonoBehaviour
    {
        [Header("Scene Installers")]
        [SerializeField] private MonoBehaviour[] _installers; 

        private readonly List<IInstaller> _installed = new();
        
        private void Awake()
        {
            foreach (var installer in _installers)
            {
                if (installer is IInstaller i)
                {
                    i.Install();
                    _installed.Add(i);
                }
                else
                {
                    Debug.LogWarning($"Installer {installer.name} does not implement IInstaller.");
                }
            }
        }

        private void OnDestroy()
        {
            for (var i = _installed.Count - 1; i >= 0; i--)
            {
                _installed[i].Uninstall();
            }
            _installed.Clear();
        }
    }
}
