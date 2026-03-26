using System.Collections.Generic;
using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Inventory.Hotbar
{
    public class HotBarView : MonoBehaviour
    {
        [SerializeField] private List<InventorySlotView> _hotBarInventorySlotViews;
        public List<InventorySlotView> HotBarInventorySlotViews => _hotBarInventorySlotViews;
        private void Start()
        {
            var controller = ServiceLocator.Resolve<HotBarController>();
            controller.Setup(this);
        }

        public List<SlotData> GetSlotData()
        {
            var result = new List<SlotData>(_hotBarInventorySlotViews.Count);
            for (var slotIndex = 0; slotIndex < _hotBarInventorySlotViews.Count; slotIndex++)
            {
                var slot = _hotBarInventorySlotViews[slotIndex];
                result.Add(new SlotData { Index = slotIndex, _itemConfig = slot.CurrentGameItemConfig, Amount = slot.CurrentAmount });
            }
            return result;
        }
    }
}
