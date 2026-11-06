using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200006D RID: 109
	[CallbackIdentity(2803)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInputConfigurationLoaded_t
	{
		// Token: 0x04000110 RID: 272
		public const int k_iCallback = 2803;

		// Token: 0x04000111 RID: 273
		public AppId_t m_unAppID;

		// Token: 0x04000112 RID: 274
		public InputHandle_t m_ulDeviceHandle;

		// Token: 0x04000113 RID: 275
		public CSteamID m_ulMappingCreator;

		// Token: 0x04000114 RID: 276
		public uint m_unMajorRevision;

		// Token: 0x04000115 RID: 277
		public uint m_unMinorRevision;

		// Token: 0x04000116 RID: 278
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUsesSteamInputAPI;

		// Token: 0x04000117 RID: 279
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUsesGamepadAPI;
	}
}
