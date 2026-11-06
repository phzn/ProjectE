using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000169 RID: 361
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamParamStringArray_t
	{
		// Token: 0x04000AB6 RID: 2742
		public IntPtr m_ppStrings;

		// Token: 0x04000AB7 RID: 2743
		public int m_nNumStrings;
	}
}
