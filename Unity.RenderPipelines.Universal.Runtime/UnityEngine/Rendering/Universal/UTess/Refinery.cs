using System;
using Unity.Collections;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x0200012C RID: 300
	internal struct Refinery
	{
		// Token: 0x060009ED RID: 2541 RVA: 0x00042DCE File Offset: 0x00040FCE
		private static bool RequiresRefining(UTriangle tri, float maxArea)
		{
			return tri.area > maxArea;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00042DDC File Offset: 0x00040FDC
		private static void FetchEncroachedSegments(NativeArray<float2> pgPoints, int pgPointCount, NativeArray<int2> pgEdges, int pgEdgeCount, ref NativeArray<UEncroachingSegment> encroach, ref int encroachCount, UCircle c)
		{
			for (int i = 0; i < pgEdgeCount; i++)
			{
				int2 @int = pgEdges[i];
				float2 @float = pgPoints[@int.x];
				float2 float2 = pgPoints[@int.y];
				if (math.any(c.center - @float) && math.any(c.center - float2))
				{
					float2 float3 = @float - float2;
					float2 float4 = (@float + float2) * 0.5f;
					float num = math.length(float3) * 0.5f;
					if (math.length(float4 - c.center) <= num)
					{
						UEncroachingSegment uencroachingSegment = default(UEncroachingSegment);
						uencroachingSegment.a = @float;
						uencroachingSegment.b = float2;
						uencroachingSegment.index = i;
						int num2 = encroachCount;
						encroachCount = num2 + 1;
						encroach[num2] = uencroachingSegment;
					}
				}
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00042EC0 File Offset: 0x000410C0
		private static void InsertVertex(ref NativeArray<float2> pgPoints, ref int pgPointCount, float2 newVertex, ref int nid)
		{
			nid = pgPointCount;
			pgPoints[nid] = newVertex;
			pgPointCount++;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00042ED8 File Offset: 0x000410D8
		private static int FindSegment(NativeArray<float2> pgPoints, int pgPointCount, NativeArray<int2> pgEdges, int pgEdgeCount, UEncroachingSegment es)
		{
			for (int i = es.index; i < pgEdgeCount; i++)
			{
				int2 @int = pgEdges[i];
				float2 @float = pgPoints[@int.x];
				float2 float2 = pgPoints[@int.y];
				if (!math.any(@float - es.a) && !math.any(float2 - es.b))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00042F48 File Offset: 0x00041148
		private static void SplitSegments(ref NativeArray<float2> pgPoints, ref int pgPointCount, ref NativeArray<int2> pgEdges, ref int pgEdgeCount, UEncroachingSegment es)
		{
			int num = Refinery.FindSegment(pgPoints, pgPointCount, pgEdges, pgEdgeCount, es);
			if (num == -1)
			{
				return;
			}
			int2 @int = pgEdges[num];
			float2 @float = pgPoints[@int.x];
			float2 float2 = pgPoints[@int.y];
			float2 float3 = (@float + float2) * 0.5f;
			int num2 = 0;
			if (math.abs(@int.x - @int.y) == 1)
			{
				num2 = ((@int.x > @int.y) ? @int.x : @int.y);
				Refinery.InsertVertex(ref pgPoints, ref pgPointCount, float3, ref num2);
				int2 int2 = pgEdges[num];
				pgEdges[num] = new int2(int2.x, num2);
				for (int i = pgEdgeCount; i > num + 1; i--)
				{
					pgEdges[i] = pgEdges[i - 1];
				}
				pgEdges[num + 1] = new int2(num2, int2.y);
				pgEdgeCount++;
				return;
			}
			num2 = pgPointCount;
			int num3 = pgPointCount;
			pgPointCount = num3 + 1;
			pgPoints[num3] = float3;
			pgEdges[num] = new int2(math.max(@int.x, @int.y), num2);
			num3 = pgEdgeCount;
			pgEdgeCount = num3 + 1;
			pgEdges[num3] = new int2(math.min(@int.x, @int.y), num2);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x000430A8 File Offset: 0x000412A8
		internal static bool Condition(Allocator allocator, float factorArea, float targetArea, ref NativeArray<float2> pgPoints, ref int pgPointCount, ref NativeArray<int2> pgEdges, ref int pgEdgeCount, ref NativeArray<float2> vertices, ref int vertexCount, ref NativeArray<int> indices, ref int indexCount, ref float maxArea)
		{
			maxArea = 0f;
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			bool flag2 = true;
			int num3 = 0;
			int num4 = -1;
			int num5 = pgPointCount;
			NativeArray<UEncroachingSegment> nativeArray = new NativeArray<UEncroachingSegment>(ModuleHandle.kMaxEdgeCount, allocator, NativeArrayOptions.ClearMemory);
			NativeArray<UTriangle> nativeArray2 = new NativeArray<UTriangle>(ModuleHandle.kMaxTriangleCount, allocator, NativeArrayOptions.ClearMemory);
			ModuleHandle.BuildTriangles(vertices, vertexCount, indices, indexCount, ref nativeArray2, ref num3, ref maxArea, ref num2, ref num);
			factorArea = ((factorArea != 0f) ? math.clamp(factorArea, Refinery.kMinAreaFactor, Refinery.kMaxAreaFactor) : factorArea);
			float num6 = maxArea * factorArea;
			num6 = math.max(num6, targetArea);
			while (!flag && flag2)
			{
				for (int i = 0; i < num3; i++)
				{
					if (Refinery.RequiresRefining(nativeArray2[i], num6))
					{
						num4 = i;
						break;
					}
				}
				if (num4 != -1)
				{
					UTriangle utriangle = nativeArray2[num4];
					int num7 = 0;
					Refinery.FetchEncroachedSegments(pgPoints, pgPointCount, pgEdges, pgEdgeCount, ref nativeArray, ref num7, utriangle.c);
					if (num7 != 0)
					{
						for (int j = 0; j < num7; j++)
						{
							Refinery.SplitSegments(ref pgPoints, ref pgPointCount, ref pgEdges, ref pgEdgeCount, nativeArray[j]);
						}
					}
					else
					{
						float2 center = utriangle.c.center;
						int num8 = pgPointCount;
						pgPointCount = num8 + 1;
						pgPoints[num8] = center;
					}
					indexCount = 0;
					vertexCount = 0;
					flag2 = Tessellator.Tessellate(allocator, pgPoints, pgPointCount, pgEdges, pgEdgeCount, ref vertices, ref vertexCount, ref indices, ref indexCount);
					num7 = 0;
					num3 = 0;
					num4 = -1;
					if (flag2)
					{
						ModuleHandle.BuildTriangles(vertices, vertexCount, indices, indexCount, ref nativeArray2, ref num3, ref maxArea, ref num2, ref num);
					}
					if (pgPointCount - num5 > Refinery.kMaxSteinerCount)
					{
						break;
					}
				}
				else
				{
					flag = true;
				}
			}
			nativeArray2.Dispose();
			nativeArray.Dispose();
			return flag;
		}

		// Token: 0x040008DB RID: 2267
		private static readonly float kMinAreaFactor = 0.0482f;

		// Token: 0x040008DC RID: 2268
		private static readonly float kMaxAreaFactor = 0.482f;

		// Token: 0x040008DD RID: 2269
		private static readonly int kMaxSteinerCount = 4084;
	}
}
