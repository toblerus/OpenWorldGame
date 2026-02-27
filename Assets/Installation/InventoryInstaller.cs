using Hud;
using Injection;
using Inventory;
using Inventory.HandItem;
using UnityEngine;

namespace Installation
{
    public class InventoryInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingleton<InventoryModel>();
            ServiceLocator.BindSingleton<HandItemController>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<InventoryModel>();
            ServiceLocator.Unbind<HandItemController>();
        }
    }
}