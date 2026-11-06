using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000077 RID: 119
	public interface IListPropertyVisitor
	{
		// Token: 0x060001D6 RID: 470
		void Visit<TContainer, TList, TElement>(Property<TContainer, TList> property, ref TContainer container, ref TList list) where TList : IList<TElement>;
	}
}
