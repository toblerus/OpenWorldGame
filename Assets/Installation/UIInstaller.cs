using Hud;
using UnityEngine;

namespace Installation
{
    public class UIInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingleton<HotBarController>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<HotBarController>();
        }
    }
}