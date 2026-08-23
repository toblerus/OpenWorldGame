using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Crafting
{
    public class CraftingGameItemController
    {
        private CraftingGameItemView _itemView;
        private CraftingGameItemSelectionModel _gameItemSelectionModel;

        public void Setup(CraftingGameItemView craftingGameItemView)
        {
            _itemView = craftingGameItemView;
            _gameItemSelectionModel = ServiceLocator.Resolve<CraftingGameItemSelectionModel>();

            _itemView.SelectButton.OnClickInteractable
                .SkipValueOnSubscribe(() =>
                {
                    _gameItemSelectionModel.Select(_itemView.GameItemType);
                });
        }
    }
}