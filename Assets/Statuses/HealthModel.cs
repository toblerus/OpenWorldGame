using ReactiveCore;
using Saving;

namespace Statuses
{
    public class HealthModel
    {
        public ReactiveValue<int> Health { get; set; } = new(100);

        public HealthModel()
        {
            if (ES3.KeyExists(SavegameConstants.HealthStatus))
            {
                Health.Value = ES3.Load<int>(SavegameConstants.HealthStatus);
            }
            
            Health.Subscribe(value => ES3.Save(SavegameConstants.HealthStatus, value));
        }
    }
}