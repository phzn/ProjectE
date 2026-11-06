using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200011C RID: 284
	[DebuggerDisplay("Length = {Length}, Capacity = {Capacity}, IsCreated = {IsCreated}, IsEmpty = {IsEmpty}")]
	[DebuggerTypeProxy(typeof(UnsafePtrListDebugView<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct UnsafePtrList<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable, IEnumerable<IntPtr>, IEnumerable where T : struct, ValueType
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x00024340 File Offset: 0x00022540
		// (set) Token: 0x06000C09 RID: 3081 RVA: 0x00024360 File Offset: 0x00022560
		public int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.ListDataRO<T>().Length;
			}
			set
			{
				(ref this).ListData<T>().Length = value;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00024370 File Offset: 0x00022570
		// (set) Token: 0x06000C0B RID: 3083 RVA: 0x00024390 File Offset: 0x00022590
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.ListDataRO<T>().Capacity;
			}
			set
			{
				(ref this).ListData<T>().Capacity = value;
			}
		}

		// Token: 0x17000168 RID: 360
		public unsafe T* this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return *(IntPtr*)(this.Ptr + (IntPtr)CollectionHelper.AssumePositive(index) * (IntPtr)sizeof(T*) / (IntPtr)sizeof(T*));
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				*(IntPtr*)(this.Ptr + (IntPtr)CollectionHelper.AssumePositive(index) * (IntPtr)sizeof(T*) / (IntPtr)sizeof(T*)) = value;
			}
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000243CF File Offset: 0x000225CF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref T* ElementAt(int index)
		{
			return ref this.Ptr[(IntPtr)CollectionHelper.AssumePositive(index) * (IntPtr)sizeof(T*) / (IntPtr)sizeof(T*)];
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000243E6 File Offset: 0x000225E6
		public unsafe UnsafePtrList(T** ptr, int length)
		{
			this = default(UnsafePtrList<T>);
			this.Ptr = ptr;
			this.m_length = length;
			this.m_capacity = length;
			this.Allocator = AllocatorManager.None;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0002440F File Offset: 0x0002260F
		public unsafe UnsafePtrList(int initialCapacity, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			this.Ptr = null;
			this.m_length = 0;
			this.m_capacity = 0;
			this.padding = 0;
			this.Allocator = AllocatorManager.None;
			*(ref this).ListData<T>() = new UnsafeList<IntPtr>(initialCapacity, allocator, options);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0002444C File Offset: 0x0002264C
		public unsafe static UnsafePtrList<T>* Create(T** ptr, int length)
		{
			UnsafePtrList<T>* ptr2 = AllocatorManager.Allocate<UnsafePtrList<T>>(AllocatorManager.Persistent, 1);
			*ptr2 = new UnsafePtrList<T>(ptr, length);
			return ptr2;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00024466 File Offset: 0x00022666
		public unsafe static UnsafePtrList<T>* Create(int initialCapacity, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			UnsafePtrList<T>* ptr = AllocatorManager.Allocate<UnsafePtrList<T>>(allocator, 1);
			*ptr = new UnsafePtrList<T>(initialCapacity, allocator, options);
			return ptr;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00024480 File Offset: 0x00022680
		public unsafe static void Destroy(UnsafePtrList<T>* listData)
		{
			AllocatorManager.AllocatorHandle allocatorHandle = (((ref *listData).ListData<T>().Allocator.Value == AllocatorManager.Invalid.Value) ? AllocatorManager.Persistent : (ref *listData).ListData<T>().Allocator);
			listData->Dispose();
			AllocatorManager.Free<UnsafePtrList<T>>(allocatorHandle, listData, 1);
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x000244CB File Offset: 0x000226CB
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return !this.IsCreated || this.Length == 0;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x000244E0 File Offset: 0x000226E0
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Ptr != null;
			}
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x000244EF File Offset: 0x000226EF
		public void Dispose()
		{
			(ref this).ListData<T>().Dispose();
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x000244FC File Offset: 0x000226FC
		public JobHandle Dispose(JobHandle inputDeps)
		{
			return (ref this).ListData<T>().Dispose(inputDeps);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0002450A File Offset: 0x0002270A
		public void Clear()
		{
			(ref this).ListData<T>().Clear();
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00024517 File Offset: 0x00022717
		public void Resize(int length, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			(ref this).ListData<T>().Resize(length, options);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00024526 File Offset: 0x00022726
		public void SetCapacity(int capacity)
		{
			(ref this).ListData<T>().SetCapacity(capacity);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00024534 File Offset: 0x00022734
		public void TrimExcess()
		{
			(ref this).ListData<T>().TrimExcess();
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00024544 File Offset: 0x00022744
		public unsafe int IndexOf(void* ptr)
		{
			for (int i = 0; i < this.Length; i++)
			{
				if (*(IntPtr*)(this.Ptr + (IntPtr)i * (IntPtr)sizeof(T*) / (IntPtr)sizeof(T*)) == ptr)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00024579 File Offset: 0x00022779
		public unsafe bool Contains(void* ptr)
		{
			return this.IndexOf(ptr) != -1;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00024588 File Offset: 0x00022788
		public unsafe void AddNoResize(void* value)
		{
			(ref this).ListData<T>().AddNoResize((IntPtr)value);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0002459B File Offset: 0x0002279B
		public unsafe void AddRangeNoResize(void** ptr, int count)
		{
			(ref this).ListData<T>().AddRangeNoResize((void*)ptr, count);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x000245AA File Offset: 0x000227AA
		public unsafe void AddRangeNoResize(UnsafePtrList<T> list)
		{
			(ref this).ListData<T>().AddRangeNoResize((void*)list.Ptr, list.Length);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x000245C4 File Offset: 0x000227C4
		public void Add(in IntPtr value)
		{
			(ref this).ListData<T>().Add(in value);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x000245D4 File Offset: 0x000227D4
		public unsafe void Add(void* value)
		{
			ref UnsafeList<IntPtr> ptr = ref (ref this).ListData<T>();
			IntPtr intPtr = (IntPtr)value;
			ptr.Add(in intPtr);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000245F5 File Offset: 0x000227F5
		public unsafe void AddRange(void* ptr, int length)
		{
			(ref this).ListData<T>().AddRange(ptr, length);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00024604 File Offset: 0x00022804
		public unsafe void AddRange(UnsafePtrList<T> list)
		{
			(ref this).ListData<T>().AddRange(*(ref list).ListData<T>());
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0002461D File Offset: 0x0002281D
		public void InsertRangeWithBeginEnd(int begin, int end)
		{
			(ref this).ListData<T>().InsertRangeWithBeginEnd(begin, end);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0002462C File Offset: 0x0002282C
		public void RemoveAtSwapBack(int index)
		{
			(ref this).ListData<T>().RemoveAtSwapBack(index);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0002463A File Offset: 0x0002283A
		public void RemoveRangeSwapBack(int index, int count)
		{
			(ref this).ListData<T>().RemoveRangeSwapBack(index, count);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00024649 File Offset: 0x00022849
		public void RemoveAt(int index)
		{
			(ref this).ListData<T>().RemoveAt(index);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00024657 File Offset: 0x00022857
		public void RemoveRange(int index, int count)
		{
			(ref this).ListData<T>().RemoveRange(index, count);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<IntPtr> IEnumerable<IntPtr>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00024666 File Offset: 0x00022866
		public UnsafePtrList<T>.ReadOnly AsReadOnly()
		{
			return new UnsafePtrList<T>.ReadOnly(this.Ptr, this.Length);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00024679 File Offset: 0x00022879
		public UnsafePtrList<T>.ParallelReader AsParallelReader()
		{
			return new UnsafePtrList<T>.ParallelReader(this.Ptr, this.Length);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0002468C File Offset: 0x0002288C
		public unsafe UnsafePtrList<T>.ParallelWriter AsParallelWriter()
		{
			return new UnsafePtrList<T>.ParallelWriter(this.Ptr, (UnsafeList<IntPtr>*)UnsafeUtility.AddressOf<UnsafePtrList<T>>(ref this));
		}

		// Token: 0x040004DB RID: 1243
		[NativeDisableUnsafePtrRestriction]
		public unsafe readonly T** Ptr;

		// Token: 0x040004DC RID: 1244
		public readonly int m_length;

		// Token: 0x040004DD RID: 1245
		public readonly int m_capacity;

		// Token: 0x040004DE RID: 1246
		public readonly AllocatorManager.AllocatorHandle Allocator;

		// Token: 0x040004DF RID: 1247
		private readonly int padding;

		// Token: 0x0200011D RID: 285
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ReadOnly
		{
			// Token: 0x1700016B RID: 363
			// (get) Token: 0x06000C2F RID: 3119 RVA: 0x0002469F File Offset: 0x0002289F
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.Ptr != null;
				}
			}

			// Token: 0x1700016C RID: 364
			// (get) Token: 0x06000C30 RID: 3120 RVA: 0x000246AE File Offset: 0x000228AE
			public readonly bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return !this.IsCreated || this.Length == 0;
				}
			}

			// Token: 0x06000C31 RID: 3121 RVA: 0x000246C3 File Offset: 0x000228C3
			internal unsafe ReadOnly(T** ptr, int length)
			{
				this.Ptr = ptr;
				this.Length = length;
			}

			// Token: 0x06000C32 RID: 3122 RVA: 0x000246D4 File Offset: 0x000228D4
			public unsafe int IndexOf(void* ptr)
			{
				for (int i = 0; i < this.Length; i++)
				{
					if (*(IntPtr*)(this.Ptr + (IntPtr)i * (IntPtr)sizeof(T*) / (IntPtr)sizeof(T*)) == ptr)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06000C33 RID: 3123 RVA: 0x00024709 File Offset: 0x00022909
			public unsafe bool Contains(void* ptr)
			{
				return this.IndexOf(ptr) != -1;
			}

			// Token: 0x040004E0 RID: 1248
			[NativeDisableUnsafePtrRestriction]
			public unsafe readonly T** Ptr;

			// Token: 0x040004E1 RID: 1249
			public readonly int Length;
		}

		// Token: 0x0200011E RID: 286
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ParallelReader
		{
			// Token: 0x06000C34 RID: 3124 RVA: 0x00024718 File Offset: 0x00022918
			internal unsafe ParallelReader(T** ptr, int length)
			{
				this.Ptr = ptr;
				this.Length = length;
			}

			// Token: 0x06000C35 RID: 3125 RVA: 0x00024728 File Offset: 0x00022928
			public unsafe int IndexOf(void* ptr)
			{
				for (int i = 0; i < this.Length; i++)
				{
					if (*(IntPtr*)(this.Ptr + (IntPtr)i * (IntPtr)sizeof(T*) / (IntPtr)sizeof(T*)) == ptr)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06000C36 RID: 3126 RVA: 0x0002475D File Offset: 0x0002295D
			public unsafe bool Contains(void* ptr)
			{
				return this.IndexOf(ptr) != -1;
			}

			// Token: 0x040004E2 RID: 1250
			[NativeDisableUnsafePtrRestriction]
			public unsafe readonly T** Ptr;

			// Token: 0x040004E3 RID: 1251
			public readonly int Length;
		}

		// Token: 0x0200011F RID: 287
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ParallelWriter
		{
			// Token: 0x06000C37 RID: 3127 RVA: 0x0002476C File Offset: 0x0002296C
			internal unsafe ParallelWriter(T** ptr, UnsafeList<IntPtr>* listData)
			{
				this.Ptr = ptr;
				this.ListData = listData;
			}

			// Token: 0x06000C38 RID: 3128 RVA: 0x0002477C File Offset: 0x0002297C
			public unsafe void AddNoResize(T* value)
			{
				this.ListData->AddNoResize((IntPtr)((void*)value));
			}

			// Token: 0x06000C39 RID: 3129 RVA: 0x0002478F File Offset: 0x0002298F
			public unsafe void AddRangeNoResize(T** ptr, int count)
			{
				this.ListData->AddRangeNoResize((void*)ptr, count);
			}

			// Token: 0x06000C3A RID: 3130 RVA: 0x0002479E File Offset: 0x0002299E
			public unsafe void AddRangeNoResize(UnsafePtrList<T> list)
			{
				this.ListData->AddRangeNoResize((void*)list.Ptr, list.Length);
			}

			// Token: 0x040004E4 RID: 1252
			[NativeDisableUnsafePtrRestriction]
			public unsafe readonly T** Ptr;

			// Token: 0x040004E5 RID: 1253
			[NativeDisableUnsafePtrRestriction]
			public unsafe UnsafeList<IntPtr>* ListData;
		}
	}
}
