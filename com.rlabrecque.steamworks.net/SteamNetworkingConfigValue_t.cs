using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001A8 RID: 424
	[Serializable]
	public struct SteamNetworkingConfigValue_t
	{
		// Token: 0x04000BA3 RID: 2979
		public ESteamNetworkingConfigValue m_eValue;

		// Token: 0x04000BA4 RID: 2980
		public ESteamNetworkingConfigDataType m_eDataType;

		// Token: 0x04000BA5 RID: 2981
		public SteamNetworkingConfigValue_t.OptionValue m_val;

		// Token: 0x020001ED RID: 493
		[StructLayout(LayoutKind.Explicit)]
		public struct OptionValue
		{
			// Token: 0x04000C32 RID: 3122
			[FieldOffset(0)]
			public int m_int32;

			// Token: 0x04000C33 RID: 3123
			[FieldOffset(0)]
			public long m_int64;

			// Token: 0x04000C34 RID: 3124
			[FieldOffset(0)]
			public float m_float;

			// Token: 0x04000C35 RID: 3125
			[FieldOffset(0)]
			public IntPtr m_string;

			// Token: 0x04000C36 RID: 3126
			[FieldOffset(0)]
			public IntPtr m_functionPtr;
		}
	}
}
