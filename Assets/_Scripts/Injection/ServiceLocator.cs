using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Injection
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<(Type Type, object Key), Func<object>> factories = new();
        private static readonly Dictionary<(Type Type, object Key), object> singletons = new();

        public static void BindView<TView>(TView prefab, object key = null) where TView : MonoBehaviour
        {
            var factory = new ViewFactory<TView>(prefab);
            BindSingleton(() => factory, key);
        }

        public static void BindSingleton<T>(Func<T> factory = null, object key = null) where T : class
        {
            BindSingletonInternal(factory, false, key);
        }

        public static void BindSingletonNonLazy<T>(Func<T> factory = null, object key = null) where T : class
        {
            BindSingletonInternal(factory, true, key);
        }

        private static void BindSingletonInternal<T>(Func<T> factory, bool nonLazy, object key) where T : class
        {
            var registration = (typeof(T), key);
            singletons.Remove(registration);
            factories[registration] = () =>
            {
                if (!singletons.TryGetValue(registration, out var instance))
                {
                    instance = factory != null ? factory() : Activator.CreateInstance<T>();
                    singletons[registration] = instance;
                }

                return instance;
            };

            if (nonLazy)
                Resolve<T>(key);
        }

        public static void BindTransient<T>(Func<T> factory = null, object key = null) where T : class
        {
            var registration = (typeof(T), key);
            singletons.Remove(registration);
            factories[registration] = () =>
            {
                return factory != null ? factory() : Activator.CreateInstance<T>();
            };
        }

        public static T Resolve<T>(object key = null) where T : class
        {
            if (factories.TryGetValue((typeof(T), key), out var factory))
                return (T)factory();

            throw new Exception($"Type {typeof(T).Name} with key '{key}' not bound in ServiceLocator.");
        }

        public static bool IsBound<T>(object key = null) where T : class
        {
            return factories.ContainsKey((typeof(T), key));
        }

        public static void Unbind<T>(object key = null) where T : class
        {
            var registration = (typeof(T), key);
            factories.Remove(registration);
            singletons.Remove(registration);
        }
    }
}
