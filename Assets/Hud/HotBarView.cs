using UnityEngine;

namespace Hud
{
    public class HotBarView : MonoBehaviour
    {
        private void Start()
        {
            var hotBarController = ServiceLocator.Resolve<HotBarController>();
            hotBarController.Setup();
        }
    }
}
