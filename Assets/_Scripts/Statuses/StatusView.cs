using _Scripts.Injection;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Statuses
{
    public class StatusView : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Slider _hungerSlider;
        [SerializeField] private Slider _thirstSlider;
        public Slider HealthSlider => _healthSlider;
        public Slider HungerSlider => _hungerSlider;
        public Slider ThirstSlider => _thirstSlider;

        private void Start()
        {
            var healthController = ServiceLocator.Resolve<HealthController>();
            healthController.Setup(this);
            
            var nutritionController = ServiceLocator.Resolve<NutritionController>();
            nutritionController.Setup(this);
        }
    }
}
