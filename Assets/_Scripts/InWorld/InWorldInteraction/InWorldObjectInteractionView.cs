using _Scripts.Injection;
using _Scripts.Interaction;
using System;
using _Scripts.Inventory;
using UnityEngine;

namespace _Scripts.InWorld.InWorldInteraction
{
    public class InWorldObjectInteractionView : MonoBehaviour, IHoldInteractable
    {
        private InWorldObjectInteractionController _controller;
        public event Action Harvested;

        [SerializeField] private float _interactionDuration;
        public float InteractionDuration => _interactionDuration;
        
        [SerializeField] private GameItemType _itemType;
        public GameItemType ItemType => _itemType;

        private void Start()
        {
            _controller = ServiceLocator.Resolve<InWorldObjectInteractionController>();
            _controller.Setup(this, _itemType);
        }
        
        public void Interact()
        {
            if (_controller == null || !_controller.Interact()) return;
            enabled = false;
            Harvested?.Invoke();
        }

        public void Progress(float progress)
        {
            Debug.Log($"Progress: {progress}");
        }
    }
}
