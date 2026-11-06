using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements.Collections
{
	// Token: 0x020004D4 RID: 1236
	internal static class DictionaryExtensions
	{
		// Token: 0x060026C8 RID: 9928 RVA: 0x000A328C File Offset: 0x000A148C
		public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue fallbackValue = default(TValue))
		{
			TValue tvalue;
			return dict.TryGetValue(key, out tvalue) ? tvalue : fallbackValue;
		}
	}
}
