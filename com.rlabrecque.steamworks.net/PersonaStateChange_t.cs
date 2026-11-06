using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200002D RID: 45
	[CallbackIdentity(304)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct PersonaStateChange_t
	{
		// Token: 0x04000015 RID: 21
		public const int k_iCallback = 304;

		// Token: 0x04000016 RID: 22
		public ulong m_ulSteamID;

		// Token: 0x04000017 RID: 23
		public EPersonaChange m_nChangeFlags;
	}
}
