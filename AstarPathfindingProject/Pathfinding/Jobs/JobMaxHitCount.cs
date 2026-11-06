using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Jobs
{
	// Token: 0x0200017E RID: 382
	[BurstCompile]
	public struct JobMaxHitCount : IJob
	{
		// Token: 0x06000BC2 RID: 3010 RVA: 0x00041238 File Offset: 0x0003F438
		public void Execute()
		{
			int i;
			for (i = 0; i < this.maxHits; i++)
			{
				int num = i * this.layerStride;
				bool flag = false;
				for (int j = num; j < num + this.layerStride; j++)
				{
					if (math.any(this.hits[j].normal))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			this.maxHitCount[0] = math.max(1, i);
		}

		// Token: 0x040007B0 RID: 1968
		[ReadOnly]
		public NativeArray<RaycastHit> hits;

		// Token: 0x040007B1 RID: 1969
		public int maxHits;

		// Token: 0x040007B2 RID: 1970
		public int layerStride;

		// Token: 0x040007B3 RID: 1971
		[WriteOnly]
		public NativeArray<int> maxHitCount;
	}
}
