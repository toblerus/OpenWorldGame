using _Scripts.Injection;
using _Scripts.Inventory;
using _Scripts.Inventory.HandItem;
using _Scripts.Inventory.ItemPlacement;
using UnityEngine;

namespace _Scripts.Installation
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