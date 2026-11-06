using System;
using System.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000C9 RID: 201
	public class VirtualMeshManager : IManager, IDisposable, IValid
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000389 RID: 905 RVA: 0x000253F3 File Offset: 0x000235F3
		public int ProxyVertexCount
		{
			get
			{
				ExNativeArray<short> exNativeArray = this.teamIds;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00025406 File Offset: 0x00023606
		public int ProxyTriangleCount
		{
			get
			{
				ExNativeArray<int3> exNativeArray = this.triangles;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00025419 File Offset: 0x00023619
		public int ProxyEdgeCount
		{
			get
			{
				ExNativeArray<int2> exNativeArray = this.edges;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0002542C File Offset: 0x0002362C
		public int ProxyBaseLineCount
		{
			get
			{
				ExNativeArray<ExBitFlag8> exNativeArray = this.baseLineFlags;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0002543F File Offset: 0x0002363F
		public int ProxyLocalPositionCount
		{
			get
			{
				ExNativeArray<float3> exNativeArray = this.localPositions;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00025452 File Offset: 0x00023652
		public int MappingVertexCount
		{
			get
			{
				ExNativeArray<short> exNativeArray = this.mappingIdArray;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00025468 File Offset: 0x00023668
		public void Dispose()
		{
			this.isValid = false;
			ExNativeArray<short> exNativeArray = this.teamIds;
			if (exNativeArray != null)
			{
				exNativeArray.Dispose();
			}
			ExNativeArray<VertexAttribute> exNativeArray2 = this.attributes;
			if (exNativeArray2 != null)
			{
				exNativeArray2.Dispose();
			}
			ExNativeArray<FixedList32Bytes<uint>> exNativeArray3 = this.vertexToTriangles;
			if (exNativeArray3 != null)
			{
				exNativeArray3.Dispose();
			}
			ExNativeArray<float3> exNativeArray4 = this.vertexBindPosePositions;
			if (exNativeArray4 != null)
			{
				exNativeArray4.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray5 = this.vertexBindPoseRotations;
			if (exNativeArray5 != null)
			{
				exNativeArray5.Dispose();
			}
			ExNativeArray<float> exNativeArray6 = this.vertexDepths;
			if (exNativeArray6 != null)
			{
				exNativeArray6.Dispose();
			}
			ExNativeArray<int> exNativeArray7 = this.vertexRootIndices;
			if (exNativeArray7 != null)
			{
				exNativeArray7.Dispose();
			}
			ExNativeArray<float3> exNativeArray8 = this.vertexLocalPositions;
			if (exNativeArray8 != null)
			{
				exNativeArray8.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray9 = this.vertexLocalRotations;
			if (exNativeArray9 != null)
			{
				exNativeArray9.Dispose();
			}
			ExNativeArray<int> exNativeArray10 = this.vertexParentIndices;
			if (exNativeArray10 != null)
			{
				exNativeArray10.Dispose();
			}
			ExNativeArray<uint> exNativeArray11 = this.vertexChildIndexArray;
			if (exNativeArray11 != null)
			{
				exNativeArray11.Dispose();
			}
			ExNativeArray<ushort> exNativeArray12 = this.vertexChildDataArray;
			if (exNativeArray12 != null)
			{
				exNativeArray12.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray13 = this.normalAdjustmentRotations;
			if (exNativeArray13 != null)
			{
				exNativeArray13.Dispose();
			}
			ExNativeArray<float2> exNativeArray14 = this.uv;
			if (exNativeArray14 != null)
			{
				exNativeArray14.Dispose();
			}
			this.teamIds = null;
			this.attributes = null;
			this.vertexToTriangles = null;
			this.vertexBindPosePositions = null;
			this.vertexBindPoseRotations = null;
			this.vertexDepths = null;
			this.vertexRootIndices = null;
			this.vertexLocalPositions = null;
			this.vertexLocalRotations = null;
			this.vertexParentIndices = null;
			this.vertexChildIndexArray = null;
			this.vertexChildDataArray = null;
			this.normalAdjustmentRotations = null;
			this.uv = null;
			ExNativeArray<short> exNativeArray15 = this.triangleTeamIdArray;
			if (exNativeArray15 != null)
			{
				exNativeArray15.Dispose();
			}
			ExNativeArray<int3> exNativeArray16 = this.triangles;
			if (exNativeArray16 != null)
			{
				exNativeArray16.Dispose();
			}
			ExNativeArray<float3> exNativeArray17 = this.triangleNormals;
			if (exNativeArray17 != null)
			{
				exNativeArray17.Dispose();
			}
			ExNativeArray<float3> exNativeArray18 = this.triangleTangents;
			if (exNativeArray18 != null)
			{
				exNativeArray18.Dispose();
			}
			this.triangleTeamIdArray = null;
			this.triangles = null;
			this.triangleNormals = null;
			this.triangleTangents = null;
			ExNativeArray<short> exNativeArray19 = this.edgeTeamIdArray;
			if (exNativeArray19 != null)
			{
				exNativeArray19.Dispose();
			}
			ExNativeArray<int2> exNativeArray20 = this.edges;
			if (exNativeArray20 != null)
			{
				exNativeArray20.Dispose();
			}
			ExNativeArray<ExBitFlag8> exNativeArray21 = this.edgeFlags;
			if (exNativeArray21 != null)
			{
				exNativeArray21.Dispose();
			}
			this.edgeTeamIdArray = null;
			this.edges = null;
			this.edgeFlags = null;
			ExNativeArray<float3> exNativeArray22 = this.positions;
			if (exNativeArray22 != null)
			{
				exNativeArray22.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray23 = this.rotations;
			if (exNativeArray23 != null)
			{
				exNativeArray23.Dispose();
			}
			this.positions = null;
			this.rotations = null;
			ExNativeArray<ExBitFlag8> exNativeArray24 = this.baseLineFlags;
			if (exNativeArray24 != null)
			{
				exNativeArray24.Dispose();
			}
			ExNativeArray<short> exNativeArray25 = this.baseLineTeamIds;
			if (exNativeArray25 != null)
			{
				exNativeArray25.Dispose();
			}
			ExNativeArray<ushort> exNativeArray26 = this.baseLineStartDataIndices;
			if (exNativeArray26 != null)
			{
				exNativeArray26.Dispose();
			}
			ExNativeArray<ushort> exNativeArray27 = this.baseLineDataCounts;
			if (exNativeArray27 != null)
			{
				exNativeArray27.Dispose();
			}
			ExNativeArray<ushort> exNativeArray28 = this.baseLineData;
			if (exNativeArray28 != null)
			{
				exNativeArray28.Dispose();
			}
			this.baseLineFlags = null;
			this.baseLineTeamIds = null;
			this.baseLineStartDataIndices = null;
			this.baseLineDataCounts = null;
			this.baseLineData = null;
			ExNativeArray<float3> exNativeArray29 = this.localPositions;
			if (exNativeArray29 != null)
			{
				exNativeArray29.Dispose();
			}
			ExNativeArray<float3> exNativeArray30 = this.localNormals;
			if (exNativeArray30 != null)
			{
				exNativeArray30.Dispose();
			}
			ExNativeArray<float3> exNativeArray31 = this.localTangents;
			if (exNativeArray31 != null)
			{
				exNativeArray31.Dispose();
			}
			ExNativeArray<VirtualMeshBoneWeight> exNativeArray32 = this.boneWeights;
			if (exNativeArray32 != null)
			{
				exNativeArray32.Dispose();
			}
			ExNativeArray<int> exNativeArray33 = this.skinBoneTransformIndices;
			if (exNativeArray33 != null)
			{
				exNativeArray33.Dispose();
			}
			ExNativeArray<float4x4> exNativeArray34 = this.skinBoneBindPoses;
			if (exNativeArray34 != null)
			{
				exNativeArray34.Dispose();
			}
			this.localPositions = null;
			this.localNormals = null;
			this.localTangents = null;
			this.boneWeights = null;
			this.skinBoneTransformIndices = null;
			this.skinBoneBindPoses = null;
			ExNativeArray<quaternion> exNativeArray35 = this.vertexToTransformRotations;
			if (exNativeArray35 != null)
			{
				exNativeArray35.Dispose();
			}
			this.vertexToTransformRotations = null;
			ExNativeArray<short> exNativeArray36 = this.mappingIdArray;
			if (exNativeArray36 != null)
			{
				exNativeArray36.Dispose();
			}
			ExNativeArray<int> exNativeArray37 = this.mappingReferenceIndices;
			if (exNativeArray37 != null)
			{
				exNativeArray37.Dispose();
			}
			ExNativeArray<VertexAttribute> exNativeArray38 = this.mappingAttributes;
			if (exNativeArray38 != null)
			{
				exNativeArray38.Dispose();
			}
			ExNativeArray<float3> exNativeArray39 = this.mappingLocalPositins;
			if (exNativeArray39 != null)
			{
				exNativeArray39.Dispose();
			}
			ExNativeArray<float3> exNativeArray40 = this.mappingLocalNormals;
			if (exNativeArray40 != null)
			{
				exNativeArray40.Dispose();
			}
			ExNativeArray<float3> exNativeArray41 = this.mappingLocalTangents;
			if (exNativeArray41 != null)
			{
				exNativeArray41.Dispose();
			}
			ExNativeArray<VirtualMeshBoneWeight> exNativeArray42 = this.mappingBoneWeights;
			if (exNativeArray42 != null)
			{
				exNativeArray42.Dispose();
			}
			this.mappingIdArray = null;
			this.mappingReferenceIndices = null;
			this.mappingAttributes = null;
			this.mappingLocalPositins = null;
			this.mappingLocalNormals = null;
			this.mappingLocalTangents = null;
			this.mappingBoneWeights = null;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0002586C File Offset: 0x00023A6C
		public void EnterdEditMode()
		{
			this.Dispose();
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00025874 File Offset: 0x00023A74
		public void Initialize()
		{
			this.Dispose();
			this.teamIds = new ExNativeArray<short>(0, true);
			this.attributes = new ExNativeArray<VertexAttribute>(0, true);
			this.vertexToTriangles = new ExNativeArray<FixedList32Bytes<uint>>(0, true);
			this.vertexBindPosePositions = new ExNativeArray<float3>(0, true);
			this.vertexBindPoseRotations = new ExNativeArray<quaternion>(0, true);
			this.vertexDepths = new ExNativeArray<float>(0, true);
			this.vertexRootIndices = new ExNativeArray<int>(0, true);
			this.vertexLocalPositions = new ExNativeArray<float3>(0, true);
			this.vertexLocalRotations = new ExNativeArray<quaternion>(0, true);
			this.vertexParentIndices = new ExNativeArray<int>(0, true);
			this.vertexChildIndexArray = new ExNativeArray<uint>(0, true);
			this.vertexChildDataArray = new ExNativeArray<ushort>(0, true);
			this.normalAdjustmentRotations = new ExNativeArray<quaternion>(0, true);
			this.uv = new ExNativeArray<float2>(0, true);
			this.triangleTeamIdArray = new ExNativeArray<short>(0, true);
			this.triangles = new ExNativeArray<int3>(0, true);
			this.triangleNormals = new ExNativeArray<float3>(0, true);
			this.triangleTangents = new ExNativeArray<float3>(0, true);
			this.edgeTeamIdArray = new ExNativeArray<short>(0, true);
			this.edges = new ExNativeArray<int2>(0, true);
			this.edgeFlags = new ExNativeArray<ExBitFlag8>(0, true);
			this.positions = new ExNativeArray<float3>(0, true);
			this.rotations = new ExNativeArray<quaternion>(0, true);
			this.baseLineFlags = new ExNativeArray<ExBitFlag8>(0, true);
			this.baseLineTeamIds = new ExNativeArray<short>(0, true);
			this.baseLineStartDataIndices = new ExNativeArray<ushort>(0, true);
			this.baseLineDataCounts = new ExNativeArray<ushort>(0, true);
			this.baseLineData = new ExNativeArray<ushort>(0, true);
			this.localPositions = new ExNativeArray<float3>(0, true);
			this.localNormals = new ExNativeArray<float3>(0, true);
			this.localTangents = new ExNativeArray<float3>(0, true);
			this.boneWeights = new ExNativeArray<VirtualMeshBoneWeight>(0, true);
			this.skinBoneTransformIndices = new ExNativeArray<int>(0, true);
			this.skinBoneBindPoses = new ExNativeArray<float4x4>(0, true);
			this.vertexToTransformRotations = new ExNativeArray<quaternion>(0, true);
			this.mappingIdArray = new ExNativeArray<short>(0, true);
			this.mappingReferenceIndices = new ExNativeArray<int>(0, true);
			this.mappingAttributes = new ExNativeArray<VertexAttribute>(0, true);
			this.mappingLocalPositins = new ExNativeArray<float3>(0, true);
			this.mappingLocalNormals = new ExNativeArray<float3>(0, true);
			this.mappingLocalTangents = new ExNativeArray<float3>(0, true);
			this.mappingBoneWeights = new ExNativeArray<VirtualMeshBoneWeight>(0, true);
			this.isValid = true;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00025AB0 File Offset: 0x00023CB0
		public bool IsValid()
		{
			return this.isValid;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00025AB8 File Offset: 0x00023CB8
		public void RegisterProxyMesh(int teamId, VirtualMeshContainer proxyMeshContainer)
		{
			if (!this.isValid)
			{
				return;
			}
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(teamId);
			VirtualMesh shareVirtualMesh = proxyMeshContainer.shareVirtualMesh;
			teamDataRef.proxyMeshType = shareVirtualMesh.meshType;
			teamDataRef.proxyTransformChunk = MagicaManager.Bone.AddTransform(proxyMeshContainer, teamId);
			teamDataRef.centerTransformIndex = shareVirtualMesh.centerTransformIndex + teamDataRef.proxyTransformChunk.startIndex;
			int vertexCount = shareVirtualMesh.VertexCount;
			teamDataRef.proxyCommonChunk = this.teamIds.AddRange(vertexCount, (short)teamId);
			this.attributes.AddRange(shareVirtualMesh.attributes);
			this.vertexToTriangles.AddRange<FixedList32Bytes<uint>>(shareVirtualMesh.vertexToTriangles);
			this.vertexBindPosePositions.AddRange<float3>(shareVirtualMesh.vertexBindPosePositions);
			this.vertexBindPoseRotations.AddRange<quaternion>(shareVirtualMesh.vertexBindPoseRotations);
			this.vertexDepths.AddRange<float>(shareVirtualMesh.vertexDepths);
			this.vertexRootIndices.AddRange<int>(shareVirtualMesh.vertexRootIndices);
			this.vertexLocalPositions.AddRange<float3>(shareVirtualMesh.vertexLocalPositions);
			this.vertexLocalRotations.AddRange<quaternion>(shareVirtualMesh.vertexLocalRotations);
			this.vertexParentIndices.AddRange<int>(shareVirtualMesh.vertexParentIndices);
			this.vertexChildIndexArray.AddRange<uint>(shareVirtualMesh.vertexChildIndexArray);
			this.normalAdjustmentRotations.AddRange<quaternion>(shareVirtualMesh.normalAdjustmentRotations);
			this.uv.AddRange(shareVirtualMesh.uv);
			this.positions.AddRange(vertexCount);
			this.rotations.AddRange(vertexCount);
			teamDataRef.proxyVertexChildDataChunk = this.vertexChildDataArray.AddRange<ushort>(shareVirtualMesh.vertexChildDataArray);
			if (shareVirtualMesh.TriangleCount > 0)
			{
				teamDataRef.proxyTriangleChunk = this.triangleTeamIdArray.AddRange(shareVirtualMesh.TriangleCount, (short)teamId);
				this.triangles.AddRange(shareVirtualMesh.triangles);
				this.triangleNormals.AddRange(shareVirtualMesh.TriangleCount);
				this.triangleTangents.AddRange(shareVirtualMesh.TriangleCount);
			}
			if (shareVirtualMesh.EdgeCount > 0)
			{
				teamDataRef.proxyEdgeChunk = this.edgeTeamIdArray.AddRange(shareVirtualMesh.EdgeCount, (short)teamId);
				this.edges.AddRange<int2>(shareVirtualMesh.edges);
				this.edgeFlags.AddRange<ExBitFlag8>(shareVirtualMesh.edgeFlags);
			}
			if (shareVirtualMesh.BaseLineCount > 0)
			{
				teamDataRef.baseLineChunk = this.baseLineFlags.AddRange<ExBitFlag8>(shareVirtualMesh.baseLineFlags);
				this.baseLineStartDataIndices.AddRange<ushort>(shareVirtualMesh.baseLineStartDataIndices);
				this.baseLineDataCounts.AddRange<ushort>(shareVirtualMesh.baseLineDataCounts);
				this.baseLineTeamIds.AddRange(shareVirtualMesh.BaseLineCount, (short)teamId);
				teamDataRef.baseLineDataChunk = this.baseLineData.AddRange<ushort>(shareVirtualMesh.baseLineData);
			}
			teamDataRef.proxyMeshChunk = this.localPositions.AddRange(shareVirtualMesh.localPositions);
			this.localNormals.AddRange(shareVirtualMesh.localNormals);
			this.localTangents.AddRange(shareVirtualMesh.localTangents);
			this.boneWeights.AddRange(shareVirtualMesh.boneWeights);
			teamDataRef.proxySkinBoneChunk = this.skinBoneTransformIndices.AddRange(shareVirtualMesh.skinBoneTransformIndices);
			this.skinBoneBindPoses.AddRange(shareVirtualMesh.skinBoneBindPoses);
			if (shareVirtualMesh.meshType == VirtualMesh.MeshType.ProxyBoneMesh)
			{
				teamDataRef.proxyBoneChunk = this.vertexToTransformRotations.AddRange<quaternion>(shareVirtualMesh.vertexToTransformRotations);
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00025DE0 File Offset: 0x00023FE0
		public void ExitProxyMesh(int teamId)
		{
			if (!this.isValid)
			{
				return;
			}
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(teamId);
			MagicaManager.Bone.RemoveTransform(teamDataRef.proxyTransformChunk);
			teamDataRef.proxyTransformChunk.Clear();
			this.teamIds.RemoveAndFill(teamDataRef.proxyCommonChunk, 0);
			this.attributes.RemoveAndFill(teamDataRef.proxyCommonChunk, default(VertexAttribute));
			this.vertexToTriangles.Remove(teamDataRef.proxyCommonChunk);
			this.vertexBindPosePositions.Remove(teamDataRef.proxyCommonChunk);
			this.vertexBindPoseRotations.Remove(teamDataRef.proxyCommonChunk);
			this.vertexDepths.Remove(teamDataRef.proxyCommonChunk);
			this.vertexRootIndices.Remove(teamDataRef.proxyCommonChunk);
			this.vertexLocalPositions.Remove(teamDataRef.proxyCommonChunk);
			this.vertexLocalRotations.Remove(teamDataRef.proxyCommonChunk);
			this.vertexParentIndices.Remove(teamDataRef.proxyCommonChunk);
			this.vertexChildIndexArray.Remove(teamDataRef.proxyCommonChunk);
			this.normalAdjustmentRotations.Remove(teamDataRef.proxyCommonChunk);
			this.uv.Remove(teamDataRef.proxyCommonChunk);
			this.positions.Remove(teamDataRef.proxyCommonChunk);
			this.rotations.Remove(teamDataRef.proxyCommonChunk);
			teamDataRef.proxyCommonChunk.Clear();
			this.vertexChildDataArray.Remove(teamDataRef.proxyVertexChildDataChunk);
			teamDataRef.proxyVertexChildDataChunk.Clear();
			this.triangleTeamIdArray.RemoveAndFill(teamDataRef.proxyTriangleChunk, 0);
			this.triangles.Remove(teamDataRef.proxyTriangleChunk);
			this.triangleNormals.Remove(teamDataRef.proxyTriangleChunk);
			this.triangleTangents.Remove(teamDataRef.proxyTriangleChunk);
			teamDataRef.proxyTriangleChunk.Clear();
			this.edgeTeamIdArray.RemoveAndFill(teamDataRef.proxyEdgeChunk, 0);
			this.edges.Remove(teamDataRef.proxyEdgeChunk);
			this.edgeFlags.Remove(teamDataRef.proxyEdgeChunk);
			this.baseLineFlags.RemoveAndFill(teamDataRef.baseLineChunk, default(ExBitFlag8));
			this.baseLineTeamIds.Remove(teamDataRef.baseLineChunk);
			this.baseLineStartDataIndices.Remove(teamDataRef.baseLineChunk);
			this.baseLineDataCounts.Remove(teamDataRef.baseLineChunk);
			teamDataRef.baseLineChunk.Clear();
			this.baseLineData.Remove(teamDataRef.baseLineDataChunk);
			teamDataRef.baseLineDataChunk.Clear();
			this.localPositions.Remove(teamDataRef.proxyMeshChunk);
			this.localNormals.Remove(teamDataRef.proxyMeshChunk);
			this.localTangents.Remove(teamDataRef.proxyMeshChunk);
			this.boneWeights.Remove(teamDataRef.proxyMeshChunk);
			teamDataRef.proxyMeshChunk.Clear();
			this.skinBoneTransformIndices.Remove(teamDataRef.proxySkinBoneChunk);
			this.skinBoneBindPoses.Remove(teamDataRef.proxySkinBoneChunk);
			teamDataRef.proxySkinBoneChunk.Clear();
			this.vertexToTransformRotations.Remove(teamDataRef.proxyBoneChunk);
			teamDataRef.proxyBoneChunk.Clear();
			FixedList64Bytes<short> fixedList64Bytes = MagicaManager.Team.teamMappingIndexArray[teamId];
			int length = fixedList64Bytes.Length;
			for (int i = 0; i < length; i++)
			{
				int num = (int)fixedList64Bytes[i];
				this.ExitMappingMesh(teamId, num);
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0002611C File Offset: 0x0002431C
		public DataChunk RegisterMappingMesh(int teamId, VirtualMeshContainer mappingMeshContainer, int renderDataWorkIndex)
		{
			if (!this.isValid)
			{
				return DataChunk.Empty;
			}
			MagicaManager.Team.GetTeamDataRef(teamId);
			ref FixedList64Bytes<short> teamMappingRef = ref MagicaManager.Team.GetTeamMappingRef(teamId);
			TeamManager.MappingData mappingData = default(TeamManager.MappingData);
			mappingData.teamId = teamId;
			VirtualMesh shareVirtualMesh = mappingMeshContainer.shareVirtualMesh;
			Transform centerTransform = mappingMeshContainer.GetCenterTransform();
			DataChunk dataChunk = MagicaManager.Bone.AddTransform(centerTransform, new ExBitFlag8(17), teamId);
			mappingData.centerTransformIndex = dataChunk.startIndex;
			mappingData.toProxyMatrix = shareVirtualMesh.toProxyMatrix;
			mappingData.toProxyRotation = shareVirtualMesh.toProxyRotation;
			dataChunk = MagicaManager.Team.mappingDataArray.Add(mappingData);
			int startIndex = dataChunk.startIndex;
			int vertexCount = shareVirtualMesh.VertexCount;
			mappingData.mappingCommonChunk = this.mappingIdArray.AddRange(vertexCount, (short)(startIndex + 1));
			this.mappingReferenceIndices.AddRange(shareVirtualMesh.referenceIndices);
			this.mappingAttributes.AddRange(shareVirtualMesh.attributes);
			this.mappingLocalPositins.AddRange(shareVirtualMesh.localPositions);
			this.mappingLocalNormals.AddRange(shareVirtualMesh.localNormals);
			this.mappingLocalTangents.AddRange(shareVirtualMesh.localTangents);
			this.mappingBoneWeights.AddRange(shareVirtualMesh.boneWeights);
			mappingData.renderDataWorkIndex = renderDataWorkIndex;
			MagicaManager.Render.GetRenderDataWorkRef(renderDataWorkIndex).AddMappingIndex(startIndex);
			MagicaManager.Team.mappingDataArray[startIndex] = mappingData;
			(ref teamMappingRef).MC2Set((short)startIndex);
			shareVirtualMesh.mappingId = startIndex;
			return mappingData.mappingCommonChunk;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00026290 File Offset: 0x00024490
		public void ExitMappingMesh(int teamId, int mappingIndex)
		{
			if (!this.isValid)
			{
				return;
			}
			MagicaManager.Team.GetTeamDataRef(teamId);
			ref TeamManager.MappingData @ref = ref MagicaManager.Team.mappingDataArray.GetRef(mappingIndex);
			ref FixedList64Bytes<short> teamMappingRef = ref MagicaManager.Team.GetTeamMappingRef(teamId);
			MagicaManager.Bone.RemoveTransform(new DataChunk(@ref.centerTransformIndex, 1));
			this.mappingIdArray.RemoveAndFill(@ref.mappingCommonChunk, 0);
			this.mappingReferenceIndices.Remove(@ref.mappingCommonChunk);
			this.mappingAttributes.Remove(@ref.mappingCommonChunk);
			this.mappingLocalPositins.Remove(@ref.mappingCommonChunk);
			this.mappingLocalNormals.Remove(@ref.mappingCommonChunk);
			this.mappingLocalTangents.Remove(@ref.mappingCommonChunk);
			this.mappingBoneWeights.Remove(@ref.mappingCommonChunk);
			MagicaManager.Render.GetRenderDataWorkRef(@ref.renderDataWorkIndex).RemoveMappingIndex(mappingIndex);
			(ref teamMappingRef).MC2RemoveItemAtSwapBack((short)mappingIndex);
			MagicaManager.Team.mappingDataArray.RemoveAndFill(new DataChunk(mappingIndex, 1), default(TeamManager.MappingData));
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00026398 File Offset: 0x00024598
		internal static void SimulationPreProxyMeshUpdate(DataChunk chunk, int teamId, ref TeamManager.TeamData tdata, in NativeArray<VertexAttribute> attributes, in NativeArray<float3> localPositions, in NativeArray<float3> localNormals, in NativeArray<float3> localTangents, in NativeArray<VirtualMeshBoneWeight> boneWeights, in NativeArray<int> skinBoneTransformIndices, in NativeArray<float4x4> skinBoneBindPoses, ref NativeArray<float3> positions, ref NativeArray<quaternion> rotations, in NativeArray<float4x4> transformLocalToWorldMatrixArray)
		{
			DataChunk proxyCommonChunk = tdata.proxyCommonChunk;
			if (proxyCommonChunk.dataLength == 0)
			{
				return;
			}
			int num = proxyCommonChunk.startIndex + chunk.startIndex;
			int num2 = tdata.proxyMeshChunk.startIndex + chunk.startIndex;
			int startIndex = tdata.proxySkinBoneChunk.startIndex;
			int startIndex2 = tdata.proxyTransformChunk.startIndex;
			float4x3 float4x = 0;
			float4x3 float4x2 = 0;
			float4x3 float4x3 = 0;
			int i = 0;
			while (i < chunk.dataLength)
			{
				NativeArray<VirtualMeshBoneWeight> nativeArray = boneWeights;
				VirtualMeshBoneWeight virtualMeshBoneWeight = nativeArray[num2];
				int count = virtualMeshBoneWeight.Count;
				float4x = 0;
				float4x2 = 0;
				NativeArray<float3> nativeArray2 = localPositions;
				float4x3.c0 = new float4(nativeArray2[num2], 1f);
				nativeArray2 = localNormals;
				float4x3.c1 = new float4(nativeArray2[num2], 0f);
				nativeArray2 = localTangents;
				float4x3.c2 = new float4(nativeArray2[num2], 0f);
				for (int j = 0; j < count; j++)
				{
					float num3 = virtualMeshBoneWeight.weights[j];
					float4x2 = float4x3;
					int num4 = virtualMeshBoneWeight.boneIndices[j];
					NativeArray<float4x4> nativeArray3 = skinBoneBindPoses;
					float4x2 = math.mul(nativeArray3[startIndex + num4], float4x2);
					NativeArray<int> nativeArray4 = skinBoneTransformIndices;
					int num5 = nativeArray4[startIndex + num4] + startIndex2;
					nativeArray3 = transformLocalToWorldMatrixArray;
					float4x2 = math.mul(nativeArray3[num5], float4x2);
					float4x += float4x2 * num3;
				}
				float3 xyz = float4x.c0.xyz;
				float3 @float = float4x.c1.xyz;
				float3 float2 = float4x.c2.xyz;
				@float = math.normalize(@float);
				float2 = math.normalize(float2);
				quaternion quaternion = MathUtility.ToRotation(in @float, in float2);
				positions[num] = xyz;
				rotations[num] = quaternion;
				i++;
				num++;
				num2++;
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000265B8 File Offset: 0x000247B8
		internal static void SimulationPostProxyMeshUpdateLine(DataChunk chunk, ref TeamManager.TeamData tdata, ref ClothParameters param, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float3> positions, ref NativeArray<quaternion> rotations, ref NativeArray<float3> vertexLocalPositions, ref NativeArray<quaternion> vertexLocalRotations, ref NativeArray<uint> childIndexArray, ref NativeArray<ushort> childDataArray, ref NativeArray<ExBitFlag8> baseLineFlags, ref NativeArray<ushort> baseLineStartIndices, ref NativeArray<ushort> baseLineDataCounts, ref NativeArray<ushort> baseLineData)
		{
			if (tdata.baseLineChunk.IsValid)
			{
				float rotationalInterpolation = param.rotationalInterpolation;
				float rootRotation = param.rootRotation;
				int startIndex = tdata.proxyCommonChunk.startIndex;
				int startIndex2 = tdata.baseLineDataChunk.startIndex;
				int startIndex3 = tdata.proxyVertexChildDataChunk.startIndex;
				int num = tdata.baseLineChunk.startIndex + chunk.startIndex;
				int i = 0;
				while (i < chunk.dataLength)
				{
					if (baseLineFlags[num].IsSet(1))
					{
						int num2 = (int)baseLineStartIndices[num] + startIndex2;
						int num3 = (int)baseLineDataCounts[num];
						int j = 0;
						while (j < num3)
						{
							int num4 = (int)baseLineData[num2] + startIndex;
							float3 @float = positions[num4];
							quaternion quaternion = rotations[num4];
							VertexAttribute vertexAttribute = attributes[num4];
							uint num5 = childIndexArray[num4];
							int num6 = DataUtility.Unpack12_20Low(num5);
							int num7 = DataUtility.Unpack12_20Hi(num5);
							int num8 = 0;
							if (num7 > 0)
							{
								float3 float2 = 0;
								float3 float3 = 0;
								for (int k = 0; k < num7; k++)
								{
									int num9 = (int)childDataArray[startIndex3 + num6 + k] + startIndex;
									VertexAttribute vertexAttribute2 = attributes[num9];
									float3 float4 = positions[num9];
									float3 float5 = math.mul(quaternion, vertexLocalPositions[num9] * tdata.negativeScaleDirection);
									float2 += float5;
									if (vertexAttribute2.IsMove())
									{
										float3 float6 = float4 - @float;
										float3 += float6;
										quaternion quaternion2 = MathUtility.FromToRotation(in float5, in float6, 1f);
										quaternion quaternion3 = math.mul(quaternion, vertexLocalRotations[num9].value * tdata.negativeScaleQuaternionValue);
										quaternion3 = math.mul(quaternion2, quaternion3);
										rotations[num9] = quaternion3;
										num8++;
									}
									else
									{
										float3 += float5;
									}
								}
								if (num8 != 0)
								{
									float num10 = (vertexAttribute.IsMove() ? rotationalInterpolation : rootRotation);
									quaternion = math.mul(MathUtility.FromToRotation(in float2, in float3, num10), quaternion);
									rotations[num4] = quaternion;
								}
							}
							j++;
							num2++;
						}
					}
					i++;
					num++;
				}
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00026800 File Offset: 0x00024A00
		internal static void SimulationPostProxyMeshUpdateTriangle(DataChunk chunk, ref TeamManager.TeamData tdata, ref NativeArray<float3> positions, ref NativeArray<int3> triangles, ref NativeArray<float3> triangleNormals, ref NativeArray<float3> triangleTangents, ref NativeArray<float2> uvs)
		{
			if (tdata.TriangleCount > 0)
			{
				int num = tdata.proxyTriangleChunk.startIndex + chunk.startIndex;
				int i = 0;
				while (i < chunk.dataLength)
				{
					int3 @int = triangles[num];
					int startIndex = tdata.proxyCommonChunk.startIndex;
					float3 @float = positions[startIndex + @int.x];
					float3 float2 = positions[startIndex + @int.y];
					float3 float3 = positions[startIndex + @int.z];
					float3 float4 = math.cross(float2 - @float, float3 - @float);
					float num2 = math.length(float4);
					if (num2 > 1E-08f)
					{
						float3 float5 = float4 / num2;
						float5 *= tdata.negativeScaleTriangleSign.x;
						triangleNormals[num] = float5;
					}
					float2 float6 = uvs[startIndex + @int.x];
					float2 float7 = uvs[startIndex + @int.y];
					float2 float8 = uvs[startIndex + @int.z];
					float3 float9 = MathUtility.TriangleTangent(in @float, in float2, in float3, in float6, in float7, in float8);
					if (math.lengthsq(float9) > 0f)
					{
						float9 *= tdata.negativeScaleTriangleSign.y;
						triangleTangents[num] = float9;
					}
					i++;
					num++;
				}
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00026954 File Offset: 0x00024B54
		internal static void SimulationPostProxyMeshUpdateTriangleSum(DataChunk chunk, ref TeamManager.TeamData tdata, ref NativeArray<quaternion> rotations, ref NativeArray<float3> triangleNormals, ref NativeArray<float3> triangleTangents, ref NativeArray<FixedList32Bytes<uint>> vertexToTriangles, ref NativeArray<quaternion> normalAdjustmentRotations)
		{
			if (tdata.TriangleCount > 0)
			{
				int num = tdata.proxyCommonChunk.startIndex + chunk.startIndex;
				int i = 0;
				while (i < chunk.dataLength)
				{
					FixedList32Bytes<uint> fixedList32Bytes = vertexToTriangles[num];
					if (fixedList32Bytes.Length > 0)
					{
						float3 @float = 0;
						float3 float2 = 0;
						for (int j = 0; j < fixedList32Bytes.Length; j++)
						{
							uint num2 = fixedList32Bytes[j];
							int num3 = DataUtility.Unpack12_20Hi(num2);
							int num4 = DataUtility.Unpack12_20Low(num2);
							num4 += tdata.proxyTriangleChunk.startIndex;
							@float += triangleNormals[num4] * (float)(((num3 & 1) == 0) ? 1 : (-1));
							float2 += triangleTangents[num4] * (float)(((num3 & 2) == 0) ? 1 : (-1));
						}
						float num5 = math.length(@float);
						float num6 = math.length(float2);
						if (num5 > 1E-06f && num6 > 1E-06f)
						{
							@float /= num5;
							float2 /= num6;
							float num7 = math.dot(@float, float2);
							if (num7 != 1f && num7 != -1f)
							{
								quaternion quaternion = quaternion.LookRotation(math.normalize(math.cross(@float, float2)), @float);
								quaternion = math.mul(quaternion, normalAdjustmentRotations[num].value * tdata.negativeScaleQuaternionValue);
								rotations[num] = quaternion;
							}
						}
					}
					i++;
					num++;
				}
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00026AD8 File Offset: 0x00024CD8
		internal static void SimulationPostProxyMeshUpdateWorldTransform(DataChunk chunk, ref TeamManager.TeamData tdata, ref NativeArray<float3> positions, ref NativeArray<quaternion> rotations, ref NativeArray<quaternion> vertexToTransformRotations, ref NativeArray<float3> transformPositionArray, ref NativeArray<quaternion> transformRotationArray)
		{
			if (tdata.proxyMeshType == VirtualMesh.MeshType.ProxyBoneMesh)
			{
				int num = tdata.proxyCommonChunk.startIndex + chunk.startIndex;
				int i = 0;
				while (i < chunk.dataLength)
				{
					float3 @float = positions[num];
					quaternion quaternion = rotations[num];
					int num2 = tdata.proxyBoneChunk.startIndex + chunk.startIndex + i;
					quaternion quaternion2 = vertexToTransformRotations[num2];
					quaternion = math.mul(quaternion, quaternion2.value * tdata.negativeScaleQuaternionValue);
					int num3 = tdata.proxyTransformChunk.startIndex + chunk.startIndex + i;
					transformPositionArray[num3] = @float;
					transformRotationArray[num3] = quaternion;
					i++;
					num++;
				}
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00026B98 File Offset: 0x00024D98
		internal static void SimulationPostProxyMeshUpdateLocalTransform(ref TeamManager.TeamData tdata, ref NativeArray<VertexAttribute> attributes, ref NativeArray<int> parentIndices, ref NativeArray<float3> transformPositionArray, ref NativeArray<quaternion> transformRotationArray, ref NativeArray<float3> transformScaleArray, ref NativeArray<float3> transformLocalPositionArray, ref NativeArray<quaternion> transformLocalRotationArray)
		{
			if (tdata.proxyMeshType == VirtualMesh.MeshType.ProxyBoneMesh)
			{
				int num = tdata.proxyCommonChunk.startIndex;
				int i = 0;
				while (i < tdata.proxyCommonChunk.dataLength)
				{
					int num2 = parentIndices[num];
					if (num2 >= 0 && attributes[num].IsMove())
					{
						int num3 = tdata.proxyTransformChunk.startIndex + i;
						int num4 = tdata.proxyTransformChunk.startIndex + num2;
						float3 @float = transformPositionArray[num4];
						quaternion quaternion = transformRotationArray[num4];
						float3 float2 = transformScaleArray[num4];
						float3 float3 = transformPositionArray[num3];
						quaternion quaternion2 = transformRotationArray[num3];
						quaternion quaternion3 = math.inverse(quaternion);
						float3 float4 = float3 - @float;
						float3 float5 = math.mul(quaternion3, float4);
						float5 /= float2;
						quaternion quaternion4 = math.mul(quaternion3, quaternion2);
						quaternion4 = quaternion4.value * tdata.negativeScaleQuaternionValue;
						transformLocalPositionArray[num3] = float5;
						transformLocalRotationArray[num3] = quaternion4;
					}
					i++;
					num++;
				}
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00026CB0 File Offset: 0x00024EB0
		internal JobHandle PostMappingMeshUpdateBatchSchedule(JobHandle jobHandle, int workerCount)
		{
			if (MagicaManager.Team.MappingCount == 0)
			{
				return jobHandle;
			}
			TeamManager team = MagicaManager.Team;
			TransformManager bone = MagicaManager.Bone;
			RenderManager render = MagicaManager.Render;
			jobHandle = new VirtualMeshManager.CalcMeshConvert_A_Job
			{
				teamDataArray = team.teamDataArray.GetNativeArray(),
				transformPositionArray = bone.positionArray.GetNativeArray(),
				transformRotationArray = bone.rotationArray.GetNativeArray(),
				transformScaleArray = bone.scaleArray.GetNativeArray(),
				mappingDataArray = team.mappingDataArray.GetNativeArray(),
				renderDataWorkArray = render.renderDataWorkArray.GetNativeArray()
			}.Schedule(team.MappingCount, 1, jobHandle);
			jobHandle = new VirtualMeshManager.CalcMeshConvert_B_Job
			{
				workerCount = workerCount,
				teamDataArray = team.teamDataArray.GetNativeArray(),
				mappingDataArray = team.mappingDataArray.GetNativeArray(),
				mappingAttributes = this.mappingAttributes.GetNativeArray(),
				mappingLocalPositions = this.mappingLocalPositins.GetNativeArray(),
				mappingLocalNormals = this.mappingLocalNormals.GetNativeArray(),
				mappingLocalTangents = this.mappingLocalTangents.GetNativeArray(),
				mappingBoneWeights = this.mappingBoneWeights.GetNativeArray(),
				mappingReferenceIndices = this.mappingReferenceIndices.GetNativeArray(),
				proxyPositions = this.positions.GetNativeArray(),
				proxyRotations = this.rotations.GetNativeArray(),
				proxyVertexBindPosePositions = this.vertexBindPosePositions.GetNativeArray(),
				proxyVertexBindPoseRotations = this.vertexBindPoseRotations.GetNativeArray(),
				renderDataWorkArray = render.renderDataWorkArray.GetNativeArray(),
				renderMeshPositions = render.renderMeshPositions.GetNativeArray(),
				renderMeshNormals = render.renderMeshNormals.GetNativeArray(),
				renderMeshTangents = render.renderMeshTangents.GetNativeArray(),
				renderMeshBoneWeights = render.renderMeshBoneWeights.GetNativeArray()
			}.Schedule(team.MappingCount * workerCount, 1, jobHandle);
			jobHandle = new VirtualMeshManager.PostRenderMeshWorkDataBatchJob
			{
				renderDataWorkArray = render.renderDataWorkArray.GetNativeArray(),
				mappingDataArray = team.mappingDataArray.GetNativeArray()
			}.Schedule(render.RenderDataWorkCount, 8, jobHandle);
			return jobHandle;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00026EF4 File Offset: 0x000250F4
		public void InformationLog(StringBuilder allsb)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("========== VMesh Manager ==========");
			if (!this.IsValid())
			{
				stringBuilder.AppendLine("VirtualMesh Manager. Invalid.");
			}
			else
			{
				stringBuilder.AppendLine("VirtualMesh Manager.");
				stringBuilder.AppendLine(string.Format("  -ProxyVertexCount:{0}", this.ProxyVertexCount));
				stringBuilder.AppendLine(string.Format("  -ProxyEdgeCount:{0}", this.ProxyEdgeCount));
				stringBuilder.AppendLine(string.Format("  -ProxyTriangleCount:{0}", this.ProxyTriangleCount));
				stringBuilder.AppendLine(string.Format("  -ProxyBaseLineCount:{0}", this.ProxyBaseLineCount));
				stringBuilder.AppendLine(string.Format("  -ProxyLocalPositionCount:{0}", this.ProxyLocalPositionCount));
				stringBuilder.AppendLine("  [ProxyMesh]");
				stringBuilder.AppendLine("    -teamIds:" + this.teamIds.ToSummary());
				stringBuilder.AppendLine("    -attributes:" + this.attributes.ToSummary());
				stringBuilder.AppendLine("    -vertexToTriangles:" + this.vertexToTriangles.ToSummary());
				stringBuilder.AppendLine("    -vertexBindPosePositions:" + this.vertexBindPosePositions.ToSummary());
				stringBuilder.AppendLine("    -vertexBindPoseRotations:" + this.vertexBindPoseRotations.ToSummary());
				stringBuilder.AppendLine("    -vertexDepths:" + this.vertexDepths.ToSummary());
				stringBuilder.AppendLine("    -vertexRootIndices:" + this.vertexRootIndices.ToSummary());
				stringBuilder.AppendLine("    -vertexLocalPositions:" + this.vertexLocalPositions.ToSummary());
				stringBuilder.AppendLine("    -vertexLocalRotations:" + this.vertexLocalRotations.ToSummary());
				stringBuilder.AppendLine("    -vertexParentIndices:" + this.vertexParentIndices.ToSummary());
				stringBuilder.AppendLine("    -vertexChildIndexArray:" + this.vertexChildIndexArray.ToSummary());
				stringBuilder.AppendLine("    -vertexChildDataArray:" + this.vertexChildDataArray.ToSummary());
				stringBuilder.AppendLine("    -normalAdjustmentRotations:" + this.normalAdjustmentRotations.ToSummary());
				stringBuilder.AppendLine("    -uv:" + this.uv.ToSummary());
				stringBuilder.AppendLine("    -triangleTeamIdArray:" + this.triangleTeamIdArray.ToSummary());
				stringBuilder.AppendLine("    -triangles:" + this.triangles.ToSummary());
				stringBuilder.AppendLine("    -triangleNormals:" + this.triangleNormals.ToSummary());
				stringBuilder.AppendLine("    -triangleTangents:" + this.triangleTangents.ToSummary());
				stringBuilder.AppendLine("    -edgeTeamIdArray:" + this.edgeTeamIdArray.ToSummary());
				stringBuilder.AppendLine("    -edges:" + this.edges.ToSummary());
				stringBuilder.AppendLine("    -edgeFlags:" + this.edgeFlags.ToSummary());
				stringBuilder.AppendLine("    -baseLineFlags:" + this.baseLineFlags.ToSummary());
				stringBuilder.AppendLine("    -baseLineTeamIds:" + this.baseLineTeamIds.ToSummary());
				stringBuilder.AppendLine("    -baseLineStartDataIndices:" + this.baseLineStartDataIndices.ToSummary());
				stringBuilder.AppendLine("    -baseLineDataCounts:" + this.baseLineDataCounts.ToSummary());
				stringBuilder.AppendLine("    -baseLineData:" + this.baseLineData.ToSummary());
				stringBuilder.AppendLine("  [Mesh Common]");
				stringBuilder.AppendLine("    -localPositions:" + this.localPositions.ToSummary());
				stringBuilder.AppendLine("    -localNormals:" + this.localNormals.ToSummary());
				stringBuilder.AppendLine("    -localTangents:" + this.localTangents.ToSummary());
				stringBuilder.AppendLine("    -boneWeights:" + this.boneWeights.ToSummary());
				stringBuilder.AppendLine("    -skinBoneTransformIndices:" + this.skinBoneTransformIndices.ToSummary());
				stringBuilder.AppendLine("    -skinBoneBindPoses:" + this.skinBoneBindPoses.ToSummary());
				stringBuilder.AppendLine("  [Mesh Other]");
				stringBuilder.AppendLine("    -vertexToTransformRotations:" + this.vertexToTransformRotations.ToSummary());
				stringBuilder.AppendLine("    -positions:" + this.positions.ToSummary());
				stringBuilder.AppendLine("    -rotations:" + this.rotations.ToSummary());
				stringBuilder.AppendLine("  [Mapping]");
				stringBuilder.AppendLine(string.Format("    -MappingVertexCount:{0}", this.MappingVertexCount));
				stringBuilder.AppendLine("    -mappingReferenceIndices:" + this.mappingReferenceIndices.ToSummary());
				stringBuilder.AppendLine("    -mappingAttributes:" + this.mappingAttributes.ToSummary());
				stringBuilder.AppendLine("    -mappingLocalPositins:" + this.mappingLocalPositins.ToSummary());
				stringBuilder.AppendLine("    -mappingLocalNormals:" + this.mappingLocalNormals.ToSummary());
				stringBuilder.AppendLine("    -mappingBoneWeights:" + this.mappingBoneWeights.ToSummary());
			}
			stringBuilder.AppendLine();
			Debug.Log(stringBuilder.ToString());
			allsb.Append(stringBuilder);
		}

		// Token: 0x0400077C RID: 1916
		public ExNativeArray<short> teamIds;

		// Token: 0x0400077D RID: 1917
		public ExNativeArray<VertexAttribute> attributes;

		// Token: 0x0400077E RID: 1918
		public ExNativeArray<FixedList32Bytes<uint>> vertexToTriangles;

		// Token: 0x0400077F RID: 1919
		public ExNativeArray<float3> vertexBindPosePositions;

		// Token: 0x04000780 RID: 1920
		public ExNativeArray<quaternion> vertexBindPoseRotations;

		// Token: 0x04000781 RID: 1921
		public ExNativeArray<float> vertexDepths;

		// Token: 0x04000782 RID: 1922
		public ExNativeArray<int> vertexRootIndices;

		// Token: 0x04000783 RID: 1923
		public ExNativeArray<float3> vertexLocalPositions;

		// Token: 0x04000784 RID: 1924
		public ExNativeArray<quaternion> vertexLocalRotations;

		// Token: 0x04000785 RID: 1925
		public ExNativeArray<int> vertexParentIndices;

		// Token: 0x04000786 RID: 1926
		public ExNativeArray<uint> vertexChildIndexArray;

		// Token: 0x04000787 RID: 1927
		public ExNativeArray<ushort> vertexChildDataArray;

		// Token: 0x04000788 RID: 1928
		public ExNativeArray<quaternion> normalAdjustmentRotations;

		// Token: 0x04000789 RID: 1929
		public ExNativeArray<float2> uv;

		// Token: 0x0400078A RID: 1930
		public ExNativeArray<short> triangleTeamIdArray;

		// Token: 0x0400078B RID: 1931
		public ExNativeArray<int3> triangles;

		// Token: 0x0400078C RID: 1932
		public ExNativeArray<float3> triangleNormals;

		// Token: 0x0400078D RID: 1933
		public ExNativeArray<float3> triangleTangents;

		// Token: 0x0400078E RID: 1934
		public ExNativeArray<short> edgeTeamIdArray;

		// Token: 0x0400078F RID: 1935
		public ExNativeArray<int2> edges;

		// Token: 0x04000790 RID: 1936
		public ExNativeArray<ExBitFlag8> edgeFlags;

		// Token: 0x04000791 RID: 1937
		public ExNativeArray<ExBitFlag8> baseLineFlags;

		// Token: 0x04000792 RID: 1938
		public ExNativeArray<short> baseLineTeamIds;

		// Token: 0x04000793 RID: 1939
		public ExNativeArray<ushort> baseLineStartDataIndices;

		// Token: 0x04000794 RID: 1940
		public ExNativeArray<ushort> baseLineDataCounts;

		// Token: 0x04000795 RID: 1941
		public ExNativeArray<ushort> baseLineData;

		// Token: 0x04000796 RID: 1942
		public ExNativeArray<float3> localPositions;

		// Token: 0x04000797 RID: 1943
		public ExNativeArray<float3> localNormals;

		// Token: 0x04000798 RID: 1944
		public ExNativeArray<float3> localTangents;

		// Token: 0x04000799 RID: 1945
		public ExNativeArray<VirtualMeshBoneWeight> boneWeights;

		// Token: 0x0400079A RID: 1946
		public ExNativeArray<int> skinBoneTransformIndices;

		// Token: 0x0400079B RID: 1947
		public ExNativeArray<float4x4> skinBoneBindPoses;

		// Token: 0x0400079C RID: 1948
		public ExNativeArray<quaternion> vertexToTransformRotations;

		// Token: 0x0400079D RID: 1949
		public ExNativeArray<float3> positions;

		// Token: 0x0400079E RID: 1950
		public ExNativeArray<quaternion> rotations;

		// Token: 0x0400079F RID: 1951
		public ExNativeArray<short> mappingIdArray;

		// Token: 0x040007A0 RID: 1952
		public ExNativeArray<int> mappingReferenceIndices;

		// Token: 0x040007A1 RID: 1953
		public ExNativeArray<VertexAttribute> mappingAttributes;

		// Token: 0x040007A2 RID: 1954
		public ExNativeArray<float3> mappingLocalPositins;

		// Token: 0x040007A3 RID: 1955
		public ExNativeArray<float3> mappingLocalNormals;

		// Token: 0x040007A4 RID: 1956
		public ExNativeArray<float3> mappingLocalTangents;

		// Token: 0x040007A5 RID: 1957
		public ExNativeArray<VirtualMeshBoneWeight> mappingBoneWeights;

		// Token: 0x040007A6 RID: 1958
		private bool isValid;

		// Token: 0x020000CA RID: 202
		[BurstCompile]
		private struct CalcMeshConvert_A_Job : IJobParallelFor
		{
			// Token: 0x060003A0 RID: 928 RVA: 0x0002748C File Offset: 0x0002568C
			public void Execute(int index)
			{
				TeamManager.MappingData mappingData = this.mappingDataArray[index];
				if (!mappingData.IsValid())
				{
					return;
				}
				TeamManager.TeamData teamData = this.teamDataArray[mappingData.teamId];
				if (!teamData.IsProcess)
				{
					return;
				}
				RenderManager.RenderDataWork renderDataWork = this.renderDataWorkArray[mappingData.renderDataWorkIndex];
				if (!renderDataWork.UseCustomMesh)
				{
					return;
				}
				float3 @float = this.transformPositionArray[mappingData.centerTransformIndex];
				quaternion quaternion = this.transformRotationArray[mappingData.centerTransformIndex];
				float3 float2 = this.transformScaleArray[mappingData.centerTransformIndex];
				quaternion quaternion2 = math.inverse(quaternion);
				float3 float3 = this.transformPositionArray[teamData.centerTransformIndex];
				quaternion quaternion3 = this.transformRotationArray[teamData.centerTransformIndex];
				float3 float4 = this.transformScaleArray[teamData.centerTransformIndex];
				bool flag = MathUtility.CompareTransform(in @float, in quaternion, in float2, in float3, in quaternion3, in float4);
				mappingData.sameSpace = flag;
				mappingData.toMappingMatrix = math.inverse(MathUtility.LocalToWorldMatrix(in @float, in quaternion, in float2));
				mappingData.toMappingRotation = quaternion2;
				float num = math.length(teamData.initScale);
				mappingData.scaleRatio = math.length(float4) / num;
				bool isTangent = teamData.IsTangent;
				bool flag2 = renderDataWork.HasBoneWeight && mappingData.flag.IsSet(3);
				mappingData.flag.SetBits(0, true);
				mappingData.flag.SetBits(1, isTangent);
				mappingData.flag.SetBits(2, flag2);
				this.mappingDataArray[index] = mappingData;
			}

			// Token: 0x040007A7 RID: 1959
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040007A8 RID: 1960
			[ReadOnly]
			public NativeArray<float3> transformPositionArray;

			// Token: 0x040007A9 RID: 1961
			[ReadOnly]
			public NativeArray<quaternion> transformRotationArray;

			// Token: 0x040007AA RID: 1962
			[ReadOnly]
			public NativeArray<float3> transformScaleArray;

			// Token: 0x040007AB RID: 1963
			public NativeArray<TeamManager.MappingData> mappingDataArray;

			// Token: 0x040007AC RID: 1964
			[ReadOnly]
			public NativeArray<RenderManager.RenderDataWork> renderDataWorkArray;
		}

		// Token: 0x020000CB RID: 203
		[BurstCompile]
		private struct CalcMeshConvert_B_Job : IJobParallelFor
		{
			// Token: 0x060003A1 RID: 929 RVA: 0x00027618 File Offset: 0x00025818
			public void Execute(int dataIndex)
			{
				int num = dataIndex / this.workerCount;
				int num2 = dataIndex % this.workerCount;
				TeamManager.MappingData mappingData = this.mappingDataArray[num];
				if (!mappingData.IsValid())
				{
					return;
				}
				TeamManager.TeamData teamData = this.teamDataArray[mappingData.teamId];
				if (!teamData.IsProcess)
				{
					return;
				}
				RenderManager.RenderDataWork renderDataWork = this.renderDataWorkArray[mappingData.renderDataWorkIndex];
				if (!renderDataWork.UseCustomMesh)
				{
					return;
				}
				DataChunk workerChunk = MathUtility.GetWorkerChunk(mappingData.mappingCommonChunk.dataLength, this.workerCount, num2);
				if (!workerChunk.IsValid)
				{
					return;
				}
				bool isTangent = teamData.IsTangent;
				float4 @float = new float4(teamData.negativeScaleDirection, 1f);
				float3 float2 = teamData.initScale * mappingData.scaleRatio;
				bool flag = renderDataWork.HasBoneWeight && mappingData.flag.IsSet(3);
				int num3 = mappingData.mappingCommonChunk.startIndex + workerChunk.startIndex;
				int i = 0;
				while (i < workerChunk.dataLength)
				{
					VertexAttribute vertexAttribute = this.mappingAttributes[num3];
					if (!vertexAttribute.IsInvalid() && !vertexAttribute.IsFixed())
					{
						float4x3 float4x = 0;
						float4x.c0 = new float4(this.mappingLocalPositions[num3], 1f);
						float4x.c1 = new float4(this.mappingLocalNormals[num3], 0f);
						float4x.c2 = math.select(0, new float4(this.mappingLocalTangents[num3], 0f), isTangent);
						if (!mappingData.sameSpace)
						{
							float4x = math.mul(mappingData.toProxyMatrix, float4x);
						}
						float4x *= new float4x3(@float, @float, @float);
						VirtualMeshBoneWeight virtualMeshBoneWeight = this.mappingBoneWeights[num3];
						int count = virtualMeshBoneWeight.Count;
						float4x3 float4x2 = 0;
						for (int j = 0; j < count; j++)
						{
							float num4 = virtualMeshBoneWeight.weights[j];
							int num5 = virtualMeshBoneWeight.boneIndices[j] + teamData.proxyCommonChunk.startIndex;
							float3 float3 = this.proxyVertexBindPosePositions[num5];
							ref quaternion ptr = this.proxyVertexBindPoseRotations[num5];
							float3 *= teamData.negativeScaleDirection;
							quaternion quaternion = ptr.value * teamData.negativeScaleQuaternionValue;
							float3 float4 = math.mul(quaternion, float4x.c0.xyz + float3);
							float3 float5 = math.mul(quaternion, float4x.c1.xyz);
							float3 float6 = math.mul(quaternion, float4x.c2.xyz);
							float3 float7 = this.proxyPositions[num5];
							quaternion quaternion2 = this.proxyRotations[num5];
							float4 = math.mul(quaternion2, float4 * float2) + float7;
							float5 = math.mul(quaternion2, float5);
							float6 = math.mul(quaternion2, float6);
							float4x2.c0.xyz = float4x2.c0.xyz + float4 * num4;
							float4x2.c1.xyz = float4x2.c1.xyz + float5 * num4;
							float4x2.c2.xyz = float4x2.c2.xyz + float6 * num4;
						}
						float4x2.c0.w = 1f;
						float4x2 = math.mul(mappingData.toMappingMatrix, float4x2);
						int num6 = this.mappingReferenceIndices[num3];
						int num7 = renderDataWork.renderMeshPositionAndNormalChunk.startIndex + num6;
						this.renderMeshPositions[num7] = float4x2.c0.xyz;
						this.renderMeshNormals[num7] = math.normalize(float4x2.c1.xyz);
						if (isTangent)
						{
							num7 = renderDataWork.renderMeshTangentChunk.startIndex + num6;
							float4 float8 = this.renderMeshTangents[num7];
							this.renderMeshTangents[num7] = new float4(math.normalize(float4x2.c2.xyz), float8.w);
						}
						if (flag)
						{
							num7 = renderDataWork.renderMeshBoneWeightChunk.startIndex + num6;
							this.renderMeshBoneWeights[num7] = renderDataWork.centerBoneWeight;
						}
					}
					i++;
					num3++;
				}
			}

			// Token: 0x040007AD RID: 1965
			public int workerCount;

			// Token: 0x040007AE RID: 1966
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040007AF RID: 1967
			[ReadOnly]
			public NativeArray<TeamManager.MappingData> mappingDataArray;

			// Token: 0x040007B0 RID: 1968
			[ReadOnly]
			public NativeArray<VertexAttribute> mappingAttributes;

			// Token: 0x040007B1 RID: 1969
			[ReadOnly]
			public NativeArray<float3> mappingLocalPositions;

			// Token: 0x040007B2 RID: 1970
			[ReadOnly]
			public NativeArray<float3> mappingLocalNormals;

			// Token: 0x040007B3 RID: 1971
			[ReadOnly]
			public NativeArray<float3> mappingLocalTangents;

			// Token: 0x040007B4 RID: 1972
			[ReadOnly]
			public NativeArray<VirtualMeshBoneWeight> mappingBoneWeights;

			// Token: 0x040007B5 RID: 1973
			[ReadOnly]
			public NativeArray<int> mappingReferenceIndices;

			// Token: 0x040007B6 RID: 1974
			[ReadOnly]
			public NativeArray<float3> proxyPositions;

			// Token: 0x040007B7 RID: 1975
			[ReadOnly]
			public NativeArray<quaternion> proxyRotations;

			// Token: 0x040007B8 RID: 1976
			[ReadOnly]
			public NativeArray<float3> proxyVertexBindPosePositions;

			// Token: 0x040007B9 RID: 1977
			[ReadOnly]
			public NativeArray<quaternion> proxyVertexBindPoseRotations;

			// Token: 0x040007BA RID: 1978
			[ReadOnly]
			public NativeArray<RenderManager.RenderDataWork> renderDataWorkArray;

			// Token: 0x040007BB RID: 1979
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> renderMeshPositions;

			// Token: 0x040007BC RID: 1980
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> renderMeshNormals;

			// Token: 0x040007BD RID: 1981
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> renderMeshTangents;

			// Token: 0x040007BE RID: 1982
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<BoneWeight> renderMeshBoneWeights;
		}

		// Token: 0x020000CC RID: 204
		[BurstCompile]
		private struct PostRenderMeshWorkDataBatchJob : IJobParallelFor
		{
			// Token: 0x060003A2 RID: 930 RVA: 0x00027A68 File Offset: 0x00025C68
			public void Execute(int windex)
			{
				RenderManager.RenderDataWork renderDataWork = this.renderDataWorkArray[windex];
				if (!renderDataWork.IsValid())
				{
					return;
				}
				if (!renderDataWork.UseCustomMesh)
				{
					return;
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				int length = renderDataWork.mappingDataIndexList.Length;
				for (int i = 0; i < length; i++)
				{
					int num = (int)renderDataWork.mappingDataIndexList[i];
					TeamManager.MappingData mappingData = this.mappingDataArray[num];
					if (mappingData.flag.IsSet(0))
					{
						flag = true;
						mappingData.flag.SetBits(0, false);
					}
					if (mappingData.flag.IsSet(1))
					{
						flag2 = true;
						mappingData.flag.SetBits(1, false);
					}
					if (mappingData.flag.IsSet(2))
					{
						flag3 = true;
						mappingData.flag.SetBits(2, false);
						mappingData.flag.SetBits(3, false);
					}
					this.mappingDataArray[num] = mappingData;
				}
				renderDataWork.flag.SetBits(1, flag);
				renderDataWork.flag.SetBits(6, flag2);
				renderDataWork.flag.SetBits(2, flag3);
				this.renderDataWorkArray[windex] = renderDataWork;
			}

			// Token: 0x040007BF RID: 1983
			public NativeArray<RenderManager.RenderDataWork> renderDataWorkArray;

			// Token: 0x040007C0 RID: 1984
			[NativeDisableParallelForRestriction]
			public NativeArray<TeamManager.MappingData> mappingDataArray;
		}
	}
}
