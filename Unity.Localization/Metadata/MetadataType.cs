using System;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000D6 RID: 214
	[Flags]
	public enum MetadataType
	{
		// Token: 0x040001FD RID: 509
		Locale = 1,
		// Token: 0x040001FE RID: 510
		SharedTableData = 2,
		// Token: 0x040001FF RID: 511
		StringTable = 4,
		// Token: 0x04000200 RID: 512
		AssetTable = 8,
		// Token: 0x04000201 RID: 513
		StringTableEntry = 16,
		// Token: 0x04000202 RID: 514
		AssetTableEntry = 32,
		// Token: 0x04000203 RID: 515
		SharedStringTableEntry = 64,
		// Token: 0x04000204 RID: 516
		SharedAssetTableEntry = 128,
		// Token: 0x04000205 RID: 517
		LocalizationSettings = 256,
		// Token: 0x04000206 RID: 518
		AllTables = 12,
		// Token: 0x04000207 RID: 519
		AllTableEntries = 48,
		// Token: 0x04000208 RID: 520
		AllSharedTableEntries = 192,
		// Token: 0x04000209 RID: 521
		All = 511
	}
}
