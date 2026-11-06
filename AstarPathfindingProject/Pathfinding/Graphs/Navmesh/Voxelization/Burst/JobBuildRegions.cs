using System;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001F0 RID: 496
	[BurstCompile(CompileSynchronously = true)]
	public struct JobBuildRegions : IJob
	{
		// Token: 0x06000D91 RID: 3473 RVA: 0x00050958 File Offset: 0x0004EB58
		private void MarkRectWithRegion(int minx, int maxx, int minz, int maxz, ushort region, NativeArray<ushort> srcReg)
		{
			int num = maxz * this.field.width;
			for (int i = minz * this.field.width; i < num; i += this.field.width)
			{
				for (int j = minx; j < maxx; j++)
				{
					CompactVoxelCell compactVoxelCell = this.field.cells[i + j];
					int k = compactVoxelCell.index;
					int num2 = compactVoxelCell.index + compactVoxelCell.count;
					while (k < num2)
					{
						if (this.field.areaTypes[k] != 0)
						{
							srcReg[k] = region;
						}
						k++;
					}
				}
			}
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x000509FC File Offset: 0x0004EBFC
		public static bool FloodRegion(int x, int z, int i, uint level, ushort r, CompactVoxelField field, NativeArray<ushort> distanceField, NativeArray<ushort> srcReg, NativeArray<ushort> srcDist, NativeArray<Int3> stack, NativeArray<int> flags, NativeArray<bool> closed)
		{
			int num = field.areaTypes[i];
			int j = 1;
			stack[0] = new Int3
			{
				x = x,
				y = i,
				z = z
			};
			srcReg[i] = r;
			srcDist[i] = 0;
			int num2 = (int)((level >= 2U) ? (level - 2U) : 0U);
			int num3 = 0;
			NativeList<CompactVoxelCell> cells = field.cells;
			NativeList<CompactVoxelSpan> spans = field.spans;
			NativeList<int> areaTypes = field.areaTypes;
			while (j > 0)
			{
				j--;
				Int3 @int = stack[j];
				int y = @int.y;
				int x2 = @int.x;
				int z2 = @int.z;
				CompactVoxelSpan compactVoxelSpan = spans[y];
				ushort num4 = 0;
				for (int k = 0; k < 4; k++)
				{
					if ((long)compactVoxelSpan.GetConnection(k) != 63L)
					{
						int num5 = x2 + VoxelUtilityBurst.DX[k];
						int num6 = z2 + VoxelUtilityBurst.DZ[k] * field.width;
						int num7 = cells[num5 + num6].index + compactVoxelSpan.GetConnection(k);
						if (areaTypes[num7] == num)
						{
							ushort num8 = srcReg[num7];
							if ((num8 & 32768) != 32768)
							{
								if (num8 != 0 && num8 != r)
								{
									num4 = num8;
									break;
								}
								int num9 = (k + 1) & 3;
								int connection = spans[num7].GetConnection(num9);
								if ((long)connection != 63L)
								{
									int num10 = num5 + VoxelUtilityBurst.DX[num9];
									int num11 = num6 + VoxelUtilityBurst.DZ[num9] * field.width;
									int num12 = cells[num10 + num11].index + connection;
									if (areaTypes[num12] == num)
									{
										ushort num13 = srcReg[num12];
										if ((num13 & 32768) != 32768 && num13 != 0 && num13 != r)
										{
											num4 = num13;
											break;
										}
									}
								}
							}
						}
					}
				}
				if (num4 != 0)
				{
					srcReg[y] = 0;
					srcDist[y] = ushort.MaxValue;
				}
				else
				{
					num3++;
					closed[y] = true;
					for (int l = 0; l < 4; l++)
					{
						if ((long)compactVoxelSpan.GetConnection(l) != 63L)
						{
							int num14 = x2 + VoxelUtilityBurst.DX[l];
							int num15 = z2 + VoxelUtilityBurst.DZ[l] * field.width;
							int num16 = cells[num14 + num15].index + compactVoxelSpan.GetConnection(l);
							if (areaTypes[num16] == num && srcReg[num16] == 0)
							{
								if ((int)distanceField[num16] >= num2 && flags[num16] == 0)
								{
									srcReg[num16] = r;
									srcDist[num16] = 0;
									stack[j] = new Int3
									{
										x = num14,
										y = num16,
										z = num15
									};
									j++;
								}
								else
								{
									flags[num16] = (int)r;
									srcDist[num16] = 2;
								}
							}
						}
					}
				}
			}
			return num3 > 0;
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00050D1C File Offset: 0x0004EF1C
		public void Execute()
		{
			this.srcQue.Clear();
			this.dstQue.Clear();
			int width = this.field.width;
			int depth = this.field.depth;
			int num = width * depth;
			int length = this.field.spans.Length;
			int num2 = 8;
			NativeArray<ushort> nativeArray = new NativeArray<ushort>(length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<ushort> nativeArray2 = new NativeArray<ushort>(length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<bool> nativeArray3 = new NativeArray<bool>(length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> nativeArray4 = new NativeArray<int>(length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<Int3> nativeArray5 = new NativeArray<Int3>(length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < length; i++)
			{
				nativeArray[i] = 0;
				nativeArray2[i] = ushort.MaxValue;
				nativeArray3[i] = false;
				nativeArray4[i] = 0;
			}
			NativeList<ushort> nativeList = this.distanceField;
			NativeList<int> areaTypes = this.field.areaTypes;
			NativeList<CompactVoxelCell> cells = this.field.cells;
			ushort num3 = 2;
			this.MarkRectWithRegion(0, this.borderSize, 0, depth, num3 | 32768, nativeArray);
			num3 += 1;
			this.MarkRectWithRegion(width - this.borderSize, width, 0, depth, num3 | 32768, nativeArray);
			num3 += 1;
			this.MarkRectWithRegion(0, width, 0, this.borderSize, num3 | 32768, nativeArray);
			num3 += 1;
			this.MarkRectWithRegion(0, width, depth - this.borderSize, depth, num3 | 32768, nativeArray);
			num3 += 1;
			int num4 = 0;
			for (int j = 0; j < this.distanceField.Length; j++)
			{
				num4 = math.max((int)this.distanceField[j], num4);
			}
			NativeArray<int> nativeArray6 = new NativeArray<int>(num4 / 2 + 1, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int k = 0; k < this.field.spans.Length; k++)
			{
				if ((nativeArray[k] & 32768) != 32768 && areaTypes[k] != 0)
				{
					int num5 = (int)(this.distanceField[k] / 2);
					int num6 = nativeArray6[num5];
					nativeArray6[num5] = num6 + 1;
				}
			}
			NativeArray<int> nativeArray7 = new NativeArray<int>(nativeArray6.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int l = 1; l < nativeArray7.Length; l++)
			{
				nativeArray7[l] = nativeArray7[l - 1] + nativeArray6[l - 1];
			}
			int num7 = nativeArray7[nativeArray7.Length - 1] + nativeArray6[nativeArray6.Length - 1];
			NativeArray<Int3> nativeArray8 = new NativeArray<Int3>(num7, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			int m = 0;
			int num8 = 0;
			while (m < num)
			{
				for (int n = 0; n < this.field.width; n++)
				{
					CompactVoxelCell compactVoxelCell = cells[m + n];
					int num9 = compactVoxelCell.index;
					int num10 = compactVoxelCell.index + compactVoxelCell.count;
					while (num9 < num10)
					{
						if ((nativeArray[num9] & 32768) != 32768 && areaTypes[num9] != 0)
						{
							int num11 = (int)(this.distanceField[num9] / 2);
							int num6 = num11;
							int num5 = nativeArray7[num6];
							nativeArray7[num6] = num5 + 1;
							nativeArray8[num5] = new Int3(n, num9, m);
						}
						num9++;
					}
				}
				m += width;
				num8++;
			}
			for (int num12 = nativeArray6.Length - 1; num12 >= 0; num12--)
			{
				uint num13 = (uint)(num12 * 2);
				int num14 = nativeArray6[num12];
				for (int num15 = 0; num15 < num14; num15++)
				{
					Int3 @int = nativeArray8[nativeArray7[num12] - num15 - 1];
					int y = @int.y;
					if (nativeArray4[y] != 0 && nativeArray[y] == 0)
					{
						nativeArray[y] = (ushort)nativeArray4[y];
						this.srcQue.Enqueue(@int);
						nativeArray3[y] = true;
					}
				}
				int num16 = 0;
				while (num16 < num2 && this.srcQue.Count > 0)
				{
					while (this.srcQue.Count > 0)
					{
						Int3 int2 = this.srcQue.Dequeue();
						int num17 = areaTypes[int2.y];
						CompactVoxelSpan compactVoxelSpan = this.field.spans[int2.y];
						ushort num18 = nativeArray[int2.y];
						nativeArray3[int2.y] = true;
						ushort num19 = nativeArray2[int2.y] + 2;
						for (int num20 = 0; num20 < 4; num20++)
						{
							int connection = compactVoxelSpan.GetConnection(num20);
							if ((long)connection != 63L)
							{
								int num21 = int2.x + VoxelUtilityBurst.DX[num20];
								int num22 = int2.z + VoxelUtilityBurst.DZ[num20] * this.field.width;
								int num23 = cells[num21 + num22].index + connection;
								if ((nativeArray[num23] & 32768) != 32768 && num17 == areaTypes[num23] && num19 < nativeArray2[num23])
								{
									if ((uint)nativeList[num23] < num13)
									{
										nativeArray2[num23] = num19;
										nativeArray4[num23] = (int)num18;
									}
									else if (!nativeArray3[num23])
									{
										nativeArray2[num23] = num19;
										if (nativeArray[num23] == 0)
										{
											this.dstQue.Enqueue(new Int3(num21, num23, num22));
										}
										nativeArray[num23] = num18;
									}
								}
							}
						}
					}
					Memory.Swap<NativeQueue<Int3>>(ref this.srcQue, ref this.dstQue);
					num16++;
				}
				NativeArray<ushort> nativeArray9 = this.distanceField.AsArray();
				for (int num24 = 0; num24 < num14; num24++)
				{
					Int3 int3 = nativeArray8[nativeArray7[num12] - num24 - 1];
					if (nativeArray[int3.y] == 0 && JobBuildRegions.FloodRegion(int3.x, int3.z, int3.y, num13, num3, this.field, nativeArray9, nativeArray, nativeArray2, nativeArray5, nativeArray4, nativeArray3))
					{
						num3 += 1;
					}
				}
			}
			ushort num25 = num3;
			Matrix4x4 matrix4x = Matrix4x4.TRS(this.graphSpaceBounds.min, Quaternion.identity, Vector3.one) * Matrix4x4.Scale(new Vector3(this.cellSize, this.cellHeight, this.cellSize));
			Matrix4x4 matrix4x2 = this.graphTransform * matrix4x * Matrix4x4.Translate(new Vector3(0.5f, 0f, 0.5f));
			JobBuildRegions.FilterSmallRegions(this.field, nativeArray, this.minRegionSize, (int)num25, this.relevantGraphSurfaces, this.relevantGraphSurfaceMode, matrix4x2);
			for (int num26 = 0; num26 < length; num26++)
			{
				CompactVoxelSpan compactVoxelSpan2 = this.field.spans[num26];
				compactVoxelSpan2.reg = (int)nativeArray[num26];
				this.field.spans[num26] = compactVoxelSpan2;
			}
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00051434 File Offset: 0x0004F634
		private static int union_find_find(NativeArray<int> arr, int x)
		{
			if (arr[x] < 0)
			{
				return x;
			}
			return arr[x] = JobBuildRegions.union_find_find(arr, arr[x]);
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00051468 File Offset: 0x0004F668
		private static void union_find_union(NativeArray<int> arr, int a, int b)
		{
			a = JobBuildRegions.union_find_find(arr, a);
			b = JobBuildRegions.union_find_find(arr, b);
			if (a == b)
			{
				return;
			}
			if (arr[a] > arr[b])
			{
				int num = a;
				a = b;
				b = num;
			}
			ref NativeArray<int> ptr = ref arr;
			int num2 = a;
			ptr[num2] += arr[b];
			arr[b] = a;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x000514CC File Offset: 0x0004F6CC
		public static void FilterSmallRegions(CompactVoxelField field, NativeArray<ushort> reg, int minRegionSize, int maxRegions, NativeArray<JobBuildRegions.RelevantGraphSurfaceInfo> relevantGraphSurfaces, RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode, float4x4 voxel2worldMatrix)
		{
			bool flag = relevantGraphSurfaces.Length != 0 && relevantGraphSurfaceMode > RecastGraph.RelevantGraphSurfaceMode.DoNotRequire;
			if (!flag && minRegionSize <= 0)
			{
				return;
			}
			NativeArray<int> nativeArray = new NativeArray<int>(maxRegions, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<ushort> nativeArray2 = new NativeArray<ushort>(maxRegions, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				nativeArray[i] = -1;
			}
			int length = nativeArray.Length;
			int num = field.width * field.depth;
			int num2 = 2 | ((relevantGraphSurfaceMode == RecastGraph.RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile) ? 1 : 0);
			if (flag)
			{
				float4x4 float4x = math.inverse(voxel2worldMatrix);
				for (int j = 0; j < relevantGraphSurfaces.Length; j++)
				{
					JobBuildRegions.RelevantGraphSurfaceInfo relevantGraphSurfaceInfo = relevantGraphSurfaces[j];
					int3 @int = (int3)math.round(math.transform(float4x, relevantGraphSurfaceInfo.position));
					if (@int.x >= 0 && @int.z >= 0 && @int.x < field.width && @int.z < field.depth)
					{
						float num3 = math.length(voxel2worldMatrix.c1.xyz);
						int num4 = (int)(relevantGraphSurfaceInfo.range / num3);
						CompactVoxelCell compactVoxelCell = field.cells[@int.x + @int.z * field.width];
						for (int k = compactVoxelCell.index; k < compactVoxelCell.index + compactVoxelCell.count; k++)
						{
							if (Math.Abs((int)field.spans[k].y - @int.y) <= num4 && reg[k] != 0)
							{
								ref NativeArray<ushort> ptr = ref nativeArray2;
								int num5 = JobBuildRegions.union_find_find(nativeArray, (int)reg[k] & -32769);
								ptr[num5] |= 2;
							}
						}
					}
				}
			}
			for (int l = 0; l < num; l += field.width)
			{
				for (int m = 0; m < field.width; m++)
				{
					CompactVoxelCell compactVoxelCell2 = field.cells[m + l];
					for (int n = compactVoxelCell2.index; n < compactVoxelCell2.index + compactVoxelCell2.count; n++)
					{
						CompactVoxelSpan compactVoxelSpan = field.spans[n];
						int num6 = (int)reg[n];
						if ((num6 & -32769) != 0)
						{
							if (num6 >= length)
							{
								ref NativeArray<ushort> ptr = ref nativeArray2;
								int num5 = JobBuildRegions.union_find_find(nativeArray, num6 & -32769);
								ptr[num5] |= 1;
							}
							else
							{
								int num7 = JobBuildRegions.union_find_find(nativeArray, num6);
								int num5 = num7;
								int num8 = nativeArray[num5];
								nativeArray[num5] = num8 - 1;
								for (int num9 = 0; num9 < 4; num9++)
								{
									if ((long)compactVoxelSpan.GetConnection(num9) != 63L)
									{
										int num10 = m + VoxelUtilityBurst.DX[num9];
										int num11 = l + VoxelUtilityBurst.DZ[num9] * field.width;
										int num12 = field.cells[num10 + num11].index + compactVoxelSpan.GetConnection(num9);
										int num13 = (int)reg[num12];
										if (num6 != num13 && (num13 & -32769) != 0)
										{
											if ((num13 & 32768) != 0)
											{
												ref NativeArray<ushort> ptr = ref nativeArray2;
												num8 = num7;
												ptr[num8] |= 1;
											}
											else
											{
												JobBuildRegions.union_find_union(nativeArray, num7, num13);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			for (int num14 = 0; num14 < nativeArray.Length; num14++)
			{
				ref NativeArray<ushort> ptr = ref nativeArray2;
				int num8 = JobBuildRegions.union_find_find(nativeArray, num14);
				ptr[num8] |= nativeArray2[num14];
			}
			for (int num15 = 0; num15 < nativeArray.Length; num15++)
			{
				int num16 = JobBuildRegions.union_find_find(nativeArray, num15);
				if ((nativeArray2[num16] & 1) != 0)
				{
					nativeArray[num16] = -minRegionSize - 2;
				}
				if (flag && ((int)nativeArray2[num16] & num2) == 0)
				{
					nativeArray[num16] = -1;
				}
			}
			for (int num17 = 0; num17 < reg.Length; num17++)
			{
				int num18 = (int)reg[num17];
				if (num18 < length && nativeArray[JobBuildRegions.union_find_find(nativeArray, num18)] >= -minRegionSize - 1)
				{
					reg[num17] = 0;
				}
			}
		}

		// Token: 0x04000948 RID: 2376
		public CompactVoxelField field;

		// Token: 0x04000949 RID: 2377
		public NativeList<ushort> distanceField;

		// Token: 0x0400094A RID: 2378
		public int borderSize;

		// Token: 0x0400094B RID: 2379
		public int minRegionSize;

		// Token: 0x0400094C RID: 2380
		public NativeQueue<Int3> srcQue;

		// Token: 0x0400094D RID: 2381
		public NativeQueue<Int3> dstQue;

		// Token: 0x0400094E RID: 2382
		public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		// Token: 0x0400094F RID: 2383
		public NativeArray<JobBuildRegions.RelevantGraphSurfaceInfo> relevantGraphSurfaces;

		// Token: 0x04000950 RID: 2384
		public float cellSize;

		// Token: 0x04000951 RID: 2385
		public float cellHeight;

		// Token: 0x04000952 RID: 2386
		public Matrix4x4 graphTransform;

		// Token: 0x04000953 RID: 2387
		public Bounds graphSpaceBounds;

		// Token: 0x020001F1 RID: 497
		public struct RelevantGraphSurfaceInfo
		{
			// Token: 0x04000954 RID: 2388
			public float3 position;

			// Token: 0x04000955 RID: 2389
			public float range;
		}
	}
}
