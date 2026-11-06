using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004C RID: 76
	[CallbackIdentity(210)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct AssociateWithClanResult_t
	{
		// Token: 0x04000086 RID: 134
		public const int k_iCallback = 210;

		// Token: 0x04000087 RID: 135
		public EResult m_eResult;
	}
}
