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
            ServiceLocator.BindSingleton<InWorldObjectPersistenceModel>();
            ServiceLocator.BindTransient<InWorldObjectInteractionController>();
            ServiceLocator.BindSingleton<InWorldObjectInteractionModel>();
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