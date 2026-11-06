using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000031 RID: 49
	[CallbackIdentity(334)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AvatarImageLoaded_t
	{
		// Token: 0x04000023 RID: 35
		public const int k_iCallback = 334;

		// Token: 0x04000024 RID: 36
		public CSteamID m_steamID;

		// Token: 0x04000025 RID: 37
		public int m_iImage;

		// Token: 0x04000026 RID: 38
		public int m_iWide;

		// Token: 0x04000027 RID: 39
		public int m_iTall;
	}
}
