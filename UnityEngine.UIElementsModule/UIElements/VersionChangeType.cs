using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200028B RID: 651
	[Flags]
	internal enum VersionChangeType
	{
		// Token: 0x04000844 RID: 2116
		Bindings = 1,
		// Token: 0x04000845 RID: 2117
		ViewData = 2,
		// Token: 0x04000846 RID: 2118
		Hierarchy = 4,
		// Token: 0x04000847 RID: 2119
		Layout = 8,
		// Token: 0x04000848 RID: 2120
		StyleSheet = 16,
		// Token: 0x04000849 RID: 2121
		Styles = 32,
		// Token: 0x0400084A RID: 2122
		Overflow = 64,
		// Token: 0x0400084B RID: 2123
		BorderRadius = 128,
		// Token: 0x0400084C RID: 2124
		BorderWidth = 256,
		// Token: 0x0400084D RID: 2125
		Transform = 512,
		// Token: 0x0400084E RID: 2126
		Size = 1024,
		// Token: 0x0400084F RID: 2127
		Repaint = 2048,
		// Token: 0x04000850 RID: 2128
		Opacity = 4096,
		// Token: 0x04000851 RID: 2129
		Color = 8192,
		// Token: 0x04000852 RID: 2130
		RenderHints = 16384,
		// Token: 0x04000853 RID: 2131
		TransitionProperty = 32768,
		// Token: 0x04000854 RID: 2132
		EventCallbackCategories = 65536,
		// Token: 0x04000855 RID: 2133
		Picking = 1048576
	}
}
