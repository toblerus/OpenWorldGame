using _Scripts.PlayerControls;
using UnityEngine;

namespace _Scripts.InWorld.InWorldInteraction
{
    public class InWorldStorageInteractionController
    {
        public void Interact(GameObject interactor)
        {
            if (interactor == null) return;

            var player = interactor.GetComponentInParent<FirstPersonController>();
            if (player != null)
                player.OpenInventory();
        }
    }
}
