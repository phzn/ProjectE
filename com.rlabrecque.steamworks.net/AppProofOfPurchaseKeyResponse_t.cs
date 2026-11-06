using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200002A RID: 42
	[CallbackIdentity(1021)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AppProofOfPurchaseKeyResponse_t
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x0000C139 File Offset: 0x0000A339
		// (set) Token: 0x06000851 RID: 2129 RVA: 0x0000C146 File Offset: 0x0000A346
		public string m_rgchKey
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchKey_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchKey_, 240);
			}
		}

		// Token: 0x04000006 RID: 6
		public const int k_iCallback = 1021;

		// Token: 0x04000007 RID: 7
		public EResult m_eResult;

		// Token: 0x04000008 RID: 8
		public uint m_nAppID;

		// Token: 0x04000009 RID: 9
		public uint m_cchKeyLength;

		// Token: 0x0400000A RID: 10
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 240)]
		private byte[] m_rgchKey_;
	}
}
