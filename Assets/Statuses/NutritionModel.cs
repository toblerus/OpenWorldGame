using ReactiveCore;
using ReactiveCore.Runtime;
using Saving;

namespace Statuses
{
    public class NutritionModel
    {
        public ReactiveValue<float> Hunger { get; set; } = new(100f);
        public ReactiveValue<float> Thirst { get; set; } = new(100f);

        private readonly ReactiveTimer _hungerDecline = new(0.1f);
        private readonly ReactiveTimer _thirstDecline = new(0.05f);

        public NutritionModel()
        {
            if (ES3.KeyExists(SavegameConstants.HungerStatus))
            {
                Hunger.Value = ES3.Load<float>(SavegameConstants.HungerStatus);
            }
            
            if (ES3.KeyExists(SavegameConstants.ThirstStatus))
            {
                Thirst.Value = ES3.Load<float>(SavegameConstants.ThirstStatus);
            }
            
            Hunger.Subscribe(value => ES3.Save(SavegameConstants.HungerStatus, value));
            Thirst.Subscribe(value => ES3.Save(SavegameConstants.ThirstStatus, value));
            
            _hungerDecline.Elapsed.Subscribe(() => Hunger.Value -= 0.01f);
            _thirstDecline.Elapsed.Subscribe(() => Thirst.Value -= 0.01f);
            
            _hungerDecline.Start();
            _thirstDecline.Start();
        }
    }
}