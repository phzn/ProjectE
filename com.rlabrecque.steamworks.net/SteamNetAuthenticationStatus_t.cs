using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200008E RID: 142
	[CallbackIdentity(1222)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetAuthenticationStatus_t
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x0000C24F File Offset: 0x0000A44F
		// (set) Token: 0x06000863 RID: 2147 RVA: 0x0000C25C File Offset: 0x0000A45C
		public string m_debugMsg
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_debugMsg_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_debugMsg_, 256);
			}
		}

		// Token: 0x04000183 RID: 387
		public const int k_iCallback = 1222;

		// Token: 0x04000184 RID: 388
		public ESteamNetworkingAvailability m_eAvail;

		// Token: 0x04000185 RID: 389
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_debugMsg_;
	}
}
