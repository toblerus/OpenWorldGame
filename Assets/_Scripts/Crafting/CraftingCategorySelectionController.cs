using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Crafting
{
    public class CraftingCategorySelectionController
    {
        private CraftingCategorySelectionView _view;
        private CraftingSelectionModel _categorySelectionModel;

        public void Setup(CraftingCategorySelectionView craftingCategorySelectionView)
        {
            _view = craftingCategorySelectionView;
            _categorySelectionModel = ServiceLocator.Resolve<CraftingSelectionModel>();

            _view.SelectButton.OnClickInteractable
                .Subscribe(() =>
                {
                    _categorySelectionModel.Select(_view.CategoryType);
                });
        }
    }
}