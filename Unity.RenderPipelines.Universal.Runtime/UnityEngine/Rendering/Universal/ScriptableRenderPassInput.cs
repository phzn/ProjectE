using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000B4 RID: 180
	[Flags]
	public enum ScriptableRenderPassInput
	{
		// Token: 0x0400049B RID: 1179
		None = 0,
		// Token: 0x0400049C RID: 1180
		Depth = 1,
		// Token: 0x0400049D RID: 1181
		Normal = 2,
		// Token: 0x0400049E RID: 1182
		Color = 4,
		// Token: 0x0400049F RID: 1183
		Motion = 8
	}
}
