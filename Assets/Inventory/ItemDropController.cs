using Injection;
using UnityEngine;

namespace Inventory
{
    public class ItemDropController
    {
        private ItemDropView _view;
        private ItemDropModel _model;
        private InventoryModel _inventoryModel;
        
        public void Setup(ItemDropView view, ItemDropModel model)
        {
            _view = view;
            _model = model;
            _inventoryModel = ServiceLocator.Resolve<InventoryModel>();
        }

        public void Interact(GameObject interactor)
        {
            if (_inventoryModel != null)
            {
                _inventoryModel.AddItem(_model.Item, _model.Amount);
                Object.Destroy(_view.gameObject);
            }
            else
            {
                Debug.Log($"No controller found in {interactor.name}");
            }
        }
    }
}