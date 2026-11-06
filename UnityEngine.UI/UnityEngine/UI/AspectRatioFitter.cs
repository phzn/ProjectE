using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x0200001A RID: 26
	[AddComponentMenu("Layout/Aspect Ratio Fitter", 142)]
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	public class AspectRatioFitter : UIBehaviour, ILayoutSelfController, ILayoutController
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000CF7F File Offset: 0x0000B17F
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0000CF87 File Offset: 0x0000B187
		public AspectRatioFitter.AspectMode aspectMode
		{
			get
			{
				return this.m_AspectMode;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<AspectRatioFitter.AspectMode>(ref this.m_AspectMode, value))
				{
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000CF9D File Offset: 0x0000B19D
		// (set) Token: 0x06000206 RID: 518 RVA: 0x0000CFA5 File Offset: 0x0000B1A5
		public float aspectRatio
		{
			get
			{
				return this.m_AspectRatio;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_AspectRatio, value))
				{
					this.SetDirty();
				}
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000CFBB File Offset: 0x0000B1BB
		private RectTransform rectTransform
		{
			get
			{
				if (this.m_Rect == null)
				{
					this.m_Rect = base.GetComponent<RectTransform>();
				}
				return this.m_Rect;
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000CFDD File Offset: 0x0000B1DD
		protected AspectRatioFitter()
		{
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000CFF0 File Offset: 0x0000B1F0
		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_DoesParentExist = this.rectTransform.parent;
			this.SetDirty();
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000D01A File Offset: 0x0000B21A
		protected override void Start()
		{
			base.Start();
			if (!this.IsComponentValidOnObject() || !this.IsAspectModeValid())
			{
				base.enabled = false;
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000D039 File Offset: 0x0000B239
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000D057 File Offset: 0x0000B257
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.m_DoesParentExist = this.rectTransform.parent;
			this.SetDirty();
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000D081 File Offset: 0x0000B281
		protected virtual void Update()
		{
			if (this.m_DelayedSetDirty)
			{
				this.m_DelayedSetDirty = false;
				this.SetDirty();
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000D098 File Offset: 0x0000B298
		protected override void OnRectTransformDimensionsChange()
		{
			this.UpdateRect();
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000D0A0 File Offset: 0x0000B2A0
		private void UpdateRect()
		{
			if (!this.IsActive() || !this.IsComponentValidOnObject())
			{
				return;
			}
			this.m_Tracker.Clear();
			switch (this.m_AspectMode)
			{
			case AspectRatioFitter.AspectMode.WidthControlsHeight:
				this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.SizeDeltaY);
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.rectTransform.rect.width / this.m_AspectRatio);
				return;
			case AspectRatioFitter.AspectMode.HeightControlsWidth:
				this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.SizeDeltaX);
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.rectTransform.rect.height * this.m_AspectRatio);
				return;
			case AspectRatioFitter.AspectMode.FitInParent:
			case AspectRatioFitter.AspectMode.EnvelopeParent:
				if (this.DoesParentExists())
				{
					this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
					this.rectTransform.anchorMin = Vector2.zero;
					this.rectTransform.anchorMax = Vector2.one;
					this.rectTransform.anchoredPosition = Vector2.zero;
					Vector2 zero = Vector2.zero;
					Vector2 parentSize = this.GetParentSize();
					if ((parentSize.y * this.aspectRatio < parentSize.x) ^ (this.m_AspectMode == AspectRatioFitter.AspectMode.FitInParent))
					{
						zero.y = this.GetSizeDeltaToProduceSize(parentSize.x / this.aspectRatio, 1);
					}
					else
					{
						zero.x = this.GetSizeDeltaToProduceSize(parentSize.y * this.aspectRatio, 0);
					}
					this.rectTransform.sizeDelta = zero;
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000D22C File Offset: 0x0000B42C
		private float GetSizeDeltaToProduceSize(float size, int axis)
		{
			return size - this.GetParentSize()[axis] * (this.rectTransform.anchorMax[axis] - this.rectTransform.anchorMin[axis]);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000D274 File Offset: 0x0000B474
		private Vector2 GetParentSize()
		{
			RectTransform rectTransform = this.rectTransform.parent as RectTransform;
			if (rectTransform)
			{
				return rectTransform.rect.size;
			}
			return Vector2.zero;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000D2AE File Offset: 0x0000B4AE
		public virtual void SetLayoutHorizontal()
		{
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
		public virtual void SetLayoutVertical()
		{
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000D2B2 File Offset: 0x0000B4B2
		protected void SetDirty()
		{
			this.UpdateRect();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000D2BC File Offset: 0x0000B4BC
		public bool IsComponentValidOnObject()
		{
			Canvas component = base.gameObject.GetComponent<Canvas>();
			return !component || !component.isRootCanvas || component.renderMode == RenderMode.WorldSpace;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000D2F1 File Offset: 0x0000B4F1
		public bool IsAspectModeValid()
		{
			return this.DoesParentExists() || (this.aspectMode != AspectRatioFitter.AspectMode.EnvelopeParent && this.aspectMode != AspectRatioFitter.AspectMode.FitInParent);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000D310 File Offset: 0x0000B510
		private bool DoesParentExists()
		{
			return this.m_DoesParentExist;
		}

		// Token: 0x040000BC RID: 188
		[SerializeField]
		private AspectRatioFitter.AspectMode m_AspectMode;

		// Token: 0x040000BD RID: 189
		[SerializeField]
		private float m_AspectRatio = 1f;

		// Token: 0x040000BE RID: 190
		[NonSerialized]
		private RectTransform m_Rect;

		// Token: 0x040000BF RID: 191
		private bool m_DelayedSetDirty;

		// Token: 0x040000C0 RID: 192
		private bool m_DoesParentExist;

		// Token: 0x040000C1 RID: 193
		private DrivenRectTransformTracker m_Tracker;

		// Token: 0x02000098 RID: 152
		public enum AspectMode
		{
			// Token: 0x040002B3 RID: 691
			None,
			// Token: 0x040002B4 RID: 692
			WidthControlsHeight,
			// Token: 0x040002B5 RID: 693
			HeightControlsWidth,
			// Token: 0x040002B6 RID: 694
			FitInParent,
			// Token: 0x040002B7 RID: 695
			EnvelopeParent
		}
	}
}
