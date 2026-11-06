using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000071 RID: 113
	internal class DecalEntityManager : IDisposable
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0001776D File Offset: 0x0001596D
		public Material errorMaterial
		{
			get
			{
				if (this.m_ErrorMaterial == null)
				{
					this.m_ErrorMaterial = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/InternalErrorShader"));
				}
				return this.m_ErrorMaterial;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x00017798 File Offset: 0x00015998
		public Mesh decalProjectorMesh
		{
			get
			{
				if (this.m_DecalProjectorMesh == null)
				{
					this.m_DecalProjectorMesh = CoreUtils.CreateCubeMesh(new Vector4(-0.5f, -0.5f, -0.5f, 1f), new Vector4(0.5f, 0.5f, 0.5f, 1f));
				}
				return this.m_DecalProjectorMesh;
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00017800 File Offset: 0x00015A00
		public DecalEntityManager()
		{
			this.m_AddDecalSampler = new ProfilingSampler("DecalEntityManager.CreateDecalEntity");
			this.m_ResizeChunks = new ProfilingSampler("DecalEntityManager.ResizeChunks");
			this.m_SortChunks = new ProfilingSampler("DecalEntityManager.SortChunks");
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001789B File Offset: 0x00015A9B
		public bool IsValid(DecalEntity decalEntity)
		{
			return this.m_DecalEntityIndexer.IsValid(decalEntity);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x000178AC File Offset: 0x00015AAC
		public DecalEntity CreateDecalEntity(DecalProjector decalProjector)
		{
			Material material = decalProjector.material;
			if (material == null)
			{
				material = this.errorMaterial;
			}
			DecalEntity decalEntity2;
			using (new ProfilingScope(null, this.m_AddDecalSampler))
			{
				int num = this.CreateChunkIndex(material);
				int count = this.entityChunks[num].count;
				DecalEntity decalEntity = this.m_DecalEntityIndexer.CreateDecalEntity(count, num);
				DecalEntityChunk decalEntityChunk = this.entityChunks[num];
				DecalCachedChunk decalCachedChunk = this.cachedChunks[num];
				DecalCulledChunk decalCulledChunk = this.culledChunks[num];
				DecalDrawCallChunk decalDrawCallChunk = this.drawCallChunks[num];
				if (this.entityChunks[num].capacity == this.entityChunks[num].count)
				{
					using (new ProfilingScope(null, this.m_ResizeChunks))
					{
						int num2 = this.entityChunks[num].capacity + this.entityChunks[num].capacity;
						num2 = math.max(8, num2);
						decalEntityChunk.SetCapacity(num2);
						decalCachedChunk.SetCapacity(num2);
						decalCulledChunk.SetCapacity(num2);
						decalDrawCallChunk.SetCapacity(num2);
					}
				}
				decalEntityChunk.Push();
				decalCachedChunk.Push();
				decalCulledChunk.Push();
				decalDrawCallChunk.Push();
				decalEntityChunk.decalProjectors[count] = decalProjector;
				decalEntityChunk.decalEntities[count] = decalEntity;
				decalEntityChunk.transformAccessArray.Add(decalProjector.transform);
				this.UpdateDecalEntityData(decalEntity, decalProjector);
				decalEntity2 = decalEntity;
			}
			return decalEntity2;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00017A70 File Offset: 0x00015C70
		private int CreateChunkIndex(Material material)
		{
			int num;
			if (!this.m_MaterialToChunkIndex.TryGetValue(material, out num))
			{
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				materialPropertyBlock.SetMatrixArray("_NormalToWorld", new Matrix4x4[DecalDrawSystem.MaxBatchSize]);
				materialPropertyBlock.SetFloatArray("_DecalLayerMaskFromDecal", new float[DecalDrawSystem.MaxBatchSize]);
				this.entityChunks.Add(new DecalEntityChunk
				{
					material = material
				});
				this.cachedChunks.Add(new DecalCachedChunk
				{
					propertyBlock = materialPropertyBlock
				});
				this.culledChunks.Add(new DecalCulledChunk());
				this.drawCallChunks.Add(new DecalDrawCallChunk
				{
					subCallCounts = new NativeArray<int>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory)
				});
				this.m_CombinedChunks.Add(default(DecalEntityManager.CombinedChunks));
				this.m_CombinedChunkRemmap.Add(0);
				this.m_MaterialToChunkIndex.Add(material, this.chunkCount);
				int num2 = this.chunkCount;
				this.chunkCount = num2 + 1;
				return num2;
			}
			return num;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00017B64 File Offset: 0x00015D64
		public void UpdateAllDecalEntitiesData()
		{
			foreach (DecalEntityChunk decalEntityChunk in this.entityChunks)
			{
				for (int i = 0; i < decalEntityChunk.count; i++)
				{
					DecalProjector decalProjector = decalEntityChunk.decalProjectors[i];
					if (!(decalProjector == null))
					{
						DecalEntity decalEntity = decalEntityChunk.decalEntities[i];
						if (this.IsValid(decalEntity))
						{
							this.UpdateDecalEntityData(decalEntity, decalProjector);
						}
					}
				}
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00017BF4 File Offset: 0x00015DF4
		public void UpdateDecalEntityData(DecalEntity decalEntity, DecalProjector decalProjector)
		{
			DecalEntityIndexer.DecalEntityItem item = this.m_DecalEntityIndexer.GetItem(decalEntity);
			int chunkIndex = item.chunkIndex;
			int arrayIndex = item.arrayIndex;
			DecalCachedChunk decalCachedChunk = this.cachedChunks[chunkIndex];
			decalCachedChunk.sizeOffsets[arrayIndex] = Matrix4x4.Translate(decalProjector.decalOffset) * Matrix4x4.Scale(decalProjector.decalSize);
			float drawDistance = decalProjector.drawDistance;
			float fadeScale = decalProjector.fadeScale;
			float startAngleFade = decalProjector.startAngleFade;
			float endAngleFade = decalProjector.endAngleFade;
			Vector4 uvScaleBias = decalProjector.uvScaleBias;
			int layer = decalProjector.gameObject.layer;
			ulong sceneCullingMask = decalProjector.gameObject.sceneCullingMask;
			float fadeFactor = decalProjector.fadeFactor;
			decalCachedChunk.drawDistances[arrayIndex] = new Vector2(drawDistance, fadeScale);
			if (startAngleFade == 180f)
			{
				decalCachedChunk.angleFades[arrayIndex] = new Vector2(0f, 0f);
			}
			else
			{
				float num = startAngleFade / 180f;
				float num2 = endAngleFade / 180f;
				float num3 = Mathf.Max(0.0001f, num2 - num);
				decalCachedChunk.angleFades[arrayIndex] = new Vector2(1f - (0.25f - num) / num3, -0.25f / num3);
			}
			decalCachedChunk.uvScaleBias[arrayIndex] = uvScaleBias;
			decalCachedChunk.layerMasks[arrayIndex] = layer;
			decalCachedChunk.sceneLayerMasks[arrayIndex] = sceneCullingMask;
			decalCachedChunk.fadeFactors[arrayIndex] = fadeFactor;
			decalCachedChunk.scaleModes[arrayIndex] = decalProjector.scaleMode;
			decalCachedChunk.renderingLayerMasks[arrayIndex] = RenderingLayerUtils.ToValidRenderingLayers(decalProjector.renderingLayerMask);
			decalCachedChunk.positions[arrayIndex] = decalProjector.transform.position;
			decalCachedChunk.rotation[arrayIndex] = decalProjector.transform.rotation;
			decalCachedChunk.scales[arrayIndex] = decalProjector.transform.lossyScale;
			decalCachedChunk.dirty[arrayIndex] = true;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00017DFC File Offset: 0x00015FFC
		public void DestroyDecalEntity(DecalEntity decalEntity)
		{
			if (!this.m_DecalEntityIndexer.IsValid(decalEntity))
			{
				return;
			}
			DecalEntityIndexer.DecalEntityItem item = this.m_DecalEntityIndexer.GetItem(decalEntity);
			this.m_DecalEntityIndexer.DestroyDecalEntity(decalEntity);
			int chunkIndex = item.chunkIndex;
			int arrayIndex = item.arrayIndex;
			DecalEntityChunk decalEntityChunk = this.entityChunks[chunkIndex];
			DecalCachedChunk decalCachedChunk = this.cachedChunks[chunkIndex];
			DecalCulledChunk decalCulledChunk = this.culledChunks[chunkIndex];
			DecalChunk decalChunk = this.drawCallChunks[chunkIndex];
			int num = decalEntityChunk.count - 1;
			if (arrayIndex != num)
			{
				this.m_DecalEntityIndexer.UpdateIndex(decalEntityChunk.decalEntities[num], arrayIndex);
			}
			decalEntityChunk.RemoveAtSwapBack(arrayIndex);
			decalCachedChunk.RemoveAtSwapBack(arrayIndex);
			decalCulledChunk.RemoveAtSwapBack(arrayIndex);
			decalChunk.RemoveAtSwapBack(arrayIndex);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00017EB8 File Offset: 0x000160B8
		public void Update()
		{
			using (new ProfilingScope(null, this.m_SortChunks))
			{
				for (int i = 0; i < this.chunkCount; i++)
				{
					if (this.entityChunks[i].material == null)
					{
						this.entityChunks[i].material = this.errorMaterial;
					}
				}
				for (int j = 0; j < this.chunkCount; j++)
				{
					this.m_CombinedChunks[j] = new DecalEntityManager.CombinedChunks
					{
						entityChunk = this.entityChunks[j],
						cachedChunk = this.cachedChunks[j],
						culledChunk = this.culledChunks[j],
						drawCallChunk = this.drawCallChunks[j],
						previousChunkIndex = j,
						valid = (this.entityChunks[j].count != 0)
					};
				}
				this.m_CombinedChunks.Sort(delegate(DecalEntityManager.CombinedChunks a, DecalEntityManager.CombinedChunks b)
				{
					if (a.valid && !b.valid)
					{
						return -1;
					}
					if (!a.valid && b.valid)
					{
						return 1;
					}
					if (a.cachedChunk.drawOrder < b.cachedChunk.drawOrder)
					{
						return -1;
					}
					if (a.cachedChunk.drawOrder > b.cachedChunk.drawOrder)
					{
						return 1;
					}
					return a.entityChunk.material.GetHashCode().CompareTo(b.entityChunk.material.GetHashCode());
				});
				bool flag = false;
				for (int k = 0; k < this.chunkCount; k++)
				{
					if (this.m_CombinedChunks[k].previousChunkIndex != k || !this.m_CombinedChunks[k].valid)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					int num = 0;
					this.m_MaterialToChunkIndex.Clear();
					for (int l = 0; l < this.chunkCount; l++)
					{
						DecalEntityManager.CombinedChunks combinedChunks = this.m_CombinedChunks[l];
						if (!this.m_CombinedChunks[l].valid)
						{
							combinedChunks.entityChunk.currentJobHandle.Complete();
							combinedChunks.cachedChunk.currentJobHandle.Complete();
							combinedChunks.culledChunk.currentJobHandle.Complete();
							combinedChunks.drawCallChunk.currentJobHandle.Complete();
							combinedChunks.entityChunk.Dispose();
							combinedChunks.cachedChunk.Dispose();
							combinedChunks.culledChunk.Dispose();
							combinedChunks.drawCallChunk.Dispose();
						}
						else
						{
							this.entityChunks[l] = combinedChunks.entityChunk;
							this.cachedChunks[l] = combinedChunks.cachedChunk;
							this.culledChunks[l] = combinedChunks.culledChunk;
							this.drawCallChunks[l] = combinedChunks.drawCallChunk;
							if (!this.m_MaterialToChunkIndex.ContainsKey(this.entityChunks[l].material))
							{
								this.m_MaterialToChunkIndex.Add(this.entityChunks[l].material, l);
							}
							this.m_CombinedChunkRemmap[combinedChunks.previousChunkIndex] = l;
							num++;
						}
					}
					if (this.chunkCount > num)
					{
						this.entityChunks.RemoveRange(num, this.chunkCount - num);
						this.cachedChunks.RemoveRange(num, this.chunkCount - num);
						this.culledChunks.RemoveRange(num, this.chunkCount - num);
						this.drawCallChunks.RemoveRange(num, this.chunkCount - num);
						this.m_CombinedChunks.RemoveRange(num, this.chunkCount - num);
						this.chunkCount = num;
					}
					this.m_DecalEntityIndexer.RemapChunkIndices(this.m_CombinedChunkRemmap);
				}
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00018260 File Offset: 0x00016460
		public void Dispose()
		{
			CoreUtils.Destroy(this.m_ErrorMaterial);
			CoreUtils.Destroy(this.m_DecalProjectorMesh);
			foreach (DecalEntityChunk decalEntityChunk in this.entityChunks)
			{
				decalEntityChunk.currentJobHandle.Complete();
			}
			foreach (DecalCachedChunk decalCachedChunk in this.cachedChunks)
			{
				decalCachedChunk.currentJobHandle.Complete();
			}
			foreach (DecalCulledChunk decalCulledChunk in this.culledChunks)
			{
				decalCulledChunk.currentJobHandle.Complete();
			}
			foreach (DecalDrawCallChunk decalDrawCallChunk in this.drawCallChunks)
			{
				decalDrawCallChunk.currentJobHandle.Complete();
			}
			foreach (DecalEntityChunk decalEntityChunk2 in this.entityChunks)
			{
				decalEntityChunk2.Dispose();
			}
			foreach (DecalCachedChunk decalCachedChunk2 in this.cachedChunks)
			{
				decalCachedChunk2.Dispose();
			}
			foreach (DecalCulledChunk decalCulledChunk2 in this.culledChunks)
			{
				decalCulledChunk2.Dispose();
			}
			foreach (DecalDrawCallChunk decalDrawCallChunk2 in this.drawCallChunks)
			{
				decalDrawCallChunk2.Dispose();
			}
			this.m_DecalEntityIndexer.Clear();
			this.m_MaterialToChunkIndex.Clear();
			this.entityChunks.Clear();
			this.cachedChunks.Clear();
			this.culledChunks.Clear();
			this.drawCallChunks.Clear();
			this.m_CombinedChunks.Clear();
			this.chunkCount = 0;
		}

		// Token: 0x04000308 RID: 776
		public List<DecalEntityChunk> entityChunks = new List<DecalEntityChunk>();

		// Token: 0x04000309 RID: 777
		public List<DecalCachedChunk> cachedChunks = new List<DecalCachedChunk>();

		// Token: 0x0400030A RID: 778
		public List<DecalCulledChunk> culledChunks = new List<DecalCulledChunk>();

		// Token: 0x0400030B RID: 779
		public List<DecalDrawCallChunk> drawCallChunks = new List<DecalDrawCallChunk>();

		// Token: 0x0400030C RID: 780
		public int chunkCount;

		// Token: 0x0400030D RID: 781
		private ProfilingSampler m_AddDecalSampler;

		// Token: 0x0400030E RID: 782
		private ProfilingSampler m_ResizeChunks;

		// Token: 0x0400030F RID: 783
		private ProfilingSampler m_SortChunks;

		// Token: 0x04000310 RID: 784
		private DecalEntityIndexer m_DecalEntityIndexer = new DecalEntityIndexer();

		// Token: 0x04000311 RID: 785
		private Dictionary<Material, int> m_MaterialToChunkIndex = new Dictionary<Material, int>();

		// Token: 0x04000312 RID: 786
		private List<DecalEntityManager.CombinedChunks> m_CombinedChunks = new List<DecalEntityManager.CombinedChunks>();

		// Token: 0x04000313 RID: 787
		private List<int> m_CombinedChunkRemmap = new List<int>();

		// Token: 0x04000314 RID: 788
		private Material m_ErrorMaterial;

		// Token: 0x04000315 RID: 789
		private Mesh m_DecalProjectorMesh;

		// Token: 0x0200017F RID: 383
		private struct CombinedChunks
		{
			// Token: 0x04000A28 RID: 2600
			public DecalEntityChunk entityChunk;

			// Token: 0x04000A29 RID: 2601
			public DecalCachedChunk cachedChunk;

			// Token: 0x04000A2A RID: 2602
			public DecalCulledChunk culledChunk;

			// Token: 0x04000A2B RID: 2603
			public DecalDrawCallChunk drawCallChunk;

			// Token: 0x04000A2C RID: 2604
			public int previousChunkIndex;

			// Token: 0x04000A2D RID: 2605
			public bool valid;
		}
	}
}
