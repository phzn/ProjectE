using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x02000044 RID: 68
	internal static class TMP_ListPool<T>
	{
		// Token: 0x0600032E RID: 814 RVA: 0x00022B55 File Offset: 0x00020D55
		public static List<T> Get()
		{
			return TMP_ListPool<T>.s_ListPool.Get();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00022B61 File Offset: 0x00020D61
		public static void Release(List<T> toRelease)
		{
			TMP_ListPool<T>.s_ListPool.Release(toRelease);
		}

		// Token: 0x04000273 RID: 627
		private static readonly TMP_ObjectPool<List<T>> s_ListPool = new TMP_ObjectPool<List<T>>(null, delegate(List<T> l)
		{
			l.Clear();
		});
	}
}
