using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200003F RID: 63
	[CallbackIdentity(349)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct OverlayBrowserProtocolNavigation_t
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x0000C1B3 File Offset: 0x0000A3B3
		// (set) Token: 0x06000859 RID: 2137 RVA: 0x0000C1C0 File Offset: 0x0000A3C0
		public string rgchURI
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.rgchURI_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.rgchURI_, 1024);
			}
		}

		// Token: 0x04000054 RID: 84
		public const int k_iCallback = 349;

		// Token: 0x04000055 RID: 85
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		private byte[] rgchURI_;
	}
}
