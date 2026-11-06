using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C4 RID: 196
	[CallbackIdentity(3420)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct WorkshopEULAStatus_t
	{
		// Token: 0x04000268 RID: 616
		public const int k_iCallback = 3420;

		// Token: 0x04000269 RID: 617
		public EResult m_eResult;

		// Token: 0x0400026A RID: 618
		public AppId_t m_nAppID;

		// Token: 0x0400026B RID: 619
		public uint m_unVersion;

		// Token: 0x0400026C RID: 620
		public RTime32 m_rtAction;

		// Token: 0x0400026D RID: 621
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAccepted;

		// Token: 0x0400026E RID: 622
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bNeedsAction;
	}
}
