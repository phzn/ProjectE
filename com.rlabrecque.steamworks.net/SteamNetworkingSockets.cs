using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200001B RID: 27
	public static class SteamNetworkingSockets
	{
		// Token: 0x06000310 RID: 784 RVA: 0x000087CC File Offset: 0x000069CC
		public static HSteamListenSocket CreateListenSocketIP(ref SteamNetworkingIPAddr localAddress, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamListenSocket)NativeMethods.ISteamNetworkingSockets_CreateListenSocketIP(CSteamAPIContext.GetSteamNetworkingSockets(), ref localAddress, nOptions, pOptions);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000087E5 File Offset: 0x000069E5
		public static HSteamNetConnection ConnectByIPAddress(ref SteamNetworkingIPAddr address, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamNetConnection)NativeMethods.ISteamNetworkingSockets_ConnectByIPAddress(CSteamAPIContext.GetSteamNetworkingSockets(), ref address, nOptions, pOptions);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000087FE File Offset: 0x000069FE
		public static HSteamListenSocket CreateListenSocketP2P(int nLocalVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamListenSocket)NativeMethods.ISteamNetworkingSockets_CreateListenSocketP2P(CSteamAPIContext.GetSteamNetworkingSockets(), nLocalVirtualPort, nOptions, pOptions);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00008817 File Offset: 0x00006A17
		public static HSteamNetConnection ConnectP2P(ref SteamNetworkingIdentity identityRemote, int nRemoteVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamNetConnection)NativeMethods.ISteamNetworkingSockets_ConnectP2P(CSteamAPIContext.GetSteamNetworkingSockets(), ref identityRemote, nRemoteVirtualPort, nOptions, pOptions);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00008831 File Offset: 0x00006A31
		public static EResult AcceptConnection(HSteamNetConnection hConn)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_AcceptConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hConn);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00008844 File Offset: 0x00006A44
		public static bool CloseConnection(HSteamNetConnection hPeer, int nReason, string pszDebug, bool bEnableLinger)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszDebug))
			{
				flag = NativeMethods.ISteamNetworkingSockets_CloseConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hPeer, nReason, utf8StringHandle, bEnableLinger);
			}
			return flag;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000888C File Offset: 0x00006A8C
		public static bool CloseListenSocket(HSteamListenSocket hSocket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_CloseListenSocket(CSteamAPIContext.GetSteamNetworkingSockets(), hSocket);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000889E File Offset: 0x00006A9E
		public static bool SetConnectionUserData(HSteamNetConnection hPeer, long nUserData)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_SetConnectionUserData(CSteamAPIContext.GetSteamNetworkingSockets(), hPeer, nUserData);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000088B1 File Offset: 0x00006AB1
		public static long GetConnectionUserData(HSteamNetConnection hPeer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetConnectionUserData(CSteamAPIContext.GetSteamNetworkingSockets(), hPeer);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000088C4 File Offset: 0x00006AC4
		public static void SetConnectionName(HSteamNetConnection hPeer, string pszName)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszName))
			{
				NativeMethods.ISteamNetworkingSockets_SetConnectionName(CSteamAPIContext.GetSteamNetworkingSockets(), hPeer, utf8StringHandle);
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00008908 File Offset: 0x00006B08
		public static bool GetConnectionName(HSteamNetConnection hPeer, out string pszName, int nMaxLen)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(nMaxLen);
			bool flag = NativeMethods.ISteamNetworkingSockets_GetConnectionName(CSteamAPIContext.GetSteamNetworkingSockets(), hPeer, intPtr, nMaxLen);
			pszName = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00008944 File Offset: 0x00006B44
		public static EResult SendMessageToConnection(HSteamNetConnection hConn, IntPtr pData, uint cbData, int nSendFlags, out long pOutMessageNumber)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_SendMessageToConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, pData, cbData, nSendFlags, out pOutMessageNumber);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000895B File Offset: 0x00006B5B
		public static void SendMessages(int nMessages, IntPtr[] pMessages, long[] pOutMessageNumberOrResult)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamNetworkingSockets_SendMessages(CSteamAPIContext.GetSteamNetworkingSockets(), nMessages, pMessages, pOutMessageNumberOrResult);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000896F File Offset: 0x00006B6F
		public static EResult FlushMessagesOnConnection(HSteamNetConnection hConn)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_FlushMessagesOnConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hConn);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00008981 File Offset: 0x00006B81
		public static int ReceiveMessagesOnConnection(HSteamNetConnection hConn, IntPtr[] ppOutMessages, int nMaxMessages)
		{
			InteropHelp.TestIfAvailableClient();
			if (ppOutMessages != null && ppOutMessages.Length != nMaxMessages)
			{
				throw new ArgumentException("ppOutMessages must be the same size as nMaxMessages!");
			}
			return NativeMethods.ISteamNetworkingSockets_ReceiveMessagesOnConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, ppOutMessages, nMaxMessages);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000089A9 File Offset: 0x00006BA9
		public static bool GetConnectionInfo(HSteamNetConnection hConn, out SteamNetConnectionInfo_t pInfo)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetConnectionInfo(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, out pInfo);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x000089BC File Offset: 0x00006BBC
		public static EResult GetConnectionRealTimeStatus(HSteamNetConnection hConn, ref SteamNetConnectionRealTimeStatus_t pStatus, int nLanes, ref SteamNetConnectionRealTimeLaneStatus_t pLanes)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetConnectionRealTimeStatus(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, ref pStatus, nLanes, ref pLanes);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000089D4 File Offset: 0x00006BD4
		public static int GetDetailedConnectionStatus(HSteamNetConnection hConn, out string pszBuf, int cbBuf)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cbBuf);
			int num = NativeMethods.ISteamNetworkingSockets_GetDetailedConnectionStatus(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, intPtr, cbBuf);
			pszBuf = ((num != -1) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00008A11 File Offset: 0x00006C11
		public static bool GetListenSocketAddress(HSteamListenSocket hSocket, out SteamNetworkingIPAddr address)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetListenSocketAddress(CSteamAPIContext.GetSteamNetworkingSockets(), hSocket, out address);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00008A24 File Offset: 0x00006C24
		public static bool CreateSocketPair(out HSteamNetConnection pOutConnection1, out HSteamNetConnection pOutConnection2, bool bUseNetworkLoopback, ref SteamNetworkingIdentity pIdentity1, ref SteamNetworkingIdentity pIdentity2)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_CreateSocketPair(CSteamAPIContext.GetSteamNetworkingSockets(), out pOutConnection1, out pOutConnection2, bUseNetworkLoopback, ref pIdentity1, ref pIdentity2);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00008A3B File Offset: 0x00006C3B
		public static EResult ConfigureConnectionLanes(HSteamNetConnection hConn, int nNumLanes, int[] pLanePriorities, ushort[] pLaneWeights)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_ConfigureConnectionLanes(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, nNumLanes, pLanePriorities, pLaneWeights);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00008A50 File Offset: 0x00006C50
		public static bool GetIdentity(out SteamNetworkingIdentity pIdentity)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetIdentity(CSteamAPIContext.GetSteamNetworkingSockets(), out pIdentity);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00008A62 File Offset: 0x00006C62
		public static ESteamNetworkingAvailability InitAuthentication()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_InitAuthentication(CSteamAPIContext.GetSteamNetworkingSockets());
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00008A73 File Offset: 0x00006C73
		public static ESteamNetworkingAvailability GetAuthenticationStatus(out SteamNetAuthenticationStatus_t pDetails)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetAuthenticationStatus(CSteamAPIContext.GetSteamNetworkingSockets(), out pDetails);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00008A85 File Offset: 0x00006C85
		public static HSteamNetPollGroup CreatePollGroup()
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamNetPollGroup)NativeMethods.ISteamNetworkingSockets_CreatePollGroup(CSteamAPIContext.GetSteamNetworkingSockets());
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00008A9B File Offset: 0x00006C9B
		public static bool DestroyPollGroup(HSteamNetPollGroup hPollGroup)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_DestroyPollGroup(CSteamAPIContext.GetSteamNetworkingSockets(), hPollGroup);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00008AAD File Offset: 0x00006CAD
		public static bool SetConnectionPollGroup(HSteamNetConnection hConn, HSteamNetPollGroup hPollGroup)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_SetConnectionPollGroup(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, hPollGroup);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00008AC0 File Offset: 0x00006CC0
		public static int ReceiveMessagesOnPollGroup(HSteamNetPollGroup hPollGroup, IntPtr[] ppOutMessages, int nMaxMessages)
		{
			InteropHelp.TestIfAvailableClient();
			if (ppOutMessages != null && ppOutMessages.Length != nMaxMessages)
			{
				throw new ArgumentException("ppOutMessages must be the same size as nMaxMessages!");
			}
			return NativeMethods.ISteamNetworkingSockets_ReceiveMessagesOnPollGroup(CSteamAPIContext.GetSteamNetworkingSockets(), hPollGroup, ppOutMessages, nMaxMessages);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00008AE8 File Offset: 0x00006CE8
		public static bool ReceivedRelayAuthTicket(IntPtr pvTicket, int cbTicket, out SteamDatagramRelayAuthTicket pOutParsedTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_ReceivedRelayAuthTicket(CSteamAPIContext.GetSteamNetworkingSockets(), pvTicket, cbTicket, out pOutParsedTicket);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00008AFC File Offset: 0x00006CFC
		public static int FindRelayAuthTicketForServer(ref SteamNetworkingIdentity identityGameServer, int nRemoteVirtualPort, out SteamDatagramRelayAuthTicket pOutParsedTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_FindRelayAuthTicketForServer(CSteamAPIContext.GetSteamNetworkingSockets(), ref identityGameServer, nRemoteVirtualPort, out pOutParsedTicket);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00008B10 File Offset: 0x00006D10
		public static HSteamNetConnection ConnectToHostedDedicatedServer(ref SteamNetworkingIdentity identityTarget, int nRemoteVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamNetConnection)NativeMethods.ISteamNetworkingSockets_ConnectToHostedDedicatedServer(CSteamAPIContext.GetSteamNetworkingSockets(), ref identityTarget, nRemoteVirtualPort, nOptions, pOptions);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00008B2A File Offset: 0x00006D2A
		public static ushort GetHostedDedicatedServerPort()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetHostedDedicatedServerPort(CSteamAPIContext.GetSteamNetworkingSockets());
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00008B3B File Offset: 0x00006D3B
		public static SteamNetworkingPOPID GetHostedDedicatedServerPOPID()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamNetworkingPOPID)NativeMethods.ISteamNetworkingSockets_GetHostedDedicatedServerPOPID(CSteamAPIContext.GetSteamNetworkingSockets());
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00008B51 File Offset: 0x00006D51
		public static EResult GetHostedDedicatedServerAddress(out SteamDatagramHostedAddress pRouting)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetHostedDedicatedServerAddress(CSteamAPIContext.GetSteamNetworkingSockets(), out pRouting);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00008B63 File Offset: 0x00006D63
		public static HSteamListenSocket CreateHostedDedicatedServerListenSocket(int nLocalVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamListenSocket)NativeMethods.ISteamNetworkingSockets_CreateHostedDedicatedServerListenSocket(CSteamAPIContext.GetSteamNetworkingSockets(), nLocalVirtualPort, nOptions, pOptions);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00008B7C File Offset: 0x00006D7C
		public static EResult GetGameCoordinatorServerLogin(IntPtr pLoginInfo, out int pcbSignedBlob, IntPtr pBlob)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetGameCoordinatorServerLogin(CSteamAPIContext.GetSteamNetworkingSockets(), pLoginInfo, out pcbSignedBlob, pBlob);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00008B90 File Offset: 0x00006D90
		public static HSteamNetConnection ConnectP2PCustomSignaling(out ISteamNetworkingConnectionSignaling pSignaling, ref SteamNetworkingIdentity pPeerIdentity, int nRemoteVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamNetConnection)NativeMethods.ISteamNetworkingSockets_ConnectP2PCustomSignaling(CSteamAPIContext.GetSteamNetworkingSockets(), out pSignaling, ref pPeerIdentity, nRemoteVirtualPort, nOptions, pOptions);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00008BAC File Offset: 0x00006DAC
		public static bool ReceivedP2PCustomSignal(IntPtr pMsg, int cbMsg, out ISteamNetworkingSignalingRecvContext pContext)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_ReceivedP2PCustomSignal(CSteamAPIContext.GetSteamNetworkingSockets(), pMsg, cbMsg, out pContext);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00008BC0 File Offset: 0x00006DC0
		public static bool GetCertificateRequest(out int pcbBlob, IntPtr pBlob, out SteamNetworkingErrMsg errMsg)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetCertificateRequest(CSteamAPIContext.GetSteamNetworkingSockets(), out pcbBlob, pBlob, out errMsg);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00008BD4 File Offset: 0x00006DD4
		public static bool SetCertificate(IntPtr pCertificate, int cbCertificate, out SteamNetworkingErrMsg errMsg)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_SetCertificate(CSteamAPIContext.GetSteamNetworkingSockets(), pCertificate, cbCertificate, out errMsg);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00008BE8 File Offset: 0x00006DE8
		public static void ResetIdentity(ref SteamNetworkingIdentity pIdentity)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamNetworkingSockets_ResetIdentity(CSteamAPIContext.GetSteamNetworkingSockets(), ref pIdentity);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00008BFA File Offset: 0x00006DFA
		public static void RunCallbacks()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamNetworkingSockets_RunCallbacks(CSteamAPIContext.GetSteamNetworkingSockets());
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00008C0B File Offset: 0x00006E0B
		public static bool BeginAsyncRequestFakeIP(int nNumPorts)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_BeginAsyncRequestFakeIP(CSteamAPIContext.GetSteamNetworkingSockets(), nNumPorts);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00008C1D File Offset: 0x00006E1D
		public static void GetFakeIP(int idxFirstPort, out SteamNetworkingFakeIPResult_t pInfo)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamNetworkingSockets_GetFakeIP(CSteamAPIContext.GetSteamNetworkingSockets(), idxFirstPort, out pInfo);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00008C30 File Offset: 0x00006E30
		public static HSteamListenSocket CreateListenSocketP2PFakeIP(int idxFakePort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamListenSocket)NativeMethods.ISteamNetworkingSockets_CreateListenSocketP2PFakeIP(CSteamAPIContext.GetSteamNetworkingSockets(), idxFakePort, nOptions, pOptions);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00008C49 File Offset: 0x00006E49
		public static EResult GetRemoteFakeIPForConnection(HSteamNetConnection hConn, out SteamNetworkingIPAddr pOutAddr)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_GetRemoteFakeIPForConnection(CSteamAPIContext.GetSteamNetworkingSockets(), hConn, out pOutAddr);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00008C5C File Offset: 0x00006E5C
		public static IntPtr CreateFakeUDPPort(int idxFakeServerPort)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworkingSockets_CreateFakeUDPPort(CSteamAPIContext.GetSteamNetworkingSockets(), idxFakeServerPort);
		}
	}
}
