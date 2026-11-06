using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000470 RID: 1136
	internal class VectorImageRenderInfo : LinkedPoolItem<VectorImageRenderInfo>
	{
		// Token: 0x0600233E RID: 9022 RVA: 0x00089018 File Offset: 0x00087218
		public void Reset()
		{
			this.useCount = 0;
			this.firstGradientRemap = null;
			this.gradientSettingsAlloc = default(Alloc);
		}

		// Token: 0x04001061 RID: 4193
		public int useCount;

		// Token: 0x04001062 RID: 4194
		public GradientRemap firstGradientRemap;

		// Token: 0x04001063 RID: 4195
		public Alloc gradientSettingsAlloc;
	}
}
