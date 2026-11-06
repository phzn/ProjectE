using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000076 RID: 118
	public interface ICollectionPropertyVisitor
	{
		// Token: 0x060001D5 RID: 469
		void Visit<TContainer, TCollection, TElement>(Property<TContainer, TCollection> property, ref TContainer container, ref TCollection collection) where TCollection : ICollection<TElement>;
	}
}
