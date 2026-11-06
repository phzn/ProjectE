using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000163 RID: 355
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamItemDetails_t
	{
		// Token: 0x04000A9E RID: 2718
		public SteamItemInstanceID_t m_itemId;

		// Token: 0x04000A9F RID: 2719
		public SteamItemDef_t m_iDefinition;

		// Token: 0x04000AA0 RID: 2720
		public ushort m_unQuantity;

		// Token: 0x04000AA1 RID: 2721
		public ushort m_unFlags;
	}
}
