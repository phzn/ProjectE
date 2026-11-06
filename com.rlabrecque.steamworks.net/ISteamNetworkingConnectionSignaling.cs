using System;

namespace Steamworks
{
	// Token: 0x020001A2 RID: 418
	[Serializable]
	public struct ISteamNetworkingConnectionSignaling
	{
		// Token: 0x06000A52 RID: 2642 RVA: 0x0000FE75 File Offset: 0x0000E075
		public bool SendSignal(HSteamNetConnection hConn, ref SteamNetConnectionInfo_t info, IntPtr pMsg, int cbMsg)
		{
			return NativeMethods.SteamAPI_ISteamNetworkingConnectionSignaling_SendSignal(ref this, hConn, ref info, pMsg, cbMsg);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0000FE82 File Offset: 0x0000E082
		public void Release()
		{
			NativeMethods.SteamAPI_ISteamNetworkingConnectionSignaling_Release(ref this);
		}
	}
}
