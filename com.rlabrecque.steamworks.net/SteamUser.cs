using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000023 RID: 35
	public static class SteamUser
	{
		// Token: 0x0600042D RID: 1069 RVA: 0x0000B0D5 File Offset: 0x000092D5
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamUser)NativeMethods.ISteamUser_GetHSteamUser(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000B0EB File Offset: 0x000092EB
		public static bool BLoggedOn()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BLoggedOn(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000B0FC File Offset: 0x000092FC
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamUser_GetSteamID(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000B112 File Offset: 0x00009312
		public static int InitiateGameConnection_DEPRECATED(byte[] pAuthBlob, int cbMaxAuthBlob, CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, bool bSecure)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_InitiateGameConnection_DEPRECATED(CSteamAPIContext.GetSteamUser(), pAuthBlob, cbMaxAuthBlob, steamIDGameServer, unIPServer, usPortServer, bSecure);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000B12B File Offset: 0x0000932B
		public static void TerminateGameConnection_DEPRECATED(uint unIPServer, ushort usPortServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_TerminateGameConnection_DEPRECATED(CSteamAPIContext.GetSteamUser(), unIPServer, usPortServer);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000B140 File Offset: 0x00009340
		public static void TrackAppUsageEvent(CGameID gameID, int eAppUsageEvent, string pchExtraInfo = "")
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchExtraInfo))
			{
				NativeMethods.ISteamUser_TrackAppUsageEvent(CSteamAPIContext.GetSteamUser(), gameID, eAppUsageEvent, utf8StringHandle);
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000B184 File Offset: 0x00009384
		public static bool GetUserDataFolder(out string pchBuffer, int cubBuffer)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cubBuffer);
			bool flag = NativeMethods.ISteamUser_GetUserDataFolder(CSteamAPIContext.GetSteamUser(), intPtr, cubBuffer);
			pchBuffer = (flag ? InteropHelp.PtrToStringUTF8(intPtr) : null);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000B1BF File Offset: 0x000093BF
		public static void StartVoiceRecording()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_StartVoiceRecording(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000B1D0 File Offset: 0x000093D0
		public static void StopVoiceRecording()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_StopVoiceRecording(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000B1E1 File Offset: 0x000093E1
		public static EVoiceResult GetAvailableVoice(out uint pcbCompressed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetAvailableVoice(CSteamAPIContext.GetSteamUser(), out pcbCompressed, IntPtr.Zero, 0U);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000B1FC File Offset: 0x000093FC
		public static EVoiceResult GetVoice(bool bWantCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetVoice(CSteamAPIContext.GetSteamUser(), bWantCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, false, IntPtr.Zero, 0U, IntPtr.Zero, 0U);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000B229 File Offset: 0x00009429
		public static EVoiceResult DecompressVoice(byte[] pCompressed, uint cbCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_DecompressVoice(CSteamAPIContext.GetSteamUser(), pCompressed, cbCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, nDesiredSampleRate);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000B242 File Offset: 0x00009442
		public static uint GetVoiceOptimalSampleRate()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetVoiceOptimalSampleRate(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000B253 File Offset: 0x00009453
		public static HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket, ref SteamNetworkingIdentity pSteamNetworkingIdentity)
		{
			InteropHelp.TestIfAvailableClient();
			return (HAuthTicket)NativeMethods.ISteamUser_GetAuthSessionTicket(CSteamAPIContext.GetSteamUser(), pTicket, cbMaxTicket, out pcbTicket, ref pSteamNetworkingIdentity);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000B270 File Offset: 0x00009470
		public static HAuthTicket GetAuthTicketForWebApi(string pchIdentity)
		{
			InteropHelp.TestIfAvailableClient();
			HAuthTicket hauthTicket;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchIdentity))
			{
				hauthTicket = (HAuthTicket)NativeMethods.ISteamUser_GetAuthTicketForWebApi(CSteamAPIContext.GetSteamUser(), utf8StringHandle);
			}
			return hauthTicket;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000B2B8 File Offset: 0x000094B8
		public static EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BeginAuthSession(CSteamAPIContext.GetSteamUser(), pAuthTicket, cbAuthTicket, steamID);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000B2CC File Offset: 0x000094CC
		public static void EndAuthSession(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_EndAuthSession(CSteamAPIContext.GetSteamUser(), steamID);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000B2DE File Offset: 0x000094DE
		public static void CancelAuthTicket(HAuthTicket hAuthTicket)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_CancelAuthTicket(CSteamAPIContext.GetSteamUser(), hAuthTicket);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000B2F0 File Offset: 0x000094F0
		public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_UserHasLicenseForApp(CSteamAPIContext.GetSteamUser(), steamID, appID);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000B303 File Offset: 0x00009503
		public static bool BIsBehindNAT()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsBehindNAT(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000B314 File Offset: 0x00009514
		public static void AdvertiseGame(CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_AdvertiseGame(CSteamAPIContext.GetSteamUser(), steamIDGameServer, unIPServer, usPortServer);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000B328 File Offset: 0x00009528
		public static SteamAPICall_t RequestEncryptedAppTicket(byte[] pDataToInclude, int cbDataToInclude)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUser_RequestEncryptedAppTicket(CSteamAPIContext.GetSteamUser(), pDataToInclude, cbDataToInclude);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000B340 File Offset: 0x00009540
		public static bool GetEncryptedAppTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetEncryptedAppTicket(CSteamAPIContext.GetSteamUser(), pTicket, cbMaxTicket, out pcbTicket);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000B354 File Offset: 0x00009554
		public static int GetGameBadgeLevel(int nSeries, bool bFoil)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetGameBadgeLevel(CSteamAPIContext.GetSteamUser(), nSeries, bFoil);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000B367 File Offset: 0x00009567
		public static int GetPlayerSteamLevel()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetPlayerSteamLevel(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000B378 File Offset: 0x00009578
		public static SteamAPICall_t RequestStoreAuthURL(string pchRedirectURL)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchRedirectURL))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamUser_RequestStoreAuthURL(CSteamAPIContext.GetSteamUser(), utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000B3C0 File Offset: 0x000095C0
		public static bool BIsPhoneVerified()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneVerified(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000B3D1 File Offset: 0x000095D1
		public static bool BIsTwoFactorEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsTwoFactorEnabled(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000B3E2 File Offset: 0x000095E2
		public static bool BIsPhoneIdentifying()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneIdentifying(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000B3F3 File Offset: 0x000095F3
		public static bool BIsPhoneRequiringVerification()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsPhoneRequiringVerification(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000B404 File Offset: 0x00009604
		public static SteamAPICall_t GetMarketEligibility()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUser_GetMarketEligibility(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000B41A File Offset: 0x0000961A
		public static SteamAPICall_t GetDurationControl()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUser_GetDurationControl(CSteamAPIContext.GetSteamUser());
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000B430 File Offset: 0x00009630
		public static bool BSetDurationControlOnlineState(EDurationControlOnlineState eNewState)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BSetDurationControlOnlineState(CSteamAPIContext.GetSteamUser(), eNewState);
		}
	}
}
