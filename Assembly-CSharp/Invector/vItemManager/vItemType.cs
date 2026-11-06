using System;
using System.ComponentModel;

namespace Invector.vItemManager
{
	// Token: 0x0200056D RID: 1389
	public enum vItemType
	{
		// Token: 0x040021C8 RID: 8648
		[Description("")]
		Consumable,
		// Token: 0x040021C9 RID: 8649
		[Description("Melee")]
		MeleeWeapon,
		// Token: 0x040021CA RID: 8650
		[Description("Shooter")]
		ShooterWeapon,
		// Token: 0x040021CB RID: 8651
		[Description("(VALUE)")]
		Ammo,
		// Token: 0x040021CC RID: 8652
		[Description("")]
		Archery,
		// Token: 0x040021CD RID: 8653
		[Description("")]
		Builder,
		// Token: 0x040021CE RID: 8654
		[Description("")]
		Defense,
		// Token: 0x040021CF RID: 8655
		[Description("")]
		CraftingMaterials
	}
}
