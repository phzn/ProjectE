using System;
using System.Collections.Generic;
using DA_Assets.FCU.Model;

namespace DA_Assets.FCU
{
	// Token: 0x0200003D RID: 61
	public struct GroupedSyncHelpers
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00003CC6 File Offset: 0x00001EC6
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00003CCE File Offset: 0x00001ECE
		public SyncData RootFrame { readonly get; set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00003CD7 File Offset: 0x00001ED7
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00003CDF File Offset: 0x00001EDF
		public List<SyncHelper> SyncHelpers { readonly get; set; }
	}
}
