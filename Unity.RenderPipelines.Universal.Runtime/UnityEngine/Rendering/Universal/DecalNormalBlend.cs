using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000C1 RID: 193
	internal enum DecalNormalBlend
	{
		// Token: 0x040004E0 RID: 1248
		[Tooltip("Low quality of normal reconstruction (Uses 1 sample).")]
		Low,
		// Token: 0x040004E1 RID: 1249
		[Tooltip("Medium quality of normal reconstruction (Uses 5 samples).")]
		Medium,
		// Token: 0x040004E2 RID: 1250
		[Tooltip("High quality of normal reconstruction (Uses 9 samples).")]
		High
	}
}
