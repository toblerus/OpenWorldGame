using _Scripts.Injection;
using _Scripts.Inventory;
using _Scripts.Inventory.ItemConfigs;
using UnityEngine;

namespace _Scripts.InWorld.InWorldInteraction
{
    public class InWorldObjectInteractionController
    {
        private InWorldObjectInteractionModel _model;
        private InWorldObjectInteractionView _view;
        private InventoryModel _inventoryModel;
        private GameItemConfig _config;

        public void Setup(InWorldObjectInteractionView inWorldObjectInteractionView, GameItemType itemType)
        {
            _view = inWorldObjectInteractionView;
            _inventoryModel = ServiceLocator.Resolve<InventoryModel>();
            
            var gameItemConfigController = ServiceLocator.Resolve<GameItemConfigModel>();
            
            _config = gameItemConfigController.GetConfig(itemType);
            if(_config == null) return;
            
            _model = new InWorldObjectInteractionModel(_config, Random.Range(1, 5));
        }

        public void Interact()
        {
            _inventoryModel.AddItem(_model.ItemConfig, _model.Amount);
        }
    }
}