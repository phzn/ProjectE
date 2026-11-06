using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

namespace UnityEngine.UI
{
	// Token: 0x02000026 RID: 38
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public abstract class LayoutGroup : UIBehaviour, ILayoutElement, ILayoutGroup, ILayoutController
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000E843 File Offset: 0x0000CA43
		// (set) Token: 0x06000295 RID: 661 RVA: 0x0000E84B File Offset: 0x0000CA4B
		public RectOffset padding
		{
			get
			{
				return this.m_Padding;
			}
			set
			{
				this.SetProperty<RectOffset>(ref this.m_Padding, value);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000E85A File Offset: 0x0000CA5A
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000E862 File Offset: 0x0000CA62
		public TextAnchor childAlignment
		{
			get
			{
				return this.m_ChildAlignment;
			}
			set
			{
				this.SetProperty<TextAnchor>(ref this.m_ChildAlignment, value);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000E871 File Offset: 0x0000CA71
		protected RectTransform rectTransform
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

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000E893 File Offset: 0x0000CA93
		protected List<RectTransform> rectChildren
		{
			get
			{
				return this.m_RectChildren;
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000E89C File Offset: 0x0000CA9C
		public virtual void CalculateLayoutInputHorizontal()
		{
			this.m_RectChildren.Clear();
			List<Component> list = CollectionPool<List<Component>, Component>.Get();
			for (int i = 0; i < this.rectTransform.childCount; i++)
			{
				RectTransform rectTransform = this.rectTransform.GetChild(i) as RectTransform;
				if (!(rectTransform == null) && rectTransform.gameObject.activeInHierarchy)
				{
					rectTransform.GetComponents(typeof(ILayoutIgnorer), list);
					if (list.Count == 0)
					{
						this.m_RectChildren.Add(rectTransform);
					}
					else
					{
						for (int j = 0; j < list.Count; j++)
						{
							if (!((ILayoutIgnorer)list[j]).ignoreLayout)
							{
								this.m_RectChildren.Add(rectTransform);
								break;
							}
						}
					}
				}
			}
			CollectionPool<List<Component>, Component>.Release(list);
			this.m_Tracker.Clear();
		}

		// Token: 0x0600029B RID: 667
		public abstract void CalculateLayoutInputVertical();

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000E968 File Offset: 0x0000CB68
		public virtual float minWidth
		{
			get
			{
				return this.GetTotalMinSize(0);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000E971 File Offset: 0x0000CB71
		public virtual float preferredWidth
		{
			get
			{
				return this.GetTotalPreferredSize(0);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600029E RID: 670 RVA: 0x0000E97A File Offset: 0x0000CB7A
		public virtual float flexibleWidth
		{
			get
			{
				return this.GetTotalFlexibleSize(0);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000E983 File Offset: 0x0000CB83
		public virtual float minHeight
		{
			get
			{
				return this.GetTotalMinSize(1);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000E98C File Offset: 0x0000CB8C
		public virtual float preferredHeight
		{
			get
			{
				return this.GetTotalPreferredSize(1);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000E995 File Offset: 0x0000CB95
		public virtual float flexibleHeight
		{
			get
			{
				return this.GetTotalFlexibleSize(1);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000E99E File Offset: 0x0000CB9E
		public virtual int layoutPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060002A3 RID: 675
		public abstract void SetLayoutHorizontal();

		// Token: 0x060002A4 RID: 676
		public abstract void SetLayoutVertical();

		// Token: 0x060002A5 RID: 677 RVA: 0x0000E9A4 File Offset: 0x0000CBA4
		protected LayoutGroup()
		{
			if (this.m_Padding == null)
			{
				this.m_Padding = new RectOffset();
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000EA01 File Offset: 0x0000CC01
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetDirty();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000EA0F File Offset: 0x0000CC0F
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000EA2D File Offset: 0x0000CC2D
		protected override void OnDidApplyAnimationProperties()
		{
			this.SetDirty();
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000EA35 File Offset: 0x0000CC35
		protected float GetTotalMinSize(int axis)
		{
			return this.m_TotalMinSize[axis];
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000EA43 File Offset: 0x0000CC43
		protected float GetTotalPreferredSize(int axis)
		{
			return this.m_TotalPreferredSize[axis];
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000EA51 File Offset: 0x0000CC51
		protected float GetTotalFlexibleSize(int axis)
		{
			return this.m_TotalFlexibleSize[axis];
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000EA60 File Offset: 0x0000CC60
		protected float GetStartOffset(int axis, float requiredSpaceWithoutPadding)
		{
			float num = requiredSpaceWithoutPadding + (float)((axis == 0) ? this.padding.horizontal : this.padding.vertical);
			float num2 = this.rectTransform.rect.size[axis] - num;
			float alignmentOnAxis = this.GetAlignmentOnAxis(axis);
			return (float)((axis == 0) ? this.padding.left : this.padding.top) + num2 * alignmentOnAxis;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000EAD4 File Offset: 0x0000CCD4
		protected float GetAlignmentOnAxis(int axis)
		{
			if (axis == 0)
			{
				return (float)(this.childAlignment % TextAnchor.MiddleLeft) * 0.5f;
			}
			return (float)(this.childAlignment / TextAnchor.MiddleLeft) * 0.5f;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000EAF8 File Offset: 0x0000CCF8
		protected void SetLayoutInputForAxis(float totalMin, float totalPreferred, float totalFlexible, int axis)
		{
			this.m_TotalMinSize[axis] = totalMin;
			this.m_TotalPreferredSize[axis] = totalPreferred;
			this.m_TotalFlexibleSize[axis] = totalFlexible;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000EB24 File Offset: 0x0000CD24
		protected void SetChildAlongAxis(RectTransform rect, int axis, float pos)
		{
			if (rect == null)
			{
				return;
			}
			this.SetChildAlongAxisWithScale(rect, axis, pos, 1f);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000EB40 File Offset: 0x0000CD40
		protected void SetChildAlongAxisWithScale(RectTransform rect, int axis, float pos, float scaleFactor)
		{
			if (rect == null)
			{
				return;
			}
			this.m_Tracker.Add(this, rect, DrivenTransformProperties.Anchors | ((axis == 0) ? DrivenTransformProperties.AnchoredPositionX : DrivenTransformProperties.AnchoredPositionY));
			rect.anchorMin = Vector2.up;
			rect.anchorMax = Vector2.up;
			Vector2 anchoredPosition = rect.anchoredPosition;
			anchoredPosition[axis] = ((axis == 0) ? (pos + rect.sizeDelta[axis] * rect.pivot[axis] * scaleFactor) : (-pos - rect.sizeDelta[axis] * (1f - rect.pivot[axis]) * scaleFactor));
			rect.anchoredPosition = anchoredPosition;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000EBF1 File Offset: 0x0000CDF1
		protected void SetChildAlongAxis(RectTransform rect, int axis, float pos, float size)
		{
			if (rect == null)
			{
				return;
			}
			this.SetChildAlongAxisWithScale(rect, axis, pos, size, 1f);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000EC10 File Offset: 0x0000CE10
		protected void SetChildAlongAxisWithScale(RectTransform rect, int axis, float pos, float size, float scaleFactor)
		{
			if (rect == null)
			{
				return;
			}
			this.m_Tracker.Add(this, rect, DrivenTransformProperties.Anchors | ((axis == 0) ? (DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.SizeDeltaX) : (DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.SizeDeltaY)));
			rect.anchorMin = Vector2.up;
			rect.anchorMax = Vector2.up;
			Vector2 sizeDelta = rect.sizeDelta;
			sizeDelta[axis] = size;
			rect.sizeDelta = sizeDelta;
			Vector2 anchoredPosition = rect.anchoredPosition;
			anchoredPosition[axis] = ((axis == 0) ? (pos + size * rect.pivot[axis] * scaleFactor) : (-pos - size * (1f - rect.pivot[axis]) * scaleFactor));
			rect.anchoredPosition = anchoredPosition;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000ECC7 File Offset: 0x0000CEC7
		private bool isRootLayoutGroup
		{
			get
			{
				return base.transform.parent == null || base.transform.parent.GetComponent(typeof(ILayoutGroup)) == null;
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000ECFE File Offset: 0x0000CEFE
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (this.isRootLayoutGroup)
			{
				this.SetDirty();
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000ED14 File Offset: 0x0000CF14
		protected virtual void OnTransformChildrenChanged()
		{
			this.SetDirty();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000ED1C File Offset: 0x0000CF1C
		protected void SetProperty<T>(ref T currentValue, T newValue)
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return;
			}
			currentValue = newValue;
			this.SetDirty();
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000ED6D File Offset: 0x0000CF6D
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			if (!CanvasUpdateRegistry.IsRebuildingLayout())
			{
				LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
				return;
			}
			base.StartCoroutine(this.DelayedSetDirty(this.rectTransform));
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000ED9E File Offset: 0x0000CF9E
		private IEnumerator DelayedSetDirty(RectTransform rectTransform)
		{
			yield return null;
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
			yield break;
		}

		// Token: 0x040000EB RID: 235
		[SerializeField]
		protected RectOffset m_Padding = new RectOffset();

		// Token: 0x040000EC RID: 236
		[SerializeField]
		protected TextAnchor m_ChildAlignment;

		// Token: 0x040000ED RID: 237
		[NonSerialized]
		private RectTransform m_Rect;

		// Token: 0x040000EE RID: 238
		protected DrivenRectTransformTracker m_Tracker;

		// Token: 0x040000EF RID: 239
		private Vector2 m_TotalMinSize = Vector2.zero;

		// Token: 0x040000F0 RID: 240
		private Vector2 m_TotalPreferredSize = Vector2.zero;

		// Token: 0x040000F1 RID: 241
		private Vector2 m_TotalFlexibleSize = Vector2.zero;

		// Token: 0x040000F2 RID: 242
		[NonSerialized]
		private List<RectTransform> m_RectChildren = new List<RectTransform>();
	}
}
