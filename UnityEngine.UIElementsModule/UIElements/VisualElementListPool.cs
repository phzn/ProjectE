using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020003F2 RID: 1010
	internal static class VisualElementListPool
	{
		// Token: 0x060020C1 RID: 8385 RVA: 0x0007C190 File Offset: 0x0007A390
		public static List<VisualElement> Copy(List<VisualElement> elements)
		{
			List<VisualElement> list = VisualElementListPool.pool.Get();
			list.AddRange(elements);
			return list;
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x0007C1B8 File Offset: 0x0007A3B8
		public static List<VisualElement> Get(int initialCapacity = 0)
		{
			List<VisualElement> list = VisualElementListPool.pool.Get();
			bool flag = initialCapacity > 0 && list.Capacity < initialCapacity;
			if (flag)
			{
				list.Capacity = initialCapacity;
			}
			return list;
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x0007C1F4 File Offset: 0x0007A3F4
		public static void Release(List<VisualElement> elements)
		{
			elements.Clear();
			VisualElementListPool.pool.Release(elements);
		}

		// Token: 0x04000DC3 RID: 3523
		private static ObjectPool<List<VisualElement>> pool = new ObjectPool<List<VisualElement>>(() => new List<VisualElement>(), 20);
	}
}
