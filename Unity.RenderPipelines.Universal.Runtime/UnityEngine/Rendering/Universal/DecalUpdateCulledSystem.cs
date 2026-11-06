using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000075 RID: 117
	internal class DecalUpdateCulledSystem
	{
		// Token: 0x06000435 RID: 1077 RVA: 0x00018B64 File Offset: 0x00016D64
		public DecalUpdateCulledSystem(DecalEntityManager entityManager)
		{
			this.m_EntityManager = entityManager;
			this.m_Sampler = new ProfilingSampler("DecalUpdateCulledSystem.Execute");
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00018B84 File Offset: 0x00016D84
		public void Execute()
		{
			using (new ProfilingScope(null, this.m_Sampler))
			{
				for (int i = 0; i < this.m_EntityManager.chunkCount; i++)
				{
					this.Execute(this.m_EntityManager.culledChunks[i], this.m_EntityManager.culledChunks[i].count);
				}
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00018C04 File Offset: 0x00016E04
		private void Execute(DecalCulledChunk culledChunk, int count)
		{
			if (count == 0)
			{
				return;
			}
			culledChunk.currentJobHandle.Complete();
			CullingGroup cullingGroups = culledChunk.cullingGroups;
			culledChunk.visibleDecalCount = cullingGroups.QueryIndices(true, culledChunk.visibleDecalIndexArray, 0);
			culledChunk.visibleDecalIndices.CopyFrom(culledChunk.visibleDecalIndexArray);
		}

		// Token: 0x04000334 RID: 820
		private DecalEntityManager m_EntityManager;

		// Token: 0x04000335 RID: 821
		private ProfilingSampler m_Sampler;
	}
}
