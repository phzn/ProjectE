using System;

namespace Steamworks
{
	// Token: 0x0200010B RID: 267
	[Flags]
	public enum EChatMemberStateChange
	{
		// Token: 0x04000685 RID: 1669
		k_EChatMemberStateChangeEntered = 1,
		// Token: 0x04000686 RID: 1670
		k_EChatMemberStateChangeLeft = 2,
		// Token: 0x04000687 RID: 1671
		k_EChatMemberStateChangeDisconnected = 4,
		// Token: 0x04000688 RID: 1672
		k_EChatMemberStateChangeKicked = 8,
		// Token: 0x04000689 RID: 1673
		k_EChatMemberStateChangeBanned = 16
	}
}
