using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001FF RID: 511
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobTransformTileCoordinates : IJob
	{
		// Token: 0x06000DB0 RID: 3504 RVA: 0x00053580 File Offset: 0x00051780
		public unsafe void Execute()
		{
			for (uint num = 0U; num < this.vertices.length; num += 1U)
			{
				Int3 @int = *this.vertices[num];
				*this.vertices[num] = (Int3)this.matrix.MultiplyPoint3x4(new Vector3((float)@int.x, (float)@int.y, (float)@int.z));
			}
		}

		// Token: 0x040009BB RID: 2491
		public UnsafeSpan<Int3> vertices;

		// Token: 0x040009BC RID: 2492
		public Matrix4x4 matrix;
	}
}
