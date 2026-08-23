using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Crafting
{
    public class CraftingCategoryController
    {
        private CraftingCategoryItemView _itemView;
        private CraftingCategorySelectionModel _categoryCategorySelectionModel;

        public void Setup(CraftingCategoryItemView craftingCategoryItemView)
        {
            _itemView = craftingCategoryItemView;
            _categoryCategorySelectionModel = ServiceLocator.Resolve<CraftingCategorySelectionModel>();

            _itemView.SelectButton.OnClickInteractable
                .SkipValueOnSubscribe(() =>
                {
                    _categoryCategorySelectionModel.Select(_itemView.CategoryType);
                });
        }
    }
}