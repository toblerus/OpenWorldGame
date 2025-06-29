using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryModel
    {
        private Dictionary<GameItem, int> _inventory = new Dictionary<GameItem, int>();

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
        }

        public void RemoveItem(GameItem gameItem, int amount)
        {
            if (!_inventory.ContainsKey(gameItem)) return;
            _inventory[gameItem] -= amount;
            if (_inventory[gameItem] <= 0)
            {
                _inventory.Remove(gameItem);
            }
        }

        public Dictionary<GameItem, int> GetAllItems()
        {
            return _inventory;
        }
    }
}