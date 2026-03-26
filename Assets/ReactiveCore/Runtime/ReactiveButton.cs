using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReactiveCore.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public sealed class ReactiveButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Button button;

        public ReactiveEmitter OnClickInteractable { get; } = new();
        public ReactiveEmitter OnClickNonInteractable { get; } = new();
        
        private void Reset()
        {
            button = GetComponent<Button>();
        }

        private void Awake()
        {
            if (button == null)
                button = GetComponent<Button>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (button == null)
                return;

            if (button.interactable)
                OnClickInteractable.Emit();
            else
                OnClickNonInteractable.Emit();
        }
    }
}