using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200003D RID: 61
	public static class SplineUtility
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0000CDD8 File Offset: 0x0000AFD8
		public static bool Evaluate<T>(this T spline, float t, out float3 position, out float3 tangent, out float3 upVector) where T : ISpline
		{
			if (spline.Count < 1)
			{
				position = float3.zero;
				tangent = new float3(0f, 0f, 1f);
				upVector = new float3(0f, 1f, 0f);
				return false;
			}
			float num2;
			int num = spline.SplineToCurveT(t, out num2);
			BezierCurve curve = spline.GetCurve(num);
			position = CurveUtility.EvaluatePosition(curve, num2);
			tangent = CurveUtility.EvaluateTangent(curve, num2);
			upVector = spline.GetCurveUpVector(num, num2);
			return true;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000CE84 File Offset: 0x0000B084
		public static bool EvaluateNurbs(float t, List<float3> controlPoints, List<double> knotVector, int order, out float3 position)
		{
			position = float3.zero;
			if (knotVector.Count < controlPoints.Count + order - 1 || controlPoints.Count < order || t < 0f || 1f < t)
			{
				return false;
			}
			knotVector = new List<double>(knotVector);
			double num = knotVector[0];
			double num2 = knotVector[knotVector.Count - 1] - knotVector[0];
			if (knotVector[0] != 0.0 || knotVector[knotVector.Count - 1] != 1.0)
			{
				for (int i = 0; i < knotVector.Count; i++)
				{
					knotVector[i] = (knotVector[i] - num) / num2;
				}
			}
			int num3 = order;
			while (num3 < controlPoints.Count && knotVector[num3] <= (double)t)
			{
				num3++;
			}
			num3--;
			float[] nurbsBasisFunctions = SplineUtility.GetNurbsBasisFunctions(order, t, knotVector, num3);
			for (int j = 0; j < order; j++)
			{
				position += nurbsBasisFunctions[j] * controlPoints[num3 - order + 1 + j];
			}
			return true;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000CFAC File Offset: 0x0000B1AC
		private static float[] GetNurbsBasisFunctions(int degree, float t, List<double> knotVector, int span)
		{
			float[] array = new float[degree];
			float[] array2 = new float[degree];
			float[] array3 = new float[degree];
			for (int i = 0; i < degree; i++)
			{
				array[i] = 0f;
				array2[i] = 0f;
				array3[i] = 1f;
			}
			for (int j = 1; j < degree; j++)
			{
				array[j] = (float)((double)t - knotVector[span + 1 - j]);
				array2[j] = (float)(knotVector[span + j] - (double)t);
				float num = 0f;
				for (int k = 0; k < j; k++)
				{
					float num2 = array3[k] / (array2[k + 1] + array[j - k]);
					array3[k] = num + array2[k + 1] * num2;
					num = array[j - k] * num2;
				}
				array3[j] = num;
			}
			return array3;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000D078 File Offset: 0x0000B278
		public static float3 EvaluatePosition<T>(this T spline, float t) where T : ISpline
		{
			if (spline.Count < 1)
			{
				return float.PositiveInfinity;
			}
			float num;
			return CurveUtility.EvaluatePosition(spline.GetCurve(spline.SplineToCurveT(t, out num)), num);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000D0BC File Offset: 0x0000B2BC
		public static float3 EvaluateTangent<T>(this T spline, float t) where T : ISpline
		{
			if (spline.Count < 1)
			{
				return float.PositiveInfinity;
			}
			float num;
			return CurveUtility.EvaluateTangent(spline.GetCurve(spline.SplineToCurveT(t, out num)), num);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000D100 File Offset: 0x0000B300
		public static float3 EvaluateUpVector<T>(this T spline, float t) where T : ISpline
		{
			if (spline.Count < 1)
			{
				return float3.zero;
			}
			float num2;
			int num = spline.SplineToCurveT(t, out num2);
			return spline.GetCurveUpVector(num, num2);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000D13C File Offset: 0x0000B33C
		public static float3 CalculateUpVector<T>(this T spline, float t) where T : ISpline
		{
			if (spline.Count < 1)
			{
				return float3.zero;
			}
			float num2;
			int num = spline.SplineToCurveT(t, out num2);
			return spline.CalculateUpVector(num, num2);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000D174 File Offset: 0x0000B374
		internal static float3 CalculateUpVector<T>(this T spline, int curveIndex, float curveT) where T : ISpline
		{
			if (spline.Count < 1)
			{
				return float3.zero;
			}
			BezierCurve curve = spline.GetCurve(curveIndex);
			float3 @float = math.rotate(spline[curveIndex].Rotation, math.up());
			if (curveT == 0f)
			{
				return @float;
			}
			int num = spline.NextIndex(curveIndex);
			float3 float2 = math.rotate(spline[num].Rotation, math.up());
			if (curveT == 1f)
			{
				return float2;
			}
			return CurveUtility.EvaluateUpVector(curve, curveT, @float, float2, true);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000D208 File Offset: 0x0000B408
		internal static void EvaluateUpVectorsForCurve<T>(this T spline, int curveIndex, NativeArray<float3> upVectors) where T : ISpline
		{
			if (spline.Count < 1 || !upVectors.IsCreated)
			{
				return;
			}
			float3 @float = math.rotate(spline[curveIndex].Rotation, math.up());
			int num = spline.NextIndex(curveIndex);
			float3 float2 = math.rotate(spline[num].Rotation, math.up());
			CurveUtility.EvaluateUpVectors(spline.GetCurve(curveIndex), @float, float2, upVectors);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000D28C File Offset: 0x0000B48C
		internal static void EvaluateUpVectorsForCurve<T>(this T spline, int curveIndex, float3[] upVectors) where T : ISpline
		{
			if (upVectors == null)
			{
				return;
			}
			NativeArray<float3> nativeArray = new NativeArray<float3>(upVectors, Allocator.Temp);
			spline.EvaluateUpVectorsForCurve(curveIndex, nativeArray);
			nativeArray.CopyTo(upVectors);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000D2B8 File Offset: 0x0000B4B8
		public static float3 EvaluateAcceleration<T>(this T spline, float t) where T : ISpline
		{
			if (spline.Count < 1)
			{
				return float3.zero;
			}
			float num;
			return CurveUtility.EvaluateAcceleration(spline.GetCurve(spline.SplineToCurveT(t, out num)), num);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000D2F8 File Offset: 0x0000B4F8
		public static float EvaluateCurvature<T>(this T spline, float t) where T : ISpline
		{
			if (spline.Count < 1)
			{
				return 0f;
			}
			float num2;
			int num = spline.SplineToCurveT(t, out num2);
			return CurveUtility.EvaluateCurvature(spline.GetCurve(num), num2);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000D33C File Offset: 0x0000B53C
		public static float3 EvaluateCurvatureCenter<T>(this T spline, float t) where T : ISpline
		{
			if (spline.Count < 1)
			{
				return 0f;
			}
			float num2;
			int num = spline.SplineToCurveT(t, out num2);
			BezierCurve curve = spline.GetCurve(num);
			float num3 = CurveUtility.EvaluateCurvature(curve, num2);
			if (num3 != 0f)
			{
				float num4 = 1f / num3;
				float3 @float = CurveUtility.EvaluatePosition(curve, num2);
				float3 float2 = CurveUtility.EvaluateTangent(curve, num2);
				float3 float3 = math.normalize(math.cross(CurveUtility.EvaluateAcceleration(curve, num2), float2));
				float3 float4 = math.normalize(math.cross(float2, float3));
				return @float + num4 * float4;
			}
			return float3.zero;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000D3E3 File Offset: 0x0000B5E3
		public static int SplineToCurveT<T>(this T spline, float splineT, out float curveT) where T : ISpline
		{
			return spline.SplineToCurveT(splineT, out curveT, true);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000D3F0 File Offset: 0x0000B5F0
		private static int SplineToCurveT<T>(this T spline, float splineT, out float curveT, bool useLUT) where T : ISpline
		{
			int count = spline.Count;
			if (count <= 1)
			{
				curveT = 0f;
				return 0;
			}
			splineT = math.clamp(splineT, 0f, 1f);
			float num = splineT * spline.GetLength();
			float num2 = 0f;
			bool closed = spline.Closed;
			int i = 0;
			int num3 = (closed ? count : (count - 1));
			while (i < num3)
			{
				int num4 = i % count;
				float curveLength = spline.GetCurveLength(num4);
				if (num <= num2 + curveLength)
				{
					curveT = (useLUT ? spline.GetCurveInterpolation(num4, num - num2) : ((num - num2) / curveLength));
					return num4;
				}
				num2 += curveLength;
				i++;
			}
			curveT = 1f;
			if (!closed)
			{
				return count - 2;
			}
			return count - 1;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000D4C0 File Offset: 0x0000B6C0
		public static float CurveToSplineT<T>(this T spline, float curve) where T : ISpline
		{
			if (spline.Count <= 1 || curve < 0f)
			{
				return 0f;
			}
			if (curve >= (float)(spline.Closed ? spline.Count : (spline.Count - 1)))
			{
				return 1f;
			}
			int num = (int)math.floor(curve);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				num2 += spline.GetCurveLength(i);
			}
			num2 += spline.GetCurveLength(num) * math.frac(curve);
			return num2 / spline.GetLength();
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000D574 File Offset: 0x0000B774
		public static float CalculateLength<T>(this T spline, float4x4 transform) where T : ISpline
		{
			float length;
			using (NativeSpline nativeSpline = new NativeSpline(spline, transform, Allocator.Temp))
			{
				length = nativeSpline.GetLength();
			}
			return length;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000D5BC File Offset: 0x0000B7BC
		public static int GetCurveCount<T>(this T spline) where T : ISpline
		{
			return math.max(0, spline.Count - (spline.Closed ? 0 : 1));
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000D5E5 File Offset: 0x0000B7E5
		public static Bounds GetBounds<T>(this T spline) where T : ISpline
		{
			return spline.GetBounds(float4x4.identity);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000D5F4 File Offset: 0x0000B7F4
		public static Bounds GetBounds<T>(this T spline, float4x4 transform) where T : ISpline
		{
			if (spline.Count < 1)
			{
				return default(Bounds);
			}
			BezierKnot bezierKnot = spline[0];
			Bounds bounds = new Bounds(math.transform(transform, bezierKnot.Position), Vector3.zero);
			if (spline.Closed)
			{
				bounds.Encapsulate(math.transform(transform, bezierKnot.Position + math.rotate(bezierKnot.Rotation, bezierKnot.TangentIn)));
			}
			bounds.Encapsulate(math.transform(transform, bezierKnot.Position + math.rotate(bezierKnot.Rotation, bezierKnot.TangentOut)));
			int i = 1;
			int count = spline.Count;
			while (i < count)
			{
				bezierKnot = spline[i];
				bounds.Encapsulate(math.transform(transform, bezierKnot.Position));
				bounds.Encapsulate(math.transform(transform, bezierKnot.Position + math.rotate(bezierKnot.Rotation, bezierKnot.TangentIn)));
				if (spline.Closed || (!spline.Closed && i < count - 1))
				{
					bounds.Encapsulate(math.transform(transform, bezierKnot.Position + math.rotate(bezierKnot.Rotation, bezierKnot.TangentOut)));
				}
				i++;
			}
			return bounds;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000D77D File Offset: 0x0000B97D
		[Obsolete("Use GetSubdivisionCount instead.", false)]
		public static int GetSegmentCount(float length, int resolution)
		{
			return SplineUtility.GetSubdivisionCount(length, resolution);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000D786 File Offset: 0x0000B986
		public static int GetSubdivisionCount(float length, int resolution)
		{
			return (int)math.max(6f, math.min(1024f, math.sqrt(length) * (float)resolution));
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000D7A8 File Offset: 0x0000B9A8
		private static SplineUtility.Segment GetNearestPoint<T>(T spline, float3 ro, float3 rd, SplineUtility.Segment range, out float distance, out float3 nearest, out float time, int segments) where T : ISpline
		{
			distance = float.PositiveInfinity;
			nearest = float.PositiveInfinity;
			time = float.PositiveInfinity;
			SplineUtility.Segment segment = new SplineUtility.Segment(-1f, 0f);
			float num = range.start;
			float3 @float = spline.EvaluatePosition(num);
			for (int i = 1; i < segments; i++)
			{
				float num2 = range.start + range.length * ((float)i / ((float)segments - 1f));
				float3 float2 = spline.EvaluatePosition(num2);
				float num3;
				float num4;
				ValueTuple<float3, float3> valueTuple = SplineMath.RayLineNearestPoint(ro, rd, @float, float2, out num3, out num4);
				float3 item = valueTuple.Item1;
				float3 item2 = valueTuple.Item2;
				float num5 = math.lengthsq(item2 - item);
				if (num5 < distance)
				{
					segment.start = num;
					segment.length = num2 - num;
					time = segment.start + segment.length * num4;
					distance = num5;
					nearest = item2;
				}
				num = num2;
				@float = float2;
			}
			distance = math.sqrt(distance);
			return segment;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000D8AC File Offset: 0x0000BAAC
		private static SplineUtility.Segment GetNearestPoint<T>(T spline, float3 point, SplineUtility.Segment range, out float distance, out float3 nearest, out float time, int segments) where T : ISpline
		{
			distance = float.PositiveInfinity;
			nearest = float.PositiveInfinity;
			time = float.PositiveInfinity;
			SplineUtility.Segment segment = new SplineUtility.Segment(-1f, 0f);
			float num = range.start;
			float3 @float = spline.EvaluatePosition(num);
			for (int i = 1; i < segments; i++)
			{
				float num2 = range.start + range.length * ((float)i / ((float)segments - 1f));
				float3 float2 = spline.EvaluatePosition(num2);
				float num3;
				float3 float3 = SplineMath.PointLineNearestPoint(point, @float, float2, out num3);
				float num4 = math.distancesq(float3, point);
				if (num4 < distance)
				{
					segment.start = num;
					segment.length = num2 - num;
					time = segment.start + segment.length * num3;
					distance = num4;
					nearest = float3;
				}
				num = num2;
				@float = float2;
			}
			distance = math.sqrt(distance);
			return segment;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000D994 File Offset: 0x0000BB94
		public static float GetNearestPoint<T>(T spline, Ray ray, out float3 nearest, out float t, int resolution = 4, int iterations = 2) where T : ISpline
		{
			float positiveInfinity = float.PositiveInfinity;
			nearest = float.PositiveInfinity;
			float3 @float = ray.origin;
			float3 float2 = ray.direction;
			SplineUtility.Segment nearestPoint = new SplineUtility.Segment(0f, 1f);
			t = 0f;
			int num = math.min(math.max(2, resolution), 64);
			int i = 0;
			int num2 = math.min(10, iterations);
			while (i < num2)
			{
				int subdivisionCount = SplineUtility.GetSubdivisionCount(spline.GetLength() * nearestPoint.length, num);
				nearestPoint = SplineUtility.GetNearestPoint<T>(spline, @float, float2, nearestPoint, out positiveInfinity, out nearest, out t, subdivisionCount);
				i++;
			}
			return positiveInfinity;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000DA44 File Offset: 0x0000BC44
		public static float GetNearestPoint<T>(T spline, float3 point, out float3 nearest, out float t, int resolution = 4, int iterations = 2) where T : ISpline
		{
			float positiveInfinity = float.PositiveInfinity;
			nearest = float.PositiveInfinity;
			SplineUtility.Segment nearestPoint = new SplineUtility.Segment(0f, 1f);
			t = 0f;
			int num = math.min(math.max(2, resolution), 64);
			int i = 0;
			int num2 = math.min(10, iterations);
			while (i < num2)
			{
				int subdivisionCount = SplineUtility.GetSubdivisionCount(spline.GetLength() * nearestPoint.length, num);
				nearestPoint = SplineUtility.GetNearestPoint<T>(spline, point, nearestPoint, out positiveInfinity, out nearest, out t, subdivisionCount);
				i++;
			}
			return positiveInfinity;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000DAD4 File Offset: 0x0000BCD4
		public static float3 GetPointAtLinearDistance<T>(this T spline, float fromT, float relativeDistance, out float resultPointT) where T : ISpline
		{
			if (fromT < 0f)
			{
				resultPointT = 0f;
				return spline.EvaluatePosition(0f);
			}
			float length = spline.GetLength();
			float num = fromT * length;
			if (num + relativeDistance >= length)
			{
				resultPointT = 1f;
				return spline.EvaluatePosition(1f);
			}
			if (num + relativeDistance <= 0f)
			{
				resultPointT = 0f;
				return spline.EvaluatePosition(0f);
			}
			float3 @float = spline.EvaluatePosition(fromT);
			resultPointT = fromT;
			bool flag = relativeDistance >= 0f;
			float num2 = math.abs(relativeDistance);
			float3 float2 = @float;
			while (num2 > 0.001f && (flag ? (resultPointT < 1f) : (resultPointT > 0f)))
			{
				num += (flag ? num2 : (-num2));
				resultPointT = num / length;
				if (resultPointT > 1f)
				{
					resultPointT = 1f;
				}
				else if (resultPointT < 0f)
				{
					resultPointT = 0f;
				}
				float2 = spline.EvaluatePosition(resultPointT);
				float num3 = math.distance(@float, float2);
				num2 = math.abs(relativeDistance) - num3;
			}
			return float2;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000DBE6 File Offset: 0x0000BDE6
		public static float ConvertIndexUnit<T>(this T spline, float t, PathIndexUnit targetPathUnit) where T : ISpline
		{
			if (targetPathUnit == PathIndexUnit.Normalized)
			{
				return SplineUtility.WrapInterpolation(t, spline.Closed);
			}
			return SplineUtility.ConvertNormalizedIndexUnit<T>(spline, t, targetPathUnit);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000DC08 File Offset: 0x0000BE08
		public static float ConvertIndexUnit<T>(this T spline, float value, PathIndexUnit fromPathUnit, PathIndexUnit targetPathUnit) where T : ISpline
		{
			if (fromPathUnit == targetPathUnit)
			{
				if (targetPathUnit == PathIndexUnit.Normalized)
				{
					value = SplineUtility.WrapInterpolation(value, spline.Closed);
				}
				return value;
			}
			return SplineUtility.ConvertNormalizedIndexUnit<T>(spline, SplineUtility.GetNormalizedInterpolation<T>(spline, value, fromPathUnit), targetPathUnit);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000DC38 File Offset: 0x0000BE38
		private static float ConvertNormalizedIndexUnit<T>(T spline, float t, PathIndexUnit targetPathUnit) where T : ISpline
		{
			if (targetPathUnit == PathIndexUnit.Distance)
			{
				return t * spline.GetLength();
			}
			if (targetPathUnit == PathIndexUnit.Knot)
			{
				float num;
				return (float)spline.SplineToCurveT(t, out num, false) + num;
			}
			return t;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000DC6B File Offset: 0x0000BE6B
		private static float WrapInterpolation(float t, bool closed)
		{
			if (!closed)
			{
				return math.clamp(t, 0f, 1f);
			}
			if (t % 1f != 0f)
			{
				return t - math.floor(t);
			}
			return math.clamp(t, 0f, 1f);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000DCA8 File Offset: 0x0000BEA8
		public static float GetNormalizedInterpolation<T>(T spline, float t, PathIndexUnit originalPathUnit) where T : ISpline
		{
			if (originalPathUnit == PathIndexUnit.Distance)
			{
				float length = spline.GetLength();
				return SplineUtility.WrapInterpolation((length > 0f) ? (t / length) : 0f, spline.Closed);
			}
			if (originalPathUnit == PathIndexUnit.Knot)
			{
				return SplineUtility.WrapInterpolation(spline.CurveToSplineT(t), spline.Closed);
			}
			return SplineUtility.WrapInterpolation(t, spline.Closed);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000DD1C File Offset: 0x0000BF1C
		public static int PreviousIndex<T>(this T spline, int index) where T : ISpline
		{
			return SplineUtility.PreviousIndex(index, spline.Count, spline.Closed);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000DD3E File Offset: 0x0000BF3E
		public static int NextIndex<T>(this T spline, int index) where T : ISpline
		{
			return SplineUtility.NextIndex(index, spline.Count, spline.Closed);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000DD60 File Offset: 0x0000BF60
		public static BezierKnot Previous<T>(this T spline, int index) where T : ISpline
		{
			return spline[spline.PreviousIndex(index)];
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000DD76 File Offset: 0x0000BF76
		public static BezierKnot Next<T>(this T spline, int index) where T : ISpline
		{
			return spline[spline.NextIndex(index)];
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000DD8C File Offset: 0x0000BF8C
		internal static int PreviousIndex(int index, int count, bool wrap)
		{
			if (!wrap)
			{
				return math.max(index - 1, 0);
			}
			return (index + (count - 1)) % count;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000DDA2 File Offset: 0x0000BFA2
		internal static int NextIndex(int index, int count, bool wrap)
		{
			if (!wrap)
			{
				return math.min(index + 1, count - 1);
			}
			return (index + 1) % count;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000DDB8 File Offset: 0x0000BFB8
		internal static float3 GetExplicitLinearTangent(float3 point, float3 to)
		{
			return (to - point) / 3f;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000DDCC File Offset: 0x0000BFCC
		internal static float3 GetExplicitLinearTangent(BezierKnot from, BezierKnot to)
		{
			float3 @float = to.Position - from.Position;
			return math.mul(math.inverse(from.Rotation), @float * 0.33f);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000DE06 File Offset: 0x0000C006
		public static float3 GetCatmullRomTangent(float3 previous, float3 next)
		{
			return SplineUtility.GetAutoSmoothTangent(previous, next, 0.5f);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000DE14 File Offset: 0x0000C014
		public static float3 GetAutoSmoothTangent(float3 previous, float3 next, float tension = 0.33333334f)
		{
			if (next.Equals(previous))
			{
				return 0f;
			}
			return (next - previous) / math.sqrt(math.length(next - previous)) * tension;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000DE50 File Offset: 0x0000C050
		public static float3 GetAutoSmoothTangent(float3 previous, float3 current, float3 next, float tension = 0.33333334f)
		{
			float num = math.length(current - previous);
			float num2 = math.length(next - current);
			if (num == 0f)
			{
				return (next - current) * 0.1f;
			}
			if (num2 == 0f)
			{
				return (current - previous) * 0.1f;
			}
			float num3 = 2f * tension;
			float num4 = math.pow(num, tension);
			float num5 = math.pow(num, num3);
			float num6 = math.pow(num2, tension);
			float num7 = math.pow(num2, num3);
			return (num5 * next - num7 * previous + (num7 - num5) * current) / (3f * num4 * (num4 + num6));
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000DF11 File Offset: 0x0000C111
		private static float3 GetUniformAutoSmoothTangent(float3 previous, float3 next, float tension)
		{
			return (next - previous) * tension;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000DF20 File Offset: 0x0000C120
		public static BezierKnot GetAutoSmoothKnot(float3 position, float3 previous, float3 next)
		{
			return SplineUtility.GetAutoSmoothKnot(position, previous, next, math.up());
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000DF2F File Offset: 0x0000C12F
		public static BezierKnot GetAutoSmoothKnot(float3 position, float3 previous, float3 next, float3 normal)
		{
			return SplineUtility.GetAutoSmoothKnot(position, previous, next, normal, 0.5f);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000DF40 File Offset: 0x0000C140
		public static BezierKnot GetAutoSmoothKnot(float3 position, float3 previous, float3 next, float3 normal, float tension = 0.33333334f)
		{
			float3 autoSmoothTangent = SplineUtility.GetAutoSmoothTangent(next, position, previous, tension);
			float3 autoSmoothTangent2 = SplineUtility.GetAutoSmoothTangent(previous, position, next, tension);
			float3 @float = new float3(0f, 0f, math.length(autoSmoothTangent));
			float3 float2 = new float3(0f, 0f, math.length(autoSmoothTangent2));
			float3 float3 = autoSmoothTangent2;
			if (@float.z == 0f)
			{
				@float.z = float2.z;
			}
			if (float2.z == 0f)
			{
				float2.z = @float.z;
				float3 = -autoSmoothTangent;
			}
			return new BezierKnot(position, -@float, float2, SplineUtility.GetKnotRotation(float3, normal));
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000DFE8 File Offset: 0x0000C1E8
		internal static quaternion GetKnotRotation(float3 tangent, float3 normal)
		{
			if (math.lengthsq(tangent) == 0f)
			{
				tangent = math.rotate(Quaternion.FromToRotation(math.up(), normal), math.forward());
			}
			float3 @float = (Mathf.Approximately(math.abs(math.dot(math.normalizesafe(tangent, default(float3)), math.normalizesafe(normal, default(float3)))), 1f) ? math.cross(math.normalizesafe(tangent, default(float3)), math.right()) : Vector3.ProjectOnPlane(normal, tangent).normalized);
			return quaternion.LookRotationSafe(math.normalizesafe(tangent, default(float3)), @float);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000E0AC File Offset: 0x0000C2AC
		public static void SetPivot(SplineContainer container, Vector3 position)
		{
			Transform transform = container.transform;
			Vector3 vector = position - transform.position;
			transform.position = position;
			Spline spline = container.Spline;
			int i = 0;
			int count = spline.Count;
			while (i < count)
			{
				spline[i] -= vector;
				i++;
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000E10C File Offset: 0x0000C30C
		public static bool FitSplineToPoints(List<float3> points, float errorThreshold, bool closed, out Spline spline)
		{
			if (points == null || points.Count < 2)
			{
				Debug.LogError("FitSplineToPoints failed: The provided points list is either null, empty or contains only one point.");
				spline = new Spline();
				return false;
			}
			List<float3> list = new List<float3>(points);
			if (closed)
			{
				float3 @float = list[0];
				List<float3> list2 = list;
				if (math.length(@float - list2[list2.Count - 1]) > 0.001f)
				{
					list.Add(list[0]);
				}
				list.Add(list[1]);
				List<float3> list3 = list;
				int num = 0;
				List<float3> list4 = list;
				list3.Insert(num, list4[list4.Count - 3]);
			}
			float3 float2 = math.normalize(list[1] - list[0]);
			List<float3> list5 = list;
			float3 float3 = list5[list5.Count - 2];
			List<float3> list6 = list;
			float3 float4 = math.normalize(float3 - list6[list6.Count - 1]);
			if (SplineUtility.FitSplineToPointsStepInternal(list, 0, list.Count - 1, float2, float4, errorThreshold, closed, closed, out spline))
			{
				if (closed && spline.Count > 2)
				{
					spline.RemoveAt(0);
					spline.RemoveAt(spline.Count - 1);
					BezierKnot bezierKnot = spline[0];
					Spline spline2 = spline;
					bezierKnot.TangentIn = spline2[spline2.Count - 1].TangentOut;
					spline[0] = bezierKnot;
					spline.RemoveAt(spline.Count - 1);
					spline.Closed = true;
				}
				float3 float5 = float3.zero;
				for (int i = 0; i < spline.Count; i++)
				{
					BezierKnot bezierKnot2 = spline[i];
					float3 float6 = bezierKnot2.TangentOut;
					if (math.lengthsq(float6) == 0f)
					{
						float6 = spline.EvaluateTangent(SplineUtility.GetNormalizedInterpolation<Spline>(spline, (float)i, PathIndexUnit.Knot));
					}
					float3 float7 = math.normalizesafe(float6, default(float3));
					if (i == 0)
					{
						float5 = SplineUtility.CalculatePreferredNormalForDirection(float7);
					}
					else
					{
						float5 = CurveUtility.EvaluateUpVector(spline.GetCurve(i - 1), 1f, float5, float3.zero, false);
					}
					TangentMode tangentMode = TangentMode.Broken;
					if (math.dot(math.normalizesafe(bezierKnot2.TangentIn, default(float3)), math.normalizesafe(bezierKnot2.TangentOut, default(float3))) <= -0.999f)
					{
						if (math.abs(math.length(bezierKnot2.TangentIn) - math.length(bezierKnot2.TangentOut)) <= 0.001f)
						{
							tangentMode = TangentMode.Mirrored;
						}
						else
						{
							tangentMode = TangentMode.Continuous;
						}
					}
					bezierKnot2.TangentIn = new float3(0f, 0f, -math.length(bezierKnot2.TangentIn));
					bezierKnot2.TangentOut = new float3(0f, 0f, math.length(bezierKnot2.TangentOut));
					bezierKnot2.Rotation = quaternion.LookRotationSafe(float7, float5);
					spline[i] = bezierKnot2;
					if (tangentMode != TangentMode.Broken)
					{
						spline.SetTangentModeNoNotify(i, tangentMode, BezierTangent.Out);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000E3D0 File Offset: 0x0000C5D0
		private static bool FitSplineToPointsStepInternal(IReadOnlyList<float3> allPoints, int rangeStart, int rangeEnd, float3 leftTangent, float3 rightTangent, float errorThreshold, bool closed, bool splineClosed, out Spline spline)
		{
			spline = new Spline();
			int num = rangeEnd - rangeStart + 1;
			if (num < 2)
			{
				return false;
			}
			if (num == 2)
			{
				float num2 = math.length(allPoints[rangeStart] - allPoints[rangeEnd]) / 3f;
				float3 @float = leftTangent * num2;
				float3 float2 = rightTangent * num2;
				BezierKnot bezierKnot = new BezierKnot(allPoints[rangeStart], -@float, @float, Quaternion.identity);
				BezierKnot bezierKnot2 = new BezierKnot(allPoints[rangeStart + 1], float2, -float2, Quaternion.identity);
				spline = new Spline();
				spline.Add(bezierKnot, TangentMode.Broken);
				spline.Add(bezierKnot2, TangentMode.Broken);
				spline.Closed = closed;
				return true;
			}
			float[] array = new float[num];
			float[] array2 = new float[num];
			array2[0] = 0f;
			float[] array3 = new float[num - 1];
			float num3 = 0f;
			for (int i = 1; i < num; i++)
			{
				float3 float3 = allPoints[rangeStart + i] - allPoints[rangeStart + i - 1];
				num3 += math.length(float3);
				array3[i - 1] = num3;
			}
			for (int j = 0; j < array3.Length; j++)
			{
				array2[j + 1] = array3[j] / num3;
			}
			array = array2;
			spline = SplineUtility.GenerateSplineFromTValues(allPoints, rangeStart, rangeEnd, closed, array, leftTangent, rightTangent);
			float3[] array4 = new float3[num];
			for (int k = 0; k < array.Length; k++)
			{
				float num4 = array[k];
				float3 float4 = spline.EvaluatePosition(num4);
				array4[k] = float4;
			}
			ValueTuple<float, int> valueTuple = SplineUtility.ComputeMaxError(allPoints, rangeStart, rangeEnd, array4, errorThreshold, splineClosed);
			float item = valueTuple.Item1;
			int num5 = valueTuple.Item2;
			if (item < errorThreshold)
			{
				return true;
			}
			if (item < 4f * errorThreshold)
			{
				for (int l = 0; l < 4; l++)
				{
					BezierKnot[] array5 = new BezierKnot[spline.Count];
					int num6 = 0;
					foreach (BezierKnot bezierKnot3 in spline.Knots)
					{
						array5[num6] = bezierKnot3;
						num6++;
					}
					float3 position = array5[0].Position;
					float3 float5 = array5[0].Position + array5[0].TangentOut;
					float3 float6 = array5[1].Position + array5[1].TangentIn;
					float3 position2 = array5[1].Position;
					float3[] array6 = new float3[] { position, float5, float6, position2 };
					float3[] array7 = new float3[3];
					for (int m = 0; m <= 2; m++)
					{
						array7[m] = (array6[m + 1] - array6[m]) * 3f;
					}
					float3[] array8 = new float3[2];
					for (int n = 0; n <= 1; n++)
					{
						array8[n] = (array7[n + 1] - array7[n]) * 2f;
					}
					for (int num7 = rangeStart; num7 < num - 1; num7++)
					{
						float num8 = array[num7];
						float3 float7 = SplineUtility.Bernstein(num8, array6, 3);
						float3 float8 = SplineUtility.Bernstein(num8, array7, 2);
						float3 float9 = SplineUtility.Bernstein(num8, array8, 1);
						float num9 = math.dot(float7 - array4[num7], float8);
						float num10 = math.dot(float8, float8) + math.dot(float7 - array4[num7], float9);
						if (num10 != 0f)
						{
							array[num7] -= num9 / num10;
						}
					}
					spline = SplineUtility.GenerateSplineFromTValues(allPoints, rangeStart, rangeEnd, closed, array, leftTangent, rightTangent);
					for (int num11 = 0; num11 < array4.Length; num11++)
					{
						float num12 = array[num11];
						float3 float10 = spline.EvaluatePosition(num12);
						array4[num11] = float10;
					}
					ValueTuple<float, int> valueTuple2 = SplineUtility.ComputeMaxError(allPoints, rangeStart, rangeEnd, array4, errorThreshold, splineClosed);
					float item2 = valueTuple2.Item1;
					num5 = valueTuple2.Item2;
					if (item2 < errorThreshold)
					{
						return true;
					}
				}
			}
			if (num == 3)
			{
				num5 = rangeStart + 1;
			}
			else if (num5 == 0)
			{
				num5++;
			}
			else if (num5 == num - 1)
			{
				num5--;
			}
			float3 float11 = SplineUtility.CalculateCenterTangent(allPoints[num5 - 1], allPoints[num5], allPoints[num5 + 1]);
			Spline spline2;
			Spline spline3;
			if (!SplineUtility.FitSplineToPointsStepInternal(allPoints, rangeStart, num5, leftTangent, float11, errorThreshold, false, splineClosed, out spline2) || !SplineUtility.FitSplineToPointsStepInternal(allPoints, num5, rangeEnd, -float11, rightTangent, errorThreshold, false, splineClosed, out spline3))
			{
				spline = new Spline();
				return false;
			}
			int num13 = spline2.Count - 1;
			Spline spline4 = spline2;
			float3 tangentIn = spline4[spline4.Count - 1].TangentIn;
			spline2.RemoveAt(num13);
			spline2.Add(spline3);
			spline = spline2;
			BezierKnot bezierKnot4 = spline[num13];
			bezierKnot4.TangentIn = tangentIn;
			spline[num13] = bezierKnot4;
			return true;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000E900 File Offset: 0x0000CB00
		private static float3 CalculatePreferredNormalForDirection(float3 splineDirection)
		{
			float3 @float = math.normalizesafe(splineDirection, default(float3));
			float3 float2;
			float2.x = math.abs(math.dot(@float, math.right()));
			float2.y = math.abs(math.dot(@float, math.up()));
			float2.z = math.abs(math.dot(@float, math.forward()));
			float3 float3;
			if (float2.x < float2.y && float2.x < float2.z)
			{
				float3 = math.normalizesafe(math.cross(math.right(), @float), default(float3));
			}
			else if (float2.y < float2.x && float2.y < float2.z)
			{
				float3 = math.normalizesafe(math.cross(math.up(), @float), default(float3));
			}
			else
			{
				float3 = math.normalizesafe(math.cross(math.forward(), @float), default(float3));
			}
			return math.normalizesafe(math.cross(@float, float3), default(float3));
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000EA04 File Offset: 0x0000CC04
		private static float3 CalculateCenterTangent(float3 prevPoint, float3 centerPoint, float3 nextPoint)
		{
			float3 @float = prevPoint - centerPoint;
			float3 float2 = centerPoint - nextPoint;
			return math.normalize((@float + float2) / 2f);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000EA38 File Offset: 0x0000CC38
		private static float3 Bernstein(float t, float3[] bezier, int degree)
		{
			float3[] array = new float3[bezier.Length];
			bezier.CopyTo(array, 0);
			for (int i = 1; i <= degree; i++)
			{
				for (int j = 0; j <= degree - i; j++)
				{
					array[j] = (1f - t) * array[j] + t * array[j + 1];
				}
			}
			return array[0];
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000EAA8 File Offset: 0x0000CCA8
		private static Spline GenerateSplineFromTValues(IReadOnlyList<float3> allPoints, int rangeStart, int rangeEnd, bool closed, float[] tValues, float3 leftTangent, float3 rightTangent)
		{
			int num = rangeEnd - rangeStart + 1;
			float3[] array = new float3[num];
			float3[] array2 = new float3[num];
			for (int i = 0; i < num; i++)
			{
				float num2 = tValues[i];
				array[i] = leftTangent * 3f * num2 * (1f - num2) * (1f - num2);
				array2[i] = rightTangent * 3f * num2 * num2 * (1f - num2);
			}
			float[,] array3 = new float[2, 2];
			float num3 = 0f;
			float num4 = 0f;
			for (int j = 0; j < num; j++)
			{
				float num5 = tValues[j];
				array3[0, 0] += math.dot(array[j], array[j]);
				array3[0, 1] += math.dot(array[j], array2[j]);
				array3[1, 0] = array3[0, 1];
				array3[1, 1] += math.dot(array2[j], array2[j]);
				float3 @float = allPoints[rangeStart + j] - (allPoints[rangeStart] * (1f - num5) * (1f - num5) * (1f - num5) + allPoints[rangeStart] * 3f * num5 * (1f - num5) * (1f - num5) + allPoints[rangeEnd] * 3f * num5 * num5 * (1f - num5) + allPoints[rangeEnd] * num5 * num5 * num5);
				num3 += math.dot(array[j], @float);
				num4 += math.dot(array2[j], @float);
			}
			float num6 = array3[0, 0] * array3[1, 1] - array3[1, 0] * array3[0, 1];
			float num7 = array3[0, 0] * num4 - array3[1, 0] * num3;
			float num8 = num3 * array3[1, 1] - num4 * array3[0, 1];
			float num9 = ((num6 == 0f) ? 0f : (num8 / num6));
			float num10 = ((num6 == 0f) ? 0f : (num7 / num6));
			float num11 = math.length(allPoints[rangeEnd] - allPoints[rangeStart]);
			float num12 = 1E-06f * num11;
			if (num9 < num12 || num10 < num12)
			{
				num10 = (num9 = num11 / 3f);
			}
			BezierKnot bezierKnot = new BezierKnot(allPoints[rangeStart], leftTangent * -num9, leftTangent * num9, quaternion.identity);
			BezierKnot bezierKnot2 = new BezierKnot(allPoints[rangeEnd], rightTangent * num10, rightTangent * -num10, quaternion.identity);
			Spline spline = new Spline();
			spline.Add(bezierKnot, TangentMode.Broken);
			spline.Add(bezierKnot2, TangentMode.Broken);
			spline.Closed = closed;
			return spline;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000EE08 File Offset: 0x0000D008
		[return: TupleElementNames(new string[] { "maxError", "maxErrorIndex" })]
		private static ValueTuple<float, int> ComputeMaxError(IReadOnlyList<float3> allPoints, int rangeStart, int rangeEnd, float3[] positions, float errorThreshold, bool splineClosed)
		{
			int num = rangeEnd - rangeStart + 1;
			if (splineClosed && num > 2)
			{
				int num2 = 1;
				if (rangeStart < num2 && rangeEnd > num2)
				{
					return new ValueTuple<float, int>(errorThreshold, num2);
				}
				int num3 = allPoints.Count - 2;
				if (rangeStart < num3 && rangeEnd > num3)
				{
					return new ValueTuple<float, int>(errorThreshold, num3);
				}
			}
			float num4 = 0f;
			int num5 = 0;
			for (int i = 0; i < num; i++)
			{
				int num6 = rangeStart + i;
				float num7 = math.length(allPoints[num6] - positions[i]);
				if (num7 > num4)
				{
					num4 = num7;
					num5 = num6;
				}
			}
			return new ValueTuple<float, int>(num4, num5);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
		public static Spline AddSpline<T>(this T container) where T : ISplineContainer
		{
			Spline spline = new Spline();
			container.AddSpline(spline);
			return spline;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000EEBC File Offset: 0x0000D0BC
		public static void AddSpline<T>(this T container, Spline spline) where T : ISplineContainer
		{
			container.Splines = new List<Spline>(container.Splines) { spline };
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000EEF4 File Offset: 0x0000D0F4
		public static bool RemoveSplineAt<T>(this T container, int splineIndex) where T : ISplineContainer
		{
			if (splineIndex < 0 || splineIndex >= container.Splines.Count)
			{
				return false;
			}
			List<Spline> list = new List<Spline>(container.Splines);
			list.RemoveAt(splineIndex);
			container.KnotLinkCollection.SplineRemoved(splineIndex);
			container.Splines = list;
			return true;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000EF58 File Offset: 0x0000D158
		public static bool RemoveSpline<T>(this T container, Spline spline) where T : ISplineContainer
		{
			List<Spline> list = new List<Spline>(container.Splines);
			int num = list.IndexOf(spline);
			if (num < 0)
			{
				return false;
			}
			list.RemoveAt(num);
			container.KnotLinkCollection.SplineRemoved(num);
			container.Splines = list;
			return true;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000EFB0 File Offset: 0x0000D1B0
		public static bool ReorderSpline<T>(this T container, int previousSplineIndex, int newSplineIndex) where T : ISplineContainer
		{
			if (previousSplineIndex < 0 || previousSplineIndex >= container.Splines.Count || newSplineIndex < 0 || newSplineIndex >= container.Splines.Count)
			{
				return false;
			}
			List<Spline> list = new List<Spline>(container.Splines);
			Spline spline = list[previousSplineIndex];
			list.RemoveAt(previousSplineIndex);
			list.Insert(newSplineIndex, spline);
			container.KnotLinkCollection.SplineIndexChanged(previousSplineIndex, newSplineIndex);
			container.Splines = list;
			return true;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000F040 File Offset: 0x0000D240
		internal static bool IsIndexValid<T>(T container, SplineKnotIndex index) where T : ISplineContainer
		{
			return index.Knot >= 0 && index.Knot < container.Splines[index.Spline].Count && index.Spline < container.Splines.Count && index.Knot < container.Splines[index.Spline].Count;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000F0BC File Offset: 0x0000D2BC
		public static void SetLinkedKnotPosition<T>(this T container, SplineKnotIndex index) where T : ISplineContainer
		{
			IReadOnlyList<SplineKnotIndex> readOnlyList;
			if (!container.KnotLinkCollection.TryGetKnotLinks(index, out readOnlyList))
			{
				return;
			}
			IReadOnlyList<Spline> splines = container.Splines;
			float3 position = splines[index.Spline][index.Knot].Position;
			foreach (SplineKnotIndex splineKnotIndex in readOnlyList)
			{
				if (!SplineUtility.IsIndexValid<T>(container, splineKnotIndex))
				{
					break;
				}
				BezierKnot bezierKnot = splines[splineKnotIndex.Spline][splineKnotIndex.Knot];
				float3 position2 = bezierKnot.Position;
				bezierKnot.Position = position;
				splines[splineKnotIndex.Spline].SetKnotNoNotify(splineKnotIndex.Knot, bezierKnot, BezierTangent.Out);
				if (!Mathf.Approximately(Vector3.Distance(position, position2), 0f))
				{
					splines[splineKnotIndex.Spline].SetDirty(SplineModification.KnotModified, splineKnotIndex.Knot);
				}
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000F1D8 File Offset: 0x0000D3D8
		public static void LinkKnots<T>(this T container, SplineKnotIndex knotA, SplineKnotIndex knotB) where T : ISplineContainer
		{
			IReadOnlyList<SplineKnotIndex> readOnlyList = (Mathf.Approximately(math.length(container.Splines[knotA.Spline][knotA.Knot].Position - container.Splines[knotB.Spline][knotB.Knot].Position), 0f) ? null : container.KnotLinkCollection.GetKnotLinks(knotB));
			container.KnotLinkCollection.Link(knotA, knotB);
			if (readOnlyList != null)
			{
				foreach (SplineKnotIndex splineKnotIndex in readOnlyList)
				{
					container.Splines[splineKnotIndex.Spline].SetDirty(SplineModification.KnotModified, splineKnotIndex.Knot);
				}
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000F2D4 File Offset: 0x0000D4D4
		public static void UnlinkKnots<T>(this T container, IReadOnlyList<SplineKnotIndex> knots) where T : ISplineContainer
		{
			foreach (SplineKnotIndex splineKnotIndex in knots)
			{
				container.KnotLinkCollection.Unlink(splineKnotIndex);
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000F328 File Offset: 0x0000D528
		public static bool AreKnotLinked(this ISplineContainer container, SplineKnotIndex knotA, SplineKnotIndex knotB)
		{
			IReadOnlyList<SplineKnotIndex> readOnlyList;
			if (!container.KnotLinkCollection.TryGetKnotLinks(knotA, out readOnlyList))
			{
				return false;
			}
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				if (readOnlyList[i] == knotB)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000F36C File Offset: 0x0000D56C
		public static void CopyKnotLinks<T>(this T container, int srcSplineIndex, int destSplineIndex) where T : ISplineContainer
		{
			if (srcSplineIndex < 0 || srcSplineIndex >= container.Splines.Count || destSplineIndex < 0 || destSplineIndex >= container.Splines.Count)
			{
				return;
			}
			Spline spline = container.Splines[srcSplineIndex];
			Spline spline2 = container.Splines[destSplineIndex];
			if (spline.Count == 0 || spline.Count != spline2.Count)
			{
				return;
			}
			int i = 0;
			int count = spline.Count;
			while (i < count)
			{
				IReadOnlyList<SplineKnotIndex> readOnlyList;
				if (container.KnotLinkCollection.TryGetKnotLinks(new SplineKnotIndex(srcSplineIndex, i), out readOnlyList))
				{
					container.KnotLinkCollection.Link(new SplineKnotIndex(srcSplineIndex, i), new SplineKnotIndex(destSplineIndex, i));
				}
				i++;
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000F43C File Offset: 0x0000D63C
		public static List<float3> ReducePoints<T>(T line, float epsilon = 0.15f) where T : IList<float3>
		{
			List<float3> list = new List<float3>();
			new RamerDouglasPeucker<T>(line).Reduce(list, epsilon);
			return list;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000F45D File Offset: 0x0000D65D
		public static void ReducePoints<T>(T line, List<float3> results, float epsilon = 0.15f) where T : IList<float3>
		{
			new RamerDouglasPeucker<T>(line).Reduce(results, epsilon);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000F46C File Offset: 0x0000D66C
		internal static bool AreTangentsModifiable(TangentMode mode)
		{
			return mode == TangentMode.Broken || mode == TangentMode.Continuous || mode == TangentMode.Mirrored;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000F47C File Offset: 0x0000D67C
		public static void ReverseFlow(this ISplineContainer container, int splineIndex)
		{
			SplineUtility.ReverseFlow(new SplineInfo(container, splineIndex));
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000F48C File Offset: 0x0000D68C
		public static void ReverseFlow(SplineInfo splineInfo)
		{
			Spline spline = splineInfo.Spline;
			BezierKnot[] array = splineInfo.Spline.ToArray();
			TangentMode[] array2 = new TangentMode[spline.Count];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = spline.GetTangentMode(i);
			}
			List<List<SplineKnotIndex>> list = new List<List<SplineKnotIndex>>();
			for (int j = 0; j < spline.Count; j++)
			{
				SplineKnotIndex splineKnotIndex = new SplineKnotIndex(splineInfo.Index, j);
				List<SplineKnotIndex> list2 = splineInfo.Container.KnotLinkCollection.GetKnotLinks(splineKnotIndex).ToList<SplineKnotIndex>();
				list.Add(list2);
			}
			foreach (List<SplineKnotIndex> list3 in list)
			{
				splineInfo.Container.UnlinkKnots(list3);
			}
			for (int k = 0; k < spline.Count; k++)
			{
				BezierKnot bezierKnot = array[k];
				BezierKnot bezierKnot2 = bezierKnot.Transform(splineInfo.LocalToWorld);
				float3 tangentIn = bezierKnot2.TangentIn;
				float3 tangentOut = bezierKnot2.TangentOut;
				quaternion quaternion = quaternion.AxisAngle(math.mul(bezierKnot.Rotation, math.up()), math.radians(180f));
				quaternion = math.normalizesafe(quaternion);
				bezierKnot.Rotation = math.mul(quaternion, bezierKnot.Rotation);
				if (array2[k] == TangentMode.Broken)
				{
					quaternion quaternion2 = quaternion.AxisAngle(math.up(), math.radians(180f));
					bezierKnot.TangentIn = math.rotate(quaternion2, tangentOut);
					bezierKnot.TangentOut = math.rotate(quaternion2, tangentIn);
				}
				else if (array2[k] == TangentMode.Continuous)
				{
					bezierKnot.TangentIn = -tangentOut;
					bezierKnot.TangentOut = -tangentIn;
				}
				int num = spline.Count - 1 - k;
				spline.SetTangentMode(num, array2[k], BezierTangent.Out);
				spline[num] = bezierKnot;
			}
			foreach (List<SplineKnotIndex> list4 in list)
			{
				if (list4.Count != 1)
				{
					SplineKnotIndex splineKnotIndex2 = list4[0];
					splineKnotIndex2 = new SplineKnotIndex(splineKnotIndex2.Spline, splineKnotIndex2.Spline.Equals(splineInfo.Index) ? (spline.Count - 1 - splineKnotIndex2.Knot) : splineKnotIndex2.Knot);
					for (int l = 1; l < list4.Count; l++)
					{
						SplineKnotIndex splineKnotIndex3 = list4[l];
						if (splineKnotIndex3.Spline.Equals(splineInfo.Index))
						{
							list4[l] = new SplineKnotIndex(splineInfo.Index, spline.Count - 1 - splineKnotIndex3.Knot);
						}
						splineInfo.Container.LinkKnots(splineKnotIndex2, list4[l]);
					}
				}
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000F77C File Offset: 0x0000D97C
		public static void ReverseFlow(Spline spline)
		{
			BezierKnot[] array = spline.ToArray();
			TangentMode[] array2 = new TangentMode[spline.Count];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = spline.GetTangentMode(i);
			}
			for (int j = 0; j < spline.Count; j++)
			{
				BezierKnot bezierKnot = array[j];
				float3 tangentIn = bezierKnot.TangentIn;
				float3 tangentOut = bezierKnot.TangentOut;
				quaternion quaternion = quaternion.AxisAngle(math.mul(bezierKnot.Rotation, math.up()), math.radians(180f));
				quaternion = math.normalizesafe(quaternion);
				bezierKnot.Rotation = math.mul(quaternion, bezierKnot.Rotation);
				if (array2[j] == TangentMode.Broken)
				{
					quaternion quaternion2 = quaternion.AxisAngle(math.up(), math.radians(180f));
					bezierKnot.TangentIn = math.rotate(quaternion2, tangentOut);
					bezierKnot.TangentOut = math.rotate(quaternion2, tangentIn);
				}
				else if (array2[j] == TangentMode.Continuous)
				{
					bezierKnot.TangentIn = -tangentOut;
					bezierKnot.TangentOut = -tangentIn;
				}
				int num = spline.Count - 1 - j;
				spline.SetTangentMode(num, array2[j], BezierTangent.Out);
				spline[num] = bezierKnot;
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
		public static SplineKnotIndex JoinSplinesOnKnots(this ISplineContainer container, SplineKnotIndex mainKnot, SplineKnotIndex otherKnot)
		{
			if (mainKnot.Spline == otherKnot.Spline)
			{
				Debug.LogError("Trying to join Knots already belonging to the same spline.", container as SplineContainer);
				return default(SplineKnotIndex);
			}
			if (mainKnot.Spline < 0 || mainKnot.Spline > container.Splines.Count)
			{
				Debug.LogError(string.Format("Spline index {0} does not exist for the current container.", mainKnot.Spline), container as SplineContainer);
				return default(SplineKnotIndex);
			}
			if (otherKnot.Spline < 0 || otherKnot.Spline > container.Splines.Count)
			{
				Debug.LogError(string.Format("Spline index {0} does not exist for the current container.", otherKnot.Spline), container as SplineContainer);
				return default(SplineKnotIndex);
			}
			if (mainKnot.Knot < 0 || mainKnot.Knot > container.Splines[mainKnot.Spline].Count)
			{
				Debug.LogError(string.Format("Knot index {0} does not exist for the current container for Spline[{1}].", mainKnot.Knot, mainKnot.Spline), container as SplineContainer);
				return default(SplineKnotIndex);
			}
			if (otherKnot.Knot < 0 || otherKnot.Knot > container.Splines[otherKnot.Spline].Count)
			{
				Debug.LogError(string.Format("Knot index {0} does not exist for the current container for Spline[{1}].", otherKnot.Knot, otherKnot.Spline), container as SplineContainer);
				return default(SplineKnotIndex);
			}
			if (mainKnot.Knot != 0 && mainKnot.Knot != container.Splines[mainKnot.Spline].Count - 1)
			{
				Debug.LogError(string.Format("Knot index {0} is not an extremity knot for the current container for Spline[{1}].", mainKnot.Knot, mainKnot.Spline) + "Only extremity knots can be joined.", container as SplineContainer);
				return default(SplineKnotIndex);
			}
			if (otherKnot.Knot != 0 && otherKnot.Knot != container.Splines[otherKnot.Spline].Count - 1)
			{
				Debug.LogError(string.Format("Knot index {0} is not an extremity knot for the current container for Spline[{1}].", otherKnot.Knot, otherKnot.Spline) + "Only extremity knots can be joined.", container as SplineContainer);
				return default(SplineKnotIndex);
			}
			bool flag = mainKnot.Knot == 0;
			bool flag2 = otherKnot.Knot == 0;
			if (flag == flag2)
			{
				container.ReverseFlow(otherKnot.Spline);
			}
			List<List<SplineKnotIndex>> list = new List<List<SplineKnotIndex>>();
			int spline = mainKnot.Spline;
			Spline spline2 = container.Splines[spline];
			int count = spline2.Count;
			int spline3 = otherKnot.Spline;
			Spline spline4 = container.Splines[spline3];
			int count2 = spline4.Count;
			for (int i = 0; i < count; i++)
			{
				SplineKnotIndex splineKnotIndex = new SplineKnotIndex(mainKnot.Spline, i);
				list.Add(container.KnotLinkCollection.GetKnotLinks(splineKnotIndex).ToList<SplineKnotIndex>());
			}
			for (int j = 0; j < count2; j++)
			{
				SplineKnotIndex splineKnotIndex2 = new SplineKnotIndex(otherKnot.Spline, j);
				list.Add(container.KnotLinkCollection.GetKnotLinks(splineKnotIndex2).ToList<SplineKnotIndex>());
			}
			foreach (List<SplineKnotIndex> list2 in list)
			{
				container.UnlinkKnots(list2);
			}
			if (count2 > 1)
			{
				if (flag)
				{
					for (int k = count2 - 2; k >= 0; k--)
					{
						spline2.Insert(0, spline4[k], spline4.GetTangentMode(k));
					}
				}
				else
				{
					for (int l = 1; l < count2; l++)
					{
						spline2.Add(spline4[l], spline4.GetTangentMode(l));
					}
				}
			}
			container.RemoveSplineAt(spline3);
			int num = ((spline3 > spline) ? spline : (mainKnot.Spline - 1));
			foreach (List<SplineKnotIndex> list3 in list)
			{
				if (list3.Count != 1)
				{
					for (int m = 0; m < list3.Count; m++)
					{
						SplineKnotIndex splineKnotIndex3 = list3[m];
						if (splineKnotIndex3.Spline == spline || splineKnotIndex3.Spline == spline3)
						{
							int num2 = splineKnotIndex3.Knot;
							if (splineKnotIndex3.Spline == spline && flag)
							{
								num2 += count2 - 1;
							}
							if (splineKnotIndex3.Spline == spline3 && !flag)
							{
								num2 += count - 1;
							}
							list3[m] = new SplineKnotIndex(spline, num2);
						}
						else if (splineKnotIndex3.Spline > spline3)
						{
							list3[m] = new SplineKnotIndex(splineKnotIndex3.Spline - 1, splineKnotIndex3.Knot);
						}
					}
					SplineKnotIndex splineKnotIndex4 = list3[0];
					for (int n = 1; n < list3.Count; n++)
					{
						container.LinkKnots(splineKnotIndex4, list3[n]);
					}
				}
			}
			return new SplineKnotIndex(num, flag ? (count2 - 1) : mainKnot.Knot);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000FDE8 File Offset: 0x0000DFE8
		internal static SplineKnotIndex DuplicateKnot(this ISplineContainer container, SplineKnotIndex originalKnot, int targetIndex)
		{
			Spline spline = container.Splines[originalKnot.Spline];
			BezierKnot bezierKnot = spline[originalKnot.Knot];
			spline.Insert(targetIndex, bezierKnot);
			spline.SetTangentMode(targetIndex, spline.GetTangentMode(originalKnot.Knot), BezierTangent.Out);
			return new SplineKnotIndex(originalKnot.Spline, targetIndex);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000FE3C File Offset: 0x0000E03C
		public static void DuplicateSpline(this ISplineContainer container, SplineKnotIndex fromKnot, SplineKnotIndex toKnot, out int newSplineIndex)
		{
			newSplineIndex = -1;
			if (!fromKnot.IsValid() || !toKnot.IsValid())
			{
				throw new ArgumentException("Duplicate failed: The 2 provided knots must be valid knots.");
			}
			if (fromKnot.Spline != toKnot.Spline)
			{
				throw new ArgumentException("Duplicate failed: The 2 provided knots must be on the same Spline.");
			}
			Spline spline = container.AddSpline<ISplineContainer>();
			int num = Math.Min(fromKnot.Knot, toKnot.Knot);
			int num2 = Math.Max(fromKnot.Knot, toKnot.Knot);
			int spline2 = fromKnot.Spline;
			Spline spline3 = container.Splines[spline2];
			newSplineIndex = container.Splines.Count - 1;
			for (int i = num; i <= num2; i++)
			{
				spline.Add(spline3[i], spline3.GetTangentMode(i));
				IReadOnlyList<SplineKnotIndex> readOnlyList;
				if (container.KnotLinkCollection.TryGetKnotLinks(new SplineKnotIndex(spline2, i), out readOnlyList))
				{
					container.KnotLinkCollection.Link(new SplineKnotIndex(spline2, i), new SplineKnotIndex(newSplineIndex, i - num));
				}
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000FF30 File Offset: 0x0000E130
		public static SplineKnotIndex SplitSplineOnKnot(this ISplineContainer container, SplineKnotIndex knotInfo)
		{
			if (knotInfo.Spline < 0 || knotInfo.Spline > container.Splines.Count)
			{
				throw new IndexOutOfRangeException(string.Format("Spline index {0} does not exist for the current container.", knotInfo.Spline));
			}
			if (knotInfo.Knot < 0 || knotInfo.Knot > container.Splines[knotInfo.Spline].Count)
			{
				throw new IndexOutOfRangeException(string.Format("Knot index {0} does not exist for the current container for Spline[{1}].", knotInfo.Knot, knotInfo.Spline));
			}
			Spline spline = container.Splines[knotInfo.Spline];
			if (spline.Closed)
			{
				spline.Closed = false;
				SplineKnotIndex splineKnotIndex = new SplineKnotIndex(knotInfo.Spline, 0);
				SplineKnotIndex splineKnotIndex2 = container.DuplicateKnot(splineKnotIndex, spline.Count);
				if (knotInfo.Knot == 0)
				{
					return splineKnotIndex;
				}
				container.LinkKnots(splineKnotIndex, splineKnotIndex2);
			}
			else if (knotInfo.Knot == 0 || knotInfo.Knot == spline.Count - 1)
			{
				return knotInfo;
			}
			int num;
			container.DuplicateSpline(knotInfo, new SplineKnotIndex(knotInfo.Spline, spline.Count - 1), out num);
			spline.Resize(knotInfo.Knot + 1);
			return new SplineKnotIndex(num, 0);
		}

		// Token: 0x040000F0 RID: 240
		private const int k_SubdivisionCountMin = 6;

		// Token: 0x040000F1 RID: 241
		private const int k_SubdivisionCountMax = 1024;

		// Token: 0x040000F2 RID: 242
		public const float DefaultTension = 0.33333334f;

		// Token: 0x040000F3 RID: 243
		public const float CatmullRomTension = 0.5f;

		// Token: 0x040000F4 RID: 244
		public const int PickResolutionMin = 2;

		// Token: 0x040000F5 RID: 245
		public const int PickResolutionDefault = 4;

		// Token: 0x040000F6 RID: 246
		public const int PickResolutionMax = 64;

		// Token: 0x040000F7 RID: 247
		public const int DrawResolutionDefault = 10;

		// Token: 0x02000071 RID: 113
		private struct Segment
		{
			// Token: 0x0600038D RID: 909 RVA: 0x00011085 File Offset: 0x0000F285
			public Segment(float start, float length)
			{
				this.start = start;
				this.length = length;
			}

			// Token: 0x0400017A RID: 378
			public float start;

			// Token: 0x0400017B RID: 379
			public float length;
		}
	}
}
