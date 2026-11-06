using System;

namespace Steamworks
{
	// Token: 0x02000157 RID: 343
	public enum ESteamNetConnectionEnd
	{
		// Token: 0x040009F0 RID: 2544
		k_ESteamNetConnectionEnd_Invalid,
		// Token: 0x040009F1 RID: 2545
		k_ESteamNetConnectionEnd_App_Min = 1000,
		// Token: 0x040009F2 RID: 2546
		k_ESteamNetConnectionEnd_App_Generic = 1000,
		// Token: 0x040009F3 RID: 2547
		k_ESteamNetConnectionEnd_App_Max = 1999,
		// Token: 0x040009F4 RID: 2548
		k_ESteamNetConnectionEnd_AppException_Min,
		// Token: 0x040009F5 RID: 2549
		k_ESteamNetConnectionEnd_AppException_Generic = 2000,
		// Token: 0x040009F6 RID: 2550
		k_ESteamNetConnectionEnd_AppException_Max = 2999,
		// Token: 0x040009F7 RID: 2551
		k_ESteamNetConnectionEnd_Local_Min,
		// Token: 0x040009F8 RID: 2552
		k_ESteamNetConnectionEnd_Local_OfflineMode,
		// Token: 0x040009F9 RID: 2553
		k_ESteamNetConnectionEnd_Local_ManyRelayConnectivity,
		// Token: 0x040009FA RID: 2554
		k_ESteamNetConnectionEnd_Local_HostedServerPrimaryRelay,
		// Token: 0x040009FB RID: 2555
		k_ESteamNetConnectionEnd_Local_NetworkConfig,
		// Token: 0x040009FC RID: 2556
		k_ESteamNetConnectionEnd_Local_Rights,
		// Token: 0x040009FD RID: 2557
		k_ESteamNetConnectionEnd_Local_P2P_ICE_NoPublicAddresses,
		// Token: 0x040009FE RID: 2558
		k_ESteamNetConnectionEnd_Local_Max = 3999,
		// Token: 0x040009FF RID: 2559
		k_ESteamNetConnectionEnd_Remote_Min,
		// Token: 0x04000A00 RID: 2560
		k_ESteamNetConnectionEnd_Remote_Timeout,
		// Token: 0x04000A01 RID: 2561
		k_ESteamNetConnectionEnd_Remote_BadCrypt,
		// Token: 0x04000A02 RID: 2562
		k_ESteamNetConnectionEnd_Remote_BadCert,
		// Token: 0x04000A03 RID: 2563
		k_ESteamNetConnectionEnd_Remote_BadProtocolVersion = 4006,
		// Token: 0x04000A04 RID: 2564
		k_ESteamNetConnectionEnd_Remote_P2P_ICE_NoPublicAddresses,
		// Token: 0x04000A05 RID: 2565
		k_ESteamNetConnectionEnd_Remote_Max = 4999,
		// Token: 0x04000A06 RID: 2566
		k_ESteamNetConnectionEnd_Misc_Min,
		// Token: 0x04000A07 RID: 2567
		k_ESteamNetConnectionEnd_Misc_Generic,
		// Token: 0x04000A08 RID: 2568
		k_ESteamNetConnectionEnd_Misc_InternalError,
		// Token: 0x04000A09 RID: 2569
		k_ESteamNetConnectionEnd_Misc_Timeout,
		// Token: 0x04000A0A RID: 2570
		k_ESteamNetConnectionEnd_Misc_SteamConnectivity = 5005,
		// Token: 0x04000A0B RID: 2571
		k_ESteamNetConnectionEnd_Misc_NoRelaySessionsToClient,
		// Token: 0x04000A0C RID: 2572
		k_ESteamNetConnectionEnd_Misc_P2P_Rendezvous = 5008,
		// Token: 0x04000A0D RID: 2573
		k_ESteamNetConnectionEnd_Misc_P2P_NAT_Firewall,
		// Token: 0x04000A0E RID: 2574
		k_ESteamNetConnectionEnd_Misc_PeerSentNoConnection,
		// Token: 0x04000A0F RID: 2575
		k_ESteamNetConnectionEnd_Misc_Max = 5999,
		// Token: 0x04000A10 RID: 2576
		k_ESteamNetConnectionEnd__Force32Bit = 2147483647
	}
}
