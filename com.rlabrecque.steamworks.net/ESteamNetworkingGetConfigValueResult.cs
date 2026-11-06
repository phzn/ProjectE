using System;

namespace Steamworks
{
	// Token: 0x0200015B RID: 347
	public enum ESteamNetworkingGetConfigValueResult
	{
		// Token: 0x04000A6E RID: 2670
		k_ESteamNetworkingGetConfigValue_BadValue = -1,
		// Token: 0x04000A6F RID: 2671
		k_ESteamNetworkingGetConfigValue_BadScopeObj = -2,
		// Token: 0x04000A70 RID: 2672
		k_ESteamNetworkingGetConfigValue_BufferTooSmall = -3,
		// Token: 0x04000A71 RID: 2673
		k_ESteamNetworkingGetConfigValue_OK = 1,
		// Token: 0x04000A72 RID: 2674
		k_ESteamNetworkingGetConfigValue_OKInherited,
		// Token: 0x04000A73 RID: 2675
		k_ESteamNetworkingGetConfigValueResult__Force32Bit = 2147483647
	}
}
