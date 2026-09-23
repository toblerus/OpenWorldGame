using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Inventory
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
            if (_inventoryModel != null && _model.Amount > 0)
            {
                _model.Amount = _inventoryModel.AddItem(_model.ItemConfig, _model.Amount);
                if (_model.Amount == 0)
                {
                    _view.gameObject.SetActive(false);
                    Object.Destroy(_view.gameObject);
                }
            }
            else if (_inventoryModel == null)
            {
                Debug.Log($"No controller found in {interactor.name}");
            }
        }
    }
}
