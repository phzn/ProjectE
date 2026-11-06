using System;

namespace Pathfinding.ECS
{
	// Token: 0x020002EB RID: 747
	public struct MovementTarget
	{
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060012BA RID: 4794 RVA: 0x00072C72 File Offset: 0x00070E72
		public bool reached
		{
			get
			{
				return this.isReached;
			}
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00072C7A File Offset: 0x00070E7A
		public MovementTarget(bool isReached)
		{
			this.isReached = isReached;
		}

		// Token: 0x04000DAA RID: 3498
		internal bool isReached;
	}
}
