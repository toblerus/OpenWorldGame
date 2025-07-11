using System.Collections.Generic;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hud
{
    public class InventoryDragModel : MonoBehaviour
    {
        public static InventoryDragModel Instance { get; private set; }

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _draggedIcon;
        [SerializeField] private TextMeshProUGUI _draggedAmount;
        [SerializeField] private GameObject _itemDropPrefab;

        private InventorySlotView _sourceSlot;
        private GameItem _draggedItem;
        private int _draggedCount;
        private bool _isDragging;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            _draggedIcon.transform.parent.gameObject.SetActive(false);
        }

        public void StartDrag(InventorySlotView sourceSlot, GameItem item, string amountStr, Vector2 position)
        {
            _sourceSlot = sourceSlot;
            _draggedItem = item;
            _draggedCount = int.TryParse(amountStr, out var parsed) ? parsed : 0;
            _isDragging = true;

            _draggedIcon.sprite = item.Icon;
            _draggedAmount.text = _draggedCount.ToString();
            _draggedIcon.transform.parent.gameObject.SetActive(true);
            _draggedIcon.gameObject.SetActive(true);

            UpdateDragPosition(position);
        }

        public void UpdateDragPosition(Vector2 position)
        {
            if (!_isDragging) return;

            _draggedIcon.transform.position = position;
            _draggedAmount.transform.position = position;
        }

        public void EndDrag(InventorySlotView sourceSlot, InventorySlotView hoveredSlot)
        {
            if (!_isDragging) return;

            if (hoveredSlot != null && hoveredSlot != sourceSlot)
            {
                hoveredSlot.SetupGameItem(_draggedItem, _draggedCount);
            }
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                SpawnItemDrop(_draggedItem, _draggedCount);
            }
            else
            {
                sourceSlot?.SetupGameItem(_draggedItem, _draggedCount);
            }

            ClearDrag();
        }

        public void HandleDrop(InventorySlotView targetSlot)
        {
            if (!_isDragging) return;

            targetSlot.SetupGameItem(_draggedItem, _draggedCount);
            ClearDrag();
        }

        private void TryDropHoveredSlot()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                var slot = result.gameObject.GetComponent<InventorySlotView>();
                if (slot != null && slot.CurrentGameItem != null)
                {
                    SpawnItemDrop(slot.CurrentGameItem, slot.CurrentAmount);
                    slot.Clear();
                    return;
                }
            }
        }


        public void SpawnItemDrop(GameItem item, int amount)
        {
            var drop = Instantiate(_itemDropPrefab, transform.position + transform.forward, Quaternion.identity);
        }

        private void ClearDrag()
        {
            _draggedIcon.transform.parent.gameObject.SetActive(false);
            _draggedItem = null;
            _draggedCount = 0;
            _isDragging = false;
        }
    }
}