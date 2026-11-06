using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BC RID: 188
	[CallbackIdentity(3411)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StopPlaytimeTrackingResult_t
	{
		// Token: 0x0400024B RID: 587
		public const int k_iCallback = 3411;

		// Token: 0x0400024C RID: 588
		public EResult m_eResult;
	}
}
