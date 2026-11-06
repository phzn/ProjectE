using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000083 RID: 131
	[CallbackIdentity(5304)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ChangeNumOpenSlotsCallback_t
	{
		// Token: 0x0400016A RID: 362
		public const int k_iCallback = 5304;

		// Token: 0x0400016B RID: 363
		public EResult m_eResult;
	}
}
