using System.Collections.Generic;
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

        public GameItemConfig GetConfig(GameItemType gameItemType)
        {
            return _view.GameItemCollectionConfig.GetItemOfType(gameItemType);
        }

        public List<GameItemConfig> GetAllConfigs()
        {
            return _view.GameItemCollectionConfig.GetAllItems();
        }
    }
}