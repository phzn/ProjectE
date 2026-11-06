using System;
using System.Runtime.InteropServices;

namespace UnityEngine.InputSystem.DualShock.LowLevel
{
	// Token: 0x020000A9 RID: 169
	[StructLayout(LayoutKind.Explicit, Size = 47)]
	internal struct DualSenseHIDOutputReportPayload
	{
		// Token: 0x040004BB RID: 1211
		[FieldOffset(0)]
		public byte enableFlags1;

		// Token: 0x040004BC RID: 1212
		[FieldOffset(1)]
		public byte enableFlags2;

		// Token: 0x040004BD RID: 1213
		[FieldOffset(2)]
		public byte highFrequencyMotorSpeed;

		// Token: 0x040004BE RID: 1214
		[FieldOffset(3)]
		public byte lowFrequencyMotorSpeed;

		// Token: 0x040004BF RID: 1215
		[FieldOffset(44)]
		public byte redColor;

		// Token: 0x040004C0 RID: 1216
		[FieldOffset(45)]
		public byte greenColor;

		// Token: 0x040004C1 RID: 1217
		[FieldOffset(46)]
		public byte blueColor;
	}
}
