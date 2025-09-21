using System;

namespace Inventory
{
    [Serializable]
    public class SlotData
    {
        public int Index;
        public GameItem Item;
        public int Amount;
    }
}