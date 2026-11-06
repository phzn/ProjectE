using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E9 RID: 233
	[CallbackIdentity(4611)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetVideoURLResult_t
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x0000C44C File Offset: 0x0000A64C
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x0000C459 File Offset: 0x0000A659
		public string m_rgchURL
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchURL_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchURL_, 256);
			}
		}

		// Token: 0x040002E3 RID: 739
		public const int k_iCallback = 4611;

		// Token: 0x040002E4 RID: 740
		public EResult m_eResult;

		// Token: 0x040002E5 RID: 741
		public AppId_t m_unVideoAppID;

		// Token: 0x040002E6 RID: 742
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_rgchURL_;
	}
}
