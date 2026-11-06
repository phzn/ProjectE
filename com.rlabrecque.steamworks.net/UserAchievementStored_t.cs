using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D6 RID: 214
	[CallbackIdentity(1103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementStored_t
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0000C40C File Offset: 0x0000A60C
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x0000C419 File Offset: 0x0000A619
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

		// Token: 0x040002AA RID: 682
		public const int k_iCallback = 1103;

		// Token: 0x040002AB RID: 683
		public ulong m_nGameID;

		// Token: 0x040002AC RID: 684
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bGroupAchievement;

		// Token: 0x040002AD RID: 685
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_rgchAchievementName_;

		// Token: 0x040002AE RID: 686
		public uint m_nCurProgress;

		// Token: 0x040002AF RID: 687
		public uint m_nMaxProgress;
	}
}
