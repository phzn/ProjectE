using System;
using Features.Inventory.Wearable;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007D4 RID: 2004
	public readonly struct WardrobeDiscoveryResult
	{
		// Token: 0x0600352A RID: 13610 RVA: 0x0012EC4E File Offset: 0x0012CE4E
		public WardrobeDiscoveryResult(bool isDiscovered, WearableItemSO wearable)
		{
			this.IsDiscovered = isDiscovered;
			this.Wearable = wearable;
		}

		// Token: 0x04003572 RID: 13682
		public readonly bool IsDiscovered;

		// Token: 0x04003573 RID: 13683
		public readonly WearableItemSO Wearable;
	}
}
