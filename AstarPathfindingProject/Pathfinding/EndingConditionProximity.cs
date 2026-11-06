using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000142 RID: 322
	public class EndingConditionProximity : ABPathEndingCondition
	{
		// Token: 0x06000A84 RID: 2692 RVA: 0x0003863F File Offset: 0x0003683F
		public EndingConditionProximity(ABPath p, float maxDistance)
			: base(p)
		{
			this.maxDistance = maxDistance;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0003865C File Offset: 0x0003685C
		public override bool TargetFound(GraphNode node, uint H, uint G)
		{
			return ((Vector3)node.position - this.abPath.originalEndPoint).sqrMagnitude <= this.maxDistance * this.maxDistance;
		}

		// Token: 0x040006ED RID: 1773
		public float maxDistance = 10f;
	}
}
