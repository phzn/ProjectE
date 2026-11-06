using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200007A RID: 122
	internal class DecalDrawScreenSpaceSystem : DecalDrawSystem
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x00019208 File Offset: 0x00017408
		public DecalDrawScreenSpaceSystem(DecalEntityManager entityManager)
			: base("DecalDrawScreenSpaceSystem.Execute", entityManager)
		{
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00019216 File Offset: 0x00017416
		protected override int GetPassIndex(DecalCachedChunk decalCachedChunk)
		{
			return decalCachedChunk.passIndexScreenSpace;
		}
	}
}
