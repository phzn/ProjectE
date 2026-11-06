using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000033 RID: 51
	public interface IRaycastableGraph
	{
		// Token: 0x0600026A RID: 618
		bool Linecast(Vector3 start, Vector3 end);

		// Token: 0x0600026B RID: 619
		[Obsolete]
		bool Linecast(Vector3 start, Vector3 end, GraphNode startNodeHint);

		// Token: 0x0600026C RID: 620
		[Obsolete]
		bool Linecast(Vector3 start, Vector3 end, GraphNode startNodeHint, out GraphHitInfo hit);

		// Token: 0x0600026D RID: 621
		bool Linecast(Vector3 start, Vector3 end, GraphNode startNodeHint, out GraphHitInfo hit, ref TraversalConstraint traversalConstraint, List<GraphNode> trace);

		// Token: 0x0600026E RID: 622 RVA: 0x0000B5B8 File Offset: 0x000097B8
		bool Linecast(Vector3 start, Vector3 end, GraphNode startNodeHint, out GraphHitInfo hit, List<GraphNode> trace)
		{
			TraversalConstraint none = TraversalConstraint.None;
			return this.Linecast(start, end, startNodeHint, out hit, ref none, trace);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000B5DC File Offset: 0x000097DC
		bool Linecast(Vector3 start, Vector3 end, out GraphHitInfo hit)
		{
			TraversalConstraint none = TraversalConstraint.None;
			return this.Linecast(start, end, out hit, ref none, null);
		}

		// Token: 0x06000270 RID: 624
		bool Linecast(Vector3 start, Vector3 end, out GraphHitInfo hit, ref TraversalConstraint traversalConstraint, List<GraphNode> trace = null);

		// Token: 0x06000271 RID: 625 RVA: 0x0000B5FC File Offset: 0x000097FC
		bool Linecast(Vector3 start, Vector3 end, out GraphHitInfo hit, List<GraphNode> trace)
		{
			TraversalConstraint none = TraversalConstraint.None;
			return this.Linecast(start, end, out hit, ref none, trace);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000B61C File Offset: 0x0000981C
		[Obsolete("Use the overload that takes a TraversalConstraint instead of a filter function")]
		bool Linecast(Vector3 start, Vector3 end, out GraphHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter)
		{
			TraversalConstraint none = TraversalConstraint.None;
			none.filter = filter;
			return this.Linecast(start, end, out hit, ref none, trace);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000B648 File Offset: 0x00009848
		[Obsolete("Use the overload that takes a TraversalConstraint instead of a filter function")]
		bool Linecast(Vector3 start, Vector3 end, GraphNode startNodeHint, out GraphHitInfo hit, List<GraphNode> trace, Func<GraphNode, bool> filter)
		{
			TraversalConstraint none = TraversalConstraint.None;
			none.filter = filter;
			return this.Linecast(start, end, startNodeHint, out hit, ref none, trace);
		}
	}
}
