using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200012E RID: 302
	internal sealed class UnsafeParallelHashMapDebuggerTypeProxy<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x06000C9C RID: 3228 RVA: 0x00025D5A File Offset: 0x00023F5A
		public UnsafeParallelHashMapDebuggerTypeProxy(UnsafeParallelHashMap<TKey, TValue> target)
		{
			this.m_Target = target;
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x00025D6C File Offset: 0x00023F6C
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

		// Token: 0x04000509 RID: 1289
		private UnsafeParallelHashMap<TKey, TValue> m_Target;
	}
}
