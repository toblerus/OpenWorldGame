using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.InWorld
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
