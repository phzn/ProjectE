using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200003B RID: 59
	internal class DefaultTreeViewController<T> : TreeViewController, IDefaultTreeViewController, IDefaultTreeViewController<T>
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000285 RID: 645 RVA: 0x00008710 File Offset: 0x00006910
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000846A File Offset: 0x0000666A
		// (set) Token: 0x06000287 RID: 647 RVA: 0x00008738 File Offset: 0x00006938
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

		// Token: 0x06000288 RID: 648 RVA: 0x00008794 File Offset: 0x00006994
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

		// Token: 0x06000289 RID: 649 RVA: 0x000087CC File Offset: 0x000069CC
		public virtual void AddItem(in TreeViewItemData<T> item, int parentId, int childIndex, bool rebuildTree = true)
		{
			this.treeDataController.AddItem(in item, parentId, childIndex);
			if (rebuildTree)
			{
				base.RebuildTree();
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x000087F8 File Offset: 0x000069F8
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

		// Token: 0x0600028B RID: 651 RVA: 0x0000882C File Offset: 0x00006A2C
		public virtual object GetItemDataForId(int id)
		{
			return this.treeDataController.GetTreeItemDataForId(id).data;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00008858 File Offset: 0x00006A58
		public virtual TreeViewItemData<T> GetTreeViewItemDataForId(int id)
		{
			return this.treeDataController.GetTreeItemDataForId(id);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00008878 File Offset: 0x00006A78
		public virtual TreeViewItemData<T> GetTreeViewItemDataForIndex(int index)
		{
			int idForIndex = this.GetIdForIndex(index);
			return this.treeDataController.GetTreeItemDataForId(idForIndex);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000088A0 File Offset: 0x00006AA0
		public virtual T GetDataForId(int id)
		{
			return this.treeDataController.GetDataForId(id);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000088C0 File Offset: 0x00006AC0
		public virtual T GetDataForIndex(int index)
		{
			return this.treeDataController.GetDataForId(this.GetIdForIndex(index));
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000088E4 File Offset: 0x00006AE4
		public override object GetItemForIndex(int index)
		{
			return this.treeDataController.GetDataForId(this.GetIdForIndex(index));
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00008910 File Offset: 0x00006B10
		public override int GetParentId(int id)
		{
			return this.treeDataController.GetParentId(id);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00008930 File Offset: 0x00006B30
		public override bool HasChildren(int id)
		{
			return this.treeDataController.HasChildren(id);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00008950 File Offset: 0x00006B50
		public override IEnumerable<int> GetChildrenIds(int id)
		{
			return this.treeDataController.GetChildrenIds(id);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00008970 File Offset: 0x00006B70
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
						base.RaiseItemParentChanged(id, newParentId);
					}
				}
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000089C0 File Offset: 0x00006BC0
		private bool IsChildOf(int childId, int id)
		{
			return this.treeDataController.IsChildOf(childId, id);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x000089E0 File Offset: 0x00006BE0
		public override IEnumerable<int> GetAllItemIds(IEnumerable<int> rootIds = null)
		{
			return this.treeDataController.GetAllItemIds(rootIds);
		}

		// Token: 0x040000B9 RID: 185
		private TreeDataController<T> m_TreeDataController;
	}
}
