using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000074 RID: 116
	public interface IDictionaryPropertyBagVisitor
	{
		// Token: 0x060001D3 RID: 467
		void Visit<TDictionary, TKey, TValue>(IDictionaryPropertyBag<TDictionary, TKey, TValue> properties, ref TDictionary container) where TDictionary : IDictionary<TKey, TValue>;
	}
}
