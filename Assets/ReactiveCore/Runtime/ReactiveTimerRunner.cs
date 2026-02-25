using System.Collections.Generic;
using UnityEngine;

namespace ReactiveCore.Runtime
{
    internal class ReactiveTimerRunner : MonoBehaviour
    {
        private static ReactiveTimerRunner _instance;
        private static readonly List<ReactiveTimer> _timers = new();

        internal static void Register(ReactiveTimer timer)
        {
            EnsureInstance();
            if (!_timers.Contains(timer))
                _timers.Add(timer);
        }

        internal static void Unregister(ReactiveTimer timer)
        {
            _timers.Remove(timer);
        }

        private static void EnsureInstance()
        {
            if (_instance != null)
                return;

            var go = new GameObject("[ReactiveTimerRunner]");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<ReactiveTimerRunner>();
        }

        void Update()
        {
            var dt = Time.deltaTime;

            for (var i = 0; i < _timers.Count; i++)
            {
                _timers[i].Tick(dt);
            }
        }
    }
}