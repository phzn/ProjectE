using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200013B RID: 315
	internal sealed class UnsafeParallelMultiHashMapDebuggerTypeProxy<[global::System.Runtime.CompilerServices.IsUnmanaged] TKey, [global::System.Runtime.CompilerServices.IsUnmanaged] TValue> where TKey : struct, ValueType, IEquatable<TKey>, IComparable<TKey> where TValue : struct, ValueType
	{
		// Token: 0x06000D79 RID: 3449 RVA: 0x000294F1 File Offset: 0x000276F1
		public UnsafeParallelMultiHashMapDebuggerTypeProxy(UnsafeParallelMultiHashMap<TKey, TValue> target)
		{
			this.m_Target = target;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00029500 File Offset: 0x00027700
		public static ValueTuple<NativeArray<TKey>, int> GetUniqueKeyArray(ref UnsafeParallelMultiHashMap<TKey, TValue> hashMap, AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<TKey> keyArray = hashMap.GetKeyArray(allocator);
			keyArray.Sort<TKey>();
			int num = keyArray.Unique<TKey>();
			return new ValueTuple<NativeArray<TKey>, int>(keyArray, num);
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00029528 File Offset: 0x00027728
		public List<ListPair<TKey, List<TValue>>> Items
		{
			get
			{
				List<ListPair<TKey, List<TValue>>> list = new List<ListPair<TKey, List<TValue>>>();
				ValueTuple<NativeArray<TKey>, int> uniqueKeyArray = UnsafeParallelMultiHashMapDebuggerTypeProxy<TKey, TValue>.GetUniqueKeyArray(ref this.m_Target, Allocator.Temp);
				using (uniqueKeyArray.Item1)
				{
					for (int i = 0; i < uniqueKeyArray.Item2; i++)
					{
						List<TValue> list2 = new List<TValue>();
						TValue tvalue;
						NativeParallelMultiHashMapIterator<TKey> nativeParallelMultiHashMapIterator;
						if (this.m_Target.TryGetFirstValue(uniqueKeyArray.Item1[i], out tvalue, out nativeParallelMultiHashMapIterator))
						{
							do
							{
								list2.Add(tvalue);
							}
							while (this.m_Target.TryGetNextValue(out tvalue, ref nativeParallelMultiHashMapIterator));
						}
						list.Add(new ListPair<TKey, List<TValue>>(uniqueKeyArray.Item1[i], list2));
					}
				}
				return list;
			}
		}

		// Token: 0x0400051C RID: 1308
		private UnsafeParallelMultiHashMap<TKey, TValue> m_Target;
	}
}
