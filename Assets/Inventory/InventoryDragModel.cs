using Injection;
using Inventory;
using TMPro;
using UnityEngine;
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
        private InventoryModel _inventoryModel;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            _draggedIcon.transform.parent.gameObject.SetActive(false);
        }

        private void Start()
        {
            _inventoryModel = ServiceLocator.Resolve<InventoryModel>();
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
            Debug.LogError(hoveredSlot.name);
            if (!_isDragging) return;
            if (hoveredSlot != null && hoveredSlot != sourceSlot)
            {
                _inventoryModel.SwapOrMove(sourceSlot.SlotIndex, hoveredSlot.SlotIndex);
                _inventoryModel.ItemDragFinished.Emit();
            }
            else if (hoveredSlot == null)
            {
                _inventoryModel.RemoveAt(sourceSlot.SlotIndex, _draggedCount);
                SpawnItemDrop(_draggedItem, _draggedCount);
                _inventoryModel.ItemDragFinished.Emit();
            }
            ClearDrag();
        }

        public void HandleDrop(InventorySlotView targetSlot)
        {
            if (!_isDragging) return;
            _inventoryModel.SwapOrMove(_sourceSlot.SlotIndex, targetSlot.SlotIndex);
            ClearDrag();
            _inventoryModel.ItemDragFinished.Emit();
        }

        public void SpawnItemDrop(GameItem item, int amount)
        {
            var model = new ItemDropModel(item, amount);
            var view = Instantiate(_itemDropPrefab, transform.position + transform.forward, Quaternion.identity);
            view.GetComponent<ItemDropView>().Setup(model);
        }

        private void ClearDrag()
        {
            _draggedIcon.transform.parent.gameObject.SetActive(false);
            _draggedItem = null;
            _draggedCount = 0;
            _isDragging = false;
            _sourceSlot = null;
        }
    }
}
