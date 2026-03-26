using ReactiveCore.Runtime;

namespace Utility
{
    public abstract class Selectable<T>
    {
        public ReactiveValue<T> Selected { get; }

        public T Current => Selected.Value;

        protected Selectable(T initialValue = default)
        {
            Selected = new ReactiveValue<T>(initialValue);
        }

        public virtual void Select(T selectable)
        {
            Selected.Value = selectable;
        }
    }
}