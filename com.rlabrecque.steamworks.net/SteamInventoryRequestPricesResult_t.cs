using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000074 RID: 116
	[CallbackIdentity(4705)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryRequestPricesResult_t
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x0000C213 File Offset: 0x0000A413
		// (set) Token: 0x0600085F RID: 2143 RVA: 0x0000C220 File Offset: 0x0000A420
		public string m_rgchCurrency
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchCurrency_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchCurrency_, 4);
			}
		}

		// Token: 0x0400012D RID: 301
		public const int k_iCallback = 4705;

		// Token: 0x0400012E RID: 302
		public EResult m_result;

		// Token: 0x0400012F RID: 303
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] m_rgchCurrency_;
	}
}
