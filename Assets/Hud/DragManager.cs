using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragManager : MonoBehaviour
    {
        public static DragManager Instance { get; private set; }

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _draggedIcon;
        [SerializeField] private TextMeshProUGUI _draggedAmount;

        private InventorySlotController _sourceController;
        private GameItem _draggedItem;
        private int _draggedCount;
        private bool _isDragging;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            _draggedIcon.transform.parent.gameObject.SetActive(false);
        }

        public void StartDrag(InventorySlotController sourceController, GameItem item, string amountStr, Vector2 position)
        {
            _sourceController = sourceController;
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

        public void EndDrag(InventorySlotController sourceController, InventorySlotView hoveredSlot)
        {
            if (!_isDragging) return;

            if (hoveredSlot != null && hoveredSlot != sourceController.View)
            {
                hoveredSlot.SetupGameItem(_draggedItem, _draggedCount);
            }
            else
            {
                sourceController.AcceptDrop(_draggedItem, _draggedCount);
            }

            ClearDrag();
        }

        public void HandleDrop(InventorySlotView targetSlot)
        {
            if (!_isDragging) return;

            targetSlot.SetupGameItem(_draggedItem, _draggedCount);
            ClearDrag();
        }

        private void ClearDrag()
        {
            _draggedIcon.transform.parent.gameObject.SetActive(false);
            _draggedItem = null;
            _draggedCount = 0;
            _isDragging = false;
        }
    }