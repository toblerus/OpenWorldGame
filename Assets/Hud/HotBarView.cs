using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBarView : MonoBehaviour
{
    private void Start()
    {
        var hotBarController = ServiceLocator.Resolve<HotBarController>();
        hotBarController.Setup();
    }
}
