using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Injection;
using _Scripts.Inventory;
using _Scripts.Inventory.ItemConfigs;
using ReactiveCore.Runtime;
using Object = UnityEngine.Object;

namespace _Scripts.Crafting
{
    public class CraftingPanelController
    {
        private CraftingPanelView _view;
        private CraftingCategorySelectionModel _categorySelectionModel;
        private CraftingGameItemSelectionModel _gameItemSelectioNModel;
        private GameItemConfigModel _gameItemConfigModel;
        private List<CraftingIngredientView> _ingredientViewPool;

        public void Setup(CraftingPanelView craftingPanelView)
        {
            _view = craftingPanelView;
            _categorySelectionModel = ServiceLocator.Resolve<CraftingCategorySelectionModel>();
            _gameItemSelectioNModel = ServiceLocator.Resolve<CraftingGameItemSelectionModel>();
            _gameItemConfigModel = ServiceLocator.Resolve<GameItemConfigModel>();

            
            foreach (var category in (CraftingCategoryType[]) Enum.GetValues(typeof(CraftingCategoryType)))
            {
                var button = Object.Instantiate(_view.CategoryButtonPrefab, _view.CategoryParent);
                var view = button.GetComponent<CraftingCategoryItemView>();
                view.SetCategoryType(category);
                button.gameObject.SetActive(category != CraftingCategoryType.None);
            }
            
            foreach (var gameItemType in (GameItemType[]) Enum.GetValues(typeof(GameItemType)))
            {
                var button = Object.Instantiate(_view.GameItemButtonPrefab, _view.GameItemParent);
                var view = button.GetComponent<CraftingGameItemView>();
                view.SetGameItemType(gameItemType);
                button.gameObject.SetActive(gameItemType != GameItemType.None);
            }

            _categorySelectionModel.Selected.Combine(_gameItemSelectioNModel.Selected).Subscribe(UpdateCraftingPanel);
            
            _categorySelectionModel.Select(CraftingCategoryType.None);
        }

        private void UpdateCraftingPanel((CraftingCategoryType category, GameItemType item) selectedItem)
        {
            _view.ListHeaderText = selectedItem.category.ToString();
            _view.ItemName = selectedItem.item.ToString();
            _view.ItemDescription = _gameItemConfigModel.GetItemDescription(selectedItem.item);

            _view.ListPanel.gameObject.SetActive(selectedItem.category != CraftingCategoryType.None);
            _view.CraftingPanel.gameObject.SetActive(selectedItem.item != GameItemType.None);

            var ingredients = _gameItemConfigModel.GetItemIngredients(selectedItem.item);
            ClearIngredientViews();
            foreach (var ingredient in ingredients)
            {
                SetupIngredientView(ingredient);
            }
        }
        
        private void ClearIngredientViews()
        {
            foreach (var ingredientView in _ingredientViewPool)
            {
                ingredientView.GameItemType = GameItemType.None;
                ingredientView.gameObject.SetActive(false);
                ingredientView.ItemName = string.Empty;
                ingredientView.Amount = string.Empty;
            }
        }

        private void SetupIngredientView(CraftingIngredient ingredient)
        {
            var ingredientView = GetEmptyIngredientView();
            ingredientView.GameItemType = ingredient.Type;
            ingredientView.gameObject.SetActive(true);
            ingredientView.ItemName = ingredient.Type.ToString();
            ingredientView.Amount = ingredient.Amount.ToString();
        }
        
        private CraftingIngredientView GetEmptyIngredientView()
        {
            var view = _ingredientViewPool.FirstOrDefault(view => view.GameItemType ==  GameItemType.None);
            
            if (view != null) return view;
            
            var instance = Object.Instantiate(_view.IngredientPrefab, _view.IngredientParent);
            var ingredientView = instance.GetComponent<CraftingIngredientView>();
            _ingredientViewPool.Add(ingredientView);
            return ingredientView;
        }
    }
}