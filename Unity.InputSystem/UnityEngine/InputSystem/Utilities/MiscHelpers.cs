using System;
using System.Collections.Generic;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000139 RID: 313
	internal static class MiscHelpers
	{
		// Token: 0x0600116F RID: 4463 RVA: 0x00053AF8 File Offset: 0x00051CF8
		public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
		{
			TValue tvalue;
			if (!dictionary.TryGetValue(key, out tvalue))
			{
				return default(TValue);
			}
			return tvalue;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00053B1B File Offset: 0x00051D1B
		public static IEnumerable<TValue> EveryNth<TValue>(this IEnumerable<TValue> enumerable, int n, int start = 0)
		{
			int index = 0;
			foreach (TValue tvalue in enumerable)
			{
				if (index < start)
				{
					int num = index + 1;
					index = num;
				}
				else
				{
					if ((index - start) % n == 0)
					{
						yield return tvalue;
					}
					int num = index + 1;
					index = num;
				}
			}
			IEnumerator<TValue> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00053B3C File Offset: 0x00051D3C
		public static int IndexOf<TValue>(this IEnumerable<TValue> enumerable, TValue value)
		{
			int num = 0;
			foreach (TValue tvalue in enumerable)
			{
				if (EqualityComparer<TValue>.Default.Equals(tvalue, value))
				{
					return num;
				}
				num++;
			}
			return -1;
		}
	}
}
