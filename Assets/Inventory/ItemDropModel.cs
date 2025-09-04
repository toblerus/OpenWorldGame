namespace Inventory
{
    public class ItemDropModel
    {
        public GameItem Item { get; set; }
        public int Amount { get; set; }

        public ItemDropModel(GameItem item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }
}