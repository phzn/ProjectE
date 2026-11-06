using System;
using DA_Assets.FCU.Attributes;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000058 RID: 88
	public enum PaintType
	{
		// Token: 0x040003B4 RID: 948
		NONE,
		// Token: 0x040003B5 RID: 949
		[PaintPriority(1)]
		SOLID,
		// Token: 0x040003B6 RID: 950
		[PaintPriority(2)]
		GRADIENT_LINEAR,
		// Token: 0x040003B7 RID: 951
		[PaintPriority(4)]
		GRADIENT_RADIAL,
		// Token: 0x040003B8 RID: 952
		[PaintPriority(3)]
		GRADIENT_ANGULAR,
		// Token: 0x040003B9 RID: 953
		[PaintPriority(5)]
		GRADIENT_DIAMOND,
		// Token: 0x040003BA RID: 954
		IMAGE,
		// Token: 0x040003BB RID: 955
		EMOJI,
		// Token: 0x040003BC RID: 956
		VIDEO
	}
}
