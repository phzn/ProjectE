using System;

namespace Steamworks
{
	// Token: 0x02000019 RID: 25
	public static class SteamNetworking
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x0000857B File Offset: 0x0000677B
		public static bool SendP2PPacket(CSteamID steamIDRemote, byte[] pubData, uint cubData, EP2PSend eP2PSendType, int nChannel = 0)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_SendP2PPacket(CSteamAPIContext.GetSteamNetworking(), steamIDRemote, pubData, cubData, eP2PSendType, nChannel);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00008592 File Offset: 0x00006792
		public static bool IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel = 0)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_IsP2PPacketAvailable(CSteamAPIContext.GetSteamNetworking(), out pcubMsgSize, nChannel);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x000085A5 File Offset: 0x000067A5
		public static bool ReadP2PPacket(byte[] pubDest, uint cubDest, out uint pcubMsgSize, out CSteamID psteamIDRemote, int nChannel = 0)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_ReadP2PPacket(CSteamAPIContext.GetSteamNetworking(), pubDest, cubDest, out pcubMsgSize, out psteamIDRemote, nChannel);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x000085BC File Offset: 0x000067BC
		public static bool AcceptP2PSessionWithUser(CSteamID steamIDRemote)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_AcceptP2PSessionWithUser(CSteamAPIContext.GetSteamNetworking(), steamIDRemote);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x000085CE File Offset: 0x000067CE
		public static bool CloseP2PSessionWithUser(CSteamID steamIDRemote)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_CloseP2PSessionWithUser(CSteamAPIContext.GetSteamNetworking(), steamIDRemote);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000085E0 File Offset: 0x000067E0
		public static bool CloseP2PChannelWithUser(CSteamID steamIDRemote, int nChannel)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_CloseP2PChannelWithUser(CSteamAPIContext.GetSteamNetworking(), steamIDRemote, nChannel);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x000085F3 File Offset: 0x000067F3
		public static bool GetP2PSessionState(CSteamID steamIDRemote, out P2PSessionState_t pConnectionState)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_GetP2PSessionState(CSteamAPIContext.GetSteamNetworking(), steamIDRemote, out pConnectionState);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00008606 File Offset: 0x00006806
		public static bool AllowP2PPacketRelay(bool bAllow)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_AllowP2PPacketRelay(CSteamAPIContext.GetSteamNetworking(), bAllow);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00008618 File Offset: 0x00006818
		public static SNetListenSocket_t CreateListenSocket(int nVirtualP2PPort, SteamIPAddress_t nIP, ushort nPort, bool bAllowUseOfPacketRelay)
		{
			InteropHelp.TestIfAvailableClient();
			return (SNetListenSocket_t)NativeMethods.ISteamNetworking_CreateListenSocket(CSteamAPIContext.GetSteamNetworking(), nVirtualP2PPort, nIP, nPort, bAllowUseOfPacketRelay);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00008632 File Offset: 0x00006832
		public static SNetSocket_t CreateP2PConnectionSocket(CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, bool bAllowUseOfPacketRelay)
		{
			InteropHelp.TestIfAvailableClient();
			return (SNetSocket_t)NativeMethods.ISteamNetworking_CreateP2PConnectionSocket(CSteamAPIContext.GetSteamNetworking(), steamIDTarget, nVirtualPort, nTimeoutSec, bAllowUseOfPacketRelay);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000864C File Offset: 0x0000684C
		public static SNetSocket_t CreateConnectionSocket(SteamIPAddress_t nIP, ushort nPort, int nTimeoutSec)
		{
			InteropHelp.TestIfAvailableClient();
			return (SNetSocket_t)NativeMethods.ISteamNetworking_CreateConnectionSocket(CSteamAPIContext.GetSteamNetworking(), nIP, nPort, nTimeoutSec);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00008665 File Offset: 0x00006865
		public static bool DestroySocket(SNetSocket_t hSocket, bool bNotifyRemoteEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_DestroySocket(CSteamAPIContext.GetSteamNetworking(), hSocket, bNotifyRemoteEnd);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00008678 File Offset: 0x00006878
		public static bool DestroyListenSocket(SNetListenSocket_t hSocket, bool bNotifyRemoteEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_DestroyListenSocket(CSteamAPIContext.GetSteamNetworking(), hSocket, bNotifyRemoteEnd);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000868B File Offset: 0x0000688B
		public static bool SendDataOnSocket(SNetSocket_t hSocket, byte[] pubData, uint cubData, bool bReliable)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_SendDataOnSocket(CSteamAPIContext.GetSteamNetworking(), hSocket, pubData, cubData, bReliable);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000086A0 File Offset: 0x000068A0
		public static bool IsDataAvailableOnSocket(SNetSocket_t hSocket, out uint pcubMsgSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_IsDataAvailableOnSocket(CSteamAPIContext.GetSteamNetworking(), hSocket, out pcubMsgSize);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000086B3 File Offset: 0x000068B3
		public static bool RetrieveDataFromSocket(SNetSocket_t hSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_RetrieveDataFromSocket(CSteamAPIContext.GetSteamNetworking(), hSocket, pubDest, cubDest, out pcubMsgSize);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000086C8 File Offset: 0x000068C8
		public static bool IsDataAvailable(SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out SNetSocket_t phSocket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_IsDataAvailable(CSteamAPIContext.GetSteamNetworking(), hListenSocket, out pcubMsgSize, out phSocket);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000086DC File Offset: 0x000068DC
		public static bool RetrieveData(SNetListenSocket_t hListenSocket, byte[] pubDest, uint cubDest, out uint pcubMsgSize, out SNetSocket_t phSocket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_RetrieveData(CSteamAPIContext.GetSteamNetworking(), hListenSocket, pubDest, cubDest, out pcubMsgSize, out phSocket);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000086F3 File Offset: 0x000068F3
		public static bool GetSocketInfo(SNetSocket_t hSocket, out CSteamID pSteamIDRemote, out int peSocketStatus, out SteamIPAddress_t punIPRemote, out ushort punPortRemote)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_GetSocketInfo(CSteamAPIContext.GetSteamNetworking(), hSocket, out pSteamIDRemote, out peSocketStatus, out punIPRemote, out punPortRemote);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000870A File Offset: 0x0000690A
		public static bool GetListenSocketInfo(SNetListenSocket_t hListenSocket, out SteamIPAddress_t pnIP, out ushort pnPort)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_GetListenSocketInfo(CSteamAPIContext.GetSteamNetworking(), hListenSocket, out pnIP, out pnPort);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000871E File Offset: 0x0000691E
		public static ESNetSocketConnectionType GetSocketConnectionType(SNetSocket_t hSocket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_GetSocketConnectionType(CSteamAPIContext.GetSteamNetworking(), hSocket);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00008730 File Offset: 0x00006930
		public static int GetMaxPacketSize(SNetSocket_t hSocket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamNetworking_GetMaxPacketSize(CSteamAPIContext.GetSteamNetworking(), hSocket);
		}
	}
}
