using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000051 RID: 81
	public static class DictionaryPool<TKey, TValue>
	{
		// Token: 0x060002A6 RID: 678 RVA: 0x0000C4FE File Offset: 0x0000A6FE
		public static Dictionary<TKey, TValue> Get()
		{
			return DictionaryPool<TKey, TValue>.s_Pool.Get();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000C50A File Offset: 0x0000A70A
		public static ObjectPool<Dictionary<TKey, TValue>>.PooledObject Get(out Dictionary<TKey, TValue> value)
		{
			return DictionaryPool<TKey, TValue>.s_Pool.Get(out value);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000C517 File Offset: 0x0000A717
		public static void Release(Dictionary<TKey, TValue> toRelease)
		{
			DictionaryPool<TKey, TValue>.s_Pool.Release(toRelease);
		}

		// Token: 0x04000199 RID: 409
		private static readonly ObjectPool<Dictionary<TKey, TValue>> s_Pool = new ObjectPool<Dictionary<TKey, TValue>>(null, delegate(Dictionary<TKey, TValue> l)
		{
			l.Clear();
		}, true);
	}
}
