using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x0200017A RID: 378
	[BurstCompile]
	public struct JobCopy<T> : IJob where T : struct
	{
		// Token: 0x06000BBE RID: 3006 RVA: 0x000411AD File Offset: 0x0003F3AD
		public void Execute()
		{
			this.from.CopyTo(this.to);
		}

		// Token: 0x040007A8 RID: 1960
		[ReadOnly]
		public NativeArray<T> from;

		// Token: 0x040007A9 RID: 1961
		[WriteOnly]
		public NativeArray<T> to;
	}
}
