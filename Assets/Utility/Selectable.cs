using ReactiveCore.Runtime;

namespace Utility
{
    public class Selectable<T>
    {
        public ReactiveValue<T> Selected { get; }
        
        public T Current => Selected.Value;

        public Selectable(T initialValue = default)
        {
            Selected = new ReactiveValue<T>(initialValue);
        }
        
        public void Select(T selectable)
        {
            Selected.Value = selectable;
        }
    }
}
