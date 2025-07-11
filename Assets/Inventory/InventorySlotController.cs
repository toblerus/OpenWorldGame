using Hud;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class InventorySlotController
    {
        public InventorySlotView View { get; private set; }

        public void Setup(InventorySlotView view)
        {
            View = view;   
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (View.CurrentGameItem == null) return;
            InventoryDragModel.Instance.StartDrag(View, View.CurrentGameItem, View.CurrentAmount.ToString(), eventData.position);
            View.Clear();
        }

        public void OnDrag(PointerEventData eventData)
        {
            InventoryDragModel.Instance.UpdateDragPosition(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var target = eventData.pointerEnter?.GetComponent<InventorySlotView>();
            InventoryDragModel.Instance.EndDrag(View, target);
        }

        public void AcceptDrop(GameItem gameItem, int amount)
        {
            View.SetupGameItem(gameItem, amount);
        }
    }
}