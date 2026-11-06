using System;

namespace Features.Inventory
{
	// Token: 0x020010F1 RID: 4337
	public interface IInventoryPlacementHeightProvider
	{
		// Token: 0x170014D2 RID: 5330
		// (get) Token: 0x0600834B RID: 33611
		bool UseSurfaceHitHeight { get; }

		// Token: 0x170014D3 RID: 5331
		// (get) Token: 0x0600834C RID: 33612
		float MinimumHeightAboveFloor { get; }

		// Token: 0x170014D4 RID: 5332
		// (get) Token: 0x0600834D RID: 33613
		float MaximumHeightAboveFloor { get; }
	}
}
