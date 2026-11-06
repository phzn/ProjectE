using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x0200013F RID: 319
	internal readonly struct TreeItem
	{
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x00029E38 File Offset: 0x00028038
		public int id { get; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x00029E40 File Offset: 0x00028040
		public int parentId { get; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x00029E48 File Offset: 0x00028048
		public IEnumerable<int> childrenIds { get; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x00029E50 File Offset: 0x00028050
		public bool hasChildren
		{
			get
			{
				return this.childrenIds != null && this.childrenIds.Any<int>();
			}
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00029E68 File Offset: 0x00028068
		public TreeItem(int id, int parentId = -1, IEnumerable<int> childrenIds = null)
		{
			this.id = id;
			this.parentId = parentId;
			this.childrenIds = childrenIds;
		}

		// Token: 0x040004FF RID: 1279
		public const int invalidId = -1;
	}
}
