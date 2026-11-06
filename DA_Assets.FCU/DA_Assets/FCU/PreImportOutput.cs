using System;
using System.Collections.Generic;
using DA_Assets.FCU.Model;

namespace DA_Assets.FCU
{
	// Token: 0x02000040 RID: 64
	public struct PreImportOutput
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00003DB4 File Offset: 0x00001FB4
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00003DBC File Offset: 0x00001FBC
		public IEnumerable<string> ToImport { readonly get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00003DC5 File Offset: 0x00001FC5
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00003DCD File Offset: 0x00001FCD
		public IEnumerable<SyncData> ToRemove { readonly get; set; }
	}
}
