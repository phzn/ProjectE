using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x0200003F RID: 63
	public struct LerpFloat2 : IInterpolator<float2>
	{
		// Token: 0x06000311 RID: 785 RVA: 0x00010067 File Offset: 0x0000E267
		public float2 Interpolate(float2 a, float2 b, float t)
		{
			return math.lerp(a, b, t);
		}
	}
}
