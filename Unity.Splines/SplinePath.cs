using System;
using System.Collections.Generic;

namespace UnityEngine.Splines
{
	// Token: 0x02000035 RID: 53
	public class SplinePath : SplinePath<SplineSlice<Spline>>
	{
		// Token: 0x0600028E RID: 654 RVA: 0x0000C339 File Offset: 0x0000A539
		public SplinePath(IEnumerable<SplineSlice<Spline>> slices)
			: base(slices)
		{
		}
	}
}
