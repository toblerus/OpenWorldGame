using System.Collections.Generic;
using System.Linq;
using Hud;
using Injection;
using Saving;
using UnityEngine;

namespace Inventory.Hotbar
{
    public class HotBarController
    {
        private HotBarView _hotBarView;
        private int _selectedIndex = 0;
        private int _hotBarOffset = 15;
        private InventoryModel _inventoryModel;
        public int SelectedIndex => _selectedIndex;
        
        public GameItem ActiveItem => _hotBarView.HotBarInventorySlotViews[_selectedIndex].CurrentGameItem;

        public void Setup(HotBarView hotBarView)
        {
            _inventoryModel = ServiceLocator.Resolve<InventoryModel>();
            _hotBarView = hotBarView;
             for (var i = 0; i < _hotBarView.HotBarInventorySlotViews.Count; i++)
                 _hotBarView.HotBarInventorySlotViews[i].SlotIndex = i+_hotBarOffset;
            SelectSlot(_hotBarOffset);
            
            _inventoryModel.InventorySlotModified.SkipValueOnSubscribe(slot =>
            {
                var (index, (item, amount)) = slot;

                if (index >= _hotBarOffset && item != null)
                {
                    var slotMatchingIndex = _hotBarView.HotBarInventorySlotViews.FirstOrDefault(slots => slots.SlotIndex == index);
                    if (slotMatchingIndex != null)
                    {
                        slotMatchingIndex.SetupGameItem(item, amount);
                    }
                }
            });
            
            LoadHotbar();
            
            _inventoryModel.InventorySlotModified.SkipValueOnSubscribe(_ => { SaveHotbar(); });
            _inventoryModel.ItemDragFinished.SkipValueOnSubscribe(SaveHotbar);
        }
        public void SelectSlot(int index)
        {
            var slots = _hotBarView.HotBarInventorySlotViews;
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
        
        private void LoadHotbar()
        {
            if (ES3.KeyExists(SavegameConstants.Inventory))
            {
                var inventorySavegame = ES3.Load<List<SlotData>>(SavegameConstants.HotBar);
                if (inventorySavegame == null) return;
                _inventoryModel.SetupInventoryFromSlotData(inventorySavegame, true);
            }
        }
        
        private void SaveHotbar()
        {
            ES3.Save(SavegameConstants.HotBar, _hotBarView.GetSlotData());
        }
    }
}