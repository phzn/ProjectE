using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200022C RID: 556
	[BurstCompile]
	public struct JobMergeRaycastCollisionHits : IJob
	{
		// Token: 0x06000E3A RID: 3642 RVA: 0x00057FF4 File Offset: 0x000561F4
		public void Execute()
		{
			for (int i = 0; i < this.hit1.Length; i++)
			{
				this.result[i] = this.hit1[i].normal == Vector3.zero && this.hit2[i].normal == Vector3.zero;
			}
		}

		// Token: 0x04000A75 RID: 2677
		[ReadOnly]
		public NativeArray<RaycastHit> hit1;

		// Token: 0x04000A76 RID: 2678
		[ReadOnly]
		public NativeArray<RaycastHit> hit2;

		// Token: 0x04000A77 RID: 2679
		[WriteOnly]
		public NativeArray<bool> result;
	}
}
