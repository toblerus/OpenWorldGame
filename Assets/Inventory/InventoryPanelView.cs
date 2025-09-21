using System;
using System.Collections.Generic;
using PanelCore;
using UnityEngine;

namespace Inventory
{
    public class InventoryPanelView : Panel
    {
        [SerializeField] private List<InventorySlotView> _inventorySlotViews;
        public List<InventorySlotView> InventorySlotViews => _inventorySlotViews;

        public List<SlotData> GetSlotData()
        {
            var result = new List<SlotData>(_inventorySlotViews.Count);
            for (var slotIndex = 0; slotIndex < _inventorySlotViews.Count; slotIndex++)
            {
                var slot = _inventorySlotViews[slotIndex];
                result.Add(new SlotData { Index = slotIndex, Item = slot.CurrentGameItem, Amount = slot.CurrentAmount });
            }
            return result;
        }
    }
}
