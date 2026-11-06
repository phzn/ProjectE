using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Jobs;
using Unity.Mathematics;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000115 RID: 277
	[DebuggerDisplay("Length = {Length}, Capacity = {Capacity}, IsCreated = {IsCreated}, IsEmpty = {IsEmpty}")]
	[DebuggerTypeProxy(typeof(UnsafeListTDebugView<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct UnsafeList<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable, INativeList<T>, IIndexable<T>, IEnumerable<T>, IEnumerable where T : struct, ValueType
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0002363F File Offset: 0x0002183F
		// (set) Token: 0x06000BBB RID: 3003 RVA: 0x0002364C File Offset: 0x0002184C
		public int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return CollectionHelper.AssumePositive(this.m_length);
			}
			set
			{
				if (value > this.Capacity)
				{
					this.Resize(value, NativeArrayOptions.UninitializedMemory);
					return;
				}
				this.m_length = value;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00023667 File Offset: 0x00021867
		// (set) Token: 0x06000BBD RID: 3005 RVA: 0x00023674 File Offset: 0x00021874
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return CollectionHelper.AssumePositive(this.m_capacity);
			}
			set
			{
				this.SetCapacity(value);
			}
		}

		// Token: 0x1700015D RID: 349
		public unsafe T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Ptr[(IntPtr)CollectionHelper.AssumePositive(index) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.Ptr[(IntPtr)CollectionHelper.AssumePositive(index) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
			}
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x000236B6 File Offset: 0x000218B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref T ElementAt(int index)
		{
			return ref this.Ptr[(IntPtr)CollectionHelper.AssumePositive(index) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x000236CD File Offset: 0x000218CD
		public unsafe UnsafeList(T* ptr, int length)
		{
			this = default(UnsafeList<T>);
			this.Ptr = ptr;
			this.m_length = length;
			this.m_capacity = length;
			this.Allocator = AllocatorManager.None;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x000236F8 File Offset: 0x000218F8
		public unsafe UnsafeList(int initialCapacity, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			this.Ptr = null;
			this.m_length = 0;
			this.m_capacity = 0;
			this.Allocator = allocator;
			this.padding = 0;
			this.SetCapacity(math.max(initialCapacity, 1));
			if (options == NativeArrayOptions.ClearMemory && this.Ptr != null)
			{
				int num = sizeof(T);
				UnsafeUtility.MemClear((void*)this.Ptr, (long)(this.Capacity * num));
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00023760 File Offset: 0x00021960
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(AllocatorManager.AllocatorHandle) })]
		internal unsafe static UnsafeList<T>* Create<[global::System.Runtime.CompilerServices.IsUnmanaged] U>(int initialCapacity, ref U allocator, NativeArrayOptions options) where U : struct, ValueType, AllocatorManager.IAllocator
		{
			UnsafeList<T>* ptr = (ref allocator).Allocate(default(UnsafeList<T>), 1);
			*ptr = new UnsafeList<T>(initialCapacity, allocator.Handle, options);
			return ptr;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00023796 File Offset: 0x00021996
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(AllocatorManager.AllocatorHandle) })]
		internal unsafe static void Destroy<[global::System.Runtime.CompilerServices.IsUnmanaged] U>(UnsafeList<T>* listData, ref U allocator) where U : struct, ValueType, AllocatorManager.IAllocator
		{
			listData->Dispose<U>(ref allocator);
			(ref allocator).Free<U>((void*)listData, sizeof(UnsafeList<T>), UnsafeUtility.AlignOf<UnsafeList<T>>(), 1);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000237B2 File Offset: 0x000219B2
		public unsafe static UnsafeList<T>* Create(int initialCapacity, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			UnsafeList<T>* ptr = AllocatorManager.Allocate<UnsafeList<T>>(allocator, 1);
			*ptr = new UnsafeList<T>(initialCapacity, allocator, options);
			return ptr;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x000237C9 File Offset: 0x000219C9
		public unsafe static void Destroy(UnsafeList<T>* listData)
		{
			AllocatorManager.AllocatorHandle allocator = listData->Allocator;
			listData->Dispose();
			AllocatorManager.Free<UnsafeList<T>>(allocator, listData, 1);
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x000237DE File Offset: 0x000219DE
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return !this.IsCreated || this.m_length == 0;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x000237F3 File Offset: 0x000219F3
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Ptr != null;
			}
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00023802 File Offset: 0x00021A02
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(AllocatorManager.AllocatorHandle) })]
		internal void Dispose<[global::System.Runtime.CompilerServices.IsUnmanaged] U>(ref U allocator) where U : struct, ValueType, AllocatorManager.IAllocator
		{
			(ref allocator).Free<U, T>(this.Ptr, this.m_capacity);
			this.Ptr = null;
			this.m_length = 0;
			this.m_capacity = 0;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0002382C File Offset: 0x00021A2C
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			if (CollectionHelper.ShouldDeallocate(this.Allocator))
			{
				AllocatorManager.Free<T>(this.Allocator, this.Ptr, this.m_capacity);
				this.Allocator = AllocatorManager.Invalid;
			}
			this.Ptr = null;
			this.m_length = 0;
			this.m_capacity = 0;
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00023888 File Offset: 0x00021A88
		public unsafe JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			if (CollectionHelper.ShouldDeallocate(this.Allocator))
			{
				JobHandle jobHandle = new UnsafeDisposeJob
				{
					Ptr = (void*)this.Ptr,
					Allocator = this.Allocator
				}.Schedule(inputDeps);
				this.Ptr = null;
				this.Allocator = AllocatorManager.Invalid;
				return jobHandle;
			}
			this.Ptr = null;
			return inputDeps;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x000238F2 File Offset: 0x00021AF2
		public void Clear()
		{
			this.m_length = 0;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x000238FC File Offset: 0x00021AFC
		public unsafe void Resize(int length, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			int length2 = this.m_length;
			if (length > this.Capacity)
			{
				this.SetCapacity(length);
			}
			this.m_length = length;
			if (options == NativeArrayOptions.ClearMemory && length2 < length)
			{
				int num = length - length2;
				byte* ptr = (byte*)this.Ptr;
				int num2 = sizeof(T);
				UnsafeUtility.MemClear((void*)(ptr + length2 * num2), (long)(num * num2));
			}
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00023950 File Offset: 0x00021B50
		private unsafe void ResizeExact<[global::System.Runtime.CompilerServices.IsUnmanaged] U>(ref U allocator, int newCapacity) where U : struct, ValueType, AllocatorManager.IAllocator
		{
			newCapacity = math.max(0, newCapacity);
			T* ptr = null;
			int num = UnsafeUtility.AlignOf<T>();
			int num2 = sizeof(T);
			if (newCapacity > 0)
			{
				ptr = (T*)(ref allocator).Allocate<U>(num2, num, newCapacity);
				if (this.Ptr != null && this.m_capacity > 0)
				{
					int num3 = math.min(newCapacity, this.Capacity) * num2;
					UnsafeUtility.MemCpy((void*)ptr, (void*)this.Ptr, (long)num3);
				}
			}
			(ref allocator).Free<U, T>(this.Ptr, this.Capacity);
			this.Ptr = ptr;
			this.m_capacity = newCapacity;
			this.m_length = math.min(this.m_length, newCapacity);
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x000239E6 File Offset: 0x00021BE6
		private void ResizeExact(int capacity)
		{
			this.ResizeExact<AllocatorManager.AllocatorHandle>(ref this.Allocator, capacity);
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x000239F8 File Offset: 0x00021BF8
		private void SetCapacity<[global::System.Runtime.CompilerServices.IsUnmanaged] U>(ref U allocator, int capacity) where U : struct, ValueType, AllocatorManager.IAllocator
		{
			int num = sizeof(T);
			int num2 = math.max(capacity, 64 / num);
			num2 = math.ceilpow2(num2);
			if (num2 == this.Capacity)
			{
				return;
			}
			this.ResizeExact<U>(ref allocator, num2);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00023A30 File Offset: 0x00021C30
		public void SetCapacity(int capacity)
		{
			this.SetCapacity<AllocatorManager.AllocatorHandle>(ref this.Allocator, capacity);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00023A3F File Offset: 0x00021C3F
		public void TrimExcess()
		{
			if (this.Capacity != this.m_length)
			{
				this.ResizeExact(this.m_length);
			}
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00023A5B File Offset: 0x00021C5B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void AddNoResize(T value)
		{
			UnsafeUtility.WriteArrayElement<T>((void*)this.Ptr, this.m_length, value);
			this.m_length++;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00023A80 File Offset: 0x00021C80
		public unsafe void AddRangeNoResize(void* ptr, int count)
		{
			int num = sizeof(T);
			void* ptr2 = (void*)(this.Ptr + this.m_length * num / sizeof(T));
			UnsafeUtility.MemCpy(ptr2, ptr, (long)(count * num));
			this.m_length += count;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00023ABD File Offset: 0x00021CBD
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe void AddRangeNoResize(UnsafeList<T> list)
		{
			this.AddRangeNoResize((void*)list.Ptr, CollectionHelper.AssumePositive(list.Length));
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00023AD8 File Offset: 0x00021CD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Add(in T value)
		{
			int length = this.m_length;
			if (this.m_length < this.m_capacity)
			{
				this.Ptr[(IntPtr)length * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
				this.m_length++;
				return;
			}
			this.Resize(length + 1, NativeArrayOptions.UninitializedMemory);
			this.Ptr[(IntPtr)length * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00023B4C File Offset: 0x00021D4C
		public unsafe void AddRange(void* ptr, int count)
		{
			int length = this.m_length;
			if (this.m_length + count > this.Capacity)
			{
				this.Resize(this.m_length + count, NativeArrayOptions.UninitializedMemory);
			}
			else
			{
				this.m_length += count;
			}
			int num = sizeof(T);
			void* ptr2 = (void*)(this.Ptr + length * num / sizeof(T));
			UnsafeUtility.MemCpy(ptr2, ptr, (long)(count * num));
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00023BAC File Offset: 0x00021DAC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe void AddRange(UnsafeList<T> list)
		{
			this.AddRange((void*)list.Ptr, list.Length);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00023BC4 File Offset: 0x00021DC4
		public unsafe void AddReplicate(in T value, int count)
		{
			int length = this.m_length;
			if (this.m_length + count > this.Capacity)
			{
				this.Resize(this.m_length + count, NativeArrayOptions.UninitializedMemory);
			}
			else
			{
				this.m_length += count;
			}
			fixed (T* ptr = &value)
			{
				void* ptr2 = (void*)ptr;
				UnsafeUtility.MemCpyReplicate((void*)(this.Ptr + (IntPtr)length * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), ptr2, UnsafeUtility.SizeOf<T>(), count);
			}
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00023C2C File Offset: 0x00021E2C
		public unsafe void InsertRangeWithBeginEnd(int begin, int end)
		{
			begin = CollectionHelper.AssumePositive(begin);
			end = CollectionHelper.AssumePositive(end);
			int num = end - begin;
			if (num < 1)
			{
				return;
			}
			int length = this.m_length;
			if (this.m_length + num > this.Capacity)
			{
				this.Resize(this.m_length + num, NativeArrayOptions.UninitializedMemory);
			}
			else
			{
				this.m_length += num;
			}
			int num2 = length - begin;
			if (num2 < 1)
			{
				return;
			}
			int num3 = sizeof(T);
			int num4 = num2 * num3;
			byte* ptr = (byte*)this.Ptr;
			void* ptr2 = (void*)(ptr + end * num3);
			byte* ptr3 = ptr + begin * num3;
			UnsafeUtility.MemMove(ptr2, (void*)ptr3, (long)num4);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00023CB9 File Offset: 0x00021EB9
		public void InsertRange(int index, int count)
		{
			this.InsertRangeWithBeginEnd(index, index + count);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00023CC8 File Offset: 0x00021EC8
		public unsafe void RemoveAtSwapBack(int index)
		{
			index = CollectionHelper.AssumePositive(index);
			int num = this.m_length - 1;
			ref T ptr = ref this.Ptr[(IntPtr)index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
			T* ptr2 = this.Ptr + (IntPtr)num * (IntPtr)sizeof(T) / (IntPtr)sizeof(T);
			ptr = *ptr2;
			this.m_length--;
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00023D20 File Offset: 0x00021F20
		public unsafe void RemoveRangeSwapBack(int index, int count)
		{
			index = CollectionHelper.AssumePositive(index);
			count = CollectionHelper.AssumePositive(count);
			if (count > 0)
			{
				int num = math.max(this.m_length - count, index + count);
				int num2 = sizeof(T);
				void* ptr = (void*)(this.Ptr + index * num2 / sizeof(T));
				void* ptr2 = (void*)(this.Ptr + num * num2 / sizeof(T));
				UnsafeUtility.MemCpy(ptr, ptr2, (long)((this.m_length - num) * num2));
				this.m_length -= count;
			}
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00023D90 File Offset: 0x00021F90
		public unsafe void RemoveAt(int index)
		{
			index = CollectionHelper.AssumePositive(index);
			T* ptr = this.Ptr + (IntPtr)index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T);
			T* ptr2 = ptr + sizeof(T) / sizeof(T);
			this.m_length--;
			for (int i = index; i < this.m_length; i++)
			{
				T* ptr3 = ptr;
				ptr = ptr3 + sizeof(T) / sizeof(T);
				T* ptr4 = ptr2;
				ptr2 = ptr4 + sizeof(T) / sizeof(T);
				*ptr3 = *ptr4;
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00023DFC File Offset: 0x00021FFC
		public unsafe void RemoveRange(int index, int count)
		{
			index = CollectionHelper.AssumePositive(index);
			count = CollectionHelper.AssumePositive(count);
			if (count > 0)
			{
				int num = math.min(index + count, this.m_length);
				int num2 = sizeof(T);
				void* ptr = (void*)(this.Ptr + index * num2 / sizeof(T));
				void* ptr2 = (void*)(this.Ptr + num * num2 / sizeof(T));
				UnsafeUtility.MemCpy(ptr, ptr2, (long)((this.m_length - num) * num2));
				this.m_length -= count;
			}
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00023E69 File Offset: 0x00022069
		public UnsafeList<T>.ReadOnly AsReadOnly()
		{
			return new UnsafeList<T>.ReadOnly(this.Ptr, this.Length);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00023E7C File Offset: 0x0002207C
		public UnsafeList<T>.ParallelReader AsParallelReader()
		{
			return new UnsafeList<T>.ParallelReader(this.Ptr, this.Length);
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00023E8F File Offset: 0x0002208F
		public unsafe UnsafeList<T>.ParallelWriter AsParallelWriter()
		{
			return new UnsafeList<T>.ParallelWriter((UnsafeList<T>*)UnsafeUtility.AddressOf<UnsafeList<T>>(ref this));
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00023E9C File Offset: 0x0002209C
		public unsafe void CopyFrom(in NativeArray<T> other)
		{
			NativeArray<T> nativeArray = other;
			this.Resize(nativeArray.Length, NativeArrayOptions.UninitializedMemory);
			void* ptr = (void*)this.Ptr;
			void* unsafeReadOnlyPtr = other.GetUnsafeReadOnlyPtr<T>();
			int num = UnsafeUtility.SizeOf<T>();
			nativeArray = other;
			UnsafeUtility.MemCpy(ptr, unsafeReadOnlyPtr, (long)(num * nativeArray.Length));
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00023EE9 File Offset: 0x000220E9
		public unsafe void CopyFrom(in UnsafeList<T> other)
		{
			this.Resize(other.Length, NativeArrayOptions.UninitializedMemory);
			UnsafeUtility.MemCpy((void*)this.Ptr, (void*)other.Ptr, (long)(UnsafeUtility.SizeOf<T>() * other.Length));
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00023F18 File Offset: 0x00022118
		public UnsafeList<T>.Enumerator GetEnumerator()
		{
			return new UnsafeList<T>.Enumerator
			{
				m_Ptr = this.Ptr,
				m_Length = this.Length,
				m_Index = -1
			};
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00023F50 File Offset: 0x00022150
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal unsafe static void CheckNull(void* listData)
		{
			if (listData == null)
			{
				throw new InvalidOperationException("UnsafeList has yet to be created or has been destroyed!");
			}
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00023F64 File Offset: 0x00022164
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckIndexCount(int index, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(string.Format("Value for count {0} must be positive.", count));
			}
			if (index < 0)
			{
				throw new IndexOutOfRangeException(string.Format("Value for index {0} must be positive.", index));
			}
			if (index > this.Length)
			{
				throw new IndexOutOfRangeException(string.Format("Value for index {0} is out of bounds.", index));
			}
			if (index + count > this.Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Value for count {0} is out of bounds.", count));
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00023FE5 File Offset: 0x000221E5
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckBeginEndNoLength(int begin, int end)
		{
			if (begin > end)
			{
				throw new ArgumentException(string.Format("Value for begin {0} index must less or equal to end {1}.", begin, end));
			}
			if (begin < 0)
			{
				throw new ArgumentOutOfRangeException(string.Format("Value for begin {0} must be positive.", begin));
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00024021 File Offset: 0x00022221
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckBeginEnd(int begin, int end)
		{
			if (begin > this.Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Value for begin {0} is out of bounds.", begin));
			}
			if (end > this.Length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Value for end {0} is out of bounds.", end));
			}
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckNoResizeHasEnoughCapacity(int length)
		{
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00024061 File Offset: 0x00022261
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckNoResizeHasEnoughCapacity(int length, int index)
		{
			if (this.Capacity < index + length)
			{
				throw new InvalidOperationException(string.Format("AddNoResize assumes that list capacity is sufficient (Capacity {0}, Length {1}), requested length {2}!", this.Capacity, this.Length, length));
			}
		}

		// Token: 0x040004CD RID: 1229
		[NativeDisableUnsafePtrRestriction]
		public unsafe T* Ptr;

		// Token: 0x040004CE RID: 1230
		public int m_length;

		// Token: 0x040004CF RID: 1231
		public int m_capacity;

		// Token: 0x040004D0 RID: 1232
		public AllocatorManager.AllocatorHandle Allocator;

		// Token: 0x040004D1 RID: 1233
		private readonly int padding;

		// Token: 0x02000116 RID: 278
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ReadOnly : IEnumerable<T>, IEnumerable
		{
			// Token: 0x17000160 RID: 352
			// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0002409A File Offset: 0x0002229A
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.Ptr != null;
				}
			}

			// Token: 0x17000161 RID: 353
			// (get) Token: 0x06000BEF RID: 3055 RVA: 0x000240A9 File Offset: 0x000222A9
			public readonly bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return !this.IsCreated || this.Length == 0;
				}
			}

			// Token: 0x06000BF0 RID: 3056 RVA: 0x000240BE File Offset: 0x000222BE
			internal unsafe ReadOnly(T* ptr, int length)
			{
				this.Ptr = ptr;
				this.Length = length;
			}

			// Token: 0x06000BF1 RID: 3057 RVA: 0x000240D0 File Offset: 0x000222D0
			public UnsafeList<T>.Enumerator GetEnumerator()
			{
				return new UnsafeList<T>.Enumerator
				{
					m_Ptr = this.Ptr,
					m_Length = this.Length,
					m_Index = -1
				};
			}

			// Token: 0x06000BF2 RID: 3058 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000BF3 RID: 3059 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x040004D2 RID: 1234
			[NativeDisableUnsafePtrRestriction]
			public unsafe readonly T* Ptr;

			// Token: 0x040004D3 RID: 1235
			public readonly int Length;
		}

		// Token: 0x02000117 RID: 279
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ParallelReader
		{
			// Token: 0x06000BF4 RID: 3060 RVA: 0x00024108 File Offset: 0x00022308
			internal unsafe ParallelReader(T* ptr, int length)
			{
				this.Ptr = ptr;
				this.Length = length;
			}

			// Token: 0x040004D4 RID: 1236
			[NativeDisableUnsafePtrRestriction]
			public unsafe readonly T* Ptr;

			// Token: 0x040004D5 RID: 1237
			public readonly int Length;
		}

		// Token: 0x02000118 RID: 280
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ParallelWriter
		{
			// Token: 0x17000162 RID: 354
			// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00024118 File Offset: 0x00022318
			public unsafe readonly void* Ptr
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return (void*)this.ListData->Ptr;
				}
			}

			// Token: 0x06000BF6 RID: 3062 RVA: 0x00024125 File Offset: 0x00022325
			internal unsafe ParallelWriter(UnsafeList<T>* listData)
			{
				this.ListData = listData;
			}

			// Token: 0x06000BF7 RID: 3063 RVA: 0x00024130 File Offset: 0x00022330
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe void AddNoResize(T value)
			{
				int num = Interlocked.Increment(ref this.ListData->m_length) - 1;
				UnsafeUtility.WriteArrayElement<T>((void*)this.ListData->Ptr, num, value);
			}

			// Token: 0x06000BF8 RID: 3064 RVA: 0x00024164 File Offset: 0x00022364
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe void AddRangeNoResize(void* ptr, int count)
			{
				int num = Interlocked.Add(ref this.ListData->m_length, count) - count;
				void* ptr2 = (void*)(this.ListData->Ptr + num * sizeof(T) / sizeof(T));
				UnsafeUtility.MemCpy(ptr2, ptr, (long)(count * sizeof(T)));
			}

			// Token: 0x06000BF9 RID: 3065 RVA: 0x000241AA File Offset: 0x000223AA
			[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
			public unsafe void AddRangeNoResize(UnsafeList<T> list)
			{
				this.AddRangeNoResize((void*)list.Ptr, list.Length);
			}

			// Token: 0x040004D6 RID: 1238
			[NativeDisableUnsafePtrRestriction]
			public unsafe UnsafeList<T>* ListData;
		}

		// Token: 0x02000119 RID: 281
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06000BFA RID: 3066 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000BFB RID: 3067 RVA: 0x000241C0 File Offset: 0x000223C0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				int num = this.m_Index + 1;
				this.m_Index = num;
				return num < this.m_Length;
			}

			// Token: 0x06000BFC RID: 3068 RVA: 0x000241E6 File Offset: 0x000223E6
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x17000163 RID: 355
			// (get) Token: 0x06000BFD RID: 3069 RVA: 0x000241EF File Offset: 0x000223EF
			public unsafe T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Ptr[(IntPtr)this.m_Index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
				}
			}

			// Token: 0x17000164 RID: 356
			// (get) Token: 0x06000BFE RID: 3070 RVA: 0x0002420B File Offset: 0x0002240B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040004D7 RID: 1239
			internal unsafe T* m_Ptr;

			// Token: 0x040004D8 RID: 1240
			internal int m_Length;

			// Token: 0x040004D9 RID: 1241
			internal int m_Index;
		}
	}
}
