using System;
using Bozo.AnimeCharacters;

namespace ProjectExhibitionist.CharacterCustomization.Core.Presets
{
	// Token: 0x02000B1D RID: 2845
	[Serializable]
	public class SystemPresetEntry
	{
		// Token: 0x0400513B RID: 20795
		public CharacterObject sourceAsset;

		// Token: 0x0400513C RID: 20796
		public string presetName;

		// Token: 0x0400513D RID: 20797
		public string characterId;

		// Token: 0x0400513E RID: 20798
		public CustomizationTargetType targetType;

		// Token: 0x0400513F RID: 20799
		public bool isInactive;
	}
}
