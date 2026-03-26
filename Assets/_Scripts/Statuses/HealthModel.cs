using _Scripts.Saving;
using ReactiveCore.Runtime;

namespace _Scripts.Statuses
{
    public class HealthModel
    {
        public ReactiveValue<float> Health { get; set; } = new(100);

        public HealthModel()
        {
            if (ES3.KeyExists(SavegameConstants.HealthStatus))
            {
                Health.Value = ES3.Load<float>(SavegameConstants.HealthStatus);
            }
            
            Health.Subscribe(value => ES3.Save(SavegameConstants.HealthStatus, value));
        }
    }
}