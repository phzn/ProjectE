using System;

namespace Pathfinding
{
	// Token: 0x0200014A RID: 330
	[Obsolete("Has been renamed to DynamicObstacle")]
	public interface DynamicGridObstacle
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000AAA RID: 2730
		// (set) Token: 0x06000AAB RID: 2731
		bool enabled { get; set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000AAC RID: 2732
		// (set) Token: 0x06000AAD RID: 2733
		float updateError { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000AAE RID: 2734
		// (set) Token: 0x06000AAF RID: 2735
		float checkTime { get; set; }

		// Token: 0x06000AB0 RID: 2736
		void DoUpdateGraphs();
	}
}
