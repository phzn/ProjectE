using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x0200004F RID: 79
	public class ListPropertyBag<TElement> : IndexedCollectionPropertyBag<List<TElement>, TElement>
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000052B1 File Offset: 0x000034B1
		protected override InstantiationKind InstantiationKind
		{
			get
			{
				return InstantiationKind.PropertyBagOverride;
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005B63 File Offset: 0x00003D63
		protected override List<TElement> InstantiateWithCount(int count)
		{
			return new List<TElement>(count);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005B6B File Offset: 0x00003D6B
		protected override List<TElement> Instantiate()
		{
			return new List<TElement>();
		}
	}
}
