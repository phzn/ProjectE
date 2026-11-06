using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001CE RID: 462
	public enum PropagationPhase
	{
		// Token: 0x040006A6 RID: 1702
		None,
		// Token: 0x040006A7 RID: 1703
		TrickleDown,
		// Token: 0x040006A8 RID: 1704
		AtTarget,
		// Token: 0x040006A9 RID: 1705
		DefaultActionAtTarget = 5,
		// Token: 0x040006AA RID: 1706
		BubbleUp = 3,
		// Token: 0x040006AB RID: 1707
		DefaultAction
	}
}
