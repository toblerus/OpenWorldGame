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
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<CraftingSelectionModel>();
        }
    }
}