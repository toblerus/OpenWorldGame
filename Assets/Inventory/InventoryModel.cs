using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveCore;
using ReactiveCore.Runtime;
using Saving;
using UnityEngine;

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

        public void SwapOrMove(int fromIndex, int toIndex)
        {
            EnsureSize(Mathf.Max(fromIndex, toIndex) + 1);
            var from = _inventory[fromIndex];
            var to = _inventory[toIndex];
            if (from.Item == null || from.Amount <= 0) return;
            if (to.Item == null || to.Amount <= 0)
            {
                _inventory[toIndex] = from;
                _inventory[fromIndex] = (null, 0);
                InventorySlotModified.Value = (fromIndex, _inventory[fromIndex]);
                InventorySlotModified.Value = (toIndex, _inventory[toIndex]);
                return;
            }
            _inventory[toIndex] = from;
            _inventory[fromIndex] = to;
            InventorySlotModified.Value = (fromIndex, _inventory[fromIndex]);
            InventorySlotModified.Value = (toIndex, _inventory[toIndex]);
        }

        public void RemoveAt(int index, int amount)
        {
            EnsureSize(index + 1);
            var entry = _inventory[index];
            if (entry.Item == null || amount <= 0) return;
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
            for (var i = 0; i < _inventory.Count; i++)
            {
                InventorySlotModified.Value = (i, _inventory[i]);
            }
        }

        private bool TryGetSlotForOrEmpty(GameItem gameItem, out int slotIndex)
        {
            slotIndex = -1;
            if (gameItem != null)
            {
                foreach (var entry in _inventory.Select(t => _inventory.FirstOrDefault(value => value.Item == gameItem)).Where(entry => entry.Item != null && gameItem != null && entry.Item == gameItem))
                {
                    slotIndex = _inventory.IndexOf(entry);
                    return true;
                }
            }
            foreach (var entry in _inventory.Select(t => _inventory.FirstOrDefault(value => value.Item == null)).Where(entry => entry.Item == null || entry.Amount <= 0))
            {
                slotIndex = _inventory.IndexOf(entry);
                return true;
            }
            return false;
        }

        private bool TryGetSlotFor(GameItem gameItem, out int slotIndex)
        {
            foreach (var entry in _inventory.Select(t => _inventory.FirstOrDefault(value => value.Item == gameItem)).Where(entry => entry.Item != null && gameItem != null && entry.Item == gameItem))
            {
                slotIndex = _inventory.IndexOf(entry);
                return true;
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
