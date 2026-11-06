using System;

namespace Pathfinding
{
	// Token: 0x0200003A RID: 58
	[Flags]
	public enum GraphUpdateThreading
	{
		// Token: 0x04000191 RID: 401
		UnityThread = 0,
		// Token: 0x04000192 RID: 402
		UnityInit = 2,
		// Token: 0x04000193 RID: 403
		UnityPost = 4
	}
}
