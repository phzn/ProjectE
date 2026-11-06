using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200016D RID: 365
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CallbackMsg_t
	{
		// Token: 0x04000ADA RID: 2778
		public int m_hSteamUser;

		// Token: 0x04000ADB RID: 2779
		public int m_iCallback;

		// Token: 0x04000ADC RID: 2780
		public IntPtr m_pubParam;

		// Token: 0x04000ADD RID: 2781
		public int m_cubParam;
	}
}
