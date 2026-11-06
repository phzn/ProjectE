using System;

namespace Pathfinding
{
	// Token: 0x02000141 RID: 321
	public class ABPathEndingCondition : PathEndingCondition
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x0003860B File Offset: 0x0003680B
		public ABPathEndingCondition(ABPath p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			this.abPath = p;
			this.path = p;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0003862F File Offset: 0x0003682F
		public override bool TargetFound(GraphNode node, uint H, uint G)
		{
			return node == this.abPath.endNode;
		}

		// Token: 0x040006EC RID: 1772
		protected ABPath abPath;
	}
}
