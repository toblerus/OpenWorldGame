using _Scripts.Injection;
using UnityEngine;

namespace _Scripts.Crafting
{
    public class CraftingCategoryPanelView : MonoBehaviour
    {
        [SerializeField] private Transform _categoryParent;
        [SerializeField] private GameObject _categoryButtonPrefab;

        public Transform CategoryParent => _categoryParent;
        public GameObject CategoryButtonPrefab => _categoryButtonPrefab;

        private void Start()
        {
            var controller = ServiceLocator.Resolve<CraftingCategoryPanelController>();
            controller.Setup(this);
        }
    }
}