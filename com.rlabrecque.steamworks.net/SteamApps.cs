using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000003 RID: 3
	public static class SteamApps
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C3 File Offset: 0x000002C3
		public static bool BIsSubscribed()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsSubscribed(CSteamAPIContext.GetSteamApps());
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020D4 File Offset: 0x000002D4
		public static bool BIsLowViolence()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsLowViolence(CSteamAPIContext.GetSteamApps());
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020E5 File Offset: 0x000002E5
		public static bool BIsCybercafe()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsCybercafe(CSteamAPIContext.GetSteamApps());
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020F6 File Offset: 0x000002F6
		public static bool BIsVACBanned()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsVACBanned(CSteamAPIContext.GetSteamApps());
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002107 File Offset: 0x00000307
		public static string GetCurrentGameLanguage()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetCurrentGameLanguage(CSteamAPIContext.GetSteamApps()));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000211D File Offset: 0x0000031D
		public static string GetAvailableGameLanguages()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetAvailableGameLanguages(CSteamAPIContext.GetSteamApps()));
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002133 File Offset: 0x00000333
		public static bool BIsSubscribedApp(AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsSubscribedApp(CSteamAPIContext.GetSteamApps(), appID);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002145 File Offset: 0x00000345
		public static bool BIsDlcInstalled(AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsDlcInstalled(CSteamAPIContext.GetSteamApps(), appID);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002157 File Offset: 0x00000357
		public static uint GetEarliestPurchaseUnixTime(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetEarliestPurchaseUnixTime(CSteamAPIContext.GetSteamApps(), nAppID);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002169 File Offset: 0x00000369
		public static bool BIsSubscribedFromFreeWeekend()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsSubscribedFromFreeWeekend(CSteamAPIContext.GetSteamApps());
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000217A File Offset: 0x0000037A
		public static int GetDLCCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetDLCCount(CSteamAPIContext.GetSteamApps());
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000218C File Offset: 0x0000038C
		public static bool BGetDLCDataByIndex(int iDLC, out AppId_t pAppID, out bool pbAvailable, out string pchName, int cchNameBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchNameBufferSize);
			bool flag = NativeMethods.ISteamApps_BGetDLCDataByIndex(CSteamAPIContext.GetSteamApps(), iDLC, out pAppID, out pbAvailable, intPtr, cchNameBufferSize);
			pchName = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021CC File Offset: 0x000003CC
		public static void InstallDLC(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamApps_InstallDLC(CSteamAPIContext.GetSteamApps(), nAppID);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021DE File Offset: 0x000003DE
		public static void UninstallDLC(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamApps_UninstallDLC(CSteamAPIContext.GetSteamApps(), nAppID);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021F0 File Offset: 0x000003F0
		public static void RequestAppProofOfPurchaseKey(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamApps_RequestAppProofOfPurchaseKey(CSteamAPIContext.GetSteamApps(), nAppID);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002204 File Offset: 0x00000404
		public static bool GetCurrentBetaName(out string pchName, int cchNameBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchNameBufferSize);
			bool flag = NativeMethods.ISteamApps_GetCurrentBetaName(CSteamAPIContext.GetSteamApps(), intPtr, cchNameBufferSize);
			pchName = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000223F File Offset: 0x0000043F
		public static bool MarkContentCorrupt(bool bMissingFilesOnly)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_MarkContentCorrupt(CSteamAPIContext.GetSteamApps(), bMissingFilesOnly);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002251 File Offset: 0x00000451
		public static uint GetInstalledDepots(AppId_t appID, DepotId_t[] pvecDepots, uint cMaxDepots)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetInstalledDepots(CSteamAPIContext.GetSteamApps(), appID, pvecDepots, cMaxDepots);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002268 File Offset: 0x00000468
		public static uint GetAppInstallDir(AppId_t appID, out string pchFolder, uint cchFolderBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchFolderBufferSize);
			uint num = NativeMethods.ISteamApps_GetAppInstallDir(CSteamAPIContext.GetSteamApps(), appID, intPtr, cchFolderBufferSize);
			pchFolder = ((num != 0U) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000022A4 File Offset: 0x000004A4
		public static bool BIsAppInstalled(AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsAppInstalled(CSteamAPIContext.GetSteamApps(), appID);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000022B6 File Offset: 0x000004B6
		public static CSteamID GetAppOwner()
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamApps_GetAppOwner(CSteamAPIContext.GetSteamApps());
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000022CC File Offset: 0x000004CC
		public static string GetLaunchQueryParam(string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string text;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				text = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetLaunchQueryParam(CSteamAPIContext.GetSteamApps(), utf8StringHandle));
			}
			return text;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002314 File Offset: 0x00000514
		public static bool GetDlcDownloadProgress(AppId_t nAppID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetDlcDownloadProgress(CSteamAPIContext.GetSteamApps(), nAppID, out punBytesDownloaded, out punBytesTotal);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002328 File Offset: 0x00000528
		public static int GetAppBuildId()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetAppBuildId(CSteamAPIContext.GetSteamApps());
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002339 File Offset: 0x00000539
		public static void RequestAllProofOfPurchaseKeys()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamApps_RequestAllProofOfPurchaseKeys(CSteamAPIContext.GetSteamApps());
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000234C File Offset: 0x0000054C
		public static SteamAPICall_t GetFileDetails(string pszFileName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszFileName))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamApps_GetFileDetails(CSteamAPIContext.GetSteamApps(), utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002394 File Offset: 0x00000594
		public static int GetLaunchCommandLine(out string pszCommandLine, int cubCommandLine)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cubCommandLine);
			int num = NativeMethods.ISteamApps_GetLaunchCommandLine(CSteamAPIContext.GetSteamApps(), intPtr, cubCommandLine);
			pszCommandLine = ((num != -1) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000023D0 File Offset: 0x000005D0
		public static bool BIsSubscribedFromFamilySharing()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsSubscribedFromFamilySharing(CSteamAPIContext.GetSteamApps());
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000023E1 File Offset: 0x000005E1
		public static bool BIsTimedTrial(out uint punSecondsAllowed, out uint punSecondsPlayed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsTimedTrial(CSteamAPIContext.GetSteamApps(), out punSecondsAllowed, out punSecondsPlayed);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000023F4 File Offset: 0x000005F4
		public static bool SetDlcContext(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_SetDlcContext(CSteamAPIContext.GetSteamApps(), nAppID);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002406 File Offset: 0x00000606
		public static int GetNumBetas(out int pnAvailable, out int pnPrivate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetNumBetas(CSteamAPIContext.GetSteamApps(), out pnAvailable, out pnPrivate);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000241C File Offset: 0x0000061C
		public static bool GetBetaInfo(int iBetaIndex, out uint punFlags, out uint punBuildID, out string pchBetaName, int cchBetaName, out string pchDescription, int cchDescription, out uint punLastUpdated)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchBetaName);
			IntPtr intPtr2 = Marshal.AllocHGlobal(cchDescription);
			bool flag = NativeMethods.ISteamApps_GetBetaInfo(CSteamAPIContext.GetSteamApps(), iBetaIndex, out punFlags, out punBuildID, intPtr, cchBetaName, intPtr2, cchDescription, out punLastUpdated);
			pchBetaName = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			pchDescription = (flag ? InteropHelp.PtrToStringUTF8(intPtr2) : null);
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002480 File Offset: 0x00000680
		public static bool SetActiveBeta(string pchBetaName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchBetaName))
			{
				flag = NativeMethods.ISteamApps_SetActiveBeta(CSteamAPIContext.GetSteamApps(), utf8StringHandle);
			}
			return flag;
		}
	}
}
