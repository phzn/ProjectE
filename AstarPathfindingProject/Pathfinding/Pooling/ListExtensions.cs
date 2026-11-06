using System;
using System.Collections.Generic;

namespace Pathfinding.Pooling
{
	// Token: 0x02000255 RID: 597
	public static class ListExtensions
	{
		// Token: 0x06000EE2 RID: 3810 RVA: 0x0005BEE0 File Offset: 0x0005A0E0
		public static T[] ToArrayFromPool<T>(this List<T> list)
		{
			T[] array = ArrayPool<T>.ClaimWithExactLength(list.Count);
			list.CopyTo(array);
			return array;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0005BF01 File Offset: 0x0005A101
		public static void ClearFast<T>(this List<T> list)
		{
			if (list.Count * 2 < list.Capacity)
			{
				list.RemoveRange(0, list.Count);
				return;
			}
			list.Clear();
		}
	}
}
