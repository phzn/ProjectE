using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200006C RID: 108
	internal class DecalCreateDrawCallSystem
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00016B1F File Offset: 0x00014D1F
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x00016B27 File Offset: 0x00014D27
		public float maxDrawDistance
		{
			get
			{
				return this.m_MaxDrawDistance;
			}
			set
			{
				this.m_MaxDrawDistance = value;
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00016B30 File Offset: 0x00014D30
		public DecalCreateDrawCallSystem(DecalEntityManager entityManager, float maxDrawDistance)
		{
			this.m_EntityManager = entityManager;
			this.m_Sampler = new ProfilingSampler("DecalCreateDrawCallSystem.Execute");
			this.m_MaxDrawDistance = maxDrawDistance;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00016B58 File Offset: 0x00014D58
		public void Execute()
		{
			using (new ProfilingScope(null, this.m_Sampler))
			{
				for (int i = 0; i < this.m_EntityManager.chunkCount; i++)
				{
					this.Execute(this.m_EntityManager.cachedChunks[i], this.m_EntityManager.culledChunks[i], this.m_EntityManager.drawCallChunks[i], this.m_EntityManager.cachedChunks[i].count);
				}
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00016BF8 File Offset: 0x00014DF8
		private void Execute(DecalCachedChunk cachedChunk, DecalCulledChunk culledChunk, DecalDrawCallChunk drawCallChunk, int count)
		{
			if (count == 0)
			{
				return;
			}
			JobHandle jobHandle = new DecalCreateDrawCallSystem.DrawCallJob
			{
				decalToWorlds = cachedChunk.decalToWorlds,
				normalToWorlds = cachedChunk.normalToWorlds,
				sizeOffsets = cachedChunk.sizeOffsets,
				drawDistances = cachedChunk.drawDistances,
				angleFades = cachedChunk.angleFades,
				uvScaleBiases = cachedChunk.uvScaleBias,
				layerMasks = cachedChunk.layerMasks,
				sceneLayerMasks = cachedChunk.sceneLayerMasks,
				fadeFactors = cachedChunk.fadeFactors,
				boundingSpheres = cachedChunk.boundingSpheres,
				renderingLayerMasks = cachedChunk.renderingLayerMasks,
				cameraPosition = culledChunk.cameraPosition,
				sceneCullingMask = culledChunk.sceneCullingMask,
				cullingMask = culledChunk.cullingMask,
				visibleDecalIndices = culledChunk.visibleDecalIndices,
				visibleDecalCount = culledChunk.visibleDecalCount,
				maxDrawDistance = this.m_MaxDrawDistance,
				decalToWorldsDraw = drawCallChunk.decalToWorlds,
				normalToDecalsDraw = drawCallChunk.normalToDecals,
				renderingLayerMasksDraw = drawCallChunk.renderingLayerMasks,
				subCalls = drawCallChunk.subCalls,
				subCallCount = drawCallChunk.subCallCounts
			}.Schedule(cachedChunk.currentJobHandle);
			drawCallChunk.currentJobHandle = jobHandle;
			cachedChunk.currentJobHandle = jobHandle;
		}

		// Token: 0x040002F6 RID: 758
		private DecalEntityManager m_EntityManager;

		// Token: 0x040002F7 RID: 759
		private ProfilingSampler m_Sampler;

		// Token: 0x040002F8 RID: 760
		private float m_MaxDrawDistance;

		// Token: 0x0200017D RID: 381
		[BurstCompile]
		private struct DrawCallJob : IJob
		{
			// Token: 0x06000A9E RID: 2718 RVA: 0x00048A7C File Offset: 0x00046C7C
			public void Execute()
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < this.visibleDecalCount; i++)
				{
					int num4 = this.visibleDecalIndices[i];
					int num5 = 1 << this.layerMasks[num4];
					if ((this.cullingMask & num5) != 0)
					{
						BoundingSphere boundingSphere = this.boundingSpheres[num4];
						float2 @float = this.drawDistances[num4];
						float magnitude = (this.cameraPosition - boundingSphere.position).magnitude;
						float num6 = math.min(@float.x, this.maxDrawDistance) + boundingSphere.radius;
						if (magnitude <= num6)
						{
							this.decalToWorldsDraw[num2] = this.decalToWorlds[num4];
							float num7 = this.fadeFactors[num4];
							float2 float2 = this.angleFades[num4];
							float4 float3 = this.uvScaleBiases[num4];
							float4x4 float4x = this.normalToWorlds[num4];
							float num8 = num7 * math.clamp((num6 - magnitude) / (num6 * (1f - @float.y)), 0f, 1f);
							float4x.c0.w = float3.x;
							float4x.c1.w = float3.y;
							float4x.c2.w = float3.z;
							float4x.c3 = new float4(num8 * 1f, float2.x, float2.y, float3.w);
							this.normalToDecalsDraw[num2] = float4x;
							this.renderingLayerMasksDraw[num2] = this.renderingLayerMasks[num4];
							num2++;
							if ((long)(num2 - num3) >= (long)((ulong)DecalDrawSystem.MaxBatchSize))
							{
								this.subCalls[num++] = new DecalSubDrawCall
								{
									start = num3,
									end = num2
								};
								num3 = num2;
							}
						}
					}
				}
				if (num2 - num3 != 0)
				{
					this.subCalls[num++] = new DecalSubDrawCall
					{
						start = num3,
						end = num2
					};
				}
				this.subCallCount[0] = num;
			}

			// Token: 0x04000A0F RID: 2575
			[ReadOnly]
			public NativeArray<float4x4> decalToWorlds;

			// Token: 0x04000A10 RID: 2576
			[ReadOnly]
			public NativeArray<float4x4> normalToWorlds;

			// Token: 0x04000A11 RID: 2577
			[ReadOnly]
			public NativeArray<float4x4> sizeOffsets;

			// Token: 0x04000A12 RID: 2578
			[ReadOnly]
			public NativeArray<float2> drawDistances;

			// Token: 0x04000A13 RID: 2579
			[ReadOnly]
			public NativeArray<float2> angleFades;

			// Token: 0x04000A14 RID: 2580
			[ReadOnly]
			public NativeArray<float4> uvScaleBiases;

			// Token: 0x04000A15 RID: 2581
			[ReadOnly]
			public NativeArray<int> layerMasks;

			// Token: 0x04000A16 RID: 2582
			[ReadOnly]
			public NativeArray<ulong> sceneLayerMasks;

			// Token: 0x04000A17 RID: 2583
			[ReadOnly]
			public NativeArray<float> fadeFactors;

			// Token: 0x04000A18 RID: 2584
			[ReadOnly]
			public NativeArray<BoundingSphere> boundingSpheres;

			// Token: 0x04000A19 RID: 2585
			[ReadOnly]
			public NativeArray<uint> renderingLayerMasks;

			// Token: 0x04000A1A RID: 2586
			public Vector3 cameraPosition;

			// Token: 0x04000A1B RID: 2587
			public ulong sceneCullingMask;

			// Token: 0x04000A1C RID: 2588
			public int cullingMask;

			// Token: 0x04000A1D RID: 2589
			[ReadOnly]
			public NativeArray<int> visibleDecalIndices;

			// Token: 0x04000A1E RID: 2590
			public int visibleDecalCount;

			// Token: 0x04000A1F RID: 2591
			public float maxDrawDistance;

			// Token: 0x04000A20 RID: 2592
			[WriteOnly]
			public NativeArray<float4x4> decalToWorldsDraw;

			// Token: 0x04000A21 RID: 2593
			[WriteOnly]
			public NativeArray<float4x4> normalToDecalsDraw;

			// Token: 0x04000A22 RID: 2594
			[WriteOnly]
			public NativeArray<float> renderingLayerMasksDraw;

			// Token: 0x04000A23 RID: 2595
			[WriteOnly]
			public NativeArray<DecalSubDrawCall> subCalls;

			// Token: 0x04000A24 RID: 2596
			[WriteOnly]
			public NativeArray<int> subCallCount;
		}
	}
}
