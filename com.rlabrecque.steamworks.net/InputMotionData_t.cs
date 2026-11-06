using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000162 RID: 354
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct InputMotionData_t
	{
		// Token: 0x04000A94 RID: 2708
		public float rotQuatX;

		// Token: 0x04000A95 RID: 2709
		public float rotQuatY;

		// Token: 0x04000A96 RID: 2710
		public float rotQuatZ;

		// Token: 0x04000A97 RID: 2711
		public float rotQuatW;

		// Token: 0x04000A98 RID: 2712
		public float posAccelX;

		// Token: 0x04000A99 RID: 2713
		public float posAccelY;

		// Token: 0x04000A9A RID: 2714
		public float posAccelZ;

		// Token: 0x04000A9B RID: 2715
		public float rotVelX;

		// Token: 0x04000A9C RID: 2716
		public float rotVelY;

		// Token: 0x04000A9D RID: 2717
		public float rotVelZ;
	}
}
