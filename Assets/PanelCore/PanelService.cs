using System;
using System.Collections.Generic;
using UnityEngine;

namespace PanelCore
{
    public class PanelService : MonoBehaviour
    {
        [Serializable]
        public class PanelEntry
        {
            public string Key;
            public Panel PanelPrefab;
        }

        [SerializeField] private List<PanelEntry> panelPrefabs;
        [SerializeField] private Transform panelParent;

        private Dictionary<string, Panel> cachedPanels = new Dictionary<string, Panel>();
        private Panel currentPanel;

        public void OpenPanel(string key)
        {
            if (currentPanel != null)
            {
                currentPanel.OnClose();
                currentPanel.gameObject.SetActive(false);
            }

            if (!cachedPanels.TryGetValue(key, out var panel))
            {
                var prefab = panelPrefabs.Find(p => p.Key == key)?.PanelPrefab;
                if (prefab == null) return;
                panel = Instantiate(prefab, panelParent);
                cachedPanels[key] = panel;
            }

            currentPanel = panel;
            currentPanel.gameObject.SetActive(true);
            currentPanel.OnOpen();
        }

        public void CloseCurrentPanel()
        {
            if (currentPanel != null)
            {
                currentPanel.OnClose();
                currentPanel.gameObject.SetActive(false);
                currentPanel = null;
            }
        }
    }
}