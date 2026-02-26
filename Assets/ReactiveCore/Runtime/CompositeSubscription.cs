using System;
using System.Collections.Generic;

namespace ReactiveCore.Runtime
{
    internal class CompositeSubscription : IDisposable
    {
        private readonly List<IDisposable> _subscriptions;
        private bool _disposed;

        public CompositeSubscription(List<IDisposable> subscriptions)
        {
            _subscriptions = subscriptions;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            for (var i = 0; i < _subscriptions.Count; i++)
            {
                _subscriptions[i]?.Dispose();
            }

            _subscriptions.Clear();
        }
    }
}