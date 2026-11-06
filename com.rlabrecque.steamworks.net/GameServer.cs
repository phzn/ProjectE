using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x02000183 RID: 387
	public static class GameServer
	{
		// Token: 0x060008F9 RID: 2297 RVA: 0x0000DEC4 File Offset: 0x0000C0C4
		public static ESteamAPIInitResult InitEx(uint unIP, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, string pchVersionString, out string OutSteamErrMsg)
		{
			InteropHelp.TestIfPlatformSupported();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SteamUtils010").Append('\0');
			stringBuilder.Append("SteamNetworkingUtils004").Append('\0');
			stringBuilder.Append("SteamGameServer015").Append('\0');
			stringBuilder.Append("SteamGameServerStats001").Append('\0');
			stringBuilder.Append("STEAMHTTP_INTERFACE_VERSION003").Append('\0');
			stringBuilder.Append("STEAMINVENTORY_INTERFACE_V003").Append('\0');
			stringBuilder.Append("SteamNetworking006").Append('\0');
			stringBuilder.Append("SteamNetworkingMessages002").Append('\0');
			stringBuilder.Append("SteamNetworkingSockets012").Append('\0');
			stringBuilder.Append("STEAMUGC_INTERFACE_VERSION021").Append('\0');
			ESteamAPIInitResult esteamAPIInitResult2;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersionString))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(stringBuilder.ToString()))
				{
					IntPtr intPtr = Marshal.AllocHGlobal(1024);
					ESteamAPIInitResult esteamAPIInitResult = NativeMethods.SteamInternal_GameServer_Init_V2(unIP, usGamePort, usQueryPort, eServerMode, utf8StringHandle, utf8StringHandle2, intPtr);
					OutSteamErrMsg = InteropHelp.PtrToStringUTF8(intPtr);
					Marshal.FreeHGlobal(intPtr);
					if (esteamAPIInitResult == ESteamAPIInitResult.k_ESteamAPIInitResult_OK)
					{
						if (CSteamGameServerAPIContext.Init())
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
			}
			return esteamAPIInitResult2;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0000E024 File Offset: 0x0000C224
		public static bool Init(uint unIP, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, string pchVersionString)
		{
			InteropHelp.TestIfPlatformSupported();
			string text;
			return GameServer.InitEx(unIP, usGamePort, usQueryPort, eServerMode, pchVersionString, out text) == ESteamAPIInitResult.k_ESteamAPIInitResult_OK;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0000E046 File Offset: 0x0000C246
		public static void Shutdown()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_Shutdown();
			CSteamGameServerAPIContext.Clear();
			CallbackDispatcher.Shutdown();
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0000E05C File Offset: 0x0000C25C
		public static void RunCallbacks()
		{
			CallbackDispatcher.RunFrame(true);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0000E064 File Offset: 0x0000C264
		public static void ReleaseCurrentThreadMemory()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_ReleaseCurrentThreadMemory();
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0000E070 File Offset: 0x0000C270
		public static bool BSecure()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamGameServer_BSecure();
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0000E07C File Offset: 0x0000C27C
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfPlatformSupported();
			return (CSteamID)NativeMethods.SteamGameServer_GetSteamID();
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0000E08D File Offset: 0x0000C28D
		public static HSteamPipe GetHSteamPipe()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamPipe)NativeMethods.SteamGameServer_GetHSteamPipe();
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0000E09E File Offset: 0x0000C29E
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.SteamGameServer_GetHSteamUser();
		}
	}
}
