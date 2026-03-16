using Injection;
using UnityEngine;

namespace InWorld
{
    public class InWorldObjectPersistenceLoader : MonoBehaviour
    {
        private void Start()
        {
            var model = ServiceLocator.Resolve<InWorldObjectPersistenceModel>();
            model.Load();
        }
    }
}