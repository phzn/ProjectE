using System;
using Pathfinding.Collections;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x0200022B RID: 555
	[BurstCompile]
	public struct JobFilterDiagonalConnections : IJobParallelForBatched
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x0001AA07 File Offset: 0x00018C07
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00057CEC File Offset: 0x00055EEC
		public unsafe void Execute(int start, int count)
		{
			this.slice.AssertMatchesOuter<ulong>(this.nodeConnections);
			int3 outerSize = this.slice.outerSize;
			NativeArray<int> nativeArray = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < 8; i++)
			{
				nativeArray[i] = GridGraph.neighbourZOffsets[i] * outerSize.x + GridGraph.neighbourXOffsets[i];
			}
			ulong num = 0UL;
			for (int j = 0; j < GridGraph.hexagonNeighbourIndices.Length; j++)
			{
				num |= 15UL << 4 * GridGraph.hexagonNeighbourIndices[j];
			}
			int num2 = (this.cutCorners ? 1 : 2);
			int num3 = outerSize.x * outerSize.z;
			start += this.slice.slice.min.z;
			for (int k = this.slice.slice.min.y; k < this.slice.slice.max.y; k++)
			{
				for (int l = start; l < start + count; l++)
				{
					for (int m = this.slice.slice.min.x; m < this.slice.slice.max.x; m++)
					{
						int num4 = l * outerSize.x + m;
						int num5 = num4 + k * num3;
						switch (this.neighbours)
						{
						case NumNeighbours.Four:
							*this.nodeConnections[num5] = *this.nodeConnections[num5] | (ulong)(-65536);
							break;
						case NumNeighbours.Eight:
						{
							ulong num6 = *this.nodeConnections[num5];
							if (num6 != (ulong)(-1))
							{
								for (int n = 0; n < 4; n++)
								{
									int num7 = 0;
									ulong num8 = (num6 >> n * 4) & 15UL;
									ulong num9 = (num6 >> (n + 1) % 4 * 4) & 15UL;
									ulong num10 = (num6 >> (n + 4) * 4) & 15UL;
									if (num10 != 15UL)
									{
										if (num8 != 15UL)
										{
											int num11 = (n + 1) % 4;
											int num12 = num4 + nativeArray[n] + (int)num8 * num3;
											if (((*this.nodeConnections[num12] >> num11 * 4) & 15UL) == num10)
											{
												num7++;
											}
										}
										if (num9 != 15UL)
										{
											int num13 = n;
											int num14 = num4 + nativeArray[(n + 1) % 4] + (int)num9 * num3;
											if (((*this.nodeConnections[num14] >> num13 * 4) & 15UL) == num10)
											{
												num7++;
											}
										}
										if (num7 < num2)
										{
											num6 |= 15UL << (n + 4) * 4;
										}
									}
								}
								*this.nodeConnections[num5] = num6;
							}
							break;
						}
						case NumNeighbours.Six:
							*this.nodeConnections[num5] = (*this.nodeConnections[num5] | ~num) & (ulong)(-1);
							break;
						}
					}
				}
			}
		}

		// Token: 0x04000A71 RID: 2673
		public Slice3D slice;

		// Token: 0x04000A72 RID: 2674
		public NumNeighbours neighbours;

		// Token: 0x04000A73 RID: 2675
		public bool cutCorners;

		// Token: 0x04000A74 RID: 2676
		public UnsafeSpan<ulong> nodeConnections;
	}
}
