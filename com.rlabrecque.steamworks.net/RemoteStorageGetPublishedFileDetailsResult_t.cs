using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009E RID: 158
	[CallbackIdentity(1318)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedFileDetailsResult_t
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x0000C2EF File Offset: 0x0000A4EF
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x0000C2FC File Offset: 0x0000A4FC
		public string m_rgchTitle
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchTitle_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchTitle_, 129);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x0000C30F File Offset: 0x0000A50F
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x0000C31C File Offset: 0x0000A51C
		public string m_rgchDescription
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchDescription_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchDescription_, 8000);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x0000C32F File Offset: 0x0000A52F
		// (set) Token: 0x06000871 RID: 2161 RVA: 0x0000C33C File Offset: 0x0000A53C
		public string m_rgchTags
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchTags_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchTags_, 1025);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0000C34F File Offset: 0x0000A54F
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x0000C35C File Offset: 0x0000A55C
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

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0000C36F File Offset: 0x0000A56F
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x0000C37C File Offset: 0x0000A57C
		public string m_rgchURL
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchURL_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchURL_, 256);
			}
		}

		// Token: 0x040001BF RID: 447
		public const int k_iCallback = 1318;

		// Token: 0x040001C0 RID: 448
		public EResult m_eResult;

		// Token: 0x040001C1 RID: 449
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040001C2 RID: 450
		public AppId_t m_nCreatorAppID;

		// Token: 0x040001C3 RID: 451
		public AppId_t m_nConsumerAppID;

		// Token: 0x040001C4 RID: 452
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
		private byte[] m_rgchTitle_;

		// Token: 0x040001C5 RID: 453
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8000)]
		private byte[] m_rgchDescription_;

		// Token: 0x040001C6 RID: 454
		public UGCHandle_t m_hFile;

		// Token: 0x040001C7 RID: 455
		public UGCHandle_t m_hPreviewFile;

		// Token: 0x040001C8 RID: 456
		public ulong m_ulSteamIDOwner;

		// Token: 0x040001C9 RID: 457
		public uint m_rtimeCreated;

		// Token: 0x040001CA RID: 458
		public uint m_rtimeUpdated;

		// Token: 0x040001CB RID: 459
		public ERemoteStoragePublishedFileVisibility m_eVisibility;

		// Token: 0x040001CC RID: 460
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x040001CD RID: 461
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1025)]
		private byte[] m_rgchTags_;

		// Token: 0x040001CE RID: 462
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bTagsTruncated;

		// Token: 0x040001CF RID: 463
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
		private byte[] m_pchFileName_;

		// Token: 0x040001D0 RID: 464
		public int m_nFileSize;

		// Token: 0x040001D1 RID: 465
		public int m_nPreviewFileSize;

		// Token: 0x040001D2 RID: 466
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_rgchURL_;

		// Token: 0x040001D3 RID: 467
		public EWorkshopFileType m_eFileType;

		// Token: 0x040001D4 RID: 468
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAcceptedForUse;
	}
}
