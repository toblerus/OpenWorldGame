using UnityEngine;

namespace PanelCore
{
    public class XYZPanelView : Panel
    {
        public override void OnOpen()
        {
            Debug.Log("XYZPanel opened");
        }

        public override void OnClose()
        {
            Debug.Log("XYZPanel closed");
        }
    }
}