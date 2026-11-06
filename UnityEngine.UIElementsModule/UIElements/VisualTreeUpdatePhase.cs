using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000419 RID: 1049
	internal enum VisualTreeUpdatePhase
	{
		// Token: 0x04000E3A RID: 3642
		ViewData,
		// Token: 0x04000E3B RID: 3643
		Bindings,
		// Token: 0x04000E3C RID: 3644
		Animation,
		// Token: 0x04000E3D RID: 3645
		Styles,
		// Token: 0x04000E3E RID: 3646
		Layout,
		// Token: 0x04000E3F RID: 3647
		TransformClip,
		// Token: 0x04000E40 RID: 3648
		Repaint,
		// Token: 0x04000E41 RID: 3649
		Count
	}
}
