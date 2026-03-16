using Injection;
using UnityEngine;

namespace InWorld
{
    public class InWorldObjectPersistenceView : MonoBehaviour
    {
        private void Start()
        {
            var controller = ServiceLocator.Resolve<InWorldObjectPersistenceController>();
            controller.Setup(this);
        }
    }
}
