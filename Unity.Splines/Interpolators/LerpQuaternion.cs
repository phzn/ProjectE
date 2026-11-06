using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x02000044 RID: 68
	public struct LerpQuaternion : IInterpolator<quaternion>
	{
		// Token: 0x06000316 RID: 790 RVA: 0x000100F2 File Offset: 0x0000E2F2
		public quaternion Interpolate(quaternion a, quaternion b, float t)
		{
			return math.nlerp(a, b, t);
		}
	}
}
