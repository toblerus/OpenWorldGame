using Injection;
using UnityEngine;

namespace Inventory.ItemConfigs
{
    public class GameItemConfigView : MonoBehaviour
    {
        [SerializeField] private GameItemCollectionConfig _gameItemCollectionConfig;
        public GameItemCollectionConfig GameItemCollectionConfig  => _gameItemCollectionConfig;
        
        private void Start()
        {
            var controller = ServiceLocator.Resolve<GameItemConfigController>();
            controller.Setup(this);
        }
    }
}