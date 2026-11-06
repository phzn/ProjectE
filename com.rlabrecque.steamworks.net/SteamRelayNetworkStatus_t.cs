using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200008F RID: 143
	[CallbackIdentity(1281)]
	public struct SteamRelayNetworkStatus_t
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x0000C26F File Offset: 0x0000A46F
		// (set) Token: 0x06000865 RID: 2149 RVA: 0x0000C27C File Offset: 0x0000A47C
		public string m_debugMsg
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_debugMsg_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_debugMsg_, 256);
			}
		}

		// Token: 0x04000186 RID: 390
		public const int k_iCallback = 1281;

		// Token: 0x04000187 RID: 391
		public ESteamNetworkingAvailability m_eAvail;

		// Token: 0x04000188 RID: 392
		public int m_bPingMeasurementInProgress;

		// Token: 0x04000189 RID: 393
		public ESteamNetworkingAvailability m_eAvailNetworkConfig;

		// Token: 0x0400018A RID: 394
		public ESteamNetworkingAvailability m_eAvailAnyRelay;

		// Token: 0x0400018B RID: 395
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_debugMsg_;
	}
}
