using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace UnityEngine.UIElements
{
	// Token: 0x0200013E RID: 318
	internal readonly struct TreeData<T>
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00029813 File Offset: 0x00027A13
		public IEnumerable<int> rootItemIds
		{
			get
			{
				return this.m_RootItemIds;
			}
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0002981B File Offset: 0x00027A1B
		public TreeData(IList<TreeViewItemData<T>> rootItems)
		{
			this.m_RootItemIds = new List<int>();
			this.m_Tree = new Dictionary<int, TreeViewItemData<T>>();
			this.m_ParentIds = new Dictionary<int, int>();
			this.m_ChildrenIds = new Dictionary<int, List<int>>();
			this.RefreshTree(rootItems);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00029854 File Offset: 0x00027A54
		public TreeViewItemData<T> GetDataForId(int id)
		{
			TreeViewItemData<T> treeViewItemData;
			bool flag = this.m_Tree.TryGetValue(id, out treeViewItemData);
			TreeViewItemData<T> treeViewItemData2;
			if (flag)
			{
				treeViewItemData2 = treeViewItemData;
			}
			else
			{
				treeViewItemData2 = default(TreeViewItemData<T>);
			}
			return treeViewItemData2;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00029888 File Offset: 0x00027A88
		public int GetParentId(int id)
		{
			int num;
			bool flag = this.m_ParentIds.TryGetValue(id, out num);
			int num2;
			if (flag)
			{
				num2 = num;
			}
			else
			{
				num2 = -1;
			}
			return num2;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x000298B4 File Offset: 0x00027AB4
		public void AddItem(TreeViewItemData<T> item, int parentId, int childIndex)
		{
			List<TreeViewItemData<T>> list = CollectionPool<List<TreeViewItemData<T>>, TreeViewItemData<T>>.Get();
			list.Add(item);
			this.BuildTree(list, false);
			this.AddItemToParent(item, parentId, childIndex);
			CollectionPool<List<TreeViewItemData<T>>, TreeViewItemData<T>>.Release(list);
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x000298EC File Offset: 0x00027AEC
		public bool TryRemove(int id)
		{
			int num;
			bool flag = this.m_ParentIds.TryGetValue(id, out num);
			if (flag)
			{
				this.RemoveFromParent(id, num);
			}
			else
			{
				this.m_RootItemIds.Remove(id);
			}
			return this.TryRemoveChildrenIds(id);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00029934 File Offset: 0x00027B34
		public void Move(int id, int newParentId, int childIndex)
		{
			TreeViewItemData<T> treeViewItemData;
			bool flag = !this.m_Tree.TryGetValue(id, out treeViewItemData);
			if (!flag)
			{
				int num;
				bool flag2 = this.m_ParentIds.TryGetValue(id, out num);
				if (flag2)
				{
					bool flag3 = num == newParentId;
					if (flag3)
					{
						int childIndex2 = this.m_Tree[num].GetChildIndex(id);
						bool flag4 = childIndex2 < childIndex;
						if (flag4)
						{
							childIndex--;
						}
					}
					this.RemoveFromParent(treeViewItemData.id, num);
				}
				else
				{
					int num2 = this.m_RootItemIds.IndexOf(id);
					bool flag5 = newParentId == -1 && num2 < childIndex;
					if (flag5)
					{
						childIndex--;
					}
					this.m_RootItemIds.Remove(id);
				}
				this.AddItemToParent(treeViewItemData, newParentId, childIndex);
			}
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x000299F4 File Offset: 0x00027BF4
		public bool HasAncestor(int childId, int ancestorId)
		{
			bool flag = childId == -1 || ancestorId == -1;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				int num = childId;
				int parentId;
				while ((parentId = this.GetParentId(num)) != -1)
				{
					bool flag3 = ancestorId == parentId;
					if (flag3)
					{
						return true;
					}
					num = parentId;
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00029A48 File Offset: 0x00027C48
		private void AddItemToParent(TreeViewItemData<T> item, int parentId, int childIndex)
		{
			bool flag = parentId == -1;
			if (flag)
			{
				this.m_ParentIds.Remove(item.id);
				bool flag2 = childIndex < 0 || childIndex >= this.m_RootItemIds.Count;
				if (flag2)
				{
					this.m_RootItemIds.Add(item.id);
				}
				else
				{
					this.m_RootItemIds.Insert(childIndex, item.id);
				}
			}
			else
			{
				TreeViewItemData<T> treeViewItemData = this.m_Tree[parentId];
				treeViewItemData.InsertChild(item, childIndex);
				this.m_Tree[parentId] = treeViewItemData;
				this.m_ParentIds[item.id] = parentId;
				this.UpdateParentTree(treeViewItemData);
			}
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00029AFC File Offset: 0x00027CFC
		private void RemoveFromParent(int id, int parentId)
		{
			TreeViewItemData<T> treeViewItemData = this.m_Tree[parentId];
			treeViewItemData.RemoveChild(id);
			this.m_Tree[parentId] = treeViewItemData;
			List<int> list;
			bool flag = this.m_ChildrenIds.TryGetValue(parentId, out list);
			if (flag)
			{
				list.Remove(id);
			}
			this.UpdateParentTree(treeViewItemData);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00029B50 File Offset: 0x00027D50
		private void UpdateParentTree(TreeViewItemData<T> current)
		{
			for (;;)
			{
				int num;
				bool flag = this.m_ParentIds.TryGetValue(current.id, out num);
				if (!flag)
				{
					break;
				}
				TreeViewItemData<T> treeViewItemData = this.m_Tree[num];
				treeViewItemData.ReplaceChild(current);
				this.m_Tree[num] = treeViewItemData;
				current = treeViewItemData;
			}
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00029BA4 File Offset: 0x00027DA4
		private bool TryRemoveChildrenIds(int id)
		{
			TreeViewItemData<T> treeViewItemData;
			bool flag = this.m_Tree.TryGetValue(id, out treeViewItemData) && treeViewItemData.children != null;
			if (flag)
			{
				foreach (TreeViewItemData<T> treeViewItemData2 in treeViewItemData.children)
				{
					this.TryRemoveChildrenIds(treeViewItemData2.id);
				}
			}
			List<int> list;
			bool flag2 = this.m_ChildrenIds.TryGetValue(id, out list);
			if (flag2)
			{
				CollectionPool<List<int>, int>.Release(list);
			}
			bool flag3 = false;
			flag3 |= this.m_RootItemIds.Remove(id);
			flag3 |= this.m_ChildrenIds.Remove(id);
			flag3 |= this.m_ParentIds.Remove(id);
			flag3 |= this.m_Tree.Remove(id);
			return flag3 | this.m_RootItemIds.Remove(id);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00029C98 File Offset: 0x00027E98
		private void RefreshTree(IList<TreeViewItemData<T>> rootItems)
		{
			this.m_Tree.Clear();
			this.m_ParentIds.Clear();
			this.m_ChildrenIds.Clear();
			this.m_RootItemIds.Clear();
			this.BuildTree(rootItems, true);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00029CD4 File Offset: 0x00027ED4
		private void BuildTree(IEnumerable<TreeViewItemData<T>> items, bool isRoot)
		{
			bool flag = items == null;
			if (!flag)
			{
				foreach (TreeViewItemData<T> treeViewItemData in items)
				{
					this.m_Tree.Add(treeViewItemData.id, treeViewItemData);
					if (isRoot)
					{
						this.m_RootItemIds.Add(treeViewItemData.id);
					}
					bool flag2 = treeViewItemData.children != null;
					if (flag2)
					{
						List<int> list;
						bool flag3 = !this.m_ChildrenIds.TryGetValue(treeViewItemData.id, out list);
						if (flag3)
						{
							this.m_ChildrenIds.Add(treeViewItemData.id, list = CollectionPool<List<int>, int>.Get());
						}
						foreach (TreeViewItemData<T> treeViewItemData2 in treeViewItemData.children)
						{
							this.m_ParentIds.Add(treeViewItemData2.id, treeViewItemData.id);
							list.Add(treeViewItemData2.id);
						}
						this.BuildTree(treeViewItemData.children, false);
					}
				}
			}
		}

		// Token: 0x040004FB RID: 1275
		private readonly IList<int> m_RootItemIds;

		// Token: 0x040004FC RID: 1276
		private readonly Dictionary<int, TreeViewItemData<T>> m_Tree;

		// Token: 0x040004FD RID: 1277
		private readonly Dictionary<int, int> m_ParentIds;

		// Token: 0x040004FE RID: 1278
		private readonly Dictionary<int, List<int>> m_ChildrenIds;
	}
}
