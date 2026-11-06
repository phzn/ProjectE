using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Pathfinding.Collections
{
	// Token: 0x0200026E RID: 622
	public struct SlabAllocator<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x06000F9D RID: 3997 RVA: 0x0005F53D File Offset: 0x0005D73D
		internal static int SizeIndexToElements(int sizeIndex)
		{
			return 1 << sizeIndex;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0005F545 File Offset: 0x0005D745
		internal static int ElementsToSizeIndex(int nElements)
		{
			if (nElements < 0)
			{
				throw new Exception("SlabAllocator cannot allocate less than 1 element");
			}
			if (nElements == 0)
			{
				return 0;
			}
			int num = CollectionHelper.Log2Ceil(nElements);
			if (num > 12)
			{
				throw new Exception("SlabAllocator cannot allocate more than MaxAllocationSize elements.");
			}
			return num;
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x0001AA07 File Offset: 0x00018C07
		public bool IsDebugAllocator
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0005F571 File Offset: 0x0005D771
		public bool IsCreated
		{
			get
			{
				return this.data != null;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0005F580 File Offset: 0x0005D780
		public unsafe int ByteSize
		{
			get
			{
				return this.data->mem.Length;
			}
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0005F592 File Offset: 0x0005D792
		public unsafe SlabAllocator(int initialCapacityBytes, AllocatorManager.AllocatorHandle allocator)
		{
			this.data = AllocatorManager.Allocate<SlabAllocator<T>.AllocatorData>(allocator, 1);
			this.data->mem = new UnsafeList<byte>(initialCapacityBytes, allocator, NativeArrayOptions.UninitializedMemory);
			this.Clear();
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0005F5BC File Offset: 0x0005D7BC
		public unsafe void Clear()
		{
			this.CheckDisposed();
			this.data->mem.Clear();
			for (int i = 0; i < 13; i++)
			{
				*((ref this.data->freeHeads.FixedElementField) + (IntPtr)i * 4) = -1;
			}
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0005F604 File Offset: 0x0005D804
		public unsafe UnsafeSpan<T> GetSpan(int allocatedIndex)
		{
			this.CheckDisposed();
			if (allocatedIndex == -1)
			{
				return new UnsafeSpan<T>(null, 0, Allocator.None);
			}
			byte* ptr = this.data->mem.Ptr + allocatedIndex;
			SlabAllocator<T>.Header* ptr2 = (SlabAllocator<T>.Header*)(ptr - sizeof(SlabAllocator<T>.Header));
			uint num = ptr2->length & 1073741823U;
			return new UnsafeSpan<T>((void*)ptr, (int)num, Allocator.None);
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0005F654 File Offset: 0x0005D854
		public unsafe void Realloc(ref int allocatedIndex, int nElements)
		{
			this.CheckDisposed();
			if (allocatedIndex == -1)
			{
				allocatedIndex = this.Allocate(nElements);
				return;
			}
			SlabAllocator<T>.Header* ptr = (SlabAllocator<T>.Header*)(this.data->mem.Ptr + allocatedIndex - sizeof(SlabAllocator<T>.Header));
			uint num = ptr->length & 1073741823U;
			int num2 = SlabAllocator<T>.ElementsToSizeIndex((int)num);
			int num3 = SlabAllocator<T>.ElementsToSizeIndex(nElements);
			if (num2 == num3)
			{
				ptr->length = (uint)(nElements | 1073741824 | int.MinValue);
				return;
			}
			int num4 = this.Allocate(nElements);
			UnsafeSpan<T> span = this.GetSpan(allocatedIndex);
			UnsafeSpan<T> span2 = this.GetSpan(num4);
			span.Slice(0, math.min((int)num, nElements)).CopyTo(span2);
			this.Free(allocatedIndex);
			allocatedIndex = num4;
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0005F704 File Offset: 0x0005D904
		public unsafe int Allocate(List<T> values)
		{
			int num = this.Allocate(values.Count);
			UnsafeSpan<T> span = this.GetSpan(num);
			for (int i = 0; i < span.Length; i++)
			{
				*span[i] = values[i];
			}
			return num;
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0005F750 File Offset: 0x0005D950
		public int Allocate(NativeList<T> values)
		{
			int num = this.Allocate(values.Length);
			this.GetSpan(num).CopyFrom<T>(values.AsArray());
			return num;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0005F780 File Offset: 0x0005D980
		public unsafe int Allocate(int nElements)
		{
			this.CheckDisposed();
			if (nElements == 0)
			{
				return -1;
			}
			int num = SlabAllocator<T>.ElementsToSizeIndex(nElements);
			int num2 = *((ref this.data->freeHeads.FixedElementField) + (IntPtr)num * 4);
			if (num2 != -1)
			{
				byte* ptr = this.data->mem.Ptr;
				*((ref this.data->freeHeads.FixedElementField) + (IntPtr)num * 4) = ((SlabAllocator<T>.NextBlock*)(ptr + num2))->next;
				*(SlabAllocator<T>.Header*)(ptr + num2 - sizeof(SlabAllocator<T>.Header)) = new SlabAllocator<T>.Header
				{
					length = (uint)(nElements | int.MinValue | 1073741824)
				};
				return num2;
			}
			int length = this.data->mem.Length;
			int num3 = length + sizeof(SlabAllocator<T>.Header) + SlabAllocator<T>.SizeIndexToElements(num) * sizeof(T);
			if (Hint.Unlikely(num3 > this.data->mem.Capacity))
			{
				this.data->mem.SetCapacity(math.max(this.data->mem.Capacity * 2, num3));
			}
			this.data->mem.m_length = num3;
			*(SlabAllocator<T>.Header*)(this.data->mem.Ptr + length) = new SlabAllocator<T>.Header
			{
				length = (uint)(nElements | int.MinValue | 1073741824)
			};
			return length + sizeof(SlabAllocator<T>.Header);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0005F8D4 File Offset: 0x0005DAD4
		public unsafe void Free(int allocatedIndex)
		{
			this.CheckDisposed();
			if (allocatedIndex == -1)
			{
				return;
			}
			byte* ptr = this.data->mem.Ptr;
			SlabAllocator<T>.Header* ptr2 = (SlabAllocator<T>.Header*)(ptr + allocatedIndex - sizeof(SlabAllocator<T>.Header));
			int num = SlabAllocator<T>.ElementsToSizeIndex((int)(ptr2->length & 1073741823U));
			*(SlabAllocator<T>.NextBlock*)(ptr + allocatedIndex) = new SlabAllocator<T>.NextBlock
			{
				next = *((ref this.data->freeHeads.FixedElementField) + (IntPtr)num * 4)
			};
			*((ref this.data->freeHeads.FixedElementField) + (IntPtr)num * 4) = allocatedIndex;
			ptr2->length &= 1073741823U;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0005F96C File Offset: 0x0005DB6C
		public unsafe void CopyTo(SlabAllocator<T> other)
		{
			this.CheckDisposed();
			other.CheckDisposed();
			other.data->mem.CopyFrom(in this.data->mem);
			for (int i = 0; i < 13; i++)
			{
				*((ref other.data->freeHeads.FixedElementField) + (IntPtr)i * 4) = *((ref this.data->freeHeads.FixedElementField) + (IntPtr)i * 4);
			}
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00003788 File Offset: 0x00001988
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckDisposed()
		{
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0005F9DC File Offset: 0x0005DBDC
		public unsafe void Dispose()
		{
			if (this.data == null)
			{
				return;
			}
			AllocatorManager.AllocatorHandle allocator = this.data->mem.Allocator;
			this.data->mem.Dispose();
			AllocatorManager.Free<SlabAllocator<T>.AllocatorData>(allocator, this.data, 1);
			this.data = null;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0005FA28 File Offset: 0x0005DC28
		public SlabAllocator<T>.List GetList(int allocatedIndex)
		{
			return new SlabAllocator<T>.List(this, allocatedIndex);
		}

		// Token: 0x04000B8A RID: 2954
		public const int InvalidAllocation = -2;

		// Token: 0x04000B8B RID: 2955
		public const int ZeroLengthArray = -1;

		// Token: 0x04000B8C RID: 2956
		public const int MaxAllocationSizeIndex = 12;

		// Token: 0x04000B8D RID: 2957
		public const int MaxAllocationSize = 4096;

		// Token: 0x04000B8E RID: 2958
		private const uint UsedBit = 2147483648U;

		// Token: 0x04000B8F RID: 2959
		private const uint AllocatedBit = 1073741824U;

		// Token: 0x04000B90 RID: 2960
		private const uint LengthMask = 1073741823U;

		// Token: 0x04000B91 RID: 2961
		[NativeDisableUnsafePtrRestriction]
		private unsafe SlabAllocator<T>.AllocatorData* data;

		// Token: 0x0200026F RID: 623
		private struct AllocatorData
		{
			// Token: 0x04000B92 RID: 2962
			public UnsafeList<byte> mem;

			// Token: 0x04000B93 RID: 2963
			[FixedBuffer(typeof(int), 13)]
			public SlabAllocator<T>.AllocatorData.<freeHeads>e__FixedBuffer freeHeads;

			// Token: 0x02000270 RID: 624
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 52)]
			public struct <freeHeads>e__FixedBuffer
			{
				// Token: 0x04000B94 RID: 2964
				public int FixedElementField;
			}
		}

		// Token: 0x02000271 RID: 625
		private struct Header
		{
			// Token: 0x04000B95 RID: 2965
			public uint length;
		}

		// Token: 0x02000272 RID: 626
		private struct NextBlock
		{
			// Token: 0x04000B96 RID: 2966
			public int next;
		}

		// Token: 0x02000273 RID: 627
		public ref struct List
		{
			// Token: 0x06000FAE RID: 4014 RVA: 0x0005FA36 File Offset: 0x0005DC36
			public List(SlabAllocator<T> allocator, int allocationIndex)
			{
				this.span = allocator.GetSpan(allocationIndex);
				this.allocator = allocator;
				this.allocationIndex = allocationIndex;
			}

			// Token: 0x06000FAF RID: 4015 RVA: 0x0005FA54 File Offset: 0x0005DC54
			public unsafe void Add(T value)
			{
				this.allocator.Realloc(ref this.allocationIndex, this.span.Length + 1);
				this.span = this.allocator.GetSpan(this.allocationIndex);
				*this.span[this.span.Length - 1] = value;
			}

			// Token: 0x06000FB0 RID: 4016 RVA: 0x0005FAB4 File Offset: 0x0005DCB4
			public void RemoveAt(int index)
			{
				this.span.Move(index + 1, index, this.span.Length - index - 1);
				this.allocator.Realloc(ref this.allocationIndex, this.span.Length - 1);
				this.span = this.allocator.GetSpan(this.allocationIndex);
			}

			// Token: 0x06000FB1 RID: 4017 RVA: 0x0005FB14 File Offset: 0x0005DD14
			public void Clear()
			{
				this.allocator.Realloc(ref this.allocationIndex, 0);
				this.span = this.allocator.GetSpan(this.allocationIndex);
			}

			// Token: 0x1700024E RID: 590
			// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0005FB3F File Offset: 0x0005DD3F
			public int Length
			{
				get
				{
					return this.span.Length;
				}
			}

			// Token: 0x1700024F RID: 591
			public ref T this[int index]
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.span[index];
				}
			}

			// Token: 0x04000B97 RID: 2967
			public UnsafeSpan<T> span;

			// Token: 0x04000B98 RID: 2968
			private SlabAllocator<T> allocator;

			// Token: 0x04000B99 RID: 2969
			public int allocationIndex;
		}
	}
}
