using System;

namespace UnityEngine
{
	// Token: 0x02000058 RID: 88
	[Obsolete("ParticleSystemVertexStreams is deprecated. Please use ParticleSystemVertexStream instead.", false)]
	[Flags]
	public enum ParticleSystemVertexStreams
	{
		// Token: 0x04000178 RID: 376
		Position = 1,
		// Token: 0x04000179 RID: 377
		Normal = 2,
		// Token: 0x0400017A RID: 378
		Tangent = 4,
		// Token: 0x0400017B RID: 379
		Color = 8,
		// Token: 0x0400017C RID: 380
		UV = 16,
		// Token: 0x0400017D RID: 381
		UV2BlendAndFrame = 32,
		// Token: 0x0400017E RID: 382
		CenterAndVertexID = 64,
		// Token: 0x0400017F RID: 383
		Size = 128,
		// Token: 0x04000180 RID: 384
		Rotation = 256,
		// Token: 0x04000181 RID: 385
		Velocity = 512,
		// Token: 0x04000182 RID: 386
		Lifetime = 1024,
		// Token: 0x04000183 RID: 387
		Custom1 = 2048,
		// Token: 0x04000184 RID: 388
		Custom2 = 4096,
		// Token: 0x04000185 RID: 389
		Random = 8192,
		// Token: 0x04000186 RID: 390
		None = 0,
		// Token: 0x04000187 RID: 391
		All = 2147483647
	}
}
