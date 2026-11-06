using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000060 RID: 96
	internal class DecalDrawDBufferSystem : DecalDrawSystem
	{
		// Token: 0x0600039F RID: 927 RVA: 0x00015967 File Offset: 0x00013B67
		public DecalDrawDBufferSystem(DecalEntityManager entityManager)
			: base("DecalDrawIntoDBufferSystem.Execute", entityManager)
		{
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00015975 File Offset: 0x00013B75
		protected override int GetPassIndex(DecalCachedChunk decalCachedChunk)
		{
			return decalCachedChunk.passIndexDBuffer;
		}
	}
}
