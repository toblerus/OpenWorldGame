using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Inventory.HandItem
{
    public class HandItemView : MonoBehaviour
    {
        [SerializeField] private Transform _itemParent;
        
        public Transform ItemParent => _itemParent;

        private void Start()
        {
            var controller = ServiceLocator.Resolve<HandItemController>();
            controller.Setup(this);
        }
    }
}
