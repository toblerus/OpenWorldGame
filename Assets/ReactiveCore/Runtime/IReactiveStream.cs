using System;

namespace ReactiveCore.Runtime
{
    public interface IReactiveStream<T>
    {
        IDisposable Subscribe(Action<T> onNext);
        IReactiveStream<T> Where(Func<T, bool> predicate);
        IReactiveStream<TResult> Map<TResult>(Func<T, TResult> selector);
    }
}