using System;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A5F RID: 2655
	[Flags]
	public enum PlayerContextResource
	{
		// Token: 0x04004D56 RID: 19798
		None = 0,
		// Token: 0x04004D57 RID: 19799
		WorldInteraction = 1,
		// Token: 0x04004D58 RID: 19800
		PlayerControl = 2,
		// Token: 0x04004D59 RID: 19801
		Camera = 4,
		// Token: 0x04004D5A RID: 19802
		FullBodyAnimation = 8,
		// Token: 0x04004D5B RID: 19803
		ModalUI = 16,
		// Token: 0x04004D5C RID: 19804
		Rider = 32,
		// Token: 0x04004D5D RID: 19805
		HeldItem = 64,
		// Token: 0x04004D5E RID: 19806
		WorldSimulation = 128,
		// Token: 0x04004D5F RID: 19807
		BodyAttachment = 256,
		// Token: 0x04004D60 RID: 19808
		WaterMovement = 512,
		// Token: 0x04004D61 RID: 19809
		All = 1023
	}
}
