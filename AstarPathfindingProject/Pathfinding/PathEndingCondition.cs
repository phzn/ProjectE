using System;

namespace Pathfinding
{
	// Token: 0x02000140 RID: 320
	public abstract class PathEndingCondition
	{
		// Token: 0x06000A7F RID: 2687 RVA: 0x0000378A File Offset: 0x0000198A
		protected PathEndingCondition()
		{
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x000385EE File Offset: 0x000367EE
		public PathEndingCondition(Path p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			this.path = p;
		}

		// Token: 0x06000A81 RID: 2689
		public abstract bool TargetFound(GraphNode node, uint H, uint G);

		// Token: 0x040006EB RID: 1771
		protected Path path;
	}
}
