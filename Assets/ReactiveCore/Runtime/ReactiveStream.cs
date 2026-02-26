using System;

namespace ReactiveCore.Runtime
{
    public class ReactiveStream<T>
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

        public ReactiveStream<T> Where(Func<T, bool> predicate)
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

        public ReactiveStream<TResult> Map<TResult>(Func<T, TResult> selector)
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