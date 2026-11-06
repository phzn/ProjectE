using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000EB RID: 235
	[Flags]
	public enum LightLayerEnum
	{
		// Token: 0x040005D9 RID: 1497
		Nothing = 0,
		// Token: 0x040005DA RID: 1498
		LightLayerDefault = 1,
		// Token: 0x040005DB RID: 1499
		LightLayer1 = 2,
		// Token: 0x040005DC RID: 1500
		LightLayer2 = 4,
		// Token: 0x040005DD RID: 1501
		LightLayer3 = 8,
		// Token: 0x040005DE RID: 1502
		LightLayer4 = 16,
		// Token: 0x040005DF RID: 1503
		LightLayer5 = 32,
		// Token: 0x040005E0 RID: 1504
		LightLayer6 = 64,
		// Token: 0x040005E1 RID: 1505
		LightLayer7 = 128,
		// Token: 0x040005E2 RID: 1506
		Everything = 255
	}
}
