using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000170 RID: 368
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetConnectionRealTimeLaneStatus_t
	{
		// Token: 0x04000AFA RID: 2810
		public int m_cbPendingUnreliable;

		// Token: 0x04000AFB RID: 2811
		public int m_cbPendingReliable;

		// Token: 0x04000AFC RID: 2812
		public int m_cbSentUnackedReliable;

		// Token: 0x04000AFD RID: 2813
		public int _reservePad1;

		// Token: 0x04000AFE RID: 2814
		public SteamNetworkingMicroseconds m_usecQueueTime;

		// Token: 0x04000AFF RID: 2815
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public uint[] reserved;
	}
}
