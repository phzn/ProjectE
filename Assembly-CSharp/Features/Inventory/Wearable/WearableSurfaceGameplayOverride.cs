using System;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x02001127 RID: 4391
	[Serializable]
	public class WearableSurfaceGameplayOverride
	{
		// Token: 0x040087F3 RID: 34803
		[Tooltip("Stable MCSurfaceVariantSO.variantId to override.")]
		public string surfaceVariantId;

		// Token: 0x040087F4 RID: 34804
		public bool overrideDetectionCoverage;

		// Token: 0x040087F5 RID: 34805
		public float detectionCoverage;

		// Token: 0x040087F6 RID: 34806
		public bool overrideShowInGameplayInventory;

		// Token: 0x040087F7 RID: 34807
		public bool showInGameplayInventory;
	}
}
