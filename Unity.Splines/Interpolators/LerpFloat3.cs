using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x02000040 RID: 64
	public struct LerpFloat3 : IInterpolator<float3>
	{
		// Token: 0x06000312 RID: 786 RVA: 0x00010071 File Offset: 0x0000E271
		public float3 Interpolate(float3 a, float3 b, float t)
		{
			return math.lerp(a, b, t);
		}
	}
}
