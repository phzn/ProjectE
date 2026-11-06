using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000011 RID: 17
	public class ClothProcess : IDisposable, IValid, ITransform
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002828 File Offset: 0x00000A28
		internal void Init()
		{
			this.result.SetSuccess();
			this.InitDataResult.Clear();
			try
			{
				if (this.isDestory)
				{
					object obj = "Already destroyed components cannot be reinitialized.";
					Develop.LogError(in obj);
					throw new OperationCanceledException();
				}
				if (this.IsState(4))
				{
					throw new OperationCanceledException();
				}
				ClothSerializeData serializeData = this.cloth.SerializeData;
				ClothSerializeData2 serializeData2 = this.cloth.GetSerializeData2();
				this.SetState(0, false);
				this.cloth.InitAnimationProperty();
				if (!serializeData.IsValid())
				{
					this.result.SetResult(serializeData.VerificationResult);
					throw new OperationCanceledException();
				}
				ResultCode resultCode = this.GenerateStatusCheck();
				this.result.Merge(resultCode);
				if (resultCode.IsError())
				{
					throw new MagicaClothProcessingException();
				}
				this.SetState(4, true);
				bool flag = serializeData2.preBuildData.UsePreBuild();
				SharePreBuildData sharePreBuildData = null;
				if (flag)
				{
					this.SetState(12, true);
					ResultCode resultCode2 = serializeData2.preBuildData.DataValidate();
					if (resultCode2.IsFaild())
					{
						this.result.Merge(resultCode2);
						throw new OperationCanceledException();
					}
					sharePreBuildData = serializeData2.preBuildData.GetSharePreBuildData();
				}
				bool flag2 = false;
				if (!flag && serializeData2.initData != null && serializeData2.initData.HasData())
				{
					this.InitDataResult = serializeData2.initData.DataValidate(this);
					flag2 = this.InitDataResult.IsSuccess();
					this.InitDataResult.IsSuccess();
				}
				this.clothType = serializeData.clothType;
				this.reductionSettings = serializeData.reductionSetting;
				this.parameters = serializeData.GetClothParameters();
				this.clothTransformRecord = new TransformRecord(this.cloth.ClothTransform, !flag2);
				if (flag)
				{
					this.clothTransformRecord.scale = sharePreBuildData.buildScale;
				}
				if (flag2)
				{
					serializeData2.initData.clothTransformRecord.Deserialize(this.clothTransformRecord);
				}
				this.normalAdjustmentTransformRecord = new TransformRecord(serializeData.normalAlignmentSetting.adjustmentTransform ? serializeData.normalAlignmentSetting.adjustmentTransform : this.cloth.ClothTransform, !flag2);
				if (flag2)
				{
					serializeData2.initData.normalAdjustmentTransformRecord.Deserialize(this.normalAdjustmentTransformRecord);
				}
				PreBuildManager.ShareDeserializationData shareDeserializationData = (flag ? MagicaManager.PreBuild.RegisterPreBuildData(sharePreBuildData, true) : null);
				UniquePreBuildData uniquePreBuildData = (flag ? serializeData2.preBuildData.uniquePreBuildData : null);
				if (this.clothType == ClothProcess.ClothType.MeshCloth)
				{
					for (int i = 0; i < serializeData.sourceRenderers.Count; i++)
					{
						Renderer renderer = serializeData.sourceRenderers[i];
						if (renderer)
						{
							RenderSetupData renderSetupData = null;
							RenderSetupData.UniqueSerializationData uniqueSerializationData = null;
							if (flag)
							{
								renderSetupData = shareDeserializationData.renderSetupDataList[i];
								uniqueSerializationData = uniquePreBuildData.renderSetupDataList[i];
								if (renderSetupData.result.IsFaild())
								{
									renderSetupData.Dispose();
									this.result.SetError(Define.Result.PreBuild_SetupDeserializationError);
									throw new OperationCanceledException();
								}
							}
							RenderSetupSerializeData renderSetupSerializeData = (flag2 ? serializeData2.initData.clothSetupDataList[i] : null);
							int num = this.AddRenderer(renderer, renderSetupData, uniqueSerializationData, renderSetupSerializeData);
							if (num == 0)
							{
								this.result.SetError(Define.Result.ClothInit_FailedAddRenderer);
								throw new OperationCanceledException();
							}
							RenderData rendererData = MagicaManager.Render.GetRendererData(num);
							this.result.Merge(rendererData.Result);
							if (rendererData.Result.IsFaild())
							{
								throw new OperationCanceledException();
							}
						}
					}
				}
				else if (this.clothType == ClothProcess.ClothType.BoneCloth && !flag)
				{
					this.CreateBoneRenderSetupData(flag2 ? serializeData2.initData : null, this.clothType, serializeData.rootBones, null, serializeData.connectionMode);
				}
				else if (this.clothType == ClothProcess.ClothType.BoneSpring && !flag)
				{
					this.CreateBoneRenderSetupData(flag2 ? serializeData2.initData : null, this.clothType, serializeData.rootBones, serializeData.colliderCollisionConstraint.collisionBones, RenderSetupData.BoneConnectionMode.Line);
				}
				int count = serializeData.customSkinningSetting.skinningBones.Count;
				for (int j = 0; j < count; j++)
				{
					TransformRecord transformRecord = new TransformRecord(serializeData.customSkinningSetting.skinningBones[j], !flag2);
					if (flag2)
					{
						serializeData2.initData.customSkinningBoneRecords[j].Deserialize(transformRecord);
					}
					this.customSkinningBoneRecords.Add(transformRecord);
				}
				MagicaCloth syncPartnerCloth = this.cloth.SyncPartnerCloth;
				if (syncPartnerCloth)
				{
					MagicaManager.Team.comp2SyncPartnerCompMap.Add(this.cloth.GetInstanceID(), syncPartnerCloth.GetInstanceID());
					MagicaCloth magicaCloth = syncPartnerCloth;
					while (magicaCloth)
					{
						if (magicaCloth == this.cloth)
						{
							magicaCloth = null;
						}
						else
						{
							if (!magicaCloth.SyncPartnerCloth)
							{
								break;
							}
							magicaCloth = magicaCloth.SyncPartnerCloth;
						}
					}
					this.SyncTopCloth = magicaCloth;
					MagicaManager.Team.comp2SyncTopCompMap.Add(this.cloth.GetInstanceID(), this.SyncTopCloth.GetInstanceID());
				}
				this.result.SetSuccess();
				this.SetState(0, true);
				this.SetState(3, true);
				this.SetState(16, true);
				if (!this.cloth.isActiveAndEnabled)
				{
					MagicaManager.Team.AddMonitoringProcess(this);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (MagicaClothProcessingException)
			{
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				this.result.SetError(Define.Result.ClothProcess_Exception);
			}
			this.result.IsSuccess();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002DC4 File Offset: 0x00000FC4
		private int AddRenderer(Renderer ren, RenderSetupData referenceSetupData, RenderSetupData.UniqueSerializationData referenceUniqueSetupData, RenderSetupSerializeData referenceInitSetupData)
		{
			if (ren == null)
			{
				return 0;
			}
			if (this.renderHandleList == null)
			{
				return 0;
			}
			int num = ren.GetInstanceID();
			if (!this.renderHandleList.Contains(num))
			{
				num = MagicaManager.Render.AddRenderer(ren, referenceSetupData, referenceUniqueSetupData, referenceInitSetupData);
				if (num != 0)
				{
					object obj = this.lockObject;
					lock (obj)
					{
						if (!this.renderHandleList.Contains(num))
						{
							this.renderHandleList.Add(num);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002E58 File Offset: 0x00001058
		private void CreateBoneRenderSetupData(ClothInitSerializeData initData, ClothProcess.ClothType ctype, List<Transform> rootTransforms, List<Transform> collisionBones, RenderSetupData.BoneConnectionMode connectionMode)
		{
			this.boneClothSetupData = new RenderSetupData((initData != null) ? initData.clothSetupDataList[0] : null, (ctype == ClothProcess.ClothType.BoneSpring) ? RenderSetupData.SetupType.BoneSpring : RenderSetupData.SetupType.BoneCloth, this.clothTransformRecord.transform, rootTransforms, collisionBones, connectionMode, this.cloth.name);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002EA6 File Offset: 0x000010A6
		internal void StartUse()
		{
			if (!MagicaManager.IsPlaying())
			{
				return;
			}
			this.SetState(15, true);
			this.UpdateUse();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002EBF File Offset: 0x000010BF
		internal void EndUse()
		{
			if (!MagicaManager.IsPlaying())
			{
				return;
			}
			this.SetState(15, false);
			this.UpdateUse();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002ED8 File Offset: 0x000010D8
		internal void UpdateUse()
		{
			bool flag = this.IsState(15) && this.IsState(16);
			this.SetState(1, flag);
			MagicaManager.Team.SetEnable(this.TeamId, flag);
			if (this.renderHandleList != null)
			{
				foreach (int num in this.renderHandleList)
				{
					if (flag)
					{
						MagicaManager.Render.StartUse(this, num);
					}
					else
					{
						MagicaManager.Render.EndUse(this, num);
					}
				}
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002F78 File Offset: 0x00001178
		internal void DataUpdate()
		{
			this.cloth.SerializeData.DataValidate();
			this.cloth.serializeData2.DataValidate();
			if (MagicaManager.IsPlaying())
			{
				MagicaManager.Team.parameterDirtyList.Add(this);
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002FB4 File Offset: 0x000011B4
		internal bool StartRuntimeBuild()
		{
			if (this.IsValid() && this.IsState(3) && !this.IsState(5) && !this.IsState(12))
			{
				this.result.SetProcess();
				this.SetState(5, true);
				this.RuntimeBuildAsync(this.cts.Token);
				return true;
			}
			if (!this.result.IsError())
			{
				this.result.SetError(Define.Result.CreateCloth_CanNotStart);
			}
			object obj = "Cloth runtime build failure! [" + this.cloth.name + "] : " + this.result.GetResultString();
			Develop.LogError(in obj);
			return false;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003058 File Offset: 0x00001258
		internal bool AutoBuild()
		{
			bool flag = true;
			bool flag2;
			if (this.IsState(7))
			{
				flag2 = false;
			}
			else if (this.IsState(12))
			{
				flag2 = this.PreBuildDataConstruction();
			}
			else
			{
				flag2 = this.StartRuntimeBuild();
				if (flag2)
				{
					flag = false;
				}
			}
			if (flag && this.cloth != null)
			{
				Action<MagicaCloth, bool> onBuildComplete = this.cloth.OnBuildComplete;
				if (onBuildComplete != null)
				{
					onBuildComplete(this.cloth, flag2);
				}
			}
			return flag2;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000030C4 File Offset: 0x000012C4
		private async Task RuntimeBuildAsync(CancellationToken ct)
		{
			ClothProcess.<>c__DisplayClass10_0 CS$<>8__locals1 = new ClothProcess.<>c__DisplayClass10_0();
			CS$<>8__locals1.ct = ct;
			CS$<>8__locals1.<>4__this = this;
			this.isBuild = true;
			this.result.SetProcess();
			CS$<>8__locals1.renderMeshInfos = new List<ClothProcess.RenderMeshInfo>();
			CS$<>8__locals1.proxyMesh = null;
			try
			{
				ClothProcess.<>c__DisplayClass10_1 CS$<>8__locals2 = new ClothProcess.<>c__DisplayClass10_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.sdata = this.cloth.SerializeData;
				CS$<>8__locals2.sdata2 = this.cloth.GetSerializeData2();
				await Task.Delay(5);
				CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
				if (this.cloth.SyncPartnerCloth)
				{
					MagicaCloth magicaCloth = this.cloth.SyncPartnerCloth;
					while (magicaCloth != this.cloth && magicaCloth != null)
					{
						magicaCloth.Process.IncrementSuspendCounter();
						magicaCloth = magicaCloth.SyncPartnerCloth;
					}
				}
				CS$<>8__locals2.useManualVertexAttribute = false;
				if (CS$<>8__locals2.sdata.clothType == ClothProcess.ClothType.MeshCloth && CS$<>8__locals2.sdata2.vertexAttributeList != null && CS$<>8__locals2.sdata2.vertexAttributeList.Count > 0)
				{
					if (CS$<>8__locals2.sdata2.vertexAttributeList.Count != this.renderHandleList.Count)
					{
						this.result.SetError(Define.Result.CreateCloth_VertexAttributeListCountMismatch);
						throw new MagicaClothProcessingException();
					}
					for (int i = 0; i < this.renderHandleList.Count; i++)
					{
						VertexAttribute[] array = CS$<>8__locals2.sdata2.vertexAttributeList[i];
						if (array == null)
						{
							this.result.SetError(Define.Result.CreateCloth_VertexAttributeListIsNull);
							throw new MagicaClothProcessingException();
						}
						int num = this.renderHandleList[i];
						if (MagicaManager.Render.GetRendererData(num).setupData.vertexCount != array.Length)
						{
							this.result.SetError(Define.Result.CreateCloth_VertexAttributeListDataMismatch);
							throw new MagicaClothProcessingException();
						}
					}
					CS$<>8__locals2.useManualVertexAttribute = true;
				}
				CS$<>8__locals2.usePaintMap = false;
				CS$<>8__locals2.paintMapDataList = new List<ClothProcess.PaintMapData>();
				if (CS$<>8__locals2.sdata.clothType == ClothProcess.ClothType.MeshCloth && CS$<>8__locals2.sdata.paintMode != ClothSerializeData.PaintMode.Manual && !CS$<>8__locals2.useManualVertexAttribute)
				{
					ResultCode resultCode = this.GeneratePaintMapDataList(CS$<>8__locals2.paintMapDataList);
					if (resultCode.IsError())
					{
						this.result.Merge(resultCode);
						throw new MagicaClothProcessingException();
					}
					if (CS$<>8__locals2.paintMapDataList.Count != this.renderHandleList.Count)
					{
						this.result.SetError(Define.Result.CreateCloth_PaintMapCountMismatch);
						throw new MagicaClothProcessingException();
					}
					CS$<>8__locals2.usePaintMap = true;
				}
				CS$<>8__locals2.selectionData = ((CS$<>8__locals2.usePaintMap | CS$<>8__locals2.useManualVertexAttribute) ? new SelectionData() : CS$<>8__locals2.sdata2.selectionData.Clone());
				CS$<>8__locals2.boneAttributeDict = null;
				if (CS$<>8__locals2.sdata2.boneAttributeDict.Count > 0)
				{
					CS$<>8__locals2.boneAttributeDict = new Dictionary<int, VertexAttribute>(CS$<>8__locals2.sdata2.boneAttributeDict.Count);
					foreach (KeyValuePair<Transform, VertexAttribute> keyValuePair in CS$<>8__locals2.sdata2.boneAttributeDict)
					{
						if (keyValuePair.Key)
						{
							CS$<>8__locals2.boneAttributeDict.Add(keyValuePair.Key.GetInstanceID(), keyValuePair.Value);
						}
					}
				}
				await Task.Run(delegate
				{
					VirtualMesh virtualMesh = null;
					try
					{
						CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
						CS$<>8__locals2.CS$<>8__locals1.proxyMesh = new VirtualMesh("Proxy");
						CS$<>8__locals2.CS$<>8__locals1.proxyMesh.result.SetProcess();
						List<int> list = null;
						if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.clothType == ClothProcess.ClothType.MeshCloth)
						{
							CS$<>8__locals2.CS$<>8__locals1.proxyMesh.SetTransform(CS$<>8__locals2.CS$<>8__locals1.<>4__this.clothTransformRecord, null);
							object obj2 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.lockObject;
							lock (obj2)
							{
								list = new List<int>(CS$<>8__locals2.CS$<>8__locals1.<>4__this.renderHandleList);
							}
						}
						SelectionData selectionData = CS$<>8__locals2.selectionData;
						bool flag3 = selectionData != null && selectionData.IsValid();
						if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.clothType == ClothProcess.ClothType.MeshCloth)
						{
							if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.renderHandleList.Count == 0)
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.SetError(Define.Result.ClothProcess_InvalidRenderHandleList);
								throw new MagicaClothProcessingException();
							}
							for (int j = 0; j < list.Count; j++)
							{
								CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
								int num3 = list[j];
								RenderData rendererData = MagicaManager.Render.GetRendererData(num3);
								virtualMesh = new VirtualMesh("[" + rendererData.Name + "]");
								virtualMesh.result.SetProcess();
								virtualMesh.ImportFrom(rendererData, CS$<>8__locals2.sdata.GetUvChannel());
								if (virtualMesh.IsError)
								{
									CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(virtualMesh.result);
									throw new MagicaClothProcessingException();
								}
								SelectionData selectionData2 = CS$<>8__locals2.selectionData;
								if (CS$<>8__locals2.useManualVertexAttribute)
								{
									ResultCode resultCode2 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.GenerateSelectionDataFromVertexAttributeData(CS$<>8__locals2.CS$<>8__locals1.<>4__this.clothTransformRecord, virtualMesh, CS$<>8__locals2.sdata2.vertexAttributeList[j], out selectionData2);
									if (resultCode2.IsError())
									{
										CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(resultCode2);
										throw new MagicaClothProcessingException();
									}
									CS$<>8__locals2.selectionData.Merge(selectionData2);
								}
								if (CS$<>8__locals2.usePaintMap)
								{
									ResultCode resultCode3 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.GenerateSelectionDataFromPaintMap(CS$<>8__locals2.CS$<>8__locals1.<>4__this.clothTransformRecord, virtualMesh, CS$<>8__locals2.paintMapDataList[j], out selectionData2);
									if (resultCode3.IsError())
									{
										CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(resultCode3);
										throw new MagicaClothProcessingException();
									}
									CS$<>8__locals2.selectionData.Merge(selectionData2);
								}
								SelectionData selectionData3 = CS$<>8__locals2.selectionData;
								flag3 = selectionData3 != null && selectionData3.IsValid();
								CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
								if (selectionData2 != null && selectionData2.IsValid())
								{
									float num4 = virtualMesh.CalcSelectionMergin(CS$<>8__locals2.CS$<>8__locals1.<>4__this.reductionSettings);
									num4 = math.max(num4, 1E-05f);
									virtualMesh.SelectionMesh(selectionData2, CS$<>8__locals2.CS$<>8__locals1.<>4__this.clothTransformRecord.localToWorldMatrix, num4);
									if (virtualMesh.IsError)
									{
										CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(virtualMesh.result);
										throw new MagicaClothProcessingException();
									}
								}
								CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
								virtualMesh.result.SetSuccess();
								CS$<>8__locals2.CS$<>8__locals1.proxyMesh.AddMesh(virtualMesh);
								ClothProcess.RenderMeshInfo renderMeshInfo4 = new ClothProcess.RenderMeshInfo();
								renderMeshInfo4.renderHandle = num3;
								renderMeshInfo4.renderMeshContainer = new VirtualMeshContainer(virtualMesh);
								renderMeshInfo4.renderDataWorkIndex = rendererData.renderDataWorkIndex;
								virtualMesh = null;
								CS$<>8__locals2.CS$<>8__locals1.renderMeshInfos.Add(renderMeshInfo4);
							}
						}
						else if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.clothType == ClothProcess.ClothType.BoneCloth || CS$<>8__locals2.CS$<>8__locals1.<>4__this.clothType == ClothProcess.ClothType.BoneSpring)
						{
							CS$<>8__locals2.CS$<>8__locals1.proxyMesh.ImportFrom(CS$<>8__locals2.CS$<>8__locals1.<>4__this.boneClothSetupData, 0);
							if (CS$<>8__locals2.CS$<>8__locals1.proxyMesh.IsError)
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(CS$<>8__locals2.CS$<>8__locals1.proxyMesh.result);
								throw new MagicaClothProcessingException();
							}
							if (!flag3)
							{
								CS$<>8__locals2.selectionData = new SelectionData(CS$<>8__locals2.CS$<>8__locals1.proxyMesh, float4x4.identity);
								if (CS$<>8__locals2.selectionData.Count > 0)
								{
									CS$<>8__locals2.selectionData.Fill(VertexAttribute.Move);
									foreach (int num5 in CS$<>8__locals2.CS$<>8__locals1.<>4__this.boneClothSetupData.rootTransformIdList)
									{
										int transformIndexFromId = CS$<>8__locals2.CS$<>8__locals1.<>4__this.boneClothSetupData.GetTransformIndexFromId(num5);
										CS$<>8__locals2.selectionData.attributes[transformIndexFromId] = VertexAttribute.Fixed;
									}
									flag3 = CS$<>8__locals2.selectionData.IsValid();
								}
							}
							if (CS$<>8__locals2.boneAttributeDict != null)
							{
								foreach (KeyValuePair<int, VertexAttribute> keyValuePair2 in CS$<>8__locals2.boneAttributeDict)
								{
									int transformIndexFromId2 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.boneClothSetupData.GetTransformIndexFromId(keyValuePair2.Key);
									if (transformIndexFromId2 >= 0)
									{
										CS$<>8__locals2.selectionData.attributes[transformIndexFromId2] = keyValuePair2.Value;
									}
								}
							}
						}
						if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.clothType == ClothProcess.ClothType.MeshCloth && CS$<>8__locals2.CS$<>8__locals1.proxyMesh.VertexCount > 1)
						{
							CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
							if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.reductionSettings.IsEnabled)
							{
								CS$<>8__locals2.CS$<>8__locals1.proxyMesh.Reduction(CS$<>8__locals2.CS$<>8__locals1.<>4__this.reductionSettings, CS$<>8__locals2.CS$<>8__locals1.ct);
								if (CS$<>8__locals2.CS$<>8__locals1.proxyMesh.IsError)
								{
									CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(CS$<>8__locals2.CS$<>8__locals1.proxyMesh.result);
									throw new MagicaClothProcessingException();
								}
							}
						}
						if (!CS$<>8__locals2.CS$<>8__locals1.proxyMesh.joinIndices.IsCreated)
						{
							CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
							CS$<>8__locals2.CS$<>8__locals1.proxyMesh.joinIndices = new NativeArray<int>(CS$<>8__locals2.CS$<>8__locals1.proxyMesh.VertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
							JobUtility.SerialNumberRun(CS$<>8__locals2.CS$<>8__locals1.proxyMesh.joinIndices, CS$<>8__locals2.CS$<>8__locals1.proxyMesh.VertexCount);
						}
						CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
						CS$<>8__locals2.CS$<>8__locals1.proxyMesh.Optimization();
						if (CS$<>8__locals2.CS$<>8__locals1.proxyMesh.IsError)
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(CS$<>8__locals2.CS$<>8__locals1.proxyMesh.result);
							throw new MagicaClothProcessingException();
						}
						if (flag3)
						{
							CS$<>8__locals2.CS$<>8__locals1.proxyMesh.ApplySelectionAttribute(CS$<>8__locals2.selectionData);
							if (CS$<>8__locals2.CS$<>8__locals1.proxyMesh.IsError)
							{
								CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(CS$<>8__locals2.CS$<>8__locals1.proxyMesh.result);
								throw new MagicaClothProcessingException();
							}
						}
						CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
						CS$<>8__locals2.CS$<>8__locals1.proxyMesh.ConvertProxyMesh(CS$<>8__locals2.sdata, CS$<>8__locals2.CS$<>8__locals1.<>4__this.clothTransformRecord, CS$<>8__locals2.CS$<>8__locals1.<>4__this.customSkinningBoneRecords, CS$<>8__locals2.CS$<>8__locals1.<>4__this.normalAdjustmentTransformRecord);
						if (CS$<>8__locals2.CS$<>8__locals1.proxyMesh.IsError)
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(CS$<>8__locals2.CS$<>8__locals1.proxyMesh.result);
							throw new MagicaClothProcessingException();
						}
						if (CS$<>8__locals2.CS$<>8__locals1.proxyMesh.VertexCount > 32767)
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.SetError(Define.Result.ProxyMesh_Over32767Vertices);
							throw new MagicaClothProcessingException();
						}
						if (CS$<>8__locals2.CS$<>8__locals1.proxyMesh.EdgeCount > 32767)
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.SetError(Define.Result.ProxyMesh_Over32767Edges);
							throw new MagicaClothProcessingException();
						}
						if (CS$<>8__locals2.CS$<>8__locals1.proxyMesh.TriangleCount > 32767)
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.SetError(Define.Result.ProxyMesh_Over32767Triangles);
							throw new MagicaClothProcessingException();
						}
						CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
						if (CS$<>8__locals2.CS$<>8__locals1.proxyMesh.IsError)
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(CS$<>8__locals2.CS$<>8__locals1.proxyMesh.result);
							throw new MagicaClothProcessingException();
						}
						CS$<>8__locals2.CS$<>8__locals1.proxyMesh.result.SetSuccess();
						if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.clothType == ClothProcess.ClothType.MeshCloth)
						{
							foreach (ClothProcess.RenderMeshInfo renderMeshInfo5 in CS$<>8__locals2.CS$<>8__locals1.renderMeshInfos)
							{
								CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
								VirtualMesh shareVirtualMesh2 = renderMeshInfo5.renderMeshContainer.shareVirtualMesh;
								shareVirtualMesh2.Mapping(CS$<>8__locals2.CS$<>8__locals1.proxyMesh);
								if (shareVirtualMesh2.IsError)
								{
									CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(shareVirtualMesh2.result);
									throw new MagicaClothProcessingException();
								}
							}
						}
					}
					catch (MagicaClothProcessingException)
					{
						throw;
					}
					catch (OperationCanceledException)
					{
						throw;
					}
					catch (Exception ex2)
					{
						Debug.LogException(ex2);
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.SetError(Define.Result.ClothProcess_Exception);
						throw;
					}
					finally
					{
						if (virtualMesh != null)
						{
							virtualMesh.Dispose();
						}
					}
				}, CS$<>8__locals2.CS$<>8__locals1.ct);
				CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
				if (this.cloth == null)
				{
					throw new OperationCanceledException();
				}
				MagicaCloth syncCloth = this.cloth.SyncPartnerCloth;
				object obj;
				if (syncCloth != null)
				{
					int timeOutCount = 100;
					while (syncCloth != null && !syncCloth.Process.IsEnable && timeOutCount >= 0)
					{
						await Task.Delay(20);
						CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
						timeOutCount--;
					}
					if (syncCloth == null || !syncCloth.Process.IsEnable)
					{
						syncCloth = null;
						obj = "Sync timeout! Is there a deadlock between synchronous cloths?";
						Develop.LogWarning(in obj);
					}
				}
				CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
				if (this.cloth == null)
				{
					throw new OperationCanceledException();
				}
				if (!this.IsValid())
				{
					this.result.SetError(Define.Result.ClothProcess_Invalid);
					throw new MagicaClothProcessingException();
				}
				if (!MagicaManager.IsPlaying())
				{
					this.result.SetError(Define.Result.ClothProcess_Invalid);
					throw new MagicaClothProcessingException();
				}
				CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
				await Task.Run(delegate
				{
					try
					{
						CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
						ClothProcess <>4__this = CS$<>8__locals2.CS$<>8__locals1.<>4__this;
						VirtualMesh proxyMesh2 = CS$<>8__locals2.CS$<>8__locals1.proxyMesh;
						ClothParameters clothParameters = CS$<>8__locals2.CS$<>8__locals1.<>4__this.parameters;
						<>4__this.distanceConstraintData = DistanceConstraint.CreateData(proxyMesh2, in clothParameters);
						if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.distanceConstraintData != null && CS$<>8__locals2.CS$<>8__locals1.<>4__this.distanceConstraintData.result.IsError())
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(CS$<>8__locals2.CS$<>8__locals1.<>4__this.distanceConstraintData.result);
							throw new MagicaClothProcessingException();
						}
						CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
						ClothProcess <>4__this2 = CS$<>8__locals2.CS$<>8__locals1.<>4__this;
						VirtualMesh proxyMesh3 = CS$<>8__locals2.CS$<>8__locals1.proxyMesh;
						clothParameters = CS$<>8__locals2.CS$<>8__locals1.<>4__this.parameters;
						<>4__this2.bendingConstraintData = TriangleBendingConstraint.CreateData(proxyMesh3, in clothParameters);
						if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.bendingConstraintData != null && CS$<>8__locals2.CS$<>8__locals1.<>4__this.bendingConstraintData.result.IsError())
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(CS$<>8__locals2.CS$<>8__locals1.<>4__this.bendingConstraintData.result);
							throw new MagicaClothProcessingException();
						}
						CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
						ClothProcess <>4__this3 = CS$<>8__locals2.CS$<>8__locals1.<>4__this;
						VirtualMesh proxyMesh4 = CS$<>8__locals2.CS$<>8__locals1.proxyMesh;
						clothParameters = CS$<>8__locals2.CS$<>8__locals1.<>4__this.parameters;
						<>4__this3.inertiaConstraintData = InertiaConstraint.CreateData(proxyMesh4, in clothParameters);
						if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.inertiaConstraintData != null && CS$<>8__locals2.CS$<>8__locals1.<>4__this.inertiaConstraintData.result.IsError())
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.Merge(CS$<>8__locals2.CS$<>8__locals1.<>4__this.inertiaConstraintData.result);
							throw new MagicaClothProcessingException();
						}
						if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.IsError())
						{
							throw new MagicaClothProcessingException();
						}
					}
					catch (MagicaClothProcessingException)
					{
						throw;
					}
					catch (OperationCanceledException)
					{
						throw;
					}
					catch (Exception ex3)
					{
						Debug.LogException(ex3);
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.result.SetError(Define.Result.Constraint_Exception);
						throw;
					}
				}, CS$<>8__locals2.CS$<>8__locals1.ct);
				CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
				if (this.cloth == null)
				{
					throw new OperationCanceledException();
				}
				obj = this.lockObject;
				lock (obj)
				{
					this.ProxyMeshContainer = new VirtualMeshContainer(CS$<>8__locals2.CS$<>8__locals1.proxyMesh);
					CS$<>8__locals2.CS$<>8__locals1.proxyMesh = null;
					ClothManager cloth = MagicaManager.Cloth;
					ClothParameters parameters = this.parameters;
					this.TeamId = cloth.AddCloth(this, in parameters);
					if (this.TeamId <= 0)
					{
						this.result.SetError(Define.Result.ClothProcess_OverflowTeamCount4096);
						throw new MagicaClothProcessingException();
					}
					MagicaManager.Team.parameterDirtyList.Add(this);
					MagicaManager.VMesh.RegisterProxyMesh(this.TeamId, this.ProxyMeshContainer);
					MagicaManager.Simulation.RegisterProxyMesh(this);
					MagicaManager.Collider.Register(this);
				}
				CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
				MagicaManager.Simulation.RegisterConstraint(this);
				obj = this.lockObject;
				lock (obj)
				{
					if (this.clothType == ClothProcess.ClothType.MeshCloth)
					{
						foreach (ClothProcess.RenderMeshInfo renderMeshInfo in CS$<>8__locals2.CS$<>8__locals1.renderMeshInfos)
						{
							VirtualMesh shareVirtualMesh = renderMeshInfo.renderMeshContainer.shareVirtualMesh;
							if (!shareVirtualMesh.IsError && shareVirtualMesh.IsMapping && shareVirtualMesh.IsValid())
							{
								renderMeshInfo.mappingChunk = MagicaManager.VMesh.RegisterMappingMesh(this.TeamId, renderMeshInfo.renderMeshContainer, renderMeshInfo.renderDataWorkIndex);
							}
						}
					}
					foreach (ClothProcess.RenderMeshInfo renderMeshInfo2 in CS$<>8__locals2.CS$<>8__locals1.renderMeshInfos)
					{
						this.renderMeshInfoList.Add(renderMeshInfo2);
					}
					CS$<>8__locals2.CS$<>8__locals1.renderMeshInfos.Clear();
				}
				CS$<>8__locals2.CS$<>8__locals1.ct.ThrowIfCancellationRequested();
				this.UpdateUse();
				this.result.SetSuccess();
				this.SetState(6, true);
				CS$<>8__locals2 = null;
				syncCloth = null;
			}
			catch (MagicaClothProcessingException)
			{
				if (!this.result.IsError())
				{
					this.result.SetError(Define.Result.ClothProcess_UnknownError);
				}
			}
			catch (OperationCanceledException)
			{
				this.result.SetCancel();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				this.result.SetError(Define.Result.ClothProcess_Exception);
			}
			finally
			{
				List<ClothProcess.RenderMeshInfo>.Enumerator enumerator2 = CS$<>8__locals1.renderMeshInfos.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						ClothProcess.RenderMeshInfo renderMeshInfo3 = enumerator2.Current;
						if (renderMeshInfo3 != null)
						{
							VirtualMeshContainer renderMeshContainer = renderMeshInfo3.renderMeshContainer;
							if (renderMeshContainer != null)
							{
								renderMeshContainer.Dispose();
							}
						}
					}
				}
				finally
				{
					int num2;
					if (num2 < 0)
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				VirtualMesh proxyMesh = CS$<>8__locals1.proxyMesh;
				if (proxyMesh != null)
				{
					proxyMesh.Dispose();
				}
				if (this.cloth != null && this.cloth.SyncPartnerCloth)
				{
					MagicaCloth magicaCloth2 = this.cloth.SyncPartnerCloth;
					while (magicaCloth2 != this.cloth && magicaCloth2 != null)
					{
						magicaCloth2.Process.DecrementSuspendCounter();
						magicaCloth2 = magicaCloth2.SyncPartnerCloth;
					}
				}
				this.isBuild = false;
				if (this.isDestory)
				{
					this.DisposeInternal();
				}
				else if (this.cloth != null)
				{
					if (this.result.IsFaild())
					{
						object obj = "Cloth runtime build failure! [" + this.cloth.name + "] : " + this.result.GetResultString();
						Develop.LogError(in obj);
					}
					Action<MagicaCloth, bool> onBuildComplete = this.cloth.OnBuildComplete;
					if (onBuildComplete != null)
					{
						onBuildComplete(this.cloth, this.result.IsSuccess());
					}
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003110 File Offset: 0x00001310
		public ResultCode GenerateSelectionDataFromPaintMap(TransformRecord clothTransformRecord, VirtualMesh renderMesh, ClothProcess.PaintMapData paintMapData, out SelectionData selectionData)
		{
			ResultCode resultCode = default(ResultCode);
			resultCode.SetProcess();
			selectionData = new SelectionData();
			try
			{
				if (paintMapData == null)
				{
					resultCode.SetError(Define.Result.CreateCloth_PaintMapCountMismatch);
					throw new MagicaClothProcessingException();
				}
				int vertexCount = renderMesh.VertexCount;
				using (NativeArray<float3> nativeArray = new NativeArray<float3>(vertexCount, Allocator.TempJob, NativeArrayOptions.ClearMemory))
				{
					using (NativeArray<VertexAttribute> nativeArray2 = new NativeArray<VertexAttribute>(vertexCount, Allocator.TempJob, NativeArrayOptions.ClearMemory))
					{
						float4x4 float4x = clothTransformRecord.worldToLocalMatrix;
						float4x4 float4x2 = MathUtility.Transform(in renderMesh.initLocalToWorld, in float4x);
						int2 @int = new int2(paintMapData.paintMapWidth, paintMapData.paintMapHeight);
						using (NativeArray<Color32> nativeArray3 = new NativeArray<Color32>(paintMapData.paintData, Allocator.TempJob))
						{
							new ClothProcess.GenerateSelectionJob
							{
								offset = 0,
								positionList = nativeArray,
								attributeList = nativeArray2,
								attributeMapWidth = paintMapData.paintMapWidth,
								toM = float4x2,
								xySize = @int,
								attributeReadFlag = paintMapData.paintReadFlag,
								attributeMapData = nativeArray3,
								uvs = renderMesh.uv.GetNativeArray(),
								vertexs = renderMesh.localPositions.GetNativeArray()
							}.Run(vertexCount);
							selectionData.positions = nativeArray.ToArray();
							selectionData.attributes = nativeArray2.ToArray();
							SelectionData selectionData2 = selectionData;
							float value = renderMesh.maxVertexDistance.Value;
							selectionData2.maxConnectionDistance = MathUtility.TransformDistance(in value, in float4x2);
							selectionData.userEdit = true;
							resultCode.SetSuccess();
						}
					}
				}
			}
			catch (MagicaClothProcessingException)
			{
				if (resultCode.IsNone())
				{
					resultCode.SetError(Define.Result.CreateCloth_InvalidPaintMap);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				resultCode.SetError(Define.Result.CreateCloth_InvalidPaintMap);
			}
			return resultCode;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003348 File Offset: 0x00001548
		public ResultCode GeneratePaintMapDataList(List<ClothProcess.PaintMapData> dataList)
		{
			ResultCode resultCode = default(ResultCode);
			resultCode.SetProcess();
			try
			{
				int count = this.cloth.SerializeData.paintMaps.Count;
				ExBitFlag8 exBitFlag = new ExBitFlag8(3);
				if (this.cloth.SerializeData.paintMode == ClothSerializeData.PaintMode.Texture_Fixed_Move_Limit)
				{
					exBitFlag.SetFlag(4, true);
				}
				for (int i = 0; i < count; i++)
				{
					Texture2D texture2D = this.cloth.SerializeData.paintMaps[i];
					if (texture2D == null)
					{
						resultCode.SetError(Define.Result.Init_InvalidPaintMap);
						throw new MagicaClothProcessingException();
					}
					if (!texture2D.isReadable)
					{
						resultCode.SetError(Define.Result.Init_PaintMapNotReadable);
						throw new MagicaClothProcessingException();
					}
					int num = texture2D.width;
					int num2 = texture2D.height;
					int num3 = num * num2;
					int num4 = 1;
					while (num4 < texture2D.mipmapCount && num3 > 16384)
					{
						num4++;
						num3 /= 4;
						num /= 2;
						num2 /= 2;
					}
					dataList.Add(new ClothProcess.PaintMapData
					{
						paintData = texture2D.GetPixels32(num4 - 1),
						paintMapWidth = num,
						paintMapHeight = num2,
						paintReadFlag = exBitFlag
					});
				}
				resultCode.SetSuccess();
			}
			catch (MagicaClothProcessingException)
			{
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				resultCode.SetError(Define.Result.Init_InvalidPaintMap);
			}
			return resultCode;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000034D8 File Offset: 0x000016D8
		public ResultCode GenerateSelectionDataFromVertexAttributeData(TransformRecord clothTransformRecord, VirtualMesh renderMesh, VertexAttribute[] vertexAttributeArray, out SelectionData selectionData)
		{
			ResultCode resultCode = default(ResultCode);
			resultCode.SetProcess();
			selectionData = new SelectionData();
			try
			{
				int vertexCount = renderMesh.VertexCount;
				using (NativeArray<float3> nativeArray = new NativeArray<float3>(vertexCount, Allocator.TempJob, NativeArrayOptions.ClearMemory))
				{
					float4x4 float4x = clothTransformRecord.worldToLocalMatrix;
					float4x4 float4x2 = MathUtility.Transform(in renderMesh.initLocalToWorld, in float4x);
					JobUtility.TransformPositionRun(renderMesh.localPositions.GetNativeArray(), nativeArray, vertexCount, in float4x2);
					selectionData.positions = nativeArray.ToArray();
					selectionData.attributes = vertexAttributeArray;
					SelectionData selectionData2 = selectionData;
					float value = renderMesh.maxVertexDistance.Value;
					selectionData2.maxConnectionDistance = MathUtility.TransformDistance(in value, in float4x2);
					selectionData.userEdit = true;
					resultCode.SetSuccess();
				}
			}
			catch (MagicaClothProcessingException)
			{
				if (resultCode.IsNone())
				{
					resultCode.SetError(Define.Result.CreateCloth_InvalidVertexAttributeData);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				resultCode.SetError(Define.Result.CreateCloth_InvalidVertexAttributeData);
			}
			return resultCode;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000035E8 File Offset: 0x000017E8
		internal bool PreBuildDataConstruction()
		{
			if (!this.IsState(12))
			{
				return false;
			}
			if (!this.IsState(3))
			{
				return false;
			}
			this.result.SetProcess();
			ClothSerializeData serializeData = this.cloth.SerializeData;
			ClothSerializeData2 serializeData2 = this.cloth.GetSerializeData2();
			VirtualMeshContainer virtualMeshContainer = null;
			List<VirtualMeshContainer> list = new List<VirtualMeshContainer>();
			try
			{
				UniquePreBuildData uniquePreBuildData = serializeData2.preBuildData.uniquePreBuildData;
				PreBuildManager.ShareDeserializationData preBuildData = MagicaManager.PreBuild.GetPreBuildData(serializeData2.preBuildData.GetSharePreBuildData());
				try
				{
					virtualMeshContainer = new VirtualMeshContainer(preBuildData.proxyMesh);
					if (virtualMeshContainer.shareVirtualMesh.IsError)
					{
						this.result.Merge(virtualMeshContainer.shareVirtualMesh.result);
						throw new MagicaClothProcessingException();
					}
					virtualMeshContainer.uniqueData = uniquePreBuildData.proxyMesh;
					for (int i = 0; i < preBuildData.renderMeshList.Count; i++)
					{
						VirtualMeshContainer virtualMeshContainer2 = new VirtualMeshContainer(preBuildData.renderMeshList[i]);
						list.Add(virtualMeshContainer2);
						if (virtualMeshContainer2.shareVirtualMesh.IsError)
						{
							this.result.Merge(virtualMeshContainer2.shareVirtualMesh.result);
							throw new MagicaClothProcessingException();
						}
						virtualMeshContainer2.uniqueData = uniquePreBuildData.renderMeshList[i];
					}
					this.inertiaConstraintData = preBuildData.inertiaConstraintData;
					this.distanceConstraintData = preBuildData.distanceConstraintData;
					this.bendingConstraintData = preBuildData.bendingConstraintData;
				}
				catch
				{
					throw;
				}
				finally
				{
				}
				try
				{
					this.ProxyMeshContainer = virtualMeshContainer;
					virtualMeshContainer = null;
					ClothManager cloth = MagicaManager.Cloth;
					ClothParameters parameters = this.parameters;
					this.TeamId = cloth.AddCloth(this, in parameters);
					if (this.TeamId <= 0)
					{
						this.result.SetError(Define.Result.ClothProcess_OverflowTeamCount4096);
						throw new MagicaClothProcessingException();
					}
					MagicaManager.Team.parameterDirtyList.Add(this);
					MagicaManager.VMesh.RegisterProxyMesh(this.TeamId, this.ProxyMeshContainer);
					MagicaManager.Simulation.RegisterProxyMesh(this);
					MagicaManager.Collider.Register(this);
					MagicaManager.Simulation.RegisterConstraint(this);
					for (int j = 0; j < list.Count; j++)
					{
						VirtualMeshContainer virtualMeshContainer3 = list[j];
						VirtualMesh shareVirtualMesh = virtualMeshContainer3.shareVirtualMesh;
						if (!shareVirtualMesh.IsError && shareVirtualMesh.IsMapping && shareVirtualMesh.IsValid())
						{
							list[j] = null;
							int num = this.renderHandleList[j];
							RenderData rendererData = MagicaManager.Render.GetRendererData(num);
							DataChunk dataChunk = MagicaManager.VMesh.RegisterMappingMesh(this.TeamId, virtualMeshContainer3, rendererData.renderDataWorkIndex);
							shareVirtualMesh.result.SetSuccess();
							ClothProcess.RenderMeshInfo renderMeshInfo = new ClothProcess.RenderMeshInfo
							{
								renderHandle = num,
								renderMeshContainer = virtualMeshContainer3,
								mappingChunk = dataChunk,
								renderDataWorkIndex = rendererData.renderDataWorkIndex
							};
							this.renderMeshInfoList.Add(renderMeshInfo);
						}
					}
					this.UpdateUse();
				}
				catch
				{
					throw;
				}
				finally
				{
				}
				this.result.SetSuccess();
				this.SetState(6, true);
			}
			catch (MagicaClothProcessingException)
			{
				if (!this.result.IsError())
				{
					this.result.SetError(Define.Result.PreBuild_UnknownError);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				this.result.SetError(Define.Result.PreBuild_Exception);
			}
			finally
			{
				list.ForEach(delegate(VirtualMeshContainer x)
				{
					if (x != null)
					{
						x.Dispose();
					}
				});
				list.Clear();
				if (virtualMeshContainer != null)
				{
					virtualMeshContainer.Dispose();
				}
				if (this.result.IsFaild())
				{
					object obj = "Cloth Pre-Build construction failure! [" + this.cloth.name + "] : " + this.result.GetResultString();
					Develop.LogError(in obj);
				}
			}
			return this.result.IsSuccess();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003A2C File Offset: 0x00001C2C
		internal void UpdateCullingAnimatorAndRenderers()
		{
			CullingSettings cullingSettings = this.cloth.SerializeData.cullingSettings;
			if (cullingSettings.cameraCullingMode == CullingSettings.CameraCullingMode.AnimatorLinkage || cullingSettings.cameraCullingMethod == CullingSettings.CameraCullingMethod.AutomaticRenderer || this.cloth.SerializeData.updateMode == ClothUpdateMode.AnimatorLinkage)
			{
				this.interlockingAnimator = this.cloth.GetComponentInParent<Animator>();
			}
			if (cullingSettings.cameraCullingMethod == CullingSettings.CameraCullingMethod.AutomaticRenderer && this.interlockingAnimator)
			{
				this.interlockingAnimatorRenderers.Clear();
				this.interlockingAnimator.GetComponentsInChildren<Renderer>(this.interlockingAnimatorRenderers);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003AB2 File Offset: 0x00001CB2
		internal void UpdateRendererUse()
		{
			this.renderHandleList.ForEach(delegate(int handle)
			{
				MagicaManager.Render.GetRendererData(handle).UpdateUse(this, 0);
			});
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00003ACB File Offset: 0x00001CCB
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00003AD3 File Offset: 0x00001CD3
		public MagicaCloth cloth { get; internal set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00003ADC File Offset: 0x00001CDC
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00003AE4 File Offset: 0x00001CE4
		public MagicaCloth SyncTopCloth { get; internal set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00003AED File Offset: 0x00001CED
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00003AF5 File Offset: 0x00001CF5
		internal TransformRecord clothTransformRecord { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00003AFE File Offset: 0x00001CFE
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00003B06 File Offset: 0x00001D06
		internal TransformRecord normalAdjustmentTransformRecord { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003B0F File Offset: 0x00001D0F
		public ResultCode Result
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00003B17 File Offset: 0x00001D17
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00003B1F File Offset: 0x00001D1F
		public ResultCode InitDataResult { get; internal set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003B28 File Offset: 0x00001D28
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00003B30 File Offset: 0x00001D30
		internal ClothProcess.ClothType clothType { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00003B39 File Offset: 0x00001D39
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00003B41 File Offset: 0x00001D41
		public ClothParameters parameters { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003B4A File Offset: 0x00001D4A
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00003B52 File Offset: 0x00001D52
		public VirtualMeshContainer ProxyMeshContainer { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003B5B File Offset: 0x00001D5B
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00003B63 File Offset: 0x00001D63
		public int TeamId { get; private set; }

		// Token: 0x06000048 RID: 72 RVA: 0x00003B6C File Offset: 0x00001D6C
		public BitField32 GetStateFlag()
		{
			return this.stateFlag;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003B74 File Offset: 0x00001D74
		public bool IsState(int state)
		{
			return this.stateFlag.IsSet(state);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003B82 File Offset: 0x00001D82
		public void SetState(int state, bool sw)
		{
			this.stateFlag.SetBits(state, sw);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003B91 File Offset: 0x00001D91
		public bool IsValid()
		{
			return this.IsState(0);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003B9A File Offset: 0x00001D9A
		public bool IsRunning()
		{
			return this.IsState(6);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003BA3 File Offset: 0x00001DA3
		public bool IsCameraCullingInvisible()
		{
			return this.IsState(8);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003BAC File Offset: 0x00001DAC
		public bool IsCameraCullingKeep()
		{
			return this.IsState(9);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003BB6 File Offset: 0x00001DB6
		public bool IsDistanceCullingInvisible()
		{
			return this.IsState(13);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003BC0 File Offset: 0x00001DC0
		public bool IsSkipWriting()
		{
			return this.IsState(10);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003BCA File Offset: 0x00001DCA
		public bool IsUpdateTangent()
		{
			return this.IsState(14);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003BD4 File Offset: 0x00001DD4
		public bool IsEnable
		{
			get
			{
				return this.IsValid() && this.TeamId != 0 && MagicaManager.Team.IsEnable(this.TeamId);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public bool HasProxyMesh
		{
			get
			{
				if (!this.IsValid() || this.TeamId == 0)
				{
					return false;
				}
				VirtualMeshContainer proxyMeshContainer = this.ProxyMeshContainer;
				bool? flag;
				if (proxyMeshContainer == null)
				{
					flag = null;
				}
				else
				{
					VirtualMesh shareVirtualMesh = proxyMeshContainer.shareVirtualMesh;
					flag = ((shareVirtualMesh != null) ? new bool?(shareVirtualMesh.IsSuccess) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003C52 File Offset: 0x00001E52
		public string Name
		{
			get
			{
				if (!(this.cloth != null))
				{
					return "(none)";
				}
				return this.cloth.name;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003C74 File Offset: 0x00001E74
		public ClothProcess()
		{
			this.result = ResultCode.Empty;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003CE4 File Offset: 0x00001EE4
		public void Dispose()
		{
			object obj = this.lockObject;
			lock (obj)
			{
				this.isDestory = true;
				this.SetState(0, false);
				this.result.Clear();
				this.cts.Cancel();
			}
			this.DisposeInternal();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003D50 File Offset: 0x00001F50
		private void DisposeInternal()
		{
			object obj = this.lockObject;
			lock (obj)
			{
				if (this.isDestoryInternal)
				{
					return;
				}
				if (this.isBuild)
				{
					return;
				}
				SimulationManager simulation = MagicaManager.Simulation;
				if (simulation != null)
				{
					simulation.ExitProxyMesh(this);
				}
				VirtualMeshManager vmesh = MagicaManager.VMesh;
				if (vmesh != null)
				{
					vmesh.ExitProxyMesh(this.TeamId);
				}
				ColliderManager collider = MagicaManager.Collider;
				if (collider != null)
				{
					collider.Exit(this);
				}
				ClothManager cloth = MagicaManager.Cloth;
				if (cloth != null)
				{
					cloth.RemoveCloth(this);
				}
				foreach (ClothProcess.RenderMeshInfo renderMeshInfo in this.renderMeshInfoList)
				{
					if (renderMeshInfo != null)
					{
						VirtualMeshContainer renderMeshContainer = renderMeshInfo.renderMeshContainer;
						if (renderMeshContainer != null)
						{
							renderMeshContainer.Dispose();
						}
					}
				}
				this.renderMeshInfoList.Clear();
				this.renderMeshInfoList = null;
				foreach (int num in this.renderHandleList)
				{
					RenderManager render = MagicaManager.Render;
					if (render != null)
					{
						render.RemoveRenderer(num);
					}
				}
				this.renderHandleList.Clear();
				this.renderHandleList = null;
				RenderSetupData renderSetupData = this.boneClothSetupData;
				if (renderSetupData != null)
				{
					renderSetupData.Dispose();
				}
				this.boneClothSetupData = null;
				VirtualMeshContainer proxyMeshContainer = this.ProxyMeshContainer;
				if (proxyMeshContainer != null)
				{
					proxyMeshContainer.Dispose();
				}
				this.ProxyMeshContainer = null;
				this.colliderDict.Clear();
				this.interlockingAnimator = null;
				this.interlockingAnimatorRenderers.Clear();
				PreBuildManager preBuild = MagicaManager.PreBuild;
				if (preBuild != null)
				{
					SharePreBuildData sharePreBuildData;
					if (!(this.cloth != null))
					{
						sharePreBuildData = null;
					}
					else
					{
						ClothSerializeData2 serializeData = this.cloth.GetSerializeData2();
						sharePreBuildData = ((serializeData != null) ? serializeData.preBuildData.GetSharePreBuildData() : null);
					}
					preBuild.UnregisterPreBuildData(sharePreBuildData);
				}
				this.SyncTopCloth = null;
				int instanceID = this.cloth.GetInstanceID();
				TeamManager team = MagicaManager.Team;
				if (team != null)
				{
					team.comp2SuspendCounterMap.Remove(instanceID);
				}
				TeamManager team2 = MagicaManager.Team;
				if (team2 != null)
				{
					team2.comp2TeamIdMap.Remove(instanceID);
				}
				TeamManager team3 = MagicaManager.Team;
				if (team3 != null)
				{
					team3.comp2SyncPartnerCompMap.Remove(instanceID);
				}
				TeamManager team4 = MagicaManager.Team;
				if (team4 != null)
				{
					team4.comp2SyncTopCompMap.Remove(instanceID);
				}
				this.isDestoryInternal = true;
			}
			TeamManager team5 = MagicaManager.Team;
			if (team5 == null)
			{
				return;
			}
			team5.RemoveMonitoringProcess(this);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003FF0 File Offset: 0x000021F0
		internal void IncrementSuspendCounter()
		{
			TeamManager team = MagicaManager.Team;
			int instanceID = this.cloth.GetInstanceID();
			int num;
			if (team.comp2SuspendCounterMap.TryGetValue(instanceID, out num))
			{
				num++;
				team.comp2SuspendCounterMap[instanceID] = num;
				return;
			}
			team.comp2SuspendCounterMap.Add(instanceID, 1);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004040 File Offset: 0x00002240
		internal void DecrementSuspendCounter()
		{
			TeamManager team = MagicaManager.Team;
			int instanceID = this.cloth.GetInstanceID();
			int num;
			if (team.comp2SuspendCounterMap.TryGetValue(instanceID, out num))
			{
				num--;
				if (num > 0)
				{
					team.comp2SuspendCounterMap[instanceID] = num;
					return;
				}
				team.comp2SuspendCounterMap.Remove(instanceID);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004094 File Offset: 0x00002294
		internal int GetSuspendCounter()
		{
			TeamManager team = MagicaManager.Team;
			int instanceID = this.cloth.GetInstanceID();
			int num;
			if (team.comp2SuspendCounterMap.TryGetValue(instanceID, out num))
			{
				return num;
			}
			return 0;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000040C4 File Offset: 0x000022C4
		public ClothProcess.RenderMeshInfo GetRenderMeshInfo(int index)
		{
			if (index >= 0 && index < this.renderMeshInfoList.Count)
			{
				return this.renderMeshInfoList[index];
			}
			return null;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000040E6 File Offset: 0x000022E6
		internal void SyncParameters()
		{
			this.parameters = this.cloth.SerializeData.GetClothParameters();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004100 File Offset: 0x00002300
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			this.cloth.SerializeData.GetUsedTransform(transformSet);
			this.cloth.serializeData2.GetUsedTransform(transformSet);
			TransformRecord clothTransformRecord = this.clothTransformRecord;
			if (clothTransformRecord != null)
			{
				clothTransformRecord.GetUsedTransform(transformSet);
			}
			RenderSetupData renderSetupData = this.boneClothSetupData;
			if (renderSetupData != null)
			{
				renderSetupData.GetUsedTransform(transformSet);
			}
			this.renderHandleList.ForEach(delegate(int handle)
			{
				MagicaManager.Render.GetRendererData(handle).GetUsedTransform(transformSet);
			});
			this.customSkinningBoneRecords.ForEach(delegate(TransformRecord rd)
			{
				rd.GetUsedTransform(transformSet);
			});
			TransformRecord normalAdjustmentTransformRecord = this.normalAdjustmentTransformRecord;
			if (normalAdjustmentTransformRecord != null)
			{
				normalAdjustmentTransformRecord.GetUsedTransform(transformSet);
			}
			if (transformSet.Contains(null))
			{
				transformSet.Remove(null);
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000041D4 File Offset: 0x000023D4
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			this.cloth.SerializeData.ReplaceTransform(replaceDict);
			this.cloth.serializeData2.ReplaceTransform(replaceDict);
			TransformRecord clothTransformRecord = this.clothTransformRecord;
			if (clothTransformRecord != null)
			{
				clothTransformRecord.ReplaceTransform(replaceDict);
			}
			RenderSetupData renderSetupData = this.boneClothSetupData;
			if (renderSetupData != null)
			{
				renderSetupData.ReplaceTransform(replaceDict);
			}
			this.renderHandleList.ForEach(delegate(int handle)
			{
				MagicaManager.Render.GetRendererData(handle).ReplaceTransform(replaceDict);
			});
			this.customSkinningBoneRecords.ForEach(delegate(TransformRecord rd)
			{
				rd.ReplaceTransform(replaceDict);
			});
			TransformRecord normalAdjustmentTransformRecord = this.normalAdjustmentTransformRecord;
			if (normalAdjustmentTransformRecord == null)
			{
				return;
			}
			normalAdjustmentTransformRecord.ReplaceTransform(replaceDict);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000428C File Offset: 0x0000248C
		internal void SetSkipWriting(bool sw)
		{
			this.SetState(10, sw);
			MagicaManager.Team.skipWritingDirtyList.Add(this);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000042A8 File Offset: 0x000024A8
		internal ClothUpdateMode GetClothUpdateMode()
		{
			ClothUpdateMode updateMode = this.cloth.SerializeData.updateMode;
			if (updateMode <= ClothUpdateMode.Unscaled)
			{
				return this.cloth.SerializeData.updateMode;
			}
			if (updateMode != ClothUpdateMode.AnimatorLinkage)
			{
				object obj = string.Format("[{0}] Unknown Cloth Update Mode:{1}", this.cloth.name, this.cloth.SerializeData.updateMode);
				Develop.LogError(in obj);
				return ClothUpdateMode.Normal;
			}
			if (this.interlockingAnimator)
			{
				switch (this.interlockingAnimator.updateMode)
				{
				case AnimatorUpdateMode.Normal:
					return ClothUpdateMode.Normal;
				case AnimatorUpdateMode.AnimatePhysics:
					return ClothUpdateMode.UnityPhysics;
				case AnimatorUpdateMode.UnscaledTime:
					return ClothUpdateMode.Unscaled;
				}
			}
			return ClothUpdateMode.Normal;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000434C File Offset: 0x0000254C
		public ResultCode GenerateStatusCheck()
		{
			ResultCode resultCode = default(ResultCode);
			Vector3 lossyScale = this.cloth.transform.lossyScale;
			if (Mathf.Approximately(lossyScale.x, 0f) || Mathf.Approximately(lossyScale.y, 0f) || Mathf.Approximately(lossyScale.z, 0f))
			{
				resultCode.SetError(Define.Result.Init_ScaleIsZero);
			}
			else if (lossyScale.x < 0f || lossyScale.y < 0f || lossyScale.z < 0f)
			{
				ClothSerializeData2 serializeData = this.cloth.GetSerializeData2();
				if (!serializeData.preBuildData.UsePreBuild())
				{
					ClothInitSerializeData initData = serializeData.initData;
					if (initData == null || !initData.HasData())
					{
						resultCode.SetError(Define.Result.Init_NegativeScale);
						return resultCode;
					}
				}
				if (((lossyScale.x < 0f) ? 1 : 0) + ((lossyScale.y < 0f) ? 1 : 0) + ((lossyScale.z < 0f) ? 1 : 0) != 1)
				{
					resultCode.SetError(Define.Result.Init_NegativeScale);
				}
			}
			else
			{
				float num = Mathf.Abs(1f - lossyScale.x / lossyScale.y);
				float num2 = Mathf.Abs(1f - lossyScale.x / lossyScale.z);
				if (num > 0.01f || num2 > 0.01f)
				{
					resultCode.SetWarning(Define.Result.Init_NonUniformScale);
				}
			}
			return resultCode;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000044B4 File Offset: 0x000026B4
		internal bool GenerateInitialization()
		{
			this.result.SetProcess();
			if (!this.cloth.SerializeData.IsValid())
			{
				if (this.cloth.SerializeData.VerificationResult == Define.Result.Empty)
				{
					this.result.SetError(Define.Result.CreateCloth_InvalidSerializeData);
				}
				else
				{
					this.result.SetError(this.cloth.SerializeData.VerificationResult);
				}
				return false;
			}
			this.cloth.SerializeData.DataValidate();
			this.cloth.serializeData2.DataValidate();
			this.Init();
			return !this.result.IsError();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004558 File Offset: 0x00002758
		internal bool GenerateBoneClothSelection()
		{
			RenderSetupData renderSetupData = this.boneClothSetupData;
			int skinBoneCount = renderSetupData.skinBoneCount;
			SelectionData selectionData = new SelectionData(skinBoneCount);
			for (int i = 0; i < skinBoneCount; i++)
			{
				float3 @float = math.transform(renderSetupData.initRenderWorldtoLocal, renderSetupData.transformPositions[i]);
				selectionData.positions[i] = @float;
				selectionData.attributes[i] = VertexAttribute.Move;
			}
			float num = 0f;
			for (int j = 0; j < skinBoneCount; j++)
			{
				int parentTransformIndex = renderSetupData.GetParentTransformIndex(j, true);
				if (parentTransformIndex >= 0)
				{
					float num2 = math.distance(selectionData.positions[j], selectionData.positions[parentTransformIndex]);
					num = math.max(num, num2);
				}
			}
			selectionData.maxConnectionDistance = num;
			foreach (Transform transform in this.cloth.SerializeData.rootBones)
			{
				if (transform)
				{
					int transformIndexFromId = renderSetupData.GetTransformIndexFromId(transform.GetInstanceID());
					selectionData.attributes[transformIndexFromId] = VertexAttribute.Fixed;
				}
			}
			selectionData.userEdit = true;
			this.cloth.GetSerializeData2().selectionData = selectionData;
			return true;
		}

		// Token: 0x0400003B RID: 59
		private static readonly ProfilerMarker initClothProfiler = new ProfilerMarker("InitCloth");

		// Token: 0x0400003C RID: 60
		private static readonly ProfilerMarker preBuildProfiler = new ProfilerMarker("ClothProcess.PreBuild");

		// Token: 0x0400003D RID: 61
		private static readonly ProfilerMarker preBuildDeserializationProfiler = new ProfilerMarker("ClothProcess.PreBuild.Deserialization");

		// Token: 0x0400003E RID: 62
		private static readonly ProfilerMarker preBuildRegistrationProfiler = new ProfilerMarker("ClothProcess.PreBuild.Registration");

		// Token: 0x04000041 RID: 65
		public const int State_Valid = 0;

		// Token: 0x04000042 RID: 66
		public const int State_Enable = 1;

		// Token: 0x04000043 RID: 67
		public const int State_InitSuccess = 3;

		// Token: 0x04000044 RID: 68
		public const int State_InitComplete = 4;

		// Token: 0x04000045 RID: 69
		public const int State_Build = 5;

		// Token: 0x04000046 RID: 70
		public const int State_Running = 6;

		// Token: 0x04000047 RID: 71
		public const int State_DisableAutoBuild = 7;

		// Token: 0x04000048 RID: 72
		public const int State_CameraCullingInvisible = 8;

		// Token: 0x04000049 RID: 73
		public const int State_CameraCullingKeep = 9;

		// Token: 0x0400004A RID: 74
		public const int State_SkipWriting = 10;

		// Token: 0x0400004B RID: 75
		public const int State_UsePreBuild = 12;

		// Token: 0x0400004C RID: 76
		public const int State_DistanceCullingInvisible = 13;

		// Token: 0x0400004D RID: 77
		public const int State_UpdateTangent = 14;

		// Token: 0x0400004E RID: 78
		public const int State_Component = 15;

		// Token: 0x0400004F RID: 79
		public const int State_Verification = 16;

		// Token: 0x04000050 RID: 80
		internal BitField32 stateFlag;

		// Token: 0x04000052 RID: 82
		internal List<int> renderHandleList = new List<int>();

		// Token: 0x04000053 RID: 83
		internal RenderSetupData boneClothSetupData;

		// Token: 0x04000054 RID: 84
		internal List<ClothProcess.RenderMeshInfo> renderMeshInfoList = new List<ClothProcess.RenderMeshInfo>();

		// Token: 0x04000055 RID: 85
		internal List<TransformRecord> customSkinningBoneRecords = new List<TransformRecord>();

		// Token: 0x04000057 RID: 87
		internal ResultCode result;

		// Token: 0x0400005A RID: 90
		private ReductionSettings reductionSettings;

		// Token: 0x0400005D RID: 93
		internal Dictionary<ColliderComponent, int2> colliderDict = new Dictionary<ColliderComponent, int2>();

		// Token: 0x0400005F RID: 95
		internal InertiaConstraint.ConstraintData inertiaConstraintData;

		// Token: 0x04000060 RID: 96
		internal DistanceConstraint.ConstraintData distanceConstraintData;

		// Token: 0x04000061 RID: 97
		internal TriangleBendingConstraint.ConstraintData bendingConstraintData;

		// Token: 0x04000062 RID: 98
		internal Animator interlockingAnimator;

		// Token: 0x04000063 RID: 99
		internal List<Renderer> interlockingAnimatorRenderers = new List<Renderer>();

		// Token: 0x04000064 RID: 100
		internal int anchorTransformId;

		// Token: 0x04000065 RID: 101
		internal int distanceReferenceObjectId;

		// Token: 0x04000066 RID: 102
		internal Animator cameraCullingAnimator;

		// Token: 0x04000067 RID: 103
		internal List<Renderer> cameraCullingRenderers;

		// Token: 0x04000068 RID: 104
		internal CullingSettings.CameraCullingMode cameraCullingMode;

		// Token: 0x04000069 RID: 105
		internal bool cameraCullingOldInvisible;

		// Token: 0x0400006A RID: 106
		private CancellationTokenSource cts = new CancellationTokenSource();

		// Token: 0x0400006B RID: 107
		private volatile object lockObject = new object();

		// Token: 0x0400006C RID: 108
		private volatile bool isDestory;

		// Token: 0x0400006D RID: 109
		private volatile bool isDestoryInternal;

		// Token: 0x0400006E RID: 110
		private volatile bool isBuild;

		// Token: 0x02000012 RID: 18
		[BurstCompile]
		private struct GenerateSelectionJob : IJobParallelFor
		{
			// Token: 0x06000066 RID: 102 RVA: 0x00004700 File Offset: 0x00002900
			public void Execute(int vindex)
			{
				int2 @int = (int2)((this.uvs[vindex] % 1f + 1f) % 1f * this.xySize);
				Color32 color = this.attributeMapData[@int.y * this.attributeMapWidth + @int.x];
				VertexAttribute vertexAttribute = default(VertexAttribute);
				if (this.attributeReadFlag.IsSet(2) && color.g > 32)
				{
					vertexAttribute.SetFlag(2, true);
				}
				else if (this.attributeReadFlag.IsSet(1) && color.r > 32)
				{
					vertexAttribute.SetFlag(1, true);
				}
				if (this.attributeReadFlag.IsSet(4) && color.b <= 32)
				{
					vertexAttribute.SetFlag(8, true);
				}
				float3 @float = math.transform(this.toM, this.vertexs[vindex]);
				this.positionList[this.offset + vindex] = @float;
				this.attributeList[this.offset + vindex] = vertexAttribute;
			}

			// Token: 0x0400006F RID: 111
			public int offset;

			// Token: 0x04000070 RID: 112
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> positionList;

			// Token: 0x04000071 RID: 113
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<VertexAttribute> attributeList;

			// Token: 0x04000072 RID: 114
			public int attributeMapWidth;

			// Token: 0x04000073 RID: 115
			public float4x4 toM;

			// Token: 0x04000074 RID: 116
			public int2 xySize;

			// Token: 0x04000075 RID: 117
			public ExBitFlag8 attributeReadFlag;

			// Token: 0x04000076 RID: 118
			[ReadOnly]
			public NativeArray<Color32> attributeMapData;

			// Token: 0x04000077 RID: 119
			[ReadOnly]
			public NativeArray<float2> uvs;

			// Token: 0x04000078 RID: 120
			[ReadOnly]
			public NativeArray<float3> vertexs;
		}

		// Token: 0x02000013 RID: 19
		public class RenderMeshInfo
		{
			// Token: 0x04000079 RID: 121
			public int renderHandle;

			// Token: 0x0400007A RID: 122
			public VirtualMeshContainer renderMeshContainer;

			// Token: 0x0400007B RID: 123
			public DataChunk mappingChunk;

			// Token: 0x0400007C RID: 124
			public int renderDataWorkIndex;
		}

		// Token: 0x02000014 RID: 20
		public class PaintMapData
		{
			// Token: 0x0400007D RID: 125
			public const byte ReadFlag_Fixed = 1;

			// Token: 0x0400007E RID: 126
			public const byte ReadFlag_Move = 2;

			// Token: 0x0400007F RID: 127
			public const byte ReadFlag_Limit = 4;

			// Token: 0x04000080 RID: 128
			public Color32[] paintData;

			// Token: 0x04000081 RID: 129
			public int paintMapWidth;

			// Token: 0x04000082 RID: 130
			public int paintMapHeight;

			// Token: 0x04000083 RID: 131
			public ExBitFlag8 paintReadFlag;
		}

		// Token: 0x02000015 RID: 21
		public enum ClothType
		{
			// Token: 0x04000085 RID: 133
			MeshCloth,
			// Token: 0x04000086 RID: 134
			BoneCloth,
			// Token: 0x04000087 RID: 135
			BoneSpring = 10
		}
	}
}
