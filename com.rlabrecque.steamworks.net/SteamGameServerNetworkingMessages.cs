using System;

namespace Steamworks
{
	// Token: 0x0200000B RID: 11
	public static class SteamGameServerNetworkingMessages
	{
		// Token: 0x06000132 RID: 306 RVA: 0x00004C0F File Offset: 0x00002E0F
		public static EResult SendMessageToUser(ref SteamNetworkingIdentity identityRemote, IntPtr pubData, uint cubData, int nSendFlags, int nRemoteChannel)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingMessages_SendMessageToUser(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), ref identityRemote, pubData, cubData, nSendFlags, nRemoteChannel);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00004C26 File Offset: 0x00002E26
		public static int ReceiveMessagesOnChannel(int nLocalChannel, IntPtr[] ppOutMessages, int nMaxMessages)
		{
			InteropHelp.TestIfAvailableGameServer();
			if (ppOutMessages != null && ppOutMessages.Length != nMaxMessages)
			{
				throw new ArgumentException("ppOutMessages must be the same size as nMaxMessages!");
			}
			return NativeMethods.ISteamNetworkingMessages_ReceiveMessagesOnChannel(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), nLocalChannel, ppOutMessages, nMaxMessages);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00004C4E File Offset: 0x00002E4E
		public static bool AcceptSessionWithUser(ref SteamNetworkingIdentity identityRemote)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingMessages_AcceptSessionWithUser(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), ref identityRemote);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00004C60 File Offset: 0x00002E60
		public static bool CloseSessionWithUser(ref SteamNetworkingIdentity identityRemote)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingMessages_CloseSessionWithUser(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), ref identityRemote);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00004C72 File Offset: 0x00002E72
		public static bool CloseChannelWithUser(ref SteamNetworkingIdentity identityRemote, int nLocalChannel)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingMessages_CloseChannelWithUser(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), ref identityRemote, nLocalChannel);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00004C85 File Offset: 0x00002E85
		public static ESteamNetworkingConnectionState GetSessionConnectionInfo(ref SteamNetworkingIdentity identityRemote, out SteamNetConnectionInfo_t pConnectionInfo, out SteamNetConnectionRealTimeStatus_t pQuickStatus)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamNetworkingMessages_GetSessionConnectionInfo(CSteamGameServerAPIContext.GetSteamNetworkingMessages(), ref identityRemote, out pConnectionInfo, out pQuickStatus);
		}
	}
}
