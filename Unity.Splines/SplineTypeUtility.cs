using System;

namespace UnityEngine.Splines
{
	// Token: 0x0200003C RID: 60
	internal static class SplineTypeUtility
	{
		// Token: 0x060002C2 RID: 706 RVA: 0x0000CDC4 File Offset: 0x0000AFC4
		internal static TangentMode GetTangentMode(this SplineType splineType)
		{
			if (splineType == SplineType.Bezier)
			{
				return TangentMode.Mirrored;
			}
			if (splineType != SplineType.Linear)
			{
				return TangentMode.AutoSmooth;
			}
			return TangentMode.Linear;
		}
	}
}
