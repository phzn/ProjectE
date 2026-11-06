using System;

namespace ProjectExhibitionist.CharacterCustomization.Core
{
	// Token: 0x02000AD8 RID: 2776
	public interface IClothingCustomizationBackend
	{
		// Token: 0x06004EAF RID: 20143
		bool TrySetClothingGroupState(string groupId, bool enabled);

		// Token: 0x06004EB0 RID: 20144
		bool TryGetClothingGroupState(string groupId, out bool enabled);
	}
}
