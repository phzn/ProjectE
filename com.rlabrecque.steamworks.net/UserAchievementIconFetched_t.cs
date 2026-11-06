using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DC RID: 220
	[CallbackIdentity(1109)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementIconFetched_t
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x0000C42C File Offset: 0x0000A62C
		// (set) Token: 0x06000881 RID: 2177 RVA: 0x0000C439 File Offset: 0x0000A639
		public string m_rgchAchievementName
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchAchievementName_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchAchievementName_, 128);
			}
		}

		// Token: 0x040002C3 RID: 707
		public const int k_iCallback = 1109;

		// Token: 0x040002C4 RID: 708
		public CGameID m_nGameID;

		// Token: 0x040002C5 RID: 709
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_rgchAchievementName_;

		// Token: 0x040002C6 RID: 710
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAchieved;

		// Token: 0x040002C7 RID: 711
		public int m_nIconHandle;
	}
}
