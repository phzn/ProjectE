using System;
using Pathfinding.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001DE RID: 478
	public struct CompactVoxelField : IArenaDisposable
	{
		// Token: 0x06000D58 RID: 3416 RVA: 0x0004CDE0 File Offset: 0x0004AFE0
		public CompactVoxelField(int width, int depth, int voxelWalkableHeight, Allocator allocator)
		{
			this.spans = new NativeList<CompactVoxelSpan>(0, allocator);
			this.cells = new NativeList<CompactVoxelCell>(0, allocator);
			this.areaTypes = new NativeList<int>(0, allocator);
			this.width = width;
			this.depth = depth;
			this.voxelWalkableHeight = voxelWalkableHeight;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0004CE3B File Offset: 0x0004B03B
		void IArenaDisposable.DisposeWith(DisposeArena arena)
		{
			arena.Add<CompactVoxelSpan>(this.spans);
			arena.Add<CompactVoxelCell>(this.cells);
			arena.Add<int>(this.areaTypes);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0004CE61 File Offset: 0x0004B061
		public int GetNeighbourIndex(int index, int direction)
		{
			return index + VoxelUtilityBurst.DX[direction] + VoxelUtilityBurst.DZ[direction] * this.width;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0004CE7C File Offset: 0x0004B07C
		public void BuildFromLinkedField(LinkedVoxelField field)
		{
			int num = 0;
			int num2 = field.width;
			int num3 = field.depth;
			int num4 = num2 * num3;
			int spanCount = field.GetSpanCount();
			this.spans.Resize(spanCount, NativeArrayOptions.UninitializedMemory);
			this.areaTypes.Resize(spanCount, NativeArrayOptions.UninitializedMemory);
			this.cells.Resize(num4, NativeArrayOptions.UninitializedMemory);
			NativeList<LinkedVoxelSpan> linkedSpans = field.linkedSpans;
			for (int i = 0; i < num4; i += num2)
			{
				for (int j = 0; j < num2; j++)
				{
					int num5 = j + i;
					if (linkedSpans[num5].bottom == 4294967295U)
					{
						this.cells[j + i] = new CompactVoxelCell(0, 0);
					}
					else
					{
						int num6 = num;
						int num7 = 0;
						while (num5 != -1)
						{
							if (linkedSpans[num5].area != 0)
							{
								int top = (int)linkedSpans[num5].top;
								int next = linkedSpans[num5].next;
								int num8 = (int)((next != -1) ? linkedSpans[next].bottom : 65536U);
								this.spans[num] = new CompactVoxelSpan((ushort)math.min(top, 65535), (uint)math.min(num8 - top, 65535));
								this.areaTypes[num] = linkedSpans[num5].area;
								num++;
								num7++;
							}
							num5 = linkedSpans[num5].next;
						}
						this.cells[j + i] = new CompactVoxelCell(num6, num7);
					}
				}
			}
		}

		// Token: 0x040008F1 RID: 2289
		public const int UnwalkableArea = 0;

		// Token: 0x040008F2 RID: 2290
		public const uint NotConnected = 63U;

		// Token: 0x040008F3 RID: 2291
		public readonly int voxelWalkableHeight;

		// Token: 0x040008F4 RID: 2292
		public readonly int width;

		// Token: 0x040008F5 RID: 2293
		public readonly int depth;

		// Token: 0x040008F6 RID: 2294
		public NativeList<CompactVoxelSpan> spans;

		// Token: 0x040008F7 RID: 2295
		public NativeList<CompactVoxelCell> cells;

		// Token: 0x040008F8 RID: 2296
		public NativeList<int> areaTypes;

		// Token: 0x040008F9 RID: 2297
		public const int MaxLayers = 65535;
	}
}
