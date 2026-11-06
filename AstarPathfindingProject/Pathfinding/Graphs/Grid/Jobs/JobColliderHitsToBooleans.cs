using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000228 RID: 552
	[BurstCompile]
	public struct JobColliderHitsToBooleans : IJob
	{
		// Token: 0x06000E34 RID: 3636 RVA: 0x00057450 File Offset: 0x00055650
		public void Execute()
		{
			for (int i = 0; i < this.hits.Length; i++)
			{
				this.result[i] = this.hits[i].instanceID == 0;
			}
		}

		// Token: 0x04000A5F RID: 2655
		[ReadOnly]
		public NativeArray<ColliderHit> hits;

		// Token: 0x04000A60 RID: 2656
		[WriteOnly]
		public NativeArray<bool> result;
	}
}
