namespace Hud
{
    public interface IController<TView>
    {
        void Setup(TView view);
    }
}