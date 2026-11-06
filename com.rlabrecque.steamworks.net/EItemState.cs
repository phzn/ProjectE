using System;

namespace Steamworks
{
	// Token: 0x0200012C RID: 300
	[Flags]
	public enum EItemState
	{
		// Token: 0x040007E7 RID: 2023
		k_EItemStateNone = 0,
		// Token: 0x040007E8 RID: 2024
		k_EItemStateSubscribed = 1,
		// Token: 0x040007E9 RID: 2025
		k_EItemStateLegacyItem = 2,
		// Token: 0x040007EA RID: 2026
		k_EItemStateInstalled = 4,
		// Token: 0x040007EB RID: 2027
		k_EItemStateNeedsUpdate = 8,
		// Token: 0x040007EC RID: 2028
		k_EItemStateDownloading = 16,
		// Token: 0x040007ED RID: 2029
		k_EItemStateDownloadPending = 32,
		// Token: 0x040007EE RID: 2030
		k_EItemStateDisabledLocally = 64
	}
}
