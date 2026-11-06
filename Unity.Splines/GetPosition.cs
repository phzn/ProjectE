using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200002B RID: 43
	[BurstCompile]
	public struct GetPosition : IJobParallelFor
	{
		// Token: 0x06000251 RID: 593 RVA: 0x0000ABBA File Offset: 0x00008DBA
		public void Execute(int index)
		{
			this.Positions[index] = this.Spline.EvaluatePosition((float)index / ((float)this.Positions.Length - 1f));
		}

		// Token: 0x040000B2 RID: 178
		[ReadOnly]
		public NativeSpline Spline;

		// Token: 0x040000B3 RID: 179
		[WriteOnly]
		public NativeArray<float3> Positions;
	}
}
