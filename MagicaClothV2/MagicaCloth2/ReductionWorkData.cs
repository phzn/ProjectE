using System;
using Unity.Collections;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x020000D6 RID: 214
	public class ReductionWorkData : IDisposable
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x00028158 File Offset: 0x00026358
		public ReductionWorkData(VirtualMesh vmesh)
		{
			this.vmesh = vmesh;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00028168 File Offset: 0x00026368
		public void Dispose()
		{
			if (this.vertexJoinIndices.IsCreated)
			{
				this.vertexJoinIndices.Dispose();
			}
			if (this.vertexToVertexMap.IsCreated)
			{
				this.vertexToVertexMap.Dispose();
			}
			if (this.vertexRemapIndices.IsCreated)
			{
				this.vertexRemapIndices.Dispose();
			}
			if (this.useSkinBoneMap.IsCreated)
			{
				this.useSkinBoneMap.Dispose();
			}
			if (this.newVertexToVertexMap.IsCreated)
			{
				this.newVertexToVertexMap.Dispose();
			}
			if (this.edgeSet.IsCreated)
			{
				this.edgeSet.Dispose();
			}
			if (this.triangleSet.IsCreated)
			{
				this.triangleSet.Dispose();
			}
			ExSimpleNativeArray<VertexAttribute> exSimpleNativeArray = this.newAttributes;
			if (exSimpleNativeArray != null)
			{
				exSimpleNativeArray.Dispose();
			}
			ExSimpleNativeArray<float3> exSimpleNativeArray2 = this.newLocalPositions;
			if (exSimpleNativeArray2 != null)
			{
				exSimpleNativeArray2.Dispose();
			}
			ExSimpleNativeArray<float3> exSimpleNativeArray3 = this.newLocalNormals;
			if (exSimpleNativeArray3 != null)
			{
				exSimpleNativeArray3.Dispose();
			}
			ExSimpleNativeArray<float3> exSimpleNativeArray4 = this.newLocalTangents;
			if (exSimpleNativeArray4 != null)
			{
				exSimpleNativeArray4.Dispose();
			}
			ExSimpleNativeArray<float2> exSimpleNativeArray5 = this.newUv;
			if (exSimpleNativeArray5 != null)
			{
				exSimpleNativeArray5.Dispose();
			}
			ExSimpleNativeArray<VirtualMeshBoneWeight> exSimpleNativeArray6 = this.newBoneWeights;
			if (exSimpleNativeArray6 != null)
			{
				exSimpleNativeArray6.Dispose();
			}
			if (this.newSkinBoneCount.IsCreated)
			{
				this.newSkinBoneCount.Dispose();
			}
			if (this.newSkinBoneTransformIndices.IsCreated)
			{
				this.newSkinBoneTransformIndices.Dispose();
			}
			if (this.newSkinBoneBindPoseList.IsCreated)
			{
				this.newSkinBoneBindPoseList.Dispose();
			}
			if (this.newLineList.IsCreated)
			{
				this.newLineList.Dispose();
			}
			if (this.newTriangleList.IsCreated)
			{
				this.newTriangleList.Dispose();
			}
		}

		// Token: 0x040007DC RID: 2012
		public VirtualMesh vmesh;

		// Token: 0x040007DD RID: 2013
		public NativeArray<int> vertexJoinIndices;

		// Token: 0x040007DE RID: 2014
		public NativeParallelMultiHashMap<ushort, ushort> vertexToVertexMap;

		// Token: 0x040007DF RID: 2015
		public NativeArray<int> vertexRemapIndices;

		// Token: 0x040007E0 RID: 2016
		public NativeParallelHashMap<int, int> useSkinBoneMap;

		// Token: 0x040007E1 RID: 2017
		public NativeParallelMultiHashMap<ushort, ushort> newVertexToVertexMap;

		// Token: 0x040007E2 RID: 2018
		public NativeParallelHashSet<int2> edgeSet;

		// Token: 0x040007E3 RID: 2019
		public NativeParallelHashSet<int3> triangleSet;

		// Token: 0x040007E4 RID: 2020
		public int oldVertexCount;

		// Token: 0x040007E5 RID: 2021
		public int newVertexCount;

		// Token: 0x040007E6 RID: 2022
		public int removeVertexCount;

		// Token: 0x040007E7 RID: 2023
		public ExSimpleNativeArray<VertexAttribute> newAttributes;

		// Token: 0x040007E8 RID: 2024
		public ExSimpleNativeArray<float3> newLocalPositions;

		// Token: 0x040007E9 RID: 2025
		public ExSimpleNativeArray<float3> newLocalNormals;

		// Token: 0x040007EA RID: 2026
		public ExSimpleNativeArray<float3> newLocalTangents;

		// Token: 0x040007EB RID: 2027
		public ExSimpleNativeArray<float2> newUv;

		// Token: 0x040007EC RID: 2028
		public ExSimpleNativeArray<VirtualMeshBoneWeight> newBoneWeights;

		// Token: 0x040007ED RID: 2029
		public NativeReference<int> newSkinBoneCount;

		// Token: 0x040007EE RID: 2030
		public NativeList<int> newSkinBoneTransformIndices;

		// Token: 0x040007EF RID: 2031
		public NativeList<float4x4> newSkinBoneBindPoseList;

		// Token: 0x040007F0 RID: 2032
		public NativeList<int2> newLineList;

		// Token: 0x040007F1 RID: 2033
		public NativeList<int3> newTriangleList;
	}
}
