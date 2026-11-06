using System;
using Pathfinding.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001E8 RID: 488
	public struct RasterizationMesh
	{
		// Token: 0x04000924 RID: 2340
		public UnsafeSpan<float3> vertices;

		// Token: 0x04000925 RID: 2341
		public UnsafeSpan<int> triangles;

		// Token: 0x04000926 RID: 2342
		public UnsafeSpan<int> areas;

		// Token: 0x04000927 RID: 2343
		public int area;

		// Token: 0x04000928 RID: 2344
		public Bounds bounds;

		// Token: 0x04000929 RID: 2345
		public Matrix4x4 matrix;

		// Token: 0x0400092A RID: 2346
		public bool solid;

		// Token: 0x0400092B RID: 2347
		public bool doubleSided;

		// Token: 0x0400092C RID: 2348
		public bool flatten;
	}
}
