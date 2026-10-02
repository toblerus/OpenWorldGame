using _Scripts.Injection;
using UnityEngine;
using _Scripts.Interaction;

namespace _Scripts.InWorld.InWorldInteraction
{
    public class InWorldStorageInteractionView : MonoBehaviour, IInteractable
    {
        private InWorldStorageInteractionController _controller;

        private void Start()
        {
            _controller = ServiceLocator.Resolve<InWorldStorageInteractionController>();
        }

        public void Interact(GameObject interactor)
        {
            _controller.Interact(interactor);
        }
    }
}
