using System;
using UnityEngine;
using ReactiveCore;


public class Example : MonoBehaviour
{
    private ReactiveValue<int> health = new(100);
    private ReactiveValue<float> test = new(100);
    private ReactiveValue<bool> test2 = new(true);
    private IDisposable healthSubscription;
    private IDisposable floatSubscription;
    private IDisposable boolSubscription;

    void Start()
    {
        healthSubscription = health.Subscribe(OnHealthChanged);
        floatSubscription = test.Subscribe(value => OnFloatChanged(value));
        boolSubscription = test2.SkipValueOnSubscribe(OnBoolChanged);
        
        health.Value = 80;
        test.Value = 10;
        test2.Value = false;
        test2.Value = true;
    }

    void OnDestroy()
    {
        healthSubscription.Dispose();
        floatSubscription.Dispose();
        boolSubscription.Dispose();
    }

    void OnHealthChanged(int value)
    {
        Debug.Log("Health changed to: " + value);
    }
    
    void OnFloatChanged(float value)
    {
        Debug.Log("Float changed to: " + value);
    }
    
    void OnBoolChanged(bool value)
    {
        Debug.Log("Bool changed to: " + value);
    }
}