# OpenWorldGame

## Spawning views

`ViewFactory<TView>` creates a prefab instance and returns its view component.
It uses the existing `ServiceLocator` and works with any `MonoBehaviour` view,
including `InjectableView`. No extra base class is needed.

Bind the view component on the prefab in an existing `IInstaller`. For example:

```csharp
using _Scripts.Crafting;
using _Scripts.Injection;
using _Scripts.Installation;
using UnityEngine;

public class CraftingViewInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private CraftingIngredientView _ingredientPrefab;

    public void Install()
    {
        ServiceLocator.BindView(_ingredientPrefab);
    }

    public void Uninstall()
    {
        ServiceLocator.Unbind<ViewFactory<CraftingIngredientView>>();
    }
}
```

Assign the prefab's root view component in the Inspector and add the installer
to `MainInstaller` before anything that resolves the factory. You can also put
the same binding in an existing installer. Each concrete view type has one binding.

Resolve the factory in a controller and create views:

```csharp
var factory = ServiceLocator.Resolve<ViewFactory<CraftingIngredientView>>();
var view = factory.Create(ingredientParent);

// Or spawn at a world position, with an optional parent:
var placedView = factory.Create(position, rotation, parent);

// Destroy the whole instance when finished:
Object.Destroy(view.gameObject);
```

`BindView` registers a singleton factory; every `Create` produces a fresh instance.
It is shorthand for `ServiceLocator.BindSingleton(() => new ViewFactory<TView>(prefab))`.
Use `IsBound<ViewFactory<TView>>()` and `Unbind<ViewFactory<TView>>()` as usual;
unbind before replacing an already resolved binding, following the locator's
existing singleton behavior.

The parent-only overload keeps the prefab's local transform; the position/rotation
overload uses world space. Both preserve the prefab's active state and Unity's
normal lifecycle, including the existing `InjectableView.Start` controller setup.
Unbinding removes the registration; spawned objects remain owned by their caller
or scene. There is no pooling or automatic view/controller reset.
