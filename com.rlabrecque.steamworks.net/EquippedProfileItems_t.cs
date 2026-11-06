using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000041 RID: 65
	[CallbackIdentity(351)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct EquippedProfileItems_t
	{
		// Token: 0x04000058 RID: 88
		public const int k_iCallback = 351;

		// Token: 0x04000059 RID: 89
		public EResult m_eResult;

		// Token: 0x0400005A RID: 90
		public CSteamID m_steamID;

		// Token: 0x0400005B RID: 91
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHasAnimatedAvatar;

		// Token: 0x0400005C RID: 92
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHasAvatarFrame;

		// Token: 0x0400005D RID: 93
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHasProfileModifier;

		// Token: 0x0400005E RID: 94
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHasProfileBackground;

		// Token: 0x0400005F RID: 95
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bHasMiniProfileBackground;

		// Token: 0x04000060 RID: 96
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bFromCache;
	}
}
