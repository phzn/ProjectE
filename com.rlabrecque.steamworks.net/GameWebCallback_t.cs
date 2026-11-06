using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CF RID: 207
	[CallbackIdentity(164)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameWebCallback_t
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x0000C3CC File Offset: 0x0000A5CC
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x0000C3D9 File Offset: 0x0000A5D9
		public string m_szURL
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_szURL_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_szURL_, 256);
			}
		}

		// Token: 0x0400028B RID: 651
		public const int k_iCallback = 164;

		// Token: 0x0400028C RID: 652
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_szURL_;
	}
}
