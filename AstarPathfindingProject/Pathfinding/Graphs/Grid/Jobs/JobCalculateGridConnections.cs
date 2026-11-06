using System;
using Pathfinding.Collections;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000226 RID: 550
	[BurstCompile(FloatMode = FloatMode.Fast, CompileSynchronously = true)]
	public struct JobCalculateGridConnections : IJobParallelForBatched
	{
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x0001AA07 File Offset: 0x00018C07
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00056C80 File Offset: 0x00054E80
		public static bool IsValidConnection(float y, float y2, float maxStepHeight)
		{
			return math.abs(y - y2) <= maxStepHeight;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00056C90 File Offset: 0x00054E90
		public static bool IsValidConnection(float2 yRange, float2 yRange2, float maxStepHeight, float characterHeight)
		{
			if (!JobCalculateGridConnections.IsValidConnection(yRange.x, yRange2.x, maxStepHeight))
			{
				return false;
			}
			float num = math.max(yRange.x, yRange2.x);
			return math.min(yRange.y, yRange2.y) - num >= characterHeight;
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00056CE0 File Offset: 0x00054EE0
		private unsafe static float ConnectionY(UnsafeSpan<float3> nodePositions, UnsafeSpan<float4> nodeNormals, NativeArray<float4> normalToHeightOffset, int nodeIndex, int dir, float4 up, bool reverse)
		{
			Hint.Assume(nodeIndex >= 0 && (long)nodeIndex < (long)((ulong)nodePositions.length));
			Hint.Assume(nodeIndex >= 0 && (long)nodeIndex < (long)((ulong)nodeNormals.length));
			Hint.Assume(dir >= 0 && dir < normalToHeightOffset.Length);
			float4 @float = new float4(*nodePositions[(uint)nodeIndex], 0f);
			return math.dot(up, @float) + (float)(reverse ? (-1) : 1) * math.dot(*nodeNormals[nodeIndex], normalToHeightOffset[dir]);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00056D7C File Offset: 0x00054F7C
		private unsafe static float2 ConnectionYRange(UnsafeSpan<float3> nodePositions, UnsafeSpan<float4> nodeNormals, NativeArray<float4> normalToHeightOffset, int nodeIndex, int layerStride, int y, int maxY, int dir, float4 up, bool reverse)
		{
			float num = JobCalculateGridConnections.ConnectionY(nodePositions, nodeNormals, normalToHeightOffset, nodeIndex, dir, up, reverse);
			int num2 = nodeIndex + layerStride;
			float num3;
			if (num2 < (int)nodeNormals.length && math.any(*nodeNormals[(uint)num2]))
			{
				num3 = JobCalculateGridConnections.ConnectionY(nodePositions, nodeNormals, normalToHeightOffset, num2, dir, up, reverse);
			}
			else
			{
				num3 = float.PositiveInfinity;
			}
			return new float2(num, num3);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00056DD8 File Offset: 0x00054FD8
		private static NativeArray<float4> HeightOffsetProjections(float4x4 graphToWorldTranform, bool maxStepUsesSlope)
		{
			NativeArray<float4> nativeArray = new NativeArray<float4>(8, Allocator.Temp, NativeArrayOptions.ClearMemory);
			if (maxStepUsesSlope)
			{
				for (int i = 0; i < nativeArray.Length; i++)
				{
					float3 @float = (float)GridGraph.neighbourXOffsets[i] * graphToWorldTranform.c0.xyz + (float)GridGraph.neighbourZOffsets[i] * graphToWorldTranform.c2.xyz;
					nativeArray[i] = -new float4(@float, 0f) * 0.5f;
				}
			}
			return nativeArray;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00056E60 File Offset: 0x00055060
		public void Execute(int start, int count)
		{
			if (this.nodePositions.Length != this.nodeNormals.Length)
			{
				throw new Exception("nodePositions and nodeNormals must have the same length");
			}
			if (this.nodePositions.Length != this.nodeWalkable.Length)
			{
				throw new Exception("nodePositions and nodeWalkable must have the same length");
			}
			if (this.nodePositions.Length != this.nodeConnections.Length)
			{
				throw new Exception("nodePositions and nodeConnections must have the same length");
			}
			if (this.layeredDataLayout)
			{
				this.ExecuteLayered(start, count);
				return;
			}
			this.ExecuteFlat(start, count);
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00056EF0 File Offset: 0x000550F0
		public unsafe void ExecuteFlat(int start, int count)
		{
			if (this.maxStepHeight <= 0f || this.use2D)
			{
				this.maxStepHeight = float.PositiveInfinity;
			}
			float4 c = this.graphToWorld.c1;
			NativeArray<int> nativeArray = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < 8; i++)
			{
				nativeArray[i] = GridGraph.neighbourZOffsets[i] * this.arrayBounds.x + GridGraph.neighbourXOffsets[i];
			}
			UnsafeSpan<float3> unsafeSpan = this.nodePositions.Reinterpret<float3>();
			NativeArray<float4> nativeArray2 = JobCalculateGridConnections.HeightOffsetProjections(this.graphToWorld, this.maxStepUsesSlope);
			start += this.bounds.min.z;
			for (int j = start; j < start + count; j++)
			{
				int num = 255;
				if (j == 0)
				{
					num &= -146;
				}
				if (j == this.arrayBounds.z - 1)
				{
					num &= -101;
				}
				for (int k = this.bounds.min.x; k < this.bounds.max.x; k++)
				{
					int num2 = j * this.arrayBounds.x + k;
					if (!(*this.nodeWalkable[num2]))
					{
						*this.nodeConnections[num2] = 0UL;
					}
					else
					{
						int num3 = num;
						if (k == 0)
						{
							num3 &= -201;
						}
						if (k == this.arrayBounds.x - 1)
						{
							num3 &= -51;
						}
						for (int l = 0; l < 8; l++)
						{
							float num4 = JobCalculateGridConnections.ConnectionY(unsafeSpan, this.nodeNormals, nativeArray2, num2, l, c, false);
							int num5 = num2 + nativeArray[l];
							if ((num3 & (1 << l)) != 0)
							{
								float num6 = JobCalculateGridConnections.ConnectionY(unsafeSpan, this.nodeNormals, nativeArray2, num5, l, c, true);
								if (!(*this.nodeWalkable[num5]) || !JobCalculateGridConnections.IsValidConnection(num4, num6, this.maxStepHeight))
								{
									num3 &= ~(1 << l);
								}
							}
						}
						*this.nodeConnections[num2] = (ulong)((long)GridNode.FilterDiagonalConnections(num3, this.neighbours, this.cutCorners));
					}
				}
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0005711C File Offset: 0x0005531C
		public unsafe void ExecuteLayered(int start, int count)
		{
			if (this.maxStepHeight <= 0f || this.use2D)
			{
				this.maxStepHeight = float.PositiveInfinity;
			}
			float4 c = this.graphToWorld.c1;
			NativeArray<int> nativeArray = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < 8; i++)
			{
				nativeArray[i] = GridGraph.neighbourZOffsets[i] * this.arrayBounds.x + GridGraph.neighbourXOffsets[i];
			}
			UnsafeSpan<float3> unsafeSpan = this.nodePositions.Reinterpret<float3>();
			NativeArray<float4> nativeArray2 = JobCalculateGridConnections.HeightOffsetProjections(this.graphToWorld, this.maxStepUsesSlope);
			int num = this.arrayBounds.z * this.arrayBounds.x;
			start += this.bounds.min.z;
			for (int j = this.bounds.min.y; j < this.bounds.max.y; j++)
			{
				for (int k = start; k < start + count; k++)
				{
					for (int l = this.bounds.min.x; l < this.bounds.max.x; l++)
					{
						ulong num2 = 0UL;
						int num3 = k * this.arrayBounds.x + l;
						int num4 = num3 + j * num;
						if (*this.nodeWalkable[num4])
						{
							for (int m = 0; m < 8; m++)
							{
								int num5 = l + GridGraph.neighbourXOffsets[m];
								int num6 = k + GridGraph.neighbourZOffsets[m];
								int num7 = 15;
								if (num5 >= 0 && num6 >= 0 && num5 < this.arrayBounds.x && num6 < this.arrayBounds.z)
								{
									float2 @float = JobCalculateGridConnections.ConnectionYRange(unsafeSpan, this.nodeNormals, nativeArray2, num4, num, j, this.arrayBounds.y, m, c, false);
									int num8 = num3 + nativeArray[m];
									for (int n = 0; n < this.arrayBounds.y; n++)
									{
										int num9 = num8 + n * num;
										if (*this.nodeWalkable[num9])
										{
											float2 float2 = JobCalculateGridConnections.ConnectionYRange(unsafeSpan, this.nodeNormals, nativeArray2, num9, num, n, this.arrayBounds.y, m, c, true);
											if (JobCalculateGridConnections.IsValidConnection(@float, float2, this.maxStepHeight, this.characterHeight))
											{
												num7 = n;
												break;
											}
										}
									}
								}
								num2 |= (ulong)((ulong)((long)num7) << 4 * m);
							}
						}
						else
						{
							num2 = (ulong)(-1);
						}
						*this.nodeConnections[num4] = num2;
					}
				}
			}
		}

		// Token: 0x04000A4D RID: 2637
		public float maxStepHeight;

		// Token: 0x04000A4E RID: 2638
		public float4x4 graphToWorld;

		// Token: 0x04000A4F RID: 2639
		public IntBounds bounds;

		// Token: 0x04000A50 RID: 2640
		public int3 arrayBounds;

		// Token: 0x04000A51 RID: 2641
		public NumNeighbours neighbours;

		// Token: 0x04000A52 RID: 2642
		public float characterHeight;

		// Token: 0x04000A53 RID: 2643
		public bool use2D;

		// Token: 0x04000A54 RID: 2644
		public bool cutCorners;

		// Token: 0x04000A55 RID: 2645
		public bool maxStepUsesSlope;

		// Token: 0x04000A56 RID: 2646
		public bool layeredDataLayout;

		// Token: 0x04000A57 RID: 2647
		[ReadOnly]
		public UnsafeSpan<bool> nodeWalkable;

		// Token: 0x04000A58 RID: 2648
		[ReadOnly]
		public UnsafeSpan<float4> nodeNormals;

		// Token: 0x04000A59 RID: 2649
		[ReadOnly]
		public UnsafeSpan<Vector3> nodePositions;

		// Token: 0x04000A5A RID: 2650
		[WriteOnly]
		public UnsafeSpan<ulong> nodeConnections;
	}
}
