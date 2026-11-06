using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200002F RID: 47
	public static class SplineMath
	{
		// Token: 0x06000260 RID: 608 RVA: 0x0000AE34 File Offset: 0x00009034
		public static float RayLineParameter(float3 ro, float3 rd, float3 lineOrigin, float3 lineDir)
		{
			float3 @float = ro - lineOrigin;
			float3 float2 = math.cross(rd, math.cross(rd, lineDir));
			return math.dot(@float, float2) / math.dot(lineDir, float2);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000AE64 File Offset: 0x00009064
		public static float3 RayLineDistance(float3 ro, float3 rd, float3 a, float3 b)
		{
			ValueTuple<float3, float3> valueTuple = SplineMath.RayLineNearestPoint(ro, rd, a, b);
			return valueTuple.Item2 - valueTuple.Item1;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000AE8C File Offset: 0x0000908C
		[return: TupleElementNames(new string[] { "rayPoint", "linePoint" })]
		public static ValueTuple<float3, float3> RayLineNearestPoint(float3 ro, float3 rd, float3 a, float3 b)
		{
			float num;
			float num2;
			return SplineMath.RayLineNearestPoint(ro, rd, a, b, out num, out num2);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000AEA8 File Offset: 0x000090A8
		[return: TupleElementNames(new string[] { "rayPoint", "linePoint" })]
		public static ValueTuple<float3, float3> RayLineNearestPoint(float3 ro, float3 rd, float3 a, float3 b, out float rayParam, out float lineParam)
		{
			float3 @float = b - a;
			lineParam = SplineMath.RayLineParameter(ro, rd, a, @float);
			float3 float2 = a + @float * math.saturate(lineParam);
			rayParam = math.dot(rd, float2 - ro);
			return new ValueTuple<float3, float3>(ro + rd * rayParam, float2);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000AF04 File Offset: 0x00009104
		public static float3 PointLineNearestPoint(float3 p, float3 a, float3 b, out float lineParam)
		{
			float3 @float = b - a;
			float num = math.length(@float);
			float3 float2 = math.select(0f, @float * (1f / num), num > 1.1754944E-38f);
			lineParam = math.dot(float2, p - a);
			return a + float2 * math.clamp(lineParam, 0f, num);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000AF70 File Offset: 0x00009170
		public static float DistancePointLine(float3 p, float3 a, float3 b)
		{
			float num;
			return math.length(SplineMath.PointLineNearestPoint(p, a, b, out num) - p);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000AF92 File Offset: 0x00009192
		internal static float GetUnitCircleTangentLength()
		{
			return 4f * (math.sqrt(2f) - 1f) / 3f;
		}
	}
}
