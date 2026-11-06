using System;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x0200000C RID: 12
	public enum HalfedgeState : byte
	{
		// Token: 0x04000033 RID: 51
		Unconstrained,
		// Token: 0x04000034 RID: 52
		Constrained,
		// Token: 0x04000035 RID: 53
		ConstrainedAndHoleBoundary
	}
}
