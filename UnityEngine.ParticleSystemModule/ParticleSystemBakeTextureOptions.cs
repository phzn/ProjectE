using System;

namespace UnityEngine
{
	// Token: 0x02000057 RID: 87
	[Flags]
	public enum ParticleSystemBakeTextureOptions
	{
		// Token: 0x04000171 RID: 369
		BakeRotationAndScale = 1,
		// Token: 0x04000172 RID: 370
		BakePosition = 2,
		// Token: 0x04000173 RID: 371
		PerVertex = 4,
		// Token: 0x04000174 RID: 372
		PerParticle = 8,
		// Token: 0x04000175 RID: 373
		IncludeParticleIndices = 16,
		// Token: 0x04000176 RID: 374
		Default = 4
	}
}
