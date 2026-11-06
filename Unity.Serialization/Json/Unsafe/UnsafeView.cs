using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json.Unsafe
{
	// Token: 0x02000096 RID: 150
	internal struct UnsafeView
	{
		// Token: 0x040001D8 RID: 472
		[NativeDisableUnsafePtrRestriction]
		public unsafe UnsafePackedBinaryStream* Stream;

		// Token: 0x040001D9 RID: 473
		public int TokenIndex;
	}
}
