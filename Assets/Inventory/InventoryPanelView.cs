using System.Collections.Generic;
using PanelCore;
using UnityEngine;

namespace Inventory
{
    public class InventoryPanelView : Panel
    {
        [SerializeField] private List<InventorySlotView> _inventorySlotViews;
        public List<InventorySlotView> InventorySlotViews => _inventorySlotViews;
    }
}
