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
        private ItemDropSpawnerModel _itemDropSpawnerModel;
        private bool _harvested;

        public void Setup(InWorldObjectInteractionView inWorldObjectInteractionView, GameItemType itemType)
        {
            _view = inWorldObjectInteractionView;
            _inventoryModel = ServiceLocator.Resolve<InventoryModel>();
            _itemDropSpawnerModel = ServiceLocator.Resolve<ItemDropSpawnerModel>();
            
            var gameItemConfigModel = ServiceLocator.Resolve<GameItemConfigModel>();
            
            var config = gameItemConfigModel.GetConfig(itemType);
            if(config == null) return;
            
            _model = ServiceLocator.Resolve<InWorldObjectInteractionModel>();
            _model.Setup(config, Random.Range(1, Mathf.Max(1, config.RandomDropRate) + 1));
        }

        public bool Interact()
        {
            if (_harvested || _model == null) return false;
            _harvested = true;

            var remaining = _inventoryModel.AddItem(_model.ItemConfig, _model.Amount);
            if (remaining > 0)
                _itemDropSpawnerModel.Spawn(_model.ItemConfig, remaining, _view.transform.position + Vector3.up);

            return true;
        }
    }
}
