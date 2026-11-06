using System;

namespace Steamworks
{
	// Token: 0x02000006 RID: 6
	public static class SteamGameServer
	{
		// Token: 0x06000093 RID: 147 RVA: 0x00003450 File Offset: 0x00001650
		public static void SetProduct(string pszProduct)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszProduct))
			{
				NativeMethods.ISteamGameServer_SetProduct(CSteamGameServerAPIContext.GetSteamGameServer(), utf8StringHandle);
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003490 File Offset: 0x00001690
		public static void SetGameDescription(string pszGameDescription)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszGameDescription))
			{
				NativeMethods.ISteamGameServer_SetGameDescription(CSteamGameServerAPIContext.GetSteamGameServer(), utf8StringHandle);
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000034D0 File Offset: 0x000016D0
		public static void SetModDir(string pszModDir)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszModDir))
			{
				NativeMethods.ISteamGameServer_SetModDir(CSteamGameServerAPIContext.GetSteamGameServer(), utf8StringHandle);
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003510 File Offset: 0x00001710
		public static void SetDedicatedServer(bool bDedicated)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetDedicatedServer(CSteamGameServerAPIContext.GetSteamGameServer(), bDedicated);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003524 File Offset: 0x00001724
		public static void LogOn(string pszToken)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszToken))
			{
				NativeMethods.ISteamGameServer_LogOn(CSteamGameServerAPIContext.GetSteamGameServer(), utf8StringHandle);
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003564 File Offset: 0x00001764
		public static void LogOnAnonymous()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_LogOnAnonymous(CSteamGameServerAPIContext.GetSteamGameServer());
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003575 File Offset: 0x00001775
		public static void LogOff()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_LogOff(CSteamGameServerAPIContext.GetSteamGameServer());
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003586 File Offset: 0x00001786
		public static bool BLoggedOn()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BLoggedOn(CSteamGameServerAPIContext.GetSteamGameServer());
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003597 File Offset: 0x00001797
		public static bool BSecure()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BSecure(CSteamGameServerAPIContext.GetSteamGameServer());
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000035A8 File Offset: 0x000017A8
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (CSteamID)NativeMethods.ISteamGameServer_GetSteamID(CSteamGameServerAPIContext.GetSteamGameServer());
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000035BE File Offset: 0x000017BE
		public static bool WasRestartRequested()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_WasRestartRequested(CSteamGameServerAPIContext.GetSteamGameServer());
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000035CF File Offset: 0x000017CF
		public static void SetMaxPlayerCount(int cPlayersMax)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetMaxPlayerCount(CSteamGameServerAPIContext.GetSteamGameServer(), cPlayersMax);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000035E1 File Offset: 0x000017E1
		public static void SetBotPlayerCount(int cBotplayers)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetBotPlayerCount(CSteamGameServerAPIContext.GetSteamGameServer(), cBotplayers);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000035F4 File Offset: 0x000017F4
		public static void SetServerName(string pszServerName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszServerName))
			{
				NativeMethods.ISteamGameServer_SetServerName(CSteamGameServerAPIContext.GetSteamGameServer(), utf8StringHandle);
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003634 File Offset: 0x00001834
		public static void SetMapName(string pszMapName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszMapName))
			{
				NativeMethods.ISteamGameServer_SetMapName(CSteamGameServerAPIContext.GetSteamGameServer(), utf8StringHandle);
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003674 File Offset: 0x00001874
		public static void SetPasswordProtected(bool bPasswordProtected)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetPasswordProtected(CSteamGameServerAPIContext.GetSteamGameServer(), bPasswordProtected);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003686 File Offset: 0x00001886
		public static void SetSpectatorPort(ushort unSpectatorPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetSpectatorPort(CSteamGameServerAPIContext.GetSteamGameServer(), unSpectatorPort);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003698 File Offset: 0x00001898
		public static void SetSpectatorServerName(string pszSpectatorServerName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszSpectatorServerName))
			{
				NativeMethods.ISteamGameServer_SetSpectatorServerName(CSteamGameServerAPIContext.GetSteamGameServer(), utf8StringHandle);
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000036D8 File Offset: 0x000018D8
		public static void ClearAllKeyValues()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_ClearAllKeyValues(CSteamGameServerAPIContext.GetSteamGameServer());
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000036EC File Offset: 0x000018EC
		public static void SetKeyValue(string pKey, string pValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pValue))
				{
					NativeMethods.ISteamGameServer_SetKeyValue(CSteamGameServerAPIContext.GetSteamGameServer(), utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000374C File Offset: 0x0000194C
		public static void SetGameTags(string pchGameTags)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchGameTags))
			{
				NativeMethods.ISteamGameServer_SetGameTags(CSteamGameServerAPIContext.GetSteamGameServer(), utf8StringHandle);
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000378C File Offset: 0x0000198C
		public static void SetGameData(string pchGameData)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchGameData))
			{
				NativeMethods.ISteamGameServer_SetGameData(CSteamGameServerAPIContext.GetSteamGameServer(), utf8StringHandle);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000037CC File Offset: 0x000019CC
		public static void SetRegion(string pszRegion)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszRegion))
			{
				NativeMethods.ISteamGameServer_SetRegion(CSteamGameServerAPIContext.GetSteamGameServer(), utf8StringHandle);
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000380C File Offset: 0x00001A0C
		public static void SetAdvertiseServerActive(bool bActive)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetAdvertiseServerActive(CSteamGameServerAPIContext.GetSteamGameServer(), bActive);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000381E File Offset: 0x00001A1E
		public static HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket, ref SteamNetworkingIdentity pSnid)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (HAuthTicket)NativeMethods.ISteamGameServer_GetAuthSessionTicket(CSteamGameServerAPIContext.GetSteamGameServer(), pTicket, cbMaxTicket, out pcbTicket, ref pSnid);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003838 File Offset: 0x00001A38
		public static EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BeginAuthSession(CSteamGameServerAPIContext.GetSteamGameServer(), pAuthTicket, cbAuthTicket, steamID);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000384C File Offset: 0x00001A4C
		public static void EndAuthSession(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_EndAuthSession(CSteamGameServerAPIContext.GetSteamGameServer(), steamID);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000385E File Offset: 0x00001A5E
		public static void CancelAuthTicket(HAuthTicket hAuthTicket)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_CancelAuthTicket(CSteamGameServerAPIContext.GetSteamGameServer(), hAuthTicket);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003870 File Offset: 0x00001A70
		public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_UserHasLicenseForApp(CSteamGameServerAPIContext.GetSteamGameServer(), steamID, appID);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003883 File Offset: 0x00001A83
		public static bool RequestUserGroupStatus(CSteamID steamIDUser, CSteamID steamIDGroup)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_RequestUserGroupStatus(CSteamGameServerAPIContext.GetSteamGameServer(), steamIDUser, steamIDGroup);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003896 File Offset: 0x00001A96
		public static void GetGameplayStats()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_GetGameplayStats(CSteamGameServerAPIContext.GetSteamGameServer());
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000038A7 File Offset: 0x00001AA7
		public static SteamAPICall_t GetServerReputation()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_GetServerReputation(CSteamGameServerAPIContext.GetSteamGameServer());
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000038BD File Offset: 0x00001ABD
		public static SteamIPAddress_t GetPublicIP()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_GetPublicIP(CSteamGameServerAPIContext.GetSteamGameServer());
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000038CE File Offset: 0x00001ACE
		public static bool HandleIncomingPacket(byte[] pData, int cbData, uint srcIP, ushort srcPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_HandleIncomingPacket(CSteamGameServerAPIContext.GetSteamGameServer(), pData, cbData, srcIP, srcPort);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000038E3 File Offset: 0x00001AE3
		public static int GetNextOutgoingPacket(byte[] pOut, int cbMaxOut, out uint pNetAdr, out ushort pPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_GetNextOutgoingPacket(CSteamGameServerAPIContext.GetSteamGameServer(), pOut, cbMaxOut, out pNetAdr, out pPort);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000038F8 File Offset: 0x00001AF8
		public static SteamAPICall_t AssociateWithClan(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_AssociateWithClan(CSteamGameServerAPIContext.GetSteamGameServer(), steamIDClan);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000390F File Offset: 0x00001B0F
		public static SteamAPICall_t ComputeNewPlayerCompatibility(CSteamID steamIDNewPlayer)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_ComputeNewPlayerCompatibility(CSteamGameServerAPIContext.GetSteamGameServer(), steamIDNewPlayer);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003926 File Offset: 0x00001B26
		public static bool SendUserConnectAndAuthenticate_DEPRECATED(uint unIPClient, byte[] pvAuthBlob, uint cubAuthBlobSize, out CSteamID pSteamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_SendUserConnectAndAuthenticate_DEPRECATED(CSteamGameServerAPIContext.GetSteamGameServer(), unIPClient, pvAuthBlob, cubAuthBlobSize, out pSteamIDUser);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000393B File Offset: 0x00001B3B
		public static CSteamID CreateUnauthenticatedUserConnection()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (CSteamID)NativeMethods.ISteamGameServer_CreateUnauthenticatedUserConnection(CSteamGameServerAPIContext.GetSteamGameServer());
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003951 File Offset: 0x00001B51
		public static void SendUserDisconnect_DEPRECATED(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SendUserDisconnect_DEPRECATED(CSteamGameServerAPIContext.GetSteamGameServer(), steamIDUser);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003964 File Offset: 0x00001B64
		public static bool BUpdateUserData(CSteamID steamIDUser, string pchPlayerName, uint uScore)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool flag;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPlayerName))
			{
				flag = NativeMethods.ISteamGameServer_BUpdateUserData(CSteamGameServerAPIContext.GetSteamGameServer(), steamIDUser, utf8StringHandle, uScore);
			}
			return flag;
		}
	}
}
