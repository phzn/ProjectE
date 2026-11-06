using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200018E RID: 398
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamDatagramHostedAddress
	{
		// Token: 0x060009A4 RID: 2468 RVA: 0x0000F4FC File Offset: 0x0000D6FC
		public void Clear()
		{
			this.m_cbSize = 0;
			this.m_data = new byte[128];
		}

		// Token: 0x04000B75 RID: 2933
		public int m_cbSize;

		// Token: 0x04000B76 RID: 2934
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public byte[] m_data;
	}
}
