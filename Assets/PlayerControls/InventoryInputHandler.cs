using System.Collections.Generic;
using Hud;
using Interaction;
using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryInputHandler : MonoBehaviour
{
    private PlayerInputActions _input;

    private void Awake()
    {
        _input = new PlayerInputActions();
        _input.Player.Interact.performed += ctx => TryInteract();
        _input.Player.Drop.performed += ctx => TryDrop();
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
                var interactable = hit.collider.transform.parent.GetComponent<IInteractable>();
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
            if (slot != null && slot.CurrentGameItem != null)
            {
                slot.RequestDrop();
                break;
            }
        }
    }
}