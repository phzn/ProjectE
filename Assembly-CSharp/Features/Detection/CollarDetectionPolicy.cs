using System;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001250 RID: 4688
	public static class CollarDetectionPolicy
	{
		// Token: 0x060091D3 RID: 37331 RVA: 0x003BC20C File Offset: 0x003BA40C
		public static float GetAttentionGainMultiplier(MCStyleCustomizationService styles)
		{
			if (styles == null)
			{
				return 1f;
			}
			foreach (string text in CollarAvailabilityPolicy.ItemIds)
			{
				string partId = CollarAvailabilityPolicy.GetPartId(text);
				string text2;
				if (styles.TryGetEquippedItem(partId, out text2) && string.Equals(text2, CollarAvailabilityPolicy.GetPresetId(text), StringComparison.Ordinal))
				{
					GameObject spawnedRoot = styles.GetSpawnedRoot(partId);
					if (spawnedRoot != null && spawnedRoot.activeInHierarchy)
					{
						return 1.15f;
					}
				}
			}
			return 1f;
		}

		// Token: 0x04009626 RID: 38438
		public const string PartId = "Accessory.Collar";

		// Token: 0x04009627 RID: 38439
		public const string PresetId = "collar";

		// Token: 0x04009628 RID: 38440
		public const float WornAttentionGainMultiplier = 1.15f;
	}
}
