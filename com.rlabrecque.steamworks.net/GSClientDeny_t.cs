using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000045 RID: 69
	[CallbackIdentity(202)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSClientDeny_t
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x0000C1D3 File Offset: 0x0000A3D3
		// (set) Token: 0x0600085B RID: 2139 RVA: 0x0000C1E0 File Offset: 0x0000A3E0
		public string m_rgchOptionalText
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchOptionalText_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchOptionalText_, 128);
			}
		}

		// Token: 0x04000067 RID: 103
		public const int k_iCallback = 202;

		// Token: 0x04000068 RID: 104
		public CSteamID m_SteamID;

		// Token: 0x04000069 RID: 105
		public EDenyReason m_eDenyReason;

		// Token: 0x0400006A RID: 106
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		private byte[] m_rgchOptionalText_;
	}
}
