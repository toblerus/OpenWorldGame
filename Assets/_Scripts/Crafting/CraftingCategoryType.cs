using System.ComponentModel;

namespace _Scripts.Crafting
{
    public enum CraftingCategoryType
    {
        [Description("None")]
        None = 0,
        Food = 1,
        Other = 2,
        Tools = 3,
        Weapons = 4,
        Equipment = 5,
        Resources = 6,
        Navigation = 7,
        Decorations = 8
    }
}
