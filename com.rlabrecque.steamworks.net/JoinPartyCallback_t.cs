using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000080 RID: 128
	[CallbackIdentity(5301)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct JoinPartyCallback_t
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x0000C22F File Offset: 0x0000A42F
		// (set) Token: 0x06000861 RID: 2145 RVA: 0x0000C23C File Offset: 0x0000A43C
		public string m_rgchConnectString
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchConnectString_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchConnectString_, 256);
			}
		}

		// Token: 0x0400015F RID: 351
		public const int k_iCallback = 5301;

		// Token: 0x04000160 RID: 352
		public EResult m_eResult;

		// Token: 0x04000161 RID: 353
		public PartyBeaconID_t m_ulBeaconID;

		// Token: 0x04000162 RID: 354
		public CSteamID m_SteamIDBeaconOwner;

		// Token: 0x04000163 RID: 355
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_rgchConnectString_;
	}
}
