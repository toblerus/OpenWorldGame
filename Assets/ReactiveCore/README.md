# ReactiveCore

ReactiveCore is a lightweight reactive state system for Unity.

It provides two core primitives:

- ReactiveValue<T> for state changes
- ReactiveEmitter for event-style emissions

The system is dependency-free, simple, and designed for predictable behavior.

---

## Features

- Type-safe reactive values (ReactiveValue<T>)
- Pairwise subscriptions (previous, current)
- .Where filtering for pairwise streams
- Event-style emitters (ReactiveEmitter)
- Subscription disposal via IDisposable
- Lightweight and allocation-conscious

---

## Installation

Download the latest ReactiveCore-vx.xx.x.unitypackage from:

https://github.com/toblerus/ReactiveCore/releases

Import it into your Unity project.

---

# ReactiveValue<T>

ReactiveValue<T> represents mutable state that notifies subscribers whenever the value is assigned.

Important:
ReactiveValue always emits when assigned. It does not check equality. If you want equality filtering, use Pairwise + Where.

---

## Basic Subscription

```csharp
using System;
using ReactiveCore.Runtime;
using UnityEngine;

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

Console output:

Health is now: 100  
Health is now: 75

---

## SkipValueOnSubscribe

Skips the immediate initial emission.

```csharp
subscription = health.SkipValueOnSubscribe(OnHealthChanged);
health.Value = 50;
```

---

## Pairwise + Where

Access both previous and current value and decide yourself whether to react.

```csharp
subscription = health
    .Pairwise()
    .Where((previous, current) => previous != current)
    .Subscribe((previous, current) =>
    {
        Debug.Log($"Health changed: {previous} -> {current}");
    });
```

Console output:

Health changed: x -> y

---

# ReactiveEmitter

ReactiveEmitter represents an event without a payload.

It does not store state. It only emits once on change.

---

## Subscribe

Subscribe will immediately invoke the callback once, then on every Emit.

```csharp
private ReactiveEmitter itemAdded = new ReactiveEmitter();
private IDisposable subscription;

void Start()
{
    subscription = itemAdded.Subscribe(OnItemAdded);
}

void OnItemAdded()
{
    Debug.Log("Item added!");
}
```

---

## SkipValueOnSubscribe

Subscribe without the initial immediate call.

```csharp
subscription = itemAdded.SkipValueOnSubscribe(OnItemAdded);
```

---

## Emit

```csharp
itemAdded.Emit();
```

All current subscribers are invoked.

---

## Merge (any-of behavior)

Combine multiple emitters so that any of them triggers the merged emitter.

```csharp
var merged = ReactiveEmitter.Merge(a, b, c, d, e);

subscription = merged.SkipValueOnSubscribe(OnAnyTriggered);
```

Whenever a, b, c, d, or e emits, the merged emitter emits.

---