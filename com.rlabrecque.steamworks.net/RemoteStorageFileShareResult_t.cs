using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000095 RID: 149
	[CallbackIdentity(1307)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileShareResult_t
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x0000C2AF File Offset: 0x0000A4AF
		// (set) Token: 0x06000869 RID: 2153 RVA: 0x0000C2BC File Offset: 0x0000A4BC
		public string m_rgchFilename
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchFilename_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchFilename_, 260);
			}
		}

		// Token: 0x04000198 RID: 408
		public const int k_iCallback = 1307;

		// Token: 0x04000199 RID: 409
		public EResult m_eResult;

		// Token: 0x0400019A RID: 410
		public UGCHandle_t m_hFile;

		// Token: 0x0400019B RID: 411
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
		private byte[] m_rgchFilename_;
	}
}
