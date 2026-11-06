using System;

namespace Steamworks
{
	// Token: 0x02000135 RID: 309
	public enum ESteamAPICallFailure
	{
		// Token: 0x04000821 RID: 2081
		k_ESteamAPICallFailureNone = -1,
		// Token: 0x04000822 RID: 2082
		k_ESteamAPICallFailureSteamGone,
		// Token: 0x04000823 RID: 2083
		k_ESteamAPICallFailureNetworkFailure,
		// Token: 0x04000824 RID: 2084
		k_ESteamAPICallFailureInvalidHandle,
		// Token: 0x04000825 RID: 2085
		k_ESteamAPICallFailureMismatchedCallback
	}
}
