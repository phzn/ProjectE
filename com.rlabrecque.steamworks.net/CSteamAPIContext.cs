using System;

namespace Steamworks
{
	// Token: 0x02000185 RID: 389
	internal static class CSteamAPIContext
	{
		// Token: 0x0600090B RID: 2315 RVA: 0x0000E158 File Offset: 0x0000C358
		internal static void Clear()
		{
			CSteamAPIContext.m_pSteamClient = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUser = IntPtr.Zero;
			CSteamAPIContext.m_pSteamFriends = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUtils = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMatchmaking = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUserStats = IntPtr.Zero;
			CSteamAPIContext.m_pSteamApps = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMatchmakingServers = IntPtr.Zero;
			CSteamAPIContext.m_pSteamNetworking = IntPtr.Zero;
			CSteamAPIContext.m_pSteamRemoteStorage = IntPtr.Zero;
			CSteamAPIContext.m_pSteamHTTP = IntPtr.Zero;
			CSteamAPIContext.m_pSteamScreenshots = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMusic = IntPtr.Zero;
			CSteamAPIContext.m_pController = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUGC = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMusic = IntPtr.Zero;
			CSteamAPIContext.m_pSteamHTMLSurface = IntPtr.Zero;
			CSteamAPIContext.m_pSteamInventory = IntPtr.Zero;
			CSteamAPIContext.m_pSteamVideo = IntPtr.Zero;
			CSteamAPIContext.m_pSteamParentalSettings = IntPtr.Zero;
			CSteamAPIContext.m_pSteamInput = IntPtr.Zero;
			CSteamAPIContext.m_pSteamParties = IntPtr.Zero;
			CSteamAPIContext.m_pSteamRemotePlay = IntPtr.Zero;
			CSteamAPIContext.m_pSteamNetworkingUtils = IntPtr.Zero;
			CSteamAPIContext.m_pSteamNetworkingSockets = IntPtr.Zero;
			CSteamAPIContext.m_pSteamNetworkingMessages = IntPtr.Zero;
			CSteamAPIContext.m_pSteamTimeline = IntPtr.Zero;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0000E274 File Offset: 0x0000C474
		internal static bool Init()
		{
			HSteamUser hsteamUser = SteamAPI.GetHSteamUser();
			HSteamPipe hsteamPipe = SteamAPI.GetHSteamPipe();
			if (hsteamPipe == (HSteamPipe)0)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle("SteamClient023"))
			{
				CSteamAPIContext.m_pSteamClient = NativeMethods.SteamInternal_CreateInterface(utf8StringHandle);
			}
			if (CSteamAPIContext.m_pSteamClient == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUser = SteamClient.GetISteamUser(hsteamUser, hsteamPipe, "SteamUser023");
			if (CSteamAPIContext.m_pSteamUser == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamFriends = SteamClient.GetISteamFriends(hsteamUser, hsteamPipe, "SteamFriends018");
			if (CSteamAPIContext.m_pSteamFriends == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUtils = SteamClient.GetISteamUtils(hsteamPipe, "SteamUtils010");
			if (CSteamAPIContext.m_pSteamUtils == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamMatchmaking = SteamClient.GetISteamMatchmaking(hsteamUser, hsteamPipe, "SteamMatchMaking009");
			if (CSteamAPIContext.m_pSteamMatchmaking == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamMatchmakingServers = SteamClient.GetISteamMatchmakingServers(hsteamUser, hsteamPipe, "SteamMatchMakingServers002");
			if (CSteamAPIContext.m_pSteamMatchmakingServers == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUserStats = SteamClient.GetISteamUserStats(hsteamUser, hsteamPipe, "STEAMUSERSTATS_INTERFACE_VERSION013");
			if (CSteamAPIContext.m_pSteamUserStats == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamApps = SteamClient.GetISteamApps(hsteamUser, hsteamPipe, "STEAMAPPS_INTERFACE_VERSION009");
			if (CSteamAPIContext.m_pSteamApps == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamNetworking = SteamClient.GetISteamNetworking(hsteamUser, hsteamPipe, "SteamNetworking006");
			if (CSteamAPIContext.m_pSteamNetworking == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamRemoteStorage = SteamClient.GetISteamRemoteStorage(hsteamUser, hsteamPipe, "STEAMREMOTESTORAGE_INTERFACE_VERSION016");
			if (CSteamAPIContext.m_pSteamRemoteStorage == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamScreenshots = SteamClient.GetISteamScreenshots(hsteamUser, hsteamPipe, "STEAMSCREENSHOTS_INTERFACE_VERSION003");
			if (CSteamAPIContext.m_pSteamScreenshots == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamHTTP = SteamClient.GetISteamHTTP(hsteamUser, hsteamPipe, "STEAMHTTP_INTERFACE_VERSION003");
			if (CSteamAPIContext.m_pSteamHTTP == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUGC = SteamClient.GetISteamUGC(hsteamUser, hsteamPipe, "STEAMUGC_INTERFACE_VERSION021");
			if (CSteamAPIContext.m_pSteamUGC == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamMusic = SteamClient.GetISteamMusic(hsteamUser, hsteamPipe, "STEAMMUSIC_INTERFACE_VERSION001");
			if (CSteamAPIContext.m_pSteamMusic == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamHTMLSurface = SteamClient.GetISteamHTMLSurface(hsteamUser, hsteamPipe, "STEAMHTMLSURFACE_INTERFACE_VERSION_005");
			if (CSteamAPIContext.m_pSteamHTMLSurface == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamInventory = SteamClient.GetISteamInventory(hsteamUser, hsteamPipe, "STEAMINVENTORY_INTERFACE_V003");
			if (CSteamAPIContext.m_pSteamInventory == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamVideo = SteamClient.GetISteamVideo(hsteamUser, hsteamPipe, "STEAMVIDEO_INTERFACE_V007");
			if (CSteamAPIContext.m_pSteamVideo == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamParentalSettings = SteamClient.GetISteamParentalSettings(hsteamUser, hsteamPipe, "STEAMPARENTALSETTINGS_INTERFACE_VERSION001");
			if (CSteamAPIContext.m_pSteamParentalSettings == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamInput = SteamClient.GetISteamInput(hsteamUser, hsteamPipe, "SteamInput006");
			if (CSteamAPIContext.m_pSteamInput == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamParties = SteamClient.GetISteamParties(hsteamUser, hsteamPipe, "SteamParties002");
			if (CSteamAPIContext.m_pSteamParties == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamRemotePlay = SteamClient.GetISteamRemotePlay(hsteamUser, hsteamPipe, "STEAMREMOTEPLAY_INTERFACE_VERSION004");
			if (CSteamAPIContext.m_pSteamRemotePlay == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle("SteamNetworkingUtils004"))
			{
				CSteamAPIContext.m_pSteamNetworkingUtils = ((NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle2) != IntPtr.Zero) ? NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle2) : NativeMethods.SteamInternal_FindOrCreateGameServerInterface(hsteamUser, utf8StringHandle2));
			}
			if (CSteamAPIContext.m_pSteamNetworkingUtils == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle("SteamNetworkingSockets012"))
			{
				CSteamAPIContext.m_pSteamNetworkingSockets = NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle3);
			}
			if (CSteamAPIContext.m_pSteamNetworkingSockets == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle("SteamNetworkingMessages002"))
			{
				CSteamAPIContext.m_pSteamNetworkingMessages = NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle4);
			}
			if (CSteamAPIContext.m_pSteamNetworkingMessages == IntPtr.Zero)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle utf8StringHandle5 = new InteropHelp.UTF8StringHandle("STEAMTIMELINE_INTERFACE_V004"))
			{
				CSteamAPIContext.m_pSteamTimeline = NativeMethods.SteamInternal_FindOrCreateUserInterface(hsteamUser, utf8StringHandle5);
			}
			return !(CSteamAPIContext.m_pSteamTimeline == IntPtr.Zero);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0000E6E4 File Offset: 0x0000C8E4
		internal static IntPtr GetSteamClient()
		{
			return CSteamAPIContext.m_pSteamClient;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0000E6EB File Offset: 0x0000C8EB
		internal static IntPtr GetSteamUser()
		{
			return CSteamAPIContext.m_pSteamUser;
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0000E6F2 File Offset: 0x0000C8F2
		internal static IntPtr GetSteamFriends()
		{
			return CSteamAPIContext.m_pSteamFriends;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0000E6F9 File Offset: 0x0000C8F9
		internal static IntPtr GetSteamUtils()
		{
			return CSteamAPIContext.m_pSteamUtils;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0000E700 File Offset: 0x0000C900
		internal static IntPtr GetSteamMatchmaking()
		{
			return CSteamAPIContext.m_pSteamMatchmaking;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0000E707 File Offset: 0x0000C907
		internal static IntPtr GetSteamUserStats()
		{
			return CSteamAPIContext.m_pSteamUserStats;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0000E70E File Offset: 0x0000C90E
		internal static IntPtr GetSteamApps()
		{
			return CSteamAPIContext.m_pSteamApps;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0000E715 File Offset: 0x0000C915
		internal static IntPtr GetSteamMatchmakingServers()
		{
			return CSteamAPIContext.m_pSteamMatchmakingServers;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0000E71C File Offset: 0x0000C91C
		internal static IntPtr GetSteamNetworking()
		{
			return CSteamAPIContext.m_pSteamNetworking;
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0000E723 File Offset: 0x0000C923
		internal static IntPtr GetSteamRemoteStorage()
		{
			return CSteamAPIContext.m_pSteamRemoteStorage;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0000E72A File Offset: 0x0000C92A
		internal static IntPtr GetSteamScreenshots()
		{
			return CSteamAPIContext.m_pSteamScreenshots;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0000E731 File Offset: 0x0000C931
		internal static IntPtr GetSteamHTTP()
		{
			return CSteamAPIContext.m_pSteamHTTP;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0000E738 File Offset: 0x0000C938
		internal static IntPtr GetSteamController()
		{
			return CSteamAPIContext.m_pController;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0000E73F File Offset: 0x0000C93F
		internal static IntPtr GetSteamUGC()
		{
			return CSteamAPIContext.m_pSteamUGC;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0000E746 File Offset: 0x0000C946
		internal static IntPtr GetSteamMusic()
		{
			return CSteamAPIContext.m_pSteamMusic;
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0000E74D File Offset: 0x0000C94D
		internal static IntPtr GetSteamHTMLSurface()
		{
			return CSteamAPIContext.m_pSteamHTMLSurface;
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0000E754 File Offset: 0x0000C954
		internal static IntPtr GetSteamInventory()
		{
			return CSteamAPIContext.m_pSteamInventory;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0000E75B File Offset: 0x0000C95B
		internal static IntPtr GetSteamVideo()
		{
			return CSteamAPIContext.m_pSteamVideo;
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0000E762 File Offset: 0x0000C962
		internal static IntPtr GetSteamParentalSettings()
		{
			return CSteamAPIContext.m_pSteamParentalSettings;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0000E769 File Offset: 0x0000C969
		internal static IntPtr GetSteamInput()
		{
			return CSteamAPIContext.m_pSteamInput;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0000E770 File Offset: 0x0000C970
		internal static IntPtr GetSteamParties()
		{
			return CSteamAPIContext.m_pSteamParties;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0000E777 File Offset: 0x0000C977
		internal static IntPtr GetSteamRemotePlay()
		{
			return CSteamAPIContext.m_pSteamRemotePlay;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0000E77E File Offset: 0x0000C97E
		internal static IntPtr GetSteamNetworkingUtils()
		{
			return CSteamAPIContext.m_pSteamNetworkingUtils;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0000E785 File Offset: 0x0000C985
		internal static IntPtr GetSteamNetworkingSockets()
		{
			return CSteamAPIContext.m_pSteamNetworkingSockets;
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0000E78C File Offset: 0x0000C98C
		internal static IntPtr GetSteamNetworkingMessages()
		{
			return CSteamAPIContext.m_pSteamNetworkingMessages;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0000E793 File Offset: 0x0000C993
		internal static IntPtr GetSteamTimeline()
		{
			return CSteamAPIContext.m_pSteamTimeline;
		}

		// Token: 0x04000B32 RID: 2866
		private static IntPtr m_pSteamClient;

		// Token: 0x04000B33 RID: 2867
		private static IntPtr m_pSteamUser;

		// Token: 0x04000B34 RID: 2868
		private static IntPtr m_pSteamFriends;

		// Token: 0x04000B35 RID: 2869
		private static IntPtr m_pSteamUtils;

		// Token: 0x04000B36 RID: 2870
		private static IntPtr m_pSteamMatchmaking;

		// Token: 0x04000B37 RID: 2871
		private static IntPtr m_pSteamUserStats;

		// Token: 0x04000B38 RID: 2872
		private static IntPtr m_pSteamApps;

		// Token: 0x04000B39 RID: 2873
		private static IntPtr m_pSteamMatchmakingServers;

		// Token: 0x04000B3A RID: 2874
		private static IntPtr m_pSteamNetworking;

		// Token: 0x04000B3B RID: 2875
		private static IntPtr m_pSteamRemoteStorage;

		// Token: 0x04000B3C RID: 2876
		private static IntPtr m_pSteamScreenshots;

		// Token: 0x04000B3D RID: 2877
		private static IntPtr m_pSteamHTTP;

		// Token: 0x04000B3E RID: 2878
		private static IntPtr m_pController;

		// Token: 0x04000B3F RID: 2879
		private static IntPtr m_pSteamUGC;

		// Token: 0x04000B40 RID: 2880
		private static IntPtr m_pSteamMusic;

		// Token: 0x04000B41 RID: 2881
		private static IntPtr m_pSteamHTMLSurface;

		// Token: 0x04000B42 RID: 2882
		private static IntPtr m_pSteamInventory;

		// Token: 0x04000B43 RID: 2883
		private static IntPtr m_pSteamVideo;

		// Token: 0x04000B44 RID: 2884
		private static IntPtr m_pSteamParentalSettings;

		// Token: 0x04000B45 RID: 2885
		private static IntPtr m_pSteamInput;

		// Token: 0x04000B46 RID: 2886
		private static IntPtr m_pSteamParties;

		// Token: 0x04000B47 RID: 2887
		private static IntPtr m_pSteamRemotePlay;

		// Token: 0x04000B48 RID: 2888
		private static IntPtr m_pSteamNetworkingUtils;

		// Token: 0x04000B49 RID: 2889
		private static IntPtr m_pSteamNetworkingSockets;

		// Token: 0x04000B4A RID: 2890
		private static IntPtr m_pSteamNetworkingMessages;

		// Token: 0x04000B4B RID: 2891
		private static IntPtr m_pSteamTimeline;
	}
}
