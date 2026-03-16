using System.Linq;

namespace Inventory.ItemConfigs
{
    public class GameItemConfigController
    {
        private GameItemConfigView _view;

        public void Setup(GameItemConfigView gameItemConfigView)
        {
            _view = gameItemConfigView;
        }

        public GameItemConfig RetrieveConfig(GameItemType gameItemType)
        {
            return _view.GameItemCollectionConfig.GetItemOfType(gameItemType);
        }
    }
}