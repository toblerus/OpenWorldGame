using System.Collections.Generic;
using Interaction;
using Inventory;
using ReactiveCore.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PlayerControls
{
    public class InventoryInputHandler : MonoBehaviour
    {
        private PlayerInputActions _input;
        private ReactiveTimer _holdInteractionTimer = new(.01f);
        private float _currentInteractionHoldingDuration = new();
        private bool _isHolding;
        private IHoldInteractable _interactable;
        [SerializeField] private Image _progressBar;

        private void Awake()
        {
            _input = new PlayerInputActions();
            _input.Player.Interact.performed += ctx => TryInteract();
            _input.Player.Drop.performed += ctx => TryDrop();

            _input.Player.InteractHold.started += ctx => StartHoldInteraction();
            _input.Player.InteractHold.canceled += ctx => CancelHoldInteraction();
            
            _holdInteractionTimer.Start();

            _holdInteractionTimer.Elapsed.Subscribe(IsHeldIncrease);
        }

        private void IsHeldIncrease()
        {
            if (!_isHolding) return;
            _currentInteractionHoldingDuration += _holdInteractionTimer.IntervalSeconds;
            
            _interactable.Progress(_currentInteractionHoldingDuration / _interactable.InteractionDuration);
            _progressBar.fillAmount = _currentInteractionHoldingDuration / _interactable.InteractionDuration;
            
            if (!(_currentInteractionHoldingDuration >= _interactable.InteractionDuration)) return;
            
            _interactable.Interact();
            _input.Player.InteractHold.Reset();
            _interactable = null;
            _isHolding = false;
        }

        private void StartHoldInteraction()
        {
            if (Camera.main != null)
            {
                var camera = Camera.main;
                Ray ray = new Ray(camera.transform.position, camera.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, 3f))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    _interactable = hit.collider.transform.GetComponent<IHoldInteractable>();

                    _isHolding = true;
                }
            }
        }

        private void CancelHoldInteraction()
        {
            _currentInteractionHoldingDuration = 0;
            _progressBar.fillAmount = 0;
            _interactable = null;
            _isHolding = false;
        }

        private void OnEnable() => _input.Enable();
        private void OnDisable() => _input.Disable();

        private void TryInteract()
        {
            if (Camera.main != null)
            {
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, 3f))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    var interactable = hit.collider.transform.parent?.GetComponent<IInteractable>();
                    interactable?.Interact(this.transform.parent.gameObject);
                }
            }
        }

        private void TryDrop()
        {
            Debug.Log("Trying to drop");
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                Debug.Log(result.gameObject.name);
                var slot = result.gameObject.GetComponentInParent<InventorySlotView>();
                if (slot != null && slot.CurrentGameItemConfig != null)
                {
                    slot.RequestDrop();
                    break;
                }
            }
        }
    }
}