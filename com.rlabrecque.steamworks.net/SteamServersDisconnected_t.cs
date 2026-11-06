using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C7 RID: 199
	[CallbackIdentity(103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamServersDisconnected_t
	{
		// Token: 0x04000273 RID: 627
		public const int k_iCallback = 103;

		// Token: 0x04000274 RID: 628
		public EResult m_eResult;
	}
}
