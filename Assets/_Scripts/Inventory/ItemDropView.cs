using _Scripts.Interaction;
using UnityEngine;

namespace _Scripts.Inventory
{
    public class ItemDropView : MonoBehaviour, IInteractable
    {
        private ItemDropController _controller;

        public void Setup(ItemDropModel model)
        {
            _controller = new ItemDropController();
            _controller.Setup(this, model);
        }

        public void Interact(GameObject interactor)
        {
            _controller.Interact(interactor);
        }
    }
}