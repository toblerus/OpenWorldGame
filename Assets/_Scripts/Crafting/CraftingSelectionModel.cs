using _Scripts.Utility;

namespace _Scripts.Crafting
{
    public class CraftingSelectionModel : Selectable<CraftingCategoryType>
    {
        public CraftingSelectionModel(CraftingCategoryType initialCategory = default) : base(initialCategory)
        {
        }
    }
}