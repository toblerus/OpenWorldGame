using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Crafting
{
    public class CraftingCategoryController
    {
        private CraftingCategoryView _view;
        private CraftingSelectionModel _categorySelectionModel;

        public void Setup(CraftingCategoryView craftingCategoryView)
        {
            _view = craftingCategoryView;
            _categorySelectionModel = ServiceLocator.Resolve<CraftingSelectionModel>();

            _view.SelectButton.OnClickInteractable
                .SkipValueOnSubscribe(() =>
                {
                    _categorySelectionModel.Select(_view.CategoryType);
                });
        }
    }
}