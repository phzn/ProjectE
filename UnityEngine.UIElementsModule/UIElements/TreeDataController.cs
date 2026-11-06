using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000041 RID: 65
	internal sealed class TreeDataController<T>
	{
		// Token: 0x060002BA RID: 698 RVA: 0x00008DC8 File Offset: 0x00006FC8
		public void SetRootItems(IList<TreeViewItemData<T>> rootItems)
		{
			this.m_TreeData = new TreeData<T>(rootItems);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00008DD7 File Offset: 0x00006FD7
		public void AddItem(in TreeViewItemData<T> item, int parentId, int childIndex)
		{
			this.m_TreeData.AddItem(item, parentId, childIndex);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00008DF0 File Offset: 0x00006FF0
		public bool TryRemoveItem(int id)
		{
			return this.m_TreeData.TryRemove(id);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00008E10 File Offset: 0x00007010
		public TreeViewItemData<T> GetTreeItemDataForId(int id)
		{
			return this.m_TreeData.GetDataForId(id);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00008E30 File Offset: 0x00007030
		public T GetDataForId(int id)
		{
			return this.m_TreeData.GetDataForId(id).data;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00008E58 File Offset: 0x00007058
		public int GetParentId(int id)
		{
			return this.m_TreeData.GetParentId(id);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00008E78 File Offset: 0x00007078
		public bool HasChildren(int id)
		{
			return this.m_TreeData.GetDataForId(id).hasChildren;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00008E9E File Offset: 0x0000709E
		private static IEnumerable<int> GetItemIds(IEnumerable<TreeViewItemData<T>> items)
		{
			bool flag = items == null;
			if (flag)
			{
				yield break;
			}
			foreach (TreeViewItemData<T> item in items)
			{
				yield return item.id;
				item = default(TreeViewItemData<T>);
			}
			IEnumerator<TreeViewItemData<T>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00008EB0 File Offset: 0x000070B0
		public IEnumerable<int> GetChildrenIds(int id)
		{
			return TreeDataController<T>.GetItemIds(this.m_TreeData.GetDataForId(id).children);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00008EDC File Offset: 0x000070DC
		public void Move(int id, int newParentId, int childIndex = -1)
		{
			bool flag = id == newParentId;
			if (!flag)
			{
				bool flag2 = this.IsChildOf(newParentId, id);
				if (!flag2)
				{
					this.m_TreeData.Move(id, newParentId, childIndex);
				}
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00008F14 File Offset: 0x00007114
		public bool IsChildOf(int childId, int id)
		{
			return this.m_TreeData.HasAncestor(childId, id);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00008F33 File Offset: 0x00007133
		public IEnumerable<int> GetAllItemIds(IEnumerable<int> rootIds = null)
		{
			this.m_IteratorStack.Clear();
			bool flag = rootIds == null;
			if (flag)
			{
				bool flag2 = this.m_TreeData.rootItemIds == null;
				if (flag2)
				{
					yield break;
				}
				rootIds = this.m_TreeData.rootItemIds;
			}
			IEnumerator<int> currentIterator = rootIds.GetEnumerator();
			for (;;)
			{
				bool hasNext = currentIterator.MoveNext();
				bool flag3 = !hasNext;
				if (flag3)
				{
					bool flag4 = this.m_IteratorStack.Count > 0;
					if (!flag4)
					{
						break;
					}
					currentIterator = this.m_IteratorStack.Pop();
				}
				else
				{
					int currentItemId = currentIterator.Current;
					yield return currentItemId;
					bool flag5 = this.HasChildren(currentItemId);
					if (flag5)
					{
						this.m_IteratorStack.Push(currentIterator);
						currentIterator = this.GetChildrenIds(currentItemId).GetEnumerator();
					}
				}
			}
			yield break;
		}

		// Token: 0x040000BC RID: 188
		private TreeData<T> m_TreeData;

		// Token: 0x040000BD RID: 189
		private Stack<IEnumerator<int>> m_IteratorStack = new Stack<IEnumerator<int>>();
	}
}
