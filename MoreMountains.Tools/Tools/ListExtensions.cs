using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000EA RID: 234
	public static class ListExtensions
	{
		// Token: 0x060004F9 RID: 1273 RVA: 0x00016E78 File Offset: 0x00015078
		public static void MMSwap<T>(this IList<T> list, int i, int j)
		{
			T t = list[i];
			list[i] = list[j];
			list[j] = t;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00016EA4 File Offset: 0x000150A4
		public static void MMShuffle<T>(this IList<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list.MMSwap<T>(i, Random.Range(i, list.Count));
			}
		}
	}
}
