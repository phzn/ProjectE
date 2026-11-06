using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000BB RID: 187
	[NativeContainer]
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
	public struct NativeReference<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : INativeDisposable, IDisposable, IEquatable<NativeReference<T>> where T : struct, ValueType
	{
		// Token: 0x060008E6 RID: 2278 RVA: 0x0001ADFF File Offset: 0x00018FFF
		public NativeReference(AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
		{
			NativeReference<T>.Allocate(allocator, out this);
			if (options == NativeArrayOptions.ClearMemory)
			{
				UnsafeUtility.MemClear(this.m_Data, (long)UnsafeUtility.SizeOf<T>());
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001AE1D File Offset: 0x0001901D
		public unsafe NativeReference(T value, AllocatorManager.AllocatorHandle allocator)
		{
			NativeReference<T>.Allocate(allocator, out this);
			*(T*)this.m_Data = value;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001AE32 File Offset: 0x00019032
		private static void Allocate(AllocatorManager.AllocatorHandle allocator, out NativeReference<T> reference)
		{
			reference = default(NativeReference<T>);
			reference.m_Data = Memory.Unmanaged.Allocate((long)UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator);
			reference.m_AllocatorLabel = allocator;
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0001AE59 File Offset: 0x00019059
		// (set) Token: 0x060008EA RID: 2282 RVA: 0x0001AE66 File Offset: 0x00019066
		public unsafe T Value
		{
			get
			{
				return *(T*)this.m_Data;
			}
			set
			{
				*(T*)this.m_Data = value;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0001AE74 File Offset: 0x00019074
		public readonly bool IsCreated
		{
			get
			{
				return this.m_Data != null;
			}
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001AE83 File Offset: 0x00019083
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			if (CollectionHelper.ShouldDeallocate(this.m_AllocatorLabel))
			{
				Memory.Unmanaged.Free(this.m_Data, this.m_AllocatorLabel);
				this.m_AllocatorLabel = Allocator.Invalid;
			}
			this.m_Data = null;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001AEC0 File Offset: 0x000190C0
		public JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			if (CollectionHelper.ShouldDeallocate(this.m_AllocatorLabel))
			{
				JobHandle jobHandle = new NativeReferenceDisposeJob
				{
					Data = new NativeReferenceDispose
					{
						m_Data = this.m_Data,
						m_AllocatorLabel = this.m_AllocatorLabel
					}
				}.Schedule(inputDeps);
				this.m_Data = null;
				this.m_AllocatorLabel = Allocator.Invalid;
				return jobHandle;
			}
			this.m_Data = null;
			return inputDeps;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001AF3B File Offset: 0x0001913B
		public void CopyFrom(NativeReference<T> reference)
		{
			NativeReference<T>.Copy(this, reference);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0001AF49 File Offset: 0x00019149
		public void CopyTo(NativeReference<T> reference)
		{
			NativeReference<T>.Copy(reference, this);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0001AF58 File Offset: 0x00019158
		[ExcludeFromBurstCompatTesting("Equals boxes because Value does not implement IEquatable<T>")]
		public bool Equals(NativeReference<T> other)
		{
			T value = this.Value;
			return value.Equals(other.Value);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0001AF85 File Offset: 0x00019185
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		public override bool Equals(object obj)
		{
			return obj != null && obj is NativeReference<T> && this.Equals((NativeReference<T>)obj);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0001AFA4 File Offset: 0x000191A4
		public override int GetHashCode()
		{
			T value = this.Value;
			return value.GetHashCode();
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001AFC5 File Offset: 0x000191C5
		public static bool operator ==(NativeReference<T> left, NativeReference<T> right)
		{
			return left.Equals(right);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001AFCF File Offset: 0x000191CF
		public static bool operator !=(NativeReference<T> left, NativeReference<T> right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001AFDC File Offset: 0x000191DC
		public static void Copy(NativeReference<T> dst, NativeReference<T> src)
		{
			UnsafeUtility.MemCpy(dst.m_Data, src.m_Data, (long)UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001AFF5 File Offset: 0x000191F5
		public NativeReference<T>.ReadOnly AsReadOnly()
		{
			return new NativeReference<T>.ReadOnly(this.m_Data);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001B002 File Offset: 0x00019202
		public static implicit operator NativeReference<T>.ReadOnly(NativeReference<T> nativeReference)
		{
			return nativeReference.AsReadOnly();
		}

		// Token: 0x040003E7 RID: 999
		[NativeDisableUnsafePtrRestriction]
		internal unsafe void* m_Data;

		// Token: 0x040003E8 RID: 1000
		internal AllocatorManager.AllocatorHandle m_AllocatorLabel;

		// Token: 0x020000BC RID: 188
		[NativeContainer]
		[NativeContainerIsReadOnly]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public struct ReadOnly
		{
			// Token: 0x060008F8 RID: 2296 RVA: 0x0001B00B File Offset: 0x0001920B
			internal unsafe ReadOnly(void* data)
			{
				this.m_Data = data;
			}

			// Token: 0x17000106 RID: 262
			// (get) Token: 0x060008F9 RID: 2297 RVA: 0x0001B014 File Offset: 0x00019214
			public unsafe T Value
			{
				get
				{
					return *(T*)this.m_Data;
				}
			}

			// Token: 0x040003E9 RID: 1001
			[NativeDisableUnsafePtrRestriction]
			private unsafe readonly void* m_Data;
		}
	}
}
