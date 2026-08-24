using System.Linq;
using _Scripts.Injection;
using _Scripts.Inventory;
using _Scripts.Inventory.ItemConfigs;
using ReactiveCore.Runtime;
using TMPro;
using UnityEngine;

namespace _Scripts.Crafting
{
    public class CraftingGameItemView : MonoBehaviour
    {
        [SerializeField] private ReactiveButton _selectButton;
        [SerializeField] private TextMeshProUGUI _listHeaderText;
        public ReactiveButton SelectButton => _selectButton;
        
        private GameItemType _gameItemType;
        private GameItemConfigController _gameItemConfigController;
        private CraftingCategoryType _category;
        public GameItemType GameItemType => _gameItemType;
        public CraftingCategoryType Category => _category;
        
        private void Start()
        {
            var controller = ServiceLocator.Resolve<CraftingGameItemController>();
            controller.Setup(this);
        }

        public void SetGameItemType(GameItemType gameItemType)
        {
            _gameItemType = gameItemType;
            _listHeaderText.text = _gameItemType.ToString();
            _gameItemConfigController = ServiceLocator.Resolve<GameItemConfigController>();
            var config = _gameItemConfigController.GetConfig(_gameItemType);
            if(config != null)
                _category = config.Category;
        }
    }
}
