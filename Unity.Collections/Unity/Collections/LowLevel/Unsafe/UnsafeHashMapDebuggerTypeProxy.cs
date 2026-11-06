using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200010E RID: 270
	internal sealed class UnsafeHashMapDebuggerTypeProxy<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x06000B93 RID: 2963 RVA: 0x00023281 File Offset: 0x00021481
		public UnsafeHashMapDebuggerTypeProxy(UnsafeHashMap<TKey, TValue> target)
		{
			this.Data = target.m_Data;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00023295 File Offset: 0x00021495
		public UnsafeHashMapDebuggerTypeProxy(UnsafeHashMap<TKey, TValue>.ReadOnly target)
		{
			this.Data = target.m_Data;
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x000232AC File Offset: 0x000214AC
		public List<Pair<TKey, TValue>> Items
		{
			get
			{
				List<Pair<TKey, TValue>> list = new List<Pair<TKey, TValue>>();
				using (NativeKeyValueArrays<TKey, TValue> keyValueArrays = this.Data.GetKeyValueArrays<TValue>(Allocator.Temp))
				{
					for (int i = 0; i < keyValueArrays.Length; i++)
					{
						List<Pair<TKey, TValue>> list2 = list;
						NativeArray<TKey> keys = keyValueArrays.Keys;
						TKey tkey = keys[i];
						NativeArray<TValue> values = keyValueArrays.Values;
						list2.Add(new Pair<TKey, TValue>(tkey, values[i]));
					}
				}
				return list;
			}
		}

		// Token: 0x040004C1 RID: 1217
		private HashMapHelper<TKey> Data;
	}
}
