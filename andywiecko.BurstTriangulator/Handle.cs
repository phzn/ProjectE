using System;
using Unity.Collections.LowLevel.Unsafe;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x02000010 RID: 16
	public readonly struct Handle
	{
		// Token: 0x06000049 RID: 73 RVA: 0x0000267E File Offset: 0x0000087E
		public Handle(ulong gcHandle)
		{
			this.gcHandle = gcHandle;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002687 File Offset: 0x00000887
		public void Free()
		{
			UnsafeUtility.ReleaseGCObject(this.gcHandle);
		}

		// Token: 0x0400003E RID: 62
		private readonly ulong gcHandle;
	}
}
