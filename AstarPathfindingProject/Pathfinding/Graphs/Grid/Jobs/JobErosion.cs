using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200022A RID: 554
	[BurstCompile]
	public struct JobErosion<AdjacencyMapper> : IJob where AdjacencyMapper : GridAdjacencyMapper, new()
	{
		// Token: 0x06000E36 RID: 3638 RVA: 0x000575A8 File Offset: 0x000557A8
		public void Execute()
		{
			Slice3D slice3D = new Slice3D(this.bounds, this.bounds);
			int3 size = slice3D.slice.size;
			slice3D.AssertMatchesOuter<ulong>(this.nodeConnections);
			slice3D.AssertMatchesOuter<bool>(this.nodeWalkable);
			slice3D.AssertMatchesOuter<bool>(this.outNodeWalkable);
			slice3D.AssertMatchesOuter<int>(this.nodeTags);
			ValueTuple<int, int, int> outerStrides = slice3D.outerStrides;
			int item = outerStrides.Item1;
			int item2 = outerStrides.Item2;
			int item3 = outerStrides.Item3;
			ValueTuple<int, int, int> innerStrides = slice3D.innerStrides;
			int item4 = innerStrides.Item1;
			int item5 = innerStrides.Item2;
			int item6 = innerStrides.Item3;
			NativeArray<int> nativeArray = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < 8; i++)
			{
				nativeArray[i] = GridGraph.neighbourZOffsets[i] * item6 + GridGraph.neighbourXOffsets[i] * item4;
			}
			NativeArray<int> nativeArray2 = new NativeArray<int>(slice3D.length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			AdjacencyMapper adjacencyMapper = new AdjacencyMapper();
			int num = adjacencyMapper.LayerCount(slice3D.slice);
			int outerStartIndex = slice3D.outerStartIndex;
			if (this.neighbours == NumNeighbours.Six)
			{
				for (int j = 1; j < size.z - 1; j++)
				{
					for (int k = 1; k < size.x - 1; k++)
					{
						for (int l = 0; l < num; l++)
						{
							int num2 = j * item3 + k * item + l * item2 + outerStartIndex;
							int num3 = j * item6 + k * item4;
							int num4 = num3 + l * item5;
							int num5 = int.MaxValue;
							for (int m = 3; m < 6; m++)
							{
								int num6 = JobErosion<AdjacencyMapper>.hexagonNeighbourIndices[m];
								if (!adjacencyMapper.HasConnection(num2, num6, this.nodeConnections))
								{
									num5 = -1;
								}
								else
								{
									num5 = math.min(num5, nativeArray2[adjacencyMapper.GetNeighbourIndex(num3, num4, num6, this.nodeConnections, nativeArray, item5)]);
								}
							}
							nativeArray2[num4] = num5 + 1;
						}
					}
				}
				for (int n = size.z - 2; n > 0; n--)
				{
					for (int num7 = size.x - 2; num7 > 0; num7--)
					{
						for (int num8 = 0; num8 < num; num8++)
						{
							int num9 = n * item3 + num7 * item + num8 * item2 + outerStartIndex;
							int num10 = n * item6 + num7 * item4;
							int num11 = num10 + num8 * item5;
							int num12 = int.MaxValue;
							for (int num13 = 3; num13 < 6; num13++)
							{
								int num14 = JobErosion<AdjacencyMapper>.hexagonNeighbourIndices[num13];
								if (!adjacencyMapper.HasConnection(num9, num14, this.nodeConnections))
								{
									num12 = -1;
								}
								else
								{
									num12 = math.min(num12, nativeArray2[adjacencyMapper.GetNeighbourIndex(num10, num11, num14, this.nodeConnections, nativeArray, item5)]);
								}
							}
							nativeArray2[num11] = math.min(nativeArray2[num11], num12 + 1);
						}
					}
				}
			}
			else
			{
				for (int num15 = 1; num15 < size.z - 1; num15++)
				{
					for (int num16 = 1; num16 < size.x - 1; num16++)
					{
						for (int num17 = 0; num17 < num; num17++)
						{
							int num18 = num15 * item3 + num16 * item + num17 * item2 + outerStartIndex;
							int num19 = num15 * item6 + num16 * item4;
							int num20 = num19 + num17 * item5;
							int num21 = -1;
							if (adjacencyMapper.HasConnection(num18, 0, this.nodeConnections))
							{
								num21 = nativeArray2[adjacencyMapper.GetNeighbourIndex(num19, num20, 0, this.nodeConnections, nativeArray, item5)];
							}
							int num22 = -1;
							if (adjacencyMapper.HasConnection(num18, 3, this.nodeConnections))
							{
								num22 = nativeArray2[adjacencyMapper.GetNeighbourIndex(num19, num20, 3, this.nodeConnections, nativeArray, item5)];
							}
							nativeArray2[num20] = math.min(num21, num22) + 1;
						}
					}
				}
				for (int num23 = size.z - 2; num23 > 0; num23--)
				{
					for (int num24 = size.x - 2; num24 > 0; num24--)
					{
						for (int num25 = 0; num25 < num; num25++)
						{
							int num26 = num23 * item3 + num24 * item + num25 * item2 + outerStartIndex;
							int num27 = num23 * item6 + num24 * item4;
							int num28 = num27 + num25 * item5;
							int num29 = -1;
							if (adjacencyMapper.HasConnection(num26, 2, this.nodeConnections))
							{
								num29 = nativeArray2[adjacencyMapper.GetNeighbourIndex(num27, num28, 2, this.nodeConnections, nativeArray, item5)];
							}
							int num30 = -1;
							if (adjacencyMapper.HasConnection(num26, 1, this.nodeConnections))
							{
								num30 = nativeArray2[adjacencyMapper.GetNeighbourIndex(num27, num28, 1, this.nodeConnections, nativeArray, item5)];
							}
							nativeArray2[num28] = math.min(nativeArray2[num26], math.min(num29, num30) + 1);
						}
					}
				}
			}
			IntBounds intBounds = this.writeMask.Offset(-this.bounds.min);
			for (int num31 = this.erosionStartTag; num31 < this.erosionStartTag + this.erosion; num31++)
			{
				this.erosionTagsPrecedenceMask |= 1 << num31;
			}
			for (int num32 = intBounds.min.y; num32 < intBounds.max.y; num32++)
			{
				for (int num33 = intBounds.min.z; num33 < intBounds.max.z; num33++)
				{
					for (int num34 = intBounds.min.x; num34 < intBounds.max.x; num34++)
					{
						int num35 = num34 * item + num32 * item2 + num33 * item3 + outerStartIndex;
						int num36 = num34 * item4 + num32 * item5 + num33 * item6;
						if (this.erosionUsesTags)
						{
							int num37 = this.nodeTags[num35];
							this.outNodeWalkable[num35] = this.nodeWalkable[num35];
							if (nativeArray2[num36] < this.erosion)
							{
								if (((this.erosionTagsPrecedenceMask >> num37) & 1) != 0)
								{
									this.nodeTags[num35] = (this.nodeWalkable[num35] ? math.min(31, nativeArray2[num36] + this.erosionStartTag) : 0);
								}
							}
							else if (num37 >= this.erosionStartTag && num37 < this.erosionStartTag + this.erosion)
							{
								this.nodeTags[num35] = 0;
							}
						}
						else
						{
							this.outNodeWalkable[num35] = this.nodeWalkable[num35] & (nativeArray2[num36] >= this.erosion);
						}
					}
				}
			}
		}

		// Token: 0x04000A65 RID: 2661
		public IntBounds bounds;

		// Token: 0x04000A66 RID: 2662
		public IntBounds writeMask;

		// Token: 0x04000A67 RID: 2663
		public NumNeighbours neighbours;

		// Token: 0x04000A68 RID: 2664
		public int erosion;

		// Token: 0x04000A69 RID: 2665
		public bool erosionUsesTags;

		// Token: 0x04000A6A RID: 2666
		public int erosionStartTag;

		// Token: 0x04000A6B RID: 2667
		[ReadOnly]
		public NativeArray<ulong> nodeConnections;

		// Token: 0x04000A6C RID: 2668
		[ReadOnly]
		public NativeArray<bool> nodeWalkable;

		// Token: 0x04000A6D RID: 2669
		[WriteOnly]
		public NativeArray<bool> outNodeWalkable;

		// Token: 0x04000A6E RID: 2670
		public NativeArray<int> nodeTags;

		// Token: 0x04000A6F RID: 2671
		public int erosionTagsPrecedenceMask;

		// Token: 0x04000A70 RID: 2672
		private static readonly int[] hexagonNeighbourIndices = new int[] { 1, 2, 5, 0, 3, 7 };
	}
}
