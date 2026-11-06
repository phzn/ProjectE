using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x02000044 RID: 68
	[BurstCompile]
	public struct MemsetNativeArray<T> : IJobParallelFor where T : struct
	{
		// Token: 0x060001F6 RID: 502 RVA: 0x0000A093 File Offset: 0x00008293
		public void Execute(int index)
		{
			this.Source[index] = this.Value;
		}

		// Token: 0x04000083 RID: 131
		public NativeArray<T> Source;

		// Token: 0x04000084 RID: 132
		public T Value;
	}
}
