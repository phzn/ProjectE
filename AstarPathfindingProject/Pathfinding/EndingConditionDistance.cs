using System;

namespace Pathfinding
{
	// Token: 0x02000138 RID: 312
	public class EndingConditionDistance : PathEndingCondition
	{
		// Token: 0x06000A3F RID: 2623 RVA: 0x00037219 File Offset: 0x00035419
		public EndingConditionDistance(Path p, int maxGScore)
			: base(p)
		{
			this.maxGScore = maxGScore;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00037231 File Offset: 0x00035431
		public override bool TargetFound(GraphNode node, uint H, uint G)
		{
			return G >= (uint)this.maxGScore;
		}

		// Token: 0x040006CA RID: 1738
		public int maxGScore = 100;
	}
}
