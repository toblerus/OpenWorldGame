using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, Func<object>> factories = new();
    private static readonly Dictionary<Type, object> singletons = new();

    public static void BindSingleton<T>(Func<T> factory = null) where T : class
    {
        factories[typeof(T)] = () =>
        {
            if (!singletons.TryGetValue(typeof(T), out var instance))
            {
                instance = factory != null ? factory() : Activator.CreateInstance<T>();
                singletons[typeof(T)] = instance;
            }
            return instance;
        };
    }

    public static void BindTransient<T>(Func<T> factory = null) where T : class
    {
        factories[typeof(T)] = () =>
        {
            return factory != null ? factory() : Activator.CreateInstance<T>();
        };
    }

    public static T Resolve<T>() where T : class
    {
        if (factories.TryGetValue(typeof(T), out var factory))
        {
            return (T)factory();
        }

        throw new Exception($"Type {typeof(T).Name} not bound in ServiceLocator.");
    }

    public static bool IsBound<T>() where T : class
    {
        return factories.ContainsKey(typeof(T));
    }

    public static void Unbind<T>() where T : class
    {
        factories.Remove(typeof(T));
        singletons.Remove(typeof(T));
    }
}
