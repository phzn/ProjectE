using System;
using Unity.Collections;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000073 RID: 115
	internal class DecalCachedChunk : DecalChunk
	{
		// Token: 0x0600042E RID: 1070 RVA: 0x0001860C File Offset: 0x0001680C
		public override void RemoveAtSwapBack(int entityIndex)
		{
			base.RemoveAtSwapBack<float4x4>(ref this.decalToWorlds, entityIndex, base.count);
			base.RemoveAtSwapBack<float4x4>(ref this.normalToWorlds, entityIndex, base.count);
			base.RemoveAtSwapBack<float4x4>(ref this.sizeOffsets, entityIndex, base.count);
			base.RemoveAtSwapBack<float2>(ref this.drawDistances, entityIndex, base.count);
			base.RemoveAtSwapBack<float2>(ref this.angleFades, entityIndex, base.count);
			base.RemoveAtSwapBack<float4>(ref this.uvScaleBias, entityIndex, base.count);
			base.RemoveAtSwapBack<int>(ref this.layerMasks, entityIndex, base.count);
			base.RemoveAtSwapBack<ulong>(ref this.sceneLayerMasks, entityIndex, base.count);
			base.RemoveAtSwapBack<float>(ref this.fadeFactors, entityIndex, base.count);
			base.RemoveAtSwapBack<BoundingSphere>(ref this.boundingSphereArray, entityIndex, base.count);
			base.RemoveAtSwapBack<BoundingSphere>(ref this.boundingSpheres, entityIndex, base.count);
			base.RemoveAtSwapBack<DecalScaleMode>(ref this.scaleModes, entityIndex, base.count);
			base.RemoveAtSwapBack<uint>(ref this.renderingLayerMasks, entityIndex, base.count);
			base.RemoveAtSwapBack<float3>(ref this.positions, entityIndex, base.count);
			base.RemoveAtSwapBack<quaternion>(ref this.rotation, entityIndex, base.count);
			base.RemoveAtSwapBack<float3>(ref this.scales, entityIndex, base.count);
			base.RemoveAtSwapBack<bool>(ref this.dirty, entityIndex, base.count);
			int count = base.count;
			base.count = count - 1;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001876C File Offset: 0x0001696C
		public override void SetCapacity(int newCapacity)
		{
			(ref this.decalToWorlds).ResizeArray<float4x4>(newCapacity);
			(ref this.normalToWorlds).ResizeArray<float4x4>(newCapacity);
			(ref this.sizeOffsets).ResizeArray<float4x4>(newCapacity);
			(ref this.drawDistances).ResizeArray<float2>(newCapacity);
			(ref this.angleFades).ResizeArray<float2>(newCapacity);
			(ref this.uvScaleBias).ResizeArray<float4>(newCapacity);
			(ref this.layerMasks).ResizeArray<int>(newCapacity);
			(ref this.sceneLayerMasks).ResizeArray<ulong>(newCapacity);
			(ref this.fadeFactors).ResizeArray<float>(newCapacity);
			(ref this.boundingSpheres).ResizeArray<BoundingSphere>(newCapacity);
			(ref this.scaleModes).ResizeArray<DecalScaleMode>(newCapacity);
			(ref this.renderingLayerMasks).ResizeArray<uint>(newCapacity);
			(ref this.positions).ResizeArray<float3>(newCapacity);
			(ref this.rotation).ResizeArray<quaternion>(newCapacity);
			(ref this.scales).ResizeArray<float3>(newCapacity);
			(ref this.dirty).ResizeArray<bool>(newCapacity);
			ArrayExtensions.ResizeArray<BoundingSphere>(ref this.boundingSphereArray, newCapacity);
			base.capacity = newCapacity;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001884C File Offset: 0x00016A4C
		public override void Dispose()
		{
			if (base.capacity == 0)
			{
				return;
			}
			this.decalToWorlds.Dispose();
			this.normalToWorlds.Dispose();
			this.sizeOffsets.Dispose();
			this.drawDistances.Dispose();
			this.angleFades.Dispose();
			this.uvScaleBias.Dispose();
			this.layerMasks.Dispose();
			this.sceneLayerMasks.Dispose();
			this.fadeFactors.Dispose();
			this.boundingSpheres.Dispose();
			this.scaleModes.Dispose();
			this.renderingLayerMasks.Dispose();
			this.positions.Dispose();
			this.rotation.Dispose();
			this.scales.Dispose();
			this.dirty.Dispose();
			base.count = 0;
			base.capacity = 0;
		}

		// Token: 0x04000319 RID: 793
		public MaterialPropertyBlock propertyBlock;

		// Token: 0x0400031A RID: 794
		public int passIndexDBuffer;

		// Token: 0x0400031B RID: 795
		public int passIndexEmissive;

		// Token: 0x0400031C RID: 796
		public int passIndexScreenSpace;

		// Token: 0x0400031D RID: 797
		public int passIndexGBuffer;

		// Token: 0x0400031E RID: 798
		public int drawOrder;

		// Token: 0x0400031F RID: 799
		public bool isCreated;

		// Token: 0x04000320 RID: 800
		public NativeArray<float4x4> decalToWorlds;

		// Token: 0x04000321 RID: 801
		public NativeArray<float4x4> normalToWorlds;

		// Token: 0x04000322 RID: 802
		public NativeArray<float4x4> sizeOffsets;

		// Token: 0x04000323 RID: 803
		public NativeArray<float2> drawDistances;

		// Token: 0x04000324 RID: 804
		public NativeArray<float2> angleFades;

		// Token: 0x04000325 RID: 805
		public NativeArray<float4> uvScaleBias;

		// Token: 0x04000326 RID: 806
		public NativeArray<int> layerMasks;

		// Token: 0x04000327 RID: 807
		public NativeArray<ulong> sceneLayerMasks;

		// Token: 0x04000328 RID: 808
		public NativeArray<float> fadeFactors;

		// Token: 0x04000329 RID: 809
		public NativeArray<BoundingSphere> boundingSpheres;

		// Token: 0x0400032A RID: 810
		public NativeArray<DecalScaleMode> scaleModes;

		// Token: 0x0400032B RID: 811
		public NativeArray<uint> renderingLayerMasks;

		// Token: 0x0400032C RID: 812
		public NativeArray<float3> positions;

		// Token: 0x0400032D RID: 813
		public NativeArray<quaternion> rotation;

		// Token: 0x0400032E RID: 814
		public NativeArray<float3> scales;

		// Token: 0x0400032F RID: 815
		public NativeArray<bool> dirty;

		// Token: 0x04000330 RID: 816
		public BoundingSphere[] boundingSphereArray;
	}
}
