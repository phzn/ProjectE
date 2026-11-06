using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000217 RID: 535
	internal interface IPointerEventInternal
	{
		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000F91 RID: 3985
		// (set) Token: 0x06000F92 RID: 3986
		bool triggeredByOS { get; set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000F93 RID: 3987
		// (set) Token: 0x06000F94 RID: 3988
		bool recomputeTopElementUnderPointer { get; set; }
	}
}
