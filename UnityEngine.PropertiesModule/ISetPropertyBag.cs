using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000046 RID: 70
	public interface ISetPropertyBag<TSet, TElement> : ICollectionPropertyBag<TSet, TElement>, IPropertyBag<TSet>, IPropertyBag, ICollectionPropertyBagAccept<TSet>, ISetPropertyBagAccept<TSet>, ISetPropertyAccept<TSet>, IKeyedProperties<TSet, object> where TSet : ISet<TElement>
	{
	}
}
