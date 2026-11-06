using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x0200003E RID: 62
	public struct LerpFloat : IInterpolator<float>
	{
		// Token: 0x06000310 RID: 784 RVA: 0x0001005D File Offset: 0x0000E25D
		public float Interpolate(float a, float b, float t)
		{
			return math.lerp(a, b, t);
		}
	}
}
