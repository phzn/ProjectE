using System;

namespace Unity.Properties
{
	// Token: 0x02000035 RID: 53
	public sealed class ArrayPropertyBag<TElement> : IndexedCollectionPropertyBag<TElement[], TElement>
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000052B1 File Offset: 0x000034B1
		protected override InstantiationKind InstantiationKind
		{
			get
			{
				return InstantiationKind.PropertyBagOverride;
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000052B4 File Offset: 0x000034B4
		protected override TElement[] InstantiateWithCount(int count)
		{
			return new TElement[count];
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000052BC File Offset: 0x000034BC
		protected override TElement[] Instantiate()
		{
			return Array.Empty<TElement>();
		}
	}
}
