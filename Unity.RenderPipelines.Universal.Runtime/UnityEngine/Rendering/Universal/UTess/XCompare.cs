using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x02000138 RID: 312
	internal struct XCompare : IComparer<double>
	{
		// Token: 0x06000A15 RID: 2581 RVA: 0x00045475 File Offset: 0x00043675
		public int Compare(double a, double b)
		{
			if (a >= b)
			{
				return 1;
			}
			return -1;
		}
	}
}
