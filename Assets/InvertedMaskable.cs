using System;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Inverted Maskable", 14)]
    [ExecuteAlways]
    [RequireComponent(typeof(Graphic))]
    [DisallowMultipleComponent]
    public class InvertedMaskable : UIBehaviour, IMaterialModifier, ICanvasRaycastFilter
    {
        [NonSerialized]
        private Graphic m_Graphic;

        [NonSerialized]
        private Material m_ModifiedMaterial;

        private Graphic graphic
        {
            get { return m_Graphic ?? (m_Graphic = GetComponent<Graphic>()); }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (graphic != null) graphic.SetMaterialDirty();
            MaskUtilities.NotifyStencilStateChanged(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StencilMaterial.Remove(m_ModifiedMaterial);
            m_ModifiedMaterial = null;
            if (graphic != null) graphic.SetMaterialDirty();
            MaskUtilities.NotifyStencilStateChanged(this);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (!IsActive()) return;
            if (graphic != null) graphic.SetMaterialDirty();
            MaskUtilities.NotifyStencilStateChanged(this);
        }
#endif

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            if (!IsActive() || graphic == null || baseMaterial == null) return baseMaterial;

            var root = MaskUtilities.FindRootSortOverrideCanvas(transform);
            var stencilDepth = MaskUtilities.GetStencilDepth(transform, root);
            if (stencilDepth <= 0) return baseMaterial;
            if (stencilDepth >= 8) return baseMaterial;

            var desiredStencilBit = 1 << stencilDepth;
            var id = desiredStencilBit - 1;
            var mat = StencilMaterial.Add(baseMaterial, id, StencilOp.Keep, CompareFunction.NotEqual, ColorWriteMask.All, id, desiredStencilBit | id);
            StencilMaterial.Remove(m_ModifiedMaterial);
            m_ModifiedMaterial = mat;
            return m_ModifiedMaterial;
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (!isActiveAndEnabled) return true;
            var mask = GetComponentInParent<Mask>(false);
            if (mask == null || !mask.isActiveAndEnabled) return true;
            var inside = RectTransformUtility.RectangleContainsScreenPoint(mask.rectTransform, sp, eventCamera);
            return !inside;
        }
    }
}
