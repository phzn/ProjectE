using System;
using Features.Inventory;
using UnityEngine;

namespace Features.TrashcanLoot
{
	// Token: 0x02000BE4 RID: 3044
	public class LootTuningController : MonoBehaviour
	{
		// Token: 0x040057DF RID: 22495
		[Header("Trashcan profiles (assets)")]
		public TrashcanLootProfileSO commonProfile;

		// Token: 0x040057E0 RID: 22496
		public TrashcanLootProfileSO rareProfile;

		// Token: 0x040057E1 RID: 22497
		public TrashcanLootProfileSO legendaryProfile;

		// Token: 0x040057E2 RID: 22498
		[Header("Trashcan tier spawn config (map randomizer)")]
		public TrashcanTierSpawnConfigSO tierSpawnConfig;

		// Token: 0x040057E3 RID: 22499
		[Header("Global rarity settings (instance roll 80/17/3 + durations)")]
		public RaritySettingsSO raritySettings;

		// Token: 0x040057E4 RID: 22500
		[Header("Loot pool (weighted table shared by the tiers)")]
		public LootTableSO lootTable;

		// Token: 0x040057E5 RID: 22501
		[Header("Debug")]
		[Tooltip("Extra logs from the tuning simulations.")]
		public bool debugLogs;
	}
}
