using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x0200004F RID: 79
	public static class ListPool<T>
	{
		// Token: 0x0600029E RID: 670 RVA: 0x0000C476 File Offset: 0x0000A676
		public static List<T> Get()
		{
			return ListPool<T>.s_Pool.Get();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000C482 File Offset: 0x0000A682
		public static ObjectPool<List<T>>.PooledObject Get(out List<T> value)
		{
			return ListPool<T>.s_Pool.Get(out value);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000C48F File Offset: 0x0000A68F
		public static void Release(List<T> toRelease)
		{
			ListPool<T>.s_Pool.Release(toRelease);
		}

		// Token: 0x04000197 RID: 407
		private static readonly ObjectPool<List<T>> s_Pool = new ObjectPool<List<T>>(null, delegate(List<T> l)
		{
			l.Clear();
		}, true);
	}
}
