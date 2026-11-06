using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unity.Collections
{
	// Token: 0x02000067 RID: 103
	[GenerateTestsForBurstCompatibility]
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 128)]
	internal struct FixedBytes128Align8
	{
		// Token: 0x040000EE RID: 238
		[SerializeField]
		[FieldOffset(0)]
		internal FixedBytes16Align8 offset0000;

		// Token: 0x040000EF RID: 239
		[SerializeField]
		[FieldOffset(16)]
		internal FixedBytes16Align8 offset0016;

		// Token: 0x040000F0 RID: 240
		[SerializeField]
		[FieldOffset(32)]
		internal FixedBytes16Align8 offset0032;

		// Token: 0x040000F1 RID: 241
		[SerializeField]
		[FieldOffset(48)]
		internal FixedBytes16Align8 offset0048;

		// Token: 0x040000F2 RID: 242
		[SerializeField]
		[FieldOffset(64)]
		internal FixedBytes16Align8 offset0064;

		// Token: 0x040000F3 RID: 243
		[SerializeField]
		[FieldOffset(80)]
		internal FixedBytes16Align8 offset0080;

		// Token: 0x040000F4 RID: 244
		[SerializeField]
		[FieldOffset(96)]
		internal FixedBytes16Align8 offset0096;

		// Token: 0x040000F5 RID: 245
		[SerializeField]
		[FieldOffset(112)]
		internal FixedBytes16Align8 offset0112;
	}
}
