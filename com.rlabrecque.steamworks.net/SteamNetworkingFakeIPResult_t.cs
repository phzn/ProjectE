using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000ED RID: 237
	[CallbackIdentity(1223)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamNetworkingFakeIPResult_t
	{
		// Token: 0x040002EE RID: 750
		public const int k_iCallback = 1223;

		// Token: 0x040002EF RID: 751
		public EResult m_eResult;

		// Token: 0x040002F0 RID: 752
		public SteamNetworkingIdentity m_identity;

		// Token: 0x040002F1 RID: 753
		public uint m_unIP;

		// Token: 0x040002F2 RID: 754
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public ushort[] m_unPorts;
	}
}
