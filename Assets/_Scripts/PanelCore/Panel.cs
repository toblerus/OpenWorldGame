using UnityEngine;

namespace _Scripts.PanelCore
{
    public abstract class Panel : MonoBehaviour
    {
        public virtual void OnOpen() { }
        public virtual void OnClose() { }
    }
}