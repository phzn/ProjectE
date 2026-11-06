using System;
using System.Diagnostics;
using Unity.Burst.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.LowLevel.Unsafe
{
	// Token: 0x020004CB RID: 1227
	public struct UnsafeUntypedBufferAccessor
	{
		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06001D77 RID: 7543 RVA: 0x00066977 File Offset: 0x00064B77
		public int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06001D78 RID: 7544 RVA: 0x0006697F File Offset: 0x00064B7F
		public int ElementSize
		{
			get
			{
				return this.m_ElementSize;
			}
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x00066987 File Offset: 0x00064B87
		internal unsafe UnsafeUntypedBufferAccessor(byte* basePointer, int length, int stride, int elementSize, int elementAlign, int internalCapacity)
		{
			this.m_Pointer = basePointer;
			this.m_InternalCapacity = internalCapacity;
			this.m_ElementSize = elementSize;
			this.m_ElementAlign = elementAlign;
			this.m_Stride = stride;
			this.m_Length = length;
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckReadAccess()
		{
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckWriteAccess()
		{
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x000669B6 File Offset: 0x00064BB6
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void AssertIndexInRange(int index)
		{
			if (Hint.Unlikely(index < 0 || index >= this.m_Length))
			{
				throw new InvalidOperationException(string.Format("index {0} out of range in LowLevelBufferAccessor of length {1}", index, this.Length));
			}
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x000669F4 File Offset: 0x00064BF4
		public unsafe void* GetUnsafePtrAndLength(int index, out int length)
		{
			BufferHeader* ptr = (BufferHeader*)(this.m_Pointer + index * this.m_Stride);
			length = ptr->Length;
			return (void*)BufferHeader.GetElementPointer(ptr);
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x00066A20 File Offset: 0x00064C20
		public unsafe void* GetUnsafeReadOnlyPtrAndLength(int index, out int length)
		{
			BufferHeader* ptr = (BufferHeader*)(this.m_Pointer + index * this.m_Stride);
			length = ptr->Length;
			return (void*)BufferHeader.GetElementPointer(ptr);
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x00066A4C File Offset: 0x00064C4C
		public unsafe void* GetUnsafePtr(int index)
		{
			BufferHeader* ptr = (BufferHeader*)(this.m_Pointer + index * this.m_Stride);
			return (void*)BufferHeader.GetElementPointer(ptr);
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x00066A70 File Offset: 0x00064C70
		public unsafe void* GetUnsafeReadOnlyPtr(int index)
		{
			BufferHeader* ptr = (BufferHeader*)(this.m_Pointer + index * this.m_Stride);
			return (void*)BufferHeader.GetElementPointer(ptr);
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x00066A94 File Offset: 0x00064C94
		public unsafe int GetBufferLength(int index)
		{
			BufferHeader* ptr = (BufferHeader*)(this.m_Pointer + index * this.m_Stride);
			return ptr->Length;
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x00066AB8 File Offset: 0x00064CB8
		public unsafe int GetBufferCapacity(int index)
		{
			BufferHeader* ptr = (BufferHeader*)(this.m_Pointer + index * this.m_Stride);
			return ptr->Capacity;
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x00066ADC File Offset: 0x00064CDC
		public unsafe void ResizeUninitialized(int index, int length)
		{
			BufferHeader* ptr = (BufferHeader*)(this.m_Pointer + index * this.m_Stride);
			BufferHeader.EnsureCapacity(ptr, length, this.m_ElementSize, this.m_ElementAlign, BufferHeader.TrashMode.RetainOldData, false, 0);
			ptr->Length = length;
		}

		// Token: 0x04000A93 RID: 2707
		[NativeDisableUnsafePtrRestriction]
		private unsafe byte* m_Pointer;

		// Token: 0x04000A94 RID: 2708
		private int m_InternalCapacity;

		// Token: 0x04000A95 RID: 2709
		private int m_Stride;

		// Token: 0x04000A96 RID: 2710
		private int m_Length;

		// Token: 0x04000A97 RID: 2711
		private int m_ElementSize;

		// Token: 0x04000A98 RID: 2712
		private int m_ElementAlign;
	}
}
