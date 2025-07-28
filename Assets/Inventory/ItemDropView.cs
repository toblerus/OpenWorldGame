using Hud;
using Injection;
using Interaction;
using UnityEngine;

namespace Inventory
{
    public class ItemDropView : MonoBehaviour, IInteractable
    {
        private ItemDropController _controller;

        public void Setup(ItemDropModel model)
        {
            
            _controller = ServiceLocator.Resolve<ItemDropController>();
        }

        public void Interact(GameObject interactor)
        {
            _controller.Interact(interactor);
        }
    }
}