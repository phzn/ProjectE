using System;
using Pathfinding.ECS;

namespace Pathfinding
{
	// Token: 0x02000084 RID: 132
	public interface IOffMeshLinkHandler
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00003792 File Offset: 0x00001992
		string name
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000490 RID: 1168
		IOffMeshLinkStateMachine GetOffMeshLinkStateMachine(AgentOffMeshLinkTraversalContext context);
	}
}
