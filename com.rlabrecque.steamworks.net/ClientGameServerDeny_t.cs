using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C8 RID: 200
	[CallbackIdentity(113)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ClientGameServerDeny_t
	{
		// Token: 0x04000275 RID: 629
		public const int k_iCallback = 113;

		// Token: 0x04000276 RID: 630
		public uint m_uAppID;

		// Token: 0x04000277 RID: 631
		public uint m_unGameServerIP;

		// Token: 0x04000278 RID: 632
		public ushort m_usGameServerPort;

		// Token: 0x04000279 RID: 633
		public ushort m_bSecure;

		// Token: 0x0400027A RID: 634
		public uint m_uReason;
	}
}
