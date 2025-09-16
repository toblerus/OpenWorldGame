using Inventory;
using UnityEngine;

namespace Hud
{
    public class HotBarController
    {
        private HotBarView _hotBarView;

        public void Setup(HotBarView hotBarView)
        {
            _hotBarView = hotBarView;
            SetupSlots();
            SelectSlot(0);
        }

        public void SetupSlots()
        {
            foreach (var slot in _hotBarView.InventorySlotViews)
            {
                slot.Clear();
            }
        }
        
        private int _selectedIndex = 0;
        public GameItem ActiveItem => _hotBarView.InventorySlotViews[_selectedIndex].CurrentGameItem;

        public void SelectSlot(int index)
        {
            var slots = _hotBarView.InventorySlotViews;
            if (slots.Count == 0) return;

            // Clamp and wrap
            _selectedIndex = (index + slots.Count) % slots.Count;

            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Highlight(i == _selectedIndex);
            }

            Debug.Log($"[HotBar] Selected slot: {_selectedIndex}, Item: {ActiveItem?.name ?? "None"}");
        }

        public void Scroll(int direction)
        {
            SelectSlot(_selectedIndex + direction);
        }

    }
}
