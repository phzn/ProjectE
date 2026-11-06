using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000105 RID: 261
	public static class MathUtility
	{
		// Token: 0x06000497 RID: 1175 RVA: 0x0002B823 File Offset: 0x00029A23
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp1(float a)
		{
			return math.clamp(a, -1f, 1f);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0002B835 File Offset: 0x00029A35
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 Project(in float3 v, in float3 n)
		{
			return math.dot(v, n) * n;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0002B853 File Offset: 0x00029A53
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ProjectOnPlane(in float3 v, in float3 n)
		{
			return v - math.dot(v, n) * n;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0002B87C File Offset: 0x00029A7C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Angle(in float3 v1, in float3 v2)
		{
			float num = math.length(v1);
			float num2 = math.length(v2);
			return math.acos(MathUtility.Clamp1(math.dot(v1, v2) / (num * num2)));
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0002B8C0 File Offset: 0x00029AC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ClampVector(float3 v, float minlength, float maxlength)
		{
			float num = math.length(v);
			if (num > 1E-09f)
			{
				if (num > maxlength)
				{
					v *= maxlength / num;
				}
				else if (num < minlength)
				{
					v *= minlength / num;
				}
			}
			return v;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0002B900 File Offset: 0x00029B00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ClampVector(float3 v, float maxlength)
		{
			float num = math.length(v);
			if (num > 1E-09f && num > maxlength)
			{
				v *= maxlength / num;
			}
			return v;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0002B92C File Offset: 0x00029B2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ClampDistance(float3 from, float3 to, float maxlength)
		{
			float num = math.distance(from, to);
			if (num <= maxlength)
			{
				return to;
			}
			float num2 = maxlength / num;
			return math.lerp(from, to, num2);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0002B954 File Offset: 0x00029B54
		public static bool ClampAngle(in float3 dir, in float3 basedir, float maxAngle, out float3 outdir)
		{
			float3 @float = math.normalize(dir);
			float3 float2 = math.normalize(basedir);
			float num = MathUtility.Clamp1(math.dot(@float, float2));
			float num2 = math.acos(num);
			if (num2 <= maxAngle)
			{
				outdir = dir;
				return false;
			}
			float num3 = (num2 - maxAngle) / num2;
			float3 float3 = math.cross(@float, float2);
			if (math.abs(1f + num) < 1E-06f)
			{
				num2 = 3.1415927f;
				if (@float.x > @float.y && @float.x > @float.z)
				{
					float3 = math.cross(@float, new float3(0f, 1f, 0f));
				}
				else
				{
					float3 = math.cross(@float, new float3(1f, 0f, 0f));
				}
			}
			else if (math.abs(1f - num) < 1E-06f)
			{
				outdir = dir;
				return false;
			}
			quaternion quaternion = quaternion.AxisAngle(math.normalize(float3), num2 * num3);
			outdir = math.mul(quaternion, dir);
			return true;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0002BA68 File Offset: 0x00029C68
		public static quaternion FromToRotation(in float3 from, in float3 to, float t = 1f)
		{
			float3 @float = math.normalize(from);
			float3 float2 = math.normalize(to);
			float num = MathUtility.Clamp1(math.dot(@float, float2));
			float num2 = math.acos(num);
			float3 float3 = math.cross(@float, float2);
			if (math.abs(1f + num) < 1E-06f)
			{
				num2 = 3.1415927f;
				if (@float.x > @float.y && @float.x > @float.z)
				{
					float3 = math.cross(@float, new float3(0f, 1f, 0f));
				}
				else
				{
					float3 = math.cross(@float, new float3(1f, 0f, 0f));
				}
			}
			else if (math.abs(1f - num) < 1E-06f)
			{
				return quaternion.identity;
			}
			return quaternion.AxisAngle(math.normalize(float3), num2 * t);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0002BB44 File Offset: 0x00029D44
		public static quaternion FromToRotationWithoutNormalize(in float3 v1, in float3 v2, float t = 1f)
		{
			float num = MathUtility.Clamp1(math.dot(v1, v2));
			float num2 = math.acos(num);
			float3 @float = math.cross(v1, v2);
			if (math.abs(1f + num) < 1E-06f)
			{
				num2 = 3.1415927f;
				if (v1.x > v1.y && v1.x > v1.z)
				{
					@float = math.cross(v1, new float3(0f, 1f, 0f));
				}
				else
				{
					@float = math.cross(v1, new float3(1f, 0f, 0f));
				}
			}
			else if (math.abs(1f - num) < 1E-06f)
			{
				return quaternion.identity;
			}
			return quaternion.AxisAngle(math.normalize(@float), num2 * t);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0002BC21 File Offset: 0x00029E21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion FromToRotation(in quaternion from, in quaternion to)
		{
			return math.mul(to, math.inverse(from));
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0002BC3C File Offset: 0x00029E3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Angle(in quaternion a, in quaternion b)
		{
			float num = math.dot(a, b);
			if (math.abs(num) >= 0.9999f)
			{
				return 0f;
			}
			float num2 = math.acos(MathUtility.Clamp1(num)) * 2f;
			if (num2 <= 3.1415927f)
			{
				return num2;
			}
			return 6.2831855f - num2;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0002BC94 File Offset: 0x00029E94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion ClampAngle(quaternion from, quaternion to, float maxAngle)
		{
			float num = MathUtility.Angle(in from, in to);
			if (num <= maxAngle)
			{
				return to;
			}
			float num2 = maxAngle / num;
			return math.slerp(from, to, num2);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0002BCBD File Offset: 0x00029EBD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion ToRotation(in float3 nor, in float3 tan)
		{
			return quaternion.LookRotation(tan, nor);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0002BCD0 File Offset: 0x00029ED0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToNormalTangent(in quaternion rot, out float3 nor, out float3 tan)
		{
			nor = math.mul(rot, math.up());
			tan = math.mul(rot, math.forward());
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0002BCFE File Offset: 0x00029EFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ToNormal(in quaternion rot)
		{
			return math.mul(rot, math.up());
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0002BD10 File Offset: 0x00029F10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ToTangent(in quaternion rot)
		{
			return math.mul(rot, math.forward());
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0002BD22 File Offset: 0x00029F22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ToBinormal(in quaternion rot)
		{
			return math.mul(rot, math.right());
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0002BD34 File Offset: 0x00029F34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 Binormal(in float3 nor, in float3 tan)
		{
			return math.cross(nor, tan);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0002BD48 File Offset: 0x00029F48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 AxisToEuler(in float3 axis)
		{
			float num = math.atan2(axis.x, axis.z);
			return new float3(math.atan2(-axis.y, math.length(axis - new float3(0f, axis.y, 0f))), num, 0f);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0002BDA3 File Offset: 0x00029FA3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion AxisQuaternion(float3 dir)
		{
			return quaternion.Euler(MathUtility.AxisToEuler(in dir), math.RotationOrder.ZXY);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0002BDB4 File Offset: 0x00029FB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ToAngleAxis(in quaternion q, out float angle, out float3 axis)
		{
			float num = ((math.abs(q.value.w) < 0.9999f) ? math.acos(q.value.w) : 0f);
			angle = 2f * num;
			float num2 = math.sin(num);
			if (math.abs(num2) < 1E-06f)
			{
				axis = 0;
				return;
			}
			float4 value = q.value;
			axis = value.xyz / num2;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0002BE34 File Offset: 0x0002A034
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ToEuler(in quaternion q)
		{
			float3 @float = 0;
			float x = q.value.x;
			float y = q.value.y;
			float z = q.value.z;
			float w = q.value.w;
			float num = 2f * (w * x - z * y);
			if (math.abs(num) >= 0.99999f)
			{
				@float.x = math.sign(num) * 90f;
				@float.y = math.atan2(2f * (w * y + x * z), 1f - 2f * (x * x + y * y));
				@float.y = math.degrees(@float.y);
				@float.z = 0f;
			}
			else
			{
				@float.x = math.asin(num);
				@float.y = math.atan2(2f * (w * y + x * z), 1f - 2f * (x * x + y * y));
				@float.z = math.atan2(2f * (w * z + x * y), 1f - 2f * (x * x + z * z));
				@float = math.degrees(@float);
			}
			return @float;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0002BF68 File Offset: 0x0002A168
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ClosestPtPointSegmentRatio(in float3 c, in float3 a, in float3 b)
		{
			float3 @float = b - a;
			float num = math.dot(@float, @float);
			if (num == 0f)
			{
				return 0f;
			}
			return math.saturate(math.dot(c - a, @float) / num);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0002BFBC File Offset: 0x0002A1BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ClosestPtPointSegmentRatioNoClamp(float3 c, float3 a, float3 b)
		{
			float3 @float = b - a;
			float num = math.dot(@float, @float);
			return math.dot(c - a, @float) / num;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0002BFE8 File Offset: 0x0002A1E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ClosestPtPointSegment(float3 c, float3 a, float3 b)
		{
			float3 @float = b - a;
			float num = math.dot(@float, @float);
			float num2 = math.dot(c - a, @float) / num;
			num2 = math.saturate(num2);
			return a + num2 * @float;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0002C02C File Offset: 0x0002A22C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ClosestPtPointSegmentNoClamp(float3 c, float3 a, float3 b)
		{
			float3 @float = b - a;
			float num = math.dot(@float, @float);
			float num2 = math.dot(c - a, @float) / num;
			return a + num2 * @float;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0002C068 File Offset: 0x0002A268
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ClosestPtSegmentSegment(in float3 p1, in float3 q1, in float3 p2, in float3 q2, out float s, out float t, out float3 c1, out float3 c2)
		{
			float3 @float = q1 - p1;
			float3 float2 = q2 - p2;
			float3 float3 = p1 - p2;
			float num = math.dot(@float, @float);
			float num2 = math.dot(float2, float2);
			float num3 = math.dot(float2, float3);
			if (num <= 1E-08f && num2 <= 1E-08f)
			{
				s = (t = 0f);
				c1 = p1;
				c2 = p2;
				return math.dot(c1 - c2, c1 - c2);
			}
			if (num <= 1E-08f)
			{
				s = 0f;
				t = math.saturate(num3 / num2);
			}
			else
			{
				float num4 = math.dot(@float, float3);
				if (num2 <= 1E-08f)
				{
					t = 0f;
					s = math.saturate(-num4 / num);
				}
				else
				{
					float num5 = math.dot(@float, float2);
					float num6 = num * num2 - num5 * num5;
					if (num6 != 0f)
					{
						s = math.saturate((num5 * num3 - num4 * num2) / num6);
					}
					else
					{
						s = 0f;
					}
					t = (num5 * s + num3) / num2;
					if (t < 0f)
					{
						t = 0f;
						s = math.saturate(-num4 / num);
					}
					else if (t > 1f)
					{
						t = 1f;
						s = math.saturate((num5 - num4) / num);
					}
				}
			}
			c1 = p1 + @float * s;
			c2 = p2 + float2 * t;
			return math.dot(c1 - c2, c1 - c2);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0002C26C File Offset: 0x0002A46C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClosestPtSegmentSegment2(in float3 p1, in float3 q1, in float3 p2, in float3 q2, out float s, out float t)
		{
			float3 @float = q1 - p1;
			float3 float2 = q2 - p2;
			float3 float3 = p1 - p2;
			float num = math.dot(@float, @float);
			float num2 = math.dot(float2, float2);
			float num3 = math.dot(float2, float3);
			if (num <= 1E-08f && num2 <= 1E-08f)
			{
				s = (t = 0f);
				return;
			}
			if (num <= 1E-08f)
			{
				s = 0f;
				t = math.saturate(num3 / num2);
				return;
			}
			float num4 = math.dot(@float, float3);
			if (num2 <= 1E-08f)
			{
				t = 0f;
				s = math.saturate(-num4 / num);
				return;
			}
			float num5 = math.dot(@float, float2);
			float num6 = num * num2 - num5 * num5;
			if (num6 != 0f)
			{
				s = math.saturate((num5 * num3 - num4 * num2) / num6);
			}
			else
			{
				s = 0f;
			}
			t = (num5 * s + num3) / num2;
			if (t < 0f)
			{
				t = 0f;
				s = math.saturate(-num4 / num);
				return;
			}
			if (t > 1f)
			{
				t = 1f;
				s = math.saturate((num5 - num4) / num);
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0002C3C0 File Offset: 0x0002A5C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 ClosestPtPointTriangle(in float3 p, in float3 a, in float3 b, in float3 c, out float3 uvw)
		{
			uvw = 0;
			float3 @float = b - a;
			float3 float2 = c - a;
			float3 float3 = p - a;
			float num = math.dot(@float, float3);
			float num2 = math.dot(float2, float3);
			if (num <= 0f && num2 <= 0f)
			{
				uvw.x = 1f;
				return a;
			}
			float3 float4 = p - b;
			float num3 = math.dot(@float, float4);
			float num4 = math.dot(float2, float4);
			if (num3 >= 0f && num4 <= num3)
			{
				uvw.y = 1f;
				return b;
			}
			float num5 = num * num4 - num3 * num2;
			float num6;
			if (num5 <= 0f && num >= 0f && num3 <= 0f)
			{
				num6 = num / (num - num3);
				uvw = new float3(1f - num6, num6, 0f);
				return a + num6 * @float;
			}
			float3 float5 = p - c;
			float num7 = math.dot(@float, float5);
			float num8 = math.dot(float2, float5);
			if (num8 >= 0f && num7 <= num8)
			{
				uvw.z = 1f;
				return c;
			}
			float num9 = num7 * num2 - num * num8;
			float num10;
			if (num9 <= 0f && num2 >= 0f && num8 <= 0f)
			{
				num10 = num2 / (num2 - num8);
				uvw = new float3(1f - num10, 0f, num10);
				return a + num10 * float2;
			}
			float num11 = num3 * num8 - num7 * num4;
			if (num11 <= 0f && num4 - num3 >= 0f && num7 - num8 >= 0f)
			{
				float num12 = num4 - num3 + (num7 - num8);
				num10 = (num4 - num3) / num12;
				uvw = new float3(0f, 1f - num10, num10);
				return b + num10 * (c - b);
			}
			float num13 = num11 + num9 + num5;
			float num14 = 1f / num13;
			num6 = num9 * num14;
			num10 = num5 * num14;
			uvw = new float3(1f - num6 - num10, num6, num10);
			return a + @float * num6 + float2 * num10;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0002C677 File Offset: 0x0002A877
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool PointInTriangleUVW(float3 uvw)
		{
			return math.all(uvw);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0002C67F File Offset: 0x0002A87F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 TriangleCenter(in float3 p0, in float3 p1, in float3 p2)
		{
			return (p0 + p1 + p2) / 3f;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0002C6A7 File Offset: 0x0002A8A7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 TriangleNormal(in float3 p0, in float3 p1, in float3 p2)
		{
			return math.normalize(math.cross(p1 - p0, p2 - p0));
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0002C6D5 File Offset: 0x0002A8D5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float TriangleArea(in float3 p0, in float3 p1, in float3 p2)
		{
			return math.length(math.cross(p1 - p0, p2 - p0));
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0002C703 File Offset: 0x0002A903
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSafeTriangle(in float3 p0, in float3 p1, in float3 p2)
		{
			return MathUtility.TriangleArea(in p0, in p1, in p2) > 1E-06f;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0002C714 File Offset: 0x0002A914
		public static float3 TriangleTangent(in float3 p0, in float3 p1, in float3 p2, in float2 uv0, in float2 uv1, in float2 uv2)
		{
			float3 @float = p1 - p0;
			float3 float2 = p2 - p0;
			float2 float3 = uv1 - uv0;
			float2 float4 = uv2 - uv0;
			float num = float3.x * float4.y - float3.y * float4.x;
			0;
			if (num == 0f)
			{
				num = 1f;
			}
			float num2 = 1f / num;
			return math.normalizesafe(-(new float3(@float.x * float4.y + float2.x * -float3.y, @float.y * float4.y + float2.y * -float3.y, @float.z * float4.y + float2.z * -float3.y) * num2), 0);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0002C81C File Offset: 0x0002AA1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float TriangleAngle(in float3 v0, in float3 v1, in float3 v2, in float3 v3)
		{
			float3 @float = v1 - v0;
			float3 float2 = v2 - v0;
			float3 float3 = v3 - v0;
			float3 float4 = math.cross(float2, @float);
			float3 float5 = math.cross(@float, float3);
			return MathUtility.Angle(in float4, in float5);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0002C878 File Offset: 0x0002AA78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistanceTriangleCenter(float3 p, float3 p0, float3 p1, float3 p2)
		{
			float3 @float = (p0 + p1 + p2) / 3f;
			return math.distance(p, @float);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0002C8A4 File Offset: 0x0002AAA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DirectionPointTriangle(float3 p, float3 a, float3 b, float3 c)
		{
			float3 @float = b - a;
			float3 float2 = c - a;
			float3 float3 = p - a;
			float3 float4 = math.cross(@float, float2);
			return math.sign(math.dot(float3, float4));
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0002C8DC File Offset: 0x0002AADC
		public static int2 GetRestTriangleVertex(int3 tri1, int3 tri2, int2 edge)
		{
			int2 @int = -1;
			for (int i = 0; i < 3; i++)
			{
				int num = tri1[i];
				if (num != edge.x && num != edge.x && num != edge.y && num != edge.y)
				{
					@int[0] = num;
					break;
				}
			}
			for (int j = 0; j < 3; j++)
			{
				int num2 = tri2[j];
				if (num2 != edge.x && num2 != edge.x && num2 != edge.y && num2 != edge.y)
				{
					@int[1] = num2;
					break;
				}
			}
			return @int;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0002C980 File Offset: 0x0002AB80
		public static int2 GetCommonEdgeFromTrianglePair(int3 tri1, int3 tri2)
		{
			int2 @int = 0;
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				if (tri1[i] == tri2.x || tri1[i] == tri2.y || tri1[i] == tri2.z)
				{
					@int[num] = tri1[i];
					num++;
				}
			}
			if (num != 2)
			{
				Debug.LogError("Common edge nothing!");
				return 0;
			}
			if (@int.x > @int.y)
			{
				int x = @int.x;
				@int.x = @int.y;
				@int.y = x;
			}
			return @int;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0002CA28 File Offset: 0x0002AC28
		public static int4 GetTrianglePairIndices(int3 tri1, int3 tri2)
		{
			int2 commonEdgeFromTrianglePair = MathUtility.GetCommonEdgeFromTrianglePair(tri1, tri2);
			int4 @int = new int4(0, 0, commonEdgeFromTrianglePair.x, commonEdgeFromTrianglePair.y);
			for (int i = 0; i < 3; i++)
			{
				if (tri1[i] != commonEdgeFromTrianglePair.x && tri1[i] != commonEdgeFromTrianglePair.y)
				{
					@int[0] = tri1[i];
				}
				if (tri2[i] != commonEdgeFromTrianglePair.x && tri2[i] != commonEdgeFromTrianglePair.y)
				{
					@int[1] = tri2[i];
				}
			}
			return @int;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0002CAC0 File Offset: 0x0002ACC0
		public static int GetUnuseTriangleIndex(int3 tri, int2 edge)
		{
			if (tri.x != edge.x && tri.x != edge.y)
			{
				return tri.x;
			}
			if (tri.y != edge.x && tri.y != edge.y)
			{
				return tri.y;
			}
			if (tri.z != edge.x && tri.z != edge.y)
			{
				return tri.z;
			}
			return -1;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0002CB38 File Offset: 0x0002AD38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetTrianglePairAngle(float3 pos0, float3 pos1, float3 pos2, float3 pos3)
		{
			float3 @float = pos3 - pos2;
			float3 float2 = pos0 - pos2;
			float3 float3 = pos1 - pos2;
			float3 float4 = math.cross(@float, float2);
			float3 float5 = math.cross(float3, @float);
			return MathUtility.Angle(in float4, in float5);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0002CB74 File Offset: 0x0002AD74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 FlipTriangle(in int3 tri)
		{
			int3 @int = tri;
			return @int.xzy;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0002CB90 File Offset: 0x0002AD90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetTriangleSphere(float3 pos0, float3 pos1, float3 pos2, out float3 sc, out float sr)
		{
			float3 @float = math.max(math.max(pos0, pos1), pos2);
			float3 float2 = math.min(math.min(pos0, pos1), pos2);
			sc = (float2 + @float) * 0.5f;
			sr = math.distance(float2, @float) * 0.5f;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0002CBE0 File Offset: 0x0002ADE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 LocalToWorldMatrix(in float3 wpos, in quaternion wrot, in float3 wscl)
		{
			return Matrix4x4.TRS(wpos, wrot, wscl);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0002CC0D File Offset: 0x0002AE0D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 WorldToLocalMatrix(in float3 wpos, in quaternion wrot, in float3 wscl)
		{
			return math.inverse(Matrix4x4.TRS(wpos, wrot, wscl));
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0002CC3F File Offset: 0x0002AE3F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 TransformPoint(in float3 pos, in float3 wpos, in quaternion wrot, in float3 wscl)
		{
			return math.transform(Matrix4x4.TRS(wpos, wrot, wscl), pos);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0002CC77 File Offset: 0x0002AE77
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 TransformPoint(in float3 pos, in float4x4 m)
		{
			return math.transform(m, pos);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0002CC8C File Offset: 0x0002AE8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 TransformVector(in float3 vec, in float4x4 m)
		{
			return math.mul(m, new float4(vec, 0f)).xyz;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0002CCBC File Offset: 0x0002AEBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 TransformDirection(in float3 dir, in float4x4 m)
		{
			float num = math.length(dir);
			if (num > 0f)
			{
				return math.normalize(MathUtility.TransformVector(in dir, in m)) * num;
			}
			return dir;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0002CCF6 File Offset: 0x0002AEF6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 TransformNormal(in float3 dir, in float4x4 m, float3 errDir)
		{
			return math.normalizesafe(MathUtility.TransformVector(in dir, in m), errDir);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0002CD08 File Offset: 0x0002AF08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static quaternion TransformRotation(in quaternion rot, in float4x4 m, in float3 normalTangentFlip)
		{
			float3 @float;
			float3 float2;
			MathUtility.ToNormalTangent(in rot, out @float, out float2);
			@float = math.mul(m, new float4(@float, 0f)).xyz * normalTangentFlip.y;
			float2 = math.mul(m, new float4(float2, 0f)).xyz * normalTangentFlip.z;
			return quaternion.LookRotation(float2, @float);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0002CD7C File Offset: 0x0002AF7C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float TransformDistance(in float dist, in float4x4 localToWorldMatrix)
		{
			return math.csum(math.mul(localToWorldMatrix, new float4(dist, dist, dist, 0f)).xyz) / 3f;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0002CDB8 File Offset: 0x0002AFB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TransformPositionNormalTangent(in float3 tpos, in quaternion trot, in float3 tscl, ref float3 pos, ref float3 nor, ref float3 tan)
		{
			pos *= tscl;
			pos = math.mul(trot, pos);
			pos += tpos;
			nor = math.mul(trot, nor);
			tan = math.mul(trot, tan);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0002CE3C File Offset: 0x0002B03C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float TransformLength(float length, in float4x4 matrix)
		{
			return math.length(math.mul(matrix, new float4(length, length, length, 0f)).xyz) / 1.73205f;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0002CC77 File Offset: 0x0002AE77
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 InverseTransformPoint(in float3 pos, in float4x4 worldToLocalMatrix)
		{
			return math.transform(worldToLocalMatrix, pos);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0002CE74 File Offset: 0x0002B074
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 InverseTransformPoint(in float3 pos, in float3 wpos, in quaternion wrot, in float3 wscl)
		{
			return math.transform(math.inverse(Matrix4x4.TRS(wpos, wrot, wscl)), pos);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0002CEB4 File Offset: 0x0002B0B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 InverseTransformVector(in float3 vec, in float4x4 worldToLocalMatrix)
		{
			return math.mul(worldToLocalMatrix, new float4(vec, 0f)).xyz;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0002CEE4 File Offset: 0x0002B0E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 InverseTransformVector(in float3 vec, in quaternion rot)
		{
			return math.mul(math.inverse(rot), vec);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0002CEFC File Offset: 0x0002B0FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 InverseTransformDirection(in float3 dir, in float4x4 worldToLocalMatrix)
		{
			float num = math.length(dir);
			if (num > 0f)
			{
				return math.normalize(MathUtility.InverseTransformVector(in dir, in worldToLocalMatrix)) * num;
			}
			return dir;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0002CF36 File Offset: 0x0002B136
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 Transform(in float4x4 fromLocalToWorldMatrix, in float4x4 toWorldToLocalMatrix)
		{
			return math.mul(toWorldToLocalMatrix, fromLocalToWorldMatrix);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0002CF4C File Offset: 0x0002B14C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool CompareMatrix(in float4x4 m1, in float4x4 m2)
		{
			bool4x4 bool4x = m1 == m2;
			return math.all(bool4x.c0) && math.all(bool4x.c1) && math.all(bool4x.c2) && math.all(bool4x.c3);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0002CFA0 File Offset: 0x0002B1A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool CompareTransform(in float3 pos1, in quaternion rot1, in float3 scl1, in float3 pos2, in quaternion rot2, in float3 scl2)
		{
			float3 @float = pos1;
			if (@float.Equals(pos2))
			{
				quaternion quaternion = rot1;
				if (quaternion.Equals(rot2))
				{
					@float = scl1;
					return @float.Equals(scl2);
				}
			}
			return false;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0002CFF4 File Offset: 0x0002B1F4
		public static bool IntersectSegmentTriangle(in float3 p, in float3 q, float3 a, float3 b, float3 c, bool doubleSide, out float u, out float v, out float w, out float t)
		{
			t = 0f;
			u = 0f;
			v = 0f;
			w = 0f;
			float3 @float = b - a;
			float3 float2 = c - a;
			float3 float3 = p - q;
			float3 float4 = math.cross(@float, float2);
			float num = math.dot(float3, float4);
			if (math.abs(num) < 1E-09f)
			{
				return false;
			}
			if (!doubleSide)
			{
				if (num <= 0f)
				{
					return false;
				}
			}
			else if (num < 0f)
			{
				float4 = -float4;
				float3 float5 = b;
				b = c;
				c = float5;
				@float = b - a;
				float2 = c - a;
				num = math.dot(float3, float4);
			}
			float3 float6 = p - a;
			t = math.dot(float6, float4);
			if (t < 0f)
			{
				return false;
			}
			if (t > num)
			{
				return false;
			}
			float3 float7 = math.cross(float3, float6);
			v = math.dot(float2, float7);
			if (v < 0f || v > num)
			{
				return false;
			}
			w = -math.dot(@float, float7);
			if (w < 0f || v + w > num)
			{
				return false;
			}
			float num2 = 1f / num;
			t *= num2;
			v *= num2;
			w *= num2;
			u = 1f - v - w;
			return true;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0002D154 File Offset: 0x0002B354
		public static bool IntersectSegmentTriangle(in float3 p, in float3 q, float3 a, float3 b, float3 c)
		{
			float3 @float = b - a;
			float3 float2 = c - a;
			float3 float3 = p - q;
			float3 float4 = math.cross(@float, float2);
			float num = math.dot(float3, float4);
			if (math.abs(num) < 1E-09f)
			{
				return false;
			}
			if (num < 0f)
			{
				float4 = -float4;
				float3 float5 = b;
				b = c;
				c = float5;
				@float = b - a;
				float2 = c - a;
				num = math.dot(float3, float4);
			}
			float3 float6 = p - a;
			float num2 = math.dot(float6, float4);
			if (num2 < 0f)
			{
				return false;
			}
			if (num2 > num)
			{
				return false;
			}
			float3 float7 = math.cross(float3, float6);
			float num3 = math.dot(float2, float7);
			if (num3 < 0f || num3 > num)
			{
				return false;
			}
			float num4 = -math.dot(@float, float7);
			return num4 >= 0f && num3 + num4 <= num;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0002D248 File Offset: 0x0002B448
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float IntersectPointPlaneDist(in float3 planePos, in float3 planeDir, in float3 pos, out float3 outPos)
		{
			float3 @float = pos - planePos;
			float3 float2 = MathUtility.Project(in @float, in planeDir);
			float num = math.length(float2);
			if (math.dot(planeDir, @float) < 0f)
			{
				outPos = pos - float2;
				return -num;
			}
			outPos = pos;
			return num;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0002D2AC File Offset: 0x0002B4AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IntersectRaySphere(in float3 p, in float3 d, in float3 sc, in float sr, ref float t, ref float3 q)
		{
			float3 @float = p - sc;
			float num = math.dot(@float, d);
			float num2 = math.dot(@float, @float) - sr * sr;
			if (num2 > 0f && num > 0f)
			{
				return false;
			}
			float num3 = num * num - num2;
			if (num3 < 0f)
			{
				return false;
			}
			t = -num - math.sqrt(num3);
			if (t < 0f)
			{
				t = 0f;
			}
			q = p + t * d;
			return true;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0002D348 File Offset: 0x0002B548
		public static float SqDistPointSegment(Vector3 a, Vector3 b, Vector3 c)
		{
			Vector3 vector = b - a;
			Vector3 vector2 = c - a;
			Vector3 vector3 = c - b;
			float num = Vector3.Dot(vector2, vector);
			if (num <= 0f)
			{
				return Vector3.Dot(vector2, vector2);
			}
			float num2 = Vector3.Dot(vector, vector);
			if (num >= num2)
			{
				return Vector3.Dot(vector3, vector3);
			}
			return Vector3.Dot(vector2, vector2) - num * num / num2;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0002D3A9 File Offset: 0x0002B5A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNaN(float3 v)
		{
			return math.any(math.isnan(v));
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0002D3B6 File Offset: 0x0002B5B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNaN(float4 v)
		{
			return math.any(math.isnan(v));
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0002D3C3 File Offset: 0x0002B5C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNaN(quaternion q)
		{
			return math.any(math.isnan(q.value));
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0002D3D8 File Offset: 0x0002B5D8
		public static float3 ShiftPosition(in float3 oldPos, in float3 oldPivotPosition, in float3 shiftVector, in quaternion shiftRotation)
		{
			float3 @float = oldPos - oldPivotPosition;
			@float = math.mul(shiftRotation, @float);
			@float += shiftVector;
			return oldPivotPosition + @float;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0002D420 File Offset: 0x0002B620
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float CalcMass(float depth)
		{
			float num = 1f - depth;
			return 1f + num * num * 5f;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0002D444 File Offset: 0x0002B644
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float CalcInverseMass(float friction)
		{
			float num = 1f + friction * 3f;
			return 1f / num;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0002D468 File Offset: 0x0002B668
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float CalcInverseMass(float friction, float depth)
		{
			float num = 1f;
			num += friction * 3f;
			float num2 = 1f - depth;
			num += num2 * num2 * 5f;
			return 1f / num;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0002D4A0 File Offset: 0x0002B6A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float CalcInverseMass(float friction, float depth, bool fix, float fixMass)
		{
			if (!fix)
			{
				return MathUtility.CalcInverseMass(friction, depth);
			}
			return 1f / fixMass;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0002D4B4 File Offset: 0x0002B6B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float CalcSelfCollisionInverseMass(float friction, bool fix, float clothMass)
		{
			float num = (fix ? 100f : (1f + friction * 10f));
			num += clothMass * 50f;
			return 1f / num;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0002D4EC File Offset: 0x0002B6EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 CalcSplitRange(int dataLength, int divCount, int divIndex)
		{
			if (dataLength <= 0)
			{
				return -1;
			}
			if (dataLength >= divCount)
			{
				float num = (float)dataLength / (float)divCount;
				int num2 = (int)(num * (float)divIndex);
				int num3 = ((divIndex == divCount - 1) ? dataLength : ((int)(num * (float)(divIndex + 1))));
				return new int2(num2, num3);
			}
			if (divIndex < dataLength)
			{
				return new int2(divIndex, divIndex + 1);
			}
			return -1;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0002D540 File Offset: 0x0002B740
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DataChunk GetWorkerChunk(int dataLenght, int workerCount, int workerIndex)
		{
			int2 @int = MathUtility.CalcSplitRange(dataLenght, workerCount, workerIndex);
			if (@int.x < 0)
			{
				return DataChunk.Empty;
			}
			return new DataChunk(@int.x, @int.y - @int.x);
		}
	}
}
