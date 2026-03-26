using _Scripts.Injection;
using _Scripts.Inventory;
using _Scripts.Inventory.Hotbar;
using UnityEngine;

namespace _Scripts.Installation
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