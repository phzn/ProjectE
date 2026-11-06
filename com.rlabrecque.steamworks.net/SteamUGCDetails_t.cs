using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200016A RID: 362
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCDetails_t
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x0000C46C File Offset: 0x0000A66C
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x0000C479 File Offset: 0x0000A679
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0000C48C File Offset: 0x0000A68C
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x0000C499 File Offset: 0x0000A699
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0000C4AC File Offset: 0x0000A6AC
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x0000C4B9 File Offset: 0x0000A6B9
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

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0000C4CC File Offset: 0x0000A6CC
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x0000C4D9 File Offset: 0x0000A6D9
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

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0000C4EC File Offset: 0x0000A6EC
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x0000C4F9 File Offset: 0x0000A6F9
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

		// Token: 0x04000AB8 RID: 2744
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000AB9 RID: 2745
		public EResult m_eResult;

		// Token: 0x04000ABA RID: 2746
		public EWorkshopFileType m_eFileType;

		// Token: 0x04000ABB RID: 2747
		public AppId_t m_nCreatorAppID;

		// Token: 0x04000ABC RID: 2748
		public AppId_t m_nConsumerAppID;

		// Token: 0x04000ABD RID: 2749
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)]
		private byte[] m_rgchTitle_;

		// Token: 0x04000ABE RID: 2750
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8000)]
		private byte[] m_rgchDescription_;

		// Token: 0x04000ABF RID: 2751
		public ulong m_ulSteamIDOwner;

		// Token: 0x04000AC0 RID: 2752
		public uint m_rtimeCreated;

		// Token: 0x04000AC1 RID: 2753
		public uint m_rtimeUpdated;

		// Token: 0x04000AC2 RID: 2754
		public uint m_rtimeAddedToUserList;

		// Token: 0x04000AC3 RID: 2755
		public ERemoteStoragePublishedFileVisibility m_eVisibility;

		// Token: 0x04000AC4 RID: 2756
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x04000AC5 RID: 2757
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAcceptedForUse;

		// Token: 0x04000AC6 RID: 2758
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bTagsTruncated;

		// Token: 0x04000AC7 RID: 2759
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1025)]
		private byte[] m_rgchTags_;

		// Token: 0x04000AC8 RID: 2760
		public UGCHandle_t m_hFile;

		// Token: 0x04000AC9 RID: 2761
		public UGCHandle_t m_hPreviewFile;

		// Token: 0x04000ACA RID: 2762
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
		private byte[] m_pchFileName_;

		// Token: 0x04000ACB RID: 2763
		public int m_nFileSize;

		// Token: 0x04000ACC RID: 2764
		public int m_nPreviewFileSize;

		// Token: 0x04000ACD RID: 2765
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] m_rgchURL_;

		// Token: 0x04000ACE RID: 2766
		public uint m_unVotesUp;

		// Token: 0x04000ACF RID: 2767
		public uint m_unVotesDown;

		// Token: 0x04000AD0 RID: 2768
		public float m_flScore;

		// Token: 0x04000AD1 RID: 2769
		public uint m_unNumChildren;

		// Token: 0x04000AD2 RID: 2770
		public ulong m_ulTotalFilesSize;
	}
}
