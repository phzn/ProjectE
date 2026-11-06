using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace NPCJobs
{
	// Token: 0x02000731 RID: 1841
	[BurstCompile(CompileSynchronously = true)]
	public struct NPCZoneCheckJob : IJobParallelFor
	{
		// Token: 0x06002F9E RID: 12190 RVA: 0x0010DB10 File Offset: 0x0010BD10
		public void Execute(int index)
		{
			NPCJobData npcjobData = this.npcs[index];
			float num = math.distancesq(this.playerPosition, npcjobData.position);
			this.results[index] = new NPCZoneResult
			{
				id = npcjobData.id,
				shouldBeActive = (num <= this.activeRadiusSqr),
				distanceSqr = num
			};
		}

		// Token: 0x0400301E RID: 12318
		[ReadOnly]
		public NativeArray<NPCJobData> npcs;

		// Token: 0x0400301F RID: 12319
		[ReadOnly]
		public float3 playerPosition;

		// Token: 0x04003020 RID: 12320
		[ReadOnly]
		public float activeRadiusSqr;

		// Token: 0x04003021 RID: 12321
		[WriteOnly]
		public NativeArray<NPCZoneResult> results;
	}
}
