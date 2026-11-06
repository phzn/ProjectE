using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000157 RID: 343
	public static class GridStringPulling
	{
		// Token: 0x06000AF7 RID: 2807 RVA: 0x0003AC9C File Offset: 0x00038E9C
		private static long Cross(int2 lhs, int2 rhs)
		{
			return (long)lhs.x * (long)rhs.y - (long)lhs.y * (long)rhs.x;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0003ACBD File Offset: 0x00038EBD
		private static long Dot(int2 a, int2 b)
		{
			return (long)a.x * (long)b.x + (long)a.y * (long)b.y;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0003ACE0 File Offset: 0x00038EE0
		private static bool RightOrColinear(int2 a, int2 b, int2 p)
		{
			return (long)(b.x - a.x) * (long)(p.y - a.y) - (long)(p.x - a.x) * (long)(b.y - a.y) <= 0L;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0003AD2F File Offset: 0x00038F2F
		private static int2 Perpendicular(int2 v)
		{
			return new int2(-v.y, v.x);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003AD43 File Offset: 0x00038F43
		private static int2 ToFixedPrecision(Vector2 p)
		{
			return new int2(math.round(new float2(p) * 1024f));
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003AD64 File Offset: 0x00038F64
		private static Vector2 FromFixedPrecision(int2 p)
		{
			return p * 0.0009765625f;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0003AD7C File Offset: 0x00038F7C
		private static Side Side2D(int2 a, int2 b, int2 p)
		{
			long num = GridStringPulling.Cross(b - a, p - a);
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

		// Token: 0x06000AFE RID: 2814 RVA: 0x0003ADAC File Offset: 0x00038FAC
		public static float IntersectionLength(int2 nodeCenter, int2 segmentStart, int2 segmentEnd)
		{
			float2 @float = math.rcp(segmentEnd - segmentStart);
			float num = math.length(segmentEnd - segmentStart);
			float num2 = float.NegativeInfinity;
			float num3 = float.PositiveInfinity;
			int2 @int = segmentEnd - segmentStart;
			int2 int2 = nodeCenter + new int2(1024, 1024);
			if ((double)@int.x != 0.0)
			{
				float num4 = (float)(nodeCenter.x - segmentStart.x) * @float.x;
				float num5 = (float)(int2.x - segmentStart.x) * @float.x;
				num2 = math.max(num2, math.min(num4, num5));
				num3 = math.min(num3, math.max(num4, num5));
			}
			else if (segmentStart.x < nodeCenter.x || segmentStart.x > int2.x)
			{
				return 0f;
			}
			if ((double)@int.y != 0.0)
			{
				float num6 = (float)(nodeCenter.y - segmentStart.y) * @float.y;
				float num7 = (float)(int2.y - segmentStart.y) * @float.y;
				num2 = math.max(num2, math.min(num6, num7));
				num3 = math.min(num3, math.max(num6, num7));
			}
			else if (segmentStart.y < nodeCenter.y || segmentStart.y > int2.y)
			{
				return 0f;
			}
			num2 = math.max(0f, num2);
			num3 = math.min(1f, num3);
			return math.max(num3 - num2, 0f) * num * 0.0009765625f;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00003788 File Offset: 0x00001988
		internal static void TestIntersectionLength()
		{
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0003AF4C File Offset: 0x0003914C
		private static uint LinecastCost(List<GraphNode> trace, int2 segmentStart, int2 segmentEnd, GridGraph gg, ref TraversalCosts traversalCosts)
		{
			uint num = 0U;
			for (int i = 0; i < trace.Count; i++)
			{
				GridNodeBase gridNodeBase = trace[i] as GridNodeBase;
				if (i > 0)
				{
					num += traversalCosts.GetConnectionCost(trace[i - 1], trace[i]);
				}
				num += (uint)(traversalCosts.GetTraversalCostMultiplier(gridNodeBase) * gg.nodeSize * 1000f * GridStringPulling.IntersectionLength(new int2(gridNodeBase.XCoordinateInGrid, gridNodeBase.ZCoordinateInGrid) * 1024, segmentStart, segmentEnd));
			}
			return num;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0003AFD4 File Offset: 0x000391D4
		public static List<Vector3> Calculate(List<GraphNode> pathNodes, int nodeStartIndex, int nodeEndIndex, Vector3 startPoint, Vector3 endPoint, ref TraversalCosts traversalCosts, ref TraversalConstraint traversalConstraint, int maxCorners = 2147483647)
		{
			List<int> list = ListPool<int>.Claim();
			list.Add(0);
			int num = nodeEndIndex - nodeStartIndex + 1;
			GridGraph gridGraph = pathNodes[nodeStartIndex].Graph as GridGraph;
			List<GraphNode> list2 = ListPool<GraphNode>.Claim();
			Side side = Side.Colinear;
			int num2 = 0;
			num += 2;
			int num3 = num;
			GridNodeBase[] array = ArrayPool<GridNodeBase>.Claim(num3 * 2);
			int2[] array2 = ArrayPool<int2>.Claim(num3 * 2);
			int2[] array3 = ArrayPool<int2>.Claim(num3 * 2);
			uint[] array4 = ArrayPool<uint>.Claim(num3 * 2);
			uint num4 = 0U;
			for (int i = 0; i < num; i++)
			{
				GridNodeBase gridNodeBase = (array[i] = pathNodes[math.clamp(nodeStartIndex + i - 1, nodeStartIndex, nodeEndIndex)] as GridNodeBase);
				int2 @int = new int2(gridNodeBase.XCoordinateInGrid, gridNodeBase.ZCoordinateInGrid);
				int2 int2 = @int * 1024;
				int2 int3;
				if (i == 0)
				{
					int3 = GridStringPulling.ToFixedPrecision(gridNodeBase.NormalizePoint(startPoint));
					int3 = math.clamp(int3, int2.zero, new int2(1024, 1024));
				}
				else if (i == num - 1)
				{
					int3 = GridStringPulling.ToFixedPrecision(gridNodeBase.NormalizePoint(endPoint));
					int3 = math.clamp(int3, int2.zero, new int2(1024, 1024));
				}
				else
				{
					int3 = new int2(512, 512);
				}
				array2[i] = int2 + int3;
				array3[i] = int3;
				if (i > 0)
				{
					if (array[i - 1] != array[i])
					{
						num4 += traversalCosts.GetConnectionCost(array[i - 1], array[i]);
					}
					num4 += (uint)(traversalCosts.GetTraversalCostMultiplier(array[i - 1]) * gridGraph.nodeSize * 1000f * GridStringPulling.IntersectionLength(new int2(array[i - 1].XCoordinateInGrid, array[i - 1].ZCoordinateInGrid) * 1024, array2[i - 1], array2[i]));
					num4 += (uint)(traversalCosts.GetTraversalCostMultiplier(array[i]) * gridGraph.nodeSize * 1000f * GridStringPulling.IntersectionLength(@int * 1024, array2[i - 1], array2[i]));
				}
				array4[i] = num4;
			}
			int num5 = 0;
			int num6 = 1;
			int j = 1;
			while (j < num)
			{
				if (list.Count < maxCorners)
				{
					num2++;
					if (num2 <= 10000)
					{
						int num7 = list[list.Count - 1];
						int2 int4 = array3[num7];
						int num8 = ((list.Count > 1) ? list[list.Count - 2] : (-1));
						GridNodeBase gridNodeBase2 = array[num7];
						int num9 = num - j - 1;
						int num10 = 0;
						int k = math.min(4, num9);
						for (;;)
						{
							int num11 = j + k;
							if (list.Count > 1 && GridStringPulling.Side2D(array2[num8], array2[num7], array2[num11]) != side)
							{
								goto Block_11;
							}
							list2.Clear();
							if (num11 != num7 + 1)
							{
								GridHitInfo gridHitInfo;
								if (gridGraph.Linecast(gridNodeBase2, int4, array[num11], array3[num11], out gridHitInfo, ref traversalConstraint, list2, false))
								{
									goto Block_13;
								}
								uint num12 = GridStringPulling.LinecastCost(list2, array2[num7], array2[num11], gridGraph, ref traversalCosts);
								if (num12 > array4[num11] - array4[num7] + 5U)
								{
									goto Block_14;
								}
							}
							if (k >= num9)
							{
								goto IL_039B;
							}
							num10 = k;
							k = math.min(k * 2, num9);
						}
						IL_039E:
						GridStringPulling.PredicateFailMode predicateFailMode;
						if (predicateFailMode == GridStringPulling.PredicateFailMode.ReachedEnd)
						{
							list.Add(num - 1);
							goto IL_094A;
						}
						while (k > num10 + 1)
						{
							int num13 = (num10 + k) / 2;
							int num14 = j + num13;
							bool flag;
							if (flag = list.Count > 1 && GridStringPulling.Side2D(array2[num8], array2[num7], array2[num14]) != side)
							{
								predicateFailMode = GridStringPulling.PredicateFailMode.Turn;
							}
							else
							{
								list2.Clear();
								if (num14 != num7 + 1)
								{
									GridHitInfo gridHitInfo;
									if (gridGraph.Linecast(gridNodeBase2, int4, array[num14], array3[num14], out gridHitInfo, ref traversalConstraint, list2, false))
									{
										predicateFailMode = GridStringPulling.PredicateFailMode.LinecastObstacle;
										flag = true;
									}
									else
									{
										uint num15 = GridStringPulling.LinecastCost(list2, array2[num7], array2[num14], gridGraph, ref traversalCosts);
										if (num15 > array4[num14] - array4[num7] + 5U)
										{
											predicateFailMode = GridStringPulling.PredicateFailMode.LinecastCost;
											flag = true;
										}
									}
								}
							}
							if (flag)
							{
								k = num13;
							}
							else
							{
								num10 = num13;
							}
						}
						if (num10 > 0)
						{
							num5 = num7;
							num6 = j + num10;
						}
						else
						{
							bool flag2;
							if (!(flag2 = list.Count > 1 && GridStringPulling.Side2D(array2[num8], array2[num7], array2[j + num10]) != side))
							{
								list2.Clear();
								if (j + num10 != num7 + 1)
								{
									GridHitInfo gridHitInfo;
									if (gridGraph.Linecast(gridNodeBase2, int4, array[j + num10], array3[j + num10], out gridHitInfo, ref traversalConstraint, list2, false))
									{
										flag2 = true;
									}
									else
									{
										uint num16 = GridStringPulling.LinecastCost(list2, array2[num7], array2[j + num10], gridGraph, ref traversalCosts);
										if (num16 > array4[j + num10] - array4[num7] + 5U)
										{
											flag2 = true;
										}
									}
								}
							}
							if (!flag2)
							{
								num5 = num7;
								num6 = j + num10;
							}
						}
						j += k;
						list2.Clear();
						list2.Clear();
						if (predicateFailMode == GridStringPulling.PredicateFailMode.LinecastCost)
						{
							list.Add(num6);
							side = GridStringPulling.Side2D(array2[num7], array2[num6], array2[j]);
							num5 = num6;
							j--;
							continue;
						}
						if (predicateFailMode == GridStringPulling.PredicateFailMode.LinecastObstacle)
						{
							list2.Clear();
							GridHitInfo gridHitInfo;
							int num17;
							if (gridGraph.Linecast(array[num5], array3[num5], array[num6], array3[num6], out gridHitInfo, ref traversalConstraint, list2, false))
							{
								num17 = num6;
								Debug.LogError("Inconsistent linecasts");
							}
							else
							{
								list2.Add(array[j]);
								GridNodeBase gridNodeBase3 = null;
								int2 int5 = default(int2);
								uint num18 = 0U;
								int2 int6 = default(int2);
								int2 int7 = array2[num5];
								int2 int8 = array2[num6];
								int2 int9 = int8 - int7;
								GridStringPulling.TriangleBounds triangleBounds = new GridStringPulling.TriangleBounds(int7, int8, array2[j]);
								int num19 = Math.Sign(GridStringPulling.Cross(int9, array2[j] - int7));
								uint num20 = array4[num5];
								for (int l = 0; l < list2.Count; l++)
								{
									GridNodeBase gridNodeBase4 = list2[l] as GridNodeBase;
									int2 int10 = new int2(gridNodeBase4.XCoordinateInGrid, gridNodeBase4.ZCoordinateInGrid) * 1024;
									if (l > 0)
									{
										num20 += traversalCosts.GetConnectionCost(list2[l - 1], list2[l]);
									}
									num20 += (uint)(traversalCosts.GetTraversalCostMultiplier(gridNodeBase4) * gridGraph.nodeSize * 1000f * GridStringPulling.IntersectionLength(int10, int7, int8));
									for (int m = 0; m < 4; m++)
									{
										if (!gridNodeBase4.HasConnectionInDirection(m) || !traversalConstraint.CanTraverse(gridNodeBase4, gridNodeBase4.GetNeighbourAlongDirection(m)))
										{
											for (int n = 0; n < 2; n++)
											{
												int2 int11 = GridStringPulling.directionToCorners[(m + n) & 3];
												int2 int12 = int10 + int11;
												if (triangleBounds.Contains(int12))
												{
													int2 int13 = int12 - int7;
													if (!math.all(int13 == 0) && !math.all(int12 == int8))
													{
														long num21 = GridStringPulling.Cross(int13, int6);
														if (gridNodeBase3 == null || Math.Sign(num21) == num19 || (num21 == 0L && math.lengthsq(int13) > math.lengthsq(int6)))
														{
															int6 = int13;
															gridNodeBase3 = gridNodeBase4;
															int5 = int11;
															num18 = num20;
														}
													}
												}
											}
										}
									}
								}
								if (gridNodeBase3 == null)
								{
									num17 = num6;
								}
								else
								{
									num17 = num3;
									array[num3] = gridNodeBase3;
									array3[num3] = int5;
									int2 int14 = new int2(gridNodeBase3.XCoordinateInGrid, gridNodeBase3.ZCoordinateInGrid);
									array2[num3] = int14 * 1024 + int5;
									array4[num3] = num18;
									num3++;
								}
							}
							list.Add(num17);
							side = GridStringPulling.Side2D(array2[num7], array2[num17], array2[j]);
							num5 = num17;
							j--;
							continue;
						}
						num5 = num7;
						num6 = j;
						if (list.Count <= 1)
						{
							continue;
						}
						int num22 = list[list.Count - 2];
						Side side2 = GridStringPulling.Side2D(array2[num22], array2[num7], array2[j]);
						if (side != side2)
						{
							num5 = list[list.Count - 2];
							num6 = list[list.Count - 1];
							list.RemoveAt(list.Count - 1);
							if (list.Count > 1)
							{
								num7 = num22;
								num22 = list[list.Count - 2];
								side = GridStringPulling.Side2D(array2[num22], array2[num7], array2[j]);
							}
							else
							{
								side = Side.Colinear;
							}
							j--;
							continue;
						}
						continue;
						Block_11:
						predicateFailMode = GridStringPulling.PredicateFailMode.Turn;
						goto IL_039E;
						Block_13:
						predicateFailMode = GridStringPulling.PredicateFailMode.LinecastObstacle;
						goto IL_039E;
						Block_14:
						predicateFailMode = GridStringPulling.PredicateFailMode.LinecastCost;
						goto IL_039E;
						IL_039B:
						predicateFailMode = GridStringPulling.PredicateFailMode.ReachedEnd;
						goto IL_039E;
					}
					Debug.LogError("Inf loop");
				}
				IL_094A:
				List<Vector3> list3 = ListPool<Vector3>.Claim(list.Count);
				for (int num23 = 0; num23 < list.Count; num23++)
				{
					int num24 = list[num23];
					list3.Add(array[num24].UnNormalizePoint(GridStringPulling.FromFixedPrecision(array3[num24])));
				}
				ArrayPool<GridNodeBase>.Release(ref array, false);
				ArrayPool<int2>.Release(ref array2, false);
				ArrayPool<int2>.Release(ref array3, false);
				ArrayPool<uint>.Release(ref array4, false);
				ListPool<int>.Release(ref list);
				ListPool<GraphNode>.Release(ref list2);
				return list3;
			}
			list.Add(num - 1);
			goto IL_094A;
		}

		// Token: 0x0400071D RID: 1821
		private static int2[] directionToCorners = new int2[]
		{
			new int2(0, 0),
			new int2(1024, 0),
			new int2(1024, 1024),
			new int2(0, 1024)
		};

		// Token: 0x0400071E RID: 1822
		private const int FixedPrecisionScale = 1024;

		// Token: 0x0400071F RID: 1823
		private static ProfilerMarker marker1 = new ProfilerMarker("Linecast hit");

		// Token: 0x04000720 RID: 1824
		private static ProfilerMarker marker2 = new ProfilerMarker("Linecast success");

		// Token: 0x04000721 RID: 1825
		private static ProfilerMarker marker3 = new ProfilerMarker("Trace");

		// Token: 0x04000722 RID: 1826
		private static ProfilerMarker marker4 = new ProfilerMarker("Neighbours");

		// Token: 0x04000723 RID: 1827
		private static ProfilerMarker marker5 = new ProfilerMarker("Re-evaluate linecast");

		// Token: 0x04000724 RID: 1828
		private static ProfilerMarker marker6 = new ProfilerMarker("Init");

		// Token: 0x04000725 RID: 1829
		private static ProfilerMarker marker7 = new ProfilerMarker("Initloop");

		// Token: 0x02000158 RID: 344
		private struct TriangleBounds
		{
			// Token: 0x06000B03 RID: 2819 RVA: 0x0003BA74 File Offset: 0x00039C74
			public TriangleBounds(int2 p1, int2 p2, int2 p3)
			{
				if (GridStringPulling.RightOrColinear(p1, p2, p3))
				{
					int2 @int = p3;
					p3 = p1;
					p1 = @int;
				}
				this.d1 = GridStringPulling.Perpendicular(p2 - p1);
				this.d2 = GridStringPulling.Perpendicular(p3 - p2);
				this.d3 = GridStringPulling.Perpendicular(p1 - p3);
				this.t1 = GridStringPulling.Dot(this.d1, p1);
				this.t2 = GridStringPulling.Dot(this.d2, p2);
				this.t3 = GridStringPulling.Dot(this.d3, p3);
			}

			// Token: 0x06000B04 RID: 2820 RVA: 0x0003BB00 File Offset: 0x00039D00
			public bool Contains(int2 p)
			{
				return GridStringPulling.Dot(this.d1, p) >= this.t1 && GridStringPulling.Dot(this.d2, p) >= this.t2 && GridStringPulling.Dot(this.d3, p) >= this.t3;
			}

			// Token: 0x04000726 RID: 1830
			private int2 d1;

			// Token: 0x04000727 RID: 1831
			private int2 d2;

			// Token: 0x04000728 RID: 1832
			private int2 d3;

			// Token: 0x04000729 RID: 1833
			private long t1;

			// Token: 0x0400072A RID: 1834
			private long t2;

			// Token: 0x0400072B RID: 1835
			private long t3;
		}

		// Token: 0x02000159 RID: 345
		private enum PredicateFailMode
		{
			// Token: 0x0400072D RID: 1837
			Undefined,
			// Token: 0x0400072E RID: 1838
			Turn,
			// Token: 0x0400072F RID: 1839
			LinecastObstacle,
			// Token: 0x04000730 RID: 1840
			LinecastCost,
			// Token: 0x04000731 RID: 1841
			ReachedEnd
		}
	}
}
