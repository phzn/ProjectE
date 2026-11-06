using System;

namespace Steamworks
{
	// Token: 0x020000F1 RID: 241
	[Flags]
	public enum EFriendFlags
	{
		// Token: 0x040003A6 RID: 934
		k_EFriendFlagNone = 0,
		// Token: 0x040003A7 RID: 935
		k_EFriendFlagBlocked = 1,
		// Token: 0x040003A8 RID: 936
		k_EFriendFlagFriendshipRequested = 2,
		// Token: 0x040003A9 RID: 937
		k_EFriendFlagImmediate = 4,
		// Token: 0x040003AA RID: 938
		k_EFriendFlagClanMember = 8,
		// Token: 0x040003AB RID: 939
		k_EFriendFlagOnGameServer = 16,
		// Token: 0x040003AC RID: 940
		k_EFriendFlagRequestingFriendship = 128,
		// Token: 0x040003AD RID: 941
		k_EFriendFlagRequestingInfo = 256,
		// Token: 0x040003AE RID: 942
		k_EFriendFlagIgnored = 512,
		// Token: 0x040003AF RID: 943
		k_EFriendFlagIgnoredFriend = 1024,
		// Token: 0x040003B0 RID: 944
		k_EFriendFlagChatMember = 4096,
		// Token: 0x040003B1 RID: 945
		k_EFriendFlagAll = 65535
	}
}
