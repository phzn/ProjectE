using System;

namespace Pathfinding
{
	// Token: 0x0200003E RID: 62
	[Flags]
	public enum GraphUpdateDebugMode
	{
		// Token: 0x040001AA RID: 426
		Nothing = 0,
		// Token: 0x040001AB RID: 427
		VisualizeOriginalBounds = 1,
		// Token: 0x040001AC RID: 428
		VisualizeAffectedBounds = 2,
		// Token: 0x040001AD RID: 429
		VisualizeOverTime = 4
	}
}
