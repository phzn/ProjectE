using System;
using Unity.Mathematics;

namespace UnityEngine.Splines.Interpolators
{
	// Token: 0x02000042 RID: 66
	public struct SlerpFloat2 : IInterpolator<float2>
	{
		// Token: 0x06000314 RID: 788 RVA: 0x00010088 File Offset: 0x0000E288
		public float2 Interpolate(float2 a, float2 b, float t)
		{
			Vector3 vector = Vector3.Slerp(new Vector3(a.x, a.y, 0f), new Vector3(b.x, b.y, 0f), t);
			return new float2(vector.x, vector.y);
		}
	}
}
