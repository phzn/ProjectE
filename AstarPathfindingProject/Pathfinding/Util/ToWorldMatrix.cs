using System;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000298 RID: 664
	public readonly struct ToWorldMatrix
	{
		// Token: 0x0600108B RID: 4235 RVA: 0x000645BD File Offset: 0x000627BD
		public ToWorldMatrix(NativeMovementPlane plane)
		{
			this.matrix = new float3x3(plane.rotation);
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x000645D0 File Offset: 0x000627D0
		public ToWorldMatrix(float3x3 matrix)
		{
			this.matrix = matrix;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x000645D9 File Offset: 0x000627D9
		public float3 ToWorld(float2 p, float elevation = 0f)
		{
			return math.mul(this.matrix, new float3(p.x, elevation, p.y));
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x000645F8 File Offset: 0x000627F8
		public Bounds ToWorld(Bounds bounds)
		{
			return new Bounds
			{
				center = math.mul(this.matrix, bounds.center),
				extents = math.mul(new float3x3(math.abs(this.matrix.c0), math.abs(this.matrix.c1), math.abs(this.matrix.c2)), bounds.extents)
			};
		}

		// Token: 0x04000BF7 RID: 3063
		public readonly float3x3 matrix;
	}
}
