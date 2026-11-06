using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.VFX
{
	// Token: 0x0200001F RID: 31
	[NativeHeader("Modules/VFX/Public/Systems/VFXParticleSystem.h")]
	[UsedByNativeCode]
	public struct VFXParticleSystemInfo
	{
		// Token: 0x06000156 RID: 342 RVA: 0x00003424 File Offset: 0x00001624
		public VFXParticleSystemInfo(uint aliveCount, uint capacity, bool sleeping, Bounds bounds)
		{
			this.aliveCount = aliveCount;
			this.capacity = capacity;
			this.sleeping = sleeping;
			this.bounds = bounds;
		}

		// Token: 0x04000128 RID: 296
		public uint aliveCount;

		// Token: 0x04000129 RID: 297
		public uint capacity;

		// Token: 0x0400012A RID: 298
		public bool sleeping;

		// Token: 0x0400012B RID: 299
		public Bounds bounds;
	}
}
