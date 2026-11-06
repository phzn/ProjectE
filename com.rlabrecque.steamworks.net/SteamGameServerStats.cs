using System;

namespace Steamworks
{
	// Token: 0x0200000E RID: 14
	public static class SteamGameServerStats
	{
		// Token: 0x06000182 RID: 386 RVA: 0x00005440 File Offset: 0x00003640
		public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerStats_RequestUserStats(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00005458 File Offset: 0x00003658
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_GetUserStatInt32(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000549C File Offset: 0x0000369C
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_GetUserStatFloat(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, out pData);
			}
			return flag;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000054E0 File Offset: 0x000036E0
		public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_GetUserAchievement(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, out pbAchieved);
			}
			return flag;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00005524 File Offset: 0x00003724
		public static bool SetUserStat(CSteamID steamIDUser, string pchName, int nData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_SetUserStatInt32(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, nData);
			}
			return flag;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00005568 File Offset: 0x00003768
		public static bool SetUserStat(CSteamID steamIDUser, string pchName, float fData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_SetUserStatFloat(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, fData);
			}
			return flag;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000055AC File Offset: 0x000037AC
		public static bool UpdateUserAvgRateStat(CSteamID steamIDUser, string pchName, float flCountThisSession, double dSessionLength)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_UpdateUserAvgRateStat(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle, flCountThisSession, dSessionLength);
			}
			return flag;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000055F4 File Offset: 0x000037F4
		public static bool SetUserAchievement(CSteamID steamIDUser, string pchName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_SetUserAchievement(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00005638 File Offset: 0x00003838
		public static bool ClearUserAchievement(CSteamID steamIDUser, string pchName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				flag = NativeMethods.ISteamGameServerStats_ClearUserAchievement(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser, utf8StringHandle);
			}
			return flag;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000567C File Offset: 0x0000387C
		public static SteamAPICall_t StoreUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerStats_StoreUserStats(CSteamGameServerAPIContext.GetSteamGameServerStats(), steamIDUser);
		}
	}
}
