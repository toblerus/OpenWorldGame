using Hud;
using Injection;
using Inventory;
using Inventory.HandItem;
using Inventory.ItemPlacement;
using UnityEngine;

namespace Installation
{
    public class InventoryInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingleton<InventoryModel>();
            ServiceLocator.BindSingleton<HandItemController>();
            ServiceLocator.BindSingleton<ItemPlacementController>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<InventoryModel>();
            ServiceLocator.Unbind<HandItemController>();
            ServiceLocator.Unbind<ItemPlacementController>();
        }
    }
}