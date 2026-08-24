using _Scripts.Injection;
using TMPro;
using UnityEngine;

namespace _Scripts.Crafting
{
    public class CraftingPanelView : MonoBehaviour
    {
        //Category Panel
        [SerializeField] private Transform _categoryParent;
        [SerializeField] private GameObject _categoryButtonPrefab;
        
        public Transform CategoryParent => _categoryParent;
        public GameObject CategoryButtonPrefab => _categoryButtonPrefab;
        
        //Item List Panel
        [SerializeField] private GameObject _listPanel;
        [SerializeField] private TextMeshProUGUI _listHeaderText;
        [SerializeField] private Transform _gameItemParent;
        [SerializeField] private GameObject _gameItemButtonPrefab;
        
        public GameObject ListPanel => _listPanel;
        public TextMeshProUGUI ListHeaderText => _listHeaderText;
        public Transform GameItemParent => _gameItemParent;
        public GameObject GameItemButtonPrefab => _gameItemButtonPrefab;
        
        //Crafting Panel
        [SerializeField] private GameObject _craftingPanel;
        public GameObject CraftingPanel => _craftingPanel;
        
        private void Start()
        {
            var controller = ServiceLocator.Resolve<CraftingPanelController>();
            controller.Setup(this);
        }
    }
}