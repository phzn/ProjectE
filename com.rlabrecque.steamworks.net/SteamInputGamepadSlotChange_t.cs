using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006E RID: 110
	[CallbackIdentity(2804)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInputGamepadSlotChange_t
	{
		// Token: 0x04000118 RID: 280
		public const int k_iCallback = 2804;

		// Token: 0x04000119 RID: 281
		public AppId_t m_unAppID;

		// Token: 0x0400011A RID: 282
		public InputHandle_t m_ulDeviceHandle;

		// Token: 0x0400011B RID: 283
		public ESteamInputType m_eDeviceType;

		// Token: 0x0400011C RID: 284
		public int m_nOldGamepadSlot;

		// Token: 0x0400011D RID: 285
		public int m_nNewGamepadSlot;
	}
}
