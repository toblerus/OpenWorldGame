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
        private List<(GameItemConfig Item, int Amount)> _inventory;
        public ReactiveValue<(int slotIndex, (GameItemConfig Item, int Amount))> InventorySlotModified { get; } = new();
        public ReactiveEmitter ItemDragFinished { get; } = new();

        public void AddItem(GameItemConfig gameItemConfig, int amount)
        {
            if (gameItemConfig == null || amount <= 0) return;
            if (TryGetSlotForOrEmpty(gameItemConfig, out var slot))
            {
                Debug.Log("Index: " + slot + " " + _inventory.Count);
                EnsureSize(slot + 1);
                var current = _inventory[slot];
                var newAmount = current.Item == null ? Mathf.Min(amount, gameItemConfig.MaxStack) : Mathf.Min(current.Amount + amount, gameItemConfig.MaxStack);
                _inventory[slot] = (gameItemConfig, newAmount);
                InventorySlotModified.Value = (slot, _inventory[slot]);
                return;
            }
            var newSlot = _inventory.Count;
            EnsureSize(newSlot + 1);
            _inventory[newSlot] = (gameItemConfig, Mathf.Min(amount, gameItemConfig.MaxStack));
            InventorySlotModified.Value = (newSlot, _inventory[newSlot]);
        }

        public List<(GameItemConfig Item, int Amount)> GetAllItems()
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

        public void SetupInventoryFromSlotData(List<SlotData> slots, bool isHotbar = false)
        {
            if (slots == null) return;

            var hotbarSize = 5;
            var inventorySize = 15;
            var hotBarOffset = isHotbar ? inventorySize : 0;
            var minimumSize = inventorySize + hotbarSize;

            var targetSize = minimumSize;

            foreach (var slot in slots)
            {
                if (slot == null) continue;
                var index = slot.Index + hotBarOffset + 1;
                if (index > targetSize)
                    targetSize = index;
            }

            _inventory ??= new List<(GameItemConfig Item, int Amount)>(targetSize);

            while (_inventory.Count < targetSize)
                _inventory.Add((null, 0));

            foreach (var slot in slots)
            {
                if (slot == null) continue;

                var item = slot._itemConfig;
                var amount = slot.Amount;
                var index = slot.Index + hotBarOffset;

                if (item == null || amount <= 0)
                {
                    _inventory[index] = (null, 0);
                    continue;
                }

                var clamped = Mathf.Min(amount, item.MaxStack);
                _inventory[index] = (item, clamped);
            }

            for (var i = 0; i < _inventory.Count; i++)
                InventorySlotModified.Value = (i, _inventory[i]);
        }
        
        private bool TryFindSlot(int fromInclusive, int toInclusive, Func<(GameItemConfig Item, int Amount), bool> predicate, out int slotIndex)
        {
            slotIndex = -1;

            var step = fromInclusive <= toInclusive ? 1 : -1;

            for (var i = fromInclusive; step > 0 ? i <= toInclusive : i >= toInclusive; i += step)
            {
                if (i < 0 || i >= _inventory.Count)
                    continue;

                if (predicate(_inventory[i]))
                {
                    slotIndex = i;
                    return true;
                }
            }

            return false;
        }

        private bool TryGetSlotForOrEmpty(GameItemConfig gameItemConfig, out int slotIndex)
        {
            slotIndex = -1;
            if (gameItemConfig == null)
                return false;

            var hotbarStart = 15;
            var hotbarEnd = 19;
            var inventoryStart = 0;
            var inventoryEnd = 14;

            if (TryFindSlot(hotbarStart, hotbarEnd, slot => slot.Item == gameItemConfig && slot.Amount < gameItemConfig.MaxStack, out slotIndex))
                return true;

            if (TryFindSlot(hotbarStart, hotbarEnd, slot => slot.Item == null || slot.Amount <= 0, out slotIndex))
                return true;

            if (TryFindSlot(inventoryStart, inventoryEnd, slot => slot.Item == gameItemConfig && slot.Amount < gameItemConfig.MaxStack, out slotIndex))
                return true;

            if (TryFindSlot(inventoryStart, inventoryEnd, slot => slot.Item == null || slot.Amount <= 0, out slotIndex))
                return true;

            return false;
        }
        
        private bool TryGetSlotFor(GameItemConfig gameItemConfig, out int slotIndex)
        {
            foreach (var entry in _inventory.Select(t => _inventory.FirstOrDefault(value => value.Item == gameItemConfig)).Where(entry => entry.Item != null && gameItemConfig != null && entry.Item == gameItemConfig))
            {
                slotIndex = _inventory.IndexOf(entry);
                return true;
            }
            slotIndex = -1;
            return false;
        }

        public void RemoveItem(GameItemConfig gameItemConfig, int amount)
        {
            if (gameItemConfig == null || amount <= 0) return;
            if (!TryGetSlotFor(gameItemConfig, out var index)) return;
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
