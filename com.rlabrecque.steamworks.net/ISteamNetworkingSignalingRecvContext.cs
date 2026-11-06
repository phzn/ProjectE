using System;

namespace Steamworks
{
	// Token: 0x020001A3 RID: 419
	[Serializable]
	public struct ISteamNetworkingSignalingRecvContext
	{
		// Token: 0x06000A54 RID: 2644 RVA: 0x0000FE8A File Offset: 0x0000E08A
		public IntPtr OnConnectRequest(HSteamNetConnection hConn, ref SteamNetworkingIdentity identityPeer, int nLocalVirtualPort)
		{
			return NativeMethods.SteamAPI_ISteamNetworkingSignalingRecvContext_OnConnectRequest(ref this, hConn, ref identityPeer, nLocalVirtualPort);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0000FE95 File Offset: 0x0000E095
		public void SendRejectionSignal(ref SteamNetworkingIdentity identityPeer, IntPtr pMsg, int cbMsg)
		{
			NativeMethods.SteamAPI_ISteamNetworkingSignalingRecvContext_SendRejectionSignal(ref this, ref identityPeer, pMsg, cbMsg);
		}
	}
}
