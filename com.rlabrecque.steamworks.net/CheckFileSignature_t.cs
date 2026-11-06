using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E4 RID: 228
	[CallbackIdentity(705)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CheckFileSignature_t
	{
		// Token: 0x040002D9 RID: 729
		public const int k_iCallback = 705;

		// Token: 0x040002DA RID: 730
		public ECheckFileSignature m_eCheckFileSignature;
	}
}
