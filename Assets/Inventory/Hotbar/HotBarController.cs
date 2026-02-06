using System.Linq;
using Hud;
using Injection;
using UnityEngine;

namespace Inventory.Hotbar
{
    public class HotBarController
    {
        private HotBarView _hotBarView;
        private int _selectedIndex = 0;
        private InventoryModel _inventoryModel;
        public int SelectedIndex => _selectedIndex;
        public GameItem ActiveItem => _hotBarView.InventorySlotViews[_selectedIndex].CurrentGameItem;

        public void Setup(HotBarView hotBarView)
        {
            _inventoryModel = ServiceLocator.Resolve<InventoryModel>();
            _hotBarView = hotBarView;
             for (var i = 0; i < _hotBarView.InventorySlotViews.Count; i++)
                 _hotBarView.InventorySlotViews[i].SlotIndex = i+15;
            SelectSlot(15);
            
            _inventoryModel.InventorySlotModified.SkipValueOnSubscribe(slot =>
            {
                var (index, (item, amount)) = slot;
                
                var slotMatchingIndex = _hotBarView.InventorySlotViews.FirstOrDefault(slots => slots.SlotIndex == index);
                if (slotMatchingIndex != null)
                {
                    slotMatchingIndex.SetupGameItem(item, amount);
                }
            });
        }
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