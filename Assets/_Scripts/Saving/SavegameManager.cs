using UnityEngine;

namespace _Scripts.Saving
{
    public class SavegameManager : MonoBehaviour
    {
        public void Awake()
        {
            ES3.Init();
        }
    }
}