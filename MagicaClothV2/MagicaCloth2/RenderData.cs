using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000084 RID: 132
	public class RenderData : IDisposable, ITransform
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00011D2F File Offset: 0x0000FF2F
		// (set) Token: 0x0600021D RID: 541 RVA: 0x00011D37 File Offset: 0x0000FF37
		public int ReferenceCount { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00011D40 File Offset: 0x0000FF40
		internal string Name
		{
			get
			{
				RenderSetupData renderSetupData = this.setupData;
				return ((renderSetupData != null) ? renderSetupData.name : null) ?? "(empty)";
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00011D5D File Offset: 0x0000FF5D
		internal bool HasSkinnedMesh
		{
			get
			{
				RenderSetupData renderSetupData = this.setupData;
				return renderSetupData != null && renderSetupData.hasSkinnedMesh;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00011D70 File Offset: 0x0000FF70
		internal bool HasBoneWeight
		{
			get
			{
				RenderSetupData renderSetupData = this.setupData;
				return renderSetupData != null && renderSetupData.hasBoneWeight;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00011D83 File Offset: 0x0000FF83
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00011D8B File Offset: 0x0000FF8B
		internal Mesh originalMesh { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00011D94 File Offset: 0x0000FF94
		// (set) Token: 0x06000224 RID: 548 RVA: 0x00011D9C File Offset: 0x0000FF9C
		internal List<Transform> transformList { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00011DA5 File Offset: 0x0000FFA5
		// (set) Token: 0x06000226 RID: 550 RVA: 0x00011DAD File Offset: 0x0000FFAD
		internal Mesh customMesh { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00011DB6 File Offset: 0x0000FFB6
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00011DBE File Offset: 0x0000FFBE
		internal int renderDataWorkIndex { get; private set; } = -1;

		// Token: 0x06000229 RID: 553 RVA: 0x00011DC8 File Offset: 0x0000FFC8
		public void Dispose()
		{
			this.SwapOriginalMesh(null);
			RenderSetupData renderSetupData = this.setupData;
			if (renderSetupData != null)
			{
				renderSetupData.Dispose();
			}
			this.preBuildUniqueSerializeData = null;
			MagicaManager.Render.RemoveRenderDataWork(this.renderDataWorkIndex);
			if (this.customMesh)
			{
				Object.Destroy(this.customMesh);
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00011E1C File Offset: 0x0001001C
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			RenderSetupData renderSetupData = this.setupData;
			if (renderSetupData == null)
			{
				return;
			}
			renderSetupData.GetUsedTransform(transformSet);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00011E2F File Offset: 0x0001002F
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			RenderSetupData renderSetupData = this.setupData;
			if (renderSetupData == null)
			{
				return;
			}
			renderSetupData.ReplaceTransform(replaceDict);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00011E44 File Offset: 0x00010044
		internal void Initialize(Renderer ren, RenderSetupData referenceSetupData, RenderSetupData.UniqueSerializationData referencePreBuildUniqueSetupData, RenderSetupSerializeData referenceInitSetupData)
		{
			if (referenceSetupData != null && referencePreBuildUniqueSetupData != null)
			{
				this.setupData = referenceSetupData;
				this.preBuildUniqueSerializeData = referencePreBuildUniqueSetupData;
				this.originalMesh = this.preBuildUniqueSerializeData.originalMesh;
				this.renderer = this.preBuildUniqueSerializeData.renderer;
				this.skinnedMeshRendere = this.preBuildUniqueSerializeData.skinRenderer;
				this.meshFilter = this.preBuildUniqueSerializeData.meshFilter;
				this.transformList = this.preBuildUniqueSerializeData.transformList;
			}
			else
			{
				this.setupData = new RenderSetupData(referenceInitSetupData, ren);
				this.preBuildUniqueSerializeData = null;
				this.originalMesh = this.setupData.originalMesh;
				this.renderer = this.setupData.renderer;
				this.skinnedMeshRendere = this.setupData.skinRenderer;
				this.meshFilter = this.setupData.meshFilter;
				this.transformList = this.setupData.transformList;
			}
			this.renderDataWorkIndex = MagicaManager.Render.AddRenderDataWork(this);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00011F37 File Offset: 0x00010137
		internal ResultCode Result
		{
			get
			{
				RenderSetupData renderSetupData = this.setupData;
				if (renderSetupData == null)
				{
					return ResultCode.None;
				}
				return renderSetupData.result;
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00011F50 File Offset: 0x00010150
		internal int AddReferenceCount()
		{
			int referenceCount = this.ReferenceCount;
			this.ReferenceCount = referenceCount + 1;
			return this.ReferenceCount;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00011F74 File Offset: 0x00010174
		internal int RemoveReferenceCount()
		{
			int referenceCount = this.ReferenceCount;
			this.ReferenceCount = referenceCount - 1;
			return this.ReferenceCount;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00011F98 File Offset: 0x00010198
		private void SwapCustomMesh(ClothProcess process)
		{
			if (this.setupData.IsFaild())
			{
				return;
			}
			if (this.originalMesh == null)
			{
				return;
			}
			if (MagicaManager.Render.IsSetRenderDataWorkFlag(this.renderDataWorkIndex, 0))
			{
				return;
			}
			if (this.customMesh == null)
			{
				this.customMesh = Object.Instantiate<Mesh>(this.originalMesh);
				this.customMesh.MarkDynamic();
				if (this.HasBoneWeight)
				{
					int num = ((this.preBuildUniqueSerializeData != null) ? this.preBuildUniqueSerializeData.transformList.Count : this.setupData.TransformCount);
					List<Matrix4x4> list = new List<Matrix4x4>(num);
					list.AddRange(this.setupData.bindPoseList);
					while (list.Count < num)
					{
						list.Add(Matrix4x4.identity);
					}
					this.customMesh.bindposes = list.ToArray();
					this.skinnedMeshRendere.bones = this.transformList.ToArray();
				}
			}
			this.ResetCustomMeshWorkData();
			this.SetMesh(this.customMesh);
			MagicaManager.Render.SetBitsRenderDataWorkFlag(this.renderDataWorkIndex, 0, true);
			if (process != null && process.cloth)
			{
				Action<MagicaCloth, Renderer, bool> onRendererMeshChange = process.cloth.OnRendererMeshChange;
				if (onRendererMeshChange == null)
				{
					return;
				}
				onRendererMeshChange(process.cloth, this.renderer, true);
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000120DC File Offset: 0x000102DC
		private void ResetCustomMeshWorkData()
		{
			RenderManager render = MagicaManager.Render;
			ref RenderManager.RenderDataWork renderDataWorkRef = ref render.GetRenderDataWorkRef(this.renderDataWorkIndex);
			int vertexCount = this.setupData.vertexCount;
			if (this.setupData.HasMeshDataArray)
			{
				Mesh.MeshData meshData = this.setupData.meshDataArray[0];
				using (NativeArray<Vector3> nativeArray = new NativeArray<Vector3>(vertexCount, Allocator.TempJob, NativeArrayOptions.ClearMemory))
				{
					using (NativeArray<Vector3> nativeArray2 = new NativeArray<Vector3>(vertexCount, Allocator.TempJob, NativeArrayOptions.ClearMemory))
					{
						meshData.GetVertices(nativeArray);
						meshData.GetNormals(nativeArray2);
						render.renderMeshPositions.CopyFrom<Vector3>(nativeArray, renderDataWorkRef.renderMeshPositionAndNormalChunk.startIndex, vertexCount);
						render.renderMeshNormals.CopyFrom<Vector3>(nativeArray2, renderDataWorkRef.renderMeshPositionAndNormalChunk.startIndex, vertexCount);
						if (renderDataWorkRef.HasMeshTangent)
						{
							using (NativeArray<Vector4> nativeArray3 = new NativeArray<Vector4>(vertexCount, Allocator.TempJob, NativeArrayOptions.ClearMemory))
							{
								meshData.GetTangents(nativeArray3);
								render.renderMeshTangents.CopyFrom<Vector4>(nativeArray3, renderDataWorkRef.renderMeshTangentChunk.startIndex, vertexCount);
								renderDataWorkRef.flag.SetBits(5, true);
							}
						}
						goto IL_0196;
					}
				}
			}
			render.renderMeshPositions.CopyFrom<Vector3>(this.setupData.localPositions, renderDataWorkRef.renderMeshPositionAndNormalChunk.startIndex, vertexCount);
			render.renderMeshNormals.CopyFrom<Vector3>(this.setupData.localNormals, renderDataWorkRef.renderMeshPositionAndNormalChunk.startIndex, vertexCount);
			if (renderDataWorkRef.HasMeshTangent && this.setupData.HasTangent)
			{
				render.renderMeshTangents.CopyFrom<Vector4>(this.setupData.localTangents, renderDataWorkRef.renderMeshTangentChunk.startIndex, vertexCount);
				renderDataWorkRef.flag.SetBits(5, true);
			}
			IL_0196:
			if (this.HasBoneWeight && renderDataWorkRef.HasBoneWeight)
			{
				using (NativeArray<BoneWeight> nativeArray4 = new NativeArray<BoneWeight>(vertexCount, Allocator.TempJob, NativeArrayOptions.ClearMemory))
				{
					this.setupData.GetBoneWeightsRun(nativeArray4);
					render.renderMeshBoneWeights.CopyFrom<BoneWeight>(nativeArray4, renderDataWorkRef.renderMeshBoneWeightChunk.startIndex, vertexCount);
				}
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00012304 File Offset: 0x00010504
		private void SwapOriginalMesh(ClothProcess process)
		{
			RenderManager render = MagicaManager.Render;
			if (render.IsSetRenderDataWorkFlag(this.renderDataWorkIndex, 0) && this.setupData != null)
			{
				this.SetMesh(this.originalMesh);
				if (this.skinnedMeshRendere != null)
				{
					this.skinnedMeshRendere.bones = this.transformList.ToArray();
				}
			}
			render.SetBitsRenderDataWorkFlag(this.renderDataWorkIndex, 0, false);
			if (process != null && process.cloth)
			{
				Action<MagicaCloth, Renderer, bool> onRendererMeshChange = process.cloth.OnRendererMeshChange;
				if (onRendererMeshChange == null)
				{
					return;
				}
				onRendererMeshChange(process.cloth, this.renderer, false);
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0001239C File Offset: 0x0001059C
		private void SetMesh(Mesh mesh)
		{
			if (mesh == null)
			{
				return;
			}
			if (this.setupData != null)
			{
				if (this.meshFilter != null)
				{
					this.meshFilter.mesh = mesh;
					return;
				}
				if (this.skinnedMeshRendere != null)
				{
					this.skinnedMeshRendere.sharedMesh = mesh;
				}
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000123F0 File Offset: 0x000105F0
		public void StartUse(ClothProcess cprocess)
		{
			this.UpdateUse(cprocess, 1);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000123FA File Offset: 0x000105FA
		public void EndUse(ClothProcess cprocess)
		{
			this.UpdateUse(cprocess, -1);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00012404 File Offset: 0x00010604
		internal void UpdateUse(ClothProcess cprocess, int add)
		{
			if (add > 0)
			{
				this.useProcessSet.Add(cprocess);
			}
			else if (add < 0)
			{
				if (!this.useProcessSet.Contains(cprocess))
				{
					return;
				}
				this.useProcessSet.Remove(cprocess);
			}
			bool flag = this.useProcessSet.Any<ClothProcess>((ClothProcess x) => (x.IsCameraCullingInvisible() && !x.IsCameraCullingKeep()) || x.IsDistanceCullingInvisible());
			bool flag2 = false;
			if (flag || this.useProcessSet.Count == 0)
			{
				this.SwapOriginalMesh(cprocess);
			}
			else if (add == 0 && this.useProcessSet.Count > 0)
			{
				this.SwapCustomMesh(cprocess);
				flag2 = true;
			}
			else if (add > 0 && this.useProcessSet.Count == 1)
			{
				this.SwapCustomMesh(cprocess);
				flag2 = true;
			}
			else if (add != 0)
			{
				this.ResetCustomMeshWorkData();
				flag2 = true;
			}
			if (flag2)
			{
				ref RenderManager.RenderDataWork renderDataWorkRef = ref MagicaManager.Render.GetRenderDataWorkRef(this.renderDataWorkIndex);
				int length = renderDataWorkRef.mappingDataIndexList.Length;
				for (int i = 0; i < length; i++)
				{
					int num = (int)renderDataWorkRef.mappingDataIndexList[i];
					MagicaManager.Team.GetMappingDataRef(num).flag.SetBits(3, true);
				}
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00012520 File Offset: 0x00010720
		internal void UpdateSkipWriting()
		{
			this.isSkipWriting = false;
			using (HashSet<ClothProcess>.Enumerator enumerator = this.useProcessSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsSkipWriting())
					{
						this.isSkipWriting = true;
					}
				}
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00012580 File Offset: 0x00010780
		internal void WriteMesh()
		{
			RenderManager render = MagicaManager.Render;
			ref RenderManager.RenderDataWork renderDataWorkRef = ref render.GetRenderDataWorkRef(this.renderDataWorkIndex);
			if (!renderDataWorkRef.UseCustomMesh || this.useProcessSet.Count == 0)
			{
				return;
			}
			if (this.isSkipWriting)
			{
				return;
			}
			if (renderDataWorkRef.flag.IsSet(1))
			{
				this.customMesh.SetVertices<float3>(render.renderMeshPositions.GetNativeArray(), renderDataWorkRef.renderMeshPositionAndNormalChunk.startIndex, renderDataWorkRef.renderMeshPositionAndNormalChunk.dataLength);
				this.customMesh.SetNormals<float3>(render.renderMeshNormals.GetNativeArray(), renderDataWorkRef.renderMeshPositionAndNormalChunk.startIndex, renderDataWorkRef.renderMeshPositionAndNormalChunk.dataLength);
				renderDataWorkRef.flag.SetBits(1, false);
			}
			if (renderDataWorkRef.flag.IsSet(6))
			{
				this.customMesh.SetTangents<float4>(render.renderMeshTangents.GetNativeArray(), renderDataWorkRef.renderMeshTangentChunk.startIndex, renderDataWorkRef.renderMeshTangentChunk.dataLength);
				renderDataWorkRef.flag.SetBits(6, false);
			}
			if (renderDataWorkRef.flag.IsSet(2))
			{
				NativeSlice<BoneWeight> nativeSlice = new NativeSlice<BoneWeight>(render.renderMeshBoneWeights.GetNativeArray(), renderDataWorkRef.renderMeshBoneWeightChunk.startIndex, renderDataWorkRef.renderMeshBoneWeightChunk.dataLength);
				this.customMesh.boneWeights = nativeSlice.ToArray();
				renderDataWorkRef.flag.SetBits(2, false);
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x000126D0 File Offset: 0x000108D0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format(">>> [{0}] ref:{1}, useProcess:{2}, HasSkinnedMesh:{3}, HasBoneWeight:{4}", new object[]
			{
				this.Name,
				this.ReferenceCount,
				this.useProcessSet.Count,
				this.HasSkinnedMesh,
				this.HasBoneWeight
			}));
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x04000389 RID: 905
		private HashSet<ClothProcess> useProcessSet = new HashSet<ClothProcess>();

		// Token: 0x0400038A RID: 906
		private bool isSkipWriting;

		// Token: 0x0400038B RID: 907
		internal RenderSetupData setupData;

		// Token: 0x0400038C RID: 908
		internal RenderSetupData.UniqueSerializationData preBuildUniqueSerializeData;

		// Token: 0x0400038E RID: 910
		private Renderer renderer;

		// Token: 0x0400038F RID: 911
		private SkinnedMeshRenderer skinnedMeshRendere;

		// Token: 0x04000390 RID: 912
		private MeshFilter meshFilter;
	}
}
