using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200003A RID: 58
	internal class DefaultMultiColumnTreeViewController<T> : MultiColumnTreeViewController, IDefaultTreeViewController<T>
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00008438 File Offset: 0x00006638
		private TreeDataController<T> treeDataController
		{
			get
			{
				TreeDataController<T> treeDataController;
				if ((treeDataController = this.m_TreeDataController) == null)
				{
					treeDataController = (this.m_TreeDataController = new TreeDataController<T>());
				}
				return treeDataController;
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000845D File Offset: 0x0000665D
		public DefaultMultiColumnTreeViewController(Columns columns, SortColumnDescriptions sortDescriptions, List<SortColumnDescription> sortedColumns)
			: base(columns, sortDescriptions, sortedColumns)
		{
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000846A File Offset: 0x0000666A
		// (set) Token: 0x06000276 RID: 630 RVA: 0x00008474 File Offset: 0x00006674
		public override IList itemsSource
		{
			get
			{
				return base.itemsSource;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					this.SetRootItems(null);
				}
				else
				{
					IList<TreeViewItemData<T>> list = value as IList<TreeViewItemData<T>>;
					bool flag2 = list != null;
					if (flag2)
					{
						this.SetRootItems(list);
					}
					else
					{
						Debug.LogError(string.Format("Type does not match this tree view controller's data type ({0}).", typeof(T)));
					}
				}
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000084D0 File Offset: 0x000066D0
		public void SetRootItems(IList<TreeViewItemData<T>> items)
		{
			bool flag = items == base.itemsSource;
			if (!flag)
			{
				this.treeDataController.SetRootItems(items);
				base.RebuildTree();
				base.RaiseItemsSourceChanged();
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00008508 File Offset: 0x00006708
		public virtual void AddItem(in TreeViewItemData<T> item, int parentId, int childIndex, bool rebuildTree = true)
		{
			this.treeDataController.AddItem(in item, parentId, childIndex);
			if (rebuildTree)
			{
				base.RebuildTree();
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00008534 File Offset: 0x00006734
		public virtual TreeViewItemData<T> GetTreeViewItemDataForId(int id)
		{
			return this.treeDataController.GetTreeItemDataForId(id);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00008554 File Offset: 0x00006754
		public virtual TreeViewItemData<T> GetTreeViewItemDataForIndex(int index)
		{
			int idForIndex = this.GetIdForIndex(index);
			return this.treeDataController.GetTreeItemDataForId(idForIndex);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000857C File Offset: 0x0000677C
		public override bool TryRemoveItem(int id, bool rebuildTree = true)
		{
			bool flag = this.treeDataController.TryRemoveItem(id);
			bool flag2;
			if (flag)
			{
				if (rebuildTree)
				{
					base.RebuildTree();
				}
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000085B0 File Offset: 0x000067B0
		public T GetDataForId(int id)
		{
			return this.treeDataController.GetDataForId(id);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000085D0 File Offset: 0x000067D0
		public T GetDataForIndex(int index)
		{
			return this.treeDataController.GetDataForId(this.GetIdForIndex(index));
		}

		// Token: 0x0600027E RID: 638 RVA: 0x000085F4 File Offset: 0x000067F4
		public override object GetItemForIndex(int index)
		{
			return this.treeDataController.GetDataForId(this.GetIdForIndex(index));
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00008620 File Offset: 0x00006820
		public override int GetParentId(int id)
		{
			return this.treeDataController.GetParentId(id);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00008640 File Offset: 0x00006840
		public override bool HasChildren(int id)
		{
			return this.treeDataController.HasChildren(id);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00008660 File Offset: 0x00006860
		public override IEnumerable<int> GetChildrenIds(int id)
		{
			return this.treeDataController.GetChildrenIds(id);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00008680 File Offset: 0x00006880
		public override void Move(int id, int newParentId, int childIndex = -1, bool rebuildTree = true)
		{
			bool flag = id == newParentId;
			if (!flag)
			{
				bool flag2 = this.IsChildOf(newParentId, id);
				if (!flag2)
				{
					this.treeDataController.Move(id, newParentId, childIndex);
					if (rebuildTree)
					{
						base.RebuildTree();
						base.RaiseItemIndexChanged(id, newParentId);
					}
				}
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000086D0 File Offset: 0x000068D0
		private bool IsChildOf(int childId, int id)
		{
			return this.treeDataController.IsChildOf(childId, id);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x000086F0 File Offset: 0x000068F0
		public override IEnumerable<int> GetAllItemIds(IEnumerable<int> rootIds = null)
		{
			return this.treeDataController.GetAllItemIds(rootIds);
		}

		// Token: 0x040000B8 RID: 184
		private TreeDataController<T> m_TreeDataController;
	}
}
