using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001EC RID: 492
	[BurstCompile(CompileSynchronously = true)]
	internal struct JobErodeWalkableArea : IJob
	{
		// Token: 0x06000D8D RID: 3469 RVA: 0x000504AC File Offset: 0x0004E6AC
		public void Execute()
		{
			NativeArray<ushort> nativeArray = new NativeArray<ushort>(this.field.spans.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			VoxelUtilityBurst.CalculateDistanceField(this.field, nativeArray);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				if ((int)nativeArray[i] < this.radius * 2)
				{
					this.field.areaTypes[i] = 0;
				}
			}
		}

		// Token: 0x0400093D RID: 2365
		public CompactVoxelField field;

		// Token: 0x0400093E RID: 2366
		public int radius;
	}
}
