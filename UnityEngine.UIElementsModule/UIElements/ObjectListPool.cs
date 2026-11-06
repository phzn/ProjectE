using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020003F4 RID: 1012
	internal class ObjectListPool<T>
	{
		// Token: 0x060020C8 RID: 8392 RVA: 0x0007C23C File Offset: 0x0007A43C
		public static List<T> Get()
		{
			return ObjectListPool<T>.pool.Get();
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x0007C258 File Offset: 0x0007A458
		public static void Release(List<T> elements)
		{
			elements.Clear();
			ObjectListPool<T>.pool.Release(elements);
		}

		// Token: 0x04000DC5 RID: 3525
		private static ObjectPool<List<T>> pool = new ObjectPool<List<T>>(() => new List<T>(), 20);
	}
}
