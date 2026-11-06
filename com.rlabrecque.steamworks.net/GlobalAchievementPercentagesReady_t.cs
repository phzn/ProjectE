using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DD RID: 221
	[CallbackIdentity(1110)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GlobalAchievementPercentagesReady_t
	{
		// Token: 0x040002C8 RID: 712
		public const int k_iCallback = 1110;

		// Token: 0x040002C9 RID: 713
		public ulong m_nGameID;

		// Token: 0x040002CA RID: 714
		public EResult m_eResult;
	}
}
