using Injection;
using UnityEngine;

namespace Statuses
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private StatusView _view;

        private HealthModel _healthModel;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            _healthModel = ServiceLocator.Resolve<HealthModel>();

            _healthModel.Health.Subscribe(value =>
            {
                _view.HealthSlider.value = value / 100f;
            });
        }
    }
}