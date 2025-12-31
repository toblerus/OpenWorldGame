using Hud;
using UnityEngine;

namespace Inventory.Hotbar
{
    public class HotBarController
    {
        private HotBarView _hotBarView;

        public void Setup(HotBarView hotBarView)
        {
            _hotBarView = hotBarView;
             for (var i = 0; i < _hotBarView.InventorySlotViews.Count; i++)
                 _hotBarView.InventorySlotViews[i].SlotIndex = i;
            SelectSlot(0);
        }
        
        private int _selectedIndex = 0;
        public int SelectedIndex => _selectedIndex;
        public GameItem ActiveItem => _hotBarView.InventorySlotViews[_selectedIndex].CurrentGameItem;

        public void SelectSlot(int index)
        {
            var slots = _hotBarView.InventorySlotViews;
            if (slots.Count == 0) return;

            _selectedIndex = (index + slots.Count) % slots.Count;
            for (var i = 0; i < slots.Count; i++)
                slots[i].Highlight(i == _selectedIndex);

            Debug.Log($"[HotBar] Selected slot: {_selectedIndex}, Item: {ActiveItem?.name ?? "None"}");
        }

        public void Scroll(int direction)
        {
            SelectSlot(_selectedIndex + direction);
        }
    }
}