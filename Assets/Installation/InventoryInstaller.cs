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
            ServiceLocator.BindSingleton<InventoryModel>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<InventoryModel>();
        }
    }
}