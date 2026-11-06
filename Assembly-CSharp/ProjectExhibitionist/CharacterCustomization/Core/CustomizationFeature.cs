using System;

namespace ProjectExhibitionist.CharacterCustomization.Core
{
	// Token: 0x02000AD5 RID: 2773
	[Flags]
	public enum CustomizationFeature
	{
		// Token: 0x04005003 RID: 20483
		None = 0,
		// Token: 0x04005004 RID: 20484
		BlendShapes = 1,
		// Token: 0x04005005 RID: 20485
		Colors = 2,
		// Token: 0x04005006 RID: 20486
		ClothingToggles = 4,
		// Token: 0x04005007 RID: 20487
		SkinDecals = 8,
		// Token: 0x04005008 RID: 20488
		Styles = 16,
		// Token: 0x04005009 RID: 20489
		Presets = 32
	}
}
