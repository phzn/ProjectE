using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000072 RID: 114
	public interface IListPropertyBagVisitor
	{
		// Token: 0x060001D1 RID: 465
		void Visit<TList, TElement>(IListPropertyBag<TList, TElement> properties, ref TList container) where TList : IList<TElement>;
	}
}
