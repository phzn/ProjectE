using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200002D RID: 45
	public static class SplineJobs
	{
		// Token: 0x06000253 RID: 595 RVA: 0x0000AC44 File Offset: 0x00008E44
		public static void EvaluatePosition<T>(T spline, NativeArray<float3> positions) where T : ISpline
		{
			using (NativeSpline nativeSpline = new NativeSpline(spline, Allocator.TempJob))
			{
				SplineJobs.EvaluatePosition(nativeSpline, positions);
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000AC88 File Offset: 0x00008E88
		public static void EvaluatePosition(NativeSpline spline, NativeArray<float3> positions)
		{
			new GetPosition
			{
				Spline = spline,
				Positions = positions
			}.Schedule(positions.Length, 1, default(JobHandle)).Complete();
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000ACCC File Offset: 0x00008ECC
		public static void EvaluatePositionTangentNormal<T>(T spline, NativeArray<float3> positions, NativeArray<float3> tangents, NativeArray<float3> normals) where T : ISpline
		{
			using (NativeSpline nativeSpline = new NativeSpline(spline, Allocator.TempJob))
			{
				SplineJobs.EvaluatePositionTangentNormal(nativeSpline, positions, tangents, normals);
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000AD10 File Offset: 0x00008F10
		public static void EvaluatePositionTangentNormal(NativeSpline spline, NativeArray<float3> positions, NativeArray<float3> tangents, NativeArray<float3> normals)
		{
			new GetPositionTangentNormal
			{
				Spline = spline,
				Positions = positions,
				Tangents = tangents,
				Normals = normals
			}.Schedule(positions.Length, 1, default(JobHandle)).Complete();
		}
	}
}
