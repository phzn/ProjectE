using System;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000062 RID: 98
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
	[UpdateAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
	public class VariableRateSimulationSystemGroup : ComponentSystemGroup
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000B83B File Offset: 0x00009A3B
		public float Timestep
		{
			get
			{
				if (base.RateManager == null)
				{
					return 0f;
				}
				return base.RateManager.Timestep;
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000B941 File Offset: 0x00009B41
		[Preserve]
		[Preserve]
		public VariableRateSimulationSystemGroup()
		{
			base.SetRateManagerCreateAllocator(new RateUtils.VariableRateManager(66U, true));
		}
	}
}
