using _Scripts.Injection;
using UnityEngine;
using _Scripts.Interaction;
using _Scripts.Storage;

namespace _Scripts.InWorld.InWorldInteraction
{
    public class InWorldStorageInteractionView : MonoBehaviour, IInteractable
    {
        private InWorldStorageInteractionController _controller;
        private StorageInteractionModel _storageInteractionModel;

        private void Start()
        {
            _controller = ServiceLocator.Resolve<InWorldStorageInteractionController>();
            _storageInteractionModel = ServiceLocator.Resolve<StorageInteractionModel>();
        }

        public void Interact(GameObject interactor)
        {
            _controller.Interact(interactor,  _storageInteractionModel);
        }
    }
}
