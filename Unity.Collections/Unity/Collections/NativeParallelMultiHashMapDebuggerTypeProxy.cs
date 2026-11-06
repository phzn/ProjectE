using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x020000B3 RID: 179
	internal sealed class NativeParallelMultiHashMapDebuggerTypeProxy<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> where TKey : struct, ValueType, IEquatable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x060008C1 RID: 2241 RVA: 0x0001AA71 File Offset: 0x00018C71
		public NativeParallelMultiHashMapDebuggerTypeProxy(NativeParallelMultiHashMap<TKey, TValue> target)
		{
			this.m_Target = target;
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0001AA80 File Offset: 0x00018C80
		public List<ListPair<TKey, List<TValue>>> Items
		{
			get
			{
				List<ListPair<TKey, List<TValue>>> list = new List<ListPair<TKey, List<TValue>>>();
				ValueTuple<NativeArray<TKey>, int> valueTuple = default(ValueTuple<NativeArray<TKey>, int>);
				using (NativeParallelHashMap<TKey, TValue> nativeParallelHashMap = new NativeParallelHashMap<TKey, TValue>(this.m_Target.Count(), Allocator.Temp))
				{
					foreach (KeyValue<TKey, TValue> keyValue in this.m_Target)
					{
						nativeParallelHashMap.TryAdd(keyValue.Key, default(TValue));
					}
					valueTuple.Item1 = nativeParallelHashMap.GetKeyArray(Allocator.Temp);
					valueTuple.Item2 = valueTuple.Item1.Length;
				}
				using (valueTuple.Item1)
				{
					for (int i = 0; i < valueTuple.Item2; i++)
					{
						List<TValue> list2 = new List<TValue>();
						TValue tvalue;
						NativeParallelMultiHashMapIterator<TKey> nativeParallelMultiHashMapIterator;
						if (this.m_Target.TryGetFirstValue(valueTuple.Item1[i], out tvalue, out nativeParallelMultiHashMapIterator))
						{
							do
							{
								list2.Add(tvalue);
							}
							while (this.m_Target.TryGetNextValue(out tvalue, ref nativeParallelMultiHashMapIterator));
						}
						list.Add(new ListPair<TKey, List<TValue>>(valueTuple.Item1[i], list2));
					}
				}
				return list;
			}
		}

		// Token: 0x040003E0 RID: 992
		private NativeParallelMultiHashMap<TKey, TValue> m_Target;
	}
}
