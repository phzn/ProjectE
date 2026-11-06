using System;

namespace Steamworks
{
	// Token: 0x02000007 RID: 7
	public static class SteamGameServerClient
	{
		// Token: 0x060000BC RID: 188 RVA: 0x000039A8 File Offset: 0x00001BA8
		public static HSteamPipe CreateSteamPipe()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (HSteamPipe)NativeMethods.ISteamClient_CreateSteamPipe(CSteamGameServerAPIContext.GetSteamClient());
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000039BE File Offset: 0x00001BBE
		public static bool BReleaseSteamPipe(HSteamPipe hSteamPipe)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamClient_BReleaseSteamPipe(CSteamGameServerAPIContext.GetSteamClient(), hSteamPipe);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000039D0 File Offset: 0x00001BD0
		public static HSteamUser ConnectToGlobalUser(HSteamPipe hSteamPipe)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (HSteamUser)NativeMethods.ISteamClient_ConnectToGlobalUser(CSteamGameServerAPIContext.GetSteamClient(), hSteamPipe);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000039E7 File Offset: 0x00001BE7
		public static HSteamUser CreateLocalUser(out HSteamPipe phSteamPipe, EAccountType eAccountType)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (HSteamUser)NativeMethods.ISteamClient_CreateLocalUser(CSteamGameServerAPIContext.GetSteamClient(), out phSteamPipe, eAccountType);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000039FF File Offset: 0x00001BFF
		public static void ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamClient_ReleaseUser(CSteamGameServerAPIContext.GetSteamClient(), hSteamPipe, hUser);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003A14 File Offset: 0x00001C14
		public static IntPtr GetISteamUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamUser(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003A58 File Offset: 0x00001C58
		public static IntPtr GetISteamGameServer(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamGameServer(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003A9C File Offset: 0x00001C9C
		public static void SetLocalIPBinding(ref SteamIPAddress_t unIP, ushort usPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamClient_SetLocalIPBinding(CSteamGameServerAPIContext.GetSteamClient(), ref unIP, usPort);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003AB0 File Offset: 0x00001CB0
		public static IntPtr GetISteamFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamFriends(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003AF4 File Offset: 0x00001CF4
		public static IntPtr GetISteamUtils(HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamUtils(CSteamGameServerAPIContext.GetSteamClient(), hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003B38 File Offset: 0x00001D38
		public static IntPtr GetISteamMatchmaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamMatchmaking(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003B7C File Offset: 0x00001D7C
		public static IntPtr GetISteamMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamMatchmakingServers(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003BC0 File Offset: 0x00001DC0
		public static IntPtr GetISteamGenericInterface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamGenericInterface(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003C04 File Offset: 0x00001E04
		public static IntPtr GetISteamUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamUserStats(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003C48 File Offset: 0x00001E48
		public static IntPtr GetISteamGameServerStats(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamGameServerStats(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003C8C File Offset: 0x00001E8C
		public static IntPtr GetISteamApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamApps(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003CD0 File Offset: 0x00001ED0
		public static IntPtr GetISteamNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamNetworking(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003D14 File Offset: 0x00001F14
		public static IntPtr GetISteamRemoteStorage(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamRemoteStorage(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003D58 File Offset: 0x00001F58
		public static IntPtr GetISteamScreenshots(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamScreenshots(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003D9C File Offset: 0x00001F9C
		public static uint GetIPCCallCount()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamClient_GetIPCCallCount(CSteamGameServerAPIContext.GetSteamClient());
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003DAD File Offset: 0x00001FAD
		public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamClient_SetWarningMessageHook(CSteamGameServerAPIContext.GetSteamClient(), pFunction);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003DBF File Offset: 0x00001FBF
		public static bool BShutdownIfAllPipesClosed()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamClient_BShutdownIfAllPipesClosed(CSteamGameServerAPIContext.GetSteamClient());
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003DD0 File Offset: 0x00001FD0
		public static IntPtr GetISteamHTTP(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamHTTP(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003E14 File Offset: 0x00002014
		public static IntPtr GetISteamController(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamController(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003E58 File Offset: 0x00002058
		public static IntPtr GetISteamUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamUGC(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003E9C File Offset: 0x0000209C
		public static IntPtr GetISteamMusic(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamMusic(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00003EE0 File Offset: 0x000020E0
		public static IntPtr GetISteamHTMLSurface(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamHTMLSurface(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003F24 File Offset: 0x00002124
		public static IntPtr GetISteamInventory(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamInventory(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003F68 File Offset: 0x00002168
		public static IntPtr GetISteamVideo(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamVideo(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003FAC File Offset: 0x000021AC
		public static IntPtr GetISteamParentalSettings(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamParentalSettings(CSteamGameServerAPIContext.GetSteamClient(), hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003FF0 File Offset: 0x000021F0
		public static IntPtr GetISteamInput(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamInput(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004034 File Offset: 0x00002234
		public static IntPtr GetISteamParties(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamParties(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004078 File Offset: 0x00002278
		public static IntPtr GetISteamRemotePlay(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				intPtr = NativeMethods.ISteamClient_GetISteamRemotePlay(CSteamGameServerAPIContext.GetSteamClient(), hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return intPtr;
		}
	}
}
