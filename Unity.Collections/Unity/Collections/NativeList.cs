using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x0200009A RID: 154
	[NativeContainer]
	[DebuggerDisplay("Length = {m_ListData == null ? default : m_ListData->Length}, Capacity = {m_ListData == null ? default : m_ListData->Capacity}")]
	[DebuggerTypeProxy(typeof(NativeListDebugView<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct NativeList<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable, INativeList<T>, IIndexable<T>, IEnumerable<T>, IEnumerable where T : struct, ValueType
	{
		// Token: 0x06000792 RID: 1938 RVA: 0x00017E2C File Offset: 0x0001602C
		public NativeList(AllocatorManager.AllocatorHandle allocator)
		{
			this = new NativeList<T>(1, allocator);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00017E38 File Offset: 0x00016038
		public NativeList(int initialCapacity, AllocatorManager.AllocatorHandle allocator)
		{
			this = default(NativeList<T>);
			AllocatorManager.AllocatorHandle allocatorHandle = allocator;
			this.Initialize<AllocatorManager.AllocatorHandle>(initialCapacity, ref allocatorHandle);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00017E57 File Offset: 0x00016057
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(AllocatorManager.AllocatorHandle) })]
		internal void Initialize<[global::System.Runtime.CompilerServices.IsUnmanaged] U>(int initialCapacity, ref U allocator) where U : struct, ValueType, AllocatorManager.IAllocator
		{
			this.m_ListData = UnsafeList<T>.Create<U>(initialCapacity, ref allocator, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00017E68 File Offset: 0x00016068
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(AllocatorManager.AllocatorHandle) })]
		internal static NativeList<T> New<[global::System.Runtime.CompilerServices.IsUnmanaged] U>(int initialCapacity, ref U allocator) where U : struct, ValueType, AllocatorManager.IAllocator
		{
			NativeList<T> nativeList = default(NativeList<T>);
			nativeList.Initialize<U>(initialCapacity, ref allocator);
			return nativeList;
		}

		// Token: 0x170000D0 RID: 208
		public unsafe T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (*this.m_ListData)[index];
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				(*this.m_ListData)[index] = value;
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00017EA4 File Offset: 0x000160A4
		public unsafe ref T ElementAt(int index)
		{
			return this.m_ListData->ElementAt(index);
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00017EB2 File Offset: 0x000160B2
		// (set) Token: 0x0600079A RID: 1946 RVA: 0x00017EC4 File Offset: 0x000160C4
		public unsafe int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return CollectionHelper.AssumePositive(this.m_ListData->Length);
			}
			set
			{
				this.m_ListData->Resize(value, NativeArrayOptions.ClearMemory);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x00017ED3 File Offset: 0x000160D3
		// (set) Token: 0x0600079C RID: 1948 RVA: 0x00017EE0 File Offset: 0x000160E0
		public unsafe int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.m_ListData->Capacity;
			}
			set
			{
				this.m_ListData->Capacity = value;
			}
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00017EEE File Offset: 0x000160EE
		public unsafe UnsafeList<T>* GetUnsafeList()
		{
			return this.m_ListData;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00017EF6 File Offset: 0x000160F6
		public unsafe void AddNoResize(T value)
		{
			this.m_ListData->AddNoResize(value);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00017F04 File Offset: 0x00016104
		public unsafe void AddRangeNoResize(void* ptr, int count)
		{
			this.m_ListData->AddRangeNoResize(ptr, count);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00017F13 File Offset: 0x00016113
		public unsafe void AddRangeNoResize(NativeList<T> list)
		{
			this.m_ListData->AddRangeNoResize(*list.m_ListData);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00017F2B File Offset: 0x0001612B
		public unsafe void Add(in T value)
		{
			this.m_ListData->Add(in value);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00017F39 File Offset: 0x00016139
		public void AddRange(NativeArray<T> array)
		{
			this.AddRange(array.GetUnsafeReadOnlyPtr<T>(), array.Length);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00017F4E File Offset: 0x0001614E
		public unsafe void AddRange(void* ptr, int count)
		{
			this.m_ListData->AddRange(ptr, CollectionHelper.AssumePositive(count));
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00017F62 File Offset: 0x00016162
		public unsafe void AddReplicate(in T value, int count)
		{
			this.m_ListData->AddReplicate(in value, CollectionHelper.AssumePositive(count));
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00017F76 File Offset: 0x00016176
		public unsafe void InsertRangeWithBeginEnd(int begin, int end)
		{
			this.m_ListData->InsertRangeWithBeginEnd(begin, end);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00017F85 File Offset: 0x00016185
		public void InsertRange(int index, int count)
		{
			this.InsertRangeWithBeginEnd(index, index + count);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00017F91 File Offset: 0x00016191
		public unsafe void RemoveAtSwapBack(int index)
		{
			this.m_ListData->RemoveAtSwapBack(index);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00017F9F File Offset: 0x0001619F
		public unsafe void RemoveRangeSwapBack(int index, int count)
		{
			this.m_ListData->RemoveRangeSwapBack(index, count);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00017FAE File Offset: 0x000161AE
		public unsafe void RemoveAt(int index)
		{
			this.m_ListData->RemoveAt(index);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00017FBC File Offset: 0x000161BC
		public unsafe void RemoveRange(int index, int count)
		{
			this.m_ListData->RemoveRange(index, count);
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x00017FCB File Offset: 0x000161CB
		public unsafe readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_ListData == null || this.m_ListData->Length == 0;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x00017FE7 File Offset: 0x000161E7
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_ListData != null;
			}
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00017FF6 File Offset: 0x000161F6
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			UnsafeList<T>.Destroy(this.m_ListData);
			this.m_ListData = null;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00018014 File Offset: 0x00016214
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(AllocatorManager.AllocatorHandle) })]
		internal void Dispose<[global::System.Runtime.CompilerServices.IsUnmanaged] U>(ref U allocator) where U : struct, ValueType, AllocatorManager.IAllocator
		{
			if (!this.IsCreated)
			{
				return;
			}
			UnsafeList<T>.Destroy<U>(this.m_ListData, ref allocator);
			this.m_ListData = null;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00018034 File Offset: 0x00016234
		public unsafe JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new NativeListDisposeJob
			{
				Data = new NativeListDispose
				{
					m_ListData = (UntypedUnsafeList*)this.m_ListData
				}
			}.Schedule(inputDeps);
			this.m_ListData = null;
			return jobHandle;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0001807F File Offset: 0x0001627F
		public unsafe void Clear()
		{
			this.m_ListData->Clear();
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001808C File Offset: 0x0001628C
		[Obsolete("Implicit cast from `NativeList<T>` to `NativeArray<T>` has been deprecated; Use '.AsArray()' method to do explicit cast instead.", false)]
		public static implicit operator NativeArray<T>(NativeList<T> nativeList)
		{
			return nativeList.AsArray();
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00018095 File Offset: 0x00016295
		public unsafe NativeArray<T> AsArray()
		{
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.m_ListData->Ptr, this.m_ListData->Length, Allocator.None);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x000180B4 File Offset: 0x000162B4
		public unsafe NativeArray<T> AsDeferredJobArray()
		{
			byte* ptr = (byte*)this.m_ListData;
			ptr++;
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)ptr, 0, Allocator.Invalid);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x000180D4 File Offset: 0x000162D4
		public unsafe NativeArray<T> ToArray(AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<T> nativeArray = CollectionHelper.CreateNativeArray<T>(this.Length, allocator, NativeArrayOptions.UninitializedMemory);
			UnsafeUtility.MemCpy(nativeArray.m_Buffer, (void*)this.m_ListData->Ptr, (long)(this.Length * UnsafeUtility.SizeOf<T>()));
			return nativeArray;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00018106 File Offset: 0x00016306
		public unsafe void CopyFrom(in NativeArray<T> other)
		{
			this.m_ListData->CopyFrom(in other);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00018114 File Offset: 0x00016314
		public unsafe void CopyFrom(in UnsafeList<T> other)
		{
			this.m_ListData->CopyFrom(in other);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00018122 File Offset: 0x00016322
		public unsafe void CopyFrom(in NativeList<T> other)
		{
			this.CopyFrom(in *other.m_ListData);
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00018130 File Offset: 0x00016330
		public NativeArray<T>.Enumerator GetEnumerator()
		{
			NativeArray<T> nativeArray = this.AsArray();
			return new NativeArray<T>.Enumerator(ref nativeArray);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001814B File Offset: 0x0001634B
		public unsafe void Resize(int length, NativeArrayOptions options)
		{
			this.m_ListData->Resize(length, options);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001815A File Offset: 0x0001635A
		public void ResizeUninitialized(int length)
		{
			this.Resize(length, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00018164 File Offset: 0x00016364
		public unsafe void SetCapacity(int capacity)
		{
			this.m_ListData->SetCapacity(capacity);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00018172 File Offset: 0x00016372
		public unsafe void TrimExcess()
		{
			this.m_ListData->TrimExcess();
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001817F File Offset: 0x0001637F
		public unsafe NativeArray<T>.ReadOnly AsReadOnly()
		{
			return new NativeArray<T>.ReadOnly((void*)this.m_ListData->Ptr, this.m_ListData->Length);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001817F File Offset: 0x0001637F
		public unsafe NativeArray<T>.ReadOnly AsParallelReader()
		{
			return new NativeArray<T>.ReadOnly((void*)this.m_ListData->Ptr, this.m_ListData->Length);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001819C File Offset: 0x0001639C
		public NativeList<T>.ParallelWriter AsParallelWriter()
		{
			return new NativeList<T>.ParallelWriter(this.m_ListData);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x000181A9 File Offset: 0x000163A9
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckInitialCapacity(int initialCapacity)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity", "Capacity must be >= 0");
			}
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x000181BF File Offset: 0x000163BF
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckTotalSize(int initialCapacity, long totalSize)
		{
			if (totalSize > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("initialCapacity", string.Format("Capacity * sizeof(T) cannot exceed {0} bytes", int.MaxValue));
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000181E9 File Offset: 0x000163E9
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckSufficientCapacity(int capacity, int length)
		{
			if (capacity < length)
			{
				throw new InvalidOperationException(string.Format("Length {0} exceeds Capacity {1}", length, capacity));
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001820B File Offset: 0x0001640B
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckIndexInRange(int value, int length)
		{
			if (value < 0)
			{
				throw new IndexOutOfRangeException(string.Format("Value {0} must be positive.", value));
			}
			if (value >= length)
			{
				throw new IndexOutOfRangeException(string.Format("Value {0} is out of range in NativeList of '{1}' Length.", value, length));
			}
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00018247 File Offset: 0x00016447
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckArgPositive(int value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException(string.Format("Value {0} must be positive.", value));
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00018264 File Offset: 0x00016464
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe void CheckHandleMatches(AllocatorManager.AllocatorHandle handle)
		{
			if (this.m_ListData == null)
			{
				throw new ArgumentOutOfRangeException(string.Format("Allocator handle {0} can't match because container is not initialized.", handle));
			}
			if (this.m_ListData->Allocator.Index != handle.Index)
			{
				throw new ArgumentOutOfRangeException(string.Format("Allocator handle {0} can't match because container handle index doesn't match.", handle));
			}
			if (this.m_ListData->Allocator.Version != handle.Version)
			{
				throw new ArgumentOutOfRangeException(string.Format("Allocator handle {0} matches container handle index, but has different version.", handle));
			}
		}

		// Token: 0x040003C3 RID: 963
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeList<T>* m_ListData;

		// Token: 0x0200009B RID: 155
		[NativeContainer]
		[NativeContainerIsAtomicWriteOnly]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ParallelWriter
		{
			// Token: 0x170000D5 RID: 213
			// (get) Token: 0x060007C8 RID: 1992 RVA: 0x000182ED File Offset: 0x000164ED
			public unsafe readonly void* Ptr
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return (void*)this.ListData->Ptr;
				}
			}

			// Token: 0x060007C9 RID: 1993 RVA: 0x000182FA File Offset: 0x000164FA
			internal unsafe ParallelWriter(UnsafeList<T>* listData)
			{
				this.ListData = listData;
			}

			// Token: 0x060007CA RID: 1994 RVA: 0x00018304 File Offset: 0x00016504
			public unsafe void AddNoResize(T value)
			{
				int num = Interlocked.Increment(ref this.ListData->m_length) - 1;
				UnsafeUtility.WriteArrayElement<T>((void*)this.ListData->Ptr, num, value);
			}

			// Token: 0x060007CB RID: 1995 RVA: 0x00018338 File Offset: 0x00016538
			public unsafe void AddRangeNoResize(void* ptr, int count)
			{
				int num = Interlocked.Add(ref this.ListData->m_length, count) - count;
				int num2 = sizeof(T);
				void* ptr2 = (void*)(this.ListData->Ptr + num * num2 / sizeof(T));
				UnsafeUtility.MemCpy(ptr2, ptr, (long)(count * num2));
			}

			// Token: 0x060007CC RID: 1996 RVA: 0x0001837B File Offset: 0x0001657B
			public unsafe void AddRangeNoResize(UnsafeList<T> list)
			{
				this.AddRangeNoResize((void*)list.Ptr, list.Length);
			}

			// Token: 0x060007CD RID: 1997 RVA: 0x00018390 File Offset: 0x00016590
			public unsafe void AddRangeNoResize(NativeList<T> list)
			{
				this.AddRangeNoResize(*list.m_ListData);
			}

			// Token: 0x040003C4 RID: 964
			[NativeDisableUnsafePtrRestriction]
			public unsafe UnsafeList<T>* ListData;
		}
	}
}
