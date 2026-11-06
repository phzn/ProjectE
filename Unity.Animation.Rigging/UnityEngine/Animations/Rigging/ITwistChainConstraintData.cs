using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000037 RID: 55
	public interface ITwistChainConstraintData
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000104 RID: 260
		Transform root { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000105 RID: 261
		Transform tip { get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000106 RID: 262
		Transform rootTarget { get; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000107 RID: 263
		Transform tipTarget { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000108 RID: 264
		AnimationCurve curve { get; }
	}
}
