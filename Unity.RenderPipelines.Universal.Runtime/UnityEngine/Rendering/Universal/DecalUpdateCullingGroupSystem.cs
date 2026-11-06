using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000077 RID: 119
	internal class DecalUpdateCullingGroupSystem
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x00018D0C File Offset: 0x00016F0C
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x00018D16 File Offset: 0x00016F16
		public float boundingDistance
		{
			get
			{
				return this.m_BoundingDistance[0];
			}
			set
			{
				this.m_BoundingDistance[0] = value;
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00018D21 File Offset: 0x00016F21
		public DecalUpdateCullingGroupSystem(DecalEntityManager entityManager, float drawDistance)
		{
			this.m_EntityManager = entityManager;
			this.m_BoundingDistance[0] = drawDistance;
			this.m_Sampler = new ProfilingSampler("DecalUpdateCullingGroupsSystem.Execute");
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00018D58 File Offset: 0x00016F58
		public void Execute(Camera camera)
		{
			using (new ProfilingScope(null, this.m_Sampler))
			{
				this.m_Camera = camera;
				for (int i = 0; i < this.m_EntityManager.chunkCount; i++)
				{
					this.Execute(this.m_EntityManager.cachedChunks[i], this.m_EntityManager.culledChunks[i], this.m_EntityManager.culledChunks[i].count);
				}
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00018DF0 File Offset: 0x00016FF0
		public void Execute(DecalCachedChunk cachedChunk, DecalCulledChunk culledChunk, int count)
		{
			cachedChunk.currentJobHandle.Complete();
			CullingGroup cullingGroups = culledChunk.cullingGroups;
			cullingGroups.targetCamera = this.m_Camera;
			cullingGroups.SetDistanceReferencePoint(this.m_Camera.transform.position);
			cullingGroups.SetBoundingDistances(this.m_BoundingDistance);
			cachedChunk.boundingSpheres.CopyTo(cachedChunk.boundingSphereArray);
			cullingGroups.SetBoundingSpheres(cachedChunk.boundingSphereArray);
			cullingGroups.SetBoundingSphereCount(count);
			culledChunk.cameraPosition = this.m_Camera.transform.position;
			culledChunk.cullingMask = this.m_Camera.cullingMask;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00018E89 File Offset: 0x00017089
		internal static ulong GetSceneCullingMaskFromCamera(Camera camera)
		{
			return 0UL;
		}

		// Token: 0x0400033D RID: 829
		private float[] m_BoundingDistance = new float[1];

		// Token: 0x0400033E RID: 830
		private Camera m_Camera;

		// Token: 0x0400033F RID: 831
		private DecalEntityManager m_EntityManager;

		// Token: 0x04000340 RID: 832
		private ProfilingSampler m_Sampler;
	}
}
