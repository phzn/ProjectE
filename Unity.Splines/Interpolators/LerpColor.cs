using System;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x02000045 RID: 69
	public struct LerpColor : IInterpolator<Color>
	{
		// Token: 0x06000317 RID: 791 RVA: 0x000100FC File Offset: 0x0000E2FC
		public Color Interpolate(Color a, Color b, float t)
		{
			return Color.Lerp(a, b, t);
		}
	}
}
