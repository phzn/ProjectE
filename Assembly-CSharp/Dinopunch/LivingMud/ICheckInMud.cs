using System;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200074E RID: 1870
	public interface ICheckInMud
	{
		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003076 RID: 12406
		bool InMud { get; }

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003077 RID: 12407
		DynamicMudSurface Surface { get; }
	}
}
