using _Scripts.Injection;
using _Scripts.InWorld;
using _Scripts.InWorld.InWorldInteraction;
using UnityEngine;

namespace _Scripts.Installation
{
    public class InWorldObjectInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingleton<InWorldObjectPersistenceController>();
            ServiceLocator.BindSingletonNonLazy<InWorldObjectPersistenceModel>();
            ServiceLocator.BindTransient<InWorldHarvestInteractionController>();
            ServiceLocator.BindTransient<InWorldHarvestInteractionModel>();
            ServiceLocator.BindTransient<InWorldStorageInteractionController>();
            ServiceLocator.BindTransient<InWorldPopulationController>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<InWorldObjectPersistenceController>();
            ServiceLocator.Unbind<InWorldObjectPersistenceModel>();
            ServiceLocator.Unbind<InWorldHarvestInteractionController>();
            ServiceLocator.Unbind<InWorldHarvestInteractionModel>();
            ServiceLocator.Unbind<InWorldStorageInteractionController>();
            ServiceLocator.Unbind<InWorldPopulationController>();
        }
    }
}
