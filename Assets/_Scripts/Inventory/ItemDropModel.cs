namespace _Scripts.Inventory
{
    public class ItemDropModel
    {
        public GameItemConfig ItemConfig { get; set; }
        public int Amount { get; set; }

        public ItemDropModel(GameItemConfig itemConfig, int amount)
        {
            ItemConfig = itemConfig;
            Amount = amount;
        }
    }
}