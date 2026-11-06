using System;

namespace UnityEngine.VFX
{
	// Token: 0x02000008 RID: 8
	internal enum VFXSystemFlag
	{
		// Token: 0x040000D4 RID: 212
		SystemDefault,
		// Token: 0x040000D5 RID: 213
		SystemHasKill,
		// Token: 0x040000D6 RID: 214
		SystemHasIndirectBuffer,
		// Token: 0x040000D7 RID: 215
		SystemReceivedEventGPU = 4,
		// Token: 0x040000D8 RID: 216
		SystemHasStrips = 8,
		// Token: 0x040000D9 RID: 217
		SystemNeedsComputeBounds = 16,
		// Token: 0x040000DA RID: 218
		SystemAutomaticBounds = 32,
		// Token: 0x040000DB RID: 219
		SystemInWorldSpace = 64,
		// Token: 0x040000DC RID: 220
		SystemHasDirectLink = 128,
		// Token: 0x040000DD RID: 221
		SystemHasAttributeBuffer = 256,
		// Token: 0x040000DE RID: 222
		SystemUsesInstancedRendering = 512
	}
}
