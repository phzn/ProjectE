using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000D5 RID: 213
	[NativeContainer]
	[GenerateTestsForBurstCompatibility]
	internal struct NativeTextDispose
	{
		// Token: 0x060009F0 RID: 2544 RVA: 0x0001D229 File Offset: 0x0001B429
		public void Dispose()
		{
			UnsafeText.Free(this.m_TextData);
		}

		// Token: 0x04000414 RID: 1044
		[NativeDisableUnsafePtrRestriction]
		public unsafe UnsafeText* m_TextData;
	}
}
