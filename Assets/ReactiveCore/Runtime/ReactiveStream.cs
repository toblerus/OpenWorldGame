using System;

namespace ReactiveCore.Runtime
{
    internal class ReactiveStream<T> : IReactiveStream<T>
    {
        private readonly Func<Action<T>, IDisposable> _subscribe;

        public ReactiveStream(Func<Action<T>, IDisposable> subscribe)
        {
            _subscribe = subscribe;
        }

        public IDisposable Subscribe(Action<T> onNext)
        {
            return _subscribe(onNext);
        }

        public IReactiveStream<T> Where(Func<T, bool> predicate)
        {
            return new ReactiveStream<T>(observer =>
            {
                return _subscribe(value =>
                {
                    if (predicate(value))
                        observer(value);
                });
            });
        }

        public IReactiveStream<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            return new ReactiveStream<TResult>(observer =>
            {
                return _subscribe(value =>
                {
                    var projected = selector(value);
                    observer(projected);
                });
            });
        }
    }
}