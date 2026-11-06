using System;

namespace UnityEngine
{
	// Token: 0x0200004F RID: 79
	[Flags]
	public enum ParticleSystemSubEmitterProperties
	{
		// Token: 0x0400014A RID: 330
		InheritNothing = 0,
		// Token: 0x0400014B RID: 331
		InheritEverything = 31,
		// Token: 0x0400014C RID: 332
		InheritColor = 1,
		// Token: 0x0400014D RID: 333
		InheritSize = 2,
		// Token: 0x0400014E RID: 334
		InheritRotation = 4,
		// Token: 0x0400014F RID: 335
		InheritLifetime = 8,
		// Token: 0x04000150 RID: 336
		InheritDuration = 16
	}
}
