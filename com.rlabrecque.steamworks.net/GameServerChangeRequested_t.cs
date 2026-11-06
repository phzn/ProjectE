using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200002F RID: 47
	[CallbackIdentity(332)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameServerChangeRequested_t
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x0000C159 File Offset: 0x0000A359
		// (set) Token: 0x06000853 RID: 2131 RVA: 0x0000C166 File Offset: 0x0000A366
		public string m_rgchServer
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchServer_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchServer_, 64);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x0000C176 File Offset: 0x0000A376
		// (set) Token: 0x06000855 RID: 2133 RVA: 0x0000C183 File Offset: 0x0000A383
		public string m_rgchPassword
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchPassword_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchPassword_, 64);
			}
		}

		// Token: 0x0400001D RID: 29
		public const int k_iCallback = 332;

		// Token: 0x0400001E RID: 30
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_rgchServer_;

		// Token: 0x0400001F RID: 31
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_rgchPassword_;
	}
}
