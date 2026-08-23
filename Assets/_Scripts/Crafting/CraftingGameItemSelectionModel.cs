using _Scripts.Inventory;
using _Scripts.Utility;

namespace _Scripts.Crafting
{
    public class CraftingGameItemSelectionModel : Selectable<GameItemType>
    {
        public CraftingGameItemSelectionModel()
        {
        }

        public CraftingGameItemSelectionModel(GameItemType initialGameItem) : base(initialGameItem)
        {
        }
    }
}