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
        private List<(GameItem Item, int Amount)> _inventory = new();
        public ReactiveValue<(int, (GameItem Item, int Amount))> InventorySlotModified { get; } = new();
        public ReactiveEmitter ItemDragFinished { get; } = new();
        
        public void AddItem(GameItem gameItem, int amount)
        {
            if (gameItem == null || amount <= 0) return;
            if (TryGetSlotForOrEmpty(gameItem, out var slot))
            {
                EnsureSize(slot + 1);
                var current = _inventory[slot];
                var newAmount = current.Item == null ? Mathf.Min(amount, gameItem.MaxStack) : Mathf.Min(current.Amount + amount, gameItem.MaxStack);
                _inventory[slot] = (gameItem, newAmount);
                InventorySlotModified.Value = (slot, _inventory[slot]);
                return;
            }
            var newSlot = _inventory.Count;
            EnsureSize(newSlot + 1);
            _inventory[newSlot] = (gameItem, Mathf.Min(amount, gameItem.MaxStack));
            InventorySlotModified.Value = (newSlot, _inventory[newSlot]);
        }
        
        public List<(GameItem Item, int Amount)> GetAllItems()
        {
            return _inventory;
        }
        

        private bool TryGetSlotForOrEmpty(GameItem gameItem, out int slotIndex)
        {
            slotIndex = -1;
            if (gameItem != null)
            {
                for (var i = 0; i < _inventory.Count; i++)
                {
                    var entry = _inventory[i];
                    if (entry.Item == gameItem && entry.Amount < gameItem.MaxStack)
                    {
                        slotIndex = i;
                        return true;
                    }
                }
            }
            for (var i = 0; i < _inventory.Count; i++)
            {
                var entry = _inventory[i];
                if (entry.Item == null || entry.Amount <= 0)
                {
                    slotIndex = i;
                    return true;
                }
            }
            return false;
        }

        private bool TryGetSlotFor(GameItem gameItem, out int slotIndex)
        {
            for (var i = 0; i < _inventory.Count; i++)
            {
                var entry = _inventory[i];
                if (entry.Item != null && gameItem != null && entry.Item == gameItem)
                {
                    slotIndex = i;
                    return true;
                }
            }
            slotIndex = -1;
            return false;
        }

        public void RemoveItem(GameItem gameItem, int amount)
        {
            if (gameItem == null || amount <= 0) return;
            if (!TryGetSlotFor(gameItem, out var index)) return;
            var entry = _inventory[index];
            var remaining = entry.Amount - amount;
            if (remaining <= 0)
            {
                _inventory[index] = (null, 0);
            }
            else
            {
                _inventory[index] = (entry.Item, remaining);
            }
            InventorySlotModified.Value = (index, _inventory[index]);
        }

        
        public void SetupInventoryFromSlotData(List<SlotData> slots)
        {
            if (slots == null) return;
            var targetSize = 0;
            foreach (var s in slots)
            {
                if (s == null) continue;
                if (s.Index + 1 > targetSize) targetSize = s.Index + 1;
            }
            var newInventory = new List<(GameItem Item, int Amount)>(targetSize);
            for (var i = 0; i < targetSize; i++)
            {
                newInventory.Add((null, 0));
            }
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
                var clamped = Mathf.Min(amount, item.MaxStack);
                newInventory[slotData.Index] = (item, clamped);
            }
            _inventory = newInventory;
        }

        private void EnsureSize(int size)
        {
            if (_inventory.Count >= size) return;
            var toAdd = size - _inventory.Count;
            for (var i = 0; i < toAdd; i++)
            {
                _inventory.Add((null, 0));
            }
        }
    }
}
