using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B0 RID: 432
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemotePlayInput_t
	{
		// Token: 0x04000BE1 RID: 3041
		public RemotePlaySessionID_t m_unSessionID;

		// Token: 0x04000BE2 RID: 3042
		public ERemotePlayInputType m_eType;

		// Token: 0x04000BE3 RID: 3043
		public RemotePlayInput_t.OptionValue m_val;

		// Token: 0x020001EE RID: 494
		[StructLayout(LayoutKind.Explicit, Size = 56)]
		public struct OptionValue
		{
			// Token: 0x04000C37 RID: 3127
			[FieldOffset(0)]
			public RemotePlayInputMouseMotion_t m_MouseMotion;

			// Token: 0x04000C38 RID: 3128
			[FieldOffset(0)]
			public ERemotePlayMouseButton m_eMouseButton;

			// Token: 0x04000C39 RID: 3129
			[FieldOffset(0)]
			public RemotePlayInputMouseWheel_t m_MouseWheel;

			// Token: 0x04000C3A RID: 3130
			[FieldOffset(0)]
			public RemotePlayInputKey_t m_Key;
		}
	}
}
