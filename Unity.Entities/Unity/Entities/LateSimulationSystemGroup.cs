using System;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000067 RID: 103
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
	[UpdateBefore(typeof(EndSimulationEntityCommandBufferSystem))]
	public class LateSimulationSystemGroup : ComponentSystemGroup
	{
		// Token: 0x06000284 RID: 644 RVA: 0x0000B77B File Offset: 0x0000997B
		[Preserve]
		public LateSimulationSystemGroup()
		{
		}
	}
}
