using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x0200004A RID: 74
	public struct SlerpQuaternion : IInterpolator<quaternion>
	{
		// Token: 0x0600031C RID: 796 RVA: 0x0001013D File Offset: 0x0000E33D
		public quaternion Interpolate(quaternion a, quaternion b, float t)
		{
			return math.slerp(a, b, t);
		}
	}
}
