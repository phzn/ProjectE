using System;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A5E RID: 2654
	[Flags]
	public enum PlayerActivity
	{
		// Token: 0x04004D46 RID: 19782
		None = 0,
		// Token: 0x04004D47 RID: 19783
		Emote = 1,
		// Token: 0x04004D48 RID: 19784
		Cover = 2,
		// Token: 0x04004D49 RID: 19785
		Flash = 4,
		// Token: 0x04004D4A RID: 19786
		RapidHUD = 8,
		// Token: 0x04004D4B RID: 19787
		Interact = 16,
		// Token: 0x04004D4C RID: 19788
		StanceSwitch = 32,
		// Token: 0x04004D4D RID: 19789
		Undress = 64,
		// Token: 0x04004D4E RID: 19790
		LingerieOpen = 128,
		// Token: 0x04004D4F RID: 19791
		Inventory = 256,
		// Token: 0x04004D50 RID: 19792
		Phone = 512,
		// Token: 0x04004D51 RID: 19793
		DebugFly = 1024,
		// Token: 0x04004D52 RID: 19794
		Crouch = 2048,
		// Token: 0x04004D53 RID: 19795
		ArmsPose = 4096,
		// Token: 0x04004D54 RID: 19796
		AllPlayerActions = 8191
	}
}
