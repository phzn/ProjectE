using System;
using System.Collections;
using Pathfinding.ECS;

namespace Pathfinding
{
	// Token: 0x02000085 RID: 133
	public interface IOffMeshLinkStateMachine
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x0001704D File Offset: 0x0001524D
		IEnumerable OnTraverseOffMeshLink(AgentOffMeshLinkTraversalContext context)
		{
			return JobStartOffMeshLinkTransition.DefaultOnTraverseOffMeshLink(context);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00003788 File Offset: 0x00001988
		void OnFinishTraversingOffMeshLink(AgentOffMeshLinkTraversalContext context)
		{
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00003788 File Offset: 0x00001988
		void OnAbortTraversingOffMeshLink()
		{
		}
	}
}
