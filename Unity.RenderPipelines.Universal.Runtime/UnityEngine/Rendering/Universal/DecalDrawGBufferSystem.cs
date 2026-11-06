using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000078 RID: 120
	internal class DecalDrawGBufferSystem : DecalDrawSystem
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x00018E8D File Offset: 0x0001708D
		public DecalDrawGBufferSystem(DecalEntityManager entityManager)
			: base("DecalDrawGBufferSystem.Execute", entityManager)
		{
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00018E9B File Offset: 0x0001709B
		protected override int GetPassIndex(DecalCachedChunk decalCachedChunk)
		{
			return decalCachedChunk.passIndexGBuffer;
		}
	}
}
