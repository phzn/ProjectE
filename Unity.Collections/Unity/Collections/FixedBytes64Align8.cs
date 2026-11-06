using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unity.Collections
{
	// Token: 0x02000066 RID: 102
	[GenerateTestsForBurstCompatibility]
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 64)]
	internal struct FixedBytes64Align8
	{
		// Token: 0x040000EA RID: 234
		[SerializeField]
		[FieldOffset(0)]
		internal FixedBytes16Align8 offset0000;

		// Token: 0x040000EB RID: 235
		[SerializeField]
		[FieldOffset(16)]
		internal FixedBytes16Align8 offset0016;

		// Token: 0x040000EC RID: 236
		[SerializeField]
		[FieldOffset(32)]
		internal FixedBytes16Align8 offset0032;

		// Token: 0x040000ED RID: 237
		[SerializeField]
		[FieldOffset(48)]
		internal FixedBytes16Align8 offset0048;
	}
}
