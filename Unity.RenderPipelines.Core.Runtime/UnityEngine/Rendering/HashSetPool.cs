using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000050 RID: 80
	public static class HashSetPool<T>
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x0000C4BA File Offset: 0x0000A6BA
		public static HashSet<T> Get()
		{
			return HashSetPool<T>.s_Pool.Get();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000C4C6 File Offset: 0x0000A6C6
		public static ObjectPool<HashSet<T>>.PooledObject Get(out HashSet<T> value)
		{
			return HashSetPool<T>.s_Pool.Get(out value);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000C4D3 File Offset: 0x0000A6D3
		public static void Release(HashSet<T> toRelease)
		{
			HashSetPool<T>.s_Pool.Release(toRelease);
		}

		// Token: 0x04000198 RID: 408
		private static readonly ObjectPool<HashSet<T>> s_Pool = new ObjectPool<HashSet<T>>(null, delegate(HashSet<T> l)
		{
			l.Clear();
		}, true);
	}
}
