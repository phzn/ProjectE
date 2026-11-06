using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x02000140 RID: 320
	internal struct ModuleHandle
	{
		// Token: 0x06000A20 RID: 2592 RVA: 0x000459FB File Offset: 0x00043BFB
		internal static void Copy<T>(NativeArray<T> src, int srcIndex, NativeArray<T> dst, int dstIndex, int length) where T : struct
		{
			NativeArray<T>.Copy(src, srcIndex, dst, dstIndex, length);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00045A08 File Offset: 0x00043C08
		internal static void Copy<T>(NativeArray<T> src, NativeArray<T> dst, int length) where T : struct
		{
			ModuleHandle.Copy<T>(src, 0, dst, 0, length);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00045A14 File Offset: 0x00043C14
		internal unsafe static void InsertionSort<T, U>(void* array, int lo, int hi, U comp) where T : struct where U : IComparer<T>
		{
			for (int i = lo; i < hi; i++)
			{
				int num = i;
				T t = UnsafeUtility.ReadArrayElement<T>(array, i + 1);
				while (num >= lo && comp.Compare(t, UnsafeUtility.ReadArrayElement<T>(array, num)) < 0)
				{
					UnsafeUtility.WriteArrayElement<T>(array, num + 1, UnsafeUtility.ReadArrayElement<T>(array, num));
					num--;
				}
				UnsafeUtility.WriteArrayElement<T>(array, num + 1, t);
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00045A78 File Offset: 0x00043C78
		internal static int GetLower<T, U, X>(NativeArray<T> values, int count, U check, X condition) where T : struct where U : struct where X : ICondition2<T, U>
		{
			int i = 0;
			int num = count - 1;
			int num2 = i - 1;
			while (i <= num)
			{
				int num3 = i + num >> 1;
				float num4 = 0f;
				if (condition.Test(values[num3], check, ref num4))
				{
					num2 = num3;
					i = num3 + 1;
				}
				else
				{
					num = num3 - 1;
				}
			}
			return num2;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00045ACC File Offset: 0x00043CCC
		internal static int GetUpper<T, U, X>(NativeArray<T> values, int count, U check, X condition) where T : struct where U : struct where X : ICondition2<T, U>
		{
			int i = 0;
			int num = count - 1;
			int num2 = num + 1;
			while (i <= num)
			{
				int num3 = i + num >> 1;
				float num4 = 0f;
				if (condition.Test(values[num3], check, ref num4))
				{
					num2 = num3;
					num = num3 - 1;
				}
				else
				{
					i = num3 + 1;
				}
			}
			return num2;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00045B20 File Offset: 0x00043D20
		internal static int GetEqual<T, U, X>(NativeArray<T> values, int count, U check, X condition) where T : struct where U : struct where X : ICondition2<T, U>
		{
			int i = 0;
			int num = count - 1;
			while (i <= num)
			{
				int num2 = i + num >> 1;
				float num3 = 0f;
				condition.Test(values[num2], check, ref num3);
				if (num3 == 0f)
				{
					return num2;
				}
				if (num3 <= 0f)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2 - 1;
				}
			}
			return -1;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00045B7C File Offset: 0x00043D7C
		internal static float OrientFast(float2 a, float2 b, float2 c)
		{
			float num = 1.110223E-16f;
			float num2 = (b.y - a.y) * (c.x - b.x) - (b.x - a.x) * (c.y - b.y);
			if (math.abs(num2) < num)
			{
				return 0f;
			}
			return num2;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00045BD8 File Offset: 0x00043DD8
		internal static double OrientFastDouble(double2 a, double2 b, double2 c)
		{
			double num = 1.1102230246251565E-16;
			double num2 = (b.y - a.y) * (c.x - b.x) - (b.x - a.x) * (c.y - b.y);
			if (math.abs(num2) < num)
			{
				return 0.0;
			}
			return num2;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00045C3C File Offset: 0x00043E3C
		internal static UCircle CircumCircle(UTriangle tri)
		{
			float num = tri.va.x * tri.va.x;
			float num2 = tri.vb.x * tri.vb.x;
			float num3 = tri.vc.x * tri.vc.x;
			float num4 = tri.va.y * tri.va.y;
			float num5 = tri.vb.y * tri.vb.y;
			float num6 = tri.vc.y * tri.vc.y;
			float num7 = 2f * ((tri.vb.x - tri.va.x) * (tri.vc.y - tri.va.y) - (tri.vb.y - tri.va.y) * (tri.vc.x - tri.va.x));
			float num8 = ((tri.vc.y - tri.va.y) * (num2 - num + num5 - num4) + (tri.va.y - tri.vb.y) * (num3 - num + num6 - num4)) / num7;
			float num9 = ((tri.va.x - tri.vc.x) * (num2 - num + num5 - num4) + (tri.vb.x - tri.va.x) * (num3 - num + num6 - num4)) / num7;
			float num10 = tri.va.x - num8;
			float num11 = tri.va.y - num9;
			return new UCircle
			{
				center = new float2(num8, num9),
				radius = math.sqrt(num10 * num10 + num11 * num11)
			};
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00045E1F File Offset: 0x0004401F
		internal static bool IsInsideCircle(UCircle c, float2 v)
		{
			return math.distance(v, c.center) < c.radius;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00045E38 File Offset: 0x00044038
		internal static float TriangleArea(float2 va, float2 vb, float2 vc)
		{
			float3 @float = new float3(va.x, va.y, 0f);
			float3 float2 = new float3(vb.x, vb.y, 0f);
			float3 float3 = new float3(vc.x, vc.y, 0f);
			return math.abs(math.cross(@float - float2, @float - float3).z) * 0.5f;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x00045EB0 File Offset: 0x000440B0
		internal static float Sign(float2 p1, float2 p2, float2 p3)
		{
			return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00045EEC File Offset: 0x000440EC
		internal static bool IsInsideTriangle(float2 pt, float2 v1, float2 v2, float2 v3)
		{
			float num = ModuleHandle.Sign(pt, v1, v2);
			float num2 = ModuleHandle.Sign(pt, v2, v3);
			float num3 = ModuleHandle.Sign(pt, v3, v1);
			bool flag = num < 0f || num2 < 0f || num3 < 0f;
			bool flag2 = num > 0f || num2 > 0f || num3 > 0f;
			return !flag || !flag2;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x00045F50 File Offset: 0x00044150
		internal static bool IsInsideTriangleApproximate(float2 pt, float2 v1, float2 v2, float2 v3)
		{
			float num = ModuleHandle.TriangleArea(v1, v2, v3);
			float num2 = ModuleHandle.TriangleArea(pt, v1, v2);
			float num3 = ModuleHandle.TriangleArea(pt, v2, v3);
			float num4 = ModuleHandle.TriangleArea(pt, v3, v1);
			float num5 = 1.110223E-16f;
			return Mathf.Abs(num - (num2 + num3 + num4)) < num5;
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00045F94 File Offset: 0x00044194
		internal static bool IsInsideCircle(float2 a, float2 b, float2 c, float2 p)
		{
			float num = math.dot(a, a);
			float num2 = math.dot(b, b);
			float num3 = math.dot(c, c);
			float x = a.x;
			float y = a.y;
			float x2 = b.x;
			float y2 = b.y;
			float x3 = c.x;
			float y3 = c.y;
			float num4 = (num * (y3 - y2) + num2 * (y - y3) + num3 * (y2 - y)) / (x * (y3 - y2) + x2 * (y - y3) + x3 * (y2 - y));
			float num5 = (num * (x3 - x2) + num2 * (x - x3) + num3 * (x2 - x)) / (y * (x3 - x2) + y2 * (x - x3) + y3 * (x2 - x));
			float2 @float = new float2
			{
				x = num4 / 2f,
				y = num5 / 2f
			};
			float num6 = math.distance(a, @float);
			float num7 = math.distance(p, @float);
			return num6 - num7 > 1E-05f;
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00046088 File Offset: 0x00044288
		internal static void BuildTriangles(NativeArray<float2> vertices, int vertexCount, NativeArray<int> indices, int indexCount, ref NativeArray<UTriangle> triangles, ref int triangleCount, ref float maxArea, ref float avgArea, ref float minArea)
		{
			for (int i = 0; i < indexCount; i += 3)
			{
				UTriangle utriangle = default(UTriangle);
				int num = indices[i];
				int num2 = indices[i + 1];
				int num3 = indices[i + 2];
				utriangle.va = vertices[num];
				utriangle.vb = vertices[num2];
				utriangle.vc = vertices[num3];
				utriangle.c = ModuleHandle.CircumCircle(utriangle);
				utriangle.area = ModuleHandle.TriangleArea(utriangle.va, utriangle.vb, utriangle.vc);
				maxArea = math.max(utriangle.area, maxArea);
				minArea = math.min(utriangle.area, minArea);
				avgArea += utriangle.area;
				int num4 = triangleCount;
				triangleCount = num4 + 1;
				triangles[num4] = utriangle;
			}
			avgArea /= (float)triangleCount;
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00046178 File Offset: 0x00044378
		internal static void BuildTriangles(NativeArray<float2> vertices, int vertexCount, NativeArray<int> indices, int indexCount, ref NativeArray<UTriangle> triangles, ref int triangleCount, ref float maxArea, ref float avgArea, ref float minArea, ref float maxEdge, ref float avgEdge, ref float minEdge)
		{
			for (int i = 0; i < indexCount; i += 3)
			{
				UTriangle utriangle = default(UTriangle);
				int num = indices[i];
				int num2 = indices[i + 1];
				int num3 = indices[i + 2];
				utriangle.va = vertices[num];
				utriangle.vb = vertices[num2];
				utriangle.vc = vertices[num3];
				utriangle.c = ModuleHandle.CircumCircle(utriangle);
				utriangle.area = ModuleHandle.TriangleArea(utriangle.va, utriangle.vb, utriangle.vc);
				maxArea = math.max(utriangle.area, maxArea);
				minArea = math.min(utriangle.area, minArea);
				avgArea += utriangle.area;
				float num4 = math.distance(utriangle.va, utriangle.vb);
				float num5 = math.distance(utriangle.vb, utriangle.vc);
				float num6 = math.distance(utriangle.vc, utriangle.va);
				maxEdge = math.max(num4, maxEdge);
				maxEdge = math.max(num5, maxEdge);
				maxEdge = math.max(num6, maxEdge);
				minEdge = math.min(num4, minEdge);
				minEdge = math.min(num5, minEdge);
				minEdge = math.min(num6, minEdge);
				avgEdge += num4;
				avgEdge += num5;
				avgEdge += num6;
				int num7 = triangleCount;
				triangleCount = num7 + 1;
				triangles[num7] = utriangle;
			}
			avgArea /= (float)triangleCount;
			avgEdge /= (float)indexCount;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00046314 File Offset: 0x00044514
		internal static void BuildTrianglesAndEdges(NativeArray<float2> vertices, int vertexCount, NativeArray<int> indices, int indexCount, ref NativeArray<UTriangle> triangles, ref int triangleCount, ref NativeArray<int4> delaEdges, ref int delaEdgeCount, ref float maxArea, ref float avgArea, ref float minArea)
		{
			for (int i = 0; i < indexCount; i += 3)
			{
				UTriangle utriangle = default(UTriangle);
				int num = indices[i];
				int num2 = indices[i + 1];
				int num3 = indices[i + 2];
				utriangle.va = vertices[num];
				utriangle.vb = vertices[num2];
				utriangle.vc = vertices[num3];
				utriangle.c = ModuleHandle.CircumCircle(utriangle);
				utriangle.area = ModuleHandle.TriangleArea(utriangle.va, utriangle.vb, utriangle.vc);
				maxArea = math.max(utriangle.area, maxArea);
				minArea = math.min(utriangle.area, minArea);
				avgArea += utriangle.area;
				utriangle.indices = new int3(num, num2, num3);
				int num4 = delaEdgeCount;
				delaEdgeCount = num4 + 1;
				delaEdges[num4] = new int4(math.min(num, num2), math.max(num, num2), triangleCount, -1);
				num4 = delaEdgeCount;
				delaEdgeCount = num4 + 1;
				delaEdges[num4] = new int4(math.min(num2, num3), math.max(num2, num3), triangleCount, -1);
				num4 = delaEdgeCount;
				delaEdgeCount = num4 + 1;
				delaEdges[num4] = new int4(math.min(num3, num), math.max(num3, num), triangleCount, -1);
				num4 = triangleCount;
				triangleCount = num4 + 1;
				triangles[num4] = utriangle;
			}
			avgArea /= (float)triangleCount;
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0004649C File Offset: 0x0004469C
		private static void CopyGraph(NativeArray<float2> srcPoints, int srcPointCount, ref NativeArray<float2> dstPoints, ref int dstPointCount, NativeArray<int2> srcEdges, int srcEdgeCount, ref NativeArray<int2> dstEdges, ref int dstEdgeCount)
		{
			dstEdgeCount = srcEdgeCount;
			dstPointCount = srcPointCount;
			ModuleHandle.Copy<int2>(srcEdges, dstEdges, srcEdgeCount);
			ModuleHandle.Copy<float2>(srcPoints, dstPoints, srcPointCount);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x000464C3 File Offset: 0x000446C3
		private static void CopyGeometry(NativeArray<int> srcIndices, int srcIndexCount, ref NativeArray<int> dstIndices, ref int dstIndexCount, NativeArray<float2> srcVertices, int srcVertexCount, ref NativeArray<float2> dstVertices, ref int dstVertexCount)
		{
			dstIndexCount = srcIndexCount;
			dstVertexCount = srcVertexCount;
			ModuleHandle.Copy<int>(srcIndices, dstIndices, srcIndexCount);
			ModuleHandle.Copy<float2>(srcVertices, dstVertices, srcVertexCount);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x000464EA File Offset: 0x000446EA
		private static void TransferOutput(NativeArray<int2> srcEdges, int srcEdgeCount, ref NativeArray<int2> dstEdges, ref int dstEdgeCount, NativeArray<int> srcIndices, int srcIndexCount, ref NativeArray<int> dstIndices, ref int dstIndexCount, NativeArray<float2> srcVertices, int srcVertexCount, ref NativeArray<float2> dstVertices, ref int dstVertexCount)
		{
			dstEdgeCount = srcEdgeCount;
			dstIndexCount = srcIndexCount;
			dstVertexCount = srcVertexCount;
			ModuleHandle.Copy<int2>(srcEdges, dstEdges, srcEdgeCount);
			ModuleHandle.Copy<int>(srcIndices, dstIndices, srcIndexCount);
			ModuleHandle.Copy<float2>(srcVertices, dstVertices, srcVertexCount);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x00046528 File Offset: 0x00044728
		private static void GraphConditioner(NativeArray<float2> points, ref NativeArray<float2> pgPoints, ref int pgPointCount, ref NativeArray<int2> pgEdges, ref int pgEdgeCount, bool resetTopology)
		{
			float2 @float = new float2(float.PositiveInfinity, float.PositiveInfinity);
			float2 float2 = float2.zero;
			for (int i = 0; i < points.Length; i++)
			{
				@float = math.min(points[i], @float);
				float2 = math.max(points[i], float2);
			}
			float2 float3 = (float2 - @float) * 0.5f;
			float num = 0.0001f;
			pgPointCount = (resetTopology ? 0 : pgPointCount);
			int num2 = pgPointCount;
			int num3 = pgPointCount;
			pgPointCount = num3 + 1;
			pgPoints[num3] = new float2(@float.x, @float.y);
			num3 = pgPointCount;
			pgPointCount = num3 + 1;
			pgPoints[num3] = new float2(@float.x - num, @float.y + float3.y);
			num3 = pgPointCount;
			pgPointCount = num3 + 1;
			pgPoints[num3] = new float2(@float.x, float2.y);
			num3 = pgPointCount;
			pgPointCount = num3 + 1;
			pgPoints[num3] = new float2(@float.x + float3.x, float2.y + num);
			num3 = pgPointCount;
			pgPointCount = num3 + 1;
			pgPoints[num3] = new float2(float2.x, float2.y);
			num3 = pgPointCount;
			pgPointCount = num3 + 1;
			pgPoints[num3] = new float2(float2.x + num, @float.y + float3.y);
			num3 = pgPointCount;
			pgPointCount = num3 + 1;
			pgPoints[num3] = new float2(float2.x, @float.y);
			num3 = pgPointCount;
			pgPointCount = num3 + 1;
			pgPoints[num3] = new float2(@float.x + float3.x, @float.y - num);
			pgEdgeCount = 8;
			pgEdges[0] = new int2(num2, num2 + 1);
			pgEdges[1] = new int2(num2 + 1, num2 + 2);
			pgEdges[2] = new int2(num2 + 2, num2 + 3);
			pgEdges[3] = new int2(num2 + 3, num2 + 4);
			pgEdges[4] = new int2(num2 + 4, num2 + 5);
			pgEdges[5] = new int2(num2 + 5, num2 + 6);
			pgEdges[6] = new int2(num2 + 6, num2 + 7);
			pgEdges[7] = new int2(num2 + 7, num2);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00046788 File Offset: 0x00044988
		private static void Reorder(int startVertexCount, int index, ref NativeArray<int> indices, ref int indexCount, ref NativeArray<float2> vertices, ref int vertexCount)
		{
			bool flag = false;
			for (int i = 0; i < indexCount; i++)
			{
				if (indices[i] == index)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				vertexCount--;
				vertices[index] = vertices[vertexCount];
				for (int j = 0; j < indexCount; j++)
				{
					if (indices[j] == vertexCount)
					{
						indices[j] = index;
					}
				}
			}
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x000467F0 File Offset: 0x000449F0
		internal static void VertexCleanupConditioner(int startVertexCount, ref NativeArray<int> indices, ref int indexCount, ref NativeArray<float2> vertices, ref int vertexCount)
		{
			for (int i = startVertexCount; i < vertexCount; i++)
			{
				ModuleHandle.Reorder(startVertexCount, i, ref indices, ref indexCount, ref vertices, ref vertexCount);
			}
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00046818 File Offset: 0x00044A18
		public static float4 ConvexQuad(Allocator allocator, NativeArray<float2> points, NativeArray<int2> edges, ref NativeArray<float2> outVertices, ref int outVertexCount, ref NativeArray<int> outIndices, ref int outIndexCount, ref NativeArray<int2> outEdges, ref int outEdgeCount)
		{
			float4 zero = float4.zero;
			outEdgeCount = 0;
			outIndexCount = 0;
			outVertexCount = 0;
			if (points.Length < 3 || points.Length >= ModuleHandle.kMaxVertexCount)
			{
				return zero;
			}
			int num = 0;
			int num2 = 0;
			NativeArray<int2> nativeArray = new NativeArray<int2>(ModuleHandle.kMaxEdgeCount, allocator, NativeArrayOptions.ClearMemory);
			NativeArray<float2> nativeArray2 = new NativeArray<float2>(ModuleHandle.kMaxVertexCount, allocator, NativeArrayOptions.ClearMemory);
			ModuleHandle.GraphConditioner(points, ref nativeArray2, ref num2, ref nativeArray, ref num, true);
			Tessellator.Tessellate(allocator, nativeArray2, num2, nativeArray, num, ref outVertices, ref outVertexCount, ref outIndices, ref outIndexCount);
			nativeArray2.Dispose();
			nativeArray.Dispose();
			return zero;
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x000468A4 File Offset: 0x00044AA4
		public static float4 Tessellate(Allocator allocator, NativeArray<float2> points, NativeArray<int2> edges, ref NativeArray<float2> outVertices, ref int outVertexCount, ref NativeArray<int> outIndices, ref int outIndexCount, ref NativeArray<int2> outEdges, ref int outEdgeCount)
		{
			float4 zero = float4.zero;
			outEdgeCount = 0;
			outIndexCount = 0;
			outVertexCount = 0;
			if (points.Length < 3 || points.Length >= ModuleHandle.kMaxVertexCount)
			{
				return zero;
			}
			bool flag = false;
			bool flag2 = false;
			int num = 0;
			int num2 = 0;
			NativeArray<int2> nativeArray = new NativeArray<int2>(edges.Length * 8, allocator, NativeArrayOptions.ClearMemory);
			NativeArray<float2> nativeArray2 = new NativeArray<float2>(points.Length * 4, allocator, NativeArrayOptions.ClearMemory);
			if (edges.Length != 0)
			{
				flag = PlanarGraph.Validate(allocator, points, points.Length, edges, edges.Length, ref nativeArray2, ref num2, ref nativeArray, ref num);
			}
			if (!flag)
			{
				outEdgeCount = edges.Length;
				outVertexCount = points.Length;
				ModuleHandle.Copy<int2>(edges, outEdges, edges.Length);
				ModuleHandle.Copy<float2>(points, outVertices, points.Length);
			}
			if (num2 > 2 && num > 2)
			{
				NativeArray<int> nativeArray3 = new NativeArray<int>(num2 * 8, allocator, NativeArrayOptions.ClearMemory);
				NativeArray<float2> nativeArray4 = new NativeArray<float2>(num2 * 4, allocator, NativeArrayOptions.ClearMemory);
				int num3 = 0;
				int num4 = 0;
				flag = Tessellator.Tessellate(allocator, nativeArray2, num2, nativeArray, num, ref nativeArray4, ref num4, ref nativeArray3, ref num3);
				if (flag)
				{
					ModuleHandle.TransferOutput(nativeArray, num, ref outEdges, ref outEdgeCount, nativeArray3, num3, ref outIndices, ref outIndexCount, nativeArray4, num4, ref outVertices, ref outVertexCount);
					if (flag2)
					{
						outEdgeCount = 0;
					}
				}
				nativeArray4.Dispose();
				nativeArray3.Dispose();
			}
			nativeArray2.Dispose();
			nativeArray.Dispose();
			return zero;
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x000469F4 File Offset: 0x00044BF4
		public static float4 Subdivide(Allocator allocator, NativeArray<float2> points, NativeArray<int2> edges, ref NativeArray<float2> outVertices, ref int outVertexCount, ref NativeArray<int> outIndices, ref int outIndexCount, ref NativeArray<int2> outEdges, ref int outEdgeCount, float areaFactor, float targetArea, int refineIterations, int smoothenIterations)
		{
			float4 zero = float4.zero;
			outEdgeCount = 0;
			outIndexCount = 0;
			outVertexCount = 0;
			if (points.Length < 3 || points.Length >= ModuleHandle.kMaxVertexCount || edges.Length == 0)
			{
				return zero;
			}
			int num = 0;
			int num2 = 0;
			NativeArray<int> nativeArray = new NativeArray<int>(ModuleHandle.kMaxIndexCount, allocator, NativeArrayOptions.ClearMemory);
			NativeArray<float2> nativeArray2 = new NativeArray<float2>(ModuleHandle.kMaxVertexCount, allocator, NativeArrayOptions.ClearMemory);
			bool flag = Tessellator.Tessellate(allocator, points, points.Length, edges, edges.Length, ref nativeArray2, ref num2, ref nativeArray, ref num);
			bool flag2 = false;
			bool flag3 = targetArea != 0f || areaFactor != 0f;
			if (flag && flag3)
			{
				float num3 = 0f;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				NativeArray<int2> nativeArray3 = new NativeArray<int2>(ModuleHandle.kMaxEdgeCount, allocator, NativeArrayOptions.ClearMemory);
				NativeArray<float2> nativeArray4 = new NativeArray<float2>(ModuleHandle.kMaxVertexCount, allocator, NativeArrayOptions.ClearMemory);
				NativeArray<int> nativeArray5 = new NativeArray<int>(ModuleHandle.kMaxIndexCount, allocator, NativeArrayOptions.ClearMemory);
				NativeArray<float2> nativeArray6 = new NativeArray<float2>(ModuleHandle.kMaxVertexCount, allocator, NativeArrayOptions.ClearMemory);
				zero.x = 0f;
				refineIterations = Math.Min(refineIterations, ModuleHandle.kMaxRefineIterations);
				if (targetArea != 0f)
				{
					float num8 = targetArea / 10f;
					while (targetArea < (float)ModuleHandle.kMaxArea)
					{
						if (refineIterations <= 0)
						{
							break;
						}
						ModuleHandle.CopyGraph(points, points.Length, ref nativeArray4, ref num5, edges, edges.Length, ref nativeArray3, ref num4);
						ModuleHandle.CopyGeometry(nativeArray, num, ref nativeArray5, ref num6, nativeArray2, num2, ref nativeArray6, ref num7);
						flag2 = Refinery.Condition(allocator, areaFactor, targetArea, ref nativeArray4, ref num5, ref nativeArray3, ref num4, ref nativeArray6, ref num7, ref nativeArray5, ref num6, ref num3);
						if (flag2 && num6 > num5)
						{
							zero.x = areaFactor;
							ModuleHandle.TransferOutput(nativeArray3, num4, ref outEdges, ref outEdgeCount, nativeArray5, num6, ref outIndices, ref outIndexCount, nativeArray6, num7, ref outVertices, ref outVertexCount);
							break;
						}
						flag2 = false;
						targetArea += num8;
						refineIterations--;
					}
				}
				else if (areaFactor != 0f)
				{
					areaFactor = math.lerp(0.1f, 0.54f, (areaFactor - 0.05f) / 0.45f);
					float num8 = areaFactor / 10f;
					while (areaFactor < 0.8f && refineIterations > 0)
					{
						ModuleHandle.CopyGraph(points, points.Length, ref nativeArray4, ref num5, edges, edges.Length, ref nativeArray3, ref num4);
						ModuleHandle.CopyGeometry(nativeArray, num, ref nativeArray5, ref num6, nativeArray2, num2, ref nativeArray6, ref num7);
						flag2 = Refinery.Condition(allocator, areaFactor, targetArea, ref nativeArray4, ref num5, ref nativeArray3, ref num4, ref nativeArray6, ref num7, ref nativeArray5, ref num6, ref num3);
						if (flag2 && num6 > num5)
						{
							zero.x = areaFactor;
							ModuleHandle.TransferOutput(nativeArray3, num4, ref outEdges, ref outEdgeCount, nativeArray5, num6, ref outIndices, ref outIndexCount, nativeArray6, num7, ref outVertices, ref outVertexCount);
							break;
						}
						flag2 = false;
						areaFactor += num8;
						refineIterations--;
					}
				}
				if (flag2)
				{
					if (zero.x != 0f)
					{
						ModuleHandle.VertexCleanupConditioner(num2, ref nativeArray5, ref num6, ref nativeArray6, ref num7);
					}
					zero.y = 0f;
					smoothenIterations = math.clamp(smoothenIterations, 0, ModuleHandle.kMaxSmoothenIterations);
					while (smoothenIterations > 0 && Smoothen.Condition(allocator, ref nativeArray4, num5, nativeArray3, num4, ref nativeArray6, ref num7, ref nativeArray5, ref num6))
					{
						zero.y = (float)smoothenIterations;
						ModuleHandle.TransferOutput(nativeArray3, num4, ref outEdges, ref outEdgeCount, nativeArray5, num6, ref outIndices, ref outIndexCount, nativeArray6, num7, ref outVertices, ref outVertexCount);
						smoothenIterations--;
					}
					if (zero.y != 0f)
					{
						ModuleHandle.VertexCleanupConditioner(num2, ref outIndices, ref outIndexCount, ref outVertices, ref outVertexCount);
					}
				}
				nativeArray6.Dispose();
				nativeArray5.Dispose();
				nativeArray4.Dispose();
				nativeArray3.Dispose();
			}
			if (flag && !flag2)
			{
				ModuleHandle.TransferOutput(edges, edges.Length, ref outEdges, ref outEdgeCount, nativeArray, num, ref outIndices, ref outIndexCount, nativeArray2, num2, ref outVertices, ref outVertexCount);
			}
			nativeArray2.Dispose();
			nativeArray.Dispose();
			return zero;
		}

		// Token: 0x04000913 RID: 2323
		internal static readonly int kMaxArea = 65536;

		// Token: 0x04000914 RID: 2324
		internal static readonly int kMaxEdgeCount = 65536;

		// Token: 0x04000915 RID: 2325
		internal static readonly int kMaxIndexCount = 65536;

		// Token: 0x04000916 RID: 2326
		internal static readonly int kMaxVertexCount = 65536;

		// Token: 0x04000917 RID: 2327
		internal static readonly int kMaxTriangleCount = ModuleHandle.kMaxIndexCount / 3;

		// Token: 0x04000918 RID: 2328
		internal static readonly int kMaxRefineIterations = 48;

		// Token: 0x04000919 RID: 2329
		internal static readonly int kMaxSmoothenIterations = 256;

		// Token: 0x0400091A RID: 2330
		internal static readonly float kIncrementAreaFactor = 1.2f;
	}
}
