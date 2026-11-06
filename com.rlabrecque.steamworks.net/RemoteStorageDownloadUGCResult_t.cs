using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009D RID: 157
	[CallbackIdentity(1317)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageDownloadUGCResult_t
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x0000C2CF File Offset: 0x0000A4CF
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x0000C2DC File Offset: 0x0000A4DC
		public string m_pchFileName
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_pchFileName_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_pchFileName_, 260);
			}
		}

		// Token: 0x040001B8 RID: 440
		public const int k_iCallback = 1317;

		// Token: 0x040001B9 RID: 441
		public EResult m_eResult;

		// Token: 0x040001BA RID: 442
		public UGCHandle_t m_hFile;

		// Token: 0x040001BB RID: 443
		public AppId_t m_nAppID;

		// Token: 0x040001BC RID: 444
		public int m_nSizeInBytes;

		// Token: 0x040001BD RID: 445
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
		private byte[] m_pchFileName_;

		// Token: 0x040001BE RID: 446
		public ulong m_ulSteamIDOwner;
	}
}
