using Hud;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class InventorySlotController
    {
        private InventorySlotView _view;

        public void Setup(InventorySlotView view)
        {
            _view = view;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_view.HasItem) return;
            InventoryDragModel.Instance.StartDrag(_view, _view.CurrentGameItem, _view.CurrentAmount.ToString(), eventData.position);
            _view.Clear();
        }

        public void OnDrag(PointerEventData eventData)
        {
            InventoryDragModel.Instance.UpdateDragPosition(eventData.position);
        }

        public void OnDropRequest()
        {
            if (!_view.HasItem)
            {
                Debug.Log("[InventorySlotController] Drop request ignored: no item to drop.");
                return;
            }

            Debug.Log($"[InventorySlotController] Dropping item: {_view.CurrentGameItem.name}, amount: {_view.CurrentAmount}");
            InventoryDragModel.Instance.SpawnItemDrop(_view.CurrentGameItem, _view.CurrentAmount);
            _view.Clear();
        }

        public InventorySlotView View => _view;
    }
}