using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000079 RID: 121
	public interface IDictionaryPropertyVisitor
	{
		// Token: 0x060001D8 RID: 472
		void Visit<TContainer, TDictionary, TKey, TValue>(Property<TContainer, TDictionary> property, ref TContainer container, ref TDictionary dictionary) where TDictionary : IDictionary<TKey, TValue>;
	}
}
