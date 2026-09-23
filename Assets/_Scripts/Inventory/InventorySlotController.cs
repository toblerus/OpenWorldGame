using UnityEngine;
using UnityEngine.EventSystems;
using _Scripts.Injection;

namespace _Scripts.Inventory
{
    public class InventorySlotController
    {
        private InventorySlotView _view;
        private InventoryModel _inventoryModel;
        private ItemDropSpawnerModel _itemDropSpawnerModel;

        public void Setup(InventorySlotView view)
        {
            _view = view;
            _inventoryModel = ServiceLocator.Resolve<InventoryModel>();
            _itemDropSpawnerModel = ServiceLocator.Resolve<ItemDropSpawnerModel>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_view.HasItem) return;
            InventoryDragModel.Instance.StartDrag(_view, _view.CurrentGameItemConfig, _view.CurrentAmount.ToString(), eventData.position);
            _view.SetDragging(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            InventoryDragModel.Instance.UpdateDragPosition(eventData.position);
        }

        public void OnDropRequest()
        {
            if (!_view.HasItem)
            {
                Debug.LogError("[InventorySlotController] Drop request ignored: no item to drop.");
                return;
            }

            Debug.LogError($"[InventorySlotController] Dropping item: {_view.CurrentGameItemConfig.name}, amount: {_view.CurrentAmount}");
            _itemDropSpawnerModel.Spawn(_view.CurrentGameItemConfig, _view.CurrentAmount);
            _inventoryModel.RemoveAt(_view.SlotIndex, _view.CurrentAmount);
        }
    }
}
