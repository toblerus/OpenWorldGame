using System;
using UnityEngine;

namespace Saving
{
    public class SavegameManager : MonoBehaviour
    {
        public void Awake()
        {
            ES3.Init();
        }
    }
}