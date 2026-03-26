using _Scripts.Injection;
using ReactiveCore.Runtime;
using UnityEngine;

namespace _Scripts.Crafting
{
    public class CraftingCategorySelectionView : MonoBehaviour
    {
        [SerializeField] private ReactiveButton _selectButton;
        public ReactiveButton SelectButton => _selectButton;
        
        private CraftingCategoryType _categoryType;
        public CraftingCategoryType CategoryType => _categoryType;
        
        private void Start()
        {
            var controller = ServiceLocator.Resolve<CraftingCategorySelectionController>();
            controller.Setup(this);
        }

        public void SetCategoryType(CraftingCategoryType categoryType)
        {
            _categoryType = categoryType;
        }
    }
}
