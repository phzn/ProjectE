using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200002C RID: 44
	[Flags]
	public enum FontStyles
	{
		// Token: 0x0400022B RID: 555
		Normal = 0,
		// Token: 0x0400022C RID: 556
		Bold = 1,
		// Token: 0x0400022D RID: 557
		Italic = 2,
		// Token: 0x0400022E RID: 558
		Underline = 4,
		// Token: 0x0400022F RID: 559
		LowerCase = 8,
		// Token: 0x04000230 RID: 560
		UpperCase = 16,
		// Token: 0x04000231 RID: 561
		SmallCaps = 32,
		// Token: 0x04000232 RID: 562
		Strikethrough = 64,
		// Token: 0x04000233 RID: 563
		Superscript = 128,
		// Token: 0x04000234 RID: 564
		Subscript = 256,
		// Token: 0x04000235 RID: 565
		Highlight = 512
	}
}
