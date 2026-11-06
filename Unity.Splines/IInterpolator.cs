using System;

namespace UnityEngine.Splines
{
	// Token: 0x02000020 RID: 32
	public interface IInterpolator<T>
	{
		// Token: 0x06000182 RID: 386
		T Interpolate(T from, T to, float t);
	}
}
