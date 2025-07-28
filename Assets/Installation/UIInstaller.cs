using Hud;
using Injection;
using Inventory;
using UnityEngine;

namespace Installation
{
    public class UIInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingleton<HotBarController>();
            ServiceLocator.BindTransient<InventorySlotController>();
            ServiceLocator.BindTransient<ItemDropController>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<HotBarController>();
            ServiceLocator.Unbind<InventorySlotController>();
        }
    }
}