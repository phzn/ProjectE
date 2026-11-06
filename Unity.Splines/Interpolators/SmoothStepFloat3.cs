using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x02000048 RID: 72
	public struct SmoothStepFloat3 : IInterpolator<float3>
	{
		// Token: 0x0600031A RID: 794 RVA: 0x0001011F File Offset: 0x0000E31F
		public float3 Interpolate(float3 a, float3 b, float t)
		{
			return math.smoothstep(a, b, t);
		}
	}
}
