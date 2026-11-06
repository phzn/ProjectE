using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200004C RID: 76
	public enum RendererType
	{
		// Token: 0x040001EA RID: 490
		Custom,
		// Token: 0x040001EB RID: 491
		UniversalRenderer,
		// Token: 0x040001EC RID: 492
		_2DRenderer,
		// Token: 0x040001ED RID: 493
		[Obsolete("ForwardRenderer has been renamed (UnityUpgradable) -> UniversalRenderer", true)]
		ForwardRenderer = 1
	}
}
