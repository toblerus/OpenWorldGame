using System;
using System.Collections.Generic;
using System.Linq;
using Injection;
using Saving;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryPanelView _view;
        private InventoryModel _inventoryModel;
        [SerializeField] private List<GameItemConfig> _defaultItems;
        private int _hotBarOffset = 15;

        private void Start()
        {
            Setup();
            LoadInventory();
        }

        private void Setup()
        {
            _inventoryModel = ServiceLocator.Resolve<InventoryModel>();
            for (var i = 0; i < _view.InventorySlotViews.Count; i++)
            {
                var view = _view.InventorySlotViews[i];
                view.SlotIndex = i;
            }
            _inventoryModel.InventorySlotModified.SkipValueOnSubscribe(slot =>
            {
                var (index, (item, amount)) = slot;

                if (index < _hotBarOffset)
                {
                    var slotMatchingIndex = _view.InventorySlotViews.FirstOrDefault(slots => slots.SlotIndex == index);
                    if (slotMatchingIndex != null)
                    {
                        slotMatchingIndex.SetupGameItem(item, amount);
                    }
                }
            });
            _inventoryModel.InventorySlotModified.SkipValueOnSubscribe(_ => { SaveInventory(); });
            _inventoryModel.ItemDragFinished.SkipValueOnSubscribe(SaveInventory);
        }

        private void LoadInventory()
        {
            if (ES3.KeyExists(SavegameConstants.Inventory))
            {
                var inventorySavegame = ES3.Load<List<SlotData>>(SavegameConstants.Inventory);
                if (inventorySavegame == null) return;
                _inventoryModel.SetupInventoryFromSlotData(inventorySavegame);
            }
            else
            {
                _inventoryModel.SetupInventoryFromSlotData(_view.GetSlotData());
                foreach (var item in _defaultItems)
                {
                    _inventoryModel.AddItem(item, Random.Range(1, 64));
                }
            }
        }

        private void SaveInventory()
        {
            ES3.Save(SavegameConstants.Inventory, _view.GetSlotData());
        }
    }
}
