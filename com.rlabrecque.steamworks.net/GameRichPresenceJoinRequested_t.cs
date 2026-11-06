using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000034 RID: 52
	[CallbackIdentity(337)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameRichPresenceJoinRequested_t
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0000C193 File Offset: 0x0000A393
		// (set) Token: 0x06000857 RID: 2135 RVA: 0x0000C1A0 File Offset: 0x0000A3A0
		public string m_rgchConnect
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchConnect_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchConnect_, 256);
			}
		}

		// Token: 0x0400002F RID: 47
		public const int k_iCallback = 337;

		// Token: 0x04000030 RID: 48
		public CSteamID m_steamIDFriend;

		// Token: 0x04000031 RID: 49
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_rgchConnect_;
	}
}
