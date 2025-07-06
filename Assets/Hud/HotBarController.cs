namespace Hud
{
    public class HotBarController
    {
        private HotBarView _hotBarView;

        public void Setup(HotBarView hotBarView)
        {
            _hotBarView = hotBarView;
            SetupSlots();
        }

        public void SetupSlots()
        {
            foreach (var slot in _hotBarView.InventorySlotViews)
            {
                slot.Clear();
            }
        }
    }
}
