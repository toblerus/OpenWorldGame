using System;
using System.Collections.Generic;

namespace ReactiveCore.Runtime
{
    public class ReactiveValue<T>
    {
        private T _value;
        private readonly List<Subscription> _subscriptions = new();

        public ReactiveValue(T initialValue = default)
        {
            _value = initialValue;
        }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifySubscribers();
            }
        }

        public IDisposable Subscribe(Action<T> onValueChanged)
        {
            return Subscribe(onValueChanged, false);
        }

        public IDisposable Subscribe(Action<T> onValueChanged, bool skipCurrentValue)
        {
            var sub = new Subscription(onValueChanged, this);
            _subscriptions.Add(sub);

            if (!skipCurrentValue)
                onValueChanged(_value);

            return sub;
        }

        public IDisposable SkipValueOnSubscribe(Action<T> onValueChanged)
        {
            return Subscribe(onValueChanged, true);
        }

        public ReactiveStream<(T previous, T current)> Pairwise()
        {
            return new ReactiveStream<(T previous, T current)>(observer =>
            {
                var previous = _value;

                return Subscribe(current =>
                {
                    var pair = (previous, current);
                    observer(pair);
                    previous = current;
                }, true);
            });
        }

        private void NotifySubscribers()
        {
            var snapshot = _subscriptions.ToArray();
            for (var i = 0; i < snapshot.Length; i++)
                snapshot[i].Callback?.Invoke(_value);
        }

        private void Unsubscribe(Subscription sub)
        {
            _subscriptions.Remove(sub);
        }

        private sealed class Subscription : IDisposable
        {
            public Action<T> Callback { get; private set; }
            private ReactiveValue<T> _parent;

            public Subscription(Action<T> callback, ReactiveValue<T> parent)
            {
                Callback = callback;
                _parent = parent;
            }

            public void Dispose()
            {
                if (_parent == null)
                    return;

                _parent.Unsubscribe(this);
                Callback = null;
                _parent = null;
            }
        }
    }
}