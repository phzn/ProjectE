using System;

namespace Steamworks
{
	// Token: 0x0200014E RID: 334
	[Flags]
	public enum EBetaBranchFlags
	{
		// Token: 0x04000975 RID: 2421
		k_EBetaBranch_None = 0,
		// Token: 0x04000976 RID: 2422
		k_EBetaBranch_Default = 1,
		// Token: 0x04000977 RID: 2423
		k_EBetaBranch_Available = 2,
		// Token: 0x04000978 RID: 2424
		k_EBetaBranch_Private = 4,
		// Token: 0x04000979 RID: 2425
		k_EBetaBranch_Selected = 8,
		// Token: 0x0400097A RID: 2426
		k_EBetaBranch_Installed = 16
	}
}
