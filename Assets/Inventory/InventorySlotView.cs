using Hud;
using Injection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _itemAmount;
        [SerializeField] private GameItemConfig _currentGameItemConfig;
        [SerializeField] private GameObject _highlight;

        public int SlotIndex { get; set; }
        public GameItemConfig CurrentGameItemConfig => _currentGameItemConfig;
        public int CurrentAmount => _currentAmount;
        private int _currentAmount;

        private InventorySlotController _controller;
        private static InventorySlotView _currentlyHovered;
        public static InventorySlotView CurrentlyHovered => _currentlyHovered;

        private void Awake()
        {
            Clear();
        }

        private void Start()
        {
            _controller = ServiceLocator.Resolve<InventorySlotController>();
            _controller.Setup(this);
        }

        public void SetupGameItem(GameItemConfig gameItemConfig, int amount)
        {
            _itemIcon.enabled = gameItemConfig != null;
            _itemIcon.sprite = gameItemConfig?.Icon;
            _currentGameItemConfig = gameItemConfig;
            _currentAmount = amount;
            _itemAmount.text = amount > 0 ? amount.ToString() : "";
        }

        public void Clear()
        {
            _currentGameItemConfig = null;
            _currentAmount = 0;
            _itemIcon.enabled = false;
            _itemAmount.text = "";
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _controller.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _controller.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var target = eventData.pointerEnter == null ? null : eventData.pointerEnter.GetComponentInParent<InventorySlotView>();
            InventoryDragModel.Instance.EndDrag(this, target);
        }

        public void OnDrop(PointerEventData eventData)
        {
            InventoryDragModel.Instance.HandleDrop(this);
        }

        public void RequestDrop()
        {
            _controller.OnDropRequest();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _currentlyHovered = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_currentlyHovered == this)
                _currentlyHovered = null;
        }

        public bool HasItem => _currentGameItemConfig != null;

        public void Highlight(bool isActive)
        {
            if (_highlight != null)
                _highlight.SetActive(isActive);
        }
    }
}
