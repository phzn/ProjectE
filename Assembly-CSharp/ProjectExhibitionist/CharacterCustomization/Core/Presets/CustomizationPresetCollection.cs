using System;
using System.Collections.Generic;

namespace ProjectExhibitionist.CharacterCustomization.Core.Presets
{
	// Token: 0x02000B17 RID: 2839
	[Serializable]
	public class CustomizationPresetCollection
	{
		// Token: 0x0400511A RID: 20762
		public int schemaVersion = 1;

		// Token: 0x0400511B RID: 20763
		public List<CustomizationPresetData> presets = new List<CustomizationPresetData>();
	}
}
