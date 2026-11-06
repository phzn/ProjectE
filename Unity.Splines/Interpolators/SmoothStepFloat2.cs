using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x02000047 RID: 71
	public struct SmoothStepFloat2 : IInterpolator<float2>
	{
		// Token: 0x06000319 RID: 793 RVA: 0x00010110 File Offset: 0x0000E310
		public float2 Interpolate(float2 a, float2 b, float t)
		{
			return math.smoothstep(a, b, t);
		}
	}
}
