using UnityEngine;

namespace PanelCore
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private PanelService panelService;

        void Start()
        {
            panelService.OpenPanel("XYZ");
        }

        public void ClosePanel()
        {
            panelService.CloseCurrentPanel();
        }
    }
}