using ReactiveCore.Runtime;
using UnityEngine;

namespace _Scripts.Storage
{
    public class StorageInteractionModel : MonoBehaviour
    {
        private ReactiveValue<bool> _isInteractingWithStorage = new();
    
        public ReactiveValue<bool> IsInteractingWithStorage => _isInteractingWithStorage;

        public void SetIsInteractingWithStorage(bool value)
        {
            _isInteractingWithStorage.Value = value;
        }
    }
}
