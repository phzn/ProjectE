using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000138 RID: 312
	public class TreeView : BaseTreeView
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x000290AE File Offset: 0x000272AE
		// (set) Token: 0x06000A47 RID: 2631 RVA: 0x000290B8 File Offset: 0x000272B8
		public new Func<VisualElement> makeItem
		{
			get
			{
				return this.m_MakeItem;
			}
			set
			{
				bool flag = value != this.m_MakeItem;
				if (flag)
				{
					this.m_MakeItem = value;
					base.Rebuild();
				}
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x000290E6 File Offset: 0x000272E6
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x000290F0 File Offset: 0x000272F0
		public new Action<VisualElement, int> bindItem
		{
			get
			{
				return this.m_BindItem;
			}
			set
			{
				bool flag = value != this.m_BindItem;
				if (flag)
				{
					this.m_BindItem = value;
					base.RefreshItems();
				}
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0002911E File Offset: 0x0002731E
		// (set) Token: 0x06000A4B RID: 2635 RVA: 0x00029126 File Offset: 0x00027326
		public new Action<VisualElement, int> unbindItem { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0002912F File Offset: 0x0002732F
		// (set) Token: 0x06000A4D RID: 2637 RVA: 0x00029137 File Offset: 0x00027337
		public new Action<VisualElement> destroyItem { get; set; }

		// Token: 0x06000A4E RID: 2638 RVA: 0x00029140 File Offset: 0x00027340
		internal override void SetRootItemsInternal<T>(IList<TreeViewItemData<T>> rootItems)
		{
			TreeViewHelpers<T, DefaultTreeViewController<T>>.SetRootItems(this, rootItems, () => new DefaultTreeViewController<T>());
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0002916C File Offset: 0x0002736C
		internal override bool HasValidDataAndBindings()
		{
			return base.HasValidDataAndBindings() && this.makeItem != null == (this.bindItem != null);
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0002919D File Offset: 0x0002739D
		public new TreeViewController viewController
		{
			get
			{
				return base.viewController as TreeViewController;
			}
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x000291AA File Offset: 0x000273AA
		protected override CollectionViewController CreateViewController()
		{
			return new DefaultTreeViewController<object>();
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x000291B1 File Offset: 0x000273B1
		public TreeView()
			: this(null, null)
		{
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x000291BD File Offset: 0x000273BD
		public TreeView(Func<VisualElement> makeItem, Action<VisualElement, int> bindItem)
			: base(-1)
		{
			this.makeItem = makeItem;
			this.bindItem = bindItem;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x000291D8 File Offset: 0x000273D8
		public TreeView(int itemHeight, Func<VisualElement> makeItem, Action<VisualElement, int> bindItem)
			: this(makeItem, bindItem)
		{
			base.fixedItemHeight = (float)itemHeight;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x000291F0 File Offset: 0x000273F0
		private protected override IEnumerable<TreeViewItemData<T>> GetSelectedItemsInternal<T>()
		{
			return TreeViewHelpers<T, DefaultTreeViewController<T>>.GetSelectedItems(this);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00029208 File Offset: 0x00027408
		private protected override T GetItemDataForIndexInternal<T>(int index)
		{
			return TreeViewHelpers<T, DefaultTreeViewController<T>>.GetItemDataForIndex(this, index);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00029224 File Offset: 0x00027424
		private protected override T GetItemDataForIdInternal<T>(int id)
		{
			return TreeViewHelpers<T, DefaultTreeViewController<T>>.GetItemDataForId(this, id);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0002923D File Offset: 0x0002743D
		private protected override void AddItemInternal<T>(TreeViewItemData<T> item, int parentId, int childIndex, bool rebuildTree)
		{
			TreeViewHelpers<T, DefaultTreeViewController<T>>.AddItem(this, item, parentId, childIndex, rebuildTree);
		}

		// Token: 0x040004EC RID: 1260
		private Func<VisualElement> m_MakeItem;

		// Token: 0x040004ED RID: 1261
		private Action<VisualElement, int> m_BindItem;

		// Token: 0x02000139 RID: 313
		public new class UxmlFactory : UxmlFactory<TreeView, TreeView.UxmlTraits>
		{
		}

		// Token: 0x0200013A RID: 314
		public new class UxmlTraits : BaseTreeView.UxmlTraits
		{
		}
	}
}
