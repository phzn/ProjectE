using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace MagicaCloth2
{
	// Token: 0x02000086 RID: 134
	public class RenderManager : IManager, IDisposable, IValid
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0001278D File Offset: 0x0001098D
		public int RenderDataWorkCount
		{
			get
			{
				ExNativeArray<RenderManager.RenderDataWork> exNativeArray = this.renderDataWorkArray;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x000127A0 File Offset: 0x000109A0
		public void Initialize()
		{
			this.Dispose();
			this.renderDataWorkArray = new ExNativeArray<RenderManager.RenderDataWork>(0, true);
			this.renderMeshPositions = new ExNativeArray<float3>(0, true);
			this.renderMeshNormals = new ExNativeArray<float3>(0, true);
			this.renderMeshTangents = new ExNativeArray<float4>(0, true);
			this.renderMeshBoneWeights = new ExNativeArray<BoneWeight>(0, true);
			MagicaManager.afterDelayedDelegate = (MagicaManager.UpdateMethod)Delegate.Combine(MagicaManager.afterDelayedDelegate, new MagicaManager.UpdateMethod(this.PreRenderingUpdate));
			this.isValid = true;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0001281B File Offset: 0x00010A1B
		public void EnterdEditMode()
		{
			this.Dispose();
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00012824 File Offset: 0x00010A24
		public void Dispose()
		{
			this.isValid = false;
			Dictionary<int, RenderData> dictionary = this.renderDataDict;
			lock (dictionary)
			{
				foreach (RenderData renderData in this.renderDataDict.Values)
				{
					if (renderData != null)
					{
						renderData.Dispose();
					}
				}
			}
			this.renderDataDict.Clear();
			ExNativeArray<RenderManager.RenderDataWork> exNativeArray = this.renderDataWorkArray;
			if (exNativeArray != null)
			{
				exNativeArray.Dispose();
			}
			ExNativeArray<float3> exNativeArray2 = this.renderMeshPositions;
			if (exNativeArray2 != null)
			{
				exNativeArray2.Dispose();
			}
			ExNativeArray<float3> exNativeArray3 = this.renderMeshNormals;
			if (exNativeArray3 != null)
			{
				exNativeArray3.Dispose();
			}
			ExNativeArray<float4> exNativeArray4 = this.renderMeshTangents;
			if (exNativeArray4 != null)
			{
				exNativeArray4.Dispose();
			}
			ExNativeArray<BoneWeight> exNativeArray5 = this.renderMeshBoneWeights;
			if (exNativeArray5 != null)
			{
				exNativeArray5.Dispose();
			}
			this.renderDataWorkArray = null;
			this.renderMeshPositions = null;
			this.renderMeshNormals = null;
			this.renderMeshBoneWeights = null;
			this.renderMeshTangents = null;
			MagicaManager.afterDelayedDelegate = (MagicaManager.UpdateMethod)Delegate.Remove(MagicaManager.afterDelayedDelegate, new MagicaManager.UpdateMethod(this.PreRenderingUpdate));
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00012950 File Offset: 0x00010B50
		public bool IsValid()
		{
			return this.isValid;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00012958 File Offset: 0x00010B58
		public int AddRenderer(Renderer ren, RenderSetupData referenceSetupData, RenderSetupData.UniqueSerializationData referenceUniqueSetupData, RenderSetupSerializeData referenceInitSetupData)
		{
			if (!this.isValid)
			{
				return 0;
			}
			int instanceID = ren.GetInstanceID();
			Dictionary<int, RenderData> dictionary = this.renderDataDict;
			lock (dictionary)
			{
				if (!this.renderDataDict.ContainsKey(instanceID))
				{
					RenderData renderData = new RenderData();
					renderData.Initialize(ren, referenceSetupData, referenceUniqueSetupData, referenceInitSetupData);
					this.renderDataDict.Add(instanceID, renderData);
				}
				this.renderDataDict[instanceID].AddReferenceCount();
			}
			return instanceID;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000129E4 File Offset: 0x00010BE4
		public bool RemoveRenderer(int handle)
		{
			if (!this.isValid)
			{
				return false;
			}
			bool flag = false;
			Dictionary<int, RenderData> dictionary = this.renderDataDict;
			lock (dictionary)
			{
				if (this.renderDataDict.ContainsKey(handle))
				{
					RenderData renderData = this.renderDataDict[handle];
					if (renderData.RemoveReferenceCount() == 0)
					{
						renderData.Dispose();
						this.renderDataDict.Remove(handle);
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00012A64 File Offset: 0x00010C64
		public RenderData GetRendererData(int handle)
		{
			if (!this.isValid)
			{
				return null;
			}
			Dictionary<int, RenderData> dictionary = this.renderDataDict;
			RenderData renderData;
			lock (dictionary)
			{
				if (this.renderDataDict.ContainsKey(handle))
				{
					renderData = this.renderDataDict[handle];
				}
				else
				{
					renderData = null;
				}
			}
			return renderData;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00012AC8 File Offset: 0x00010CC8
		public int AddRenderDataWork(RenderData rdata)
		{
			if (!this.isValid)
			{
				return -1;
			}
			RenderManager.RenderDataWork renderDataWork = default(RenderManager.RenderDataWork);
			renderDataWork.flag.SetBits(4, rdata.originalMesh.HasVertexAttribute(VertexAttribute.Tangent));
			renderDataWork.flag.SetBits(7, rdata.HasSkinnedMesh);
			renderDataWork.flag.SetBits(8, rdata.HasBoneWeight);
			renderDataWork.centerBoneWeight = new BoneWeight
			{
				boneIndex0 = rdata.setupData.renderTransformIndex,
				weight0 = 1f
			};
			int vertexCount = rdata.originalMesh.vertexCount;
			renderDataWork.renderMeshPositionAndNormalChunk = this.renderMeshPositions.AddRange(vertexCount);
			this.renderMeshNormals.AddRange(vertexCount);
			if (renderDataWork.HasMeshTangent)
			{
				renderDataWork.renderMeshTangentChunk = this.renderMeshTangents.AddRange(vertexCount);
			}
			if (renderDataWork.HasBoneWeight)
			{
				renderDataWork.renderMeshBoneWeightChunk = this.renderMeshBoneWeights.AddRange(vertexCount);
			}
			return this.renderDataWorkArray.Add(renderDataWork).startIndex;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00012BCC File Offset: 0x00010DCC
		public void RemoveRenderDataWork(int index)
		{
			if (!this.isValid)
			{
				return;
			}
			if (index < 0)
			{
				return;
			}
			ref RenderManager.RenderDataWork renderDataWorkRef = ref this.GetRenderDataWorkRef(index);
			if (renderDataWorkRef.renderMeshPositionAndNormalChunk.IsValid)
			{
				this.renderMeshPositions.Remove(renderDataWorkRef.renderMeshPositionAndNormalChunk);
				this.renderMeshNormals.Remove(renderDataWorkRef.renderMeshPositionAndNormalChunk);
			}
			if (renderDataWorkRef.renderMeshTangentChunk.IsValid)
			{
				this.renderMeshTangents.Remove(renderDataWorkRef.renderMeshTangentChunk);
			}
			if (renderDataWorkRef.renderMeshBoneWeightChunk.IsValid)
			{
				this.renderMeshBoneWeights.Remove(renderDataWorkRef.renderMeshBoneWeightChunk);
			}
			renderDataWorkRef.renderMeshPositionAndNormalChunk.Clear();
			renderDataWorkRef.renderMeshTangentChunk.Clear();
			renderDataWorkRef.renderMeshBoneWeightChunk.Clear();
			renderDataWorkRef.flag.Clear();
			this.renderDataWorkArray.RemoveAndFill(new DataChunk(index), default(RenderManager.RenderDataWork));
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00012CA0 File Offset: 0x00010EA0
		public ref RenderManager.RenderDataWork GetRenderDataWorkRef(int index)
		{
			return this.renderDataWorkArray.GetRef(index);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00012CAE File Offset: 0x00010EAE
		public bool IsSetRenderDataWorkFlag(int index, int flag)
		{
			return this.GetRenderDataWorkRef(index).flag.IsSet(flag);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00012CC2 File Offset: 0x00010EC2
		public void SetBitsRenderDataWorkFlag(int index, int flag, bool sw)
		{
			this.GetRenderDataWorkRef(index).flag.SetBits(flag, sw);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00012CD7 File Offset: 0x00010ED7
		public void StartUse(ClothProcess cprocess, int handle)
		{
			RenderData rendererData = this.GetRendererData(handle);
			if (rendererData == null)
			{
				return;
			}
			rendererData.StartUse(cprocess);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00012CEB File Offset: 0x00010EEB
		public void EndUse(ClothProcess cprocess, int handle)
		{
			RenderData rendererData = this.GetRendererData(handle);
			if (rendererData == null)
			{
				return;
			}
			rendererData.EndUse(cprocess);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00012D00 File Offset: 0x00010F00
		private void PreRenderingUpdate()
		{
			if (this.renderDataDict.Count == 0)
			{
				return;
			}
			foreach (RenderData renderData in this.renderDataDict.Values)
			{
				if (renderData != null)
				{
					renderData.WriteMesh();
				}
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00012D6C File Offset: 0x00010F6C
		public void InformationLog(StringBuilder allsb)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("========== Render Manager ==========");
			if (!this.IsValid())
			{
				stringBuilder.AppendLine("Render Manager. Invalid.");
			}
			else
			{
				stringBuilder.AppendLine(string.Format("Render Manager. Count({0})", this.renderDataDict.Count));
				stringBuilder.AppendLine("  [RenderMeshBuffer]");
				stringBuilder.AppendLine("    -renderMeshPositions:" + this.renderMeshPositions.ToSummary());
				stringBuilder.AppendLine("    -renderMeshNormals:" + this.renderMeshNormals.ToSummary());
				stringBuilder.AppendLine("    -renderMeshTangents:" + this.renderMeshTangents.ToSummary());
				stringBuilder.AppendLine("  [RenderDataWork]");
				stringBuilder.AppendLine(string.Format("    -Count:{0}", this.renderDataWorkArray.Count));
				if (this.renderDataWorkArray.Count > 0)
				{
					for (int i = 0; i < this.renderDataWorkArray.Count; i++)
					{
						RenderManager.RenderDataWork renderDataWork = this.renderDataWorkArray[i];
						if (renderDataWork.IsValid())
						{
							stringBuilder.AppendLine(string.Format("    [{0}] MappingListCount:{1}", i, renderDataWork.mappingDataIndexList.Length));
						}
					}
				}
				stringBuilder.AppendLine("  [RenderData]");
				foreach (KeyValuePair<int, RenderData> keyValuePair in this.renderDataDict)
				{
					stringBuilder.Append(keyValuePair.Value.ToString());
				}
			}
			stringBuilder.AppendLine();
			Debug.Log(stringBuilder.ToString());
			allsb.Append(stringBuilder);
		}

		// Token: 0x04000396 RID: 918
		private Dictionary<int, RenderData> renderDataDict = new Dictionary<int, RenderData>();

		// Token: 0x04000397 RID: 919
		public const int RenderDataFlag_UseCustomMesh = 0;

		// Token: 0x04000398 RID: 920
		public const int RenderDataFlag_WritePositionNormal = 1;

		// Token: 0x04000399 RID: 921
		public const int RenderDataFlag_WriteBoneWeight = 2;

		// Token: 0x0400039A RID: 922
		public const int RenderDataFlag_HasMeshTangent = 4;

		// Token: 0x0400039B RID: 923
		public const int RenderDataFlag_HasTangent = 5;

		// Token: 0x0400039C RID: 924
		public const int RenderDataFlag_WriteTangent = 6;

		// Token: 0x0400039D RID: 925
		public const int RenderDataFlag_HasSkinnedMesh = 7;

		// Token: 0x0400039E RID: 926
		public const int RenderDataFlag_HasBoneWeight = 8;

		// Token: 0x0400039F RID: 927
		public ExNativeArray<RenderManager.RenderDataWork> renderDataWorkArray;

		// Token: 0x040003A0 RID: 928
		public ExNativeArray<float3> renderMeshPositions;

		// Token: 0x040003A1 RID: 929
		public ExNativeArray<float3> renderMeshNormals;

		// Token: 0x040003A2 RID: 930
		public ExNativeArray<float4> renderMeshTangents;

		// Token: 0x040003A3 RID: 931
		public ExNativeArray<BoneWeight> renderMeshBoneWeights;

		// Token: 0x040003A4 RID: 932
		private bool isValid;

		// Token: 0x040003A5 RID: 933
		private static readonly ProfilerMarker writeMeshTimeProfiler = new ProfilerMarker("WriteMesh");

		// Token: 0x02000087 RID: 135
		public struct RenderDataWork : IValid
		{
			// Token: 0x06000251 RID: 593 RVA: 0x00012F54 File Offset: 0x00011154
			public bool IsValid()
			{
				return this.renderMeshPositionAndNormalChunk.IsValid;
			}

			// Token: 0x17000034 RID: 52
			// (get) Token: 0x06000252 RID: 594 RVA: 0x00012F61 File Offset: 0x00011161
			public bool UseCustomMesh
			{
				get
				{
					return this.flag.IsSet(0);
				}
			}

			// Token: 0x17000035 RID: 53
			// (get) Token: 0x06000253 RID: 595 RVA: 0x00012F6F File Offset: 0x0001116F
			public bool HasMeshTangent
			{
				get
				{
					return this.flag.IsSet(4);
				}
			}

			// Token: 0x17000036 RID: 54
			// (get) Token: 0x06000254 RID: 596 RVA: 0x00012F7D File Offset: 0x0001117D
			public bool HasTangent
			{
				get
				{
					return this.flag.IsSet(5);
				}
			}

			// Token: 0x17000037 RID: 55
			// (get) Token: 0x06000255 RID: 597 RVA: 0x00012F8B File Offset: 0x0001118B
			public bool HasBoneWeight
			{
				get
				{
					return this.flag.IsSet(8);
				}
			}

			// Token: 0x06000256 RID: 598 RVA: 0x00012F9C File Offset: 0x0001119C
			public void AddMappingIndex(int mindex)
			{
				short num = (short)mindex;
				this.mappingDataIndexList.Add(in num);
			}

			// Token: 0x06000257 RID: 599 RVA: 0x00012FB9 File Offset: 0x000111B9
			public void RemoveMappingIndex(int mindex)
			{
				(ref this.mappingDataIndexList).MC2RemoveItemAtSwapBack((short)mindex);
			}

			// Token: 0x040003A6 RID: 934
			public BitField32 flag;

			// Token: 0x040003A7 RID: 935
			public DataChunk renderMeshPositionAndNormalChunk;

			// Token: 0x040003A8 RID: 936
			public DataChunk renderMeshTangentChunk;

			// Token: 0x040003A9 RID: 937
			public DataChunk renderMeshBoneWeightChunk;

			// Token: 0x040003AA RID: 938
			public BoneWeight centerBoneWeight;

			// Token: 0x040003AB RID: 939
			public FixedList32Bytes<short> mappingDataIndexList;
		}
	}
}
