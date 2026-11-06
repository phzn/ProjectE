using System;

namespace Pathfinding
{
	// Token: 0x0200011E RID: 286
	public interface IPathModifier
	{
		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060009BC RID: 2492
		int Order { get; }

		// Token: 0x060009BD RID: 2493
		void Apply(Path path);

		// Token: 0x060009BE RID: 2494
		void PreProcess(Path path);
	}
}
