using _Scripts.Injection;
using _Scripts.Interaction;
using System;
using _Scripts.Inventory;
using UnityEngine;

namespace _Scripts.InWorld.InWorldInteraction
{
    public class InWorldHarvestInteractionView : MonoBehaviour, IHoldInteractable
    {
        private InWorldHarvestInteractionController _controller;
        public event Action Harvested;

        [SerializeField] private float _interactionDuration;
        public float InteractionDuration => _interactionDuration;
        
        [SerializeField] private GameItemType _itemType;
        public GameItemType ItemType => _itemType;

        private void Start()
        {
            _controller = ServiceLocator.Resolve<InWorldHarvestInteractionController>();
            _controller.Setup(this);
        }
        
        public void Interact(GameObject interactor)
        {
            if (!isActiveAndEnabled || _controller == null || !_controller.TryInteract()) return;
            enabled = false;
            Harvested?.Invoke();
        }

        public void Progress(float progress)
        {
            Debug.Log($"Progress: {progress}");
        }
    }
}
