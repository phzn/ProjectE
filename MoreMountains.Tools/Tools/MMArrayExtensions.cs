using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000E2 RID: 226
	public static class MMArrayExtensions
	{
		// Token: 0x060004E3 RID: 1251 RVA: 0x00016930 File Offset: 0x00014B30
		public static T MMRandomValue<T>(this T[] array)
		{
			int num = Random.Range(0, array.Length);
			return array[num];
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00016950 File Offset: 0x00014B50
		public static T[] MMShuffle<T>(this T[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				T t = array[i];
				int num = Random.Range(i, array.Length);
				array[i] = array[num];
				array[num] = t;
			}
			return array;
		}
	}
}
