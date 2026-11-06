using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000A5 RID: 165
	internal sealed class NativeParallelHashMapDebuggerTypeProxy<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x06000812 RID: 2066 RVA: 0x00018902 File Offset: 0x00016B02
		public NativeParallelHashMapDebuggerTypeProxy(NativeParallelHashMap<TKey, TValue> target)
		{
			this.m_Target = target.m_HashMapData;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00018916 File Offset: 0x00016B16
		internal NativeParallelHashMapDebuggerTypeProxy(NativeParallelHashMap<TKey, TValue>.ReadOnly target)
		{
			this.m_Target = target.m_HashMapData;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0001892C File Offset: 0x00016B2C
		public List<Pair<TKey, TValue>> Items
		{
			get
			{
				List<Pair<TKey, TValue>> list = new List<Pair<TKey, TValue>>();
				using (NativeKeyValueArrays<TKey, TValue> keyValueArrays = this.m_Target.GetKeyValueArrays(Allocator.Temp))
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

		// Token: 0x040003CE RID: 974
		private UnsafeParallelHashMap<TKey, TValue> m_Target;
	}
}
