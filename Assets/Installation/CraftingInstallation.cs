using Crafting;
using Injection;
using UnityEngine;

namespace Installation
{
    public class CraftingInstallation : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingletonNonLazy<CraftingSelectionModel>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<CraftingSelectionModel>();
        }
    }
}