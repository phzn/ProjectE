using System;

namespace Pathfinding.ECS
{
	// Token: 0x020002F9 RID: 761
	public class ManagedMovementOverrideBeforeControl : ManagedMovementOverride<BeforeControlDelegate>, ICloneable
	{
		// Token: 0x060012F0 RID: 4848 RVA: 0x000734E0 File Offset: 0x000716E0
		public object Clone()
		{
			return new ManagedMovementOverrideBeforeControl();
		}
	}
}
