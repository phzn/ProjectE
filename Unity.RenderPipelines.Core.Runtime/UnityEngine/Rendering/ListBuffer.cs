using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.Rendering
{
	// Token: 0x0200004A RID: 74
	public struct ListBuffer<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000C15E File Offset: 0x0000A35E
		internal unsafe T* BufferPtr
		{
			get
			{
				return this.m_BufferPtr;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000C166 File Offset: 0x0000A366
		public unsafe int Count
		{
			get
			{
				return *this.m_CountPtr;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000C16F File Offset: 0x0000A36F
		public int Capacity
		{
			get
			{
				return this.m_Capacity;
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000C177 File Offset: 0x0000A377
		public unsafe ListBuffer(T* bufferPtr, int* countPtr, int capacity)
		{
			this.m_BufferPtr = bufferPtr;
			this.m_Capacity = capacity;
			this.m_CountPtr = countPtr;
		}

		// Token: 0x1700004C RID: 76
		public unsafe ref T this[in int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException(string.Format("Expected a value between 0 and {0}, but received {1}.", this.Count, index));
				}
				return ref this.m_BufferPtr[(IntPtr)index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000C1DF File Offset: 0x0000A3DF
		public unsafe ref T GetUnchecked(in int index)
		{
			return ref this.m_BufferPtr[(IntPtr)index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000C1F2 File Offset: 0x0000A3F2
		public unsafe bool TryAdd(in T value)
		{
			if (this.Count >= this.m_Capacity)
			{
				return false;
			}
			this.m_BufferPtr[(IntPtr)this.Count * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
			(*this.m_CountPtr)++;
			return true;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000C230 File Offset: 0x0000A430
		public unsafe void CopyTo(T* dstBuffer, int startDstIndex, int copyCount)
		{
			UnsafeUtility.MemCpy((void*)(dstBuffer + (IntPtr)startDstIndex * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)this.m_BufferPtr, (long)(UnsafeUtility.SizeOf<T>() * copyCount));
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000C250 File Offset: 0x0000A450
		public unsafe bool TryCopyTo(ListBuffer<T> other)
		{
			if (other.Count + this.Count >= other.m_Capacity)
			{
				return false;
			}
			UnsafeUtility.MemCpy((void*)(other.m_BufferPtr + (IntPtr)other.Count * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)this.m_BufferPtr, (long)(UnsafeUtility.SizeOf<T>() * this.Count));
			*other.m_CountPtr += this.Count;
			return true;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
		public unsafe bool TryCopyFrom(T* srcPtr, int count)
		{
			if (count + this.Count > this.m_Capacity)
			{
				return false;
			}
			UnsafeUtility.MemCpy((void*)(this.m_BufferPtr + (IntPtr)this.Count * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)srcPtr, (long)(UnsafeUtility.SizeOf<T>() * count));
			*this.m_CountPtr += count;
			return true;
		}

		// Token: 0x0400018D RID: 397
		private unsafe T* m_BufferPtr;

		// Token: 0x0400018E RID: 398
		private int m_Capacity;

		// Token: 0x0400018F RID: 399
		private unsafe int* m_CountPtr;
	}
}
