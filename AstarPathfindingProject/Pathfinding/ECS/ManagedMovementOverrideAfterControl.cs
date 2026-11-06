using System;

namespace Pathfinding.ECS
{
	// Token: 0x020002FA RID: 762
	public class ManagedMovementOverrideAfterControl : ManagedMovementOverride<AfterControlDelegate>
	{
		// Token: 0x060012F2 RID: 4850 RVA: 0x000734EF File Offset: 0x000716EF
		public object Clone()
		{
			return new ManagedMovementOverrideAfterControl();
		}
	}
}
