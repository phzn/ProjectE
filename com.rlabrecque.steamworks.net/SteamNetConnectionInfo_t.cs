using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200016E RID: 366
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetConnectionInfo_t
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x0000C51C File Offset: 0x0000A71C
		// (set) Token: 0x06000890 RID: 2192 RVA: 0x0000C529 File Offset: 0x0000A729
		public string m_szEndDebug
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_szEndDebug_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_szEndDebug_, 128);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x0000C53C File Offset: 0x0000A73C
		// (set) Token: 0x06000892 RID: 2194 RVA: 0x0000C549 File Offset: 0x0000A749
		public string m_szConnectionDescription
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_szConnectionDescription_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_szConnectionDescription_, 128);
			}
		}

		// Token: 0x04000ADE RID: 2782
		public SteamNetworkingIdentity m_identityRemote;

		// Token: 0x04000ADF RID: 2783
		public long m_nUserData;

		// Token: 0x04000AE0 RID: 2784
		public HSteamListenSocket m_hListenSocket;

		// Token: 0x04000AE1 RID: 2785
		public SteamNetworkingIPAddr m_addrRemote;

		// Token: 0x04000AE2 RID: 2786
		public ushort m__pad1;

		// Token: 0x04000AE3 RID: 2787
		public SteamNetworkingPOPID m_idPOPRemote;

		// Token: 0x04000AE4 RID: 2788
		public SteamNetworkingPOPID m_idPOPRelay;

		// Token: 0x04000AE5 RID: 2789
		public ESteamNetworkingConnectionState m_eState;

		// Token: 0x04000AE6 RID: 2790
		public int m_eEndReason;

		// Token: 0x04000AE7 RID: 2791
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_szEndDebug_;

		// Token: 0x04000AE8 RID: 2792
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_szConnectionDescription_;

		// Token: 0x04000AE9 RID: 2793
		public int m_nFlags;

		// Token: 0x04000AEA RID: 2794
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 63)]
		public uint[] reserved;
	}
}
