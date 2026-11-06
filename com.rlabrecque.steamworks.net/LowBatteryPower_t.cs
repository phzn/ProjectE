using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E1 RID: 225
	[CallbackIdentity(702)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LowBatteryPower_t
	{
		// Token: 0x040002D2 RID: 722
		public const int k_iCallback = 702;

		// Token: 0x040002D3 RID: 723
		public byte m_nMinutesBatteryLeft;
	}
}
