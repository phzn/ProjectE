using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200022D RID: 557
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobNodeGridLayout : IJob, GridIterationUtilities.ICellAction
	{
		// Token: 0x06000E3B RID: 3643 RVA: 0x00058064 File Offset: 0x00056264
		public static Vector3 NodePosition(Matrix4x4 graphToWorld, int x, int z, float height = 0f)
		{
			return graphToWorld.MultiplyPoint3x4(new Vector3((float)x + 0.5f, height, (float)z + 0.5f));
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00058083 File Offset: 0x00056283
		public void Execute()
		{
			GridIterationUtilities.ForEachCellIn3DArray<JobNodeGridLayout>(this.bounds.size, ref this);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00058098 File Offset: 0x00056298
		public void Execute(uint innerIndex, int x, int y, int z)
		{
			this.nodePositions[(int)innerIndex] = JobNodeGridLayout.NodePosition(this.graphToWorld, x + this.bounds.min.x, z + this.bounds.min.z, 0f);
		}

		// Token: 0x04000A78 RID: 2680
		public Matrix4x4 graphToWorld;

		// Token: 0x04000A79 RID: 2681
		public IntBounds bounds;

		// Token: 0x04000A7A RID: 2682
		[WriteOnly]
		public NativeArray<Vector3> nodePositions;
	}
}
