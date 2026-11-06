using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B1 RID: 177
	[CallbackIdentity(6002)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamTimelineEventRecordingExists_t
	{
		// Token: 0x0400021D RID: 541
		public const int k_iCallback = 6002;

		// Token: 0x0400021E RID: 542
		public ulong m_ulEventID;

		// Token: 0x0400021F RID: 543
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bRecordingExists;
	}
}
