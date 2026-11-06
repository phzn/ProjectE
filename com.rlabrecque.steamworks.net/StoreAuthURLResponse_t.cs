using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D0 RID: 208
	[CallbackIdentity(165)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StoreAuthURLResponse_t
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x0000C3EC File Offset: 0x0000A5EC
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x0000C3F9 File Offset: 0x0000A5F9
		public string m_szURL
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_szURL_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_szURL_, 512);
			}
		}

		// Token: 0x0400028D RID: 653
		public const int k_iCallback = 165;

		// Token: 0x0400028E RID: 654
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
		private byte[] m_szURL_;
	}
}
