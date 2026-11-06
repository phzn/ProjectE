using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200002E RID: 46
	[CallbackIdentity(331)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameOverlayActivated_t
	{
		// Token: 0x04000018 RID: 24
		public const int k_iCallback = 331;

		// Token: 0x04000019 RID: 25
		public byte m_bActive;

		// Token: 0x0400001A RID: 26
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserInitiated;

		// Token: 0x0400001B RID: 27
		public AppId_t m_nAppID;

		// Token: 0x0400001C RID: 28
		public uint m_dwOverlayPID;
	}
}
