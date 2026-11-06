using System;

namespace Steamworks
{
	// Token: 0x02000111 RID: 273
	public enum ESNetSocketState
	{
		// Token: 0x040006A6 RID: 1702
		k_ESNetSocketStateInvalid,
		// Token: 0x040006A7 RID: 1703
		k_ESNetSocketStateConnected,
		// Token: 0x040006A8 RID: 1704
		k_ESNetSocketStateInitiated = 10,
		// Token: 0x040006A9 RID: 1705
		k_ESNetSocketStateLocalCandidatesFound,
		// Token: 0x040006AA RID: 1706
		k_ESNetSocketStateReceivedRemoteCandidates,
		// Token: 0x040006AB RID: 1707
		k_ESNetSocketStateChallengeHandshake = 15,
		// Token: 0x040006AC RID: 1708
		k_ESNetSocketStateDisconnecting = 21,
		// Token: 0x040006AD RID: 1709
		k_ESNetSocketStateLocalDisconnect,
		// Token: 0x040006AE RID: 1710
		k_ESNetSocketStateTimeoutDuringConnect,
		// Token: 0x040006AF RID: 1711
		k_ESNetSocketStateRemoteEndDisconnected,
		// Token: 0x040006B0 RID: 1712
		k_ESNetSocketStateConnectionBroken
	}
}
