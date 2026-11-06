using System;
using System.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

namespace Pathfinding.ECS
{
	// Token: 0x020002EA RID: 746
	public class ManagedAgentOffMeshLinkTraversal : IComponentData, IQueryTypeParameter, ICloneable, ICleanupComponentData
	{
		// Token: 0x060012B7 RID: 4791 RVA: 0x0000378A File Offset: 0x0000198A
		[Preserve]
		public ManagedAgentOffMeshLinkTraversal()
		{
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00072C31 File Offset: 0x00070E31
		public ManagedAgentOffMeshLinkTraversal(AgentOffMeshLinkTraversalContext context, IOffMeshLinkHandler handler)
		{
			this.context = context;
			this.handler = handler;
			this.coroutine = null;
			this.stateMachine = null;
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00072C55 File Offset: 0x00070E55
		public object Clone()
		{
			return new ManagedAgentOffMeshLinkTraversal((AgentOffMeshLinkTraversalContext)this.context.Clone(), this.handler);
		}

		// Token: 0x04000DA6 RID: 3494
		public AgentOffMeshLinkTraversalContext context;

		// Token: 0x04000DA7 RID: 3495
		public IEnumerator coroutine;

		// Token: 0x04000DA8 RID: 3496
		public IOffMeshLinkHandler handler;

		// Token: 0x04000DA9 RID: 3497
		public IOffMeshLinkStateMachine stateMachine;
	}
}
