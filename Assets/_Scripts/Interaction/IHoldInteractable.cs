namespace _Scripts.Interaction
{
    public interface IHoldInteractable
    {
        void Interact();
        
        void Progress(float progress);
        float InteractionDuration { get; }
    }
}