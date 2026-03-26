using System;
using _Scripts.Injection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.Crafting
{
    public class CraftingCategoryPanelController
    {
        private CraftingCategoryPanelView _view;
        private CraftingSelectionModel _selectionModel;

        public void Setup(CraftingCategoryPanelView craftingCategoryPanelView)
        {
            _view = craftingCategoryPanelView;
            
            //Remove after testing
            _selectionModel = ServiceLocator.Resolve<CraftingSelectionModel>();
            
            foreach (var category in (CraftingCategoryType[]) Enum.GetValues(typeof(CraftingCategoryType)))
            {
                var button = Object.Instantiate(_view.CategoryButtonPrefab, _view.CategoryParent);
                var view = button.GetComponent<CraftingCategorySelectionView>();
                
                view.SetCategoryType(category);
            }
            
            //Remove after testing
            _selectionModel.Selected.Subscribe(value => Debug.Log($"Selected {value}"));
        }
    }
}