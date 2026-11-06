using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x02000043 RID: 67
	public struct SlerpFloat3 : IInterpolator<float3>
	{
		// Token: 0x06000315 RID: 789 RVA: 0x000100D9 File Offset: 0x0000E2D9
		public float3 Interpolate(float3 a, float3 b, float t)
		{
			return Vector3.Slerp(a, b, t);
		}
	}
}
