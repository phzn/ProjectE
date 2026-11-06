using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000140 RID: 320
	internal readonly struct TreeViewItemWrapper
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x00029E80 File Offset: 0x00028080
		public int id
		{
			get
			{
				return this.item.id;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x00029E8D File Offset: 0x0002808D
		public int parentId
		{
			get
			{
				return this.item.parentId;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x00029E9A File Offset: 0x0002809A
		public IEnumerable<int> childrenIds
		{
			get
			{
				return this.item.childrenIds;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x00029EA7 File Offset: 0x000280A7
		public bool hasChildren
		{
			get
			{
				return this.item.hasChildren;
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00029EB4 File Offset: 0x000280B4
		public TreeViewItemWrapper(TreeItem item, int depth)
		{
			this.item = item;
			this.depth = depth;
		}

		// Token: 0x04000503 RID: 1283
		public readonly TreeItem item;

		// Token: 0x04000504 RID: 1284
		public readonly int depth;
	}
}
