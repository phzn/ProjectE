using System;
using System.Collections.Generic;
using UnityEngine.UIElements.Internal;

namespace UnityEngine.UIElements
{
	// Token: 0x0200003F RID: 63
	public class MultiColumnListViewController : BaseListViewController
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00008B38 File Offset: 0x00006D38
		public MultiColumnController columnController
		{
			get
			{
				return this.m_ColumnController;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x00008B40 File Offset: 0x00006D40
		internal MultiColumnCollectionHeader header
		{
			get
			{
				MultiColumnController columnController = this.m_ColumnController;
				return (columnController != null) ? columnController.header : null;
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00008B54 File Offset: 0x00006D54
		public MultiColumnListViewController(Columns columns, SortColumnDescriptions sortDescriptions, List<SortColumnDescription> sortedColumns)
		{
			this.m_ColumnController = new MultiColumnController(columns, sortDescriptions, sortedColumns);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00008B6C File Offset: 0x00006D6C
		internal override void InvokeMakeItem(ReusableCollectionItem reusableItem)
		{
			ReusableMultiColumnListViewItem reusableMultiColumnListViewItem = reusableItem as ReusableMultiColumnListViewItem;
			bool flag = reusableMultiColumnListViewItem != null;
			if (flag)
			{
				reusableMultiColumnListViewItem.Init(this.MakeItem(), this.m_ColumnController.header.columns, base.baseListView.reorderMode == ListViewReorderMode.Animated);
				base.PostInitRegistration(reusableMultiColumnListViewItem);
			}
			else
			{
				base.InvokeMakeItem(reusableItem);
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00008BCC File Offset: 0x00006DCC
		protected override VisualElement MakeItem()
		{
			return this.m_ColumnController.MakeItem();
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00008BE9 File Offset: 0x00006DE9
		protected override void BindItem(VisualElement element, int index)
		{
			this.m_ColumnController.BindItem<object>(element, index, this.GetItemForIndex(index));
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00008C01 File Offset: 0x00006E01
		protected override void UnbindItem(VisualElement element, int index)
		{
			this.m_ColumnController.UnbindItem(element, index);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00008C12 File Offset: 0x00006E12
		protected override void DestroyItem(VisualElement element)
		{
			this.m_ColumnController.DestroyItem(element);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00008C22 File Offset: 0x00006E22
		protected override void PrepareView()
		{
			this.m_ColumnController.PrepareView(base.view);
			base.baseListView.reorderModeChanged += this.UpdateReorderClassList;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00008C4F File Offset: 0x00006E4F
		public override void Dispose()
		{
			base.baseListView.reorderModeChanged -= this.UpdateReorderClassList;
			this.m_ColumnController.Dispose();
			this.m_ColumnController = null;
			base.Dispose();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00008C84 File Offset: 0x00006E84
		private void UpdateReorderClassList()
		{
			this.m_ColumnController.header.EnableInClassList(MultiColumnCollectionHeader.reorderableUssClassName, base.baseListView.reorderable && base.baseListView.reorderMode == ListViewReorderMode.Animated);
		}

		// Token: 0x040000BA RID: 186
		private MultiColumnController m_ColumnController;
	}
}
