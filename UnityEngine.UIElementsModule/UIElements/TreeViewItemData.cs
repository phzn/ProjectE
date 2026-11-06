using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000141 RID: 321
	public readonly struct TreeViewItemData<T>
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x00029EC5 File Offset: 0x000280C5
		public int id { get; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x00029ECD File Offset: 0x000280CD
		public T data
		{
			get
			{
				return this.m_Data;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x00029ED5 File Offset: 0x000280D5
		public IEnumerable<TreeViewItemData<T>> children
		{
			get
			{
				return this.m_Children;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x00029EDD File Offset: 0x000280DD
		public bool hasChildren
		{
			get
			{
				return this.m_Children != null && this.m_Children.Count > 0;
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00029EF8 File Offset: 0x000280F8
		public TreeViewItemData(int id, T data, List<TreeViewItemData<T>> children = null)
		{
			this.id = id;
			this.m_Data = data;
			this.m_Children = children ?? new List<TreeViewItemData<T>>();
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00029F19 File Offset: 0x00028119
		internal void AddChild(TreeViewItemData<T> child)
		{
			this.m_Children.Add(child);
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00029F2C File Offset: 0x0002812C
		internal void AddChildren(IList<TreeViewItemData<T>> children)
		{
			foreach (TreeViewItemData<T> treeViewItemData in children)
			{
				this.AddChild(treeViewItemData);
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00029F78 File Offset: 0x00028178
		internal void InsertChild(TreeViewItemData<T> child, int index)
		{
			bool flag = index < 0 || index >= this.m_Children.Count;
			if (flag)
			{
				this.m_Children.Add(child);
			}
			else
			{
				this.m_Children.Insert(index, child);
			}
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00029FC0 File Offset: 0x000281C0
		internal void RemoveChild(int childId)
		{
			bool flag = this.m_Children == null;
			if (!flag)
			{
				for (int i = 0; i < this.m_Children.Count; i++)
				{
					bool flag2 = childId == this.m_Children[i].id;
					if (flag2)
					{
						this.m_Children.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0002A028 File Offset: 0x00028228
		internal int GetChildIndex(int itemId)
		{
			int num = 0;
			foreach (TreeViewItemData<T> treeViewItemData in this.m_Children)
			{
				bool flag = treeViewItemData.id == itemId;
				if (flag)
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0002A094 File Offset: 0x00028294
		internal void ReplaceChild(TreeViewItemData<T> newChild)
		{
			bool flag = !this.hasChildren;
			if (!flag)
			{
				int num = 0;
				foreach (TreeViewItemData<T> treeViewItemData in this.m_Children)
				{
					bool flag2 = treeViewItemData.id == newChild.id;
					if (flag2)
					{
						this.m_Children.RemoveAt(num);
						this.m_Children.Insert(num, newChild);
						break;
					}
					num++;
				}
			}
		}

		// Token: 0x04000506 RID: 1286
		private readonly T m_Data;

		// Token: 0x04000507 RID: 1287
		private readonly IList<TreeViewItemData<T>> m_Children;
	}
}
