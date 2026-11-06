using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200025A RID: 602
	internal class UIDocumentHierarchicalIndexComparer : IComparer<UIDocumentHierarchicalIndex>
	{
		// Token: 0x0600114A RID: 4426 RVA: 0x0003EBC8 File Offset: 0x0003CDC8
		public int Compare(UIDocumentHierarchicalIndex x, UIDocumentHierarchicalIndex y)
		{
			return x.CompareTo(y);
		}
	}
}
