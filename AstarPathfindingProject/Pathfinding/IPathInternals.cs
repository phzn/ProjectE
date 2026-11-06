using System;
using System.Text;

namespace Pathfinding
{
	// Token: 0x020000BB RID: 187
	internal interface IPathInternals
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000642 RID: 1602
		// (set) Token: 0x06000643 RID: 1603
		bool Pooled { get; set; }

		// Token: 0x06000644 RID: 1604
		void AdvanceState(PathState s);

		// Token: 0x06000645 RID: 1605
		void OnEnterPool();

		// Token: 0x06000646 RID: 1606
		void Reset();

		// Token: 0x06000647 RID: 1607
		void ReturnPath();

		// Token: 0x06000648 RID: 1608
		void PrepareBase(PathHandler handler);

		// Token: 0x06000649 RID: 1609
		void Prepare(ref Path.SearchContext ctx);

		// Token: 0x0600064A RID: 1610
		void Cleanup(ref Path.SearchContext ctx);

		// Token: 0x0600064B RID: 1611
		void CalculateStep(ref Path.SearchContext ctx, long targetTick);

		// Token: 0x0600064C RID: 1612
		void DebugString(StringBuilder builder, PathLog logMode);
	}
}
