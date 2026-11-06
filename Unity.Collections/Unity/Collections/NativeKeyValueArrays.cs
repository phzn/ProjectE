using System;
using System.Runtime.CompilerServices;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000A0 RID: 160
	[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
	{
		typeof(int),
		typeof(int)
	})]
	public struct NativeKeyValueArrays<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> : INativeDisposable, IDisposable where TKey : struct, ValueType where TValue : struct, ValueType
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x000184CA File Offset: 0x000166CA
		public int Length
		{
			get
			{
				return this.Keys.Length;
			}
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x000184D7 File Offset: 0x000166D7
		public NativeKeyValueArrays(int length, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options)
		{
			this.Keys = CollectionHelper.CreateNativeArray<TKey>(length, allocator, options);
			this.Values = CollectionHelper.CreateNativeArray<TValue>(length, allocator, options);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000184F5 File Offset: 0x000166F5
		public void Dispose()
		{
			this.Keys.Dispose();
			this.Values.Dispose();
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001850D File Offset: 0x0001670D
		public JobHandle Dispose(JobHandle inputDeps)
		{
			return this.Keys.Dispose(this.Values.Dispose(inputDeps));
		}

		// Token: 0x040003C8 RID: 968
		public NativeArray<TKey> Keys;

		// Token: 0x040003C9 RID: 969
		public NativeArray<TValue> Values;
	}
}
