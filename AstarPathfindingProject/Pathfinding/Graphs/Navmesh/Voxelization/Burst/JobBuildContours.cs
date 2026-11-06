using System;
using Pathfinding.Collections;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001E5 RID: 485
	[BurstCompile(CompileSynchronously = true)]
	public struct JobBuildContours : IJob
	{
		// Token: 0x06000D6B RID: 3435 RVA: 0x0004D650 File Offset: 0x0004B850
		public void Execute()
		{
			this.outputContours.Clear();
			this.outputVerts.Clear();
			int width = this.field.width;
			int depth = this.field.depth;
			int num = width * depth;
			NativeArray<ushort> nativeArray = new NativeArray<ushort>(this.field.spans.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < num; i += this.field.width)
			{
				for (int j = 0; j < this.field.width; j++)
				{
					CompactVoxelCell compactVoxelCell = this.field.cells[j + i];
					int k = compactVoxelCell.index;
					int num2 = compactVoxelCell.index + compactVoxelCell.count;
					while (k < num2)
					{
						ushort num3 = 0;
						CompactVoxelSpan compactVoxelSpan = this.field.spans[k];
						if (compactVoxelSpan.reg == 0 || (compactVoxelSpan.reg & 32768) == 32768)
						{
							nativeArray[k] = 0;
						}
						else
						{
							for (int l = 0; l < 4; l++)
							{
								int num4 = 0;
								if ((long)compactVoxelSpan.GetConnection(l) != 63L)
								{
									int num5 = this.field.cells[this.field.GetNeighbourIndex(j + i, l)].index + compactVoxelSpan.GetConnection(l);
									num4 = this.field.spans[num5].reg;
								}
								if (num4 == compactVoxelSpan.reg)
								{
									num3 |= (ushort)(1 << l);
								}
							}
							nativeArray[k] = num3 ^ 15;
						}
						k++;
					}
				}
			}
			NativeList<int> nativeList = new NativeList<int>(256, Allocator.Temp);
			NativeList<int> nativeList2 = new NativeList<int>(64, Allocator.Temp);
			for (int m = 0; m < num; m += this.field.width)
			{
				for (int n = 0; n < this.field.width; n++)
				{
					CompactVoxelCell compactVoxelCell2 = this.field.cells[n + m];
					int num6 = compactVoxelCell2.index;
					int num7 = compactVoxelCell2.index + compactVoxelCell2.count;
					while (num6 < num7)
					{
						if (nativeArray[num6] == 0 || nativeArray[num6] == 15)
						{
							nativeArray[num6] = 0;
						}
						else
						{
							int reg = this.field.spans[num6].reg;
							if (reg != 0 && (reg & 32768) != 32768)
							{
								int num8 = this.field.areaTypes[num6];
								nativeList.Clear();
								nativeList2.Clear();
								this.WalkContour(n, m, num6, nativeArray, nativeList);
								this.SimplifyContour(nativeList, nativeList2, this.maxError, this.buildFlags);
								JobBuildContours.RemoveDegenerateSegments(nativeList2);
								VoxelContour voxelContour = new VoxelContour
								{
									vertexStartIndex = this.outputVerts.Length,
									nverts = nativeList2.Length / 4,
									reg = reg,
									area = num8
								};
								this.outputVerts.AddRange(nativeList2.AsArray());
								this.outputContours.Add(in voxelContour);
							}
						}
						num6++;
					}
				}
			}
			nativeList.Dispose();
			nativeList2.Dispose();
			for (int num9 = 0; num9 < this.outputContours.Length; num9++)
			{
				VoxelContour voxelContour2 = this.outputContours[num9];
				NativeArray<int> nativeArray2 = this.outputVerts.AsArray();
				if (this.CalcAreaOfPolygon2D(nativeArray2, voxelContour2.vertexStartIndex, voxelContour2.nverts) < 0)
				{
					int num10 = -1;
					for (int num11 = 0; num11 < this.outputContours.Length; num11++)
					{
						if (num9 != num11 && this.outputContours[num11].nverts > 0 && this.outputContours[num11].reg == voxelContour2.reg && this.CalcAreaOfPolygon2D(nativeArray2, this.outputContours[num11].vertexStartIndex, this.outputContours[num11].nverts) > 0)
						{
							num10 = num11;
							break;
						}
					}
					if (num10 != -1)
					{
						VoxelContour voxelContour3 = this.outputContours[num10];
						int num12;
						int num13;
						this.GetClosestIndices(nativeArray2, voxelContour3.vertexStartIndex, voxelContour3.nverts, voxelContour2.vertexStartIndex, voxelContour2.nverts, out num12, out num13);
						if (num12 != -1 && num13 != -1 && JobBuildContours.MergeContours(this.outputVerts, ref voxelContour3, ref voxelContour2, num12, num13))
						{
							this.outputContours[num10] = voxelContour3;
							this.outputContours[num9] = voxelContour2;
						}
					}
				}
			}
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0004DB18 File Offset: 0x0004BD18
		private void GetClosestIndices(NativeArray<int> verts, int vertexStartIndexA, int nvertsa, int vertexStartIndexB, int nvertsb, out int ia, out int ib)
		{
			int num = 268435455;
			ia = -1;
			ib = -1;
			for (int i = 0; i < nvertsa; i++)
			{
				int num2 = (i + 1) % nvertsa;
				int num3 = (i + nvertsa - 1) % nvertsa;
				int num4 = vertexStartIndexA + i * 4;
				int num5 = vertexStartIndexA + num2 * 4;
				int num6 = vertexStartIndexA + num3 * 4;
				for (int j = 0; j < nvertsb; j++)
				{
					int num7 = vertexStartIndexB + j * 4;
					if (JobBuildContours.Ileft(verts, num6, num4, num7) && JobBuildContours.Ileft(verts, num4, num5, num7))
					{
						int num8 = verts[num7] - verts[num4];
						int num9 = verts[num7 + 2] / this.field.width - verts[num4 + 2] / this.field.width;
						int num10 = num8 * num8 + num9 * num9;
						if (num10 < num)
						{
							ia = i;
							ib = j;
							num = num10;
						}
					}
				}
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0004DC04 File Offset: 0x0004BE04
		public static bool MergeContours(NativeList<int> verts, ref VoxelContour ca, ref VoxelContour cb, int ia, int ib)
		{
			int num = 0;
			int length = verts.Length;
			for (int i = 0; i <= ca.nverts; i++)
			{
				int num2 = ca.vertexStartIndex + (ia + i) % ca.nverts * 4;
				int num3 = verts[num2];
				verts.Add(in num3);
				num3 = verts[num2 + 1];
				verts.Add(in num3);
				num3 = verts[num2 + 2];
				verts.Add(in num3);
				num3 = verts[num2 + 3];
				verts.Add(in num3);
				num++;
			}
			for (int j = 0; j <= cb.nverts; j++)
			{
				int num4 = cb.vertexStartIndex + (ib + j) % cb.nverts * 4;
				int num3 = verts[num4];
				verts.Add(in num3);
				num3 = verts[num4 + 1];
				verts.Add(in num3);
				num3 = verts[num4 + 2];
				verts.Add(in num3);
				num3 = verts[num4 + 3];
				verts.Add(in num3);
				num++;
			}
			ca.vertexStartIndex = length;
			ca.nverts = num;
			cb.vertexStartIndex = 0;
			cb.nverts = 0;
			return true;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0004DD3C File Offset: 0x0004BF3C
		public void SimplifyContour(NativeList<int> verts, NativeList<int> simplified, float maxError, int buildFlags)
		{
			bool flag = false;
			for (int i = 0; i < verts.Length; i += 4)
			{
				if ((verts[i + 3] & 65535) != 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				int j = 0;
				int num = verts.Length / 4;
				while (j < num)
				{
					int num2 = (j + 1) % num;
					bool flag2 = (verts[j * 4 + 3] & 65535) != (verts[num2 * 4 + 3] & 65535);
					bool flag3 = (verts[j * 4 + 3] & 131072) != (verts[num2 * 4 + 3] & 131072);
					if (flag2 || flag3)
					{
						int num3 = verts[j * 4];
						simplified.Add(in num3);
						num3 = verts[j * 4 + 1];
						simplified.Add(in num3);
						num3 = verts[j * 4 + 2];
						simplified.Add(in num3);
						simplified.Add(in j);
					}
					j++;
				}
			}
			if (simplified.Length == 0)
			{
				int num4 = verts[0];
				int num5 = verts[1];
				int num6 = verts[2];
				int num7 = 0;
				int num8 = verts[0];
				int num9 = verts[1];
				int num10 = verts[2];
				int num11 = 0;
				for (int k = 0; k < verts.Length; k += 4)
				{
					int num12 = verts[k];
					int num13 = verts[k + 1];
					int num14 = verts[k + 2];
					if (num12 < num4 || (num12 == num4 && num14 < num6))
					{
						num4 = num12;
						num5 = num13;
						num6 = num14;
						num7 = k / 4;
					}
					if (num12 > num8 || (num12 == num8 && num14 > num10))
					{
						num8 = num12;
						num9 = num13;
						num10 = num14;
						num11 = k / 4;
					}
				}
				simplified.Add(in num4);
				simplified.Add(in num5);
				simplified.Add(in num6);
				simplified.Add(in num7);
				simplified.Add(in num8);
				simplified.Add(in num9);
				simplified.Add(in num10);
				simplified.Add(in num11);
			}
			int num15 = verts.Length / 4;
			maxError *= maxError;
			int l = 0;
			while (l < simplified.Length / 4)
			{
				int num16 = (l + 1) % (simplified.Length / 4);
				int num17 = simplified[l * 4];
				int num18 = simplified[l * 4 + 1];
				int num19 = simplified[l * 4 + 2];
				int num20 = simplified[l * 4 + 3];
				int num21 = simplified[num16 * 4];
				int num22 = simplified[num16 * 4 + 1];
				int num23 = simplified[num16 * 4 + 2];
				int num24 = simplified[num16 * 4 + 3];
				float num25 = 0f;
				int num26 = -1;
				int num27;
				int num28;
				int num29;
				if (num21 > num17 || (num21 == num17 && num23 > num19))
				{
					num27 = 1;
					num28 = (num20 + num27) % num15;
					num29 = num24;
				}
				else
				{
					num27 = num15 - 1;
					num28 = (num24 + num27) % num15;
					num29 = num20;
					Memory.Swap<int>(ref num17, ref num21);
					Memory.Swap<int>(ref num19, ref num23);
				}
				if ((verts[num28 * 4 + 3] & 65535) != 0)
				{
					if ((verts[num28 * 4 + 3] & 131072) != 131072)
					{
						goto IL_03A4;
					}
				}
				while (num28 != num29)
				{
					float num30 = VectorMath.SqrDistancePointSegmentApproximate(verts[num28 * 4], verts[num28 * 4 + 2] / this.field.width, num17, num19 / this.field.width, num21, num23 / this.field.width);
					if (num30 > num25)
					{
						num25 = num30;
						num26 = num28;
					}
					num28 = (num28 + num27) % num15;
				}
				IL_03A4:
				if (num26 != -1 && num25 > maxError)
				{
					simplified.ResizeUninitialized(simplified.Length + 4);
					simplified.AsUnsafeSpan<int>().Move((l + 1) * 4, (l + 2) * 4, simplified.Length - (l + 2) * 4);
					simplified[(l + 1) * 4] = verts[num26 * 4];
					simplified[(l + 1) * 4 + 1] = verts[num26 * 4 + 1];
					simplified[(l + 1) * 4 + 2] = verts[num26 * 4 + 2];
					simplified[(l + 1) * 4 + 3] = num26;
				}
				else
				{
					l++;
				}
			}
			float num31 = this.maxEdgeLength / this.cellSize;
			if (num31 > 0f && (buildFlags & 7) != 0)
			{
				int num32 = 0;
				while (num32 < simplified.Length / 4 && simplified.Length / 4 <= 200)
				{
					int num33 = (num32 + 1) % (simplified.Length / 4);
					int num34 = simplified[num32 * 4];
					int num35 = simplified[num32 * 4 + 2];
					int num36 = simplified[num32 * 4 + 3];
					int num37 = simplified[num33 * 4];
					int num38 = simplified[num33 * 4 + 2];
					int num39 = simplified[num33 * 4 + 3];
					int num40 = -1;
					int num41 = (num36 + 1) % num15;
					bool flag4 = false;
					if ((buildFlags & 1) != 0 && (verts[num41 * 4 + 3] & 65535) == 0)
					{
						flag4 = true;
					}
					if ((buildFlags & 2) != 0 && (verts[num41 * 4 + 3] & 131072) == 131072)
					{
						flag4 = true;
					}
					if ((buildFlags & 4) != 0 && (verts[num41 * 4 + 3] & 32768) == 32768)
					{
						flag4 = true;
					}
					if (flag4)
					{
						int num42 = num37 - num34;
						int num43 = num38 / this.field.width - num35 / this.field.width;
						if ((float)(num42 * num42 + num43 * num43) > num31 * num31)
						{
							int num44 = ((num39 < num36) ? (num39 + num15 - num36) : (num39 - num36));
							if (num44 > 1)
							{
								if (num37 > num34 || (num37 == num34 && num38 > num35))
								{
									num40 = (num36 + num44 / 2) % num15;
								}
								else
								{
									num40 = (num36 + (num44 + 1) / 2) % num15;
								}
							}
						}
					}
					if (num40 != -1)
					{
						simplified.Resize(simplified.Length + 4, NativeArrayOptions.UninitializedMemory);
						simplified.AsUnsafeSpan<int>().Move((num32 + 1) * 4, (num32 + 2) * 4, simplified.Length - (num32 + 2) * 4);
						simplified[(num32 + 1) * 4] = verts[num40 * 4];
						simplified[(num32 + 1) * 4 + 1] = verts[num40 * 4 + 1];
						simplified[(num32 + 1) * 4 + 2] = verts[num40 * 4 + 2];
						simplified[(num32 + 1) * 4 + 3] = num40;
					}
					else
					{
						num32++;
					}
				}
			}
			for (int m = 0; m < simplified.Length / 4; m++)
			{
				int num45 = (simplified[m * 4 + 3] + 1) % num15;
				int num46 = simplified[m * 4 + 3];
				simplified[m * 4 + 3] = (verts[num45 * 4 + 3] & 65535) | (verts[num46 * 4 + 3] & 65536);
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0004E46C File Offset: 0x0004C66C
		public void WalkContour(int x, int z, int i, NativeArray<ushort> flags, NativeList<int> verts)
		{
			int num = 0;
			while ((flags[i] & (ushort)(1 << num)) == 0)
			{
				num++;
			}
			int num2 = num;
			int num3 = i;
			int num4 = this.field.areaTypes[i];
			int num5 = 0;
			while (num5++ < 40000)
			{
				if ((flags[i] & (ushort)(1 << num)) != 0)
				{
					bool flag = false;
					bool flag2 = false;
					int num6 = x;
					int cornerHeight = this.GetCornerHeight(x, z, i, num, ref flag);
					int num7 = z;
					switch (num)
					{
					case 0:
						num7 += this.field.width;
						break;
					case 1:
						num6++;
						num7 += this.field.width;
						break;
					case 2:
						num6++;
						break;
					}
					int num8 = 0;
					CompactVoxelSpan compactVoxelSpan = this.field.spans[i];
					if ((long)compactVoxelSpan.GetConnection(num) != 63L)
					{
						int num9 = this.field.cells[this.field.GetNeighbourIndex(x + z, num)].index + compactVoxelSpan.GetConnection(num);
						num8 = this.field.spans[num9].reg;
						if (num4 != this.field.areaTypes[num9])
						{
							flag2 = true;
						}
					}
					if (flag)
					{
						num8 |= 65536;
					}
					if (flag2)
					{
						num8 |= 131072;
					}
					verts.Add(in num6);
					verts.Add(in cornerHeight);
					verts.Add(in num7);
					verts.Add(in num8);
					flags[i] = (ushort)((int)flags[i] & ~(1 << num));
					num = (num + 1) & 3;
				}
				else
				{
					int num10 = -1;
					int num11 = x + VoxelUtilityBurst.DX[num];
					int num12 = z + VoxelUtilityBurst.DZ[num] * this.field.width;
					CompactVoxelSpan compactVoxelSpan2 = this.field.spans[i];
					if ((long)compactVoxelSpan2.GetConnection(num) != 63L)
					{
						num10 = this.field.cells[num11 + num12].index + compactVoxelSpan2.GetConnection(num);
					}
					if (num10 == -1)
					{
						Debug.LogWarning("Degenerate triangles might have been generated.\nUsually this is not a problem, but if you have a static level, try to modify the graph settings slightly to avoid this edge case.");
						return;
					}
					x = num11;
					z = num12;
					i = num10;
					num = (num + 3) & 3;
				}
				if (num3 == i && num2 == num)
				{
					break;
				}
			}
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0004E6B4 File Offset: 0x0004C8B4
		public unsafe int GetCornerHeight(int x, int z, int i, int dir, ref bool isBorderVertex)
		{
			CompactVoxelSpan compactVoxelSpan = this.field.spans[i];
			int num = (int)compactVoxelSpan.y;
			int num2 = (dir + 1) & 3;
			IntPtr intPtr = stackalloc byte[(UIntPtr)16];
			initblk(intPtr, 0, 16);
			uint* ptr = intPtr;
			*ptr = (uint)(this.field.spans[i].reg | (this.field.areaTypes[i] << 16));
			if ((long)compactVoxelSpan.GetConnection(dir) != 63L)
			{
				int neighbourIndex = this.field.GetNeighbourIndex(x + z, dir);
				int num3 = this.field.cells[neighbourIndex].index + compactVoxelSpan.GetConnection(dir);
				CompactVoxelSpan compactVoxelSpan2 = this.field.spans[num3];
				num = Math.Max(num, (int)compactVoxelSpan2.y);
				ptr[1] = (uint)(compactVoxelSpan2.reg | (this.field.areaTypes[num3] << 16));
				if ((long)compactVoxelSpan2.GetConnection(num2) != 63L)
				{
					int neighbourIndex2 = this.field.GetNeighbourIndex(neighbourIndex, num2);
					int num4 = this.field.cells[neighbourIndex2].index + compactVoxelSpan2.GetConnection(num2);
					CompactVoxelSpan compactVoxelSpan3 = this.field.spans[num4];
					num = Math.Max(num, (int)compactVoxelSpan3.y);
					ptr[2] = (uint)(compactVoxelSpan3.reg | (this.field.areaTypes[num4] << 16));
				}
			}
			if ((long)compactVoxelSpan.GetConnection(num2) != 63L)
			{
				int neighbourIndex3 = this.field.GetNeighbourIndex(x + z, num2);
				int num5 = this.field.cells[neighbourIndex3].index + compactVoxelSpan.GetConnection(num2);
				CompactVoxelSpan compactVoxelSpan4 = this.field.spans[num5];
				num = Math.Max(num, (int)compactVoxelSpan4.y);
				ptr[3] = (uint)(compactVoxelSpan4.reg | (this.field.areaTypes[num5] << 16));
				if ((long)compactVoxelSpan4.GetConnection(dir) != 63L)
				{
					int neighbourIndex4 = this.field.GetNeighbourIndex(neighbourIndex3, dir);
					int num6 = this.field.cells[neighbourIndex4].index + compactVoxelSpan4.GetConnection(dir);
					CompactVoxelSpan compactVoxelSpan5 = this.field.spans[num6];
					num = Math.Max(num, (int)compactVoxelSpan5.y);
					ptr[2] = (uint)(compactVoxelSpan5.reg | (this.field.areaTypes[num6] << 16));
				}
			}
			bool flag = *ptr != 0U && ptr[1] != 0U && ptr[2] != 0U && ptr[3] > 0U;
			for (int j = 0; j < 4; j++)
			{
				int num7 = j;
				int num8 = (j + 1) & 3;
				int num9 = (j + 2) & 3;
				int num10 = (j + 3) & 3;
				bool flag2 = (ptr[num7] & ptr[num8] & 32768U) != 0U && ptr[num7] == ptr[num8];
				bool flag3 = ((ptr[num9] | ptr[num10]) & 32768U) == 0U;
				bool flag4 = ptr[num9] >> 16 == ptr[num10] >> 16;
				if (flag2 && flag3 && flag4 && flag)
				{
					isBorderVertex = true;
					break;
				}
			}
			return num;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0004EA08 File Offset: 0x0004CC08
		private static void RemoveRange(NativeList<int> arr, int index, int count)
		{
			for (int i = index; i < arr.Length - count; i++)
			{
				arr[i] = arr[i + count];
			}
			arr.Resize(arr.Length - count, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0004EA4C File Offset: 0x0004CC4C
		private static void RemoveDegenerateSegments(NativeList<int> simplified)
		{
			for (int i = 0; i < simplified.Length / 4; i++)
			{
				int num = i + 1;
				if (num >= simplified.Length / 4)
				{
					num = 0;
				}
				if (simplified[i * 4] == simplified[num * 4] && simplified[i * 4 + 2] == simplified[num * 4 + 2])
				{
					JobBuildContours.RemoveRange(simplified, i, 4);
				}
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0004EAB8 File Offset: 0x0004CCB8
		private int CalcAreaOfPolygon2D(NativeArray<int> verts, int vertexStartIndex, int nverts)
		{
			int num = 0;
			int i = 0;
			int num2 = nverts - 1;
			while (i < nverts)
			{
				int num3 = vertexStartIndex + i * 4;
				int num4 = vertexStartIndex + num2 * 4;
				num += verts[num3] * (verts[num4 + 2] / this.field.width) - verts[num4] * (verts[num3 + 2] / this.field.width);
				num2 = i++;
			}
			return (num + 1) / 2;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0004EB30 File Offset: 0x0004CD30
		private static bool Ileft(NativeArray<int> verts, int a, int b, int c)
		{
			return (verts[b] - verts[a]) * (verts[c + 2] - verts[a + 2]) - (verts[c] - verts[a]) * (verts[b + 2] - verts[a + 2]) <= 0;
		}

		// Token: 0x04000916 RID: 2326
		public CompactVoxelField field;

		// Token: 0x04000917 RID: 2327
		public float maxError;

		// Token: 0x04000918 RID: 2328
		public float maxEdgeLength;

		// Token: 0x04000919 RID: 2329
		public int buildFlags;

		// Token: 0x0400091A RID: 2330
		public float cellSize;

		// Token: 0x0400091B RID: 2331
		public NativeList<VoxelContour> outputContours;

		// Token: 0x0400091C RID: 2332
		public NativeList<int> outputVerts;
	}
}
