using ReactiveCore;
using Saving;

namespace Statuses
{
    public class NutritionModel
    {
        public ReactiveValue<int> Hunger { get; set; } = new(100);
        public ReactiveValue<int> Thirst { get; set; } = new(100);

        public NutritionModel()
        {
            if (ES3.KeyExists(SavegameConstants.HungerStatus))
            {
                Hunger.Value = ES3.Load<int>(SavegameConstants.HungerStatus);
            }
            
            if (ES3.KeyExists(SavegameConstants.ThirstStatus))
            {
                Hunger.Value = ES3.Load<int>(SavegameConstants.ThirstStatus);
            }
            
            Hunger.Subscribe(value => ES3.Save(SavegameConstants.HungerStatus, value));
            Thirst.Subscribe(value => ES3.Save(SavegameConstants.ThirstStatus, value));
        }
    }
}