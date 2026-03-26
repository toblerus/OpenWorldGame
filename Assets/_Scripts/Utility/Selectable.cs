using ReactiveCore.Runtime;

namespace _Scripts.Utility
{
    public abstract class Selectable<T>
    {
        public ReactiveValue<T> Selected { get; }

        public T Current => Selected.Value;

        protected Selectable()
        {
            Selected = new ReactiveValue<T>();
        }

        protected Selectable(T initialValue)
        {
            Selected = new ReactiveValue<T>(initialValue);
        }

        public virtual void Select(T selectable)
        {
            Selected.Value = selectable;
        }
    }
}