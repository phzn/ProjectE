using System;
using Features.Rank;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B0D RID: 2829
	public static class MainCharacterAppearanceEvents
	{
		// Token: 0x1400008A RID: 138
		// (add) Token: 0x0600500D RID: 20493 RVA: 0x001EF73C File Offset: 0x001ED93C
		// (remove) Token: 0x0600500E RID: 20494 RVA: 0x001EF770 File Offset: 0x001ED970
		public static event Action InUsePresetChanged;

		// Token: 0x0600500F RID: 20495 RVA: 0x001EF7A3 File Offset: 0x001ED9A3
		public static void RaiseInUsePresetChanged()
		{
			RankProgressionService.RecordCustomOutfitChoice();
			Action inUsePresetChanged = MainCharacterAppearanceEvents.InUsePresetChanged;
			if (inUsePresetChanged == null)
			{
				return;
			}
			inUsePresetChanged();
		}
	}
}
