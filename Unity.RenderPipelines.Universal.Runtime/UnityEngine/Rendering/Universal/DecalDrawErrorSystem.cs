using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000064 RID: 100
	internal class DecalDrawErrorSystem : DecalDrawSystem
	{
		// Token: 0x060003B2 RID: 946 RVA: 0x00015FA8 File Offset: 0x000141A8
		public DecalDrawErrorSystem(DecalEntityManager entityManager, DecalTechnique technique)
			: base("DecalDrawErrorSystem.Execute", entityManager)
		{
			this.m_Technique = technique;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00015FC0 File Offset: 0x000141C0
		protected override int GetPassIndex(DecalCachedChunk decalCachedChunk)
		{
			switch (this.m_Technique)
			{
			case DecalTechnique.Invalid:
				return 0;
			case DecalTechnique.DBuffer:
				if (decalCachedChunk.passIndexDBuffer != -1 || decalCachedChunk.passIndexEmissive != -1)
				{
					return -1;
				}
				return 0;
			case DecalTechnique.ScreenSpace:
				if (decalCachedChunk.passIndexScreenSpace != -1)
				{
					return -1;
				}
				return 0;
			case DecalTechnique.GBuffer:
				if (decalCachedChunk.passIndexGBuffer != -1)
				{
					return -1;
				}
				return 0;
			default:
				return 0;
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001601F File Offset: 0x0001421F
		protected override Material GetMaterial(DecalEntityChunk decalEntityChunk)
		{
			return this.m_EntityManager.errorMaterial;
		}

		// Token: 0x040002BE RID: 702
		private DecalTechnique m_Technique;
	}
}
