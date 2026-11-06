using System;

namespace CompassNavigatorPro
{
	// Token: 0x02000282 RID: 642
	public static class CompassStyleExtensions
	{
		// Token: 0x060010BA RID: 4282 RVA: 0x0006F9A6 File Offset: 0x0006DBA6
		public static bool HasDegreesOrTicks(this CompassStyle style)
		{
			return style >= CompassStyle.CleanWithIntegratedDegrees && style <= CompassStyle.CleanWithIntegratedDegreesAndTicks4;
		}
	}
}
