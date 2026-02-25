using Injection;
using UnityEngine;

namespace Statuses
{
    public class HealthController
    {
        private HealthModel _healthModel;

        public void Setup(StatusView view)
        {
            _healthModel = ServiceLocator.Resolve<HealthModel>();

            _healthModel.Health.Subscribe(value =>
            {
                view.HealthSlider.value = value / 100f;
            });
        }
    }
}