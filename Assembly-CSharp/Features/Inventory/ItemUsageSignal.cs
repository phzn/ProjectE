using System;
using System.Collections.Generic;

namespace Features.Inventory
{
	// Token: 0x020010FC RID: 4348
	public static class ItemUsageSignal
	{
		// Token: 0x170014DE RID: 5342
		// (get) Token: 0x06008386 RID: 33670 RVA: 0x0035A8F4 File Offset: 0x00358AF4
		public static bool Active
		{
			get
			{
				return ItemUsageSignal._users.Count > 0;
			}
		}

		// Token: 0x06008387 RID: 33671 RVA: 0x0035A903 File Offset: 0x00358B03
		public static void Set(object owner, bool active)
		{
			if (owner == null)
			{
				return;
			}
			if (active)
			{
				ItemUsageSignal._users.Add(owner);
				return;
			}
			ItemUsageSignal._users.Remove(owner);
		}

		// Token: 0x040086A0 RID: 34464
		private static readonly HashSet<object> _users = new HashSet<object>();
	}
}
