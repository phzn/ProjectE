using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200046E RID: 1134
	internal class VectorImageRenderInfoPool : LinkedPool<VectorImageRenderInfo>
	{
		// Token: 0x06002339 RID: 9017 RVA: 0x00088FA4 File Offset: 0x000871A4
		public VectorImageRenderInfoPool()
			: base(() => new VectorImageRenderInfo(), delegate(VectorImageRenderInfo vectorImageInfo)
			{
				vectorImageInfo.Reset();
			}, 10000)
		{
		}
	}
}
