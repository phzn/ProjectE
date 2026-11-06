using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x0200012E RID: 302
	internal struct Tessellator
	{
		// Token: 0x060009FB RID: 2555 RVA: 0x00043994 File Offset: 0x00041B94
		private static float FindSplit(UHull hull, UEvent edge)
		{
			float num;
			if (hull.a.x < edge.a.x)
			{
				num = ModuleHandle.OrientFast(hull.a, hull.b, edge.a);
			}
			else
			{
				num = ModuleHandle.OrientFast(edge.b, edge.a, hull.a);
			}
			if (0f != num)
			{
				return num;
			}
			if (edge.b.x < hull.b.x)
			{
				num = ModuleHandle.OrientFast(hull.a, hull.b, edge.b);
			}
			else
			{
				num = ModuleHandle.OrientFast(edge.b, edge.a, hull.b);
			}
			if (0f != num)
			{
				return num;
			}
			return (float)(hull.idx - edge.idx);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00043A5D File Offset: 0x00041C5D
		private void SetAllocator(Allocator allocator)
		{
			this.m_Allocator = allocator;
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00043A68 File Offset: 0x00041C68
		private bool AddPoint(NativeArray<UHull> hulls, int hullCount, NativeArray<float2> points, float2 p, int idx)
		{
			int lower = ModuleHandle.GetLower<UHull, float2, Tessellator.TestHullPointL>(hulls, hullCount, p, default(Tessellator.TestHullPointL));
			int upper = ModuleHandle.GetUpper<UHull, float2, Tessellator.TestHullPointU>(hulls, hullCount, p, default(Tessellator.TestHullPointU));
			if (lower < 0 || upper < 0)
			{
				return false;
			}
			for (int i = lower; i < upper; i++)
			{
				UHull uhull = hulls[i];
				int num = uhull.ilcount;
				while (num > 1 && ModuleHandle.OrientFast(points[uhull.ilarray[num - 2]], points[uhull.ilarray[num - 1]], p) > 0f)
				{
					int3 @int = default(int3);
					@int.x = uhull.ilarray[num - 1];
					@int.y = uhull.ilarray[num - 2];
					@int.z = idx;
					int num2 = this.m_CellCount;
					this.m_CellCount = num2 + 1;
					this.m_Cells[num2] = @int;
					num--;
				}
				uhull.ilcount = num + 1;
				if (uhull.ilcount > uhull.ilarray.Length)
				{
					return false;
				}
				uhull.ilarray[num] = idx;
				num = uhull.iucount;
				while (num > 1 && ModuleHandle.OrientFast(points[uhull.iuarray[num - 2]], points[uhull.iuarray[num - 1]], p) < 0f)
				{
					int3 int2 = default(int3);
					int2.x = uhull.iuarray[num - 2];
					int2.y = uhull.iuarray[num - 1];
					int2.z = idx;
					int num2 = this.m_CellCount;
					this.m_CellCount = num2 + 1;
					this.m_Cells[num2] = int2;
					num--;
				}
				uhull.iucount = num + 1;
				if (uhull.iucount > uhull.iuarray.Length)
				{
					return false;
				}
				uhull.iuarray[num] = idx;
				hulls[i] = uhull;
			}
			return true;
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00043CA0 File Offset: 0x00041EA0
		private static void InsertHull(NativeArray<UHull> Hulls, int Pos, ref int Count, UHull Value)
		{
			if (Count < Hulls.Length - 1)
			{
				for (int i = Count; i > Pos; i--)
				{
					Hulls[i] = Hulls[i - 1];
				}
				Hulls[Pos] = Value;
				Count++;
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00043CE8 File Offset: 0x00041EE8
		private static void EraseHull(NativeArray<UHull> Hulls, int Pos, ref int Count)
		{
			if (Count < Hulls.Length)
			{
				for (int i = Pos; i < Count - 1; i++)
				{
					Hulls[i] = Hulls[i + 1];
				}
				Count--;
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00043D28 File Offset: 0x00041F28
		private bool SplitHulls(NativeArray<UHull> hulls, ref int hullCount, NativeArray<float2> points, UEvent evt)
		{
			int lower = ModuleHandle.GetLower<UHull, UEvent, Tessellator.TestHullEventLe>(hulls, hullCount, evt, default(Tessellator.TestHullEventLe));
			if (lower < 0)
			{
				return false;
			}
			UHull uhull = hulls[lower];
			UHull uhull2;
			uhull2.a = evt.a;
			uhull2.b = evt.b;
			uhull2.idx = evt.idx;
			int num = uhull.iuarray[uhull.iucount - 1];
			uhull2.iuarray = new ArraySlice<int>(this.m_IUArray, uhull2.idx * this.m_NumHulls, this.m_NumHulls);
			uhull2.iucount = uhull.iucount;
			for (int i = 0; i < uhull2.iucount; i++)
			{
				uhull2.iuarray[i] = uhull.iuarray[i];
			}
			uhull.iuarray[0] = num;
			uhull.iucount = 1;
			hulls[lower] = uhull;
			uhull2.ilarray = new ArraySlice<int>(this.m_ILArray, uhull2.idx * this.m_NumHulls, this.m_NumHulls);
			uhull2.ilarray[0] = num;
			uhull2.ilcount = 1;
			Tessellator.InsertHull(hulls, lower + 1, ref hullCount, uhull2);
			return true;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00043E64 File Offset: 0x00042064
		private bool MergeHulls(NativeArray<UHull> hulls, ref int hullCount, NativeArray<float2> points, UEvent evt)
		{
			float2 a = evt.a;
			evt.a = evt.b;
			evt.b = a;
			int equal = ModuleHandle.GetEqual<UHull, UEvent, Tessellator.TestHullEventE>(hulls, hullCount, evt, default(Tessellator.TestHullEventE));
			if (equal < 0)
			{
				return false;
			}
			UHull uhull = hulls[equal];
			UHull uhull2 = hulls[equal - 1];
			uhull2.iucount = uhull.iucount;
			for (int i = 0; i < uhull2.iucount; i++)
			{
				uhull2.iuarray[i] = uhull.iuarray[i];
			}
			hulls[equal - 1] = uhull2;
			Tessellator.EraseHull(hulls, equal, ref hullCount);
			return true;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00043F10 File Offset: 0x00042110
		private static void InsertUniqueEdge(NativeArray<int2> edges, int2 e, ref int edgeCount)
		{
			TessEdgeCompare tessEdgeCompare = default(TessEdgeCompare);
			bool flag = true;
			int num = 0;
			while (flag && num < edgeCount)
			{
				if (tessEdgeCompare.Compare(e, edges[num]) == 0)
				{
					flag = false;
				}
				num++;
			}
			if (flag)
			{
				int num2 = edgeCount;
				edgeCount = num2 + 1;
				edges[num2] = e;
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00043F60 File Offset: 0x00042160
		private void PrepareDelaunay(NativeArray<int2> edges, int edgeCount)
		{
			this.m_StarCount = this.m_CellCount * 3;
			this.m_Stars = new NativeArray<UStar>(this.m_StarCount, this.m_Allocator, NativeArrayOptions.ClearMemory);
			this.m_SPArray = new NativeArray<int>(this.m_StarCount * this.m_StarCount, this.m_Allocator, NativeArrayOptions.ClearMemory);
			int num = 0;
			NativeArray<int2> nativeArray = new NativeArray<int2>(this.m_StarCount, this.m_Allocator, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < edgeCount; i++)
			{
				int2 @int = edges[i];
				@int.x = ((edges[i].x < edges[i].y) ? edges[i].x : edges[i].y);
				@int.y = ((edges[i].x > edges[i].y) ? edges[i].x : edges[i].y);
				edges[i] = @int;
				Tessellator.InsertUniqueEdge(nativeArray, @int, ref num);
			}
			this.m_Edges = new NativeArray<int2>(num, this.m_Allocator, NativeArrayOptions.ClearMemory);
			for (int j = 0; j < num; j++)
			{
				this.m_Edges[j] = nativeArray[j];
			}
			nativeArray.Dispose();
			ModuleHandle.InsertionSort<int2, TessEdgeCompare>(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<int2>(this.m_Edges), 0, this.m_Edges.Length - 1, default(TessEdgeCompare));
			for (int k = 0; k < this.m_StarCount; k++)
			{
				UStar ustar = this.m_Stars[k];
				ustar.points = new ArraySlice<int>(this.m_SPArray, k * this.m_StarCount, this.m_StarCount);
				ustar.pointCount = 0;
				this.m_Stars[k] = ustar;
			}
			for (int l = 0; l < this.m_CellCount; l++)
			{
				int x = this.m_Cells[l].x;
				int y = this.m_Cells[l].y;
				int z = this.m_Cells[l].z;
				UStar ustar2 = this.m_Stars[x];
				UStar ustar3 = this.m_Stars[y];
				UStar ustar4 = this.m_Stars[z];
				int num2 = ustar2.pointCount;
				ustar2.pointCount = num2 + 1;
				ustar2.points[num2] = y;
				num2 = ustar2.pointCount;
				ustar2.pointCount = num2 + 1;
				ustar2.points[num2] = z;
				num2 = ustar3.pointCount;
				ustar3.pointCount = num2 + 1;
				ustar3.points[num2] = z;
				num2 = ustar3.pointCount;
				ustar3.pointCount = num2 + 1;
				ustar3.points[num2] = x;
				num2 = ustar4.pointCount;
				ustar4.pointCount = num2 + 1;
				ustar4.points[num2] = x;
				num2 = ustar4.pointCount;
				ustar4.pointCount = num2 + 1;
				ustar4.points[num2] = y;
				this.m_Stars[x] = ustar2;
				this.m_Stars[y] = ustar3;
				this.m_Stars[z] = ustar4;
			}
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x000442A8 File Offset: 0x000424A8
		private int OppositeOf(int a, int b)
		{
			ArraySlice<int> points = this.m_Stars[b].points;
			int i = 1;
			int pointCount = this.m_Stars[b].pointCount;
			while (i < pointCount)
			{
				if (points[i] == a)
				{
					return points[i - 1];
				}
				i += 2;
			}
			return -1;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x000442FC File Offset: 0x000424FC
		private int FindConstraint(int a, int b)
		{
			int2 @int;
			@int.x = ((a < b) ? a : b);
			@int.y = ((a > b) ? a : b);
			return ModuleHandle.GetEqual<int2, int2, Tessellator.TestEdgePointE>(this.m_Edges, this.m_Edges.Length, @int, default(Tessellator.TestEdgePointE));
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00044348 File Offset: 0x00042548
		private void AddTriangle(int i, int j, int k)
		{
			UStar ustar = this.m_Stars[i];
			UStar ustar2 = this.m_Stars[j];
			UStar ustar3 = this.m_Stars[k];
			int num = ustar.pointCount;
			ustar.pointCount = num + 1;
			ustar.points[num] = j;
			num = ustar.pointCount;
			ustar.pointCount = num + 1;
			ustar.points[num] = k;
			num = ustar2.pointCount;
			ustar2.pointCount = num + 1;
			ustar2.points[num] = k;
			num = ustar2.pointCount;
			ustar2.pointCount = num + 1;
			ustar2.points[num] = i;
			num = ustar3.pointCount;
			ustar3.pointCount = num + 1;
			ustar3.points[num] = i;
			num = ustar3.pointCount;
			ustar3.pointCount = num + 1;
			ustar3.points[num] = j;
			this.m_Stars[i] = ustar;
			this.m_Stars[j] = ustar2;
			this.m_Stars[k] = ustar3;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0004444C File Offset: 0x0004264C
		private void RemovePair(int r, int j, int k)
		{
			UStar ustar = this.m_Stars[r];
			ArraySlice<int> points = ustar.points;
			int i = 1;
			int pointCount = ustar.pointCount;
			while (i < pointCount)
			{
				if (points[i - 1] == j && points[i] == k)
				{
					points[i - 1] = points[pointCount - 2];
					points[i] = points[pointCount - 1];
					ustar.points = points;
					ustar.pointCount -= 2;
					this.m_Stars[r] = ustar;
					return;
				}
				i += 2;
			}
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x000444E3 File Offset: 0x000426E3
		private void RemoveTriangle(int i, int j, int k)
		{
			this.RemovePair(i, j, k);
			this.RemovePair(j, k, i);
			this.RemovePair(k, i, j);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00044500 File Offset: 0x00042700
		private void EdgeFlip(int i, int j)
		{
			int num = this.OppositeOf(i, j);
			int num2 = this.OppositeOf(j, i);
			this.RemoveTriangle(i, j, num);
			this.RemoveTriangle(j, i, num2);
			this.AddTriangle(i, num2, num);
			this.AddTriangle(j, num, num2);
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00044544 File Offset: 0x00042744
		private bool Flip(NativeArray<float2> points, ref NativeArray<int> stack, ref int stackCount, int a, int b, int x)
		{
			int num = this.OppositeOf(a, b);
			if (num < 0)
			{
				return true;
			}
			if (b < a)
			{
				int num2 = a;
				a = b;
				b = num2;
				int num3 = x;
				x = num;
				num = num3;
			}
			if (this.FindConstraint(a, b) != -1)
			{
				return true;
			}
			if (ModuleHandle.IsInsideCircle(points[a], points[b], points[x], points[num]))
			{
				if (2 + stackCount >= stack.Length)
				{
					return false;
				}
				int num4 = stackCount;
				stackCount = num4 + 1;
				stack[num4] = a;
				num4 = stackCount;
				stackCount = num4 + 1;
				stack[num4] = b;
			}
			return true;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x000445E0 File Offset: 0x000427E0
		private NativeArray<int3> GetCells(ref int count)
		{
			NativeArray<int3> nativeArray = new NativeArray<int3>(this.m_NumPoints * (this.m_NumPoints + 1), this.m_Allocator, NativeArrayOptions.ClearMemory);
			count = 0;
			int i = 0;
			int length = this.m_Stars.Length;
			while (i < length)
			{
				ArraySlice<int> points = this.m_Stars[i].points;
				int j = 0;
				int pointCount = this.m_Stars[i].pointCount;
				while (j < pointCount)
				{
					int num = points[j];
					int num2 = points[j + 1];
					if (i < math.min(num, num2))
					{
						int3 @int = default(int3);
						@int.x = i;
						@int.y = num;
						@int.z = num2;
						int num3 = count;
						count = num3 + 1;
						nativeArray[num3] = @int;
					}
					j += 2;
				}
				i++;
			}
			return nativeArray;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x000446C0 File Offset: 0x000428C0
		internal bool ApplyDelaunay(NativeArray<float2> points, NativeArray<int2> edges)
		{
			if (this.m_CellCount == 0)
			{
				return false;
			}
			NativeArray<int> nativeArray = new NativeArray<int>(this.m_NumPoints * (this.m_NumPoints + 1), this.m_Allocator, NativeArrayOptions.ClearMemory);
			int num = 0;
			bool flag = true;
			this.PrepareDelaunay(edges, this.m_NumEdges);
			int num2 = 0;
			while (flag && num2 < this.m_NumPoints)
			{
				UStar ustar = this.m_Stars[num2];
				for (int i = 1; i < ustar.pointCount; i += 2)
				{
					int num3 = ustar.points[i];
					if (num3 >= num2 && this.FindConstraint(num2, num3) < 0)
					{
						int num4 = ustar.points[i - 1];
						int num5 = -1;
						for (int j = 1; j < ustar.pointCount; j += 2)
						{
							if (ustar.points[j - 1] == num3)
							{
								num5 = ustar.points[j];
								break;
							}
						}
						if (num5 >= 0 && ModuleHandle.IsInsideCircle(points[num2], points[num3], points[num4], points[num5]))
						{
							if (2 + num >= nativeArray.Length)
							{
								flag = false;
								break;
							}
							nativeArray[num++] = num2;
							nativeArray[num++] = num3;
						}
					}
				}
				num2++;
			}
			int num6 = this.m_NumPoints * this.m_NumPoints;
			while (num > 0 && flag)
			{
				int num7 = nativeArray[num - 1];
				num--;
				int num8 = nativeArray[num - 1];
				num--;
				int num9 = -1;
				int num10 = -1;
				UStar ustar2 = this.m_Stars[num8];
				for (int k = 1; k < ustar2.pointCount; k += 2)
				{
					int num11 = ustar2.points[k - 1];
					int num12 = ustar2.points[k];
					if (num11 == num7)
					{
						num10 = num12;
					}
					else if (num12 == num7)
					{
						num9 = num11;
					}
				}
				if (num9 >= 0 && num10 >= 0 && ModuleHandle.IsInsideCircle(points[num8], points[num7], points[num9], points[num10]))
				{
					this.EdgeFlip(num8, num7);
					flag = this.Flip(points, ref nativeArray, ref num, num9, num8, num10);
					flag = flag && this.Flip(points, ref nativeArray, ref num, num8, num10, num9);
					flag = flag && this.Flip(points, ref nativeArray, ref num, num10, num7, num9);
					flag = flag && this.Flip(points, ref nativeArray, ref num, num7, num9, num10);
					flag = flag && --num6 > 0;
				}
			}
			nativeArray.Dispose();
			return flag;
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0004497C File Offset: 0x00042B7C
		private int FindNeighbor(NativeArray<int3> cells, int count, int a, int b, int c)
		{
			int num = a;
			int num2 = b;
			int num3 = c;
			if (b < c)
			{
				if (b < a)
				{
					num = b;
					num2 = c;
					num3 = a;
				}
			}
			else if (c < a)
			{
				num = c;
				num2 = a;
				num3 = b;
			}
			if (num < 0)
			{
				return -1;
			}
			int3 @int;
			@int.x = num;
			@int.y = num2;
			@int.z = num3;
			return ModuleHandle.GetEqual<int3, int3, Tessellator.TestCellE>(cells, count, @int, default(Tessellator.TestCellE));
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x000449E4 File Offset: 0x00042BE4
		private NativeArray<int3> Constrain(ref int count)
		{
			NativeArray<int3> cells = this.GetCells(ref count);
			int num = count;
			for (int i = 0; i < num; i++)
			{
				int3 @int = cells[i];
				int x = @int.x;
				int y = @int.y;
				int z = @int.z;
				if (y < z)
				{
					if (y < x)
					{
						@int.x = y;
						@int.y = z;
						@int.z = x;
					}
				}
				else if (z < x)
				{
					@int.x = z;
					@int.y = x;
					@int.z = y;
				}
				cells[i] = @int;
			}
			ModuleHandle.InsertionSort<int3, TessCellCompare>(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<int3>(cells), 0, this.m_CellCount - 1, default(TessCellCompare));
			this.m_Flags = new NativeArray<int>(num, this.m_Allocator, NativeArrayOptions.ClearMemory);
			this.m_Neighbors = new NativeArray<int>(num * 3, this.m_Allocator, NativeArrayOptions.ClearMemory);
			this.m_Constraints = new NativeArray<int>(num * 3, this.m_Allocator, NativeArrayOptions.ClearMemory);
			NativeArray<int> nativeArray = new NativeArray<int>(num * 3, this.m_Allocator, NativeArrayOptions.ClearMemory);
			NativeArray<int> nativeArray2 = new NativeArray<int>(num * 3, this.m_Allocator, NativeArrayOptions.ClearMemory);
			int num2 = 1;
			int num3 = 0;
			int j = 0;
			for (int k = 0; k < num; k++)
			{
				int3 int2 = cells[k];
				for (int l = 0; l < 3; l++)
				{
					int num4 = l;
					int num5 = (l + 1) % 3;
					num4 = ((num4 == 0) ? int2.x : ((l == 1) ? int2.y : int2.z));
					num5 = ((num5 == 0) ? int2.x : ((num5 == 1) ? int2.y : int2.z));
					int num6 = this.OppositeOf(num5, num4);
					int num7 = (this.m_Neighbors[3 * k + l] = this.FindNeighbor(cells, count, num5, num4, num6));
					int num8 = (this.m_Constraints[3 * k + l] = ((-1 != this.FindConstraint(num4, num5)) ? 1 : 0));
					if (num7 < 0)
					{
						if (num8 != 0)
						{
							nativeArray[num3++] = k;
						}
						else
						{
							nativeArray2[j++] = k;
							this.m_Flags[k] = 1;
						}
					}
				}
			}
			while (j > 0 || num3 > 0)
			{
				while (j > 0)
				{
					int num9 = nativeArray2[j - 1];
					j--;
					if (this.m_Flags[num9] != -num2)
					{
						this.m_Flags[num9] = num2;
						int3 int3 = cells[num9];
						for (int m = 0; m < 3; m++)
						{
							int num10 = this.m_Neighbors[3 * num9 + m];
							if (num10 >= 0 && this.m_Flags[num10] == 0)
							{
								if (this.m_Constraints[3 * num9 + m] != 0)
								{
									nativeArray[num3++] = num10;
								}
								else
								{
									nativeArray2[j++] = num10;
									this.m_Flags[num10] = num2;
								}
							}
						}
					}
				}
				for (int n = 0; n < num3; n++)
				{
					nativeArray2[n] = nativeArray[n];
				}
				j = num3;
				num3 = 0;
				num2 = -num2;
			}
			nativeArray2.Dispose();
			nativeArray.Dispose();
			return cells;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00044D44 File Offset: 0x00042F44
		internal NativeArray<int3> RemoveExterior(ref int cellCount)
		{
			int num = 0;
			NativeArray<int3> nativeArray = this.Constrain(ref num);
			NativeArray<int3> nativeArray2 = new NativeArray<int3>(num, this.m_Allocator, NativeArrayOptions.ClearMemory);
			cellCount = 0;
			for (int i = 0; i < num; i++)
			{
				if (this.m_Flags[i] == -1)
				{
					int num2 = cellCount;
					cellCount = num2 + 1;
					nativeArray2[num2] = nativeArray[i];
				}
			}
			nativeArray.Dispose();
			return nativeArray2;
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00044DAC File Offset: 0x00042FAC
		internal NativeArray<int3> RemoveInterior(int cellCount)
		{
			int num = 0;
			NativeArray<int3> nativeArray = this.Constrain(ref num);
			NativeArray<int3> nativeArray2 = new NativeArray<int3>(num, this.m_Allocator, NativeArrayOptions.ClearMemory);
			cellCount = 0;
			for (int i = 0; i < num; i++)
			{
				if (this.m_Flags[i] == 1)
				{
					nativeArray2[cellCount++] = nativeArray[i];
				}
			}
			nativeArray.Dispose();
			return nativeArray2;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00044E10 File Offset: 0x00043010
		internal bool Triangulate(NativeArray<float2> points, int pointCount, NativeArray<int2> edges, int edgeCount)
		{
			this.m_NumEdges = edgeCount;
			this.m_NumHulls = edgeCount * 2;
			this.m_NumPoints = pointCount;
			this.m_CellCount = 0;
			this.m_Cells = new NativeArray<int3>(ModuleHandle.kMaxTriangleCount, this.m_Allocator, NativeArrayOptions.ClearMemory);
			this.m_ILArray = new NativeArray<int>(this.m_NumHulls * (this.m_NumHulls + 1), this.m_Allocator, NativeArrayOptions.ClearMemory);
			this.m_IUArray = new NativeArray<int>(this.m_NumHulls * (this.m_NumHulls + 1), this.m_Allocator, NativeArrayOptions.ClearMemory);
			NativeArray<UHull> nativeArray = new NativeArray<UHull>(this.m_NumPoints * 8, this.m_Allocator, NativeArrayOptions.ClearMemory);
			int num = 0;
			NativeArray<UEvent> nativeArray2 = new NativeArray<UEvent>(this.m_NumPoints + this.m_NumEdges * 2, this.m_Allocator, NativeArrayOptions.ClearMemory);
			int num2 = 0;
			for (int i = 0; i < this.m_NumPoints; i++)
			{
				UEvent uevent = default(UEvent);
				uevent.a = points[i];
				uevent.b = default(float2);
				uevent.idx = i;
				uevent.type = 0;
				nativeArray2[num2++] = uevent;
			}
			for (int j = 0; j < this.m_NumEdges; j++)
			{
				int2 @int = edges[j];
				float2 @float = points[@int.x];
				float2 float2 = points[@int.y];
				if (@float.x < float2.x)
				{
					UEvent uevent2 = default(UEvent);
					uevent2.a = @float;
					uevent2.b = float2;
					uevent2.idx = j;
					uevent2.type = 2;
					UEvent uevent3 = default(UEvent);
					uevent3.a = float2;
					uevent3.b = @float;
					uevent3.idx = j;
					uevent3.type = 1;
					nativeArray2[num2++] = uevent2;
					nativeArray2[num2++] = uevent3;
				}
				else if (@float.x > float2.x)
				{
					UEvent uevent4 = default(UEvent);
					uevent4.a = float2;
					uevent4.b = @float;
					uevent4.idx = j;
					uevent4.type = 2;
					UEvent uevent5 = default(UEvent);
					uevent5.a = @float;
					uevent5.b = float2;
					uevent5.idx = j;
					uevent5.type = 1;
					nativeArray2[num2++] = uevent4;
					nativeArray2[num2++] = uevent5;
				}
			}
			ModuleHandle.InsertionSort<UEvent, TessEventCompare>(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<UEvent>(nativeArray2), 0, num2 - 1, default(TessEventCompare));
			bool flag = true;
			float num3 = nativeArray2[0].a.x - (1f + math.abs(nativeArray2[0].a.x)) * math.pow(2f, -16f);
			UHull uhull;
			uhull.a.x = num3;
			uhull.a.y = 1f;
			uhull.b.x = num3;
			uhull.b.y = 0f;
			uhull.idx = -1;
			uhull.ilarray = new ArraySlice<int>(this.m_ILArray, this.m_NumHulls * this.m_NumHulls, this.m_NumHulls);
			uhull.iuarray = new ArraySlice<int>(this.m_IUArray, this.m_NumHulls * this.m_NumHulls, this.m_NumHulls);
			uhull.ilcount = 0;
			uhull.iucount = 0;
			nativeArray[num++] = uhull;
			int k = 0;
			int num4 = num2;
			while (k < num4)
			{
				int type = nativeArray2[k].type;
				if (type != 0)
				{
					if (type != 2)
					{
						flag = this.MergeHulls(nativeArray, ref num, points, nativeArray2[k]);
					}
					else
					{
						flag = this.SplitHulls(nativeArray, ref num, points, nativeArray2[k]);
					}
				}
				else
				{
					flag = this.AddPoint(nativeArray, num, points, nativeArray2[k].a, nativeArray2[k].idx);
				}
				if (!flag)
				{
					break;
				}
				k++;
			}
			nativeArray2.Dispose();
			nativeArray.Dispose();
			return flag;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0004521C File Offset: 0x0004341C
		internal static bool Tessellate(Allocator allocator, NativeArray<float2> pgPoints, int pgPointCount, NativeArray<int2> pgEdges, int pgEdgeCount, ref NativeArray<float2> outputVertices, ref int vertexCount, ref NativeArray<int> outputIndices, ref int indexCount)
		{
			Tessellator tessellator = default(Tessellator);
			tessellator.SetAllocator(allocator);
			int num = 0;
			int num2 = 0;
			bool flag = tessellator.Triangulate(pgPoints, pgPointCount, pgEdges, pgEdgeCount);
			flag = flag && tessellator.ApplyDelaunay(pgPoints, pgEdges);
			if (flag)
			{
				NativeArray<int3> nativeArray = tessellator.RemoveExterior(ref num2);
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = (ushort)nativeArray[i].x;
					ushort num4 = (ushort)nativeArray[i].y;
					ushort num5 = (ushort)nativeArray[i].z;
					if (num3 != num4 && num4 != num5 && num3 != num5)
					{
						int num6 = indexCount;
						indexCount = num6 + 1;
						outputIndices[num6] = (int)num3;
						num6 = indexCount;
						indexCount = num6 + 1;
						outputIndices[num6] = (int)num5;
						num6 = indexCount;
						indexCount = num6 + 1;
						outputIndices[num6] = (int)num4;
					}
					num = math.max(math.max(math.max(nativeArray[i].x, nativeArray[i].y), nativeArray[i].z), num);
				}
				num = ((num != 0) ? (num + 1) : 0);
				for (int j = 0; j < num; j++)
				{
					int num6 = vertexCount;
					vertexCount = num6 + 1;
					outputVertices[num6] = pgPoints[j];
				}
				nativeArray.Dispose();
			}
			tessellator.Cleanup();
			return flag;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00045390 File Offset: 0x00043590
		internal void Cleanup()
		{
			if (this.m_Edges.IsCreated)
			{
				this.m_Edges.Dispose();
			}
			if (this.m_Stars.IsCreated)
			{
				this.m_Stars.Dispose();
			}
			if (this.m_SPArray.IsCreated)
			{
				this.m_SPArray.Dispose();
			}
			if (this.m_Cells.IsCreated)
			{
				this.m_Cells.Dispose();
			}
			if (this.m_ILArray.IsCreated)
			{
				this.m_ILArray.Dispose();
			}
			if (this.m_IUArray.IsCreated)
			{
				this.m_IUArray.Dispose();
			}
			if (this.m_Flags.IsCreated)
			{
				this.m_Flags.Dispose();
			}
			if (this.m_Neighbors.IsCreated)
			{
				this.m_Neighbors.Dispose();
			}
			if (this.m_Constraints.IsCreated)
			{
				this.m_Constraints.Dispose();
			}
		}

		// Token: 0x040008E0 RID: 2272
		private NativeArray<int2> m_Edges;

		// Token: 0x040008E1 RID: 2273
		private NativeArray<UStar> m_Stars;

		// Token: 0x040008E2 RID: 2274
		private NativeArray<int3> m_Cells;

		// Token: 0x040008E3 RID: 2275
		private int m_CellCount;

		// Token: 0x040008E4 RID: 2276
		private NativeArray<int> m_ILArray;

		// Token: 0x040008E5 RID: 2277
		private NativeArray<int> m_IUArray;

		// Token: 0x040008E6 RID: 2278
		private NativeArray<int> m_SPArray;

		// Token: 0x040008E7 RID: 2279
		private int m_NumEdges;

		// Token: 0x040008E8 RID: 2280
		private int m_NumHulls;

		// Token: 0x040008E9 RID: 2281
		private int m_NumPoints;

		// Token: 0x040008EA RID: 2282
		private int m_StarCount;

		// Token: 0x040008EB RID: 2283
		private NativeArray<int> m_Flags;

		// Token: 0x040008EC RID: 2284
		private NativeArray<int> m_Neighbors;

		// Token: 0x040008ED RID: 2285
		private NativeArray<int> m_Constraints;

		// Token: 0x040008EE RID: 2286
		private Allocator m_Allocator;

		// Token: 0x020001F9 RID: 505
		private struct TestHullPointL : ICondition2<UHull, float2>
		{
			// Token: 0x06000BB0 RID: 2992 RVA: 0x0004CA0F File Offset: 0x0004AC0F
			public bool Test(UHull h, float2 p, ref float t)
			{
				t = ModuleHandle.OrientFast(h.a, h.b, p);
				return t < 0f;
			}
		}

		// Token: 0x020001FA RID: 506
		private struct TestHullPointU : ICondition2<UHull, float2>
		{
			// Token: 0x06000BB1 RID: 2993 RVA: 0x0004CA2E File Offset: 0x0004AC2E
			public bool Test(UHull h, float2 p, ref float t)
			{
				t = ModuleHandle.OrientFast(h.a, h.b, p);
				return t > 0f;
			}
		}

		// Token: 0x020001FB RID: 507
		private struct TestHullEventLe : ICondition2<UHull, UEvent>
		{
			// Token: 0x06000BB2 RID: 2994 RVA: 0x0004CA4D File Offset: 0x0004AC4D
			public bool Test(UHull h, UEvent p, ref float t)
			{
				t = Tessellator.FindSplit(h, p);
				return t <= 0f;
			}
		}

		// Token: 0x020001FC RID: 508
		private struct TestHullEventE : ICondition2<UHull, UEvent>
		{
			// Token: 0x06000BB3 RID: 2995 RVA: 0x0004CA64 File Offset: 0x0004AC64
			public bool Test(UHull h, UEvent p, ref float t)
			{
				t = Tessellator.FindSplit(h, p);
				return t == 0f;
			}
		}

		// Token: 0x020001FD RID: 509
		private struct TestEdgePointE : ICondition2<int2, int2>
		{
			// Token: 0x06000BB4 RID: 2996 RVA: 0x0004CA78 File Offset: 0x0004AC78
			public bool Test(int2 h, int2 p, ref float t)
			{
				t = (float)default(TessEdgeCompare).Compare(h, p);
				return t == 0f;
			}
		}

		// Token: 0x020001FE RID: 510
		private struct TestCellE : ICondition2<int3, int3>
		{
			// Token: 0x06000BB5 RID: 2997 RVA: 0x0004CAA4 File Offset: 0x0004ACA4
			public bool Test(int3 h, int3 p, ref float t)
			{
				t = (float)default(TessCellCompare).Compare(h, p);
				return t == 0f;
			}
		}
	}
}
