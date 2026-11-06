using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000E5 RID: 229
	[CallbackIdentity(714)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GamepadTextInputDismissed_t
	{
		// Token: 0x040002DB RID: 731
		public const int k_iCallback = 714;

		// Token: 0x040002DC RID: 732
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSubmitted;

		// Token: 0x040002DD RID: 733
		public uint m_unSubmittedText;

		// Token: 0x040002DE RID: 734
		public AppId_t m_unAppID;
	}
}
