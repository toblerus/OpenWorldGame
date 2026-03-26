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