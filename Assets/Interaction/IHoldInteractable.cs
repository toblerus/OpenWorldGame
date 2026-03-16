using Inventory;

namespace Interaction
{
    public interface IHoldInteractable
    {
        void Interact();
        
        void Progress(float progress);
        float InteractionDuration { get; }
    }
}