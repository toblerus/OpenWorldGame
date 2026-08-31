using System.Collections.Generic;
using ReactiveCore.Runtime;
using UnityEngine;

namespace _Scripts.Inventory
{
    public class InventoryModel
    {
        private const int InventorySize = 15;
        private const int HotbarSize = 5;
        private const int TotalSize = InventorySize + HotbarSize;

        private readonly List<(GameItemConfig Item, int Amount)> _inventory = new();

        public ReactiveValue<(int slotIndex, (GameItemConfig Item, int Amount))> InventorySlotModified { get; } = new();
        public ReactiveEmitter ItemDragFinished { get; } = new();

        public InventoryModel()
        {
            EnsureSize(TotalSize);
        }

        public List<(GameItemConfig Item, int Amount)> GetAllItems()
        {
            return _inventory;
        }

        public void AddItem(GameItemConfig gameItemConfig, int amount)
        {
            if (gameItemConfig == null || amount <= 0) return;

            if (!TryGetSlotForOrEmpty(gameItemConfig, out var index))
                index = _inventory.Count;

            EnsureSize(index + 1);
            var current = _inventory[index];
            var newAmount = current.Item == null ? amount : current.Amount + amount;
            SetSlot(index, gameItemConfig, newAmount);
            NotifySlotModified(index);
        }

        public void RemoveItem(GameItemConfig gameItemConfig, int amount)
        {
            if (gameItemConfig == null || amount <= 0) return;

            if (TryFindItemSlot(gameItemConfig, out var index))
                RemoveAt(index, amount);
        }

        public void RemoveAt(int index, int amount)
        {
            if (index < 0 || index >= _inventory.Count || amount <= 0) return;

            var entry = _inventory[index];
            if (entry.Item == null) return;

            SetSlot(index, entry.Item, entry.Amount - amount);
            NotifySlotModified(index);
        }

        public void SwapOrMove(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= _inventory.Count || toIndex < 0 || fromIndex == toIndex) return;

            var from = _inventory[fromIndex];
            if (IsEmpty(from)) return;

            EnsureSize(toIndex + 1);
            var to = _inventory[toIndex];
            _inventory[fromIndex] = IsEmpty(to) ? (null, 0) : to;
            _inventory[toIndex] = from;

            // Both slots must be updated before notifying UI and save subscribers.
            NotifySlotModified(fromIndex);
            NotifySlotModified(toIndex);
        }

        public void SetupInventoryFromSlotData(List<SlotData> slots, bool isHotbar = false)
        {
            if (slots == null) return;

            var offset = isHotbar ? InventorySize : 0;
            foreach (var slot in slots)
            {
                if (slot == null || slot.Index < 0) continue;
                SetSlot(slot.Index + offset, slot._itemConfig, slot.Amount);
            }

            for (var i = 0; i < _inventory.Count; i++)
                NotifySlotModified(i);
        }

        private bool TryGetSlotForOrEmpty(GameItemConfig gameItemConfig, out int slotIndex)
        {
            // Preserve hotbar priority, including empty hotbar slots before inventory stacks.
            return TryFindStackWithSpace(gameItemConfig, InventorySize, TotalSize, out slotIndex)
                || TryFindEmptySlot(InventorySize, TotalSize, out slotIndex)
                || TryFindStackWithSpace(gameItemConfig, 0, InventorySize, out slotIndex)
                || TryFindEmptySlot(0, InventorySize, out slotIndex);
        }

        private bool TryFindItemSlot(GameItemConfig gameItemConfig, out int slotIndex)
        {
            for (var i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].Item != gameItemConfig) continue;
                slotIndex = i;
                return true;
            }

            slotIndex = -1;
            return false;
        }

        private bool TryFindStackWithSpace(GameItemConfig gameItemConfig, int startIndex, int endExclusive, out int slotIndex)
        {
            for (var i = startIndex; i < endExclusive && i < _inventory.Count; i++)
            {
                var slot = _inventory[i];
                if (slot.Item != gameItemConfig || slot.Amount >= gameItemConfig.MaxStack) continue;
                slotIndex = i;
                return true;
            }

            slotIndex = -1;
            return false;
        }

        private bool TryFindEmptySlot(int startIndex, int endExclusive, out int slotIndex)
        {
            for (var i = startIndex; i < endExclusive && i < _inventory.Count; i++)
            {
                if (!IsEmpty(_inventory[i])) continue;
                slotIndex = i;
                return true;
            }

            slotIndex = -1;
            return false;
        }

        private static bool IsEmpty((GameItemConfig Item, int Amount) slot)
        {
            return slot.Item == null || slot.Amount <= 0;
        }

        private void SetSlot(int index, GameItemConfig item, int amount)
        {
            EnsureSize(index + 1);
            _inventory[index] = item == null || amount <= 0
                ? (null, 0)
                : (item, Mathf.Min(amount, item.MaxStack));
        }

        private void NotifySlotModified(int index)
        {
            InventorySlotModified.Value = (index, _inventory[index]);
        }

        private void EnsureSize(int size)
        {
            while (_inventory.Count < size)
                _inventory.Add((null, 0));
        }
    }
}
