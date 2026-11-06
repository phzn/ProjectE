using System;
using System.Collections.Generic;
using UnityEngine.UIElements.Internal;

namespace UnityEngine.UIElements
{
	// Token: 0x02000040 RID: 64
	public abstract class MultiColumnTreeViewController : BaseTreeViewController
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00008CBB File Offset: 0x00006EBB
		public MultiColumnController columnController
		{
			get
			{
				return this.m_ColumnController;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00008CC3 File Offset: 0x00006EC3
		internal MultiColumnCollectionHeader header
		{
			get
			{
				MultiColumnController columnController = this.m_ColumnController;
				return (columnController != null) ? columnController.header : null;
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00008CD7 File Offset: 0x00006ED7
		protected MultiColumnTreeViewController(Columns columns, SortColumnDescriptions sortDescriptions, List<SortColumnDescription> sortedColumns)
		{
			this.m_ColumnController = new MultiColumnController(columns, sortDescriptions, sortedColumns);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00008CF0 File Offset: 0x00006EF0
		internal override void InvokeMakeItem(ReusableCollectionItem reusableItem)
		{
			ReusableMultiColumnTreeViewItem reusableMultiColumnTreeViewItem = reusableItem as ReusableMultiColumnTreeViewItem;
			bool flag = reusableMultiColumnTreeViewItem != null;
			if (flag)
			{
				reusableMultiColumnTreeViewItem.Init(this.MakeItem(), this.m_ColumnController.header.columns);
				base.PostInitRegistration(reusableMultiColumnTreeViewItem);
			}
			else
			{
				base.InvokeMakeItem(reusableItem);
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00008D40 File Offset: 0x00006F40
		protected override VisualElement MakeItem()
		{
			return this.m_ColumnController.MakeItem();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00008D5D File Offset: 0x00006F5D
		protected override void BindItem(VisualElement element, int index)
		{
			this.m_ColumnController.BindItem<object>(element, index, this.GetItemForIndex(index));
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00008D75 File Offset: 0x00006F75
		protected override void UnbindItem(VisualElement element, int index)
		{
			this.m_ColumnController.UnbindItem(element, index);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00008D86 File Offset: 0x00006F86
		protected override void DestroyItem(VisualElement element)
		{
			this.m_ColumnController.DestroyItem(element);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00008D96 File Offset: 0x00006F96
		protected override void PrepareView()
		{
			this.m_ColumnController.PrepareView(base.view);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00008DAB File Offset: 0x00006FAB
		public override void Dispose()
		{
			this.m_ColumnController.Dispose();
			this.m_ColumnController = null;
			base.Dispose();
		}

		// Token: 0x040000BB RID: 187
		private MultiColumnController m_ColumnController;
	}
}
