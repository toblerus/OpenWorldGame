using Injection;
using Inventory.ItemConfigs;
using UnityEngine;

namespace Installation
{
    public class GameItemConfigInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingleton<GameItemConfigController>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<GameItemConfigController>();
        }
    }
}