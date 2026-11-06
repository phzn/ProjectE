using System;
using System.Collections.Generic;

namespace UnityEngine.Splines
{
	// Token: 0x0200000B RID: 11
	internal class DataPointComparer<T> : IComparer<T> where T : IDataPoint
	{
		// Token: 0x06000042 RID: 66 RVA: 0x000033CC File Offset: 0x000015CC
		public int Compare(T x, T y)
		{
			return x.Index.CompareTo(y.Index);
		}
	}
}
