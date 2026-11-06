using System;
using DA_Assets.FCU.Model;

namespace DA_Assets.FCU
{
	// Token: 0x0200003F RID: 63
	public struct PreImportInput
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00003D92 File Offset: 0x00001F92
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00003D9A File Offset: 0x00001F9A
		public SelectableObject<DiffInfo> ToImport { readonly get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00003DA3 File Offset: 0x00001FA3
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00003DAB File Offset: 0x00001FAB
		public SelectableObject<SyncData> ToRemove { readonly get; set; }
	}
}
