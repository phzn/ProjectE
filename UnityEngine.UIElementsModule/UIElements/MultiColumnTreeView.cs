using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x020000FF RID: 255
	public class MultiColumnTreeView : BaseTreeView
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00022B01 File Offset: 0x00020D01
		public new MultiColumnTreeViewController viewController
		{
			get
			{
				return base.viewController as MultiColumnTreeViewController;
			}
		}

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x060008D5 RID: 2261 RVA: 0x00022B10 File Offset: 0x00020D10
		// (remove) Token: 0x060008D6 RID: 2262 RVA: 0x00022B48 File Offset: 0x00020D48
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action columnSortingChanged;

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x060008D7 RID: 2263 RVA: 0x00022B80 File Offset: 0x00020D80
		// (remove) Token: 0x060008D8 RID: 2264 RVA: 0x00022BB8 File Offset: 0x00020DB8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<ContextualMenuPopulateEvent, Column> headerContextMenuPopulateEvent;

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x00022BED File Offset: 0x00020DED
		public IEnumerable<SortColumnDescription> sortedColumns
		{
			get
			{
				return this.m_SortedColumns;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x00022BF5 File Offset: 0x00020DF5
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x00022C00 File Offset: 0x00020E00
		public Columns columns
		{
			get
			{
				return this.m_Columns;
			}
			private set
			{
				bool flag = value == null;
				if (flag)
				{
					this.m_Columns.Clear();
				}
				else
				{
					this.m_Columns = value;
					bool flag2 = this.m_Columns.Count > 0;
					if (flag2)
					{
						base.GetOrCreateViewController();
					}
				}
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00022C47 File Offset: 0x00020E47
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x00022C50 File Offset: 0x00020E50
		public SortColumnDescriptions sortColumnDescriptions
		{
			get
			{
				return this.m_SortColumnDescriptions;
			}
			private set
			{
				bool flag = value == null;
				if (flag)
				{
					this.m_SortColumnDescriptions.Clear();
				}
				else
				{
					this.m_SortColumnDescriptions = value;
					bool flag2 = this.viewController != null;
					if (flag2)
					{
						this.viewController.columnController.header.sortDescriptions = value;
						this.RaiseColumnSortingChanged();
					}
				}
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x00022CA9 File Offset: 0x00020EA9
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x00022CB4 File Offset: 0x00020EB4
		public bool sortingEnabled
		{
			get
			{
				return this.m_SortingEnabled;
			}
			set
			{
				this.m_SortingEnabled = value;
				bool flag = this.viewController != null;
				if (flag)
				{
					this.viewController.columnController.header.sortingEnabled = value;
				}
			}
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00022CEF File Offset: 0x00020EEF
		public MultiColumnTreeView()
			: this(new Columns())
		{
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00022D00 File Offset: 0x00020F00
		public MultiColumnTreeView(Columns columns)
		{
			base.scrollView.viewDataKey = "unity-multi-column-scroll-view";
			this.columns = columns ?? new Columns();
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00022D4D File Offset: 0x00020F4D
		internal override void SetRootItemsInternal<T>(IList<TreeViewItemData<T>> rootItems)
		{
			TreeViewHelpers<T, DefaultMultiColumnTreeViewController<T>>.SetRootItems(this, rootItems, () => new DefaultMultiColumnTreeViewController<T>(this.columns, this.m_SortColumnDescriptions, this.m_SortedColumns));
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00022D64 File Offset: 0x00020F64
		private protected override IEnumerable<TreeViewItemData<T>> GetSelectedItemsInternal<T>()
		{
			return TreeViewHelpers<T, DefaultMultiColumnTreeViewController<T>>.GetSelectedItems(this);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00022D7C File Offset: 0x00020F7C
		private protected override T GetItemDataForIndexInternal<T>(int index)
		{
			return TreeViewHelpers<T, DefaultMultiColumnTreeViewController<T>>.GetItemDataForIndex(this, index);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00022D98 File Offset: 0x00020F98
		private protected override T GetItemDataForIdInternal<T>(int id)
		{
			return TreeViewHelpers<T, DefaultMultiColumnTreeViewController<T>>.GetItemDataForId(this, id);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00022DB1 File Offset: 0x00020FB1
		private protected override void AddItemInternal<T>(TreeViewItemData<T> item, int parentId, int childIndex, bool rebuildTree)
		{
			TreeViewHelpers<T, DefaultMultiColumnTreeViewController<T>>.AddItem(this, item, parentId, childIndex, rebuildTree);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00022DC0 File Offset: 0x00020FC0
		protected override CollectionViewController CreateViewController()
		{
			return new DefaultMultiColumnTreeViewController<object>(this.columns, this.sortColumnDescriptions, this.m_SortedColumns);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00022DDC File Offset: 0x00020FDC
		public override void SetViewController(CollectionViewController controller)
		{
			bool flag = this.viewController != null;
			if (flag)
			{
				this.viewController.columnController.columnSortingChanged -= this.RaiseColumnSortingChanged;
				this.viewController.columnController.headerContextMenuPopulateEvent -= this.RaiseHeaderContextMenuPopulate;
			}
			base.SetViewController(controller);
			bool flag2 = this.viewController != null;
			if (flag2)
			{
				this.viewController.header.sortingEnabled = this.m_SortingEnabled;
				this.viewController.columnController.columnSortingChanged += this.RaiseColumnSortingChanged;
				this.viewController.columnController.headerContextMenuPopulateEvent += this.RaiseHeaderContextMenuPopulate;
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00022E9B File Offset: 0x0002109B
		private protected override void CreateVirtualizationController()
		{
			base.CreateVirtualizationController<ReusableMultiColumnTreeViewItem>();
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00022EA5 File Offset: 0x000210A5
		private void RaiseColumnSortingChanged()
		{
			Action action = this.columnSortingChanged;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00022EBA File Offset: 0x000210BA
		private void RaiseHeaderContextMenuPopulate(ContextualMenuPopulateEvent evt, Column column)
		{
			Action<ContextualMenuPopulateEvent, Column> action = this.headerContextMenuPopulateEvent;
			if (action != null)
			{
				action(evt, column);
			}
		}

		// Token: 0x040003FE RID: 1022
		private Columns m_Columns;

		// Token: 0x040003FF RID: 1023
		private bool m_SortingEnabled;

		// Token: 0x04000400 RID: 1024
		private SortColumnDescriptions m_SortColumnDescriptions = new SortColumnDescriptions();

		// Token: 0x04000401 RID: 1025
		private List<SortColumnDescription> m_SortedColumns = new List<SortColumnDescription>();

		// Token: 0x02000100 RID: 256
		public new class UxmlFactory : UxmlFactory<MultiColumnTreeView, MultiColumnTreeView.UxmlTraits>
		{
		}

		// Token: 0x02000101 RID: 257
		public new class UxmlTraits : BaseTreeView.UxmlTraits
		{
			// Token: 0x060008EE RID: 2286 RVA: 0x00022EF4 File Offset: 0x000210F4
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				MultiColumnTreeView multiColumnTreeView = (MultiColumnTreeView)ve;
				multiColumnTreeView.sortingEnabled = this.m_SortingEnabled.GetValueFromBag(bag, cc);
				multiColumnTreeView.sortColumnDescriptions = this.m_SortColumnDescriptions.GetValueFromBag(bag, cc);
				multiColumnTreeView.columns = this.m_Columns.GetValueFromBag(bag, cc);
			}

			// Token: 0x04000404 RID: 1028
			private readonly UxmlBoolAttributeDescription m_SortingEnabled = new UxmlBoolAttributeDescription
			{
				name = "sorting-enabled"
			};

			// Token: 0x04000405 RID: 1029
			private readonly UxmlObjectAttributeDescription<Columns> m_Columns = new UxmlObjectAttributeDescription<Columns>();

			// Token: 0x04000406 RID: 1030
			private readonly UxmlObjectAttributeDescription<SortColumnDescriptions> m_SortColumnDescriptions = new UxmlObjectAttributeDescription<SortColumnDescriptions>();
		}
	}
}
