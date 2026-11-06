using System;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.ClothingLoot
{
	// Token: 0x0200134C RID: 4940
	[Serializable]
	public sealed class ClothingLootPoolEntry
	{
		// Token: 0x04009DC9 RID: 40393
		public WearableItemSO clothing;

		// Token: 0x04009DCA RID: 40394
		[Min(0f)]
		[Tooltip("Relative chance inside the mall clothing pool.")]
		public float weight = 1f;
	}
}
