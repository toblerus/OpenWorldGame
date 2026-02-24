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
            var snapshot = _subscribers.ToArray();
            for (var i = 0; i < snapshot.Length; i++)
            {
                snapshot[i]?.Invoke();
            }
        }

        public static ReactiveEmitter Merge(params ReactiveEmitter[] emitters)
        {
            var merged = new ReactiveEmitter();

            for (var i = 0; i < emitters.Length; i++)
            {
                var emitter = emitters[i];
                emitter.SkipValueOnSubscribe(() => merged.Emit());
            }

            return merged;
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