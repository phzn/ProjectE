using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000047 RID: 71
	[CallbackIdentity(206)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSClientAchievementStatus_t
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x0000C1F3 File Offset: 0x0000A3F3
		// (set) Token: 0x0600085D RID: 2141 RVA: 0x0000C200 File Offset: 0x0000A400
		public string m_pchAchievement
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_pchAchievement_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_pchAchievement_, 128);
			}
		}

		// Token: 0x0400006E RID: 110
		public const int k_iCallback = 206;

		// Token: 0x0400006F RID: 111
		public ulong m_SteamID;

		// Token: 0x04000070 RID: 112
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_pchAchievement_;

		// Token: 0x04000071 RID: 113
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUnlocked;
	}
}
