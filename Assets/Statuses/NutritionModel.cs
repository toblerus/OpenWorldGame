using Injection;
using ReactiveCore.Runtime;
using Saving;

namespace Statuses
{
    public class NutritionModel
    {
        private const float HungerDeclineSpeed = 0.005f;
        private const float ThirstDeclineSpeed = 0.01f;
        private const float HealthDeclineSpeed = 0.2f;
        public ReactiveValue<float> Hunger { get; } = new(100f);
        public ReactiveValue<float> Thirst { get; } = new(100f);

        private readonly ReactiveTimer _nutritionDecline = new(0.1f);
        private readonly ReactiveTimer _healthDecline = new(0.1f);

        private ReactiveValue<bool> IsDyingOfHunger { get; } = new();
        private ReactiveValue<bool> IsDyingOfThirst { get; } = new();

        public NutritionModel()
        {
            var healthModel = ServiceLocator.Resolve<HealthModel>();
            
            if (ES3.KeyExists(SavegameConstants.HungerStatus))
            {
                Hunger.Value = ES3.Load<float>(SavegameConstants.HungerStatus);
            }
            
            if (ES3.KeyExists(SavegameConstants.ThirstStatus))
            {
                Thirst.Value = ES3.Load<float>(SavegameConstants.ThirstStatus);
            }
            
            Hunger.Subscribe(value =>
            {
                ES3.Save(SavegameConstants.HungerStatus, value);
                IsDyingOfHunger.Value = value <= 0;
            });
            Thirst.Subscribe(value =>
            {
                ES3.Save(SavegameConstants.ThirstStatus, value);
                IsDyingOfThirst.Value = value <= 0;
            });

            _nutritionDecline.Elapsed.SkipValueOnSubscribe(() =>
            {
                if (Hunger.Value > 0) Hunger.Value -= HungerDeclineSpeed;
                if (Thirst.Value > 0) Thirst.Value -= ThirstDeclineSpeed;
            });
            
            _healthDecline.Elapsed.SkipValueOnSubscribe(() =>
            {
                if (healthModel.Health.Value > 0) healthModel.Health.Value -= HealthDeclineSpeed;
            });
            
            IsDyingOfHunger.CombineUsingOr(IsDyingOfThirst)
                .Subscribe(value =>
                {
                    if(value)
                        _healthDecline.Start();
                    else
                        _healthDecline.Stop();
                });
            
            _nutritionDecline.Start();
        }
    }
}