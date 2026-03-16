using Inventory;

namespace InWorld.InWorldInteraction
{
    public class InWorldObjectInteractionModel
    {
        public GameItemConfig ItemConfig { get; set; }
        public int Amount { get; set; }

        public InWorldObjectInteractionModel(GameItemConfig itemConfig, int amount)
        {
            ItemConfig = itemConfig;
            Amount = amount;
        }
    }
}