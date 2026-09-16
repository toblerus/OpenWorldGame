using System;
using System.Collections.Generic;
using System.Reflection;
using _Scripts.Injection;
using _Scripts.Installation;
using UnityEngine;
using UnityEngine.Scripting;

public class PrefabInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private List<MonoBehaviour> _prefabs = new();

    private readonly Dictionary<Type, Action> _unbindViews = new();

    public void Install()
    {
        var bindView = typeof(PrefabInstaller).GetMethod(nameof(BindView), BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var prefab in _prefabs)
        {
            
            var viewType = prefab.GetType();
            if (_unbindViews.ContainsKey(viewType))
            {
                Debug.LogWarning($"View {viewType.Name} is already installed by this PrefabInstaller.", this);
                continue;
            }

            // The Inspector list erases the concrete type; restore it for BindView<TView>.
            bindView?.MakeGenericMethod(viewType).Invoke(this, new object[] { prefab });
        }
    }

    [Preserve]
    private void BindView<TView>(TView prefab) where TView : MonoBehaviour
    {
        ServiceLocator.BindView(prefab);
        _unbindViews.Add(typeof(TView), ServiceLocator.Unbind<ViewFactory<TView>>);
    }

    public void Uninstall()
    {
        foreach (var unbind in _unbindViews.Values)
            unbind();

        _unbindViews.Clear();
    }
}
