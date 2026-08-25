using _Scripts.Injection;
using _Scripts.Inventory;
using TMPro;
using UnityEngine;

namespace _Scripts.Crafting
{
    public class CraftingIngredientView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _amount;
        [SerializeField] private GameItemType _gameItemType = GameItemType.None;
        public string ItemName
        {
            set => _itemName.text = value;
        }

        public string Amount
        {
            set => _amount.text = value;
        }
        public GameItemType GameItemType
        {
            get => _gameItemType;
            set => _gameItemType = value;
        }

        private void Start()
        {
            var controller = ServiceLocator.Resolve<CraftingIngredientController>();
            controller.Setup(this);
        }
    }
}