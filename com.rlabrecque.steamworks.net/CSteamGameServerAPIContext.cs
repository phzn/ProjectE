using System;

namespace Steamworks
{
	// Token: 0x02000186 RID: 390
	internal static class CSteamGameServerAPIContext
	{
		// Token: 0x06000927 RID: 2343 RVA: 0x0000E79C File Offset: 0x0000C99C
		internal static void Clear()
		{
			CSteamGameServerAPIContext.m_pSteamClient = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamGameServer = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamUtils = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamNetworking = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamGameServerStats = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamHTTP = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamInventory = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamUGC = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamNetworkingUtils = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamNetworkingSockets = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamNetworkingMessages = IntPtr.Zero;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0000E818 File Offset: 0x0000CA18
		internal static bool Init()
		{
			HSteamUser hsteamUser = GameServer.GetHSteamUser();
			HSteamPipe hsteamPipe = GameServer.GetHSteamPipe();
			if (hsteamPipe == (HSteamPipe)0)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle("SteamClient023"))
			{
				CSteamGameServerAPIContext.m_pSteamClient = NativeMethods.SteamInternal_CreateInterface(utf8StringHandle);
			}
			if (CSteamGameServerAPIContext.m_pSteamClient == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamGameServer = SteamGameServerClient.GetISteamGameServer(hsteamUser, hsteamPipe, "SteamGameServer015");
			if (CSteamGameServerAPIContext.m_pSteamGameServer == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamUtils = SteamGameServerClient.GetISteamUtils(hsteamPipe, "SteamUtils010");
			if (CSteamGameServerAPIContext.m_pSteamUtils == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamNetworking = SteamGameServerClient.GetISteamNetworking(hsteamUser, hsteamPipe, "SteamNetworking006");
			if (CSteamGameServerAPIContext.m_pSteamNetworking == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamGameServerStats = SteamGameServerClient.GetISteamGameServerStats(hsteamUser, hsteamPipe, "SteamGameServerStats001");
			if (CSteamGameServerAPIContext.m_pSteamGameServerStats == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamHTTP = SteamGameServerClient.GetISteamHTTP(hsteamUser, hsteamPipe, "STEAMHTTP_INTERFACE_VERSION003");
			if (CSteamGameServerAPIContext.m_pSteamHTTP == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamInventory = SteamGameServerClient.GetISteamInventory(hsteamUser, hsteamPipe, "STEAMINVENTORY_INTERFACE_V003");
			if (CSteamGameServerAPIContext.m_pSteamInventory == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamUGC = SteamGameServerClient.GetISteamUGC(hsteamUser, hsteamPipe, "STEAMUGC_INTERFACE_VERSION021");
			if (CSteamGameServerAPIContext.m_pSteamUGC == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle("SteamNetworkingUtils004"))
			{
				CSteamGameServerAPIContext.m_pSteamNetworkingUtils = ((NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle2) != IntPtr.Zero) ? NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle2) : NativeMethods.SteamInternal_FindOrCreateGameServerInterface(hsteamUser, utf8StringHandle2));
			}
			if (CSteamGameServerAPIContext.m_pSteamNetworkingUtils == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle("SteamNetworkingSockets012"))
			{
				CSteamGameServerAPIContext.m_pSteamNetworkingSockets = NativeMethods.SteamInternal_FindOrCreateGameServerInterface(hsteamUser, utf8StringHandle3);
			}
			if (CSteamGameServerAPIContext.m_pSteamNetworkingSockets == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle("SteamNetworkingMessages002"))
			{
				CSteamGameServerAPIContext.m_pSteamNetworkingMessages = NativeMethods.SteamInternal_FindOrCreateGameServerInterface(hsteamUser, utf8StringHandle4);
			}
			return !(CSteamGameServerAPIContext.m_pSteamNetworkingMessages == IntPtr.Zero);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0000EA6C File Offset: 0x0000CC6C
		internal static IntPtr GetSteamClient()
		{
			return CSteamGameServerAPIContext.m_pSteamClient;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0000EA73 File Offset: 0x0000CC73
		internal static IntPtr GetSteamGameServer()
		{
			return CSteamGameServerAPIContext.m_pSteamGameServer;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0000EA7A File Offset: 0x0000CC7A
		internal static IntPtr GetSteamUtils()
		{
			return CSteamGameServerAPIContext.m_pSteamUtils;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0000EA81 File Offset: 0x0000CC81
		internal static IntPtr GetSteamNetworking()
		{
			return CSteamGameServerAPIContext.m_pSteamNetworking;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0000EA88 File Offset: 0x0000CC88
		internal static IntPtr GetSteamGameServerStats()
		{
			return CSteamGameServerAPIContext.m_pSteamGameServerStats;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0000EA8F File Offset: 0x0000CC8F
		internal static IntPtr GetSteamHTTP()
		{
			return CSteamGameServerAPIContext.m_pSteamHTTP;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0000EA96 File Offset: 0x0000CC96
		internal static IntPtr GetSteamInventory()
		{
			return CSteamGameServerAPIContext.m_pSteamInventory;
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0000EA9D File Offset: 0x0000CC9D
		internal static IntPtr GetSteamUGC()
		{
			return CSteamGameServerAPIContext.m_pSteamUGC;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0000EAA4 File Offset: 0x0000CCA4
		internal static IntPtr GetSteamNetworkingUtils()
		{
			return CSteamGameServerAPIContext.m_pSteamNetworkingUtils;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0000EAAB File Offset: 0x0000CCAB
		internal static IntPtr GetSteamNetworkingSockets()
		{
			return CSteamGameServerAPIContext.m_pSteamNetworkingSockets;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0000EAB2 File Offset: 0x0000CCB2
		internal static IntPtr GetSteamNetworkingMessages()
		{
			return CSteamGameServerAPIContext.m_pSteamNetworkingMessages;
		}

		// Token: 0x04000B4C RID: 2892
		private static IntPtr m_pSteamClient;

		// Token: 0x04000B4D RID: 2893
		private static IntPtr m_pSteamGameServer;

		// Token: 0x04000B4E RID: 2894
		private static IntPtr m_pSteamUtils;

		// Token: 0x04000B4F RID: 2895
		private static IntPtr m_pSteamNetworking;

		// Token: 0x04000B50 RID: 2896
		private static IntPtr m_pSteamGameServerStats;

		// Token: 0x04000B51 RID: 2897
		private static IntPtr m_pSteamHTTP;

		// Token: 0x04000B52 RID: 2898
		private static IntPtr m_pSteamInventory;

		// Token: 0x04000B53 RID: 2899
		private static IntPtr m_pSteamUGC;

		// Token: 0x04000B54 RID: 2900
		private static IntPtr m_pSteamNetworkingUtils;

		// Token: 0x04000B55 RID: 2901
		private static IntPtr m_pSteamNetworkingSockets;

		// Token: 0x04000B56 RID: 2902
		private static IntPtr m_pSteamNetworkingMessages;
	}
}
