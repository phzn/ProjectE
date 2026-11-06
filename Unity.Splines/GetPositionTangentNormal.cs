using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200002C RID: 44
	public struct GetPositionTangentNormal : IJobParallelFor
	{
		// Token: 0x06000252 RID: 594 RVA: 0x0000ABE8 File Offset: 0x00008DE8
		public void Execute(int index)
		{
			float3 @float;
			float3 float2;
			float3 float3;
			this.Spline.Evaluate((float)index / ((float)this.Positions.Length - 1f), out @float, out float2, out float3);
			this.Positions[index] = @float;
			this.Tangents[index] = float2;
			this.Normals[index] = float3;
		}

		// Token: 0x040000B4 RID: 180
		[ReadOnly]
		public NativeSpline Spline;

		// Token: 0x040000B5 RID: 181
		[WriteOnly]
		public NativeArray<float3> Positions;

		// Token: 0x040000B6 RID: 182
		[WriteOnly]
		public NativeArray<float3> Tangents;

		// Token: 0x040000B7 RID: 183
		[WriteOnly]
		public NativeArray<float3> Normals;
	}
}
