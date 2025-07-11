namespace Inventory
{
    public class ItemDropModel
    {
        public GameItem Item { get; }
        public int Amount { get; }

        public ItemDropModel(GameItem item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }
}