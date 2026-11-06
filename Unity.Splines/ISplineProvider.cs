using System;
using System.Collections.Generic;

namespace UnityEngine.Splines
{
	// Token: 0x02000014 RID: 20
	[Obsolete("Use ISplineContainer instead.")]
	public interface ISplineProvider
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000074 RID: 116
		IEnumerable<Spline> Splines { get; }
	}
}
