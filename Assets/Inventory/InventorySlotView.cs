using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _itemAmount;
        [SerializeField] private GameItem _currentGameItem;

        public GameItem CurrentGameItem => _currentGameItem;

        public void SetupGameItem(GameItem gameItem, int kvpValue)
        {
            _currentGameItem = gameItem;
            _itemIcon.sprite = gameItem.Icon;
            _itemAmount.text = kvpValue.ToString();
        }

        public void Clear()
        {
            _currentGameItem = null;
            _itemAmount.text = "";
        }
    }
}