using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000BB RID: 187
	[CallbackIdentity(3410)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StartPlaytimeTrackingResult_t
	{
		// Token: 0x04000249 RID: 585
		public const int k_iCallback = 3410;

		// Token: 0x0400024A RID: 586
		public EResult m_eResult;
	}
}
