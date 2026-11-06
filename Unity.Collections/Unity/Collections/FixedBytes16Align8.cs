using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unity.Collections
{
	// Token: 0x02000063 RID: 99
	[GenerateTestsForBurstCompatibility]
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	internal struct FixedBytes16Align8
	{
		// Token: 0x040000D6 RID: 214
		[SerializeField]
		[FieldOffset(0)]
		public ulong byte0000;

		// Token: 0x040000D7 RID: 215
		[SerializeField]
		[FieldOffset(8)]
		public ulong byte0008;
	}
}
