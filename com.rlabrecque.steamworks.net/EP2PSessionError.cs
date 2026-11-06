using System;

namespace Steamworks
{
	// Token: 0x0200010F RID: 271
	public enum EP2PSessionError
	{
		// Token: 0x0400069A RID: 1690
		k_EP2PSessionErrorNone,
		// Token: 0x0400069B RID: 1691
		k_EP2PSessionErrorNoRightsToApp = 2,
		// Token: 0x0400069C RID: 1692
		k_EP2PSessionErrorTimeout = 4,
		// Token: 0x0400069D RID: 1693
		k_EP2PSessionErrorNotRunningApp_DELETED = 1,
		// Token: 0x0400069E RID: 1694
		k_EP2PSessionErrorDestinationNotLoggedIn_DELETED = 3,
		// Token: 0x0400069F RID: 1695
		k_EP2PSessionErrorMax = 5
	}
}
