using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Inventory.ItemConfigs
{
    public class GameItemConfigModel
    {
        private GameItemConfigView _view;

        public void Setup(GameItemConfigView gameItemConfigView)
        {
            _view = gameItemConfigView;
        }

        public GameItemConfig GetConfig(GameItemType gameItemType)
        {
            return _view.GameItemCollectionConfig.GetItemOfType(gameItemType);
        }

        public List<GameItemConfig> GetAllConfigs()
        {
            return _view.GameItemCollectionConfig.GetAllItems();
        }

        public string GetItemDescription(GameItemType gameItemType)
        {
            return _view.GameItemCollectionConfig.GetItemOfType(gameItemType)?.Description;
        }

        public List<CraftingIngredient> GetItemIngredients(GameItemType gameItemType)
        {
            return _view.GameItemCollectionConfig.GetItemOfType(gameItemType)?.Ingredients;
        }

        public Sprite GetItemSprite(GameItemType gameItemType)
        {
            return _view.GameItemCollectionConfig.GetItemOfType(gameItemType)?.Icon;
        }
    }
}