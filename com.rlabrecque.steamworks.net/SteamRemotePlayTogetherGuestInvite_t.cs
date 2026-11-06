using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000093 RID: 147
	[CallbackIdentity(5703)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamRemotePlayTogetherGuestInvite_t
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x0000C28F File Offset: 0x0000A48F
		// (set) Token: 0x06000867 RID: 2151 RVA: 0x0000C29C File Offset: 0x0000A49C
		public string m_szConnectURL
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_szConnectURL_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_szConnectURL_, 1024);
			}
		}

		// Token: 0x04000191 RID: 401
		public const int k_iCallback = 5703;

		// Token: 0x04000192 RID: 402
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		private byte[] m_szConnectURL_;
	}
}
