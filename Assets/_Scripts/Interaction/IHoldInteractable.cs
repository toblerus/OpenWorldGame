namespace _Scripts.Interaction
{
    public interface IHoldInteractable : IInteractable
    {
        void Progress(float progress);
        float InteractionDuration { get; }
    }
}
