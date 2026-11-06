using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000072 RID: 114
	internal class DecalSkipCulledSystem
	{
		// Token: 0x0600042A RID: 1066 RVA: 0x000184F8 File Offset: 0x000166F8
		public DecalSkipCulledSystem(DecalEntityManager entityManager)
		{
			this.m_EntityManager = entityManager;
			this.m_Sampler = new ProfilingSampler("DecalSkipCulledSystem.Execute");
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00018518 File Offset: 0x00016718
		public void Execute(Camera camera)
		{
			using (new ProfilingScope(null, this.m_Sampler))
			{
				this.m_Camera = camera;
				for (int i = 0; i < this.m_EntityManager.chunkCount; i++)
				{
					this.Execute(this.m_EntityManager.culledChunks[i], this.m_EntityManager.culledChunks[i].count);
				}
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x000185A0 File Offset: 0x000167A0
		private void Execute(DecalCulledChunk culledChunk, int count)
		{
			if (count == 0)
			{
				return;
			}
			culledChunk.currentJobHandle.Complete();
			for (int i = 0; i < count; i++)
			{
				culledChunk.visibleDecalIndices[i] = i;
			}
			culledChunk.visibleDecalCount = count;
			culledChunk.cameraPosition = this.m_Camera.transform.position;
			culledChunk.cullingMask = this.m_Camera.cullingMask;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00018606 File Offset: 0x00016806
		internal static ulong GetSceneCullingMaskFromCamera(Camera camera)
		{
			return 0UL;
		}

		// Token: 0x04000316 RID: 790
		private DecalEntityManager m_EntityManager;

		// Token: 0x04000317 RID: 791
		private ProfilingSampler m_Sampler;

		// Token: 0x04000318 RID: 792
		private Camera m_Camera;
	}
}
