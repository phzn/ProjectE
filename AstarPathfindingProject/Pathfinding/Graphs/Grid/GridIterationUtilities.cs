using System;
using Pathfinding.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x0200020A RID: 522
	public static class GridIterationUtilities
	{
		// Token: 0x06000DE7 RID: 3559 RVA: 0x000552E0 File Offset: 0x000534E0
		public static void ForEachCellIn3DSlice<T>(Slice3D slice, ref T action) where T : struct, GridIterationUtilities.ISliceAction
		{
			int3 size = slice.slice.size;
			ValueTuple<int, int, int> outerStrides = slice.outerStrides;
			int item = outerStrides.Item2;
			int item2 = outerStrides.Item3;
			int outerStartIndex = slice.outerStartIndex;
			uint num = 0U;
			for (int i = 0; i < size.y; i++)
			{
				for (int j = 0; j < size.z; j++)
				{
					int num2 = i * item + j * item2 + outerStartIndex;
					int k = 0;
					while (k < size.x)
					{
						action.Execute((uint)(num2 + k), num);
						k++;
						num += 1U;
					}
				}
			}
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00055384 File Offset: 0x00053584
		public static void ForEachCellIn3DSliceWithCoords<T>(Slice3D slice, ref T action) where T : struct, GridIterationUtilities.ISliceActionWithCoords
		{
			int3 size = slice.slice.size;
			ValueTuple<int, int, int> outerStrides = slice.outerStrides;
			int item = outerStrides.Item2;
			int item2 = outerStrides.Item3;
			int outerStartIndex = slice.outerStartIndex;
			uint num = (uint)(size.x * size.y * size.z - 1);
			for (int i = size.y - 1; i >= 0; i--)
			{
				for (int j = size.z - 1; j >= 0; j--)
				{
					int num2 = i * item + j * item2 + outerStartIndex;
					int k = size.x - 1;
					while (k >= 0)
					{
						action.Execute((uint)(num2 + k), num, new int3(k, i, j));
						k--;
						num -= 1U;
					}
				}
			}
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0005544C File Offset: 0x0005364C
		public static void ForEachCellIn3DArray<T>(int3 size, ref T action) where T : struct, GridIterationUtilities.ICellAction
		{
			uint num = (uint)(size.x * size.y * size.z - 1);
			for (int i = size.y - 1; i >= 0; i--)
			{
				for (int j = size.z - 1; j >= 0; j--)
				{
					int k = size.x - 1;
					while (k >= 0)
					{
						action.Execute(num, k, i, j);
						k--;
						num -= 1U;
					}
				}
			}
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x000554C0 File Offset: 0x000536C0
		public static void ForEachNode<T>(int3 arrayBounds, NativeArray<float4> nodeNormals, ref T callback) where T : struct, GridIterationUtilities.INodeModifier
		{
			int num = 0;
			for (int i = 0; i < arrayBounds.y; i++)
			{
				for (int j = 0; j < arrayBounds.z; j++)
				{
					int k = 0;
					while (k < arrayBounds.x)
					{
						if (math.any(nodeNormals[num]))
						{
							callback.ModifyNode(num, k, i, j);
						}
						k++;
						num++;
					}
				}
			}
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00055528 File Offset: 0x00053728
		public unsafe static void FilterNodeConnections<T>(IntBounds bounds, NativeArray<ulong> nodeConnections, bool layeredDataLayout, ref T filter) where T : struct, GridIterationUtilities.IConnectionFilter
		{
			int3 size = bounds.size;
			int* ptr = stackalloc int[(UIntPtr)32];
			for (int i = 0; i < 8; i++)
			{
				ptr[i] = GridGraph.neighbourZOffsets[i] * size.x + GridGraph.neighbourXOffsets[i];
			}
			int num = size.x * size.z;
			int num2 = 0;
			for (int j = 0; j < size.y; j++)
			{
				for (int k = 0; k < size.z; k++)
				{
					int l = 0;
					while (l < size.x)
					{
						ulong num3 = nodeConnections[num2];
						if (layeredDataLayout)
						{
							for (int m = 0; m < 8; m++)
							{
								int num4 = (int)((num3 >> 4 * m) & 15UL);
								if (num4 != 15 && !filter.IsValidConnection(num2, l, j, k, m, num2 + ptr[m] + (num4 - j) * num))
								{
									num3 |= 15UL << 4 * m;
								}
							}
						}
						else
						{
							for (int n = 0; n < 8; n++)
							{
								if (((int)num3 & (1 << n)) != 0 && !filter.IsValidConnection(num2, l, j, k, n, num2 + ptr[n]))
								{
									num3 &= ~(1UL << n);
								}
							}
						}
						nodeConnections[num2] = num3;
						l++;
						num2++;
					}
				}
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x000556A0 File Offset: 0x000538A0
		public static int? GetNeighbourDataIndex(IntBounds bounds, NativeArray<ulong> nodeConnections, bool layeredDataLayout, int dataX, int dataLayer, int dataZ, int direction)
		{
			int num = GridGraph.neighbourXOffsets[direction];
			int num2 = GridGraph.neighbourZOffsets[direction];
			int num3 = dataX + num;
			int num4 = dataZ + num2;
			int x = bounds.size.x;
			int num5 = bounds.size.x * bounds.size.z;
			int num6 = dataLayer * num5 + dataZ * x + dataX;
			int num7 = num4 * x + num3;
			if (layeredDataLayout)
			{
				ulong num8 = (nodeConnections[num6] >> 4 * direction) & 15UL;
				if (num8 == 15UL)
				{
					return null;
				}
				if (num3 < 0 || num4 < 0 || num3 >= bounds.size.x || num4 >= bounds.size.z)
				{
					throw new Exception("Node has an invalid connection to a node outside the bounds of the graph");
				}
				num7 += (int)num8 * num5;
			}
			else if ((nodeConnections[num6] & (1UL << direction)) == 0UL)
			{
				return null;
			}
			if (num3 < 0 || num4 < 0 || num3 >= bounds.size.x || num4 >= bounds.size.z)
			{
				throw new Exception("Node has an invalid connection to a node outside the bounds of the graph");
			}
			return new int?(num7);
		}

		// Token: 0x0200020B RID: 523
		public interface ISliceAction
		{
			// Token: 0x06000DED RID: 3565
			void Execute(uint outerIdx, uint innerIdx);
		}

		// Token: 0x0200020C RID: 524
		public interface ISliceActionWithCoords
		{
			// Token: 0x06000DEE RID: 3566
			void Execute(uint outerIdx, uint innerIdx, int3 innerCoords);
		}

		// Token: 0x0200020D RID: 525
		public interface ICellAction
		{
			// Token: 0x06000DEF RID: 3567
			void Execute(uint idx, int x, int y, int z);
		}

		// Token: 0x0200020E RID: 526
		public interface INodeModifier
		{
			// Token: 0x06000DF0 RID: 3568
			void ModifyNode(int dataIndex, int dataX, int dataLayer, int dataZ);
		}

		// Token: 0x0200020F RID: 527
		public interface IConnectionFilter
		{
			// Token: 0x06000DF1 RID: 3569
			bool IsValidConnection(int dataIndex, int dataX, int dataLayer, int dataZ, int direction, int neighbourDataIndex);
		}
	}
}
