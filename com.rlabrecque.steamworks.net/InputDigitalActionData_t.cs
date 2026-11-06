using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000161 RID: 353
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InputDigitalActionData_t
	{
		// Token: 0x04000A92 RID: 2706
		public byte bState;

		// Token: 0x04000A93 RID: 2707
		public byte bActive;
	}
}
