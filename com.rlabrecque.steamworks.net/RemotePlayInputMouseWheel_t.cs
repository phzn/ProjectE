using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000167 RID: 359
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemotePlayInputMouseWheel_t
	{
		// Token: 0x04000AB1 RID: 2737
		public ERemotePlayMouseWheelDirection m_eDirection;

		// Token: 0x04000AB2 RID: 2738
		public float m_flAmount;
	}
}
