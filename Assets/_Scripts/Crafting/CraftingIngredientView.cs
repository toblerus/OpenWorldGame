using _Scripts.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace _Scripts.Crafting
{
    public class CraftingIngredientView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _amount;
        [SerializeField] private GameItemType _gameItemType = GameItemType.None;
        [SerializeField] private Image _gameItemSprite;
        
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
        public Sprite GameItemSprite
        {
            get => _gameItemSprite.sprite;
            set => _gameItemSprite.sprite = value;
        }
        public void SetupIngredient(CraftingIngredient ingredient, Sprite  gameItemSprite)
        {
            _gameItemType = ingredient.Type;
            _itemName.text = ingredient.Type.ToString();
            _amount.text = ingredient.Amount.ToString();
            _gameItemSprite.sprite = gameItemSprite;
            gameObject.SetActive(true);
        }
        public void ClearIngredient()
        {
            _gameItemType = GameItemType.None;
            _itemName.text = string.Empty;
            _amount.text = string.Empty;
            _gameItemSprite.sprite = null;
            gameObject.SetActive(false);
        }
    }
}