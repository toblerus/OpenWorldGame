using System.Collections.Generic;
using Injection;
using UnityEngine;

namespace Inventory.Hotbar
{
    public class HotBarView : MonoBehaviour
    {
        [SerializeField] private List<InventorySlotView> _inventorySlotViews;
        public List<InventorySlotView> InventorySlotViews => _inventorySlotViews;
        private void Start()
        {
            var controller = ServiceLocator.Resolve<HotBarController>();
            controller.Setup(this);
        }
    }
}
