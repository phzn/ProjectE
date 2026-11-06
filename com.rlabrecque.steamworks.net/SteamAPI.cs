using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x02000182 RID: 386
	public static class SteamAPI
	{
		// Token: 0x060008F0 RID: 2288 RVA: 0x0000DBC8 File Offset: 0x0000BDC8
		public static ESteamAPIInitResult InitEx(out string OutSteamErrMsg)
		{
			InteropHelp.TestIfPlatformSupported();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SteamUtils010").Append("\0");
			stringBuilder.Append("SteamNetworkingUtils004").Append("\0");
			stringBuilder.Append("STEAMAPPS_INTERFACE_VERSION009").Append("\0");
			stringBuilder.Append("SteamFriends018").Append("\0");
			stringBuilder.Append("STEAMHTMLSURFACE_INTERFACE_VERSION_005").Append("\0");
			stringBuilder.Append("STEAMHTTP_INTERFACE_VERSION003").Append("\0");
			stringBuilder.Append("SteamInput006").Append("\0");
			stringBuilder.Append("STEAMINVENTORY_INTERFACE_V003").Append("\0");
			stringBuilder.Append("SteamMatchMakingServers002").Append("\0");
			stringBuilder.Append("SteamMatchMaking009").Append("\0");
			stringBuilder.Append("STEAMMUSIC_INTERFACE_VERSION001").Append("\0");
			stringBuilder.Append("SteamNetworkingMessages002").Append("\0");
			stringBuilder.Append("SteamNetworkingSockets012").Append("\0");
			stringBuilder.Append("SteamNetworking006").Append("\0");
			stringBuilder.Append("STEAMPARENTALSETTINGS_INTERFACE_VERSION001").Append("\0");
			stringBuilder.Append("SteamParties002").Append("\0");
			stringBuilder.Append("STEAMREMOTEPLAY_INTERFACE_VERSION004").Append("\0");
			stringBuilder.Append("STEAMREMOTESTORAGE_INTERFACE_VERSION016").Append("\0");
			stringBuilder.Append("STEAMSCREENSHOTS_INTERFACE_VERSION003").Append("\0");
			stringBuilder.Append("STEAMUGC_INTERFACE_VERSION021").Append("\0");
			stringBuilder.Append("STEAMUSERSTATS_INTERFACE_VERSION013").Append("\0");
			stringBuilder.Append("SteamUser023").Append("\0");
			stringBuilder.Append("STEAMVIDEO_INTERFACE_V007").Append("\0");
			ESteamAPIInitResult esteamAPIInitResult2;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(stringBuilder.ToString()))
			{
				IntPtr intPtr = Marshal.AllocHGlobal(1024);
				ESteamAPIInitResult esteamAPIInitResult = NativeMethods.SteamInternal_SteamAPI_Init(utf8StringHandle, intPtr);
				OutSteamErrMsg = InteropHelp.PtrToStringUTF8(intPtr);
				Marshal.FreeHGlobal(intPtr);
				if (esteamAPIInitResult == ESteamAPIInitResult.k_ESteamAPIInitResult_OK)
				{
					if (CSteamAPIContext.Init())
					{
						CallbackDispatcher.Initialize();
					}
					else
					{
						esteamAPIInitResult = ESteamAPIInitResult.k_ESteamAPIInitResult_FailedGeneric;
						OutSteamErrMsg = "[Steamworks.NET] Failed to initialize CSteamAPIContext";
					}
				}
				esteamAPIInitResult2 = esteamAPIInitResult;
			}
			return esteamAPIInitResult2;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0000DE40 File Offset: 0x0000C040
		public static bool Init()
		{
			InteropHelp.TestIfPlatformSupported();
			string text;
			return SteamAPI.InitEx(out text) == ESteamAPIInitResult.k_ESteamAPIInitResult_OK;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public static void Shutdown()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_Shutdown();
			CSteamAPIContext.Clear();
			CallbackDispatcher.Shutdown();
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0000DE72 File Offset: 0x0000C072
		public static bool RestartAppIfNecessary(AppId_t unOwnAppID)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_RestartAppIfNecessary(unOwnAppID);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0000DE7F File Offset: 0x0000C07F
		public static void ReleaseCurrentThreadMemory()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_ReleaseCurrentThreadMemory();
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0000DE8B File Offset: 0x0000C08B
		public static void RunCallbacks()
		{
			CallbackDispatcher.RunFrame(false);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0000DE93 File Offset: 0x0000C093
		public static bool IsSteamRunning()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_IsSteamRunning();
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0000DE9F File Offset: 0x0000C09F
		public static HSteamPipe GetHSteamPipe()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamPipe)NativeMethods.SteamAPI_GetHSteamPipe();
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0000DEB0 File Offset: 0x0000C0B0
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.SteamAPI_GetHSteamUser();
		}
	}
}
