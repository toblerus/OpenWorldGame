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
        private GameItemConfig _draggedItemConfig;
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

        public void StartDrag(InventorySlotView sourceSlot, GameItemConfig itemConfig, string amountStr, Vector2 position)
        {
            _sourceSlot = sourceSlot;
            _draggedItemConfig = itemConfig;
            _draggedCount = int.TryParse(amountStr, out var parsed) ? parsed : 0;
            _isDragging = true;
            _draggedIcon.sprite = itemConfig.Icon;
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
                _inventoryModel.SwapOrMove(sourceSlot.SlotIndex, hoveredSlot.SlotIndex);
                _inventoryModel.ItemDragFinished.Emit();
            }
            else if (hoveredSlot == null)
            {
                _inventoryModel.RemoveAt(sourceSlot.SlotIndex, _draggedCount);
                SpawnItemDrop(_draggedItemConfig, _draggedCount);
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

        public void SpawnItemDrop(GameItemConfig itemConfig, int amount)
        {
            var model = new ItemDropModel(itemConfig, amount);
            var view = Instantiate(_itemDropPrefab, transform.position + transform.forward, Quaternion.identity);
            view.GetComponent<ItemDropView>().Setup(model);
        }

        private void ClearDrag()
        {
            _draggedIcon.transform.parent.gameObject.SetActive(false);
            _draggedItemConfig = null;
            _draggedCount = 0;
            _isDragging = false;
            _sourceSlot = null;
        }
    }
}
