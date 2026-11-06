using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200002E RID: 46
	[GenerateTestsForBurstCompatibility]
	internal struct BufferAllocatorHeap : IBufferAllocator, IDisposable
	{
		// Token: 0x060000CA RID: 202 RVA: 0x00003C8C File Offset: 0x00001E8C
		public BufferAllocatorHeap(int budgetInBytes, int bufferSizeInBytes, AllocatorManager.AllocatorHandle handle)
		{
			this.Handle = handle;
			this.BufferSizeInBytes = bufferSizeInBytes;
			int num = (budgetInBytes + bufferSizeInBytes - 1) / bufferSizeInBytes;
			this.Buffers = new UnsafeList<IntPtr>(num, AllocatorManager.LegacyOf(handle), NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < num; i++)
			{
				this.Buffers.Add(in IntPtr.Zero);
			}
			this.FreeList = new UnsafeBitArray(num, AllocatorManager.LegacyOf(handle), NativeArrayOptions.ClearMemory);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003CFC File Offset: 0x00001EFC
		public unsafe int Allocate()
		{
			int num = this.FreeList.Find(0, this.BufferCapacity, 1);
			if (num >= this.BufferCapacity)
			{
				return -1;
			}
			void* ptr = AllocatorManager.Allocate(this.Handle, 1, 64, this.BufferSizeInBytes);
			if (ptr == null)
			{
				return -1;
			}
			this.FreeList.Set(num, true);
			this.Buffers[num] = (IntPtr)ptr;
			return num;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003D64 File Offset: 0x00001F64
		public unsafe void Free(int index)
		{
			AllocatorManager.Free(this.Handle, (void*)this.Buffers[index]);
			this.FreeList.Set(index, false);
			this.Buffers[index] = IntPtr.Zero;
		}

		// Token: 0x1700001C RID: 28
		public unsafe void* this[int index]
		{
			get
			{
				return (void*)this.Buffers[index];
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00003DB3 File Offset: 0x00001FB3
		public int BufferCapacity
		{
			get
			{
				return this.Buffers.Length;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00003DC0 File Offset: 0x00001FC0
		public bool IsEmpty
		{
			get
			{
				return this.FreeList.CountBits(0, this.FreeList.Length) >= this.BufferCapacity;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003DE4 File Offset: 0x00001FE4
		public unsafe void Dispose()
		{
			for (int i = 0; i < this.Buffers.Length; i++)
			{
				AllocatorManager.Free(this.Handle, (void*)this.Buffers[i]);
			}
			this.Buffers.Dispose();
			this.FreeList.Dispose();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003E39 File Offset: 0x00002039
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void ThrowFreeListEmpty()
		{
			throw new InvalidOperationException("Cannot allocate, allocator is exhausted.");
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003E45 File Offset: 0x00002045
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void ThrowAllocationFailed()
		{
			throw new InvalidOperationException("Failed to allocate buffer.");
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003E51 File Offset: 0x00002051
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckInvalidIndexToFree(int index)
		{
			if (index < 0 || index >= this.BufferCapacity)
			{
				throw new ArgumentException(string.Format("Cannot free index {0}, it is outside the expected range [0, {1}).", index, this.BufferCapacity));
			}
		}

		// Token: 0x0400003F RID: 63
		private UnsafeList<IntPtr> Buffers;

		// Token: 0x04000040 RID: 64
		private UnsafeBitArray FreeList;

		// Token: 0x04000041 RID: 65
		private readonly AllocatorManager.AllocatorHandle Handle;

		// Token: 0x04000042 RID: 66
		private readonly int BufferSizeInBytes;

		// Token: 0x04000043 RID: 67
		private const int kBufferAlignment = 64;
	}
}
