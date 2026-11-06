using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000045 RID: 69
	public interface IListPropertyBag<TList, TElement> : ICollectionPropertyBag<TList, TElement>, IPropertyBag<TList>, IPropertyBag, ICollectionPropertyBagAccept<TList>, IListPropertyBagAccept<TList>, IListPropertyAccept<TList>, IIndexedProperties<TList> where TList : IList<TElement>
	{
	}
}
