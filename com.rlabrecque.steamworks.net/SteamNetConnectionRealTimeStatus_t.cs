using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200016F RID: 367
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetConnectionRealTimeStatus_t
	{
		// Token: 0x04000AEB RID: 2795
		public ESteamNetworkingConnectionState m_eState;

		// Token: 0x04000AEC RID: 2796
		public int m_nPing;

		// Token: 0x04000AED RID: 2797
		public float m_flConnectionQualityLocal;

		// Token: 0x04000AEE RID: 2798
		public float m_flConnectionQualityRemote;

		// Token: 0x04000AEF RID: 2799
		public float m_flOutPacketsPerSec;

		// Token: 0x04000AF0 RID: 2800
		public float m_flOutBytesPerSec;

		// Token: 0x04000AF1 RID: 2801
		public float m_flInPacketsPerSec;

		// Token: 0x04000AF2 RID: 2802
		public float m_flInBytesPerSec;

		// Token: 0x04000AF3 RID: 2803
		public int m_nSendRateBytesPerSecond;

		// Token: 0x04000AF4 RID: 2804
		public int m_cbPendingUnreliable;

		// Token: 0x04000AF5 RID: 2805
		public int m_cbPendingReliable;

		// Token: 0x04000AF6 RID: 2806
		public int m_cbSentUnackedReliable;

		// Token: 0x04000AF7 RID: 2807
		public SteamNetworkingMicroseconds m_usecQueueTime;

		// Token: 0x04000AF8 RID: 2808
		public int m_usecMaxJitter;

		// Token: 0x04000AF9 RID: 2809
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public uint[] reserved;
	}
}
