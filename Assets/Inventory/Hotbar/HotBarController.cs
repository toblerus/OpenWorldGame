using System.Collections.Generic;
using System.Linq;
using Hud;
using Injection;
using ReactiveCore.Runtime;
using Saving;
using UnityEngine;

namespace Inventory.Hotbar
{
    public class HotBarController
    {
        private HotBarView _hotBarView;
        private ReactiveValue<int> _selectedIndex = new(0);
        private int _hotBarOffset = 15;
        private InventoryModel _inventoryModel;

        private ReactiveValue<GameItemConfig> _activeItemConfig = new();
        public ReactiveValue<GameItemConfig> ActiveItemConfig => _activeItemConfig;    

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

                if (index >= _hotBarOffset)
                {
                    var slotMatchingIndex = _hotBarView.HotBarInventorySlotViews.FirstOrDefault(slots => slots.SlotIndex == index);
                    if (slotMatchingIndex != null)
                        slotMatchingIndex.SetupGameItem(item, amount);
                }
            });
            
            LoadHotbar();
            
            _inventoryModel.InventorySlotModified.SkipValueOnSubscribe(_ => { SaveHotbar(); });
            _inventoryModel.ItemDragFinished.SkipValueOnSubscribe(SaveHotbar);

            var index = 0;

            _selectedIndex.Subscribe(value =>
            {
                index = value;
                _activeItemConfig.Value = _hotBarView.HotBarInventorySlotViews[value].CurrentGameItemConfig;
            });
            _inventoryModel.InventorySlotModified.SkipValueOnSubscribe(_ =>
            {
                _activeItemConfig.Value = _hotBarView.HotBarInventorySlotViews[index].CurrentGameItemConfig;
            });
        }
        public void SelectSlot(int index)
        {
            var slots = _hotBarView.HotBarInventorySlotViews;
            if (slots.Count == 0) return;

            _selectedIndex.Value = (index + slots.Count) % slots.Count;
            for (var i = 0; i < slots.Count; i++)
                slots[i].Highlight(i == _selectedIndex.Value);

            Debug.Log($"[HotBar] Selected slot: {_selectedIndex?.Value}, Item: {ActiveItemConfig?.Value?.name ?? "None"}");
        }

        public void Scroll(int direction)
        {
            SelectSlot(_selectedIndex.Value + direction);
        }
        
        private void LoadHotbar()
        {
            if (ES3.KeyExists(SavegameConstants.HotBar))
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

        public void RemoveActiveItem()
        {
            _inventoryModel.RemoveItem(_activeItemConfig.Value, 1);
        }
    }
}