using UnityEngine;

namespace Inventory
{
    public class ItemDropController
    {
        private readonly ItemDropModel _model;
        private readonly ItemDropView _view;

        public ItemDropController(ItemDropView view, ItemDropModel model)
        {
            _model = model;
            _view = view;
            _view.Setup(model);
        }

        public void Interact(GameObject interactor)
        {
            InventoryController controller = interactor.GetComponentInChildren<InventoryController>();
            if (controller != null)
            {
                controller.AddItem(_model.Item, _model.Amount);
                Object.Destroy(_view.gameObject);
            }
            else
            {
                Debug.Log($"No controller found in {interactor.name}");
            }
        }
    }
}