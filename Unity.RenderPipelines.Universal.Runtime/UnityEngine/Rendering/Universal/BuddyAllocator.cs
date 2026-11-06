using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000088 RID: 136
	internal struct BuddyAllocator : IDisposable
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001EBC5 File Offset: 0x0001CDC5
		private ref BuddyAllocator.Header header
		{
			get
			{
				return UnsafeUtility.AsRef<BuddyAllocator.Header>(this.m_Data);
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001EBD2 File Offset: 0x0001CDD2
		private NativeArray<int> freeMaskCounts
		{
			get
			{
				return this.GetNativeArray<int>(this.m_ActiveFreeMaskCounts.Item1, this.m_ActiveFreeMaskCounts.Item2);
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001EBF0 File Offset: 0x0001CDF0
		private NativeArray<ulong> freeMasksStorage
		{
			get
			{
				return this.GetNativeArray<ulong>(this.m_FreeMasksStorage.Item1, this.m_FreeMasksStorage.Item2);
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001EC10 File Offset: 0x0001CE10
		private NativeArray<ulong> FreeMasks(int level)
		{
			return this.freeMasksStorage.GetSubArray(BuddyAllocator.LevelOffset64(level, this.header.branchingOrder), BuddyAllocator.LevelLength64(level, this.header.branchingOrder));
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0001EC4D File Offset: 0x0001CE4D
		private NativeArray<int> freeMaskIndicesStorage
		{
			get
			{
				return this.GetNativeArray<int>(this.m_FreeMaskIndicesStorage.Item1, this.m_FreeMaskIndicesStorage.Item2);
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001EC6C File Offset: 0x0001CE6C
		private NativeArray<int> FreeMaskIndices(int level)
		{
			return this.freeMaskIndicesStorage.GetSubArray(BuddyAllocator.LevelOffset64(level, this.header.branchingOrder), BuddyAllocator.LevelLength64(level, this.header.branchingOrder));
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0001ECA9 File Offset: 0x0001CEA9
		public int levelCount
		{
			get
			{
				return this.header.levelCount;
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001ECB8 File Offset: 0x0001CEB8
		public unsafe BuddyAllocator(int levelCount, int branchingOrder, Allocator allocator = Allocator.Persistent)
		{
			int num = sizeof(BuddyAllocator.Header);
			this.m_ActiveFreeMaskCounts = BuddyAllocator.AllocateRange<int>(levelCount, ref num);
			this.m_FreeMasksStorage = BuddyAllocator.AllocateRange<ulong>(BuddyAllocator.LevelOffset64(levelCount, branchingOrder), ref num);
			this.m_FreeMaskIndicesStorage = BuddyAllocator.AllocateRange<int>(BuddyAllocator.LevelOffset64(levelCount, branchingOrder), ref num);
			this.m_Data = UnsafeUtility.Malloc((long)num, 64, allocator);
			UnsafeUtility.MemClear(this.m_Data, (long)num);
			this.m_Allocator = allocator;
			*this.header = new BuddyAllocator.Header
			{
				branchingOrder = branchingOrder,
				levelCount = levelCount
			};
			this.FreeMasks(0)[0] = 15UL;
			this.freeMaskCounts[0] = 1;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001ED70 File Offset: 0x0001CF70
		public bool TryAllocate(int requestedLevel, out BuddyAllocation allocation)
		{
			allocation = default(BuddyAllocation);
			int i = requestedLevel;
			NativeArray<int> freeMaskCounts = this.freeMaskCounts;
			while (i >= 0 && freeMaskCounts[i] <= 0)
			{
				i--;
			}
			if (i < 0)
			{
				return false;
			}
			NativeArray<int> nativeArray = this.FreeMaskIndices(i);
			int num = i;
			int num2 = freeMaskCounts[num] - 1;
			freeMaskCounts[num] = num2;
			int num3 = nativeArray[num2];
			NativeArray<ulong> nativeArray2 = this.FreeMasks(i);
			ulong num4 = nativeArray2[num3];
			int num5 = math.tzcnt(num4);
			num4 ^= 1UL << num5;
			nativeArray2[num3] = num4;
			if (num4 != 0UL)
			{
				num2 = i;
				num = freeMaskCounts[num2];
				freeMaskCounts[num2] = num + 1;
				nativeArray[num] = num3;
			}
			int num6 = num3 * 64 + num5;
			while (i < requestedLevel)
			{
				i++;
				num6 <<= this.header.branchingOrder;
				int num7 = num6 >> 6;
				int num8 = num6 & 63;
				NativeArray<ulong> nativeArray3 = this.FreeMasks(i);
				ulong num9 = nativeArray3[num7];
				if (num9 == 0UL)
				{
					NativeArray<int> nativeArray4 = this.FreeMaskIndices(i);
					num = i;
					num2 = freeMaskCounts[num];
					freeMaskCounts[num] = num2 + 1;
					nativeArray4[num2] = num7;
				}
				num9 |= (1UL << BuddyAllocator.Pow2(this.header.branchingOrder)) - 2UL << num8;
				nativeArray3[num7] = num9;
			}
			allocation.level = i;
			allocation.index = num6;
			return true;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001EEEC File Offset: 0x0001D0EC
		public void Free(BuddyAllocation allocation)
		{
			int i = allocation.level;
			int num = allocation.index;
			while (i >= 0)
			{
				int num2 = num >> 6;
				int num3 = num & 63;
				NativeArray<ulong> nativeArray = this.FreeMasks(i);
				ulong num4 = nativeArray[num2];
				bool flag = num4 == 0UL;
				num4 |= 1UL << num3;
				NativeArray<int> nativeArray2 = this.FreeMaskIndices(i);
				NativeArray<int> freeMaskCounts = this.freeMaskCounts;
				ulong num5 = (1UL << BuddyAllocator.Pow2(this.header.branchingOrder)) - 1UL << (num3 >> this.header.branchingOrder) * BuddyAllocator.Pow2(this.header.branchingOrder);
				if (i == 0 || (~(num4 != 0UL) & num5) != 0UL)
				{
					nativeArray[num2] = num4;
					if (flag)
					{
						int num6 = i;
						int num7 = freeMaskCounts[num6];
						freeMaskCounts[num6] = num7 + 1;
						nativeArray2[num7] = num2;
						return;
					}
					break;
				}
				else
				{
					num4 &= ~num5;
					nativeArray[num2] = num4;
					if (!flag && num4 == 0UL)
					{
						for (int j = 0; j < nativeArray2.Length; j++)
						{
							if (nativeArray2[j] == num2)
							{
								int num8 = j;
								int num7 = i;
								int num6 = freeMaskCounts[num7] - 1;
								freeMaskCounts[num7] = num6;
								nativeArray2[num8] = nativeArray2[num6];
								break;
							}
						}
					}
					i--;
					num >>= this.header.branchingOrder;
				}
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001F054 File Offset: 0x0001D254
		public unsafe void Dispose()
		{
			UnsafeUtility.Free(this.m_Data, this.m_Allocator);
			this.m_Data = default(void*);
			this.m_Allocator = Allocator.Invalid;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001F07A File Offset: 0x0001D27A
		private NativeArray<T> GetNativeArray<T>(int offset, int length) where T : struct
		{
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(BuddyAllocator.PtrAdd(this.m_Data, offset), length, this.m_Allocator);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001F094 File Offset: 0x0001D294
		private static int LevelOffset(int level, int branchingOrder)
		{
			return BuddyAllocator.Pow2(branchingOrder) * (BuddyAllocator.Pow2(branchingOrder * (level - 1) + branchingOrder) - 1) / (BuddyAllocator.Pow2(branchingOrder) - 1);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001F0B4 File Offset: 0x0001D2B4
		private static int LevelLength(int level, int branchingOrder)
		{
			return BuddyAllocator.Pow2N(branchingOrder, level + 1);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001F0BF File Offset: 0x0001D2BF
		private static int LevelOffset64(int level, int branchingOrder)
		{
			return math.min(level, 6 / branchingOrder) + BuddyAllocator.LevelOffset(math.max(0, level - 6 / branchingOrder), branchingOrder);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001F0DC File Offset: 0x0001D2DC
		private static int LevelLength64(int level, int branchingOrder)
		{
			return BuddyAllocator.Pow2N(branchingOrder, math.max(0, level - 6 / branchingOrder + 1));
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001F0F1 File Offset: 0x0001D2F1
		private static ValueTuple<int, int> AllocateRange<T>(int length, ref int dataSize) where T : struct
		{
			dataSize = BuddyAllocator.AlignForward(dataSize, UnsafeUtility.AlignOf<T>());
			ValueTuple<int, int> valueTuple = new ValueTuple<int, int>(dataSize, length);
			dataSize += length * UnsafeUtility.SizeOf<T>();
			return valueTuple;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001F118 File Offset: 0x0001D318
		private static int AlignForward(int offset, int alignment)
		{
			int num = offset % alignment;
			if (num != 0)
			{
				offset += alignment - num;
			}
			return offset;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001F134 File Offset: 0x0001D334
		private unsafe static void* PtrAdd(void* ptr, int bytes)
		{
			return (void*)((IntPtr)ptr + bytes);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001F147 File Offset: 0x0001D347
		private static int Pow2(int n)
		{
			return 1 << n;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001F14F File Offset: 0x0001D34F
		private static int Pow2N(int x, int n)
		{
			return 1 << x * n;
		}

		// Token: 0x040003C3 RID: 963
		private unsafe void* m_Data;

		// Token: 0x040003C4 RID: 964
		private ValueTuple<int, int> m_ActiveFreeMaskCounts;

		// Token: 0x040003C5 RID: 965
		private ValueTuple<int, int> m_FreeMasksStorage;

		// Token: 0x040003C6 RID: 966
		private ValueTuple<int, int> m_FreeMaskIndicesStorage;

		// Token: 0x040003C7 RID: 967
		private Allocator m_Allocator;

		// Token: 0x02000198 RID: 408
		private struct Header
		{
			// Token: 0x04000AA9 RID: 2729
			public int branchingOrder;

			// Token: 0x04000AAA RID: 2730
			public int levelCount;

			// Token: 0x04000AAB RID: 2731
			public int allocationCount;

			// Token: 0x04000AAC RID: 2732
			public int freeAllocationIdsCount;
		}
	}
}
