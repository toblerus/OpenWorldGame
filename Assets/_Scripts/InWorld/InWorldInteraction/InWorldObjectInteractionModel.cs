using _Scripts.Inventory;

namespace _Scripts.InWorld.InWorldInteraction
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