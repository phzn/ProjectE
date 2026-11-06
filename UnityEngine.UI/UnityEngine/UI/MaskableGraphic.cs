using System;
using System.ComponentModel;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEngine.UI
{
	// Token: 0x0200002B RID: 43
	public abstract class MaskableGraphic : Graphic, IClippable, IMaskable, IMaterialModifier
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000F8CF File Offset: 0x0000DACF
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000F8D7 File Offset: 0x0000DAD7
		public MaskableGraphic.CullStateChangedEvent onCullStateChanged
		{
			get
			{
				return this.m_OnCullStateChanged;
			}
			set
			{
				this.m_OnCullStateChanged = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000F8E0 File Offset: 0x0000DAE0
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000F8E8 File Offset: 0x0000DAE8
		public bool maskable
		{
			get
			{
				return this.m_Maskable;
			}
			set
			{
				if (value == this.m_Maskable)
				{
					return;
				}
				this.m_Maskable = value;
				this.m_ShouldRecalculateStencil = true;
				this.SetMaterialDirty();
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000F908 File Offset: 0x0000DB08
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0000F910 File Offset: 0x0000DB10
		public bool isMaskingGraphic
		{
			get
			{
				return this.m_IsMaskingGraphic;
			}
			set
			{
				if (value == this.m_IsMaskingGraphic)
				{
					return;
				}
				this.m_IsMaskingGraphic = value;
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000F924 File Offset: 0x0000DB24
		public virtual Material GetModifiedMaterial(Material baseMaterial)
		{
			Material material = baseMaterial;
			if (this.m_ShouldRecalculateStencil)
			{
				if (this.maskable)
				{
					Transform transform = MaskUtilities.FindRootSortOverrideCanvas(base.transform);
					this.m_StencilValue = MaskUtilities.GetStencilDepth(base.transform, transform);
				}
				else
				{
					this.m_StencilValue = 0;
				}
				this.m_ShouldRecalculateStencil = false;
			}
			if (this.m_StencilValue > 0 && !this.isMaskingGraphic)
			{
				Material material2 = StencilMaterial.Add(material, (1 << this.m_StencilValue) - 1, StencilOp.Keep, CompareFunction.Equal, ColorWriteMask.All, (1 << this.m_StencilValue) - 1, 0);
				StencilMaterial.Remove(this.m_MaskMaterial);
				this.m_MaskMaterial = material2;
				material = this.m_MaskMaterial;
			}
			return material;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000F9C4 File Offset: 0x0000DBC4
		public virtual void Cull(Rect clipRect, bool validRect)
		{
			bool flag = !validRect || !clipRect.Overlaps(this.rootCanvasRect, true);
			this.UpdateCull(flag);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000F9F0 File Offset: 0x0000DBF0
		private void UpdateCull(bool cull)
		{
			if (base.canvasRenderer.cull != cull)
			{
				base.canvasRenderer.cull = cull;
				UISystemProfilerApi.AddMarker("MaskableGraphic.cullingChanged", this);
				this.m_OnCullStateChanged.Invoke(cull);
				this.OnCullingChanged();
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000FA29 File Offset: 0x0000DC29
		public virtual void SetClipRect(Rect clipRect, bool validRect)
		{
			if (validRect)
			{
				base.canvasRenderer.EnableRectClipping(clipRect);
				return;
			}
			base.canvasRenderer.DisableRectClipping();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000FA46 File Offset: 0x0000DC46
		public virtual void SetClipSoftness(Vector2 clipSoftness)
		{
			base.canvasRenderer.clippingSoftness = clipSoftness;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000FA54 File Offset: 0x0000DC54
		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_ShouldRecalculateStencil = true;
			this.UpdateClipParent();
			this.SetMaterialDirty();
			if (this.isMaskingGraphic)
			{
				MaskUtilities.NotifyStencilStateChanged(this);
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000FA7D File Offset: 0x0000DC7D
		protected override void OnDisable()
		{
			base.OnDisable();
			this.m_ShouldRecalculateStencil = true;
			this.SetMaterialDirty();
			this.UpdateClipParent();
			StencilMaterial.Remove(this.m_MaskMaterial);
			this.m_MaskMaterial = null;
			if (this.isMaskingGraphic)
			{
				MaskUtilities.NotifyStencilStateChanged(this);
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000FAB8 File Offset: 0x0000DCB8
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.m_ShouldRecalculateStencil = true;
			this.UpdateClipParent();
			this.SetMaterialDirty();
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000FADC File Offset: 0x0000DCDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Not used anymore.", true)]
		public virtual void ParentMaskStateChanged()
		{
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000FADE File Offset: 0x0000DCDE
		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.m_ShouldRecalculateStencil = true;
			this.UpdateClipParent();
			this.SetMaterialDirty();
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000FB04 File Offset: 0x0000DD04
		private Rect rootCanvasRect
		{
			get
			{
				base.rectTransform.GetWorldCorners(this.m_Corners);
				if (base.canvas)
				{
					Matrix4x4 worldToLocalMatrix = base.canvas.rootCanvas.transform.worldToLocalMatrix;
					for (int i = 0; i < 4; i++)
					{
						this.m_Corners[i] = worldToLocalMatrix.MultiplyPoint(this.m_Corners[i]);
					}
				}
				Vector2 vector = this.m_Corners[0];
				Vector2 vector2 = this.m_Corners[0];
				for (int j = 1; j < 4; j++)
				{
					vector.x = Mathf.Min(this.m_Corners[j].x, vector.x);
					vector.y = Mathf.Min(this.m_Corners[j].y, vector.y);
					vector2.x = Mathf.Max(this.m_Corners[j].x, vector2.x);
					vector2.y = Mathf.Max(this.m_Corners[j].y, vector2.y);
				}
				return new Rect(vector, vector2 - vector);
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000FC48 File Offset: 0x0000DE48
		private void UpdateClipParent()
		{
			RectMask2D rectMask2D = ((this.maskable && this.IsActive()) ? MaskUtilities.GetRectMaskForClippable(this) : null);
			if (this.m_ParentMask != null && (rectMask2D != this.m_ParentMask || !rectMask2D.IsActive()))
			{
				this.m_ParentMask.RemoveClippable(this);
				this.UpdateCull(false);
			}
			if (rectMask2D != null && rectMask2D.IsActive())
			{
				rectMask2D.AddClippable(this);
			}
			this.m_ParentMask = rectMask2D;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000FCC5 File Offset: 0x0000DEC5
		public virtual void RecalculateClipping()
		{
			this.UpdateClipParent();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000FCCD File Offset: 0x0000DECD
		public virtual void RecalculateMasking()
		{
			StencilMaterial.Remove(this.m_MaskMaterial);
			this.m_MaskMaterial = null;
			this.m_ShouldRecalculateStencil = true;
			this.SetMaterialDirty();
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000FD22 File Offset: 0x0000DF22
		GameObject IClippable.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x040000FB RID: 251
		[NonSerialized]
		protected bool m_ShouldRecalculateStencil = true;

		// Token: 0x040000FC RID: 252
		[NonSerialized]
		protected Material m_MaskMaterial;

		// Token: 0x040000FD RID: 253
		[NonSerialized]
		private RectMask2D m_ParentMask;

		// Token: 0x040000FE RID: 254
		[SerializeField]
		private bool m_Maskable = true;

		// Token: 0x040000FF RID: 255
		private bool m_IsMaskingGraphic;

		// Token: 0x04000100 RID: 256
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Not used anymore.", true)]
		[NonSerialized]
		protected bool m_IncludeForMasking;

		// Token: 0x04000101 RID: 257
		[SerializeField]
		private MaskableGraphic.CullStateChangedEvent m_OnCullStateChanged = new MaskableGraphic.CullStateChangedEvent();

		// Token: 0x04000102 RID: 258
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Not used anymore", true)]
		[NonSerialized]
		protected bool m_ShouldRecalculate = true;

		// Token: 0x04000103 RID: 259
		[NonSerialized]
		protected int m_StencilValue;

		// Token: 0x04000104 RID: 260
		private readonly Vector3[] m_Corners = new Vector3[4];

		// Token: 0x020000A3 RID: 163
		[Serializable]
		public class CullStateChangedEvent : UnityEvent<bool>
		{
		}
	}
}
