using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000028 RID: 40
	public static class SplineFactory
	{
		// Token: 0x060001EB RID: 491 RVA: 0x00008AD5 File Offset: 0x00006CD5
		public static Spline CreateLinear(IList<float3> positions, bool closed = false)
		{
			return SplineFactory.CreateLinear(positions, null, closed);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008AE0 File Offset: 0x00006CE0
		public static Spline CreateLinear(IList<float3> positions, IList<quaternion> rotations, bool closed = false)
		{
			int count = positions.Count;
			Spline spline = new Spline(count, closed);
			for (int i = 0; i < count; i++)
			{
				float3 @float = positions[i];
				quaternion quaternion = ((rotations != null) ? rotations[i] : quaternion.identity);
				float3 zero = float3.zero;
				float3 zero2 = float3.zero;
				spline.Add(new BezierKnot(@float, zero, zero2, quaternion), TangentMode.Linear);
			}
			return spline;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00008B45 File Offset: 0x00006D45
		public static Spline CreateCatmullRom(IList<float3> positions, bool closed = false)
		{
			return SplineFactory.CreateCatmullRom(positions, null, closed);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00008B50 File Offset: 0x00006D50
		internal static Spline CreateCatmullRom(IList<float3> positions, IList<quaternion> rotations, bool closed = false)
		{
			int count = positions.Count;
			Spline spline = new Spline(count, closed);
			for (int i = 0; i < count; i++)
			{
				float3 @float = positions[i];
				quaternion quaternion = ((rotations != null) ? rotations[i] : quaternion.identity);
				int num = SplineUtility.NextIndex(i, count, closed);
				int num2 = SplineUtility.PreviousIndex(i, count, closed);
				float3 float2 = math.rotate(math.inverse(quaternion), SplineUtility.GetAutoSmoothTangent(positions[num2], positions[i], positions[num], 0.5f));
				float3 float3 = -float2;
				spline.Add(new BezierKnot(@float, float3, float2, quaternion), TangentMode.AutoSmooth);
			}
			return spline;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00008BF8 File Offset: 0x00006DF8
		public static Spline CreateRoundedSquare(float radius, float rounding)
		{
			float3 @float = new float3(-0.5f, 0f, -0.5f);
			float3 float2 = new float3(-0.5f, 0f, 0.5f);
			float3 float3 = new float3(0.5f, 0f, 0.5f);
			float3 float4 = new float3(0.5f, 0f, -0.5f);
			float3 float5 = new float3(0f, 0f, -1f);
			float3 float6 = new float3(0f, 0f, 1f);
			Spline spline = new Spline(new BezierKnot[]
			{
				new BezierKnot(@float * radius, float5 * rounding, float6 * rounding, Quaternion.Euler(0f, -45f, 0f)),
				new BezierKnot(float2 * radius, float5 * rounding, float6 * rounding, Quaternion.Euler(0f, 45f, 0f)),
				new BezierKnot(float3 * radius, float5 * rounding, float6 * rounding, Quaternion.Euler(0f, 135f, 0f)),
				new BezierKnot(float4 * radius, float5 * rounding, float6 * rounding, Quaternion.Euler(0f, -135f, 0f))
			}, true);
			for (int i = 0; i < spline.Count; i++)
			{
				spline.SetTangentMode(i, TangentMode.Mirrored, BezierTangent.Out);
			}
			return spline;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008DAC File Offset: 0x00006FAC
		public static Spline CreateHelix(float radius, float height, int revolutions)
		{
			revolutions = math.max(1, revolutions);
			float num = height / (float)revolutions;
			float num2 = 1.5707964f;
			float num3 = num / 6.2831855f;
			float num4 = radius * math.cos(num2);
			float num5 = radius * math.sin(num2);
			float num6 = num3 * num2 * (radius - num4) * (3f * radius - num4) / (num5 * (4f * radius - num4) * math.tan(num2));
			float num7 = num * 0.25f;
			float3 @float = new float3(num4, -num2 * num3 + num7, -num5);
			float3 float2 = new float3((4f * radius - num4) / 3f, -num6 + num7, -(radius - num4) * (3f * radius - num4) / (3f * num5));
			float3 float3 = new float3((4f * radius - num4) / 3f, num6 + num7, (radius - num4) * (3f * radius - num4) / (3f * num5));
			float3 float4 = new float3(num4, num2 * num3 + num7, num5);
			Spline spline = new Spline();
			float3 float5 = float2 - @float;
			float num8 = math.length(float5);
			float3 float6 = math.normalize(float5);
			float3 float7 = math.cross(math.cross(float6, math.up()), float6);
			spline.Add(new BezierKnot(@float, new float3(0f, 0f, -num8), new float3(0f, 0f, num8), quaternion.LookRotation(float6, float7)));
			float6 = math.normalize(float4 - float3);
			float7 = math.cross(math.cross(float6, math.up()), float6);
			spline.Add(new BezierKnot(float4, new float3(0f, 0f, -num8), new float3(0f, 0f, num8), quaternion.LookRotation(float6, float7)));
			quaternion quaternion = quaternion.AxisAngle(math.up(), math.radians(180f));
			num7 = num * 0.5f;
			float4 = math.rotate(quaternion, float4);
			float4.y += num7;
			float6 = math.normalize(float2 - @float);
			float7 = math.cross(math.cross(float6, math.up()), float6);
			spline.Add(new BezierKnot(float4, new float3(0f, 0f, -num8), new float3(0f, 0f, num8), quaternion.LookRotation(float6, float7)));
			float3 float8 = new float3(0f, num, 0f);
			for (int i = 1; i < revolutions; i++)
			{
				Spline spline2 = spline;
				BezierKnot bezierKnot = spline2[spline2.Count - 1];
				bezierKnot.Position += float8;
				Spline spline3 = spline;
				BezierKnot bezierKnot2 = spline3[spline3.Count - 2];
				bezierKnot2.Position += float8;
				spline.Add(bezierKnot2);
				spline.Add(bezierKnot);
			}
			return spline;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00009084 File Offset: 0x00007284
		public static Spline CreateRoundedCornerSquare(float size, float cornerRadius)
		{
			float num = size * 0.5f;
			cornerRadius = math.clamp(cornerRadius, 0f, num);
			if (cornerRadius == 0f)
			{
				return SplineFactory.CreateSquare(size);
			}
			float3 @float = new float3(-num, 0f, num - cornerRadius);
			float3 float2 = new float3(-num + cornerRadius, 0f, num);
			float num2 = SplineMath.GetUnitCircleTangentLength() * cornerRadius;
			float num3 = 0f;
			Spline spline = new Spline();
			for (int i = 0; i < 4; i++)
			{
				Quaternion quaternion = Quaternion.Euler(0f, num3, 0f);
				if (cornerRadius < 1f)
				{
					spline.Add(new BezierKnot(quaternion * @float, new float3(0f, 0f, -math.min(num2, 0f)), new float3(0f, 0f, num2), Quaternion.identity * quaternion));
					spline.Add(new BezierKnot(quaternion * float2, new float3(0f, 0f, -num2), new float3(0f, 0f, math.min(num2, 0f)), Quaternion.Euler(0f, 90f, 0f) * quaternion));
				}
				else
				{
					spline.Add(new BezierKnot(quaternion * @float, new float3(0f, 0f, -num2), new float3(0f, 0f, num2), Quaternion.identity * quaternion));
				}
				num3 += 90f;
			}
			spline.Closed = true;
			return spline;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000924C File Offset: 0x0000744C
		public static Spline CreateSquare(float size)
		{
			float3 @float = new float3(-0.5f, 0f, -0.5f) * size;
			float3 float2 = new float3(-0.5f, 0f, 0.5f) * size;
			float3 float3 = new float3(0.5f, 0f, 0.5f) * size;
			float3 float4 = new float3(0.5f, 0f, -0.5f) * size;
			return SplineFactory.CreateLinear(new float3[] { @float, float2, float3, float4 }, true);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000092F4 File Offset: 0x000074F4
		public static Spline CreateCircle(float radius)
		{
			float3 @float = new float3(-radius, 0f, 0f);
			float3 float2 = new float3(0f, 0f, SplineMath.GetUnitCircleTangentLength() * radius);
			Spline spline = new Spline();
			quaternion quaternion = quaternion.identity;
			for (int i = 0; i < 4; i++)
			{
				spline.Add(new BezierKnot(math.rotate(quaternion, @float), -float2, float2, quaternion));
				quaternion = math.mul(quaternion, quaternion.AxisAngle(math.up(), 1.5707964f));
			}
			spline.Closed = true;
			return spline;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00009380 File Offset: 0x00007580
		public static Spline CreatePolygon(float edgeSize, int sides)
		{
			sides = math.max(3, sides);
			float3[] array = new float3[sides];
			float num = 6.2831855f / (float)sides;
			float num2 = edgeSize * 0.5f / math.sin(num * 0.5f);
			float3 @float = new float3(0f, 0f, num2);
			quaternion quaternion = quaternion.identity;
			for (int i = 0; i < sides; i++)
			{
				array[i] = math.rotate(quaternion, @float);
				quaternion = math.mul(quaternion, quaternion.AxisAngle(math.up(), num));
			}
			return SplineFactory.CreateLinear(array, true);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00009410 File Offset: 0x00007610
		public static Spline CreateStarPolygon(float edgeSize, int corners, float concavity)
		{
			concavity = math.clamp(concavity, 0f, 1f);
			if (concavity == 0f)
			{
				SplineFactory.CreatePolygon(edgeSize, corners);
			}
			corners = math.max(3, corners);
			int num = corners * 2;
			float3[] array = new float3[num];
			float num2 = 6.2831855f / (float)corners;
			float num3 = edgeSize * 0.5f / math.sin(num2 * 0.5f);
			float3 @float = new float3(0f, 0f, num3);
			quaternion quaternion = quaternion.identity;
			for (int i = 0; i < num; i += 2)
			{
				array[i] = math.rotate(quaternion, @float);
				quaternion = math.mul(quaternion, quaternion.AxisAngle(math.up(), num2));
				if (i != 0)
				{
					array[i - 1] = (array[i - 2] + array[i]) * 0.5f * (1f - concavity);
				}
				if (i == num - 2)
				{
					array[i + 1] = (array[0] + array[i]) * 0.5f * (1f - concavity);
				}
			}
			return SplineFactory.CreateLinear(array, true);
		}
	}
}
