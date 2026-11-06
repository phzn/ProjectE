using System;

namespace Pathfinding
{
	// Token: 0x02000041 RID: 65
	public enum PathState
	{
		// Token: 0x040001BB RID: 443
		Created,
		// Token: 0x040001BC RID: 444
		PathQueue,
		// Token: 0x040001BD RID: 445
		Processing,
		// Token: 0x040001BE RID: 446
		ReturnQueue,
		// Token: 0x040001BF RID: 447
		Returning,
		// Token: 0x040001C0 RID: 448
		Returned
	}
}
