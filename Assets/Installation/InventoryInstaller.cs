using Hud;
using Injection;
using Inventory;
using UnityEngine;

namespace Installation
{
    public class InventoryInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingleton<ItemDropModel>();
            ServiceLocator.BindSingleton<InventoryModel>();
            ServiceLocator.BindSingleton<ItemDropController>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<ItemDropModel>();
            ServiceLocator.Unbind<ItemDropController>();
        }
    }
}