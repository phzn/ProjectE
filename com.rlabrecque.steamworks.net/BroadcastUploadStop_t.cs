using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000EC RID: 236
	[CallbackIdentity(4605)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct BroadcastUploadStop_t
	{
		// Token: 0x040002EC RID: 748
		public const int k_iCallback = 4605;

		// Token: 0x040002ED RID: 749
		public EBroadcastUploadResult m_eResult;
	}
}
