using _Scripts.Crafting;
using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Installation
{
    public class CraftingInstallation : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingletonNonLazy<CraftingCategorySelectionModel>();
            ServiceLocator.BindSingletonNonLazy<CraftingGameItemSelectionModel>();
            ServiceLocator.BindTransient<CraftingCategoryController>();
            ServiceLocator.BindSingleton<CraftingPanelController>();
            ServiceLocator.BindTransient<CraftingGameItemController>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<CraftingCategorySelectionModel>();
            ServiceLocator.Unbind<CraftingCategoryController>();
            ServiceLocator.Unbind<CraftingPanelController>();
            ServiceLocator.Unbind<CraftingGameItemController>();
        }
    }
}