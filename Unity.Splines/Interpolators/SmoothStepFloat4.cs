using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x02000049 RID: 73
	public struct SmoothStepFloat4 : IInterpolator<float4>
	{
		// Token: 0x0600031B RID: 795 RVA: 0x0001012E File Offset: 0x0000E32E
		public float4 Interpolate(float4 a, float4 b, float t)
		{
			return math.smoothstep(a, b, t);
		}
	}
}
