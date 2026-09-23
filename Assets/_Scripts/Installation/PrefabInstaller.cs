using System;
using System.Collections.Generic;
using System.Reflection;
using _Scripts.Injection;
using _Scripts.Installation;
using _Scripts.InWorld;
using UnityEngine;
using UnityEngine.Scripting;

public class PrefabInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private List<MonoBehaviour> _prefabs = new();

    private readonly Dictionary<(Type Type, object Key), Action> _unbindViews = new();

    public void Install()
    {
        var bindView = typeof(PrefabInstaller).GetMethod(nameof(BindView), BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var prefab in _prefabs)
        {
            if (prefab == null)
            {
                Debug.LogWarning("Missing prefab in PrefabInstaller.", this);
                continue;
            }

            var resource = prefab as InWorldResourceView;
            var viewType = resource != null ? typeof(InWorldResourceView) : prefab.GetType();
            object key = resource != null ? resource.ItemType : null;
            if (_unbindViews.ContainsKey((viewType, key)))
            {
                Debug.LogWarning($"View {viewType.Name} with key '{key}' is already installed by this PrefabInstaller.", this);
                continue;
            }

            // Resource prefabs share a view type and are distinguished by their item key.
            bindView?.MakeGenericMethod(viewType).Invoke(this, new object[] { prefab, key });
        }
    }

    [Preserve]
    private void BindView<TView>(TView prefab, object key) where TView : MonoBehaviour
    {
        ServiceLocator.BindView(prefab, key);
        _unbindViews.Add((typeof(TView), key), () => ServiceLocator.Unbind<ViewFactory<TView>>(key));
    }

    public void Uninstall()
    {
        foreach (var unbind in _unbindViews.Values)
            unbind();

        _unbindViews.Clear();
    }
}
