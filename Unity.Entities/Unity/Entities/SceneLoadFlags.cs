using System;

namespace Unity.Entities
{
	// Token: 0x0200031B RID: 795
	[Flags]
	public enum SceneLoadFlags
	{
		// Token: 0x040007BE RID: 1982
		DisableAutoLoad = 1,
		// Token: 0x040007BF RID: 1983
		BlockOnImport = 2,
		// Token: 0x040007C0 RID: 1984
		BlockOnStreamIn = 4,
		// Token: 0x040007C1 RID: 1985
		LoadAdditive = 8,
		// Token: 0x040007C2 RID: 1986
		NewInstance = 16
	}
}
