using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.LowLevel.Unsafe
{
	// Token: 0x020004CC RID: 1228
	[DebuggerDisplay("Count = {Count}, Capacity = {Capacity}, IsCreated = {IsCreated}, IsEmpty = {IsEmpty}, IsFull = {IsFull}")]
	[DebuggerTypeProxy(typeof(UnsafeCircularBufferTDebugView<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	internal struct UnsafeCircularBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IDisposable, IEnumerable<T>, IEnumerable where T : struct, ValueType
	{
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06001D84 RID: 7556 RVA: 0x00066B16 File Offset: 0x00064D16
		public unsafe T* Ptr
		{
			get
			{
				return this.m_Ptr;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06001D85 RID: 7557 RVA: 0x00066B1E File Offset: 0x00064D1E
		public bool IsCreated
		{
			get
			{
				return this.m_Ptr != null;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06001D86 RID: 7558 RVA: 0x00066B2D File Offset: 0x00064D2D
		public int Capacity
		{
			get
			{
				return this.m_Capacity;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06001D87 RID: 7559 RVA: 0x00066B35 File Offset: 0x00064D35
		public int Count
		{
			get
			{
				return this.m_Count;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x00066B3D File Offset: 0x00064D3D
		public bool IsEmpty
		{
			get
			{
				return this.m_Count == 0;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06001D89 RID: 7561 RVA: 0x00066B48 File Offset: 0x00064D48
		public bool IsFull
		{
			get
			{
				return this.m_Count == this.m_Capacity;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x00066B58 File Offset: 0x00064D58
		public int FrontIndex
		{
			get
			{
				return this.m_Front;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001D8B RID: 7563 RVA: 0x00066B60 File Offset: 0x00064D60
		public int BackIndex
		{
			get
			{
				return this.m_Back;
			}
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x00066B68 File Offset: 0x00064D68
		public UnsafeCircularBuffer(int capacity, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			this.m_Allocator = default(AllocatorManager.AllocatorHandle);
			this.m_Ptr = null;
			this.m_Front = 0;
			this.m_Back = 0;
			this.m_Capacity = 0;
			this.m_Count = 0;
			this.Construct(capacity, allocator, options);
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x00066BA4 File Offset: 0x00064DA4
		public unsafe UnsafeCircularBuffer(in NativeArray<T> array, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_Allocator = allocator;
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = UnsafeUtility.AlignOf<T>();
			int num3 = num;
			int num4 = num2;
			NativeArray<T> nativeArray = array;
			this.m_Ptr = (T*)(ref this.m_Allocator).Allocate<AllocatorManager.AllocatorHandle>(num3, num4, nativeArray.Length);
			this.m_Front = 0;
			this.m_Back = 0;
			nativeArray = array;
			this.m_Capacity = nativeArray.Length;
			nativeArray = array;
			this.m_Count = nativeArray.Length;
			void* ptr = (void*)this.m_Ptr;
			void* unsafeReadOnlyPtr = array.GetUnsafeReadOnlyPtr<T>();
			nativeArray = array;
			UnsafeUtility.MemCpy(ptr, unsafeReadOnlyPtr, (long)(nativeArray.Length * num));
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x00066C44 File Offset: 0x00064E44
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public unsafe UnsafeCircularBuffer(T[] array, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_Allocator = allocator;
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = UnsafeUtility.AlignOf<T>();
			this.m_Ptr = (T*)(ref this.m_Allocator).Allocate<AllocatorManager.AllocatorHandle>(num, num2, array.Length);
			this.m_Front = 0;
			this.m_Back = 0;
			this.m_Capacity = array.Length;
			this.m_Count = array.Length;
			fixed (T[] array2 = array)
			{
				T* ptr;
				if (array == null || array2.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array2[0];
				}
				UnsafeUtility.MemCpy((void*)this.m_Ptr, (void*)ptr, (long)(array.Length * num));
			}
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x00066CC8 File Offset: 0x00064EC8
		public unsafe void Construct(int capacity, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			this.m_Allocator = allocator;
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = UnsafeUtility.AlignOf<T>();
			this.m_Ptr = (T*)(ref this.m_Allocator).Allocate<AllocatorManager.AllocatorHandle>(num, num2, capacity);
			this.m_Front = 0;
			this.m_Back = 0;
			this.m_Capacity = capacity;
			this.m_Count = 0;
			if (options == NativeArrayOptions.ClearMemory && this.m_Ptr != null)
			{
				UnsafeUtility.MemClear((void*)this.m_Ptr, (long)(this.m_Capacity * num));
			}
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x00066D3A File Offset: 0x00064F3A
		public void Dispose()
		{
			(ref this.m_Allocator).Free<AllocatorManager.AllocatorHandle, T>(this.m_Ptr, this.m_Capacity);
			this.m_Ptr = null;
			this.m_Front = 0;
			this.m_Back = 0;
			this.m_Capacity = 0;
			this.m_Count = 0;
		}

		// Token: 0x17000271 RID: 625
		public unsafe T this[int index]
		{
			get
			{
				return this.m_Ptr[(IntPtr)this.GetIndex(index) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
			set
			{
				this.m_Ptr[(IntPtr)this.GetIndex(index) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
			}
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x00066DB2 File Offset: 0x00064FB2
		public unsafe ref T ElementAt(int index)
		{
			return ref this.m_Ptr[(IntPtr)this.GetIndex(index) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x00066DCA File Offset: 0x00064FCA
		public unsafe T Front()
		{
			return this.m_Ptr[(IntPtr)this.m_Front * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x00066DE6 File Offset: 0x00064FE6
		public unsafe T Back()
		{
			return this.m_Ptr[(IntPtr)(((this.m_Back == 0) ? this.m_Capacity : this.m_Back) - 1) * (IntPtr)sizeof(T)];
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x00066E14 File Offset: 0x00065014
		public unsafe bool PushFront(in T element)
		{
			if (this.IsFull)
			{
				return false;
			}
			this.m_Front = this.Modulo(this.m_Front - 1, this.m_Capacity);
			this.m_Ptr[(IntPtr)this.m_Front * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = element;
			this.m_Count++;
			return true;
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x00066E74 File Offset: 0x00065074
		public unsafe bool PushFront(T* elements, int count)
		{
			if (count <= 0)
			{
				return true;
			}
			if (this.m_Count + count > this.m_Capacity)
			{
				return false;
			}
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = ((this.m_Front <= this.m_Back) ? this.m_Front : (this.m_Front - this.m_Back));
			if (count <= num2)
			{
				UnsafeUtility.MemCpy((void*)(this.m_Ptr + (IntPtr)(this.m_Front - count) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)elements, (long)(count * num));
			}
			else
			{
				int num3 = count - num2;
				UnsafeUtility.MemCpy((void*)(this.m_Ptr + (IntPtr)(this.m_Capacity - num3) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)elements, (long)(num3 * num));
				UnsafeUtility.MemCpy((void*)this.m_Ptr, (void*)(elements + (IntPtr)num3 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (long)(num2 * num));
			}
			this.m_Front = this.Modulo(this.m_Front - count, this.m_Capacity);
			this.m_Count += count;
			return true;
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x00066F54 File Offset: 0x00065154
		public unsafe bool PushFront(in NativeArray<T> array)
		{
			T* unsafeReadOnlyPtr = (T*)array.GetUnsafeReadOnlyPtr<T>();
			NativeArray<T> nativeArray = array;
			return this.PushFront(unsafeReadOnlyPtr, nativeArray.Length);
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x00066F80 File Offset: 0x00065180
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public unsafe bool PushFront(T[] array)
		{
			T* ptr;
			if (array == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			return this.PushFront(ptr, array.Length);
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x00066FB0 File Offset: 0x000651B0
		public unsafe bool PushBack(in T element)
		{
			if (this.IsFull)
			{
				return false;
			}
			this.m_Ptr[(IntPtr)this.m_Back * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = element;
			this.m_Back = this.Modulo(this.m_Back + 1, this.m_Capacity);
			this.m_Count++;
			return true;
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x00067010 File Offset: 0x00065210
		public unsafe bool PushBack(T* elements, int count)
		{
			if (count <= 0)
			{
				return true;
			}
			if (this.m_Count + count > this.m_Capacity)
			{
				return false;
			}
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = ((this.m_Front <= this.m_Back) ? (this.m_Capacity - this.m_Back) : (this.m_Front - this.m_Back));
			if (count <= num2)
			{
				UnsafeUtility.MemCpy((void*)(this.m_Ptr + (IntPtr)this.m_Back * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)elements, (long)(count * num));
			}
			else
			{
				UnsafeUtility.MemCpy((void*)(this.m_Ptr + (IntPtr)this.m_Back * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)elements, (long)(num2 * num));
				UnsafeUtility.MemCpy((void*)this.m_Ptr, (void*)(elements + (IntPtr)num2 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (long)((count - num2) * num));
			}
			this.m_Back = this.Modulo(this.m_Back + count, this.m_Capacity);
			this.m_Count += count;
			return true;
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x000670F0 File Offset: 0x000652F0
		public unsafe bool PushBack(in NativeArray<T> array)
		{
			T* unsafeReadOnlyPtr = (T*)array.GetUnsafeReadOnlyPtr<T>();
			NativeArray<T> nativeArray = array;
			return this.PushBack(unsafeReadOnlyPtr, nativeArray.Length);
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x0006711C File Offset: 0x0006531C
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public unsafe bool PushBack(T[] array)
		{
			T* ptr;
			if (array == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			return this.PushBack(ptr, array.Length);
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x0006714B File Offset: 0x0006534B
		public bool PopFront()
		{
			return this.PopFront(1);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x00067154 File Offset: 0x00065354
		public bool PopFront(int count)
		{
			if (this.IsEmpty || this.m_Count < count)
			{
				return false;
			}
			this.m_Front = this.Modulo(this.m_Front + count, this.m_Capacity);
			this.m_Count -= count;
			return true;
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x00067192 File Offset: 0x00065392
		public bool PopBack()
		{
			return this.PopBack(1);
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x0006719B File Offset: 0x0006539B
		public bool PopBack(int count)
		{
			if (this.IsEmpty || this.m_Count < count)
			{
				return false;
			}
			this.m_Back = this.Modulo(this.m_Back - count, this.m_Capacity);
			this.m_Count -= count;
			return true;
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000671DC File Offset: 0x000653DC
		public unsafe void Unwind()
		{
			if (this.m_Front == 0)
			{
				return;
			}
			if (this.m_Count > 0)
			{
				int num = UnsafeUtility.SizeOf<T>();
				if (this.m_Front < this.m_Back)
				{
					UnsafeUtility.MemMove((void*)this.m_Ptr, (void*)(this.m_Ptr + (IntPtr)this.m_Front * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (long)(this.m_Count * num));
				}
				else
				{
					int num2 = this.m_Capacity - this.m_Front;
					int back = this.m_Back;
					int num3 = UnsafeUtility.AlignOf<T>();
					if (num2 < back)
					{
						void* ptr = (ref this.m_Allocator).Allocate<AllocatorManager.AllocatorHandle>(num, num3, num2);
						UnsafeUtility.MemCpy(ptr, (void*)(this.m_Ptr + (IntPtr)this.m_Front * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (long)(num2 * num));
						UnsafeUtility.MemMove((void*)(this.m_Ptr + (IntPtr)num2 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)this.m_Ptr, (long)(back * num));
						UnsafeUtility.MemCpy((void*)this.m_Ptr, ptr, (long)(num2 * num));
						(ref this.m_Allocator).Free<AllocatorManager.AllocatorHandle>(ptr, num, num3, num2);
					}
					else
					{
						void* ptr2 = (ref this.m_Allocator).Allocate<AllocatorManager.AllocatorHandle>(num, num3, back);
						UnsafeUtility.MemCpy(ptr2, (void*)this.m_Ptr, (long)(back * num));
						UnsafeUtility.MemMove((void*)this.m_Ptr, (void*)(this.m_Ptr + (IntPtr)this.m_Front * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (long)(num2 * num));
						UnsafeUtility.MemCpy((void*)(this.m_Ptr + (IntPtr)num2 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), ptr2, (long)(back * num));
						(ref this.m_Allocator).Free<AllocatorManager.AllocatorHandle>(ptr2, num, num3, back);
					}
				}
			}
			this.m_Front = 0;
			this.m_Back = this.m_Count;
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x00067354 File Offset: 0x00065554
		public void Clear()
		{
			this.m_Count = 0;
			this.m_Back = 0;
			this.m_Front = 0;
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0006736C File Offset: 0x0006556C
		public unsafe NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<T> nativeArray = new NativeArray<T>(this.m_Count, allocator.ToAllocator, NativeArrayOptions.UninitializedMemory);
			if (this.m_Count > 0)
			{
				this.CopyTo((T*)nativeArray.GetUnsafePtr<T>());
			}
			return nativeArray;
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x000673A4 File Offset: 0x000655A4
		[ExcludeFromBurstCompatTesting("Returns managed array")]
		public unsafe T[] ToArray()
		{
			T[] array = new T[this.m_Count];
			if (this.m_Count > 0)
			{
				T[] array2;
				T* ptr;
				if ((array2 = array) == null || array2.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array2[0];
				}
				this.CopyTo(ptr);
				array2 = null;
			}
			return array;
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x000673E8 File Offset: 0x000655E8
		private unsafe void CopyTo(T* dest)
		{
			int num = UnsafeUtility.SizeOf<T>();
			if (this.m_Front < this.m_Back)
			{
				UnsafeUtility.MemCpy((void*)dest, (void*)(this.m_Ptr + (IntPtr)this.m_Front * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (long)(this.m_Count * num));
				return;
			}
			int num2 = this.m_Capacity - this.m_Front;
			UnsafeUtility.MemCpy((void*)dest, (void*)(this.m_Ptr + (IntPtr)this.m_Front * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (long)(num2 * num));
			UnsafeUtility.MemCpy((void*)(dest + (IntPtr)num2 * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (void*)this.m_Ptr, (long)(this.m_Back * num));
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x0006747A File Offset: 0x0006567A
		public UnsafeCircularBuffer<T>.Enumerator GetEnumerator()
		{
			return new UnsafeCircularBuffer<T>.Enumerator(this);
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x00067487 File Offset: 0x00065687
		[ExcludeFromBurstCompatTesting("Returns managed object")]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x00067487 File Offset: 0x00065687
		[ExcludeFromBurstCompatTesting("Returns managed object")]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x00067494 File Offset: 0x00065694
		private int GetIndex(int index)
		{
			return this.m_Front + ((index < this.m_Capacity - this.m_Front) ? index : (index - this.m_Capacity));
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x000674B8 File Offset: 0x000656B8
		private int Modulo(int x, int y)
		{
			int num = x % y;
			if (num >= 0)
			{
				return num;
			}
			return num + y;
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x000674D2 File Offset: 0x000656D2
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ThrowIfAllocated()
		{
			if (this.m_Ptr != null)
			{
				throw new InvalidOperationException("Buffer is already allocated.");
			}
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x000674E9 File Offset: 0x000656E9
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ThrowIfEmpty()
		{
			if (this.IsEmpty)
			{
				throw new InvalidOperationException("Buffer is empty.");
			}
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x00067500 File Offset: 0x00065700
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ThrowIfIndexOutOfRange(int index)
		{
			if (this.IsEmpty)
			{
				throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer is empty.", index));
			}
			if (index < 0)
			{
				throw new IndexOutOfRangeException(string.Format("Cannot access index {0}.", index));
			}
			if (index >= this.m_Count)
			{
				throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer count is {1}.", index, this.m_Count));
			}
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x0006756F File Offset: 0x0006576F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ThrowIfCapacityExceeded(int count)
		{
			if (count > this.m_Capacity)
			{
				throw new InvalidOperationException(string.Format("Count {0} exceeds capacity. Buffer capacity is {1}.", count, this.m_Capacity));
			}
		}

		// Token: 0x04000A99 RID: 2713
		private AllocatorManager.AllocatorHandle m_Allocator;

		// Token: 0x04000A9A RID: 2714
		[NativeDisableUnsafePtrRestriction]
		private unsafe T* m_Ptr;

		// Token: 0x04000A9B RID: 2715
		private int m_Front;

		// Token: 0x04000A9C RID: 2716
		private int m_Back;

		// Token: 0x04000A9D RID: 2717
		private int m_Capacity;

		// Token: 0x04000A9E RID: 2718
		private int m_Count;

		// Token: 0x020004CD RID: 1229
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x17000272 RID: 626
			// (get) Token: 0x06001DB0 RID: 7600 RVA: 0x0006759C File Offset: 0x0006579C
			public T Current
			{
				get
				{
					return this.m_Buffer[this.m_Index];
				}
			}

			// Token: 0x17000273 RID: 627
			// (get) Token: 0x06001DB1 RID: 7601 RVA: 0x000675BD File Offset: 0x000657BD
			[ExcludeFromBurstCompatTesting("Returns managed object")]
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06001DB2 RID: 7602 RVA: 0x000675CA File Offset: 0x000657CA
			internal Enumerator(UnsafeCircularBuffer<T> buffer)
			{
				this.m_Buffer = buffer;
				this.m_Index = -1;
			}

			// Token: 0x06001DB3 RID: 7603 RVA: 0x00002287 File Offset: 0x00000487
			public void Dispose()
			{
			}

			// Token: 0x06001DB4 RID: 7604 RVA: 0x000675DC File Offset: 0x000657DC
			public bool MoveNext()
			{
				int num = this.m_Index + 1;
				this.m_Index = num;
				return num < this.m_Buffer.m_Count;
			}

			// Token: 0x06001DB5 RID: 7605 RVA: 0x00067607 File Offset: 0x00065807
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x04000A9F RID: 2719
			private readonly UnsafeCircularBuffer<T> m_Buffer;

			// Token: 0x04000AA0 RID: 2720
			private int m_Index;
		}
	}
}
