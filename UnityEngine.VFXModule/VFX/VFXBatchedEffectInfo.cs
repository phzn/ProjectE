using System;
using UnityEngine.Scripting;

namespace UnityEngine.VFX
{
	// Token: 0x02000013 RID: 19
	[RequiredByNativeCode]
	public struct VFXBatchedEffectInfo
	{
		// Token: 0x0400010A RID: 266
		public VisualEffectAsset vfxAsset;

		// Token: 0x0400010B RID: 267
		public uint activeBatchCount;

		// Token: 0x0400010C RID: 268
		public uint inactiveBatchCount;

		// Token: 0x0400010D RID: 269
		public uint activeInstanceCount;

		// Token: 0x0400010E RID: 270
		public uint unbatchedInstanceCount;

		// Token: 0x0400010F RID: 271
		public uint totalInstanceCapacity;

		// Token: 0x04000110 RID: 272
		public uint maxInstancePerBatchCapacity;

		// Token: 0x04000111 RID: 273
		public ulong totalGPUSizeInBytes;

		// Token: 0x04000112 RID: 274
		public ulong totalCPUSizeInBytes;
	}
}
