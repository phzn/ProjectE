using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x02000041 RID: 65
	public struct LerpFloat4 : IInterpolator<float4>
	{
		// Token: 0x06000313 RID: 787 RVA: 0x0001007B File Offset: 0x0000E27B
		public float4 Interpolate(float4 a, float4 b, float t)
		{
			return math.lerp(a, b, t);
		}
	}
}
