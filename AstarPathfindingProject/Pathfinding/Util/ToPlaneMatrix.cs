using System;
using Unity.Mathematics;

namespace Pathfinding.Util
{
	// Token: 0x02000297 RID: 663
	public readonly struct ToPlaneMatrix
	{
		// Token: 0x06001087 RID: 4231 RVA: 0x0006454A File Offset: 0x0006274A
		public ToPlaneMatrix(NativeMovementPlane plane)
		{
			this.matrix = new float3x3(math.conjugate(plane.rotation));
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00064564 File Offset: 0x00062764
		public float2 ToPlane(float3 p)
		{
			return math.mul(this.matrix, p).xz;
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00064585 File Offset: 0x00062785
		public float3 ToXZPlane(float3 p)
		{
			return math.mul(this.matrix, p);
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00064594 File Offset: 0x00062794
		public float2 ToPlane(float3 p, out float elevation)
		{
			float3 @float = math.mul(this.matrix, p);
			elevation = @float.y;
			return @float.xz;
		}

		// Token: 0x04000BF6 RID: 3062
		public readonly float3x3 matrix;
	}
}
