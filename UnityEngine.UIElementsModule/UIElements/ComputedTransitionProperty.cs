using System;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x020002C8 RID: 712
	internal struct ComputedTransitionProperty
	{
		// Token: 0x0400099F RID: 2463
		public StylePropertyId id;

		// Token: 0x040009A0 RID: 2464
		public int durationMs;

		// Token: 0x040009A1 RID: 2465
		public int delayMs;

		// Token: 0x040009A2 RID: 2466
		public Func<float, float> easingCurve;
	}
}
