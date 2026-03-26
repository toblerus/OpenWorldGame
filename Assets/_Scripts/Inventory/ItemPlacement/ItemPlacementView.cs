using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Inventory.ItemPlacement
{
    public class ItemPlacementView : MonoBehaviour
    {
        [SerializeField] private Transform _placementItemPreviewParent;
        [SerializeField] private Transform _placementParent;
        public Transform PlacementItemPreviewParent => _placementItemPreviewParent;
        public Transform PlacementParent => _placementParent;
        private void Start()
        {
            var controller = ServiceLocator.Resolve<ItemPlacementController>();
            controller.Setup(this);
        }
    }
}