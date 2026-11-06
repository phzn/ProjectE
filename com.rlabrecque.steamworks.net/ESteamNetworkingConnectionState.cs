using System;

namespace Steamworks
{
	// Token: 0x02000156 RID: 342
	public enum ESteamNetworkingConnectionState
	{
		// Token: 0x040009E5 RID: 2533
		k_ESteamNetworkingConnectionState_None,
		// Token: 0x040009E6 RID: 2534
		k_ESteamNetworkingConnectionState_Connecting,
		// Token: 0x040009E7 RID: 2535
		k_ESteamNetworkingConnectionState_FindingRoute,
		// Token: 0x040009E8 RID: 2536
		k_ESteamNetworkingConnectionState_Connected,
		// Token: 0x040009E9 RID: 2537
		k_ESteamNetworkingConnectionState_ClosedByPeer,
		// Token: 0x040009EA RID: 2538
		k_ESteamNetworkingConnectionState_ProblemDetectedLocally,
		// Token: 0x040009EB RID: 2539
		k_ESteamNetworkingConnectionState_FinWait = -1,
		// Token: 0x040009EC RID: 2540
		k_ESteamNetworkingConnectionState_Linger = -2,
		// Token: 0x040009ED RID: 2541
		k_ESteamNetworkingConnectionState_Dead = -3,
		// Token: 0x040009EE RID: 2542
		k_ESteamNetworkingConnectionState__Force32Bit = 2147483647
	}
}
