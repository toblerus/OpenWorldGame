using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryPanelView _inventoryPanelView;
        private InventoryModel _inventoryModel;
        [SerializeField] private List<GameItem> _defaultItems;

        private void Awake()
        {
            _inventoryModel = new InventoryModel();
        }

        private void Start()
        {
            foreach (var gameItem in _defaultItems)
            {
                AddItem(gameItem, Random.Range(0,64));
            }
        }

        public void AddItem(GameItem gameItem, int amount)
        {
            _inventoryModel.AddItem(gameItem, amount);
            RefreshView();
        }

        public void RemoveItem(GameItem gameItem, int amount)
        {
            _inventoryModel.RemoveItem(gameItem, amount);
            RefreshView();
        }

        private void RefreshView()
        {
            var items = _inventoryModel.GetAllItems();
            var slots = _inventoryPanelView.InventorySlotViews;

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