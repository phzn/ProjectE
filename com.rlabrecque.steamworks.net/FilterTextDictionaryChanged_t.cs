using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E8 RID: 232
	[CallbackIdentity(739)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FilterTextDictionaryChanged_t
	{
		// Token: 0x040002E1 RID: 737
		public const int k_iCallback = 739;

		// Token: 0x040002E2 RID: 738
		public int m_eLanguage;
	}
}
