using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000CD RID: 205
	[CallbackIdentity(154)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct EncryptedAppTicketResponse_t
	{
		// Token: 0x04000286 RID: 646
		public const int k_iCallback = 154;

		// Token: 0x04000287 RID: 647
		public EResult m_eResult;
	}
}
