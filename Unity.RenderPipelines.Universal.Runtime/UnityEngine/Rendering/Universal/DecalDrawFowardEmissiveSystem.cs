using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000062 RID: 98
	internal class DecalDrawFowardEmissiveSystem : DecalDrawSystem
	{
		// Token: 0x060003AE RID: 942 RVA: 0x00015E8F File Offset: 0x0001408F
		public DecalDrawFowardEmissiveSystem(DecalEntityManager entityManager)
			: base("DecalDrawFowardEmissiveSystem.Execute", entityManager)
		{
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00015E9D File Offset: 0x0001409D
		protected override int GetPassIndex(DecalCachedChunk decalCachedChunk)
		{
			return decalCachedChunk.passIndexEmissive;
		}
	}
}
