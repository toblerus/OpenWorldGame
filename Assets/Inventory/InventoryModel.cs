using System;
using System.Collections.Generic;
using ReactiveCore;
using ReactiveCore.Runtime;
using Saving;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory
{
    public class InventoryModel
    {
        private Dictionary<GameItem, int> _inventory = new();

        public ReactiveEmitter ItemAdded { get; } = new();
        public ReactiveEmitter ItemRemoved { get; } = new();
        public ReactiveEmitter InventoryModified { get; } = new();

        public InventoryModel()
        {
            if (!ES3.KeyExists(SavegameConstants.Inventory)) return;
            
            var inventorySavegame = ES3.Load<Dictionary<GameItem, int>>(SavegameConstants.Inventory);
            
            foreach (var item in inventorySavegame)
            {
                AddItem(item.Key, item.Value);
            }
        }
        
        public void AddItem(GameItem gameItem, int amount)
        {
            if (_inventory.ContainsKey(gameItem))
            {
                _inventory[gameItem] = Mathf.Min(_inventory[gameItem] + amount, gameItem.MaxStack);
            }
            else
            {
                _inventory[gameItem] = Mathf.Min(amount, gameItem.MaxStack);
            }

            ItemAdded.Emit();
            InventoryModified.Emit();
            ES3.Save(SavegameConstants.Inventory, _inventory);
        }

        public void RemoveItem(GameItem gameItem, int amount)
        {
            if (!_inventory.ContainsKey(gameItem)) return;
            _inventory[gameItem] -= amount;
            if (_inventory[gameItem] <= 0)
            {
                _inventory.Remove(gameItem);
            }
            ItemRemoved.Emit();
            InventoryModified.Emit();
            ES3.Save(SavegameConstants.Inventory, _inventory);
        }

        public Dictionary<GameItem, int> GetAllItems()
        {
            return _inventory;
        }
    }
}