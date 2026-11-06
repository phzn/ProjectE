using System;

namespace Pathfinding.ECS
{
	// Token: 0x020002FB RID: 763
	public class ManagedMovementOverrideBeforeMovement : ManagedMovementOverride<BeforeMovementDelegate>
	{
		// Token: 0x060012F4 RID: 4852 RVA: 0x000734FE File Offset: 0x000716FE
		public object Clone()
		{
			return new ManagedMovementOverrideBeforeMovement();
		}
	}
}
