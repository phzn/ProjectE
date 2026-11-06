using System;
using System.Collections.Generic;

namespace Invector.vItemManager
{
	// Token: 0x02000570 RID: 1392
	[Serializable]
	public class vItemFilter
	{
		// Token: 0x06002121 RID: 8481 RVA: 0x000C199D File Offset: 0x000BFB9D
		public vItemFilter()
		{
			this.filter = new List<vItemType>();
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000C19B0 File Offset: 0x000BFBB0
		public vItemFilter(bool invertFilterResult = false, params vItemType[] itemTypesToFilter)
		{
			if (itemTypesToFilter != null && itemTypesToFilter.Length != 0)
			{
				this.filter = itemTypesToFilter.vToList<vItemType>();
			}
			else
			{
				this.filter = new List<vItemType>();
			}
			this.invertFilterResult = invertFilterResult;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000C19DF File Offset: 0x000BFBDF
		public bool Validate(vItem item)
		{
			if (item == null)
			{
				return false;
			}
			if (!this.invertFilterResult)
			{
				return this.filter.Contains(item.type);
			}
			return !this.filter.Contains(item.type);
		}

		// Token: 0x040021E1 RID: 8673
		public bool invertFilterResult;

		// Token: 0x040021E2 RID: 8674
		public List<vItemType> filter;
	}
}
