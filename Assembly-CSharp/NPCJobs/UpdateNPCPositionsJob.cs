using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace NPCJobs
{
	// Token: 0x02000734 RID: 1844
	[BurstCompile(CompileSynchronously = true)]
	public struct UpdateNPCPositionsJob : IJobParallelFor
	{
		// Token: 0x06002FA1 RID: 12193 RVA: 0x0010DD90 File Offset: 0x0010BF90
		public void Execute(int index)
		{
			NPCJobData npcjobData = this.npcs[index];
			npcjobData.position = this.newPositions[index];
			this.npcs[index] = npcjobData;
		}

		// Token: 0x0400302A RID: 12330
		[ReadOnly]
		public NativeArray<float3> newPositions;

		// Token: 0x0400302B RID: 12331
		public NativeArray<NPCJobData> npcs;
	}
}
