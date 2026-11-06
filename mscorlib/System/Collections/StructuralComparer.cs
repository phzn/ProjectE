using System;
using System.Collections.Generic;

namespace System.Collections
{
	// Token: 0x02000A3A RID: 2618
	[Serializable]
	internal sealed class StructuralComparer : IComparer
	{
		// Token: 0x06005CFF RID: 23807 RVA: 0x00138DD4 File Offset: 0x00136FD4
		public int Compare(object x, object y)
		{
			if (x == null)
			{
				if (y != null)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (y == null)
				{
					return 1;
				}
				IStructuralComparable structuralComparable = x as IStructuralComparable;
				if (structuralComparable != null)
				{
					return structuralComparable.CompareTo(y, this);
				}
				return Comparer<object>.Default.Compare(x, y);
			}
		}
	}
}
