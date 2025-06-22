# ReactiveCore

**ReactiveCore** is a lightweight reactive state system for Unity. It offers a simple `ReactiveValue<T>` class that allows for clean, safe value subscriptions and automatic lifecycle handling.

## Features

- Type-safe reactive values
- Auto-disposal of subscriptions
- Lightweight and dependency-free
- Great for decoupling UI and logic

## Installation

1. Install the latest `ReactiveCore-vx.xx.x.unitypackage` from https://github.com/toblerus/ReactiveCore/releases

## Usage
### Subscribe
```
public class Example : MonoBehaviour
{
    private ReactiveValue<int> health = new ReactiveValue<int>(100);
    private IDisposable subscription;

    void Start()
    {
        subscription = health.Subscribe(OnHealthChanged);
        health.Value = 75;
    }

    void OnDestroy()
    {
        subscription.Dispose();
    }

    void OnHealthChanged(int newHealth)
    {
        Debug.Log("Health is now: " + newHealth);
    }
}
```

#### Console Output:

```
Health is now: 100

Health is now: 75
```
### SkipValueOnSubscribe

```
void Start()
{
    subscription = health.SkipValueOnSubscribe(OnHealthChanged);
    health.Value = 50;
}
```

#### Console Output:

`Health is now: 50`
