using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x02000094 RID: 148
	[NativeContainer]
	[DebuggerTypeProxy(typeof(NativeHashSetDebuggerTypeProxy<>))]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct NativeHashSet<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable, IEnumerable<T>, IEnumerable where T : struct, ValueType, IEquatable<T>
	{
		// Token: 0x06000763 RID: 1891 RVA: 0x00017B60 File Offset: 0x00015D60
		public NativeHashSet(int initialCapacity, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_Data = HashMapHelper<T>.Alloc(initialCapacity, 0, 256, allocator);
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x00017B75 File Offset: 0x00015D75
		public unsafe readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return !this.IsCreated || this.m_Data->IsEmpty;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x00017B8C File Offset: 0x00015D8C
		public unsafe readonly int Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data->Count;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00017B99 File Offset: 0x00015D99
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x00017BA6 File Offset: 0x00015DA6
		public unsafe int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.m_Data->Capacity;
			}
			set
			{
				this.m_Data->Resize(value);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00017BB4 File Offset: 0x00015DB4
		public unsafe readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data != null && this.m_Data->IsCreated;
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00017BCD File Offset: 0x00015DCD
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			HashMapHelper<T>.Free(this.m_Data);
			this.m_Data = null;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00017BEC File Offset: 0x00015DEC
		public unsafe JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new NativeHashMapDisposeJob
			{
				Data = new NativeHashMapDispose
				{
					m_HashMapData = (UnsafeHashMap<int, int>*)this.m_Data
				}
			}.Schedule(inputDeps);
			this.m_Data = null;
			return jobHandle;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00017C37 File Offset: 0x00015E37
		public unsafe void Clear()
		{
			this.m_Data->Clear();
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00017C44 File Offset: 0x00015E44
		public unsafe bool Add(T item)
		{
			return -1 != this.m_Data->TryAdd(in item);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00017C59 File Offset: 0x00015E59
		public unsafe bool Remove(T item)
		{
			return -1 != this.m_Data->TryRemove(item);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00017C6D File Offset: 0x00015E6D
		public unsafe bool Contains(T item)
		{
			return -1 != this.m_Data->Find(item);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00017C81 File Offset: 0x00015E81
		public unsafe void TrimExcess()
		{
			this.m_Data->TrimExcess();
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00017C8E File Offset: 0x00015E8E
		public unsafe NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this.m_Data->GetKeyArray(allocator);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00017C9C File Offset: 0x00015E9C
		public NativeHashSet<T>.Enumerator GetEnumerator()
		{
			return new NativeHashSet<T>.Enumerator
			{
				m_Enumerator = new HashMapHelper<T>.Enumerator(this.m_Data)
			};
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0000790F File Offset: 0x00005B0F
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00017CC4 File Offset: 0x00015EC4
		public NativeHashSet<T>.ReadOnly AsReadOnly()
		{
			return new NativeHashSet<T>.ReadOnly(ref this);
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void CheckRead()
		{
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckWrite()
		{
		}

		// Token: 0x040003BF RID: 959
		[NativeDisableUnsafePtrRestriction]
		internal unsafe HashMapHelper<T>* m_Data;

		// Token: 0x02000095 RID: 149
		[NativeContainer]
		[NativeContainerIsReadOnly]
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06000777 RID: 1911 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000778 RID: 1912 RVA: 0x00017CCC File Offset: 0x00015ECC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x06000779 RID: 1913 RVA: 0x00017CD9 File Offset: 0x00015ED9
			public void Reset()
			{
				this.m_Enumerator.Reset();
			}

			// Token: 0x170000C5 RID: 197
			// (get) Token: 0x0600077A RID: 1914 RVA: 0x00017CE6 File Offset: 0x00015EE6
			public T Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Enumerator.GetCurrentKey();
				}
			}

			// Token: 0x170000C6 RID: 198
			// (get) Token: 0x0600077B RID: 1915 RVA: 0x00017CF3 File Offset: 0x00015EF3
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040003C0 RID: 960
			[NativeDisableUnsafePtrRestriction]
			internal HashMapHelper<T>.Enumerator m_Enumerator;
		}

		// Token: 0x02000096 RID: 150
		[NativeContainer]
		[NativeContainerIsReadOnly]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ReadOnly : IEnumerable<T>, IEnumerable
		{
			// Token: 0x0600077C RID: 1916 RVA: 0x00017D00 File Offset: 0x00015F00
			internal ReadOnly(ref NativeHashSet<T> data)
			{
				this.m_Data = data.m_Data;
			}

			// Token: 0x170000C7 RID: 199
			// (get) Token: 0x0600077D RID: 1917 RVA: 0x00017D0E File Offset: 0x00015F0E
			public unsafe readonly bool IsCreated
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data != null && this.m_Data->IsCreated;
				}
			}

			// Token: 0x170000C8 RID: 200
			// (get) Token: 0x0600077E RID: 1918 RVA: 0x00017D27 File Offset: 0x00015F27
			public unsafe readonly bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return !this.IsCreated || this.m_Data->IsEmpty;
				}
			}

			// Token: 0x170000C9 RID: 201
			// (get) Token: 0x0600077F RID: 1919 RVA: 0x00017D3E File Offset: 0x00015F3E
			public unsafe readonly int Count
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data->Count;
				}
			}

			// Token: 0x170000CA RID: 202
			// (get) Token: 0x06000780 RID: 1920 RVA: 0x00017D4B File Offset: 0x00015F4B
			public unsafe readonly int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.m_Data->Capacity;
				}
			}

			// Token: 0x06000781 RID: 1921 RVA: 0x00017D58 File Offset: 0x00015F58
			public unsafe readonly bool Contains(T item)
			{
				return -1 != this.m_Data->Find(item);
			}

			// Token: 0x06000782 RID: 1922 RVA: 0x00017D6C File Offset: 0x00015F6C
			public unsafe readonly NativeArray<T> ToNativeArray(AllocatorManager.AllocatorHandle allocator)
			{
				return this.m_Data->GetKeyArray(allocator);
			}

			// Token: 0x06000783 RID: 1923 RVA: 0x00017D7C File Offset: 0x00015F7C
			public readonly NativeHashSet<T>.Enumerator GetEnumerator()
			{
				return new NativeHashSet<T>.Enumerator
				{
					m_Enumerator = new HashMapHelper<T>.Enumerator(this.m_Data)
				};
			}

			// Token: 0x06000784 RID: 1924 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000785 RID: 1925 RVA: 0x0000790F File Offset: 0x00005B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000786 RID: 1926 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private readonly void CheckRead()
			{
			}

			// Token: 0x040003C1 RID: 961
			[NativeDisableUnsafePtrRestriction]
			internal unsafe HashMapHelper<T>* m_Data;
		}
	}
}
