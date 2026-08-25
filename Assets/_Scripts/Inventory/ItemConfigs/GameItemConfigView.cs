using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Inventory.ItemConfigs
{
    public class GameItemConfigView : MonoBehaviour
    {
        [SerializeField] private GameItemCollectionConfig _gameItemCollectionConfig;
        public GameItemCollectionConfig GameItemCollectionConfig  => _gameItemCollectionConfig;
        
        private void Start()
        {
            var controller = ServiceLocator.Resolve<GameItemConfigModel>();
            controller.Setup(this);
        }
    }
}