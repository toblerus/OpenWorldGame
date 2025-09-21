using System;
using System.Collections.Generic;
using System.Linq;
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
        private Dictionary<int, (GameItem Item, int Amount)> _inventory = new();

        public ReactiveEmitter ItemAdded { get; } = new();
        public ReactiveEmitter ItemRemoved { get; } = new();
        public ReactiveValue<(int, (GameItem Item, int Amount))> InventorySlotModified { get; } = new();
        
        public ReactiveEmitter ItemDragFinished { get; } = new();
        
        public void AddItem(GameItem gameItem, int amount)
        {
            if (TryGetSlotForOrEmpty(gameItem, out var slot))
            {
                _inventory[slot] = (gameItem, Mathf.Min(_inventory[slot].Amount + amount, gameItem.MaxStack));
            }
            else
            {
                _inventory[slot] = (gameItem,Mathf.Min(amount, gameItem.MaxStack));
            }

            ItemAdded.Emit();
            InventorySlotModified.Value = (slot, (gameItem, _inventory[slot].Amount));
        }

        public void RemoveItem(GameItem gameItem, int amount)
        {
            var slot = _inventory.FirstOrDefault(stack => stack.Value.Item == gameItem);
            if (slot.Value.Item != null)
            {
                _inventory[slot.Key] = (slot.Value.Item, slot.Value.Amount - amount);
                if (_inventory[slot.Key].Amount <= 0)
                {
                    _inventory.Remove(slot.Key);
                }
                ItemRemoved.Emit();
                InventorySlotModified.Value = (slot.Key, (slot.Value.Item, slot.Value.Amount));
            }
        }

        public Dictionary<int, (GameItem Item, int Amount)> GetAllItems()
        {
            return _inventory;
        }

        private bool TryGetSlotFor(GameItem gameItem, out int slotIndex)
        {
            foreach (var (index, entry) in _inventory)
            {
                if (entry.Item != gameItem) continue;
                slotIndex = index;
                return true;
            }
            slotIndex = -1;
            return false;
        }
        
        private bool TryGetSlotForOrEmpty(GameItem gameItem, out int slotIndex)
        {
            foreach (var (index, entry) in _inventory)
            {
                if (entry.Item != gameItem) continue;
                slotIndex = index;
                return true;
            }
            
            foreach (var (index, entry) in _inventory)
            {
                if (entry.Item != null) continue;
                slotIndex = index;
                return true;
            }
            
            slotIndex = -1;
            return false;
        }
        
        public void SetupInventoryFromSlotData(List<SlotData> slots)
        {
            if (slots == null) return;

            var newInventory = new Dictionary<int, (GameItem Item, int Amount)>(slots.Count);
            foreach (var slotData in slots)
            {
                if (slotData == null) continue;

                var item = slotData.Item;
                var amount = slotData.Amount;

                if (item == null || amount <= 0)
                {
                    newInventory[slotData.Index] = (null, 0);
                    continue;
                }

                var clampedAmount = Mathf.Min(amount, item.MaxStack);
                newInventory[slotData.Index] = (item, clampedAmount);
            }

            _inventory = newInventory;
        }

    }
}