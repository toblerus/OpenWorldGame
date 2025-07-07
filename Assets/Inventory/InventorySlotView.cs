using Injection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _itemAmount;
        [SerializeField] private GameItem _currentGameItem;

        public GameItem CurrentGameItem => _currentGameItem;
        public int CurrentAmount => _currentAmount;
        private int _currentAmount;

        private InventorySlotController _controller;

        private void Start()
        {
            _controller = ServiceLocator.Resolve<InventorySlotController>();
            _controller.Setup(this);
        }

        public void SetupGameItem(GameItem gameItem, int amount)
        {
            _currentGameItem = gameItem;
            _currentAmount = amount;
            _itemIcon.sprite = gameItem.Icon;
            _itemIcon.enabled = true;
            _itemAmount.text = amount.ToString();
        }

        public void Clear()
        {
            _currentGameItem = null;
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
            _controller.OnEndDrag(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            InventoryDragModel.Instance.HandleDrop(this);
        }
    }
}