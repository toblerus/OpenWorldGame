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
        [SerializeField] private List<GameItem> _defaultItems;

        private void Start()
        {
            _inventoryModel = ServiceLocator.Resolve<InventoryModel>();

            _inventoryModel.ItemAdded.Subscribe(_ => RefreshView());
        }

        private void RefreshView()
        {
            var items = _inventoryModel.GetAllItems();
            var slots = _view.InventorySlotViews;

            var index = 0;
            foreach (var kvp in items.TakeWhile(kvp => index < slots.Count))
            {
                slots[index].SetupGameItem(kvp.Key, kvp.Value);
                index++;
            }

            for (; index < slots.Count; index++)
            {
                slots[index].Clear();
            }
        }
    }
}