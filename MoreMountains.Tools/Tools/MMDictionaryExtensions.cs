using System;
using System.Collections.Generic;

namespace MoreMountains.Tools
{
	// Token: 0x020000E6 RID: 230
	public static class MMDictionaryExtensions
	{
		// Token: 0x060004EE RID: 1262 RVA: 0x00016C3C File Offset: 0x00014E3C
		public static T KeyByValue<T, W>(this Dictionary<T, W> dictionary, T value)
		{
			T t = default(T);
			foreach (KeyValuePair<T, W> keyValuePair in dictionary)
			{
				W value2 = keyValuePair.Value;
				if (value2.Equals(value))
				{
					t = keyValuePair.Key;
					break;
				}
			}
			return t;
		}
	}
}
