using Injection;
using Interaction;
using Inventory;
using UnityEngine;

namespace Hud
{
    public class ItemDropController : MonoBehaviour, IInteractable, IController<ItemDropView>
    {
        private ItemDropModel _model;
        private ItemDropView _view;
        
        public void Interact(GameObject interactor)
        {
            var inventory = interactor.GetComponentInChildren<InventoryController>();
            if (inventory != null)
            {
                inventory.AddItem(_model.Item, _model.Amount);
                Destroy(gameObject);
            }
        }

        public void Setup(ItemDropView view)
        {
            _model = ServiceLocator.Resolve<ItemDropModel>();
        }
    }
}