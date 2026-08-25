using _Scripts.Injection;
using _Scripts.Inventory.ItemConfigs;
using UnityEngine;

namespace _Scripts.Installation
{
    public class GameItemConfigInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingleton<GameItemConfigModel>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<GameItemConfigModel>();
        }
    }
}