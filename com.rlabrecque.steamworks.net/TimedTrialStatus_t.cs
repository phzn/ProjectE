using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200002C RID: 44
	[CallbackIdentity(1030)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct TimedTrialStatus_t
	{
		// Token: 0x04000010 RID: 16
		public const int k_iCallback = 1030;

		// Token: 0x04000011 RID: 17
		public AppId_t m_unAppID;

		// Token: 0x04000012 RID: 18
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bIsOffline;

		// Token: 0x04000013 RID: 19
		public uint m_unSecondsAllowed;

		// Token: 0x04000014 RID: 20
		public uint m_unSecondsPlayed;
	}
}
