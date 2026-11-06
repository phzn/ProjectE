using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000471 RID: 1137
	internal class GradientRemapPool : LinkedPool<GradientRemap>
	{
		// Token: 0x06002340 RID: 9024 RVA: 0x00089040 File Offset: 0x00087240
		public GradientRemapPool()
			: base(() => new GradientRemap(), delegate(GradientRemap gradientRemap)
			{
				gradientRemap.Reset();
			}, 10000)
		{
		}
	}
}
