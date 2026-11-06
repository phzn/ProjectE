using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000008 RID: 8
	public static class CurveUtility
	{
		// Token: 0x06000025 RID: 37 RVA: 0x000027B0 File Offset: 0x000009B0
		public static float3 EvaluatePosition(BezierCurve curve, float t)
		{
			t = math.clamp(t, 0f, 1f);
			float num = t * t;
			float num2 = num * t;
			return curve.P0 * (-1f * num2 + 3f * num - 3f * t + 1f) + curve.P1 * (3f * num2 - 6f * num + 3f * t) + curve.P2 * (-3f * num2 + 3f * num) + curve.P3 * num2;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002858 File Offset: 0x00000A58
		public static float3 EvaluateTangent(BezierCurve curve, float t)
		{
			t = math.clamp(t, 0f, 1f);
			float num = t * t;
			return curve.P0 * (-3f * num + 6f * t - 3f) + curve.P1 * (9f * num - 12f * t + 3f) + curve.P2 * (-9f * num + 6f * t) + curve.P3 * (3f * num);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000028F8 File Offset: 0x00000AF8
		public static float3 EvaluateAcceleration(BezierCurve curve, float t)
		{
			t = math.clamp(t, 0f, 1f);
			return curve.P0 * (-6f * t + 6f) + curve.P1 * (18f * t - 12f) + curve.P2 * (-18f * t + 6f) + curve.P3 * (6f * t);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002980 File Offset: 0x00000B80
		public static float EvaluateCurvature(BezierCurve curve, float t)
		{
			t = math.clamp(t, 0f, 1f);
			float3 @float = CurveUtility.EvaluateTangent(curve, t);
			float3 float2 = CurveUtility.EvaluateAcceleration(curve, t);
			float num = math.lengthsq(@float);
			float num2 = math.lengthsq(float2);
			float num3 = math.dot(@float, float2);
			return math.sqrt(num * num2 - num3 * num3) / (num * math.length(@float));
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000029E0 File Offset: 0x00000BE0
		private static float3 DeCasteljau(BezierCurve curve, float t)
		{
			float3 p = curve.P0;
			float3 p2 = curve.P1;
			float3 p3 = curve.P2;
			float3 p4 = curve.P3;
			float3 @float = math.lerp(p, p2, t);
			float3 float2 = math.lerp(p2, p3, t);
			float3 float3 = math.lerp(p3, p4, t);
			float3 float4 = math.lerp(@float, float2, t);
			float3 float5 = math.lerp(float2, float3, t);
			return math.lerp(float4, float5, t);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A3C File Offset: 0x00000C3C
		public static void Split(BezierCurve curve, float t, out BezierCurve left, out BezierCurve right)
		{
			t = math.clamp(t, 0f, 1f);
			float3 @float = math.lerp(curve.P0, curve.P1, t);
			float3 float2 = math.lerp(curve.P1, curve.P2, t);
			float3 float3 = math.lerp(curve.P2, curve.P3, t);
			float3 float4 = math.lerp(@float, float2, t);
			float3 float5 = math.lerp(float2, float3, t);
			float3 float6 = math.lerp(float4, float5, t);
			left = new BezierCurve(curve.P0, @float, float4, float6);
			right = new BezierCurve(float6, float5, float3, curve.P3);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002AE0 File Offset: 0x00000CE0
		public static float CalculateLength(BezierCurve curve, int resolution = 30)
		{
			float num = 0f;
			float3 @float = CurveUtility.EvaluatePosition(curve, 0f);
			for (int i = 1; i < resolution; i++)
			{
				float3 float2 = CurveUtility.EvaluatePosition(curve, (float)i / ((float)resolution - 1f));
				float3 float3 = float2 - @float;
				num += math.length(float3);
				@float = float2;
			}
			return num;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B30 File Offset: 0x00000D30
		public static void CalculateCurveLengths(BezierCurve curve, DistanceToInterpolation[] lookupTable)
		{
			NativeArray<DistanceToInterpolation> nativeArray = new NativeArray<DistanceToInterpolation>(lookupTable, Allocator.Temp);
			CurveUtility.CalculateCurveLengths(curve, nativeArray);
			nativeArray.CopyTo(lookupTable);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002B58 File Offset: 0x00000D58
		public static void CalculateCurveLengths(BezierCurve curve, NativeArray<DistanceToInterpolation> lookupTable)
		{
			int length = lookupTable.Length;
			float num = 0f;
			float3 @float = CurveUtility.EvaluatePosition(curve, 0f);
			lookupTable[0] = new DistanceToInterpolation
			{
				Distance = 0f,
				T = 0f
			};
			for (int i = 1; i < length; i++)
			{
				float num2 = (float)i / ((float)length - 1f);
				float3 float2 = CurveUtility.EvaluatePosition(curve, num2);
				float3 float3 = float2 - @float;
				num += math.length(float3);
				lookupTable[i] = new DistanceToInterpolation
				{
					Distance = num,
					T = num2
				};
				@float = float2;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002C05 File Offset: 0x00000E05
		private static bool Approximately(float a, float b)
		{
			return math.abs(b - a) < math.max(1E-06f * math.max(math.abs(a), math.abs(b)), 0.0008f);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002C34 File Offset: 0x00000E34
		public static float ApproximateLength(BezierCurve curve)
		{
			float num = math.length(curve.P3 - curve.P0);
			return (math.length(curve.P0 - curve.P1) + math.length(curve.P2 - curve.P1) + math.length(curve.P3 - curve.P2) + num) / 2f;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002CA4 File Offset: 0x00000EA4
		internal static void EvaluateUpVectors(BezierCurve curve, float3 startUp, float3 endUp, NativeArray<float3> upVectors)
		{
			upVectors[0] = startUp;
			upVectors[upVectors.Length - 1] = endUp;
			for (int i = 1; i < upVectors.Length - 1; i++)
			{
				float num = (float)i / (float)(upVectors.Length - 1);
				upVectors[i] = CurveUtility.EvaluateUpVector(curve, num, upVectors[0], endUp, true);
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002D08 File Offset: 0x00000F08
		internal static float3 EvaluateUpVector(BezierCurve curve, float t, float3 startUp, float3 endUp, bool fixEndUpMismatch = true)
		{
			float num = math.length(SplineUtility.GetExplicitLinearTangent(curve.P0, curve.P3));
			float3 @float = math.normalize(curve.P3 - curve.P0) * num;
			if (CurveUtility.Approximately(math.length(curve.P1 - curve.P0), 0f))
			{
				curve.P1 = curve.P0 + @float;
			}
			if (CurveUtility.Approximately(math.length(curve.P2 - curve.P3), 0f))
			{
				curve.P2 = curve.P3 - @float;
			}
			NativeArray<float3> nativeArray = new NativeArray<float3>(16, Allocator.Temp, NativeArrayOptions.ClearMemory);
			CurveUtility.FrenetFrame nextRotationMinimizingFrame;
			nextRotationMinimizingFrame.origin = curve.P0;
			nextRotationMinimizingFrame.tangent = curve.P1 - curve.P0;
			nextRotationMinimizingFrame.normal = startUp;
			nextRotationMinimizingFrame.binormal = math.normalize(math.cross(nextRotationMinimizingFrame.tangent, nextRotationMinimizingFrame.normal));
			if (float.IsNaN(nextRotationMinimizingFrame.binormal.x))
			{
				return float3.zero;
			}
			nativeArray[0] = nextRotationMinimizingFrame.normal;
			float num2 = 0.06666667f;
			float num3 = num2;
			float num4 = 0f;
			float3 float2 = float3.zero;
			for (int i = 1; i < 16; i++)
			{
				CurveUtility.FrenetFrame frenetFrame = nextRotationMinimizingFrame;
				nextRotationMinimizingFrame = CurveUtility.GetNextRotationMinimizingFrame(curve, frenetFrame, num3);
				nativeArray[i] = nextRotationMinimizingFrame.normal;
				if (num4 <= t && num3 >= t)
				{
					float num5 = (t - num4) / num2;
					float2 = Vector3.Slerp(frenetFrame.normal, nextRotationMinimizingFrame.normal, num5);
				}
				num4 = num3;
				num3 += num2;
			}
			if (!fixEndUpMismatch)
			{
				return float2;
			}
			if (num4 <= t && num3 >= t)
			{
				float2 = endUp;
			}
			float3 float3 = nativeArray[15];
			float num6 = math.acos(math.clamp(math.dot(float3, endUp), -1f, 1f));
			if (num6 == 0f)
			{
				return float2;
			}
			float3 float4 = math.normalize(nextRotationMinimizingFrame.tangent);
			quaternion quaternion = quaternion.AxisAngle(float4, num6);
			quaternion quaternion2 = quaternion.AxisAngle(float4, -num6);
			float num7 = math.acos(math.clamp(math.dot(math.rotate(quaternion, endUp), float3), -1f, 1f));
			float num8 = math.acos(math.clamp(math.dot(math.rotate(quaternion2, endUp), float3), -1f, 1f));
			if (num7 > num8)
			{
				num6 *= -1f;
			}
			num3 = num2;
			num4 = 0f;
			for (int j = 1; j < nativeArray.Length; j++)
			{
				float3 float5 = nativeArray[j];
				float num9 = math.lerp(0f, num6, num3);
				float3 float6 = math.rotate(quaternion.AxisAngle(math.normalize(CurveUtility.EvaluateTangent(curve, num3)), -num9), float5);
				nativeArray[j] = float6;
				if (num4 <= t && num3 >= t)
				{
					float num10 = (t - num4) / num2;
					return Vector3.Slerp(nativeArray[j - 1], nativeArray[j], num10);
				}
				num4 = num3;
				num3 += num2;
			}
			return endUp;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003030 File Offset: 0x00001230
		private static CurveUtility.FrenetFrame GetNextRotationMinimizingFrame(BezierCurve curve, CurveUtility.FrenetFrame previousRMFrame, float nextRMFrameT)
		{
			CurveUtility.FrenetFrame frenetFrame;
			frenetFrame.origin = CurveUtility.EvaluatePosition(curve, nextRMFrameT);
			frenetFrame.tangent = CurveUtility.EvaluateTangent(curve, nextRMFrameT);
			float3 @float = frenetFrame.origin - previousRMFrame.origin;
			float num = math.dot(@float, @float);
			float3 float2 = previousRMFrame.binormal - @float * 2f / num * math.dot(@float, previousRMFrame.binormal);
			float3 float3 = previousRMFrame.tangent - @float * 2f / num * math.dot(@float, previousRMFrame.tangent);
			float3 float4 = frenetFrame.tangent - float3;
			float num2 = math.dot(float4, float4);
			frenetFrame.binormal = math.normalize(float2 - float4 * 2f / num2 * math.dot(float4, float2));
			frenetFrame.normal = math.normalize(math.cross(frenetFrame.binormal, frenetFrame.tangent));
			return frenetFrame;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000313A File Offset: 0x0000133A
		public static float GetDistanceToInterpolation(BezierCurve curve, float distance)
		{
			CurveUtility.CalculateCurveLengths(curve, CurveUtility.k_DistanceLUT);
			return CurveUtility.GetDistanceToInterpolation<DistanceToInterpolation[]>(CurveUtility.k_DistanceLUT, distance);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003154 File Offset: 0x00001354
		public static float GetDistanceToInterpolation<T>(T lut, float distance) where T : IReadOnlyList<DistanceToInterpolation>
		{
			if (lut == null || lut.Count < 1 || distance <= 0f)
			{
				return 0f;
			}
			int count = lut.Count;
			float distance2 = lut[count - 1].Distance;
			if (distance >= distance2)
			{
				return 1f;
			}
			DistanceToInterpolation distanceToInterpolation = lut[0];
			for (int i = 1; i < count; i++)
			{
				DistanceToInterpolation distanceToInterpolation2 = lut[i];
				if (distance < distanceToInterpolation2.Distance)
				{
					return math.lerp(distanceToInterpolation.T, distanceToInterpolation2.T, (distance - distanceToInterpolation.Distance) / (distanceToInterpolation2.Distance - distanceToInterpolation.Distance));
				}
				distanceToInterpolation = distanceToInterpolation2;
			}
			return 1f;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000321C File Offset: 0x0000141C
		public static float3 GetNearestPoint(BezierCurve curve, Ray ray, int resolution = 16)
		{
			float3 @float;
			float num;
			CurveUtility.GetNearestPoint(curve, ray, out @float, out num, resolution);
			return @float;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003238 File Offset: 0x00001438
		public static float GetNearestPoint(BezierCurve curve, Ray ray, out float3 position, out float interpolation, int resolution = 16)
		{
			float num = float.PositiveInfinity;
			float num2 = 0f;
			interpolation = 0f;
			position = float3.zero;
			float3 @float = CurveUtility.EvaluatePosition(curve, 0f);
			float3 float2 = ray.origin;
			float3 float3 = ray.direction;
			for (int i = 1; i < resolution; i++)
			{
				float num3 = (float)i / ((float)resolution - 1f);
				float3 float4 = CurveUtility.EvaluatePosition(curve, num3);
				float num4;
				float num5;
				ValueTuple<float3, float3> valueTuple = SplineMath.RayLineNearestPoint(float2, float3, @float, float4, out num4, out num5);
				float3 item = valueTuple.Item1;
				float3 item2 = valueTuple.Item2;
				float num6 = math.lengthsq(item2 - item);
				if (num6 < num)
				{
					position = item2;
					num = num6;
					num2 = num5;
					interpolation = (float)(i - 1) / ((float)resolution - 1f);
				}
				@float = float4;
			}
			interpolation += num2 * (1f / ((float)resolution - 1f));
			return math.sqrt(num);
		}

		// Token: 0x04000012 RID: 18
		private const int k_NormalsPerCurve = 16;

		// Token: 0x04000013 RID: 19
		private const float k_Epsilon = 0.0001f;

		// Token: 0x04000014 RID: 20
		private static readonly DistanceToInterpolation[] k_DistanceLUT = new DistanceToInterpolation[24];

		// Token: 0x02000053 RID: 83
		private struct FrenetFrame
		{
			// Token: 0x0400010C RID: 268
			public float3 origin;

			// Token: 0x0400010D RID: 269
			public float3 tangent;

			// Token: 0x0400010E RID: 270
			public float3 normal;

			// Token: 0x0400010F RID: 271
			public float3 binormal;
		}
	}
}
