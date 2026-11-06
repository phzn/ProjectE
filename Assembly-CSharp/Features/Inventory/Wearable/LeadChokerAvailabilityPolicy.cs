using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Rank;

namespace Features.Inventory.Wearable
{
	// Token: 0x0200111D RID: 4381
	public static class LeadChokerAvailabilityPolicy
	{
		// Token: 0x060084D3 RID: 34003 RVA: 0x00362F08 File Offset: 0x00361108
		public static bool IsPresetAvailable(MCStylePresetSO preset)
		{
			if (preset == null)
			{
				return false;
			}
			if (preset.presetId != "accessory_lead_choker")
			{
				return true;
			}
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
				if (list[i] != null && string.Equals(list[i].rankId, "shy", StringComparison.Ordinal))
				{
					return RankProgressionService.RankIndex >= i;
				}
			}
			return false;
		}

		// Token: 0x04008784 RID: 34692
		public const string PartId = "Accessory.LeadChoker";

		// Token: 0x04008785 RID: 34693
		public const string ChokerPresetId = "accessory_lead_choker";

		// Token: 0x04008786 RID: 34694
		public const string RequiredRankId = "shy";
	}
}
