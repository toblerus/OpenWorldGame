using _Scripts.Injection;
using _Scripts.Interaction;
using _Scripts.Inventory;
using UnityEngine;

namespace _Scripts.InWorld.InWorldInteraction
{
    public class InWorldObjectInteractionView : MonoBehaviour, IHoldInteractable
    {
        private InWorldObjectInteractionController _controller;

        [SerializeField] private float _interactionDuration;
        public float InteractionDuration => _interactionDuration;
        
        [SerializeField] private GameItemType _itemType;

        private void Start()
        {
            _controller = ServiceLocator.Resolve<InWorldObjectInteractionController>();
            _controller.Setup(this, _itemType);
        }
        
        public void Interact()
        {
            _controller.Interact();
        }

        public void Progress(float progress)
        {
            Debug.Log($"Progress: {progress}");
        }
    }
}