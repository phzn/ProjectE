using System;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x02000179 RID: 377
	[BurstCompile]
	public struct JobMemSet<[IsUnmanaged] T> : IJob where T : struct, ValueType
	{
		// Token: 0x06000BBD RID: 3005 RVA: 0x00041195 File Offset: 0x0003F395
		public void Execute()
		{
			this.data.AsUnsafeSpan<T>().Fill(this.value);
		}

		// Token: 0x040007A6 RID: 1958
		[WriteOnly]
		public NativeArray<T> data;

		// Token: 0x040007A7 RID: 1959
		public T value;
	}
}
