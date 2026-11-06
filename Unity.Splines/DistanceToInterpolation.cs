using System;

namespace UnityEngine.Splines
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public struct DistanceToInterpolation
	{
		// Token: 0x0400002D RID: 45
		public float Distance;

		// Token: 0x0400002E RID: 46
		public float T;

		// Token: 0x0400002F RID: 47
		internal static readonly DistanceToInterpolation Invalid = new DistanceToInterpolation
		{
			Distance = -1f,
			T = -1f
		};
	}
}
