using Hud;
using Injection;
using Inventory;
using Inventory.Hotbar;
using UnityEngine;

namespace Installation
{
    public class UIInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingleton<HotBarController>();
            ServiceLocator.BindTransient<InventorySlotController>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<HotBarController>();
            ServiceLocator.Unbind<InventorySlotController>();
        }
    }
}