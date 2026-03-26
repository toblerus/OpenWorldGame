using _Scripts.Injection;

namespace _Scripts.Statuses
{
    public class NutritionController
    {
        private NutritionModel _nutritionModel;

        public void Setup(StatusView view)
        {
            _nutritionModel = ServiceLocator.Resolve<NutritionModel>();

            _nutritionModel.Hunger.Subscribe(value =>
            {
                view.HungerSlider.value = value / 100f;
            });

            _nutritionModel.Thirst.Subscribe(value =>
            {
                view.ThirstSlider.value = value / 100f;
            });
        }
    }
}