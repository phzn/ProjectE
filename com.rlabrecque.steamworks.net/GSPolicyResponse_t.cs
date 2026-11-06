using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000048 RID: 72
	[CallbackIdentity(115)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSPolicyResponse_t
	{
		// Token: 0x04000072 RID: 114
		public const int k_iCallback = 115;

		// Token: 0x04000073 RID: 115
		public byte m_bSecure;
	}
}
