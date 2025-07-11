using Injection;
using UnityEngine;

namespace Hud
{
    public class InjectableView<TController, TView> : MonoBehaviour
        where TController : class, IController<TView>
        where TView : MonoBehaviour
    {
        private TController _controller;

        protected virtual void Start()
        {
            _controller = ServiceLocator.Resolve<TController>();
            _controller.Setup(this as TView);
        }
    }
}