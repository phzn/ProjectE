using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200010F RID: 271
	[DebuggerTypeProxy(typeof(UnsafeHashSetDebuggerTypeProxy<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct UnsafeHashSet<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable, IEnumerable<T>, IEnumerable where T : struct, ValueType, IEquatable<T>
	{
		// Token: 0x06000B96 RID: 2966 RVA: 0x00023330 File Offset: 0x00021530
		public UnsafeHashSet(int initialCapacity, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_Data = default(HashMapHelper<T>);
			this.m_Data.Init(initialCapacity, 0, 256, allocator);
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x00023351 File Offset: 0x00021551
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return !this.IsCreated || this.m_Data.IsEmpty;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00023368 File Offset: 0x00021568
		public readonly int Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.Count;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x00023375 File Offset: 0x00021575
		// (set) Token: 0x06000B9A RID: 2970 RVA: 0x00023382 File Offset: 0x00021582
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.m_Data.Capacity;
			}
			set
			{
				this.m_Data.Resize(value);
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x00023390 File Offset: 0x00021590
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data.IsCreated;
			}
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0002339D File Offset: 0x0002159D
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			this.m_Data.Dispose();
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x000233B4 File Offset: 0x000215B4
		public unsafe JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new UnsafeDisposeJob
			{
				Ptr = (void*)this.m_Data.Ptr,
				Allocator = this.m_Data.Allocator
			}.Schedule(inputDeps);
			this.m_Data.Ptr = null;
			return jobHandle;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0002340B File Offset: 0x0002160B
		public void Clear()
		{
			this.m_Data.Clear();
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00023418 File Offset: 0x00021618
		public bool Add(T item)
		{
			return -1 != this.m_Data.TryAdd(in item);
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0002342D File Offset: 0x0002162D
		public bool Remove(T item)
		{
			return -1 != this.m_Data.TryRemove(item);
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00023441 File Offset: 0x00021641
		public bool Contains(T item)
		{
			return -1 != this.m_Data.Find(item);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00023455 File Offset: 0x00021655
		public void TrimExcess()
		{
			this.m_Data.TrimExcess();
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00023462 File Offset: 0x00021662
		public NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_Data.GetKeyArray(allocator);
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00023470 File Offset: 0x00021670
		public unsafe UnsafeHashSet<T>.Enumerator GetEnumerator()
		{
			fixed (HashMapHelper<T>* ptr = &this.m_Data)
			{
				HashMapHelper<T>* ptr2 = ptr;
				return new UnsafeHashSet<T>.Enumerator
				{
					m_Enumerator = new HashMapHelper<T>.Enumerator(ptr2)
				};
			}
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0002349D File Offset: 0x0002169D
		public UnsafeHashSet<T>.ReadOnly AsReadOnly()
		{
			return new UnsafeHashSet<T>.ReadOnly(ref this.m_Data);
		}

		// Token: 0x040004C2 RID: 1218
		internal HashMapHelper<T> m_Data;

		// Token: 0x02000110 RID: 272
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06000BA8 RID: 2984 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000BA9 RID: 2985 RVA: 0x000234AA File Offset: 0x000216AA
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000BAA RID: 2986 RVA: 0x000234B7 File Offset: 0x000216B7
			public void Reset()
			{
				this.m_Enumerator.Reset();
			}

			// Token: 0x17000154 RID: 340
			// (get) Token: 0x06000BAB RID: 2987 RVA: 0x000234C4 File Offset: 0x000216C4
			public unsafe T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Enumerator.m_Data->Keys[(IntPtr)this.m_Enumerator.m_Index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
				}
			}

			// Token: 0x17000155 RID: 341
			// (get) Token: 0x06000BAC RID: 2988 RVA: 0x000234EF File Offset: 0x000216EF
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040004C3 RID: 1219
			internal HashMapHelper<T>.Enumerator m_Enumerator;
		}

		// Token: 0x02000111 RID: 273
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ReadOnly : IEnumerable<T>, IEnumerable
		{
			// Token: 0x06000BAD RID: 2989 RVA: 0x000234FC File Offset: 0x000216FC
			internal ReadOnly(ref HashMapHelper<T> data)
			{
				this.m_Data = data;
			}

			// Token: 0x17000156 RID: 342
			// (get) Token: 0x06000BAE RID: 2990 RVA: 0x0002350A File Offset: 0x0002170A
			public readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.IsCreated;
				}
			}

			// Token: 0x17000157 RID: 343
			// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00023517 File Offset: 0x00021717
			public readonly bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.IsEmpty;
				}
			}

			// Token: 0x17000158 RID: 344
			// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x00023524 File Offset: 0x00021724
			public readonly int Count
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.Count;
				}
			}

			// Token: 0x17000159 RID: 345
			// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00023531 File Offset: 0x00021731
			public readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data.Capacity;
				}
			}

			// Token: 0x06000BB2 RID: 2994 RVA: 0x00023540 File Offset: 0x00021740
			public readonly bool Contains(T item)
			{
				int num = -1;
				HashMapHelper<T> data = this.m_Data;
				return num != data.Find(item);
			}

			// Token: 0x06000BB3 RID: 2995 RVA: 0x00023564 File Offset: 0x00021764
			public readonly NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
			{
				HashMapHelper<T> data = this.m_Data;
				return data.GetKeyArray(allocator);
			}

			// Token: 0x06000BB4 RID: 2996 RVA: 0x00023580 File Offset: 0x00021780
			public unsafe readonly UnsafeHashSet<T>.Enumerator GetEnumerator()
			{
				fixed (HashMapHelper<T>* ptr = &this.m_Data)
				{
					HashMapHelper<T>* ptr2 = ptr;
					return new UnsafeHashSet<T>.Enumerator
					{
						m_Enumerator = new HashMapHelper<T>.Enumerator(ptr2)
					};
				}
			}

			// Token: 0x06000BB5 RID: 2997 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000BB6 RID: 2998 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x040004C4 RID: 1220
			internal HashMapHelper<T> m_Data;
		}
	}
}
