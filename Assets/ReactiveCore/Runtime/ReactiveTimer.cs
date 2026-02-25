using UnityEngine;

namespace ReactiveCore.Runtime
{
    public class ReactiveTimer
    {
        public ReactiveEmitter Elapsed { get; } = new ReactiveEmitter();

        private float _interval;
        private float _accumulator;
        private bool _isRunning;

        public ReactiveTimer(float intervalSeconds)
        {
            _interval = intervalSeconds;
        }

        public float IntervalSeconds
        {
            get => _interval;
            set => _interval = value;
        }

        public bool IsRunning => _isRunning;

        public void Start()
        {
            if (_isRunning)
                return;

            _isRunning = true;
            _accumulator = 0f;
            ReactiveTimerRunner.Register(this);
        }

        public void Stop()
        {
            if (!_isRunning)
                return;

            _isRunning = false;
            ReactiveTimerRunner.Unregister(this);
        }

        internal void Tick(float deltaTime)
        {
            _accumulator += deltaTime;

            while (_accumulator >= _interval)
            {
                _accumulator -= _interval;
                Elapsed.Emit();
            }
        }
    }
}