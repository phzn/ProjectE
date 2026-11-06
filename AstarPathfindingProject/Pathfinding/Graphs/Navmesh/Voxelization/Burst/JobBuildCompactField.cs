using System;
using Unity.Burst;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001EA RID: 490
	[BurstCompile(CompileSynchronously = true)]
	internal struct JobBuildCompactField : IJob
	{
		// Token: 0x06000D8B RID: 3467 RVA: 0x00050284 File Offset: 0x0004E484
		public void Execute()
		{
			this.output.BuildFromLinkedField(this.input);
		}

		// Token: 0x04000938 RID: 2360
		public LinkedVoxelField input;

		// Token: 0x04000939 RID: 2361
		public CompactVoxelField output;
	}
}
