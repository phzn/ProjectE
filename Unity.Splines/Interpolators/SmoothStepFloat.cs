using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x02000046 RID: 70
	public struct SmoothStepFloat : IInterpolator<float>
	{
		// Token: 0x06000318 RID: 792 RVA: 0x00010106 File Offset: 0x0000E306
		public float Interpolate(float a, float b, float t)
		{
			return math.smoothstep(a, b, t);
		}
	}
}
