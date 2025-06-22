using System;
using System.Collections.Generic;
using UnityEngine;

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

        private IDisposable Subscribe(Action<T> onValueChanged, bool skipCurrentValue)
        {
            var sub = new Subscription(onValueChanged, this);
            _subscriptions.Add(sub);
            if (!skipCurrentValue)
                onValueChanged(_value);
            return sub;
        }

        public IDisposable Subscribe(Action<T> onValueChanged)
        {
            return Subscribe(onValueChanged, false);
        }

        public IDisposable SkipValueOnSubscribe(Action<T> onValueChanged)
        {
            return Subscribe(onValueChanged, true);
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    NotifySubscribers();
                }
            }
        }

        private void NotifySubscribers()
        {
            foreach (var sub in _subscriptions.ToArray())
            {
                sub.Callback?.Invoke(_value);
            }
        }

        private void Unsubscribe(Subscription sub)
        {
            _subscriptions.Remove(sub);
        }

        private class Subscription : IDisposable
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
                _parent.Unsubscribe(this);
                Callback = null;
                _parent = null;
            }
        }
    }
}