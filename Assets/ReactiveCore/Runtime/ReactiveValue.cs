using System;
using System.Collections.Generic;

namespace ReactiveCore
{
    public class ReactiveValue<T>
    {
        private T _value;
        private List<Subscription> _subscriptions = new List<Subscription>();

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

        public PairwiseStream<T> Pairwise()
        {
            return new PairwiseStream<T>(this);
        }

        private void NotifySubscribers()
        {
            var snapshot = _subscriptions.ToArray();
            foreach (var subscription in snapshot)
                subscription.Callback?.Invoke(_value);
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
                if (_parent == null) return;
                _parent.Unsubscribe(this);
                Callback = null;
                _parent = null;
            }
        }
    }

    public sealed class PairwiseStream<T>
    {
        private readonly Func<Action<T, T>, IDisposable> _subscribePairwise;

        public PairwiseStream(ReactiveValue<T> source)
        {
            _subscribePairwise = onPair =>
            {
                var previous = source.Value;

                return source.Subscribe(current =>
                {
                    onPair(previous, current);
                    previous = current;
                }, true);
            };
        }

        private PairwiseStream(Func<Action<T, T>, IDisposable> subscribePairwise)
        {
            _subscribePairwise = subscribePairwise;
        }

        public PairwiseStream<T> Where(Func<T, T, bool> predicate)
        {
            return new PairwiseStream<T>(onPair =>
            {
                return _subscribePairwise((previous, current) =>
                {
                    if (predicate(previous, current))
                        onPair(previous, current);
                });
            });
        }

        public IDisposable Subscribe(Action<T, T> onPair)
        {
            return _subscribePairwise(onPair);
        }
    }
}