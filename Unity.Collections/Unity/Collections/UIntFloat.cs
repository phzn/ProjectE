using System;
using System.Runtime.InteropServices;

namespace Unity.Collections
{
	// Token: 0x020000E3 RID: 227
	[StructLayout(LayoutKind.Explicit)]
	internal struct UIntFloat
	{
		// Token: 0x04000444 RID: 1092
		[FieldOffset(0)]
		public float floatValue;

		// Token: 0x04000445 RID: 1093
		[FieldOffset(0)]
		public uint intValue;

		// Token: 0x04000446 RID: 1094
		[FieldOffset(0)]
		public double doubleValue;

		// Token: 0x04000447 RID: 1095
		[FieldOffset(0)]
		public ulong longValue;
	}
}
