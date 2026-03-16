using Injection;
using InWorld;
using UnityEngine;

namespace Installation
{
    public class InWorldObjectInstaller : MonoBehaviour, IInstaller
    {
        public void Install()
        {
            ServiceLocator.BindSingleton<InWorldObjectPersistenceController>();
            ServiceLocator.BindSingleton<InWorldObjectPersistenceModel>();
        }

        public void Uninstall()
        {
            ServiceLocator.Unbind<InWorldObjectPersistenceController>();
            ServiceLocator.Unbind<InWorldObjectPersistenceModel>();
        }
    }
}