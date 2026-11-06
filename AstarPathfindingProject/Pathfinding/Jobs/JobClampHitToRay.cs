using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Jobs
{
	// Token: 0x0200017F RID: 383
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobClampHitToRay : IJob
	{
		// Token: 0x06000BC3 RID: 3011 RVA: 0x000412B0 File Offset: 0x0003F4B0
		public void Execute()
		{
			for (int i = 0; i < this.hits.Length; i++)
			{
				RaycastHit raycastHit = this.hits[i];
				raycastHit.point = VectorMath.ClosestPointOnLine(this.commands[i].origin, this.commands[i].origin + this.commands[i].direction, raycastHit.point);
				this.hits[i] = raycastHit;
			}
		}

		// Token: 0x040007B4 RID: 1972
		[ReadOnly]
		public NativeArray<SpherecastCommand> commands;

		// Token: 0x040007B5 RID: 1973
		public NativeArray<RaycastHit> hits;
	}
}
