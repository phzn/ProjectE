using System;

namespace MoreMountains.Tools
{
	// Token: 0x02000016 RID: 22
	[Flags]
	public enum TriggerAndCollisionMask
	{
		// Token: 0x0400006E RID: 110
		IgnoreAll = 0,
		// Token: 0x0400006F RID: 111
		OnTriggerEnter = 1,
		// Token: 0x04000070 RID: 112
		OnTriggerStay = 2,
		// Token: 0x04000071 RID: 113
		OnTriggerExit = 4,
		// Token: 0x04000072 RID: 114
		OnCollisionEnter = 8,
		// Token: 0x04000073 RID: 115
		OnCollisionStay = 16,
		// Token: 0x04000074 RID: 116
		OnCollisionExit = 32,
		// Token: 0x04000075 RID: 117
		OnTriggerEnter2D = 64,
		// Token: 0x04000076 RID: 118
		OnTriggerStay2D = 128,
		// Token: 0x04000077 RID: 119
		OnTriggerExit2D = 256,
		// Token: 0x04000078 RID: 120
		OnCollisionEnter2D = 512,
		// Token: 0x04000079 RID: 121
		OnCollisionStay2D = 1024,
		// Token: 0x0400007A RID: 122
		OnCollisionExit2D = 2048,
		// Token: 0x0400007B RID: 123
		OnAnyTrigger3D = 7,
		// Token: 0x0400007C RID: 124
		OnAnyCollision3D = 56,
		// Token: 0x0400007D RID: 125
		OnAnyTrigger2D = 448,
		// Token: 0x0400007E RID: 126
		OnAnyCollision2D = 3584,
		// Token: 0x0400007F RID: 127
		OnAnyTrigger = 455,
		// Token: 0x04000080 RID: 128
		OnAnyCollision = 3640,
		// Token: 0x04000081 RID: 129
		All_3D = 63,
		// Token: 0x04000082 RID: 130
		All_2D = 4032,
		// Token: 0x04000083 RID: 131
		All = 4095
	}
}
