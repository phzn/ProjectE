using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000035 RID: 53
	[Flags]
	public enum ClearFlag
	{
		// Token: 0x04000135 RID: 309
		None = 0,
		// Token: 0x04000136 RID: 310
		Color = 1,
		// Token: 0x04000137 RID: 311
		Depth = 2,
		// Token: 0x04000138 RID: 312
		Stencil = 4,
		// Token: 0x04000139 RID: 313
		DepthStencil = 6,
		// Token: 0x0400013A RID: 314
		ColorStencil = 5,
		// Token: 0x0400013B RID: 315
		All = 7
	}
}
