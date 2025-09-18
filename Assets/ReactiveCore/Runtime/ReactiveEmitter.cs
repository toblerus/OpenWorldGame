using System;
using System.Collections.Generic;

namespace ReactiveCore.Runtime
{
    public class ReactiveEmitter
    {
        private readonly List<Action> _subscribers = new();

        public IDisposable SkipValueOnSubscribe(Action callback)
        {
            _subscribers.Add(callback);
            return new Subscription(this, callback);
        }

        public IDisposable Subscribe(Action callback)
        {
            var sub = SkipValueOnSubscribe(callback);
            callback?.Invoke();
            return sub;
        }

        public void Emit()
        {
            foreach (var callback in _subscribers.ToArray())
            {
                callback?.Invoke();
            }
        }

        private void Unsubscribe(Action callback)
        {
            _subscribers.Remove(callback);
        }

        private class Subscription : IDisposable
        {
            private ReactiveEmitter _owner;
            private Action _callback;

            public Subscription(ReactiveEmitter owner, Action callback)
            {
                _owner = owner;
                _callback = callback;
            }

            public void Dispose()
            {
                _owner?.Unsubscribe(_callback);
                _owner = null;
                _callback = null;
            }
        }
    }
}