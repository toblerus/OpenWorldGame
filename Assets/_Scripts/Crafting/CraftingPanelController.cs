using System;
using _Scripts.Injection;
using _Scripts.Inventory;
using ReactiveCore.Runtime;
using Object = UnityEngine.Object;

namespace _Scripts.Crafting
{
    public class CraftingPanelController
    {
        private CraftingPanelView _view;
        private CraftingCategorySelectionModel _categorySelectionModel;
        private CraftingGameItemSelectionModel _gameItemSelectioNModel;

        public void Setup(CraftingPanelView craftingPanelView)
        {
            _view = craftingPanelView;
            _categorySelectionModel = ServiceLocator.Resolve<CraftingCategorySelectionModel>();
            _gameItemSelectioNModel = ServiceLocator.Resolve<CraftingGameItemSelectionModel>();
            
            foreach (var category in (CraftingCategoryType[]) Enum.GetValues(typeof(CraftingCategoryType)))
            {
                var button = Object.Instantiate(_view.CategoryButtonPrefab, _view.CategoryParent);
                var view = button.GetComponent<CraftingCategoryItemView>();
                view.SetCategoryType(category);
                button.gameObject.SetActive(category != CraftingCategoryType.None);
            }

            _categorySelectionModel.Selected.Combine(_gameItemSelectioNModel.Selected).Subscribe(UpdateCraftingPanel);
            
            _categorySelectionModel.Select(CraftingCategoryType.None);
        }

        private void UpdateCraftingPanel((CraftingCategoryType category, GameItemType item) selectedItem)
        {
            _view.ListHeaderText.text = selectedItem.category.ToString();

            _view.ListPanel.gameObject.SetActive(selectedItem.category != CraftingCategoryType.None);
            _view.CraftingPanel.gameObject.SetActive(selectedItem.item != GameItemType.None);
        }
    }
}