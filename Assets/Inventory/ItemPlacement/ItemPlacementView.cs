using Injection;
using Inventory.HandItem;
using UnityEngine;

namespace Inventory.ItemPlacement
{
    public class ItemPlacementView : MonoBehaviour
    {
        [SerializeField] private Transform _placementItemParent;
        public Transform PlacementItemParent => _placementItemParent;
        private void Start()
        {
            var controller = ServiceLocator.Resolve<ItemPlacementController>();
            controller.Setup(this);
        }
    }
}