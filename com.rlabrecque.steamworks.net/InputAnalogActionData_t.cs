using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000160 RID: 352
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InputAnalogActionData_t
	{
		// Token: 0x04000A8E RID: 2702
		public EInputSourceMode eMode;

		// Token: 0x04000A8F RID: 2703
		public float x;

		// Token: 0x04000A90 RID: 2704
		public float y;

		// Token: 0x04000A91 RID: 2705
		public byte bActive;
	}
}
