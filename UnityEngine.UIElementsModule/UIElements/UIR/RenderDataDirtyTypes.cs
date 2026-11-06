using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200045A RID: 1114
	[Flags]
	internal enum RenderDataDirtyTypes
	{
		// Token: 0x04000FD9 RID: 4057
		None = 0,
		// Token: 0x04000FDA RID: 4058
		Transform = 1,
		// Token: 0x04000FDB RID: 4059
		ClipRectSize = 2,
		// Token: 0x04000FDC RID: 4060
		Clipping = 4,
		// Token: 0x04000FDD RID: 4061
		ClippingHierarchy = 8,
		// Token: 0x04000FDE RID: 4062
		Visuals = 16,
		// Token: 0x04000FDF RID: 4063
		VisualsHierarchy = 32,
		// Token: 0x04000FE0 RID: 4064
		VisualsOpacityId = 64,
		// Token: 0x04000FE1 RID: 4065
		Opacity = 128,
		// Token: 0x04000FE2 RID: 4066
		OpacityHierarchy = 256,
		// Token: 0x04000FE3 RID: 4067
		Color = 512,
		// Token: 0x04000FE4 RID: 4068
		AllVisuals = 112
	}
}
