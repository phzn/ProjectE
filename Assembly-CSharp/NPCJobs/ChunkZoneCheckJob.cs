using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace NPCJobs
{
	// Token: 0x02000732 RID: 1842
	[BurstCompile(CompileSynchronously = true)]
	public struct ChunkZoneCheckJob : IJobParallelFor
	{
		// Token: 0x06002F9F RID: 12191 RVA: 0x0010DB7C File Offset: 0x0010BD7C
		public void Execute(int index)
		{
			ChunkJobData chunkJobData = this.chunks[index];
			float num = math.distancesq(this.playerPosition, chunkJobData.position);
			bool flag = num <= this.warmupRadiusSqr;
			bool flag2 = num <= this.activeRadiusSqr;
			this.results[index] = new ChunkZoneResult
			{
				id = chunkJobData.id,
				inWarmupZone = flag,
				inActiveZone = flag2,
				needsSpawn = (flag && !chunkJobData.hasNpcs && !chunkJobData.isQueued),
				needsDespawn = (!flag && chunkJobData.hasNpcs)
			};
		}

		// Token: 0x04003022 RID: 12322
		[ReadOnly]
		public NativeArray<ChunkJobData> chunks;

		// Token: 0x04003023 RID: 12323
		[ReadOnly]
		public float3 playerPosition;

		// Token: 0x04003024 RID: 12324
		[ReadOnly]
		public float warmupRadiusSqr;

		// Token: 0x04003025 RID: 12325
		[ReadOnly]
		public float activeRadiusSqr;

		// Token: 0x04003026 RID: 12326
		[WriteOnly]
		public NativeArray<ChunkZoneResult> results;
	}
}
