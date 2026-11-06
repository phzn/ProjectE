using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000052 RID: 82
	[BurstCompile]
	public static class Polygon
	{
		// Token: 0x06000352 RID: 850 RVA: 0x0000F1D3 File Offset: 0x0000D3D3
		public static bool ContainsPointXZ(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
		{
			return VectorMath.IsClockwiseMarginXZ(a, b, p) && VectorMath.IsClockwiseMarginXZ(b, c, p) && VectorMath.IsClockwiseMarginXZ(c, a, p);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000F1F3 File Offset: 0x0000D3F3
		public static bool ContainsPointXZ(Int3 a, Int3 b, Int3 c, Int3 p)
		{
			return VectorMath.IsClockwiseOrColinearXZ(a, b, p) && VectorMath.IsClockwiseOrColinearXZ(b, c, p) && VectorMath.IsClockwiseOrColinearXZ(c, a, p);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000F213 File Offset: 0x0000D413
		public static bool ContainsPoint(Vector2Int a, Vector2Int b, Vector2Int c, Vector2Int p)
		{
			return VectorMath.IsClockwiseOrColinear(a, b, p) && VectorMath.IsClockwiseOrColinear(b, c, p) && VectorMath.IsClockwiseOrColinear(c, a, p);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000F234 File Offset: 0x0000D434
		public static bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
		{
			int num = polyPoints.Length - 1;
			bool flag = false;
			int i = 0;
			while (i < polyPoints.Length)
			{
				if (((polyPoints[i].y <= p.y && p.y < polyPoints[num].y) || (polyPoints[num].y <= p.y && p.y < polyPoints[i].y)) && p.x < (polyPoints[num].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[num].y - polyPoints[i].y) + polyPoints[i].x)
				{
					flag = !flag;
				}
				num = i++;
			}
			return flag;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000F314 File Offset: 0x0000D514
		public static bool ContainsPointXZ(Vector3[] polyPoints, Vector3 p)
		{
			int num = polyPoints.Length - 1;
			bool flag = false;
			int i = 0;
			while (i < polyPoints.Length)
			{
				if (((polyPoints[i].z <= p.z && p.z < polyPoints[num].z) || (polyPoints[num].z <= p.z && p.z < polyPoints[i].z)) && p.x < (polyPoints[num].x - polyPoints[i].x) * (p.z - polyPoints[i].z) / (polyPoints[num].z - polyPoints[i].z) + polyPoints[i].x)
				{
					flag = !flag;
				}
				num = i++;
			}
			return flag;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000F3F1 File Offset: 0x0000D5F1
		[BurstCompile]
		public static bool ContainsPoint(ref int3 aWorld, ref int3 bWorld, ref int3 cWorld, ref int3 pWorld, ref NativeMovementPlane movementPlane)
		{
			return Polygon.ContainsPoint_00000357$BurstDirectCall.Invoke(ref aWorld, ref bWorld, ref cWorld, ref pWorld, ref movementPlane);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000F400 File Offset: 0x0000D600
		public static bool ContainsPoint(ref int3 aWorld, ref int3 bWorld, ref int3 cWorld, ref int3 pWorld, in float2x3 planeProjection)
		{
			int2x3 int2x = new int2x3(planeProjection * 1024f);
			int4 @int = new int4(aWorld.x, bWorld.x, cWorld.x, pWorld.x);
			int4 int2 = new int4(aWorld.y, bWorld.y, cWorld.y, pWorld.y);
			int4 int3 = new int4(aWorld.z, bWorld.z, cWorld.z, pWorld.z);
			int4 int4 = @int - @int.x;
			int2 -= int2.x;
			int3 -= int3.x;
			int4 int5 = (int4 * int2x.c0.x + int2 * int2x.c1.x + int3 * int2x.c2.x) / 1024;
			int4 int6 = (int4 * int2x.c0.y + int2 * int2x.c1.y + int3 * int2x.c2.y) / 1024;
			int3 int7 = int5.yzx - int5.xyz;
			int3 int8 = int6.www - int6.xyz;
			int3 int9 = int5.www - int5.xyz;
			int3 int10 = int6.yzx - int6.xyz;
			long num = (long)int7.x * (long)int8.x - (long)int9.x * (long)int10.x;
			long num2 = (long)int7.y * (long)int8.y - (long)int9.y * (long)int10.y;
			long num3 = (long)int7.z * (long)int8.z - (long)int9.z * (long)int10.z;
			return ((num >= 0L) & (num2 >= 0L) & (num3 >= 0L)) | ((num <= 0L) & (num2 <= 0L) & (num3 <= 0L));
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000F630 File Offset: 0x0000D830
		public static Vector3[] ConvexHullXZ(Vector3[] points)
		{
			if (points.Length == 0)
			{
				return new Vector3[0];
			}
			List<Vector3> list = ListPool<Vector3>.Claim();
			int num = 0;
			for (int i = 1; i < points.Length; i++)
			{
				if (points[i].x < points[num].x)
				{
					num = i;
				}
			}
			int num2 = num;
			int num3 = 0;
			for (;;)
			{
				list.Add(points[num]);
				int num4 = 0;
				for (int j = 0; j < points.Length; j++)
				{
					if (num4 == num || !VectorMath.RightOrColinearXZ(points[num], points[num4], points[j]))
					{
						num4 = j;
					}
				}
				num = num4;
				num3++;
				if (num3 > 10000)
				{
					break;
				}
				if (num == num2)
				{
					goto IL_00AF;
				}
			}
			Debug.LogWarning("Infinite Loop in Convex Hull Calculation");
			IL_00AF:
			Vector3[] array = list.ToArray();
			ListPool<Vector3>.Release(list);
			return array;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000F6FC File Offset: 0x0000D8FC
		public static Vector2 ClosestPointOnTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
		{
			Vector2 vector = b - a;
			Vector2 vector2 = c - a;
			Vector2 vector3 = p - a;
			float num = Vector2.Dot(vector, vector3);
			float num2 = Vector2.Dot(vector2, vector3);
			if (num <= 0f && num2 <= 0f)
			{
				return a;
			}
			Vector2 vector4 = p - b;
			float num3 = Vector2.Dot(vector, vector4);
			float num4 = Vector2.Dot(vector2, vector4);
			if (num3 >= 0f && num4 <= num3)
			{
				return b;
			}
			if (num >= 0f && num3 <= 0f && num * num4 - num3 * num2 <= 0f)
			{
				float num5 = num / (num - num3);
				return a + vector * num5;
			}
			Vector2 vector5 = p - c;
			float num6 = Vector2.Dot(vector, vector5);
			float num7 = Vector2.Dot(vector2, vector5);
			if (num7 >= 0f && num6 <= num7)
			{
				return c;
			}
			if (num2 >= 0f && num7 <= 0f && num6 * num2 - num * num7 <= 0f)
			{
				float num8 = num2 / (num2 - num7);
				return a + vector2 * num8;
			}
			if (num4 - num3 >= 0f && num6 - num7 >= 0f && num3 * num7 - num6 * num4 <= 0f)
			{
				float num9 = (num4 - num3) / (num4 - num3 + (num6 - num7));
				return b + (c - b) * num9;
			}
			return p;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000F86C File Offset: 0x0000DA6C
		public static Vector3 ClosestPointOnTriangleXZ(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
		{
			Vector2 vector = new Vector2(b.x - a.x, b.z - a.z);
			Vector2 vector2 = new Vector2(c.x - a.x, c.z - a.z);
			Vector2 vector3 = new Vector2(p.x - a.x, p.z - a.z);
			float num = Vector2.Dot(vector, vector3);
			float num2 = Vector2.Dot(vector2, vector3);
			if (num <= 0f && num2 <= 0f)
			{
				return a;
			}
			Vector2 vector4 = new Vector2(p.x - b.x, p.z - b.z);
			float num3 = Vector2.Dot(vector, vector4);
			float num4 = Vector2.Dot(vector2, vector4);
			if (num3 >= 0f && num4 <= num3)
			{
				return b;
			}
			float num5 = num * num4 - num3 * num2;
			if (num >= 0f && num3 <= 0f && num5 <= 0f)
			{
				float num6 = num / (num - num3);
				return (1f - num6) * a + num6 * b;
			}
			Vector2 vector5 = new Vector2(p.x - c.x, p.z - c.z);
			float num7 = Vector2.Dot(vector, vector5);
			float num8 = Vector2.Dot(vector2, vector5);
			if (num8 >= 0f && num7 <= num8)
			{
				return c;
			}
			float num9 = num7 * num2 - num * num8;
			if (num2 >= 0f && num8 <= 0f && num9 <= 0f)
			{
				float num10 = num2 / (num2 - num8);
				return (1f - num10) * a + num10 * c;
			}
			float num11 = num3 * num8 - num7 * num4;
			if (num4 - num3 >= 0f && num7 - num8 >= 0f && num11 <= 0f)
			{
				float num12 = (num4 - num3) / (num4 - num3 + (num7 - num8));
				return b + (c - b) * num12;
			}
			float num13 = 1f / (num11 + num9 + num5);
			float num14 = num9 * num13;
			float num15 = num5 * num13;
			return new Vector3(p.x, (1f - num14 - num15) * a.y + num14 * b.y + num15 * c.y, p.z);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000FAD0 File Offset: 0x0000DCD0
		public static float3 ClosestPointOnTriangle(float3 a, float3 b, float3 c, float3 p)
		{
			float3 @float;
			Polygon.ClosestPointOnTriangleByRef(in a, in b, in c, in p, out @float);
			return @float;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000FAEE File Offset: 0x0000DCEE
		[BurstCompile]
		public static bool ClosestPointOnTriangleByRef(in float3 a, in float3 b, in float3 c, in float3 p, [NoAlias] out float3 output)
		{
			return Polygon.ClosestPointOnTriangleByRef_0000035D$BurstDirectCall.Invoke(in a, in b, in c, in p, out output);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000FAFC File Offset: 0x0000DCFC
		public static float3 ClosestPointOnTriangleBarycentric(float2 a, float2 b, float2 c, float2 p)
		{
			float2 @float = b - a;
			float2 float2 = c - a;
			float2 float3 = p - a;
			float num = math.dot(@float, float3);
			float num2 = math.dot(float2, float3);
			if (num <= 0f && num2 <= 0f)
			{
				return new float3(1f, 0f, 0f);
			}
			float2 float4 = p - b;
			float num3 = math.dot(@float, float4);
			float num4 = math.dot(float2, float4);
			if (num3 >= 0f && num4 <= num3)
			{
				return new float3(0f, 1f, 0f);
			}
			float num5 = num * num4 - num3 * num2;
			if (num >= 0f && num3 <= 0f && num5 <= 0f)
			{
				float num6 = num / (num - num3);
				return new float3(1f - num6, num6, 0f);
			}
			float2 float5 = p - c;
			float num7 = math.dot(@float, float5);
			float num8 = math.dot(float2, float5);
			if (num8 >= 0f && num7 <= num8)
			{
				return new float3(0f, 0f, 1f);
			}
			float num9 = num7 * num2 - num * num8;
			if (num2 >= 0f && num8 <= 0f && num9 <= 0f)
			{
				float num10 = num2 / (num2 - num8);
				return new float3(1f - num10, 0f, num10);
			}
			float num11 = num3 * num8 - num7 * num4;
			if (num4 - num3 >= 0f && num7 - num8 >= 0f && num11 <= 0f)
			{
				float num12 = (num4 - num3) / (num4 - num3 + (num7 - num8));
				return new float3(0f, 1f - num12, num12);
			}
			float num13 = 1f / (num11 + num9 + num5);
			float num14 = num9 * num13;
			float num15 = num5 * num13;
			return new float3(1f - num14 - num15, num14, num15);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000FCEC File Offset: 0x0000DEEC
		public static float3 ClosestPointOnTriangleBarycentric(float3 a, float3 b, float3 c, float3 p)
		{
			float3 @float = b - a;
			float3 float2 = c - a;
			float3 float3 = p - a;
			float num = math.dot(@float, float3);
			float num2 = math.dot(float2, float3);
			if (num <= 0f && num2 <= 0f)
			{
				return new float3(1f, 0f, 0f);
			}
			float3 float4 = p - b;
			float num3 = math.dot(@float, float4);
			float num4 = math.dot(float2, float4);
			if (num3 >= 0f && num4 <= num3)
			{
				return new float3(0f, 1f, 0f);
			}
			float num5 = num * num4 - num3 * num2;
			if (num >= 0f && num3 <= 0f && num5 <= 0f)
			{
				float num6 = num / (num - num3);
				return new float3(1f - num6, num6, 0f);
			}
			float3 float5 = p - c;
			float num7 = math.dot(@float, float5);
			float num8 = math.dot(float2, float5);
			if (num8 >= 0f && num7 <= num8)
			{
				return new float3(0f, 0f, 1f);
			}
			float num9 = num7 * num2 - num * num8;
			if (num2 >= 0f && num8 <= 0f && num9 <= 0f)
			{
				float num10 = num2 / (num2 - num8);
				return new float3(1f - num10, 0f, num10);
			}
			float num11 = num3 * num8 - num7 * num4;
			if (num4 - num3 >= 0f && num7 - num8 >= 0f && num11 <= 0f)
			{
				float num12 = (num4 - num3) / (num4 - num3 + (num7 - num8));
				return new float3(0f, 1f - num12, num12);
			}
			float num13 = 1f / (num11 + num9 + num5);
			float num14 = num9 * num13;
			float num15 = num5 * num13;
			return new float3(1f - num14 - num15, num14, num15);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000FEDB File Offset: 0x0000E0DB
		[BurstCompile]
		public static void ClosestPointOnTriangleProjected(ref Int3 vi1, ref Int3 vi2, ref Int3 vi3, ref BBTree.ProjectionParams projection, ref float3 point, [NoAlias] out float3 closest, [NoAlias] out float sqrDist, [NoAlias] out float distAlongProjection)
		{
			Polygon.ClosestPointOnTriangleProjected_00000360$BurstDirectCall.Invoke(ref vi1, ref vi2, ref vi3, ref projection, ref point, out closest, out sqrDist, out distAlongProjection);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000FEF0 File Offset: 0x0000E0F0
		public static void CompressMesh(List<Int3> vertices, List<int> triangles, List<uint> tags, out Int3[] outVertices, out int[] outTriangles, out uint[] outTags)
		{
			Dictionary<Int3, int> dictionary = Polygon.cached_Int3_int_dict;
			dictionary.Clear();
			int[] array = ArrayPool<int>.Claim(vertices.Count);
			int num = 0;
			for (int i = 0; i < vertices.Count; i++)
			{
				int num2;
				if (!dictionary.TryGetValue(vertices[i], out num2) && !dictionary.TryGetValue(vertices[i] + new Int3(0, 1, 0), out num2) && !dictionary.TryGetValue(vertices[i] + new Int3(0, -1, 0), out num2))
				{
					dictionary.Add(vertices[i], num);
					array[i] = num;
					vertices[num] = vertices[i];
					num++;
				}
				else
				{
					array[i] = num2;
				}
			}
			outTriangles = new int[triangles.Count];
			for (int j = 0; j < outTriangles.Length; j++)
			{
				outTriangles[j] = array[triangles[j]];
			}
			outVertices = new Int3[num];
			for (int k = 0; k < num; k++)
			{
				outVertices[k] = vertices[k];
			}
			ArrayPool<int>.Release(ref array, false);
			outTags = tags.ToArray();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00010010 File Offset: 0x0000E210
		public static void TraceContours(Dictionary<int, int> outline, HashSet<int> hasInEdge, Action<List<int>, bool> results)
		{
			List<int> list = ListPool<int>.Claim();
			List<int> list2 = ListPool<int>.Claim();
			list2.AddRange(outline.Keys);
			for (int i = 0; i <= 1; i++)
			{
				bool flag = i == 1;
				for (int j = 0; j < list2.Count; j++)
				{
					int num = list2[j];
					if (flag || !hasInEdge.Contains(num))
					{
						int num2 = num;
						list.Clear();
						list.Add(num2);
						while (outline.ContainsKey(num2))
						{
							int num3 = outline[num2];
							outline.Remove(num2);
							list.Add(num3);
							if (num3 == num)
							{
								break;
							}
							num2 = num3;
						}
						if (list.Count > 1)
						{
							results(list, flag);
						}
					}
				}
			}
			ListPool<int>.Release(ref list2);
			ListPool<int>.Release(ref list);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000100DC File Offset: 0x0000E2DC
		public static void Subdivide(List<Vector3> points, List<Vector3> result, int subSegments)
		{
			for (int i = 0; i < points.Count - 1; i++)
			{
				for (int j = 0; j < subSegments; j++)
				{
					result.Add(Vector3.Lerp(points[i], points[i + 1], (float)j / (float)subSegments));
				}
			}
			result.Add(points[points.Count - 1]);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00010148 File Offset: 0x0000E348
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool ContainsPoint$BurstManaged(ref int3 aWorld, ref int3 bWorld, ref int3 cWorld, ref int3 pWorld, ref NativeMovementPlane movementPlane)
		{
			float3x3 float3x = new float3x3(movementPlane.rotation.value);
			float2x3 float2x = math.transpose(new float3x2(float3x.c0, float3x.c2));
			return Polygon.ContainsPoint(ref aWorld, ref bWorld, ref cWorld, ref pWorld, in float2x);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00010190 File Offset: 0x0000E390
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool ClosestPointOnTriangleByRef$BurstManaged(in float3 a, in float3 b, in float3 c, in float3 p, [NoAlias] out float3 output)
		{
			float3 @float = b - a;
			float3 float2 = c - a;
			float3 float3 = p - a;
			float num = math.dot(@float, float3);
			float num2 = math.dot(float2, float3);
			if (num <= 0f && num2 <= 0f)
			{
				output = a;
				return false;
			}
			float3 float4 = p - b;
			float num3 = math.dot(@float, float4);
			float num4 = math.dot(float2, float4);
			if (num3 >= 0f && num4 <= num3)
			{
				output = b;
				return false;
			}
			float num5 = num * num4 - num3 * num2;
			if (num >= 0f && num3 <= 0f && num5 <= 0f)
			{
				float num6 = num / (num - num3);
				output = a + @float * num6;
				return false;
			}
			float3 float5 = p - c;
			float num7 = math.dot(@float, float5);
			float num8 = math.dot(float2, float5);
			if (num8 >= 0f && num7 <= num8)
			{
				output = c;
				return false;
			}
			float num9 = num7 * num2 - num * num8;
			if (num2 >= 0f && num8 <= 0f && num9 <= 0f)
			{
				float num10 = num2 / (num2 - num8);
				output = a + float2 * num10;
				return false;
			}
			float num11 = num3 * num8 - num7 * num4;
			if (num4 - num3 >= 0f && num7 - num8 >= 0f && num11 <= 0f)
			{
				float num12 = (num4 - num3) / (num4 - num3 + (num7 - num8));
				output = b + (c - b) * num12;
				return false;
			}
			float num13 = 1f / (num11 + num9 + num5);
			float num14 = num9 * num13;
			float num15 = num5 * num13;
			output = a + @float * num14 + float2 * num15;
			return true;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x000103D8 File Offset: 0x0000E5D8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ClosestPointOnTriangleProjected$BurstManaged(ref Int3 vi1, ref Int3 vi2, ref Int3 vi3, ref BBTree.ProjectionParams projection, ref float3 point, [NoAlias] out float3 closest, [NoAlias] out float sqrDist, [NoAlias] out float distAlongProjection)
		{
			float3 @float = (float3)vi1;
			float3 float2 = (float3)vi2;
			float3 float3 = (float3)vi3;
			float2 float4 = math.mul(projection.planeProjection, @float);
			float2 float5 = math.mul(projection.planeProjection, float2);
			float2 float6 = math.mul(projection.planeProjection, float3);
			float2 float7 = math.mul(projection.planeProjection, point);
			float3 float8 = Polygon.ClosestPointOnTriangleBarycentric(float4, float5, float6, float7);
			closest = @float * float8.x + float2 * float8.y + float3 * float8.z;
			float2 float9 = float4 * float8.x + float5 * float8.y + float6 * float8.z;
			distAlongProjection = math.abs(math.dot(closest - point, projection.projectionAxis));
			float num = math.length(float9 - float7);
			if (num < 0.01f)
			{
				int3 @int = (int3)vi1;
				int3 int2 = (int3)vi2;
				int3 int3 = (int3)vi3;
				int3 int4 = (int3)((Int3)point);
				if (Polygon.ContainsPoint(ref @int, ref int2, ref int3, ref int4, in projection.planeProjection))
				{
					num = 0f;
				}
			}
			float num2 = num + distAlongProjection * projection.distanceScaleAlongProjectionAxis;
			sqrDist = num2 * num2;
		}

		// Token: 0x040001FE RID: 510
		private static readonly Dictionary<Int3, int> cached_Int3_int_dict = new Dictionary<Int3, int>();

		// Token: 0x02000053 RID: 83
		public struct BarycentricTriangleInterpolator
		{
			// Token: 0x06000368 RID: 872 RVA: 0x0001056C File Offset: 0x0000E76C
			public BarycentricTriangleInterpolator(Int3 p1, Int3 p2, Int3 p3)
			{
				double num = (double)(p2.z - p3.z) * (double)(p1.x - p3.x) + (double)(p3.x - p2.x) * (double)(p1.z - p3.z);
				double2 @double = new double2((double)(p2.x - p1.x), (double)(p2.z - p1.z));
				double2 double2 = new double2((double)(p3.x - p2.x), (double)(p3.z - p2.z));
				double2 double3 = new double2((double)(p1.x - p3.x), (double)(p1.z - p3.z));
				double num2 = math.lengthsq(@double);
				double num3 = math.lengthsq(double2);
				double num4 = math.lengthsq(double3);
				this.thresholds = math.sqrt(new double3(num3, num4, num2)) * (2.0 / math.abs(num));
				this.origin = new int2(p3.x, p3.z);
				this.barycentricMapping = new double2x2(-double2.y, double2.x, -double3.y, double3.x) / num;
				this.ys = new double3((double)p1.y, (double)p2.y, (double)p3.y);
				this.linear1 = Polygon.BarycentricTriangleInterpolator.<.ctor>g__ProjectPointOnLine|7_0(p2, p3, num3, this.origin);
				this.linear2 = Polygon.BarycentricTriangleInterpolator.<.ctor>g__ProjectPointOnLine|7_0(p3, p1, num4, this.origin);
				this.linear3 = Polygon.BarycentricTriangleInterpolator.<.ctor>g__ProjectPointOnLine|7_0(p1, p2, num2, this.origin);
			}

			// Token: 0x06000369 RID: 873 RVA: 0x000106FC File Offset: 0x0000E8FC
			public int SampleY(int2 p)
			{
				p -= this.origin;
				double2 @double = math.mul(this.barycentricMapping, new double2((double)p.x, (double)p.y));
				double3 double2 = new double3(@double.x, @double.y, 1.0 - @double.x - @double.y);
				if (double2.x < this.thresholds.x)
				{
					return (int)math.round(math.dot(this.linear1, new double3((double)p.x, (double)p.y, 1.0)));
				}
				if (double2.y < this.thresholds.y)
				{
					return (int)math.round(math.dot(this.linear2, new double3((double)p.x, (double)p.y, 1.0)));
				}
				if (double2.z < this.thresholds.z)
				{
					return (int)math.round(math.dot(this.linear3, new double3((double)p.x, (double)p.y, 1.0)));
				}
				return (int)math.round(math.dot(this.ys, double2));
			}

			// Token: 0x0600036A RID: 874 RVA: 0x00010838 File Offset: 0x0000EA38
			[CompilerGenerated]
			internal static double3 <.ctor>g__ProjectPointOnLine|7_0(Int3 a, Int3 b, double lengthSq, int2 origin)
			{
				return new double3((double)(b.x - a.x), (double)(b.z - a.z), (double)(origin.x - a.x) * (double)(b.x - a.x) + (double)(origin.y - a.z) * (double)(b.z - a.z)) / lengthSq * (double)(b.y - a.y) + new double3(0.0, 0.0, (double)a.y);
			}

			// Token: 0x040001FF RID: 511
			private int2 origin;

			// Token: 0x04000200 RID: 512
			private double2x2 barycentricMapping;

			// Token: 0x04000201 RID: 513
			private double3 thresholds;

			// Token: 0x04000202 RID: 514
			private double3 linear1;

			// Token: 0x04000203 RID: 515
			private double3 linear2;

			// Token: 0x04000204 RID: 516
			private double3 linear3;

			// Token: 0x04000205 RID: 517
			private double3 ys;
		}

		// Token: 0x02000054 RID: 84
		// (Invoke) Token: 0x0600036C RID: 876
		internal delegate bool ContainsPoint_00000357$PostfixBurstDelegate(ref int3 aWorld, ref int3 bWorld, ref int3 cWorld, ref int3 pWorld, ref NativeMovementPlane movementPlane);

		// Token: 0x02000055 RID: 85
		internal static class ContainsPoint_00000357$BurstDirectCall
		{
			// Token: 0x0600036F RID: 879 RVA: 0x000108DD File Offset: 0x0000EADD
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Polygon.ContainsPoint_00000357$BurstDirectCall.Pointer == 0)
				{
					Polygon.ContainsPoint_00000357$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Polygon.ContainsPoint_00000357$BurstDirectCall.DeferredCompilation, methodof(Polygon.ContainsPoint$BurstManaged(ref int3, ref int3, ref int3, ref int3, ref NativeMovementPlane)).MethodHandle, typeof(Polygon.ContainsPoint_00000357$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Polygon.ContainsPoint_00000357$BurstDirectCall.Pointer;
			}

			// Token: 0x06000370 RID: 880 RVA: 0x0001090C File Offset: 0x0000EB0C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				Polygon.ContainsPoint_00000357$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000371 RID: 881 RVA: 0x00010924 File Offset: 0x0000EB24
			public static void Constructor()
			{
				Polygon.ContainsPoint_00000357$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Polygon.ContainsPoint(ref int3, ref int3, ref int3, ref int3, ref NativeMovementPlane)).MethodHandle);
			}

			// Token: 0x06000372 RID: 882 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000373 RID: 883 RVA: 0x00010935 File Offset: 0x0000EB35
			// Note: this type is marked as 'beforefieldinit'.
			static ContainsPoint_00000357$BurstDirectCall()
			{
				Polygon.ContainsPoint_00000357$BurstDirectCall.Constructor();
			}

			// Token: 0x06000374 RID: 884 RVA: 0x0001093C File Offset: 0x0000EB3C
			public static bool Invoke(ref int3 aWorld, ref int3 bWorld, ref int3 cWorld, ref int3 pWorld, ref NativeMovementPlane movementPlane)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Polygon.ContainsPoint_00000357$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Mathematics.int3&,Unity.Mathematics.int3&,Unity.Mathematics.int3&,Unity.Mathematics.int3&,Pathfinding.Util.NativeMovementPlane&), ref aWorld, ref bWorld, ref cWorld, ref pWorld, ref movementPlane, functionPointer);
					}
				}
				return Polygon.ContainsPoint$BurstManaged(ref aWorld, ref bWorld, ref cWorld, ref pWorld, ref movementPlane);
			}

			// Token: 0x04000206 RID: 518
			private static IntPtr Pointer;

			// Token: 0x04000207 RID: 519
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000056 RID: 86
		// (Invoke) Token: 0x06000376 RID: 886
		internal delegate bool ClosestPointOnTriangleByRef_0000035D$PostfixBurstDelegate(in float3 a, in float3 b, in float3 c, in float3 p, [NoAlias] out float3 output);

		// Token: 0x02000057 RID: 87
		internal static class ClosestPointOnTriangleByRef_0000035D$BurstDirectCall
		{
			// Token: 0x06000379 RID: 889 RVA: 0x00010977 File Offset: 0x0000EB77
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Polygon.ClosestPointOnTriangleByRef_0000035D$BurstDirectCall.Pointer == 0)
				{
					Polygon.ClosestPointOnTriangleByRef_0000035D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Polygon.ClosestPointOnTriangleByRef_0000035D$BurstDirectCall.DeferredCompilation, methodof(Polygon.ClosestPointOnTriangleByRef$BurstManaged(ref float3, ref float3, ref float3, ref float3, ref float3)).MethodHandle, typeof(Polygon.ClosestPointOnTriangleByRef_0000035D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Polygon.ClosestPointOnTriangleByRef_0000035D$BurstDirectCall.Pointer;
			}

			// Token: 0x0600037A RID: 890 RVA: 0x000109A4 File Offset: 0x0000EBA4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				Polygon.ClosestPointOnTriangleByRef_0000035D$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600037B RID: 891 RVA: 0x000109BC File Offset: 0x0000EBBC
			public static void Constructor()
			{
				Polygon.ClosestPointOnTriangleByRef_0000035D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Polygon.ClosestPointOnTriangleByRef(ref float3, ref float3, ref float3, ref float3, ref float3)).MethodHandle);
			}

			// Token: 0x0600037C RID: 892 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x0600037D RID: 893 RVA: 0x000109CD File Offset: 0x0000EBCD
			// Note: this type is marked as 'beforefieldinit'.
			static ClosestPointOnTriangleByRef_0000035D$BurstDirectCall()
			{
				Polygon.ClosestPointOnTriangleByRef_0000035D$BurstDirectCall.Constructor();
			}

			// Token: 0x0600037E RID: 894 RVA: 0x000109D4 File Offset: 0x0000EBD4
			public static bool Invoke(in float3 a, in float3 b, in float3 c, in float3 p, [NoAlias] out float3 output)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Polygon.ClosestPointOnTriangleByRef_0000035D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Mathematics.float3&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,Unity.Mathematics.float3&), ref a, ref b, ref c, ref p, ref output, functionPointer);
					}
				}
				return Polygon.ClosestPointOnTriangleByRef$BurstManaged(in a, in b, in c, in p, out output);
			}

			// Token: 0x04000208 RID: 520
			private static IntPtr Pointer;

			// Token: 0x04000209 RID: 521
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000058 RID: 88
		// (Invoke) Token: 0x06000380 RID: 896
		internal delegate void ClosestPointOnTriangleProjected_00000360$PostfixBurstDelegate(ref Int3 vi1, ref Int3 vi2, ref Int3 vi3, ref BBTree.ProjectionParams projection, ref float3 point, [NoAlias] out float3 closest, [NoAlias] out float sqrDist, [NoAlias] out float distAlongProjection);

		// Token: 0x02000059 RID: 89
		internal static class ClosestPointOnTriangleProjected_00000360$BurstDirectCall
		{
			// Token: 0x06000383 RID: 899 RVA: 0x00010A0F File Offset: 0x0000EC0F
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Polygon.ClosestPointOnTriangleProjected_00000360$BurstDirectCall.Pointer == 0)
				{
					Polygon.ClosestPointOnTriangleProjected_00000360$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Polygon.ClosestPointOnTriangleProjected_00000360$BurstDirectCall.DeferredCompilation, methodof(Polygon.ClosestPointOnTriangleProjected$BurstManaged(ref Int3, ref Int3, ref Int3, ref BBTree.ProjectionParams, ref float3, ref float3, ref float, ref float)).MethodHandle, typeof(Polygon.ClosestPointOnTriangleProjected_00000360$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Polygon.ClosestPointOnTriangleProjected_00000360$BurstDirectCall.Pointer;
			}

			// Token: 0x06000384 RID: 900 RVA: 0x00010A3C File Offset: 0x0000EC3C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				Polygon.ClosestPointOnTriangleProjected_00000360$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000385 RID: 901 RVA: 0x00010A54 File Offset: 0x0000EC54
			public static void Constructor()
			{
				Polygon.ClosestPointOnTriangleProjected_00000360$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Polygon.ClosestPointOnTriangleProjected(ref Int3, ref Int3, ref Int3, ref BBTree.ProjectionParams, ref float3, ref float3, ref float, ref float)).MethodHandle);
			}

			// Token: 0x06000386 RID: 902 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x06000387 RID: 903 RVA: 0x00010A65 File Offset: 0x0000EC65
			// Note: this type is marked as 'beforefieldinit'.
			static ClosestPointOnTriangleProjected_00000360$BurstDirectCall()
			{
				Polygon.ClosestPointOnTriangleProjected_00000360$BurstDirectCall.Constructor();
			}

			// Token: 0x06000388 RID: 904 RVA: 0x00010A6C File Offset: 0x0000EC6C
			public static void Invoke(ref Int3 vi1, ref Int3 vi2, ref Int3 vi3, ref BBTree.ProjectionParams projection, ref float3 point, [NoAlias] out float3 closest, [NoAlias] out float sqrDist, [NoAlias] out float distAlongProjection)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Polygon.ClosestPointOnTriangleProjected_00000360$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Int3&,Pathfinding.Int3&,Pathfinding.Int3&,Pathfinding.Collections.BBTree/ProjectionParams&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,System.Single&,System.Single&), ref vi1, ref vi2, ref vi3, ref projection, ref point, ref closest, ref sqrDist, ref distAlongProjection, functionPointer);
						return;
					}
				}
				Polygon.ClosestPointOnTriangleProjected$BurstManaged(ref vi1, ref vi2, ref vi3, ref projection, ref point, out closest, out sqrDist, out distAlongProjection);
			}

			// Token: 0x0400020A RID: 522
			private static IntPtr Pointer;

			// Token: 0x0400020B RID: 523
			private static IntPtr DeferredCompilation;
		}
	}
}
