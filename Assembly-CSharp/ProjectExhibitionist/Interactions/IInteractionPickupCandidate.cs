using System;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x0200090F RID: 2319
	public interface IInteractionPickupCandidate : IInteractionCandidate
	{
		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003ED1 RID: 16081
		bool CanPrioritizePickup { get; }
	}
}
