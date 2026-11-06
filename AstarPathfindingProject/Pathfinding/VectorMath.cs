using System;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000050 RID: 80
	public static class VectorMath
	{
		// Token: 0x06000304 RID: 772 RVA: 0x0000D41F File Offset: 0x0000B61F
		public static Vector2 ComplexMultiply(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x * b.x - a.y * b.y, a.x * b.y + a.y * b.x);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000D45C File Offset: 0x0000B65C
		public static float2 ComplexMultiply(float2 a, float2 b)
		{
			return a.x * b + a.y * new float2(-b.y, b.x);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000D48C File Offset: 0x0000B68C
		public static float2 ComplexMultiplyConjugate(float2 a, float2 b)
		{
			return new float2(a.x * b.x + a.y * b.y, a.y * b.x - a.x * b.y);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000D4C9 File Offset: 0x0000B6C9
		public static Vector2 ComplexMultiplyConjugate(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x * b.x + a.y * b.y, a.y * b.x - a.x * b.y);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000D508 File Offset: 0x0000B708
		public static Vector3 ClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			Vector3 vector = Vector3.Normalize(lineEnd - lineStart);
			float num = Vector3.Dot(point - lineStart, vector);
			return lineStart + num * vector;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000D53D File Offset: 0x0000B73D
		public static float3 ClosestPointOnLine(float3 lineStart, float3 lineEnd, float3 point)
		{
			return lineStart + VectorMath.ClosestPointOnLineFactor(lineStart, lineEnd, point) * (lineEnd - lineStart);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000D55C File Offset: 0x0000B75C
		public static float ClosestPointOnLineFactor(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			Vector3 vector = lineEnd - lineStart;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude <= 1E-06f)
			{
				return 0f;
			}
			return Vector3.Dot(point - lineStart, vector) / sqrMagnitude;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000D598 File Offset: 0x0000B798
		public static float ClosestPointOnLineFactor(float3 lineStart, float3 lineEnd, float3 point)
		{
			float3 @float = lineEnd - lineStart;
			float num = math.dot(@float, @float);
			return math.select(0f, math.dot(point - lineStart, @float) / num, num > 1E-06f);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000D5D8 File Offset: 0x0000B7D8
		public static float ClosestPointOnLineFactor(Int3 lineStart, Int3 lineEnd, Int3 point)
		{
			Int3 @int = lineEnd - lineStart;
			float sqrMagnitude = @int.sqrMagnitude;
			float num = (float)Int3.DotLong(point - lineStart, @int);
			if (sqrMagnitude != 0f)
			{
				num /= sqrMagnitude;
			}
			return num;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000D614 File Offset: 0x0000B814
		public static float ClosestPointOnLineFactor(Vector2Int lineStart, Vector2Int lineEnd, Vector2Int point)
		{
			Vector2Int vector2Int = lineEnd - lineStart;
			double num = (double)((long)vector2Int.x * (long)vector2Int.x + (long)vector2Int.y * (long)vector2Int.y);
			Vector2Int vector2Int2 = point - lineStart;
			double num2 = (double)((long)vector2Int2.x * (long)vector2Int.x + (long)vector2Int2.y * (long)vector2Int.y);
			if (num != 0.0)
			{
				num2 /= num;
			}
			return (float)num2;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000D690 File Offset: 0x0000B890
		public static Vector3 ClosestPointOnSegment(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			Vector3 vector = lineEnd - lineStart;
			float sqrMagnitude = vector.sqrMagnitude;
			if ((double)sqrMagnitude <= 1E-06)
			{
				return lineStart;
			}
			float num = Vector3.Dot(point - lineStart, vector) / sqrMagnitude;
			return lineStart + Mathf.Clamp01(num) * vector;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000D6E0 File Offset: 0x0000B8E0
		public static Vector3 ClosestPointOnSegmentXZ(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			lineStart.y = point.y;
			lineEnd.y = point.y;
			Vector3 vector = lineEnd - lineStart;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			Vector3 vector2 = ((magnitude > float.Epsilon) ? (vector / magnitude) : Vector3.zero);
			float num = Vector3.Dot(point - lineStart, vector2);
			return lineStart + Mathf.Clamp(num, 0f, vector.magnitude) * vector2;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000D768 File Offset: 0x0000B968
		public static float SqrDistancePointSegmentApproximate(int x, int z, int px, int pz, int qx, int qz)
		{
			float num = (float)(qx - px);
			float num2 = (float)(qz - pz);
			float num3 = (float)(x - px);
			float num4 = (float)(z - pz);
			float num5 = num * num + num2 * num2;
			float num6 = num * num3 + num2 * num4;
			if (num5 > 0f)
			{
				num6 /= num5;
			}
			if (num6 < 0f)
			{
				num6 = 0f;
			}
			else if (num6 > 1f)
			{
				num6 = 1f;
			}
			num3 = (float)px + num6 * num - (float)x;
			num4 = (float)pz + num6 * num2 - (float)z;
			return num3 * num3 + num4 * num4;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000D7EC File Offset: 0x0000B9EC
		public static float SqrDistancePointSegmentApproximate(Int3 a, Int3 b, Int3 p)
		{
			float num = (float)(b.x - a.x);
			float num2 = (float)(b.z - a.z);
			float num3 = (float)(p.x - a.x);
			float num4 = (float)(p.z - a.z);
			float num5 = num * num + num2 * num2;
			float num6 = num * num3 + num2 * num4;
			if (num5 > 0f)
			{
				num6 /= num5;
			}
			if (num6 < 0f)
			{
				num6 = 0f;
			}
			else if (num6 > 1f)
			{
				num6 = 1f;
			}
			num3 = (float)a.x + num6 * num - (float)p.x;
			num4 = (float)a.z + num6 * num2 - (float)p.z;
			return num3 * num3 + num4 * num4;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000D8AC File Offset: 0x0000BAAC
		public static float SqrDistancePointSegment(Vector3 a, Vector3 b, Vector3 p)
		{
			return (VectorMath.ClosestPointOnSegment(a, b, p) - p).sqrMagnitude;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000D8D0 File Offset: 0x0000BAD0
		public static float SqrDistanceSegmentSegment(Vector3 s1, Vector3 e1, Vector3 s2, Vector3 e2)
		{
			Vector3 vector = e1 - s1;
			Vector3 vector2 = e2 - s2;
			Vector3 vector3 = s1 - s2;
			double num = (double)Vector3.Dot(vector, vector);
			double num2 = (double)Vector3.Dot(vector, vector2);
			double num3 = (double)Vector3.Dot(vector2, vector2);
			double num4 = (double)Vector3.Dot(vector, vector3);
			double num5 = (double)Vector3.Dot(vector2, vector3);
			double num7;
			double num6;
			double num8;
			double num9;
			if ((num6 = (num7 = num * num3 - num2 * num2)) < 1E-06 * num * num3)
			{
				num8 = 0.0;
				num7 = 1.0;
				num9 = num5;
				num6 = num3;
			}
			else
			{
				num8 = num2 * num5 - num3 * num4;
				num9 = num * num5 - num2 * num4;
				if (num8 < 0.0)
				{
					num8 = 0.0;
					num9 = num5;
					num6 = num3;
				}
				else if (num8 > num7)
				{
					num8 = num7;
					num9 = num5 + num2;
					num6 = num3;
				}
			}
			if (num9 < 0.0)
			{
				num9 = 0.0;
				if (-num4 < 0.0)
				{
					num8 = 0.0;
				}
				else if (-num4 > num)
				{
					num8 = num7;
				}
				else
				{
					num8 = -num4;
					num7 = num;
				}
			}
			else if (num9 > num6)
			{
				num9 = num6;
				if (-num4 + num2 < 0.0)
				{
					num8 = 0.0;
				}
				else if (-num4 + num2 > num)
				{
					num8 = num7;
				}
				else
				{
					num8 = -num4 + num2;
					num7 = num;
				}
			}
			double num10 = ((Math.Abs(num8) < 9.999999747378752E-06) ? 0.0 : (num8 / num7));
			double num11 = ((Math.Abs(num9) < 9.999999747378752E-06) ? 0.0 : (num9 / num6));
			return (vector3 + (float)num10 * vector - (float)num11 * vector2).sqrMagnitude;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000DAAF File Offset: 0x0000BCAF
		public static float Determinant(float2 c1, float2 c2)
		{
			return c1.x * c2.y - c1.y * c2.x;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000DACC File Offset: 0x0000BCCC
		public static float SqrDistanceXZ(Vector3 a, Vector3 b)
		{
			Vector3 vector = a - b;
			return vector.x * vector.x + vector.z * vector.z;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000DAFC File Offset: 0x0000BCFC
		public static long SignedTriangleAreaTimes2(int2 a, int2 b, int2 c)
		{
			return (long)(b.x - a.x) * (long)(c.y - a.y) - (long)(c.x - a.x) * (long)(b.y - a.y);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000DB39 File Offset: 0x0000BD39
		public static long SignedTriangleAreaTimes2XZ(Int3 a, Int3 b, Int3 c)
		{
			return (long)(b.x - a.x) * (long)(c.z - a.z) - (long)(c.x - a.x) * (long)(b.z - a.z);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000DB76 File Offset: 0x0000BD76
		public static float SignedTriangleAreaTimes2XZ(Vector3 a, Vector3 b, Vector3 c)
		{
			return (b.x - a.x) * (c.z - a.z) - (c.x - a.x) * (b.z - a.z);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000DBAF File Offset: 0x0000BDAF
		public static bool RightXZ(Vector3 a, Vector3 b, Vector3 p)
		{
			return (b.x - a.x) * (p.z - a.z) - (p.x - a.x) * (b.z - a.z) < -1E-45f;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000DBF0 File Offset: 0x0000BDF0
		public static bool RightXZ(Int3 a, Int3 b, Int3 p)
		{
			return (long)(b.x - a.x) * (long)(p.z - a.z) - (long)(p.x - a.x) * (long)(b.z - a.z) < 0L;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000DC3C File Offset: 0x0000BE3C
		public static bool Right(int2 a, int2 b, int2 p)
		{
			return (long)(b.x - a.x) * (long)(p.y - a.y) - (long)(p.x - a.x) * (long)(b.y - a.y) < 0L;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000DC88 File Offset: 0x0000BE88
		public static Side SideXZ(Int3 a, Int3 b, Int3 p)
		{
			long num = (long)(b.x - a.x) * (long)(p.z - a.z) - (long)(p.x - a.x) * (long)(b.z - a.z);
			if (num > 0L)
			{
				return Side.Left;
			}
			if (num >= 0L)
			{
				return Side.Colinear;
			}
			return Side.Right;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000DCE0 File Offset: 0x0000BEE0
		public static bool RightOrColinear(Vector2 a, Vector2 b, Vector2 p)
		{
			return (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y) <= 0f;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000DD30 File Offset: 0x0000BF30
		public static bool RightOrColinear(Vector2Int a, Vector2Int b, Vector2Int p)
		{
			return (long)(b.x - a.x) * (long)(p.y - a.y) - (long)(p.x - a.x) * (long)(b.y - a.y) <= 0L;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000DD88 File Offset: 0x0000BF88
		public static bool RightOrColinearXZ(Vector3 a, Vector3 b, Vector3 p)
		{
			return (b.x - a.x) * (p.z - a.z) - (p.x - a.x) * (b.z - a.z) <= 0f;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000DDD8 File Offset: 0x0000BFD8
		public static bool RightOrColinearXZ(Int3 a, Int3 b, Int3 p)
		{
			return (long)(b.x - a.x) * (long)(p.z - a.z) - (long)(p.x - a.x) * (long)(b.z - a.z) <= 0L;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000DE28 File Offset: 0x0000C028
		public static bool IsClockwiseMarginXZ(Vector3 a, Vector3 b, Vector3 c)
		{
			return (b.x - a.x) * (c.z - a.z) - (c.x - a.x) * (b.z - a.z) <= float.Epsilon;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000DE76 File Offset: 0x0000C076
		public static bool IsClockwiseXZ(Vector3 a, Vector3 b, Vector3 c)
		{
			return (b.x - a.x) * (c.z - a.z) - (c.x - a.x) * (b.z - a.z) < 0f;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000DEB6 File Offset: 0x0000C0B6
		public static bool IsClockwiseXZ(Int3 a, Int3 b, Int3 c)
		{
			return VectorMath.RightXZ(a, b, c);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000DEC0 File Offset: 0x0000C0C0
		public static bool IsClockwise(int2 a, int2 b, int2 c)
		{
			return VectorMath.Right(a, b, c);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000DECA File Offset: 0x0000C0CA
		public static bool IsClockwiseOrColinearXZ(Int3 a, Int3 b, Int3 c)
		{
			return VectorMath.RightOrColinearXZ(a, b, c);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000DED4 File Offset: 0x0000C0D4
		public static bool IsClockwiseOrColinear(Vector2Int a, Vector2Int b, Vector2Int c)
		{
			return VectorMath.RightOrColinear(a, b, c);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000DEE0 File Offset: 0x0000C0E0
		public static bool IsColinear(Vector3 a, Vector3 b, Vector3 c)
		{
			Vector3 vector = b - a;
			Vector3 vector2 = c - a;
			float num = vector.y * vector2.z - vector.z * vector2.y;
			float num2 = vector.z * vector2.x - vector.x * vector2.z;
			float num3 = vector.x * vector2.y - vector.y * vector2.x;
			float num4 = num * num + num2 * num2 + num3 * num3;
			float num5 = vector.sqrMagnitude * vector2.sqrMagnitude;
			return num4 <= math.sqrt(num5) * 0.0001f || num5 == 0f;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000DF88 File Offset: 0x0000C188
		public static bool IsColinear(Vector2 a, Vector2 b, Vector2 c)
		{
			float num = (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y);
			return num <= 0.0001f && num >= -0.0001f;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000DFE4 File Offset: 0x0000C1E4
		public static bool IsColinear(int2 a, int2 b, int2 c)
		{
			return (long)(b.x - a.x) * (long)(c.y - a.y) - (long)(c.x - a.x) * (long)(b.y - a.y) == 0L;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000E030 File Offset: 0x0000C230
		public static bool IsColinearXZ(Int3 a, Int3 b, Int3 c)
		{
			return (long)(b.x - a.x) * (long)(c.z - a.z) - (long)(c.x - a.x) * (long)(b.z - a.z) == 0L;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000E07C File Offset: 0x0000C27C
		public static bool IsColinearXZ(Vector3 a, Vector3 b, Vector3 c)
		{
			float num = (b.x - a.x) * (c.z - a.z) - (c.x - a.x) * (b.z - a.z);
			return num <= 1E-07f && num >= -1E-07f;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000E0D8 File Offset: 0x0000C2D8
		public static bool IsColinearAlmostXZ(Int3 a, Int3 b, Int3 c)
		{
			long num = (long)(b.x - a.x) * (long)(c.z - a.z) - (long)(c.x - a.x) * (long)(b.z - a.z);
			return num > -1L && num < 1L;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000E12D File Offset: 0x0000C32D
		public static bool SegmentsIntersect(Vector2Int start1, Vector2Int end1, Vector2Int start2, Vector2Int end2)
		{
			return VectorMath.RightOrColinear(start1, end1, start2) != VectorMath.RightOrColinear(start1, end1, end2) && VectorMath.RightOrColinear(start2, end2, start1) != VectorMath.RightOrColinear(start2, end2, end1);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000E158 File Offset: 0x0000C358
		public static bool SegmentsIntersectXZ(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
		{
			return VectorMath.RightOrColinearXZ(start1, end1, start2) != VectorMath.RightOrColinearXZ(start1, end1, end2) && VectorMath.RightOrColinearXZ(start2, end2, start1) != VectorMath.RightOrColinearXZ(start2, end2, end1);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000E184 File Offset: 0x0000C384
		public static bool SegmentsIntersectXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
		{
			Vector3 vector = end1 - start1;
			Vector3 vector2 = end2 - start2;
			float num = vector2.z * vector.x - vector2.x * vector.z;
			if (num == 0f)
			{
				return false;
			}
			float num2 = vector2.x * (start1.z - start2.z) - vector2.z * (start1.x - start2.x);
			float num3 = vector.x * (start1.z - start2.z) - vector.z * (start1.x - start2.x);
			float num4 = num2 / num;
			float num5 = num3 / num;
			return num4 >= 0f && num4 <= 1f && num5 >= 0f && num5 <= 1f;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000E24C File Offset: 0x0000C44C
		public static float2 CapsuleLineIntersectionFactors(float2 capsuleStart, float2 capsuleDir, float capsuleLength, float2 lineStart, float2 lineDir, float radius)
		{
			float num = math.dot(capsuleDir, lineDir);
			float num2 = math.sqrt(1f - num * num);
			float num3 = float.PositiveInfinity;
			float num4 = float.NegativeInfinity;
			float num5;
			float num6;
			if (VectorMath.LineCircleIntersectionFactors(lineStart - capsuleStart, lineDir, radius, out num5, out num6))
			{
				num3 = math.min(num3, num5);
				num4 = math.max(num4, num6);
			}
			float num7;
			float num8;
			if (VectorMath.LineCircleIntersectionFactors(lineStart - (capsuleStart + capsuleDir * capsuleLength), lineDir, radius, out num7, out num8))
			{
				num3 = math.min(num3, num7);
				num4 = math.max(num4, num8);
			}
			float num9;
			if (VectorMath.LineLineIntersectionFactor(capsuleStart, capsuleDir, lineStart, lineDir, out num9))
			{
				float2 @float = new float2(-capsuleDir.y, capsuleDir.x);
				float num10 = radius * num / num2;
				float num11 = math.sign(capsuleDir.y * lineDir.x - capsuleDir.x * lineDir.y);
				float num12 = num9 + num10 * num11;
				float num13 = num9 - num10 * num11;
				if (num12 >= 0f && num12 <= capsuleLength)
				{
					float num14 = math.dot(capsuleStart + capsuleDir * num12 - @float * radius - lineStart, lineDir);
					num3 = math.min(num3, num14);
					num4 = math.max(num4, num14);
				}
				if (num13 >= 0f && num13 <= capsuleLength)
				{
					float num15 = math.dot(capsuleStart + capsuleDir * num13 + @float * radius - lineStart, lineDir);
					num3 = math.min(num3, num15);
					num4 = math.max(num4, num15);
				}
			}
			return new float2(num3, num4);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000E3DC File Offset: 0x0000C5DC
		public static bool LineLineIntersectionFactor(float2 start1, float2 dir1, float2 start2, float2 dir2, out float t)
		{
			float num = dir2.y * dir1.x - dir2.x * dir1.y;
			if (math.abs(num) < 0.0001f)
			{
				t = 0f;
				return false;
			}
			float num2 = dir2.x * (start1.y - start2.y) - dir2.y * (start1.x - start2.x);
			t = num2 / num;
			return true;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000E450 File Offset: 0x0000C650
		public static bool LineLineIntersectionFactors(float2 start1, float2 dir1, float2 start2, float2 dir2, out float factor1, out float factor2)
		{
			float num = dir2.y * dir1.x - dir2.x * dir1.y;
			if (math.abs(num) < 0.0001f)
			{
				factor1 = (factor2 = 0f);
				return false;
			}
			float num2 = dir2.x * (start1.y - start2.y) - dir2.y * (start1.x - start2.x);
			float num3 = dir1.x * (start1.y - start2.y) - dir1.y * (start1.x - start2.x);
			factor1 = num2 / num;
			factor2 = num3 / num;
			return true;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000E4F8 File Offset: 0x0000C6F8
		public static Vector3 LineDirIntersectionPointXZ(Vector3 start1, Vector3 dir1, Vector3 start2, Vector3 dir2)
		{
			float num = dir2.z * dir1.x - dir2.x * dir1.z;
			if (num == 0f)
			{
				return start1;
			}
			float num2 = (dir2.x * (start1.z - start2.z) - dir2.z * (start1.x - start2.x)) / num;
			return start1 + dir1 * num2;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000E564 File Offset: 0x0000C764
		public static Vector3 LineDirIntersectionPointXZ(Vector3 start1, Vector3 dir1, Vector3 start2, Vector3 dir2, out bool intersects)
		{
			float num = dir2.z * dir1.x - dir2.x * dir1.z;
			if (num == 0f)
			{
				intersects = false;
				return start1;
			}
			float num2 = (dir2.x * (start1.z - start2.z) - dir2.z * (start1.x - start2.x)) / num;
			intersects = true;
			return start1 + dir1 * num2;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000E5D8 File Offset: 0x0000C7D8
		public static bool RaySegmentIntersectXZ(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
		{
			Int3 @int = end1 - start1;
			Int3 int2 = end2 - start2;
			long num = (long)(int2.z * @int.x - int2.x * @int.z);
			if (num == 0L)
			{
				return false;
			}
			long num2 = (long)(int2.x * (start1.z - start2.z) - int2.z * (start1.x - start2.x));
			long num3 = (long)(@int.x * (start1.z - start2.z) - @int.z * (start1.x - start2.x));
			return ((num2 < 0L) ^ (num < 0L)) && ((num3 < 0L) ^ (num < 0L)) && (num < 0L || num3 <= num) && (num >= 0L || num3 > num);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000E6A0 File Offset: 0x0000C8A0
		public static bool LineIntersectionFactorXZ(Int3 start1, Int3 end1, Int3 start2, Int3 end2, out float factor1, out float factor2)
		{
			Int3 @int = end1 - start1;
			Int3 int2 = end2 - start2;
			long num = (long)(int2.z * @int.x - int2.x * @int.z);
			if (num == 0L)
			{
				factor1 = 0f;
				factor2 = 0f;
				return false;
			}
			long num2 = (long)(int2.x * (start1.z - start2.z) - int2.z * (start1.x - start2.x));
			long num3 = (long)(@int.x * (start1.z - start2.z) - @int.z * (start1.x - start2.x));
			factor1 = (float)num2 / (float)num;
			factor2 = (float)num3 / (float)num;
			return true;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000E758 File Offset: 0x0000C958
		public static bool LineIntersectionFactorXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out float factor1, out float factor2)
		{
			Vector3 vector = end1 - start1;
			Vector3 vector2 = end2 - start2;
			float num = vector2.z * vector.x - vector2.x * vector.z;
			if (num <= 1E-05f && num >= -1E-05f)
			{
				factor1 = 0f;
				factor2 = 0f;
				return false;
			}
			float num2 = vector2.x * (start1.z - start2.z) - vector2.z * (start1.x - start2.x);
			float num3 = vector.x * (start1.z - start2.z) - vector.z * (start1.x - start2.x);
			float num4 = num2 / num;
			float num5 = num3 / num;
			factor1 = num4;
			factor2 = num5;
			return true;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000E81C File Offset: 0x0000CA1C
		public static float LineRayIntersectionFactorXZ(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
		{
			Int3 @int = end1 - start1;
			Int3 int2 = end2 - start2;
			int num = int2.z * @int.x - int2.x * @int.z;
			if (num == 0)
			{
				return float.NaN;
			}
			int num2 = int2.x * (start1.z - start2.z) - int2.z * (start1.x - start2.x);
			if ((float)(@int.x * (start1.z - start2.z) - @int.z * (start1.x - start2.x)) / (float)num < 0f)
			{
				return float.NaN;
			}
			return (float)num2 / (float)num;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000E8C8 File Offset: 0x0000CAC8
		public static float LineIntersectionFactorXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
		{
			Vector3 vector = end1 - start1;
			Vector3 vector2 = end2 - start2;
			float num = vector2.z * vector.x - vector2.x * vector.z;
			if (num == 0f)
			{
				return -1f;
			}
			return (vector2.x * (start1.z - start2.z) - vector2.z * (start1.x - start2.x)) / num;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000E93C File Offset: 0x0000CB3C
		public static Vector3 LineIntersectionPointXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
		{
			bool flag;
			return VectorMath.LineIntersectionPointXZ(start1, end1, start2, end2, out flag);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000E954 File Offset: 0x0000CB54
		public static Vector3 LineIntersectionPointXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out bool intersects)
		{
			Vector3 vector = end1 - start1;
			Vector3 vector2 = end2 - start2;
			float num = vector2.z * vector.x - vector2.x * vector.z;
			if (num == 0f)
			{
				intersects = false;
				return start1;
			}
			float num2 = (vector2.x * (start1.z - start2.z) - vector2.z * (start1.x - start2.x)) / num;
			intersects = true;
			return start1 + vector * num2;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000E9D8 File Offset: 0x0000CBD8
		public static Vector2 LineIntersectionPoint(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
		{
			bool flag;
			return VectorMath.LineIntersectionPoint(start1, end1, start2, end2, out flag);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000E9F0 File Offset: 0x0000CBF0
		public static Vector2 LineIntersectionPoint(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out bool intersects)
		{
			Vector2 vector = end1 - start1;
			Vector2 vector2 = end2 - start2;
			float num = vector2.y * vector.x - vector2.x * vector.y;
			if (num == 0f)
			{
				intersects = false;
				return start1;
			}
			float num2 = (vector2.x * (start1.y - start2.y) - vector2.y * (start1.x - start2.x)) / num;
			intersects = true;
			return start1 + vector * num2;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000EA74 File Offset: 0x0000CC74
		public static Vector3 SegmentIntersectionPointXZ(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out bool intersects)
		{
			Vector3 vector = end1 - start1;
			Vector3 vector2 = end2 - start2;
			float num = vector2.z * vector.x - vector2.x * vector.z;
			if (num == 0f)
			{
				intersects = false;
				return start1;
			}
			float num2 = vector2.x * (start1.z - start2.z) - vector2.z * (start1.x - start2.x);
			float num3 = vector.x * (start1.z - start2.z) - vector.z * (start1.x - start2.x);
			float num4 = num2 / num;
			float num5 = num3 / num;
			if (num4 < 0f || num4 > 1f || num5 < 0f || num5 > 1f)
			{
				intersects = false;
				return start1;
			}
			intersects = true;
			return start1 + vector * num4;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000EB54 File Offset: 0x0000CD54
		public static bool SegmentIntersectsBounds(Bounds bounds, Vector3 a, Vector3 b)
		{
			a -= bounds.center;
			b -= bounds.center;
			Vector3 vector = (a + b) * 0.5f;
			Vector3 vector2 = a - vector;
			Vector3 vector3 = new Vector3(Math.Abs(vector2.x), Math.Abs(vector2.y), Math.Abs(vector2.z));
			Vector3 extents = bounds.extents;
			return Math.Abs(vector.x) <= extents.x + vector3.x && Math.Abs(vector.y) <= extents.y + vector3.y && Math.Abs(vector.z) <= extents.z + vector3.z && Math.Abs(vector.y * vector2.z - vector.z * vector2.y) <= extents.y * vector3.z + extents.z * vector3.y && Math.Abs(vector.x * vector2.z - vector.z * vector2.x) <= extents.x * vector3.z + extents.z * vector3.x && Math.Abs(vector.x * vector2.y - vector.y * vector2.x) <= extents.x * vector3.y + extents.y * vector3.x;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000ECDC File Offset: 0x0000CEDC
		public static bool LineCircleIntersectionFactors(float2 point, float2 direction, float radius, out float t1, out float t2)
		{
			float num = math.dot(point, direction);
			float num2 = math.lengthsq(point) - num * num;
			float num3 = radius * radius - num2;
			if (num3 < 0f)
			{
				t1 = float.PositiveInfinity;
				t2 = float.NegativeInfinity;
				return false;
			}
			float num4 = math.sqrt(num3);
			t1 = -num - num4;
			t2 = -num + num4;
			return true;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000ED30 File Offset: 0x0000CF30
		public static bool SegmentCircleIntersectionFactors(float2 point1, float2 point2, float radiusSq, out float t1, out float t2)
		{
			float2 @float = point2 - point1;
			float num = math.lengthsq(@float);
			float num2 = math.dot(point1, @float) / num;
			float num3 = math.lengthsq(point1) / num - num2 * num2;
			float num4 = radiusSq / num - num3;
			if (num4 < 0f)
			{
				t1 = float.PositiveInfinity;
				t2 = float.NegativeInfinity;
				return false;
			}
			float num5 = math.sqrt(num4);
			t1 = -num2 - num5;
			t2 = -num2 + num5;
			t1 = math.max(0f, t1);
			t2 = math.min(1f, t2);
			return t1 < 1f && t2 > 0f;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000EDD0 File Offset: 0x0000CFD0
		public static float LineCircleIntersectionFactor(Vector3 circleCenter, Vector3 linePoint1, Vector3 linePoint2, float radius)
		{
			float num;
			Vector3 vector = VectorMath.Normalize(linePoint2 - linePoint1, out num);
			Vector3 vector2 = linePoint1 - circleCenter;
			float num2 = Vector3.Dot(vector2, vector);
			float num3 = num2 * num2 - (vector2.sqrMagnitude - radius * radius);
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			float num4 = -num2 + Mathf.Sqrt(num3);
			if (num <= 1E-05f)
			{
				return 1f;
			}
			return num4 / num;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000EE38 File Offset: 0x0000D038
		public static bool ReversesFaceOrientations(Matrix4x4 matrix)
		{
			Vector3 vector = matrix.MultiplyVector(new Vector3(1f, 0f, 0f));
			Vector3 vector2 = matrix.MultiplyVector(new Vector3(0f, 1f, 0f));
			Vector3 vector3 = matrix.MultiplyVector(new Vector3(0f, 0f, 1f));
			return Vector3.Dot(Vector3.Cross(vector, vector2), vector3) < 0f;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000EEAB File Offset: 0x0000D0AB
		public static Vector3 Normalize(Vector3 v, out float magnitude)
		{
			magnitude = v.magnitude;
			if (magnitude > 1E-05f)
			{
				return v / magnitude;
			}
			return Vector3.zero;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000EECD File Offset: 0x0000D0CD
		public static Vector2 Normalize(Vector2 v, out float magnitude)
		{
			magnitude = v.magnitude;
			if (magnitude > 1E-05f)
			{
				return v / magnitude;
			}
			return Vector2.zero;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000EEF0 File Offset: 0x0000D0F0
		public static Vector3 ClampMagnitudeXZ(Vector3 v, float maxMagnitude)
		{
			float num = v.x * v.x + v.z * v.z;
			if (num > maxMagnitude * maxMagnitude && maxMagnitude > 0f)
			{
				float num2 = maxMagnitude / Mathf.Sqrt(num);
				v.x *= num2;
				v.z *= num2;
			}
			return v;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000EF49 File Offset: 0x0000D149
		public static float MagnitudeXZ(Vector3 v)
		{
			return Mathf.Sqrt(v.x * v.x + v.z * v.z);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000EF6B File Offset: 0x0000D16B
		public static float QuaternionAngle(quaternion rot)
		{
			return 2f * math.atan2(math.length(rot.value.xyz), rot.value.w);
		}
	}
}
