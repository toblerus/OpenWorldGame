using System;
using _Scripts.Injection;
using Object = UnityEngine.Object;

namespace _Scripts.Crafting
{
    public class CraftingPanelController
    {
        private CraftingPanelView _view;
        private CraftingSelectionModel _selectionModel;

        public void Setup(CraftingPanelView craftingPanelView)
        {
            _view = craftingPanelView;
            _selectionModel = ServiceLocator.Resolve<CraftingSelectionModel>();
            
            foreach (var category in (CraftingCategoryType[]) Enum.GetValues(typeof(CraftingCategoryType)))
            {
                var button = Object.Instantiate(_view.CategoryButtonPrefab, _view.CategoryParent);
                var view = button.GetComponent<CraftingCategoryView>();
                view.SetCategoryType(category);
                button.gameObject.SetActive(category != CraftingCategoryType.None);
            }
            
            _selectionModel.Selected.SkipValueOnSubscribe(UpdateCraftingPanel);
            
            _selectionModel.Select(CraftingCategoryType.None);
        }

        private void UpdateCraftingPanel(CraftingCategoryType value)
        {
            _view.ListHeaderText.text = value.ToString();
            
            _view.ListPanel.gameObject.SetActive(value != CraftingCategoryType.None);
            _view.CraftingPanel.gameObject.SetActive(value != CraftingCategoryType.None);
        }
    }
}