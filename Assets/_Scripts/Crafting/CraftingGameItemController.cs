using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Crafting
{
    public class CraftingGameItemController
    {
        private CraftingGameItemView _itemView;
        private CraftingGameItemSelectionModel _gameItemSelectionModel;
        private CraftingCategorySelectionModel _categorySelectionModel;

        public void Setup(CraftingGameItemView craftingGameItemView)
        {
            _itemView = craftingGameItemView;
            _gameItemSelectionModel = ServiceLocator.Resolve<CraftingGameItemSelectionModel>();
            _categorySelectionModel = ServiceLocator.Resolve<CraftingCategorySelectionModel>();

            _itemView.SelectButton.OnClickInteractable
                .SkipValueOnSubscribe(() =>
                {
                    _gameItemSelectionModel.Select(_itemView.GameItemType);
                });
            
            _categorySelectionModel.Selected
                .Subscribe(category => _itemView.gameObject.SetActive(_itemView.Category ==  category));
        }
    }
}