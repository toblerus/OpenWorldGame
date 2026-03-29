using _Scripts.Crafting;
using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Installation
{
    public class CraftingInstallation : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingletonNonLazy<CraftingSelectionModel>();
            ServiceLocator.BindTransient<CraftingCategoryController>();
            ServiceLocator.BindSingleton<CraftingPanelController>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<CraftingSelectionModel>();
            ServiceLocator.Unbind<CraftingCategoryController>();
            ServiceLocator.Unbind<CraftingPanelController>();
        }
    }
}