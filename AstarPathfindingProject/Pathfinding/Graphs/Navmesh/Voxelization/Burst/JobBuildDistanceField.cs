using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001ED RID: 493
	[BurstCompile(CompileSynchronously = true)]
	internal struct JobBuildDistanceField : IJob
	{
		// Token: 0x06000D8E RID: 3470 RVA: 0x00050514 File Offset: 0x0004E714
		public void Execute()
		{
			NativeArray<ushort> nativeArray = new NativeArray<ushort>(this.field.spans.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			VoxelUtilityBurst.CalculateDistanceField(this.field, nativeArray);
			this.output.ResizeUninitialized(this.field.spans.Length);
			VoxelUtilityBurst.BoxBlur(this.field, nativeArray, this.output.AsArray());
		}

		// Token: 0x0400093F RID: 2367
		public CompactVoxelField field;

		// Token: 0x04000940 RID: 2368
		public NativeList<ushort> output;
	}
}
