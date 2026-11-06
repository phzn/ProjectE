using System;

namespace System.Linq
{
	// Token: 0x020000E0 RID: 224
	internal abstract class CachingComparer<TElement>
	{
		// Token: 0x060007F6 RID: 2038
		internal abstract int Compare(TElement element, bool cacheLower);

		// Token: 0x060007F7 RID: 2039
		internal abstract void SetElement(TElement element);
	}
}
