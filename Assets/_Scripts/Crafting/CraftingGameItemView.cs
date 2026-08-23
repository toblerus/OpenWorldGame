using _Scripts.Injection;
using _Scripts.Inventory;
using ReactiveCore.Runtime;
using UnityEngine;

namespace _Scripts.Crafting
{
    public class CraftingGameItemView : MonoBehaviour
    {
        [SerializeField] private ReactiveButton _selectButton;
        public ReactiveButton SelectButton => _selectButton;
        
        private GameItemType _gameItemType;
        public GameItemType GameItemType => _gameItemType;
        
        private void Start()
        {
            var controller = ServiceLocator.Resolve<CraftingGameItemController>();
            controller.Setup(this);
        }

        public void SetCategoryType(GameItemType categoryType)
        {
            _gameItemType = categoryType;
        }
    }
}
