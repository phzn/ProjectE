using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000229 RID: 553
	[BurstCompile]
	public struct JobCopyBuffers : IJob
	{
		// Token: 0x06000E35 RID: 3637 RVA: 0x00057498 File Offset: 0x00055698
		public void Execute()
		{
			Slice3D slice3D = new Slice3D(this.input.bounds, this.bounds);
			Slice3D slice3D2 = new Slice3D(this.output.bounds, this.bounds);
			JobCopyRectangle<Vector3>.Copy(this.input.positions, this.output.positions, slice3D, slice3D2);
			JobCopyRectangle<float4>.Copy(this.input.normals, this.output.normals, slice3D, slice3D2);
			JobCopyRectangle<ulong>.Copy(this.input.connections, this.output.connections, slice3D, slice3D2);
			if (this.copyPenaltyAndTags)
			{
				JobCopyRectangle<uint>.Copy(this.input.penalties, this.output.penalties, slice3D, slice3D2);
				JobCopyRectangle<int>.Copy(this.input.tags, this.output.tags, slice3D, slice3D2);
			}
			JobCopyRectangle<bool>.Copy(this.input.walkable, this.output.walkable, slice3D, slice3D2);
			JobCopyRectangle<bool>.Copy(this.input.walkableWithErosion, this.output.walkableWithErosion, slice3D, slice3D2);
		}

		// Token: 0x04000A61 RID: 2657
		[ReadOnly]
		[DisableUninitializedReadCheck]
		public GridGraphNodeData input;

		// Token: 0x04000A62 RID: 2658
		[WriteOnly]
		public GridGraphNodeData output;

		// Token: 0x04000A63 RID: 2659
		public IntBounds bounds;

		// Token: 0x04000A64 RID: 2660
		public bool copyPenaltyAndTags;
	}
}
