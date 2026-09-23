using _Scripts.Injection;
using _Scripts.Inventory;
using _Scripts.InWorld.InWorldInteraction;
using UnityEngine;

namespace _Scripts.InWorld
{
    public class InWorldResourceView : MonoBehaviour
    {
        [SerializeField] private InWorldObjectInteractionView _interactionView;
        private InWorldObjectPersistenceModel _persistenceModel;

        public string Id { get; private set; }
        public GameItemType ItemType => _interactionView.ItemType;

        public void Setup(string id)
        {
            Id = id;
            _persistenceModel = ServiceLocator.Resolve<InWorldObjectPersistenceModel>();
        }

        private void Awake()
        {
            _interactionView.Harvested += OnHarvested;
        }

        private void OnDestroy()
        {
            _interactionView.Harvested -= OnHarvested;
        }

        private void OnHarvested()
        {
            if (!string.IsNullOrEmpty(Id))
                _persistenceModel.MarkHarvested(Id);

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
