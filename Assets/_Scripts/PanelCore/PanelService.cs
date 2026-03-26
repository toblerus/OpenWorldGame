using System;
using System.Collections.Generic;
using ReactiveCore.Runtime;
using UnityEngine;

namespace _Scripts.PanelCore
{
    public class PanelService : MonoBehaviour
    {
        [Serializable]
        public class PanelEntry
        {
            public Panel PanelPrefab;
        }

        [SerializeField] private List<PanelEntry> panelPrefabs;
        [SerializeField] private Transform panelParent;

        private readonly Dictionary<Type, Panel> cachedPanels = new Dictionary<Type, Panel>();
        private Panel _currentPanel;
        private ReactiveValue<bool> _isAnyPanelOpen = new ReactiveValue<bool>();
        
        public ReactiveValue<bool> IsAnyPanelOpen => _isAnyPanelOpen;

        public void OpenPanel<T>() where T : Panel
        {
            CloseCurrentPanel();

            Type panelType = typeof(T);

            if (!cachedPanels.TryGetValue(panelType, out var panel))
            {
                var prefab = FindPrefabOfType<T>();
                if (prefab == null) return;

                panel = Instantiate(prefab, panelParent);
                cachedPanels[panelType] = panel;
            }

            _currentPanel = panel;
            _currentPanel.gameObject.SetActive(true);
            _isAnyPanelOpen.Value = true;
            _currentPanel.OnOpen();
        }

        public void CloseCurrentPanel()
        {
            if (_currentPanel != null)
            {
                _currentPanel.OnClose();
                _currentPanel.gameObject.SetActive(false);
                _currentPanel = null;
                _isAnyPanelOpen.Value = false;
            }
        }

        public void ClosePanel<T>() where T : Panel
        {
            if (_currentPanel != null && _currentPanel is T)
            {
                _currentPanel.OnClose();
                _currentPanel.gameObject.SetActive(false);
                _currentPanel = null;
                _isAnyPanelOpen.Value = false;
            }
        }

        public bool IsPanelOpen<T>() where T : Panel
        {
            return _currentPanel != null && _currentPanel is T;
        }

        private T FindPrefabOfType<T>() where T : Panel
        {
            foreach (var entry in panelPrefabs)
            {
                if (entry.PanelPrefab is T typed)
                    return typed;
            }

            Debug.LogWarning($"No panel prefab of type {typeof(T).Name} found.");
            return null;
        }
    }
}