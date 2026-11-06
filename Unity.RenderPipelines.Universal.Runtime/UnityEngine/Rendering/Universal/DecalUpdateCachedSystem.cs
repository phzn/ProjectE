using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Jobs;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000074 RID: 116
	internal class DecalUpdateCachedSystem
	{
		// Token: 0x06000432 RID: 1074 RVA: 0x00018928 File Offset: 0x00016B28
		public DecalUpdateCachedSystem(DecalEntityManager entityManager)
		{
			this.m_EntityManager = entityManager;
			this.m_Sampler = new ProfilingSampler("DecalUpdateCachedSystem.Execute");
			this.m_SamplerJob = new ProfilingSampler("DecalUpdateCachedSystem.ExecuteJob");
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00018958 File Offset: 0x00016B58
		public void Execute()
		{
			using (new ProfilingScope(null, this.m_Sampler))
			{
				for (int i = 0; i < this.m_EntityManager.chunkCount; i++)
				{
					this.Execute(this.m_EntityManager.entityChunks[i], this.m_EntityManager.cachedChunks[i], this.m_EntityManager.entityChunks[i].count);
				}
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000189E8 File Offset: 0x00016BE8
		private void Execute(DecalEntityChunk entityChunk, DecalCachedChunk cachedChunk, int count)
		{
			if (count == 0)
			{
				return;
			}
			cachedChunk.currentJobHandle.Complete();
			Material material = entityChunk.material;
			if (material.HasProperty("_DrawOrder"))
			{
				cachedChunk.drawOrder = material.GetInt("_DrawOrder");
			}
			if (!cachedChunk.isCreated)
			{
				int num = material.FindPass("DBufferProjector");
				cachedChunk.passIndexDBuffer = num;
				int num2 = material.FindPass("DecalProjectorForwardEmissive");
				cachedChunk.passIndexEmissive = num2;
				int num3 = material.FindPass("DecalScreenSpaceProjector");
				cachedChunk.passIndexScreenSpace = num3;
				int num4 = material.FindPass("DecalGBufferProjector");
				cachedChunk.passIndexGBuffer = num4;
				cachedChunk.isCreated = true;
			}
			using (new ProfilingScope(null, this.m_SamplerJob))
			{
				JobHandle jobHandle = new DecalUpdateCachedSystem.UpdateTransformsJob
				{
					positions = cachedChunk.positions,
					rotations = cachedChunk.rotation,
					scales = cachedChunk.scales,
					dirty = cachedChunk.dirty,
					scaleModes = cachedChunk.scaleModes,
					sizeOffsets = cachedChunk.sizeOffsets,
					decalToWorlds = cachedChunk.decalToWorlds,
					normalToWorlds = cachedChunk.normalToWorlds,
					boundingSpheres = cachedChunk.boundingSpheres,
					minDistance = float.Epsilon
				}.Schedule(entityChunk.transformAccessArray, default(JobHandle));
				cachedChunk.currentJobHandle = jobHandle;
			}
		}

		// Token: 0x04000331 RID: 817
		private DecalEntityManager m_EntityManager;

		// Token: 0x04000332 RID: 818
		private ProfilingSampler m_Sampler;

		// Token: 0x04000333 RID: 819
		private ProfilingSampler m_SamplerJob;

		// Token: 0x02000181 RID: 385
		[BurstCompile]
		public struct UpdateTransformsJob : IJobParallelForTransform
		{
			// Token: 0x06000AA2 RID: 2722 RVA: 0x00048D5D File Offset: 0x00046F5D
			private float DistanceBetweenQuaternions(quaternion a, quaternion b)
			{
				return math.distancesq(a.value, b.value);
			}

			// Token: 0x06000AA3 RID: 2723 RVA: 0x00048D70 File Offset: 0x00046F70
			public void Execute(int index, TransformAccess transform)
			{
				bool flag = math.distancesq(transform.position, this.positions[index]) > this.minDistance;
				if (flag)
				{
					this.positions[index] = transform.position;
				}
				bool flag2 = this.DistanceBetweenQuaternions(transform.rotation, this.rotations[index]) > this.minDistance;
				if (flag2)
				{
					this.rotations[index] = transform.rotation;
				}
				bool flag3 = math.distancesq(transform.localScale, this.scales[index]) > this.minDistance;
				if (flag3)
				{
					this.scales[index] = transform.localScale;
				}
				if (!flag && !flag2 && !flag3 && !this.dirty[index])
				{
					return;
				}
				float4x4 float4x;
				if (this.scaleModes[index] == DecalScaleMode.InheritFromHierarchy)
				{
					float4x = transform.localToWorldMatrix;
					float4x = math.mul(float4x, new float4x4(DecalUpdateCachedSystem.UpdateTransformsJob.k_MinusYtoZRotation, float3.zero));
				}
				else
				{
					quaternion quaternion = math.mul(transform.rotation, DecalUpdateCachedSystem.UpdateTransformsJob.k_MinusYtoZRotation);
					float4x = float4x4.TRS(this.positions[index], quaternion, new float3(1f, 1f, 1f));
				}
				float4x4 float4x2 = float4x;
				float4 c = float4x2.c1;
				float4x2.c1 = float4x2.c2;
				float4x2.c2 = c;
				this.normalToWorlds[index] = float4x2;
				float4x4 float4x3 = this.sizeOffsets[index];
				float4x4 float4x4 = math.mul(float4x, float4x3);
				this.decalToWorlds[index] = float4x4;
				this.boundingSpheres[index] = this.GetDecalProjectBoundingSphere(float4x4);
				this.dirty[index] = false;
			}

			// Token: 0x06000AA4 RID: 2724 RVA: 0x00048F44 File Offset: 0x00047144
			private BoundingSphere GetDecalProjectBoundingSphere(Matrix4x4 decalToWorld)
			{
				float4 @float = new float4(-0.5f, -0.5f, -0.5f, 1f);
				float4 float2 = new float4(0.5f, 0.5f, 0.5f, 1f);
				@float = math.mul(decalToWorld, @float);
				float2 = math.mul(decalToWorld, float2);
				float3 xyz = ((float2 + @float) / 2f).xyz;
				float num = math.length(float2 - @float) / 2f;
				return new BoundingSphere
				{
					position = xyz,
					radius = num
				};
			}

			// Token: 0x04000A30 RID: 2608
			private static readonly quaternion k_MinusYtoZRotation = quaternion.EulerXYZ(-1.5707964f, 0f, 0f);

			// Token: 0x04000A31 RID: 2609
			public NativeArray<float3> positions;

			// Token: 0x04000A32 RID: 2610
			public NativeArray<quaternion> rotations;

			// Token: 0x04000A33 RID: 2611
			public NativeArray<float3> scales;

			// Token: 0x04000A34 RID: 2612
			public NativeArray<bool> dirty;

			// Token: 0x04000A35 RID: 2613
			[ReadOnly]
			public NativeArray<DecalScaleMode> scaleModes;

			// Token: 0x04000A36 RID: 2614
			[ReadOnly]
			public NativeArray<float4x4> sizeOffsets;

			// Token: 0x04000A37 RID: 2615
			[WriteOnly]
			public NativeArray<float4x4> decalToWorlds;

			// Token: 0x04000A38 RID: 2616
			[WriteOnly]
			public NativeArray<float4x4> normalToWorlds;

			// Token: 0x04000A39 RID: 2617
			[WriteOnly]
			public NativeArray<BoundingSphere> boundingSpheres;

			// Token: 0x04000A3A RID: 2618
			public float minDistance;
		}
	}
}
