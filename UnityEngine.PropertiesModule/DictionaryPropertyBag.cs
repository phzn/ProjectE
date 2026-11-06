using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000037 RID: 55
	public class DictionaryPropertyBag<TKey, TValue> : KeyValueCollectionPropertyBag<Dictionary<TKey, TValue>, TKey, TValue>
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000052B1 File Offset: 0x000034B1
		protected override InstantiationKind InstantiationKind
		{
			get
			{
				return InstantiationKind.PropertyBagOverride;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005369 File Offset: 0x00003569
		protected override Dictionary<TKey, TValue> Instantiate()
		{
			return new Dictionary<TKey, TValue>();
		}
	}
}
