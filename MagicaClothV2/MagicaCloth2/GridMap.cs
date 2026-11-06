using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x020000EE RID: 238
	public class GridMap<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
	{
		// Token: 0x06000431 RID: 1073 RVA: 0x0002A59C File Offset: 0x0002879C
		public GridMap(int capacity = 0)
		{
			this.gridMap = new NativeParallelMultiHashMap<int3, T>(capacity, Allocator.Persistent);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0002A5B6 File Offset: 0x000287B6
		public void Dispose()
		{
			if (this.gridMap.IsCreated)
			{
				this.gridMap.Dispose();
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0002A5D0 File Offset: 0x000287D0
		public NativeParallelMultiHashMap<int3, T> GetMultiHashMap()
		{
			return this.gridMap;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0002A5D8 File Offset: 0x000287D8
		public int DataCount
		{
			get
			{
				return this.gridMap.Count();
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0002A5E8 File Offset: 0x000287E8
		public static GridMap<T>.GridEnumerator GetArea(int3 startGrid, int3 endGrid, NativeParallelMultiHashMap<int3, T> gridMap)
		{
			return new GridMap<T>.GridEnumerator
			{
				gridMap = gridMap,
				startGrid = math.min(startGrid, endGrid),
				endGrid = math.max(startGrid, endGrid),
				currentGrid = math.min(startGrid, endGrid),
				isFirst = true
			};
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0002A638 File Offset: 0x00028838
		public static GridMap<T>.GridEnumerator GetArea(float3 pos, float radius, NativeParallelMultiHashMap<int3, T> gridMap, float gridSize)
		{
			int3 grid = GridMap<T>.GetGrid(pos - radius, gridSize);
			int3 grid2 = GridMap<T>.GetGrid(pos + radius, gridSize);
			return GridMap<T>.GetArea(grid, grid2, gridMap);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000BB93 File Offset: 0x00009D93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 GetGrid(float3 pos, float gridSize)
		{
			return new int3(math.floor(pos / gridSize));
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0002A667 File Offset: 0x00028867
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AddGrid(int3 grid, T data, NativeParallelMultiHashMap<int3, T> gridMap)
		{
			gridMap.Add(grid, data);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0002A674 File Offset: 0x00028874
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 AddGrid(float3 pos, T data, NativeParallelMultiHashMap<int3, T> gridMap, float gridSize)
		{
			int3 grid = GridMap<T>.GetGrid(pos, gridSize);
			gridMap.Add(grid, data);
			return grid;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0002A694 File Offset: 0x00028894
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int3 AddGrid(float3 pos, T data, NativeParallelMultiHashMap<int3, T>.ParallelWriter gridMap, float gridSize)
		{
			int3 grid = GridMap<T>.GetGrid(pos, gridSize);
			gridMap.Add(grid, data);
			return grid;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0002A6B4 File Offset: 0x000288B4
		public static bool RemoveGrid(int3 grid, T data, NativeParallelMultiHashMap<int3, T> gridMap)
		{
			T t;
			NativeParallelMultiHashMapIterator<int3> nativeParallelMultiHashMapIterator;
			if (gridMap.ContainsKey(grid) && gridMap.TryGetFirstValue(grid, out t, out nativeParallelMultiHashMapIterator))
			{
				while (!t.Equals(data))
				{
					if (!gridMap.TryGetNextValue(out t, ref nativeParallelMultiHashMapIterator))
					{
						return false;
					}
				}
				gridMap.Remove(nativeParallelMultiHashMapIterator);
				return true;
			}
			return false;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0002A706 File Offset: 0x00028906
		public static bool MoveGrid(int3 fromGrid, int3 toGrid, T data, NativeParallelMultiHashMap<int3, T> gridMap)
		{
			if (fromGrid.Equals(toGrid))
			{
				return false;
			}
			GridMap<T>.RemoveGrid(fromGrid, data, gridMap);
			GridMap<T>.AddGrid(toGrid, data, gridMap);
			return true;
		}

		// Token: 0x04000862 RID: 2146
		private NativeParallelMultiHashMap<int3, T> gridMap;

		// Token: 0x020000EF RID: 239
		public struct GridEnumerator : IEnumerator<int3>, IEnumerator, IDisposable
		{
			// Token: 0x17000074 RID: 116
			// (get) Token: 0x0600043D RID: 1085 RVA: 0x0002A726 File Offset: 0x00028926
			public int3 Current
			{
				get
				{
					return this.currentGrid;
				}
			}

			// Token: 0x17000075 RID: 117
			// (get) Token: 0x0600043E RID: 1086 RVA: 0x0002A72E File Offset: 0x0002892E
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0600043F RID: 1087 RVA: 0x0000706D File Offset: 0x0000526D
			public void Dispose()
			{
			}

			// Token: 0x06000440 RID: 1088 RVA: 0x0002A73C File Offset: 0x0002893C
			public bool MoveNext()
			{
				if (this.isFirst)
				{
					this.isFirst = false;
					return true;
				}
				this.currentGrid.x = this.currentGrid.x + 1;
				if (this.currentGrid.x > this.endGrid.x)
				{
					this.currentGrid.x = this.startGrid.x;
					this.currentGrid.y = this.currentGrid.y + 1;
					if (this.currentGrid.y > this.endGrid.y)
					{
						this.currentGrid.y = this.startGrid.y;
						this.currentGrid.z = this.currentGrid.z + 1;
						if (this.currentGrid.z > this.endGrid.z)
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x06000441 RID: 1089 RVA: 0x0002A801 File Offset: 0x00028A01
			public void Reset()
			{
				this.currentGrid = this.startGrid;
				this.isFirst = true;
			}

			// Token: 0x06000442 RID: 1090 RVA: 0x0002A816 File Offset: 0x00028A16
			public GridMap<T>.GridEnumerator GetEnumerator()
			{
				return this;
			}

			// Token: 0x04000863 RID: 2147
			internal NativeParallelMultiHashMap<int3, T> gridMap;

			// Token: 0x04000864 RID: 2148
			internal int3 startGrid;

			// Token: 0x04000865 RID: 2149
			internal int3 endGrid;

			// Token: 0x04000866 RID: 2150
			internal int3 currentGrid;

			// Token: 0x04000867 RID: 2151
			internal bool isFirst;
		}
	}
}
