using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory.Wearable;

namespace Features.Rank
{
	// Token: 0x02000D9C RID: 3484
	public static class HalloweenWardrobePolicy
	{
		// Token: 0x0600669A RID: 26266 RVA: 0x0029016C File Offset: 0x0028E36C
		public static bool IsHalloweenGarmentId(string id)
		{
			return id == "cloth_halloween_mask" || id == "cloth_halloween_corset" || id == "cloth_halloween_collar" || id == "cloth_halloween_breast_pasties" || id == "cloth_halloween_ass_pasties";
		}

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x0600669B RID: 26267 RVA: 0x002901BC File Offset: 0x0028E3BC
		public static bool IsRankUnlocked
		{
			get
			{
				if (RankProgressionService.IsAdminRankActive)
				{
					return true;
				}
				RankProgressionConfigSO config = RankProgressionService.Config;
				List<RankDefinitionSO> list = ((config != null) ? config.ranksInOrder : null);
				if (list == null)
				{
					return false;
				}
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != null && string.Equals(list[i].rankId, "hornyslut_declared", StringComparison.Ordinal))
					{
						return RankProgressionService.RankIndex >= i;
					}
				}
				return false;
			}
		}

		// Token: 0x0600669C RID: 26268 RVA: 0x0029022F File Offset: 0x0028E42F
		public static bool IsItemAvailable(WearableItemSO item)
		{
			return item != null && (!HalloweenWardrobePolicy.IsHalloweenGarmentId(item.itemId) || HalloweenWardrobePolicy.IsRankUnlocked);
		}

		// Token: 0x0600669D RID: 26269 RVA: 0x00290250 File Offset: 0x0028E450
		public static bool IsPresetAvailable(MCStylePresetSO preset)
		{
			return preset != null && (!HalloweenWardrobePolicy.IsHalloweenGarmentId(preset.presetId) || HalloweenWardrobePolicy.IsRankUnlocked);
		}

		// Token: 0x0600669E RID: 26270 RVA: 0x00290271 File Offset: 0x0028E471
		public static bool IsGranted(WearableItemSO item)
		{
			return item != null && !item.isVaulted && HalloweenWardrobePolicy.IsGranted(item.mcStylePreset) && HalloweenWardrobePolicy.IsHalloweenGarmentId(item.itemId);
		}

		// Token: 0x0600669F RID: 26271 RVA: 0x0029029E File Offset: 0x0028E49E
		public static bool IsGranted(MCStylePresetSO preset)
		{
			return preset != null && !preset.isVaulted && !preset.gameplayOnly && HalloweenWardrobePolicy.IsHalloweenGarmentId(preset.presetId) && HalloweenWardrobePolicy.IsRankUnlocked;
		}

		// Token: 0x04006915 RID: 26901
		public const string RequiredRankId = "hornyslut_declared";
	}
}
