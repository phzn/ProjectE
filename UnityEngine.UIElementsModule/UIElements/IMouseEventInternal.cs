using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001E2 RID: 482
	internal interface IMouseEventInternal
	{
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000E79 RID: 3705
		// (set) Token: 0x06000E7A RID: 3706
		bool triggeredByOS { get; set; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000E7B RID: 3707
		// (set) Token: 0x06000E7C RID: 3708
		bool recomputeTopElementUnderMouse { get; set; }

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000E7D RID: 3709
		// (set) Token: 0x06000E7E RID: 3710
		IPointerEvent sourcePointerEvent { get; set; }
	}
}
