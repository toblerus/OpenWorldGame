using _Scripts.Injection;
using _Scripts.Statuses;
using UnityEngine;

namespace _Scripts.Installation
{
    public class StatusInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            //Health
            ServiceLocator.BindSingleton<HealthModel>();
            ServiceLocator.BindSingleton<HealthController>();
            
            //Nutrition
            ServiceLocator.BindSingleton<NutritionModel>();
            ServiceLocator.BindSingleton<NutritionController>();
        }

        public void Uninstall()
        {
            //Health
            ServiceLocator.Unbind<HealthModel>();
            ServiceLocator.Unbind<HealthController>();
            
            //Nutrition
            ServiceLocator.Unbind<NutritionModel>();
            ServiceLocator.Unbind<NutritionController>();
        }
    }
}
