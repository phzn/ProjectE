using System;

namespace Steamworks
{
	// Token: 0x02000153 RID: 339
	public enum ESteamNetworkingAvailability
	{
		// Token: 0x040009CA RID: 2506
		k_ESteamNetworkingAvailability_CannotTry = -102,
		// Token: 0x040009CB RID: 2507
		k_ESteamNetworkingAvailability_Failed,
		// Token: 0x040009CC RID: 2508
		k_ESteamNetworkingAvailability_Previously,
		// Token: 0x040009CD RID: 2509
		k_ESteamNetworkingAvailability_Retrying = -10,
		// Token: 0x040009CE RID: 2510
		k_ESteamNetworkingAvailability_NeverTried = 1,
		// Token: 0x040009CF RID: 2511
		k_ESteamNetworkingAvailability_Waiting,
		// Token: 0x040009D0 RID: 2512
		k_ESteamNetworkingAvailability_Attempting,
		// Token: 0x040009D1 RID: 2513
		k_ESteamNetworkingAvailability_Current = 100,
		// Token: 0x040009D2 RID: 2514
		k_ESteamNetworkingAvailability_Unknown = 0,
		// Token: 0x040009D3 RID: 2515
		k_ESteamNetworkingAvailability__Force32bit = 2147483647
	}
}
