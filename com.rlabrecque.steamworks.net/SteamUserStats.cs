using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000024 RID: 36
	public static class SteamUserStats
	{
		// Token: 0x0600044E RID: 1102 RVA: 0x0000B444 File Offset: 0x00009644
		public static bool GetStat(string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetStatInt32(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000B488 File Offset: 0x00009688
		public static bool GetStat(string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetStatFloat(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000B4CC File Offset: 0x000096CC
		public static bool SetStat(string pchName, int nData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_SetStatInt32(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, nData);
			}
			return flag;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000B510 File Offset: 0x00009710
		public static bool SetStat(string pchName, float fData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_SetStatFloat(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, fData);
			}
			return flag;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000B554 File Offset: 0x00009754
		public static bool UpdateAvgRateStat(string pchName, float flCountThisSession, double dSessionLength)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_UpdateAvgRateStat(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, flCountThisSession, dSessionLength);
			}
			return flag;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000B598 File Offset: 0x00009798
		public static bool GetAchievement(string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetAchievement(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, out pbAchieved);
			}
			return flag;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000B5DC File Offset: 0x000097DC
		public static bool SetAchievement(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_SetAchievement(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000B620 File Offset: 0x00009820
		public static bool ClearAchievement(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_ClearAchievement(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000B664 File Offset: 0x00009864
		public static bool GetAchievementAndUnlockTime(string pchName, out bool pbAchieved, out uint punUnlockTime)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetAchievementAndUnlockTime(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, out pbAchieved, out punUnlockTime);
			}
			return flag;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000B6A8 File Offset: 0x000098A8
		public static bool StoreStats()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_StoreStats(CSteamAPIContext.GetSteamUserStats());
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000B6BC File Offset: 0x000098BC
		public static int GetAchievementIcon(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			int num;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				num = NativeMethods.ISteamUserStats_GetAchievementIcon(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle);
			}
			return num;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000B700 File Offset: 0x00009900
		public static string GetAchievementDisplayAttribute(string pchName, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string text;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchKey))
				{
					text = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementDisplayAttribute(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, utf8StringHandle2));
				}
			}
			return text;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000B768 File Offset: 0x00009968
		public static bool IndicateAchievementProgress(string pchName, uint nCurProgress, uint nMaxProgress)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_IndicateAchievementProgress(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, nCurProgress, nMaxProgress);
			}
			return flag;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000B7AC File Offset: 0x000099AC
		public static uint GetNumAchievements()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetNumAchievements(CSteamAPIContext.GetSteamUserStats());
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000B7BD File Offset: 0x000099BD
		public static string GetAchievementName(uint iAchievement)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementName(CSteamAPIContext.GetSteamUserStats(), iAchievement));
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000B7D4 File Offset: 0x000099D4
		public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestUserStats(CSteamAPIContext.GetSteamUserStats(), steamIDUser);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000B7EC File Offset: 0x000099EC
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetUserStatInt32(CSteamAPIContext.GetSteamUserStats(), steamIDUser, utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000B830 File Offset: 0x00009A30
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetUserStatFloat(CSteamAPIContext.GetSteamUserStats(), steamIDUser, utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000B874 File Offset: 0x00009A74
		public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetUserAchievement(CSteamAPIContext.GetSteamUserStats(), steamIDUser, utf8StringHandle, out pbAchieved);
			}
			return flag;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000B8B8 File Offset: 0x00009AB8
		public static bool GetUserAchievementAndUnlockTime(CSteamID steamIDUser, string pchName, out bool pbAchieved, out uint punUnlockTime)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetUserAchievementAndUnlockTime(CSteamAPIContext.GetSteamUserStats(), steamIDUser, utf8StringHandle, out pbAchieved, out punUnlockTime);
			}
			return flag;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000B900 File Offset: 0x00009B00
		public static bool ResetAllStats(bool bAchievementsToo)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_ResetAllStats(CSteamAPIContext.GetSteamUserStats(), bAchievementsToo);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000B914 File Offset: 0x00009B14
		public static SteamAPICall_t FindOrCreateLeaderboard(string pchLeaderboardName, ELeaderboardSortMethod eLeaderboardSortMethod, ELeaderboardDisplayType eLeaderboardDisplayType)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamUserStats_FindOrCreateLeaderboard(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, eLeaderboardSortMethod, eLeaderboardDisplayType);
			}
			return steamAPICall_t;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000B960 File Offset: 0x00009B60
		public static SteamAPICall_t FindLeaderboard(string pchLeaderboardName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamUserStats_FindLeaderboard(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000B9A8 File Offset: 0x00009BA8
		public static string GetLeaderboardName(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetLeaderboardName(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard));
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000B9BF File Offset: 0x00009BBF
		public static int GetLeaderboardEntryCount(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardEntryCount(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000B9D1 File Offset: 0x00009BD1
		public static ELeaderboardSortMethod GetLeaderboardSortMethod(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardSortMethod(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000B9E3 File Offset: 0x00009BE3
		public static ELeaderboardDisplayType GetLeaderboardDisplayType(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardDisplayType(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000B9F5 File Offset: 0x00009BF5
		public static SteamAPICall_t DownloadLeaderboardEntries(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_DownloadLeaderboardEntries(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard, eLeaderboardDataRequest, nRangeStart, nRangeEnd);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000BA0F File Offset: 0x00009C0F
		public static SteamAPICall_t DownloadLeaderboardEntriesForUsers(SteamLeaderboard_t hSteamLeaderboard, CSteamID[] prgUsers, int cUsers)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_DownloadLeaderboardEntriesForUsers(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard, prgUsers, cUsers);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000BA28 File Offset: 0x00009C28
		public static bool GetDownloadedLeaderboardEntry(SteamLeaderboardEntries_t hSteamLeaderboardEntries, int index, out LeaderboardEntry_t pLeaderboardEntry, int[] pDetails, int cDetailsMax)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetDownloadedLeaderboardEntry(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboardEntries, index, out pLeaderboardEntry, pDetails, cDetailsMax);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000BA3F File Offset: 0x00009C3F
		public static SteamAPICall_t UploadLeaderboardScore(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, int[] pScoreDetails, int cScoreDetailsCount)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_UploadLeaderboardScore(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard, eLeaderboardUploadScoreMethod, nScore, pScoreDetails, cScoreDetailsCount);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000BA5B File Offset: 0x00009C5B
		public static SteamAPICall_t AttachLeaderboardUGC(SteamLeaderboard_t hSteamLeaderboard, UGCHandle_t hUGC)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_AttachLeaderboardUGC(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard, hUGC);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000BA73 File Offset: 0x00009C73
		public static SteamAPICall_t GetNumberOfCurrentPlayers()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_GetNumberOfCurrentPlayers(CSteamAPIContext.GetSteamUserStats());
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000BA89 File Offset: 0x00009C89
		public static SteamAPICall_t RequestGlobalAchievementPercentages()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestGlobalAchievementPercentages(CSteamAPIContext.GetSteamUserStats());
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000BAA0 File Offset: 0x00009CA0
		public static int GetMostAchievedAchievementInfo(out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)unNameBufLen);
			int num = NativeMethods.ISteamUserStats_GetMostAchievedAchievementInfo(CSteamAPIContext.GetSteamUserStats(), intPtr, unNameBufLen, out pflPercent, out pbAchieved);
			pchName = ((num != -1) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000BAE0 File Offset: 0x00009CE0
		public static int GetNextMostAchievedAchievementInfo(int iIteratorPrevious, out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)unNameBufLen);
			int num = NativeMethods.ISteamUserStats_GetNextMostAchievedAchievementInfo(CSteamAPIContext.GetSteamUserStats(), iIteratorPrevious, intPtr, unNameBufLen, out pflPercent, out pbAchieved);
			pchName = ((num != -1) ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000BB20 File Offset: 0x00009D20
		public static bool GetAchievementAchievedPercent(string pchName, out float pflPercent)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetAchievementAchievedPercent(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, out pflPercent);
			}
			return flag;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000BB64 File Offset: 0x00009D64
		public static SteamAPICall_t RequestGlobalStats(int nHistoryDays)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestGlobalStats(CSteamAPIContext.GetSteamUserStats(), nHistoryDays);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000BB7C File Offset: 0x00009D7C
		public static bool GetGlobalStat(string pchStatName, out long pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				flag = NativeMethods.ISteamUserStats_GetGlobalStatInt64(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		public static bool GetGlobalStat(string pchStatName, out double pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				flag = NativeMethods.ISteamUserStats_GetGlobalStatDouble(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000BC04 File Offset: 0x00009E04
		public static int GetGlobalStatHistory(string pchStatName, long[] pData, uint cubData)
		{
			InteropHelp.TestIfAvailableClient();
			int num;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				num = NativeMethods.ISteamUserStats_GetGlobalStatHistoryInt64(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, pData, cubData);
			}
			return num;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000BC48 File Offset: 0x00009E48
		public static int GetGlobalStatHistory(string pchStatName, double[] pData, uint cubData)
		{
			InteropHelp.TestIfAvailableClient();
			int num;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				num = NativeMethods.ISteamUserStats_GetGlobalStatHistoryDouble(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, pData, cubData);
			}
			return num;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000BC8C File Offset: 0x00009E8C
		public static bool GetAchievementProgressLimits(string pchName, out int pnMinProgress, out int pnMaxProgress)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetAchievementProgressLimitsInt32(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, out pnMinProgress, out pnMaxProgress);
			}
			return flag;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000BCD0 File Offset: 0x00009ED0
		public static bool GetAchievementProgressLimits(string pchName, out float pfMinProgress, out float pfMaxProgress)
		{
			InteropHelp.TestIfAvailableClient();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamUserStats_GetAchievementProgressLimitsFloat(CSteamAPIContext.GetSteamUserStats(), utf8StringHandle, out pfMinProgress, out pfMaxProgress);
			}
			return flag;
		}
	}
}
