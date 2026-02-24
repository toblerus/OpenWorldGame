using Injection;
using Statuses;
using UnityEngine;

namespace Installation
{
    public class StatusInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            //Health
            ServiceLocator.BindSingleton<HealthModel>();
            ServiceLocator.BindSingleton<HealthController>();
            
            //Food...
        }

        public void Uninstall()
        {
            //Health
            ServiceLocator.Unbind<HealthModel>();
            ServiceLocator.Unbind<HealthController>();
            
            //Food...
        }
    }
}
