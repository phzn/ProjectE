using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000081 RID: 129
	[CallbackIdentity(5302)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CreateBeaconCallback_t
	{
		// Token: 0x04000164 RID: 356
		public const int k_iCallback = 5302;

		// Token: 0x04000165 RID: 357
		public EResult m_eResult;

		// Token: 0x04000166 RID: 358
		public PartyBeaconID_t m_ulBeaconID;
	}
}
