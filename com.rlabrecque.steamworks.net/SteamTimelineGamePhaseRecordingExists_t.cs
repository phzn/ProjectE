using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B0 RID: 176
	[CallbackIdentity(6001)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamTimelineGamePhaseRecordingExists_t
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x0000C38F File Offset: 0x0000A58F
		// (set) Token: 0x06000877 RID: 2167 RVA: 0x0000C39C File Offset: 0x0000A59C
		public string m_rgchPhaseID
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchPhaseID_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchPhaseID_, 64);
			}
		}

		// Token: 0x04000217 RID: 535
		public const int k_iCallback = 6001;

		// Token: 0x04000218 RID: 536
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_rgchPhaseID_;

		// Token: 0x04000219 RID: 537
		public ulong m_ulRecordingMS;

		// Token: 0x0400021A RID: 538
		public ulong m_ulLongestClipMS;

		// Token: 0x0400021B RID: 539
		public uint m_unClipCount;

		// Token: 0x0400021C RID: 540
		public uint m_unScreenshotCount;
	}
}
