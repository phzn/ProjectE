using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x020000FC RID: 252
	public class MultiColumnListView : BaseListView
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00022709 File Offset: 0x00020909
		public new MultiColumnListViewController viewController
		{
			get
			{
				return base.viewController as MultiColumnListViewController;
			}
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x060008BF RID: 2239 RVA: 0x00022718 File Offset: 0x00020918
		// (remove) Token: 0x060008C0 RID: 2240 RVA: 0x00022750 File Offset: 0x00020950
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action columnSortingChanged;

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x060008C1 RID: 2241 RVA: 0x00022788 File Offset: 0x00020988
		// (remove) Token: 0x060008C2 RID: 2242 RVA: 0x000227C0 File Offset: 0x000209C0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<ContextualMenuPopulateEvent, Column> headerContextMenuPopulateEvent;

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x000227F5 File Offset: 0x000209F5
		public IEnumerable<SortColumnDescription> sortedColumns
		{
			get
			{
				return this.m_SortedColumns;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x000227FD File Offset: 0x000209FD
		// (set) Token: 0x060008C5 RID: 2245 RVA: 0x00022808 File Offset: 0x00020A08
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

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x0002284F File Offset: 0x00020A4F
		// (set) Token: 0x060008C7 RID: 2247 RVA: 0x00022858 File Offset: 0x00020A58
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

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x000228B1 File Offset: 0x00020AB1
		// (set) Token: 0x060008C9 RID: 2249 RVA: 0x000228BC File Offset: 0x00020ABC
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

		// Token: 0x060008CA RID: 2250 RVA: 0x000228F7 File Offset: 0x00020AF7
		public MultiColumnListView()
			: this(new Columns())
		{
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00022908 File Offset: 0x00020B08
		public MultiColumnListView(Columns columns)
		{
			base.scrollView.viewDataKey = "unity-multi-column-scroll-view";
			this.columns = columns ?? new Columns();
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00022955 File Offset: 0x00020B55
		protected override CollectionViewController CreateViewController()
		{
			return new MultiColumnListViewController(this.columns, this.sortColumnDescriptions, this.m_SortedColumns);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00022970 File Offset: 0x00020B70
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

		// Token: 0x060008CE RID: 2254 RVA: 0x00022A2F File Offset: 0x00020C2F
		private protected override void CreateVirtualizationController()
		{
			base.CreateVirtualizationController<ReusableMultiColumnListViewItem>();
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00022A39 File Offset: 0x00020C39
		private void RaiseColumnSortingChanged()
		{
			Action action = this.columnSortingChanged;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00022A4E File Offset: 0x00020C4E
		private void RaiseHeaderContextMenuPopulate(ContextualMenuPopulateEvent evt, Column column)
		{
			Action<ContextualMenuPopulateEvent, Column> action = this.headerContextMenuPopulateEvent;
			if (action != null)
			{
				action(evt, column);
			}
		}

		// Token: 0x040003F5 RID: 1013
		private Columns m_Columns;

		// Token: 0x040003F6 RID: 1014
		private bool m_SortingEnabled;

		// Token: 0x040003F7 RID: 1015
		private SortColumnDescriptions m_SortColumnDescriptions = new SortColumnDescriptions();

		// Token: 0x040003F8 RID: 1016
		private List<SortColumnDescription> m_SortedColumns = new List<SortColumnDescription>();

		// Token: 0x020000FD RID: 253
		public new class UxmlFactory : UxmlFactory<MultiColumnListView, MultiColumnListView.UxmlTraits>
		{
		}

		// Token: 0x020000FE RID: 254
		public new class UxmlTraits : BaseListView.UxmlTraits
		{
			// Token: 0x060008D2 RID: 2258 RVA: 0x00022A70 File Offset: 0x00020C70
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				MultiColumnListView multiColumnListView = (MultiColumnListView)ve;
				multiColumnListView.sortingEnabled = this.m_SortingEnabled.GetValueFromBag(bag, cc);
				multiColumnListView.sortColumnDescriptions = this.m_SortColumnDescriptions.GetValueFromBag(bag, cc);
				multiColumnListView.columns = this.m_Columns.GetValueFromBag(bag, cc);
			}

			// Token: 0x040003FB RID: 1019
			private readonly UxmlBoolAttributeDescription m_SortingEnabled = new UxmlBoolAttributeDescription
			{
				name = "sorting-enabled"
			};

			// Token: 0x040003FC RID: 1020
			private readonly UxmlObjectAttributeDescription<Columns> m_Columns = new UxmlObjectAttributeDescription<Columns>();

			// Token: 0x040003FD RID: 1021
			private readonly UxmlObjectAttributeDescription<SortColumnDescriptions> m_SortColumnDescriptions = new UxmlObjectAttributeDescription<SortColumnDescriptions>();
		}
	}
}
