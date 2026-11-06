using System;
using Pathfinding.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001E2 RID: 482
	public struct LinkedVoxelField : IArenaDisposable
	{
		// Token: 0x06000D60 RID: 3424 RVA: 0x0004D08C File Offset: 0x0004B28C
		public LinkedVoxelField(int width, int depth, int height)
		{
			this.width = width;
			this.depth = depth;
			this.height = height;
			this.flatten = true;
			this.linkedSpans = new NativeList<LinkedVoxelSpan>(0, Allocator.Persistent);
			this.removedStack = new NativeList<int>(128, Allocator.Persistent);
			this.linkedCellMinMax = new NativeList<CellMinMax>(0, Allocator.Persistent);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0004D0EF File Offset: 0x0004B2EF
		void IArenaDisposable.DisposeWith(DisposeArena arena)
		{
			arena.Add<LinkedVoxelSpan>(this.linkedSpans);
			arena.Add<int>(this.removedStack);
			arena.Add<CellMinMax>(this.linkedCellMinMax);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0004D118 File Offset: 0x0004B318
		public void ResetLinkedVoxelSpans()
		{
			int num = this.width * this.depth;
			LinkedVoxelSpan linkedVoxelSpan = new LinkedVoxelSpan(uint.MaxValue, uint.MaxValue, -1, -1);
			this.linkedSpans.ResizeUninitialized(num);
			this.linkedCellMinMax.Resize(num, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < num; i++)
			{
				this.linkedSpans[i] = linkedVoxelSpan;
				this.linkedCellMinMax[i] = new CellMinMax
				{
					objectID = -1,
					min = 0,
					max = 0
				};
			}
			this.removedStack.Clear();
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0004D1A8 File Offset: 0x0004B3A8
		private void PushToSpanRemovedStack(int index)
		{
			this.removedStack.Add(in index);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0004D1B8 File Offset: 0x0004B3B8
		public int GetSpanCount()
		{
			int num = 0;
			int num2 = this.width * this.depth;
			for (int i = 0; i < num2; i++)
			{
				int num3 = i;
				while (num3 != -1 && this.linkedSpans[num3].bottom != 4294967295U)
				{
					num += ((this.linkedSpans[num3].area != 0) ? 1 : 0);
					num3 = this.linkedSpans[num3].next;
				}
			}
			return num;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0004D22C File Offset: 0x0004B42C
		public void ResolveSolid(int index, int objectID, int voxelWalkableClimb)
		{
			CellMinMax cellMinMax = this.linkedCellMinMax[index];
			if (cellMinMax.objectID != objectID)
			{
				return;
			}
			if (cellMinMax.min < cellMinMax.max - 1)
			{
				this.AddLinkedSpan(index, cellMinMax.min, cellMinMax.max - 1, 0, voxelWalkableClimb, objectID);
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0004D278 File Offset: 0x0004B478
		public void SetWalkableBackground()
		{
			int num = this.width * this.depth;
			for (int i = 0; i < num; i++)
			{
				this.linkedSpans[i] = new LinkedVoxelSpan(0U, 1U, 1);
			}
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0004D2B4 File Offset: 0x0004B4B4
		public void AddFlattenedSpan(int index, int area)
		{
			if (this.linkedSpans[index].bottom == 4294967295U)
			{
				this.linkedSpans[index] = new LinkedVoxelSpan(0U, 1U, area);
				return;
			}
			this.linkedSpans[index] = new LinkedVoxelSpan(0U, 1U, (this.linkedSpans[index].area == 0 || area == 0) ? 0 : math.max(this.linkedSpans[index].area, area));
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0004D330 File Offset: 0x0004B530
		public void AddLinkedSpan(int index, int bottom, int top, int area, int voxelWalkableClimb, int objectID)
		{
			CellMinMax cellMinMax = this.linkedCellMinMax[index];
			if (cellMinMax.objectID != objectID)
			{
				this.linkedCellMinMax[index] = new CellMinMax
				{
					objectID = objectID,
					min = bottom,
					max = top
				};
			}
			else
			{
				cellMinMax.min = math.min(cellMinMax.min, bottom);
				cellMinMax.max = math.max(cellMinMax.max, top);
				this.linkedCellMinMax[index] = cellMinMax;
			}
			top = math.min(top, this.height);
			bottom = math.max(bottom, 0);
			if (bottom >= top)
			{
				return;
			}
			uint num = (uint)top;
			uint num2 = (uint)bottom;
			if (this.linkedSpans[index].bottom == 4294967295U)
			{
				this.linkedSpans[index] = new LinkedVoxelSpan(num2, num, area);
				return;
			}
			int num3 = -1;
			int num4 = index;
			while (index != -1)
			{
				LinkedVoxelSpan linkedVoxelSpan = this.linkedSpans[index];
				if (linkedVoxelSpan.bottom > num)
				{
					break;
				}
				if (linkedVoxelSpan.top < num2)
				{
					num3 = index;
					index = linkedVoxelSpan.next;
				}
				else
				{
					if (math.abs((int)(num - linkedVoxelSpan.top)) < voxelWalkableClimb && (area == 0 || linkedVoxelSpan.area == 0))
					{
						area = math.max(area, linkedVoxelSpan.area);
					}
					else if (num < linkedVoxelSpan.top)
					{
						area = linkedVoxelSpan.area;
					}
					num2 = math.min(linkedVoxelSpan.bottom, num2);
					num = math.max(linkedVoxelSpan.top, num);
					int next = linkedVoxelSpan.next;
					if (num3 != -1)
					{
						LinkedVoxelSpan linkedVoxelSpan2 = this.linkedSpans[num3];
						linkedVoxelSpan2.next = next;
						this.linkedSpans[num3] = linkedVoxelSpan2;
						this.PushToSpanRemovedStack(index);
						index = next;
					}
					else
					{
						if (next == -1)
						{
							this.linkedSpans[num4] = new LinkedVoxelSpan(num2, num, area);
							return;
						}
						this.linkedSpans[num4] = this.linkedSpans[next];
						this.PushToSpanRemovedStack(next);
					}
				}
			}
			int num5;
			if (this.removedStack.Length > 0)
			{
				num5 = this.removedStack[this.removedStack.Length - 1];
				this.removedStack.RemoveAtSwapBack(this.removedStack.Length - 1);
			}
			else
			{
				num5 = this.linkedSpans.Length;
				this.linkedSpans.Resize(this.linkedSpans.Length + 1, NativeArrayOptions.UninitializedMemory);
			}
			if (num3 != -1)
			{
				this.linkedSpans[num5] = new LinkedVoxelSpan(num2, num, area, this.linkedSpans[num3].next);
				LinkedVoxelSpan linkedVoxelSpan3 = this.linkedSpans[num3];
				linkedVoxelSpan3.next = num5;
				this.linkedSpans[num3] = linkedVoxelSpan3;
				return;
			}
			this.linkedSpans[num5] = this.linkedSpans[num4];
			this.linkedSpans[num4] = new LinkedVoxelSpan(num2, num, area, num5);
		}

		// Token: 0x04000903 RID: 2307
		public const uint MaxHeight = 65536U;

		// Token: 0x04000904 RID: 2308
		public const int MaxHeightInt = 65536;

		// Token: 0x04000905 RID: 2309
		public const uint InvalidSpanValue = 4294967295U;

		// Token: 0x04000906 RID: 2310
		public const float AvgSpanLayerCountEstimate = 8f;

		// Token: 0x04000907 RID: 2311
		public int width;

		// Token: 0x04000908 RID: 2312
		public int depth;

		// Token: 0x04000909 RID: 2313
		public int height;

		// Token: 0x0400090A RID: 2314
		public bool flatten;

		// Token: 0x0400090B RID: 2315
		public NativeList<LinkedVoxelSpan> linkedSpans;

		// Token: 0x0400090C RID: 2316
		private NativeList<int> removedStack;

		// Token: 0x0400090D RID: 2317
		private NativeList<CellMinMax> linkedCellMinMax;
	}
}
