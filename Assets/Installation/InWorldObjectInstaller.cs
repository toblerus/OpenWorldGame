using Injection;
using InWorld;
using InWorld.InWorldInteraction;
using UnityEngine;

namespace Installation
{
    public class InWorldObjectInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingleton<InWorldObjectPersistenceController>();
            ServiceLocator.BindSingletonNonLazy<InWorldObjectPersistenceModel>();
            ServiceLocator.BindTransient<InWorldObjectInteractionController>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<InWorldObjectPersistenceController>();
            ServiceLocator.Unbind<InWorldObjectPersistenceModel>();
            ServiceLocator.Unbind<InWorldObjectInteractionController>();
            ServiceLocator.Unbind<InWorldObjectInteractionModel>();
        }
    }
}