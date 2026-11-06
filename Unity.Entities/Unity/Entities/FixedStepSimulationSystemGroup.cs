using System;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x0200005D RID: 93
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.ThinClientSimulation, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
	[UpdateAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
	public class FixedStepSimulationSystemGroup : ComponentSystemGroup
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000B83B File Offset: 0x00009A3B
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000B856 File Offset: 0x00009A56
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
			set
			{
				if (base.RateManager != null)
				{
					base.RateManager.Timestep = value;
				}
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000B86C File Offset: 0x00009A6C
		[Preserve]
		[Preserve]
		public FixedStepSimulationSystemGroup()
		{
			float num = 0.016666668f;
			base.SetRateManagerCreateAllocator(new RateUtils.FixedRateCatchUpManager(num));
		}
	}
}
