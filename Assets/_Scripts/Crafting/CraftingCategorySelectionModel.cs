using _Scripts.Utility;

namespace _Scripts.Crafting
{
    public class CraftingCategorySelectionModel : Selectable<CraftingCategoryType>
    {
        public CraftingCategorySelectionModel()
        {
        }

        public CraftingCategorySelectionModel(CraftingCategoryType initialCategory) : base(initialCategory)
        {
        }
    }
}