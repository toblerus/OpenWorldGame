using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceInstaller : MonoBehaviour
{
    void Awake()
    {
        ServiceLocator.BindSingleton<HotBarController>();
    }
}
