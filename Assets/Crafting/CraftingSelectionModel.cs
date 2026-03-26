using Utility;

namespace Crafting
{
    public class CraftingSelectionModel : Selectable<CraftingCategoryType>
    {
        public CraftingSelectionModel(CraftingCategoryType initialCategory = default) : base(initialCategory)
        {
        }
    }
}