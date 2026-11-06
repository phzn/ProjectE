using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x0200001C RID: 28
	[AddComponentMenu("Layout/Content Size Fitter", 141)]
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class ContentSizeFitter : UIBehaviour, ILayoutSelfController, ILayoutController
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000D7F8 File Offset: 0x0000B9F8
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0000D800 File Offset: 0x0000BA00
		public ContentSizeFitter.FitMode horizontalFit
		{
			get
			{
				return this.m_HorizontalFit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<ContentSizeFitter.FitMode>(ref this.m_HorizontalFit, value))
				{
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000D816 File Offset: 0x0000BA16
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000D81E File Offset: 0x0000BA1E
		public ContentSizeFitter.FitMode verticalFit
		{
			get
			{
				return this.m_VerticalFit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<ContentSizeFitter.FitMode>(ref this.m_VerticalFit, value))
				{
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000D834 File Offset: 0x0000BA34
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

		// Token: 0x0600023C RID: 572 RVA: 0x0000D856 File Offset: 0x0000BA56
		protected ContentSizeFitter()
		{
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000D85E File Offset: 0x0000BA5E
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetDirty();
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000D86C File Offset: 0x0000BA6C
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000D88A File Offset: 0x0000BA8A
		protected override void OnRectTransformDimensionsChange()
		{
			this.SetDirty();
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000D894 File Offset: 0x0000BA94
		private void HandleSelfFittingAlongAxis(int axis)
		{
			ContentSizeFitter.FitMode fitMode = ((axis == 0) ? this.horizontalFit : this.verticalFit);
			if (fitMode == ContentSizeFitter.FitMode.Unconstrained)
			{
				this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.None);
				return;
			}
			this.m_Tracker.Add(this, this.rectTransform, (axis == 0) ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY);
			if (fitMode == ContentSizeFitter.FitMode.MinSize)
			{
				this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetMinSize(this.m_Rect, axis));
				return;
			}
			this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetPreferredSize(this.m_Rect, axis));
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000D920 File Offset: 0x0000BB20
		public virtual void SetLayoutHorizontal()
		{
			this.m_Tracker.Clear();
			this.HandleSelfFittingAlongAxis(0);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000D934 File Offset: 0x0000BB34
		public virtual void SetLayoutVertical()
		{
			this.HandleSelfFittingAlongAxis(1);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000D93D File Offset: 0x0000BB3D
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		}

		// Token: 0x040000D1 RID: 209
		[SerializeField]
		protected ContentSizeFitter.FitMode m_HorizontalFit;

		// Token: 0x040000D2 RID: 210
		[SerializeField]
		protected ContentSizeFitter.FitMode m_VerticalFit;

		// Token: 0x040000D3 RID: 211
		[NonSerialized]
		private RectTransform m_Rect;

		// Token: 0x040000D4 RID: 212
		private DrivenRectTransformTracker m_Tracker;

		// Token: 0x0200009C RID: 156
		public enum FitMode
		{
			// Token: 0x040002C7 RID: 711
			Unconstrained,
			// Token: 0x040002C8 RID: 712
			MinSize,
			// Token: 0x040002C9 RID: 713
			PreferredSize
		}
	}
}
