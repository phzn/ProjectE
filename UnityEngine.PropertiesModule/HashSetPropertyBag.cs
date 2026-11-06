using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000038 RID: 56
	public class HashSetPropertyBag<TElement> : SetPropertyBagBase<HashSet<TElement>, TElement>
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000052B1 File Offset: 0x000034B1
		protected override InstantiationKind InstantiationKind
		{
			get
			{
				return InstantiationKind.PropertyBagOverride;
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005379 File Offset: 0x00003579
		protected override HashSet<TElement> Instantiate()
		{
			return new HashSet<TElement>();
		}
	}
}
