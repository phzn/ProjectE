using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x02000093 RID: 147
	internal sealed class NativeHashMapDebuggerTypeProxy<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x06000760 RID: 1888 RVA: 0x00017AA8 File Offset: 0x00015CA8
		public NativeHashMapDebuggerTypeProxy(NativeHashMap<TKey, TValue> target)
		{
			this.Data = target.m_Data;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00017ABC File Offset: 0x00015CBC
		public NativeHashMapDebuggerTypeProxy(NativeHashMap<TKey, TValue>.ReadOnly target)
		{
			this.Data = target.m_Data;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00017AD0 File Offset: 0x00015CD0
		public unsafe List<Pair<TKey, TValue>> Items
		{
			get
			{
				if (this.Data == null)
				{
					return null;
				}
				List<Pair<TKey, TValue>> list = new List<Pair<TKey, TValue>>();
				using (NativeKeyValueArrays<TKey, TValue> keyValueArrays = this.Data->GetKeyValueArrays<TValue>(Allocator.Temp))
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

		// Token: 0x040003BE RID: 958
		private unsafe HashMapHelper<TKey>* Data;
	}
}
