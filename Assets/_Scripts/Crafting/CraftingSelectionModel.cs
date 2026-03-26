using _Scripts.Utility;

namespace _Scripts.Crafting
{
    public class CraftingSelectionModel : Selectable<CraftingCategoryType>
    {
        public CraftingSelectionModel()
        {
        }

        public CraftingSelectionModel(CraftingCategoryType initialCategory) : base(initialCategory)
        {
        }
    }
}