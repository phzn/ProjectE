using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000045 RID: 69
	internal abstract class CollectionVirtualizationController
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002DF RID: 735
		// (set) Token: 0x060002E0 RID: 736
		public abstract int firstVisibleIndex { get; protected set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002E1 RID: 737
		public abstract int visibleItemCount { get; }

		// Token: 0x060002E2 RID: 738 RVA: 0x00009473 File Offset: 0x00007673
		protected CollectionVirtualizationController(ScrollView scrollView)
		{
			this.m_ScrollView = scrollView;
		}

		// Token: 0x060002E3 RID: 739
		public abstract void Refresh(bool rebuild);

		// Token: 0x060002E4 RID: 740
		public abstract void ScrollToItem(int id);

		// Token: 0x060002E5 RID: 741
		public abstract void Resize(Vector2 size);

		// Token: 0x060002E6 RID: 742
		public abstract void OnScroll(Vector2 offset);

		// Token: 0x060002E7 RID: 743
		public abstract int GetIndexFromPosition(Vector2 position);

		// Token: 0x060002E8 RID: 744
		public abstract float GetExpectedItemHeight(int index);

		// Token: 0x060002E9 RID: 745
		public abstract float GetExpectedContentHeight();

		// Token: 0x060002EA RID: 746
		public abstract void OnFocus(VisualElement leafTarget);

		// Token: 0x060002EB RID: 747
		public abstract void OnBlur(VisualElement willFocus);

		// Token: 0x060002EC RID: 748
		public abstract void UpdateBackground();

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002ED RID: 749
		public abstract IEnumerable<ReusableCollectionItem> activeItems { get; }

		// Token: 0x060002EE RID: 750
		internal abstract void StartDragItem(ReusableCollectionItem item);

		// Token: 0x060002EF RID: 751
		internal abstract void EndDrag(int dropIndex);

		// Token: 0x040000CE RID: 206
		protected readonly ScrollView m_ScrollView;
	}
}
