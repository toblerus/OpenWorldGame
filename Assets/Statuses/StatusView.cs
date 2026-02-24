using Injection;
using UnityEngine;
using UnityEngine.UI;

namespace Statuses
{
    public class StatusView : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        public Slider HealthSlider => _healthSlider;
    }
}
