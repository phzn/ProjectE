using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000236 RID: 566
	[BurstCompile]
	public struct JobRelocateNodes : IJob, GridIterationUtilities.ICellAction
	{
		// Token: 0x06000E47 RID: 3655 RVA: 0x000587A0 File Offset: 0x000569A0
		public void Execute()
		{
			GridIterationUtilities.ForEachCellIn3DArray<JobRelocateNodes>(this.bounds.size, ref this);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x000587B4 File Offset: 0x000569B4
		public void Execute(uint innerIndex, int x, int y, int z)
		{
			float y2 = this.previousWorldToGraph.MultiplyPoint3x4(this.positions[(int)innerIndex]).y;
			this.positions[(int)innerIndex] = JobNodeGridLayout.NodePosition(this.graphToWorld, x, z, y2);
		}

		// Token: 0x04000AB7 RID: 2743
		public Matrix4x4 previousWorldToGraph;

		// Token: 0x04000AB8 RID: 2744
		public Matrix4x4 graphToWorld;

		// Token: 0x04000AB9 RID: 2745
		public NativeArray<Vector3> positions;

		// Token: 0x04000ABA RID: 2746
		public IntBounds bounds;
	}
}
