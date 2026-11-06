using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000AB RID: 171
	[CallbackIdentity(1331)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileWriteAsyncComplete_t
	{
		// Token: 0x0400020B RID: 523
		public const int k_iCallback = 1331;

		// Token: 0x0400020C RID: 524
		public EResult m_eResult;
	}
}
