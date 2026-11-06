using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D1 RID: 209
	[CallbackIdentity(166)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MarketEligibilityResponse_t
	{
		// Token: 0x0400028F RID: 655
		public const int k_iCallback = 166;

		// Token: 0x04000290 RID: 656
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAllowed;

		// Token: 0x04000291 RID: 657
		public EMarketNotAllowedReasonFlags m_eNotAllowedReason;

		// Token: 0x04000292 RID: 658
		public RTime32 m_rtAllowedAtTime;

		// Token: 0x04000293 RID: 659
		public int m_cdaySteamGuardRequiredDays;

		// Token: 0x04000294 RID: 660
		public int m_cdayNewDeviceCooldown;
	}
}
