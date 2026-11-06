using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000072 RID: 114
	[CallbackIdentity(4703)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryEligiblePromoItemDefIDs_t
	{
		// Token: 0x04000124 RID: 292
		public const int k_iCallback = 4703;

		// Token: 0x04000125 RID: 293
		public EResult m_result;

		// Token: 0x04000126 RID: 294
		public CSteamID m_steamID;

		// Token: 0x04000127 RID: 295
		public int m_numEligiblePromoItemDefs;

		// Token: 0x04000128 RID: 296
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
