using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x0200013F RID: 319
	internal struct ComponentTypeList
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x00021CDC File Offset: 0x0001FEDC
		internal ComponentTypeList(int sizeOf, int alignOf, int initialCapacity, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			this = default(ComponentTypeList);
			this.Allocator = allocator;
			this.Ptr = null;
			this.Length = 0;
			this.Capacity = 0;
			if (initialCapacity != 0)
			{
				this.SetCapacity<AllocatorManager.AllocatorHandle>(ref this.Allocator, sizeOf, alignOf, initialCapacity);
			}
			if (options == NativeArrayOptions.ClearMemory && this.Ptr != null)
			{
				UnsafeUtility.MemClear(this.Ptr, (long)(this.Capacity * sizeOf));
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x00021D43 File Offset: 0x0001FF43
		internal bool IsEmpty
		{
			get
			{
				return !this.IsCreated || this.Length == 0;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00021D58 File Offset: 0x0001FF58
		internal bool IsCreated
		{
			get
			{
				return this.Ptr != null;
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00021D68 File Offset: 0x0001FF68
		internal void Dispose()
		{
			if (CollectionHelper.ShouldDeallocate(this.Allocator))
			{
				AllocatorManager.Free(this.Allocator, this.Ptr);
				this.Allocator = AllocatorManager.Invalid;
			}
			this.Ptr = null;
			this.Length = 0;
			this.Capacity = 0;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00021DB4 File Offset: 0x0001FFB4
		internal void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00021DC0 File Offset: 0x0001FFC0
		internal unsafe void Resize(int sizeOf, int alignOf, int length, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			int length2 = this.Length;
			if (length > this.Capacity)
			{
				this.SetCapacity<AllocatorManager.AllocatorHandle>(ref this.Allocator, sizeOf, alignOf, length);
			}
			this.Length = length;
			if (options == NativeArrayOptions.ClearMemory && length2 < length)
			{
				int num = length - length2;
				byte* ptr = (byte*)this.Ptr;
				UnsafeUtility.MemClear((void*)(ptr + length2 * sizeOf), (long)(num * sizeOf));
			}
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00021E18 File Offset: 0x00020018
		private unsafe void Realloc<[global::System.Runtime.CompilerServices.IsUnmanaged] U>(ref U allocator, int sizeOf, int alignOf, int capacity) where U : struct, ValueType, AllocatorManager.IAllocator
		{
			void* ptr = null;
			if (capacity > 0)
			{
				ptr = (ref allocator).Allocate<U>(sizeOf, alignOf, capacity);
				if (this.Capacity > 0)
				{
					int num = math.min(capacity, this.Capacity) * sizeOf;
					UnsafeUtility.MemCpy(ptr, this.Ptr, (long)num);
				}
			}
			(ref allocator).Free<U>(this.Ptr, sizeOf, alignOf, this.Capacity);
			this.Ptr = ptr;
			this.Capacity = capacity;
			this.Length = math.min(this.Length, capacity);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00021E98 File Offset: 0x00020098
		private void SetCapacity<[global::System.Runtime.CompilerServices.IsUnmanaged] U>(ref U allocator, int sizeOf, int alignOf, int capacity) where U : struct, ValueType, AllocatorManager.IAllocator
		{
			int num = math.max(capacity, 64 / sizeOf);
			num = math.ceilpow2(num);
			if (num == this.Capacity)
			{
				return;
			}
			this.Realloc<U>(ref allocator, sizeOf, alignOf, num);
		}

		// Token: 0x04000393 RID: 915
		internal unsafe void* Ptr;

		// Token: 0x04000394 RID: 916
		internal int Length;

		// Token: 0x04000395 RID: 917
		internal int Capacity;

		// Token: 0x04000396 RID: 918
		internal AllocatorManager.AllocatorHandle Allocator;
	}
}
