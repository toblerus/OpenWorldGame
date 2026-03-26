using System;

namespace _Scripts.Inventory
{
    [Serializable]
    public class SlotData
    {
        public int Index;
        public GameItemConfig _itemConfig;
        public int Amount;
    }
}