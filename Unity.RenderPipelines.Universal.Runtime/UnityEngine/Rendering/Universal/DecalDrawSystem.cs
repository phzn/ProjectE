using System;
using Unity.Collections;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200006D RID: 109
	internal abstract class DecalDrawSystem
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00016D4B File Offset: 0x00014F4B
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x00016D53 File Offset: 0x00014F53
		public Material overrideMaterial { get; set; }

		// Token: 0x06000406 RID: 1030 RVA: 0x00016D5C File Offset: 0x00014F5C
		public DecalDrawSystem(string sampler, DecalEntityManager entityManager)
		{
			this.m_EntityManager = entityManager;
			this.m_WorldToDecals = new Matrix4x4[DecalDrawSystem.MaxBatchSize];
			this.m_NormalToDecals = new Matrix4x4[DecalDrawSystem.MaxBatchSize];
			this.m_DecalLayerMasks = new float[DecalDrawSystem.MaxBatchSize];
			this.m_Sampler = new ProfilingSampler(sampler);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00016DB4 File Offset: 0x00014FB4
		public void Execute(CommandBuffer cmd)
		{
			using (new ProfilingScope(cmd, this.m_Sampler))
			{
				for (int i = 0; i < this.m_EntityManager.chunkCount; i++)
				{
					this.Execute(cmd, this.m_EntityManager.entityChunks[i], this.m_EntityManager.cachedChunks[i], this.m_EntityManager.drawCallChunks[i], this.m_EntityManager.entityChunks[i].count);
				}
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00016E58 File Offset: 0x00015058
		protected virtual Material GetMaterial(DecalEntityChunk decalEntityChunk)
		{
			return decalEntityChunk.material;
		}

		// Token: 0x06000409 RID: 1033
		protected abstract int GetPassIndex(DecalCachedChunk decalCachedChunk);

		// Token: 0x0600040A RID: 1034 RVA: 0x00016E60 File Offset: 0x00015060
		private void Execute(CommandBuffer cmd, DecalEntityChunk decalEntityChunk, DecalCachedChunk decalCachedChunk, DecalDrawCallChunk decalDrawCallChunk, int count)
		{
			decalCachedChunk.currentJobHandle.Complete();
			decalDrawCallChunk.currentJobHandle.Complete();
			Material material = this.GetMaterial(decalEntityChunk);
			int passIndex = this.GetPassIndex(decalCachedChunk);
			if (count == 0 || passIndex == -1 || material == null)
			{
				return;
			}
			if (SystemInfo.supportsInstancing && material.enableInstancing)
			{
				this.DrawInstanced(cmd, decalEntityChunk, decalCachedChunk, decalDrawCallChunk, passIndex);
				return;
			}
			this.Draw(cmd, decalEntityChunk, decalCachedChunk, decalDrawCallChunk, passIndex);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00016ED4 File Offset: 0x000150D4
		private void Draw(CommandBuffer cmd, DecalEntityChunk decalEntityChunk, DecalCachedChunk decalCachedChunk, DecalDrawCallChunk decalDrawCallChunk, int passIndex)
		{
			Mesh decalProjectorMesh = this.m_EntityManager.decalProjectorMesh;
			Material material = this.GetMaterial(decalEntityChunk);
			decalCachedChunk.propertyBlock.SetVector("unity_LightData", new Vector4(1f, 1f, 1f, 0f));
			int subCallCount = decalDrawCallChunk.subCallCount;
			for (int i = 0; i < subCallCount; i++)
			{
				DecalSubDrawCall decalSubDrawCall = decalDrawCallChunk.subCalls[i];
				for (int j = decalSubDrawCall.start; j < decalSubDrawCall.end; j++)
				{
					decalCachedChunk.propertyBlock.SetMatrix("_NormalToWorld", decalDrawCallChunk.normalToDecals[j]);
					decalCachedChunk.propertyBlock.SetFloat("_DecalLayerMaskFromDecal", decalDrawCallChunk.renderingLayerMasks[j]);
					cmd.DrawMesh(decalProjectorMesh, decalDrawCallChunk.decalToWorlds[j], material, 0, passIndex, decalCachedChunk.propertyBlock);
				}
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00016FC8 File Offset: 0x000151C8
		private void DrawInstanced(CommandBuffer cmd, DecalEntityChunk decalEntityChunk, DecalCachedChunk decalCachedChunk, DecalDrawCallChunk decalDrawCallChunk, int passIndex)
		{
			Mesh decalProjectorMesh = this.m_EntityManager.decalProjectorMesh;
			Material material = this.GetMaterial(decalEntityChunk);
			decalCachedChunk.propertyBlock.SetVector("unity_LightData", new Vector4(1f, 1f, 1f, 0f));
			int subCallCount = decalDrawCallChunk.subCallCount;
			for (int i = 0; i < subCallCount; i++)
			{
				DecalSubDrawCall decalSubDrawCall = decalDrawCallChunk.subCalls[i];
				NativeArray<Matrix4x4>.Copy(decalDrawCallChunk.decalToWorlds.Reinterpret<Matrix4x4>(), decalSubDrawCall.start, this.m_WorldToDecals, 0, decalSubDrawCall.count);
				NativeArray<Matrix4x4>.Copy(decalDrawCallChunk.normalToDecals.Reinterpret<Matrix4x4>(), decalSubDrawCall.start, this.m_NormalToDecals, 0, decalSubDrawCall.count);
				NativeArray<float>.Copy(decalDrawCallChunk.renderingLayerMasks.Reinterpret<float>(), decalSubDrawCall.start, this.m_DecalLayerMasks, 0, decalSubDrawCall.count);
				decalCachedChunk.propertyBlock.SetMatrixArray("_NormalToWorld", this.m_NormalToDecals);
				decalCachedChunk.propertyBlock.SetFloatArray("_DecalLayerMaskFromDecal", this.m_DecalLayerMasks);
				cmd.DrawMeshInstanced(decalProjectorMesh, 0, material, passIndex, this.m_WorldToDecals, decalSubDrawCall.end - decalSubDrawCall.start, decalCachedChunk.propertyBlock);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00017100 File Offset: 0x00015300
		public void Execute(in CameraData cameraData)
		{
			using (new ProfilingScope(null, this.m_Sampler))
			{
				for (int i = 0; i < this.m_EntityManager.chunkCount; i++)
				{
					this.Execute(in cameraData, this.m_EntityManager.entityChunks[i], this.m_EntityManager.cachedChunks[i], this.m_EntityManager.drawCallChunks[i], this.m_EntityManager.entityChunks[i].count);
				}
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000171A4 File Offset: 0x000153A4
		private void Execute(in CameraData cameraData, DecalEntityChunk decalEntityChunk, DecalCachedChunk decalCachedChunk, DecalDrawCallChunk decalDrawCallChunk, int count)
		{
			decalCachedChunk.currentJobHandle.Complete();
			decalDrawCallChunk.currentJobHandle.Complete();
			Material material = this.GetMaterial(decalEntityChunk);
			int passIndex = this.GetPassIndex(decalCachedChunk);
			if (count == 0 || passIndex == -1 || material == null)
			{
				return;
			}
			if (SystemInfo.supportsInstancing && material.enableInstancing)
			{
				this.DrawInstanced(in cameraData, decalEntityChunk, decalCachedChunk, decalDrawCallChunk);
				return;
			}
			this.Draw(in cameraData, decalEntityChunk, decalCachedChunk, decalDrawCallChunk);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00017218 File Offset: 0x00015418
		private void Draw(in CameraData cameraData, DecalEntityChunk decalEntityChunk, DecalCachedChunk decalCachedChunk, DecalDrawCallChunk decalDrawCallChunk)
		{
			Mesh decalProjectorMesh = this.m_EntityManager.decalProjectorMesh;
			Material material = this.GetMaterial(decalEntityChunk);
			int subCallCount = decalDrawCallChunk.subCallCount;
			for (int i = 0; i < subCallCount; i++)
			{
				DecalSubDrawCall decalSubDrawCall = decalDrawCallChunk.subCalls[i];
				for (int j = decalSubDrawCall.start; j < decalSubDrawCall.end; j++)
				{
					decalCachedChunk.propertyBlock.SetMatrix("_NormalToWorld", decalDrawCallChunk.normalToDecals[j]);
					decalCachedChunk.propertyBlock.SetFloat("_DecalLayerMaskFromDecal", decalDrawCallChunk.renderingLayerMasks[j]);
					Graphics.DrawMesh(decalProjectorMesh, decalDrawCallChunk.decalToWorlds[j], material, decalCachedChunk.layerMasks[j], cameraData.camera, 0, decalCachedChunk.propertyBlock);
				}
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x000172F8 File Offset: 0x000154F8
		private void DrawInstanced(in CameraData cameraData, DecalEntityChunk decalEntityChunk, DecalCachedChunk decalCachedChunk, DecalDrawCallChunk decalDrawCallChunk)
		{
			Mesh decalProjectorMesh = this.m_EntityManager.decalProjectorMesh;
			Material material = this.GetMaterial(decalEntityChunk);
			decalCachedChunk.propertyBlock.SetVector("unity_LightData", new Vector4(1f, 1f, 1f, 0f));
			int subCallCount = decalDrawCallChunk.subCallCount;
			for (int i = 0; i < subCallCount; i++)
			{
				DecalSubDrawCall decalSubDrawCall = decalDrawCallChunk.subCalls[i];
				NativeArray<Matrix4x4>.Copy(decalDrawCallChunk.decalToWorlds.Reinterpret<Matrix4x4>(), decalSubDrawCall.start, this.m_WorldToDecals, 0, decalSubDrawCall.count);
				NativeArray<Matrix4x4>.Copy(decalDrawCallChunk.normalToDecals.Reinterpret<Matrix4x4>(), decalSubDrawCall.start, this.m_NormalToDecals, 0, decalSubDrawCall.count);
				NativeArray<float>.Copy(decalDrawCallChunk.renderingLayerMasks.Reinterpret<float>(), decalSubDrawCall.start, this.m_DecalLayerMasks, 0, decalSubDrawCall.count);
				decalCachedChunk.propertyBlock.SetMatrixArray("_NormalToWorld", this.m_NormalToDecals);
				decalCachedChunk.propertyBlock.SetFloatArray("_DecalLayerMaskFromDecal", this.m_DecalLayerMasks);
				Graphics.DrawMeshInstanced(decalProjectorMesh, 0, material, this.m_WorldToDecals, decalSubDrawCall.count, decalCachedChunk.propertyBlock, ShadowCastingMode.On, true, 0, cameraData.camera);
			}
		}

		// Token: 0x040002F9 RID: 761
		internal static readonly uint MaxBatchSize = 250U;

		// Token: 0x040002FA RID: 762
		protected DecalEntityManager m_EntityManager;

		// Token: 0x040002FB RID: 763
		private Matrix4x4[] m_WorldToDecals;

		// Token: 0x040002FC RID: 764
		private Matrix4x4[] m_NormalToDecals;

		// Token: 0x040002FD RID: 765
		private float[] m_DecalLayerMasks;

		// Token: 0x040002FE RID: 766
		private ProfilingSampler m_Sampler;
	}
}
