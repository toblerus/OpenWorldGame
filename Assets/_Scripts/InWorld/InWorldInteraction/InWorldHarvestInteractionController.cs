using _Scripts.Injection;
using _Scripts.Inventory;
using _Scripts.Inventory.ItemConfigs;
using UnityEngine;

namespace _Scripts.InWorld.InWorldInteraction
{
    public class InWorldHarvestInteractionController
    {
        private InWorldHarvestInteractionModel _model;
        private InWorldHarvestInteractionView _view;
        private InventoryModel _inventoryModel;
        private ItemDropSpawnerModel _itemDropSpawnerModel;
        private bool _harvested;

        public void Setup(InWorldHarvestInteractionView view)
        {
            _view = view;
            _inventoryModel = ServiceLocator.Resolve<InventoryModel>();
            _itemDropSpawnerModel = ServiceLocator.Resolve<ItemDropSpawnerModel>();
            
            var gameItemConfigModel = ServiceLocator.Resolve<GameItemConfigModel>();
            
            var config = gameItemConfigModel.GetConfig(view.ItemType);
            if(config == null) return;
            
            _model = ServiceLocator.Resolve<InWorldHarvestInteractionModel>();
            _model.Setup(config, Random.Range(1, Mathf.Max(1, config.RandomDropRate) + 1));
        }

        public bool TryInteract()
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
