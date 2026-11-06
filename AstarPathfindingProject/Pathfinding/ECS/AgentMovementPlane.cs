using System;
using Pathfinding.Util;
using Unity.Entities;
using Unity.Mathematics;

namespace Pathfinding.ECS
{
	// Token: 0x020002E5 RID: 741
	[Serializable]
	public struct AgentMovementPlane : IComponentData, IQueryTypeParameter
	{
		// Token: 0x060012B2 RID: 4786 RVA: 0x00072BDA File Offset: 0x00070DDA
		public AgentMovementPlane(quaternion rotation)
		{
			this.value = new NativeMovementPlane(rotation);
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00072BE8 File Offset: 0x00070DE8
		public AgentMovementPlane(NativeMovementPlane plane)
		{
			this.value = plane;
		}

		// Token: 0x04000DA1 RID: 3489
		public NativeMovementPlane value;
	}
}
