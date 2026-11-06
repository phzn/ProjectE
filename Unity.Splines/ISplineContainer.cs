using System;
using System.Collections.Generic;

namespace UnityEngine.Splines
{
	// Token: 0x02000013 RID: 19
	public interface ISplineContainer
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000071 RID: 113
		// (set) Token: 0x06000072 RID: 114
		IReadOnlyList<Spline> Splines { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000073 RID: 115
		KnotLinkCollection KnotLinkCollection { get; }
	}
}
