using System;
using System.Collections.Generic;
using System.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000B3 RID: 179
	public class TeamManager : IManager, IDisposable, IValid
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0001DDD1 File Offset: 0x0001BFD1
		public int MappingCount
		{
			get
			{
				ExNativeArray<TeamManager.MappingData> exNativeArray = this.mappingDataArray;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0001DDE4 File Offset: 0x0001BFE4
		public int TeamCount
		{
			get
			{
				ExNativeArray<TeamManager.TeamData> exNativeArray = this.teamDataArray;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0001DDF7 File Offset: 0x0001BFF7
		public int TrueTeamCount
		{
			get
			{
				return this.clothProcessDict.Count;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0001DE04 File Offset: 0x0001C004
		public int ActiveTeamCount
		{
			get
			{
				return this.enableTeamSet.Count;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0001DE11 File Offset: 0x0001C011
		public int TeamMaxUpdateCount
		{
			get
			{
				return this.teamStatus.Value.x;
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0001DE24 File Offset: 0x0001C024
		public void Dispose()
		{
			this.MonitoringProcess(true);
			this.isValid = false;
			ExNativeArray<TeamManager.TeamData> exNativeArray = this.teamDataArray;
			if (exNativeArray != null)
			{
				exNativeArray.Dispose();
			}
			ExNativeArray<TeamWindData> exNativeArray2 = this.teamWindArray;
			if (exNativeArray2 != null)
			{
				exNativeArray2.Dispose();
			}
			ExNativeArray<TeamManager.MappingData> exNativeArray3 = this.mappingDataArray;
			if (exNativeArray3 != null)
			{
				exNativeArray3.Dispose();
			}
			ExNativeArray<FixedList64Bytes<short>> exNativeArray4 = this.teamMappingIndexArray;
			if (exNativeArray4 != null)
			{
				exNativeArray4.Dispose();
			}
			ExNativeArray<ClothParameters> exNativeArray5 = this.parameterArray;
			if (exNativeArray5 != null)
			{
				exNativeArray5.Dispose();
			}
			ExNativeArray<InertiaConstraint.CenterData> exNativeArray6 = this.centerDataArray;
			if (exNativeArray6 != null)
			{
				exNativeArray6.Dispose();
			}
			this.teamDataArray = null;
			this.teamWindArray = null;
			this.mappingDataArray = null;
			this.teamMappingIndexArray = null;
			this.parameterArray = null;
			this.centerDataArray = null;
			if (this.teamStatus.IsCreated)
			{
				this.teamStatus.Dispose();
			}
			this.enableTeamSet.Clear();
			this.clothProcessDict.Clear();
			if (this.edgeColliderCollisionCountBuff.IsCreated)
			{
				this.edgeColliderCollisionCountBuff.Dispose();
			}
			(ref this.comp2SuspendCounterMap).MC2DisposeSafe<int, int>();
			(ref this.comp2TeamIdMap).MC2DisposeSafe<int, int>();
			(ref this.comp2SyncPartnerCompMap).MC2DisposeSafe<int, int>();
			(ref this.comp2SyncTopCompMap).MC2DisposeSafe<int, int>();
			if (this.batchNormalClothTeamList.IsCreated)
			{
				this.batchNormalClothTeamList.Dispose();
			}
			if (this.batchSplitClothTeamList.IsCreated)
			{
				this.batchSplitClothTeamList.Dispose();
			}
			List<ClothProcess> list = this.parameterDirtyList;
			if (list != null)
			{
				list.Clear();
			}
			List<ClothProcess> list2 = this.skipWritingDirtyList;
			if (list2 != null)
			{
				list2.Clear();
			}
			if (this.cullingDirtyList.IsCreated)
			{
				this.cullingDirtyList.Dispose();
			}
			if (this.selfCollisionUpdateSet.IsCreated)
			{
				this.selfCollisionUpdateSet.Dispose();
			}
			(ref this.animatorUpdateModeMap).MC2DisposeSafe<int, int>();
			ExSimpleNativeArray<int> exSimpleNativeArray = this.teamAnchorTransformIndexArray;
			if (exSimpleNativeArray != null)
			{
				exSimpleNativeArray.Dispose();
			}
			ExSimpleNativeArray<int> exSimpleNativeArray2 = this.teamDistanceTransformIndexArray;
			if (exSimpleNativeArray2 != null)
			{
				exSimpleNativeArray2.Dispose();
			}
			(ref this.transformPositionMap).MC2DisposeSafe<int, float3>();
			(ref this.transformRotationMap).MC2DisposeSafe<int, quaternion>();
			this.cameraCullingClothSet.Clear();
			MagicaManager.afterUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Remove(MagicaManager.afterUpdateDelegate, new MagicaManager.UpdateMethod(this.MonitoringProcessUpdate));
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0001E031 File Offset: 0x0001C231
		public void EnterdEditMode()
		{
			this.Dispose();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0001E03C File Offset: 0x0001C23C
		public void Initialize()
		{
			this.Dispose();
			this.teamDataArray = new ExNativeArray<TeamManager.TeamData>(32, false);
			this.teamWindArray = new ExNativeArray<TeamWindData>(32, false);
			this.mappingDataArray = new ExNativeArray<TeamManager.MappingData>(32, false);
			this.teamMappingIndexArray = new ExNativeArray<FixedList64Bytes<short>>(32, false);
			this.parameterArray = new ExNativeArray<ClothParameters>(32, false);
			this.centerDataArray = new ExNativeArray<InertiaConstraint.CenterData>(32, false);
			TeamManager.TeamData teamData = default(TeamManager.TeamData);
			this.teamDataArray.Add(teamData);
			this.teamWindArray.Add(default(TeamWindData));
			this.teamMappingIndexArray.Add(default(FixedList64Bytes<short>));
			this.parameterArray.Add(default(ClothParameters));
			this.centerDataArray.Add(default(InertiaConstraint.CenterData));
			this.teamStatus = new NativeReference<int4>(Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.edgeColliderCollisionCountBuff = new NativeReference<int>(Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.comp2SuspendCounterMap = new NativeParallelHashMap<int, int>(256, Allocator.Persistent);
			this.comp2TeamIdMap = new NativeParallelHashMap<int, int>(256, Allocator.Persistent);
			this.comp2SyncPartnerCompMap = new NativeParallelHashMap<int, int>(256, Allocator.Persistent);
			this.comp2SyncTopCompMap = new NativeParallelHashMap<int, int>(256, Allocator.Persistent);
			this.batchNormalClothTeamList = new NativeList<int>(Allocator.Persistent);
			this.batchSplitClothTeamList = new NativeList<int>(Allocator.Persistent);
			this.parameterDirtyList = new List<ClothProcess>(128);
			this.skipWritingDirtyList = new List<ClothProcess>(128);
			this.cullingDirtyList = new NativeList<int>(128, Allocator.Persistent);
			this.selfCollisionUpdateSet = new NativeParallelHashSet<int>(256, Allocator.Persistent);
			this.animatorUpdateModeMap = new NativeParallelHashMap<int, int>(128, Allocator.Persistent);
			this.teamAnchorTransformIndexArray = new ExSimpleNativeArray<int>(256, true);
			this.teamDistanceTransformIndexArray = new ExSimpleNativeArray<int>(256, true);
			this.transformPositionMap = new NativeParallelHashMap<int, float3>(32, Allocator.Persistent);
			this.transformRotationMap = new NativeParallelHashMap<int, quaternion>(32, Allocator.Persistent);
			MagicaManager.afterUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Combine(MagicaManager.afterUpdateDelegate, new MagicaManager.UpdateMethod(this.MonitoringProcessUpdate));
			this.isValid = true;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0001E27C File Offset: 0x0001C47C
		public bool IsValid()
		{
			return this.isValid;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0001E284 File Offset: 0x0001C484
		internal int AddTeam(ClothProcess cprocess, ClothParameters clothParams)
		{
			if (!this.isValid)
			{
				return 0;
			}
			TeamManager.TeamData teamData = default(TeamManager.TeamData);
			teamData.componentId = cprocess.cloth.GetInstanceID();
			teamData.flag.SetBits(0, true);
			teamData.flag.SetBits(2, true);
			teamData.flag.SetBits(3, true);
			teamData.originalUpdateMode = cprocess.cloth.SerializeData.updateMode;
			teamData.updateMode = cprocess.cloth.SerializeData.updateMode;
			teamData.timeScale = 1f;
			teamData.initScale = cprocess.clothTransformRecord.scale;
			teamData.scaleRatio = 1f;
			teamData.negativeScaleSign = 1f;
			teamData.negativeScaleDirection = 1;
			teamData.negativeScaleChange = 1;
			teamData.negativeScaleQuaternionValue = 1;
			teamData.negativeScaleTriangleSign = 1;
			teamData.animationPoseRatio = cprocess.cloth.SerializeData.animationPoseRatio;
			teamData.distanceWeight = 1f;
			teamData.componentTransformIndex = MagicaManager.Bone.AddComponentTransform(cprocess.cloth.transform);
			DataChunk dataChunk = this.teamDataArray.Add(teamData);
			int startIndex = dataChunk.startIndex;
			if (startIndex >= 4096)
			{
				object obj = string.Format("Cannot create more than {0} teams.", 4096);
				Develop.LogError(in obj);
				this.teamDataArray.Remove(dataChunk);
				return 0;
			}
			TeamWindData teamWindData = default(TeamWindData);
			teamWindData.movingWind.time = -10000f;
			this.teamWindArray.Add(teamWindData);
			this.teamMappingIndexArray.Add(default(FixedList64Bytes<short>));
			this.parameterArray.Add(clothParams);
			InertiaConstraint.CenterData centerData = default(InertiaConstraint.CenterData);
			centerData.frameLocalPosition = cprocess.ProxyMeshContainer.shareVirtualMesh.localCenterPosition.Value;
			this.centerDataArray.Add(centerData);
			this.clothProcessDict.Add(startIndex, cprocess);
			return startIndex;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0001E48C File Offset: 0x0001C68C
		internal void RemoveTeam(int teamId)
		{
			if (!this.isValid || teamId == 0)
			{
				return;
			}
			ref TeamManager.TeamData teamDataRef = ref this.GetTeamDataRef(teamId);
			if (teamDataRef.syncTeamId > 0 && this.ContainsTeamData(teamDataRef.syncTeamId))
			{
				ref TeamManager.TeamData teamDataRef2 = ref this.GetTeamDataRef(teamDataRef.syncTeamId);
				this.RemoveSyncParent(ref teamDataRef2, teamId);
			}
			MagicaManager.Bone.RemoveComponentTransform(teamDataRef.componentTransformIndex);
			DataChunk dataChunk = new DataChunk(teamId);
			this.teamDataArray.RemoveAndFill(dataChunk, default(TeamManager.TeamData));
			this.teamWindArray.RemoveAndFill(dataChunk, default(TeamWindData));
			this.teamMappingIndexArray.RemoveAndFill(dataChunk, default(FixedList64Bytes<short>));
			this.parameterArray.Remove(dataChunk);
			this.centerDataArray.Remove(dataChunk);
			this.teamAnchorTransformIndexArray[teamId] = 0;
			this.teamDistanceTransformIndexArray[teamId] = 0;
			this.clothProcessDict.Remove(teamId);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0001E574 File Offset: 0x0001C774
		public void SetEnable(int teamId, bool sw)
		{
			if (!this.isValid || teamId == 0)
			{
				return;
			}
			ref TeamManager.TeamData @ref = ref this.teamDataArray.GetRef(teamId);
			@ref.flag.SetBits(1, sw);
			@ref.flag.SetBits(2, sw);
			if (sw)
			{
				this.enableTeamSet.Add(teamId);
			}
			else
			{
				this.enableTeamSet.Remove(teamId);
			}
			if (!sw)
			{
				@ref.flag.SetBits(20, true);
			}
			MagicaManager.Collider.EnableTeamCollider(teamId);
			MagicaManager.Bone.EnableTransform(@ref.centerTransformIndex, sw);
			MagicaManager.Bone.EnableTransform(@ref.proxyTransformChunk, sw);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0001E611 File Offset: 0x0001C811
		public bool IsEnable(int teamId)
		{
			return this.enableTeamSet.Contains(teamId);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0001E61F File Offset: 0x0001C81F
		internal void SetSkipWriting(int teamId, bool sw)
		{
			if (!this.isValid || teamId == 0)
			{
				return;
			}
			this.teamDataArray.GetRef(teamId).flag.SetBits(14, sw);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0001E646 File Offset: 0x0001C846
		public bool ContainsTeamData(int teamId)
		{
			return teamId >= 0 && this.clothProcessDict.ContainsKey(teamId);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0001E65A File Offset: 0x0001C85A
		public ref TeamManager.TeamData GetTeamDataRef(int teamId)
		{
			return this.teamDataArray.GetRef(teamId);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0001E668 File Offset: 0x0001C868
		public ref FixedList64Bytes<short> GetTeamMappingRef(int teamId)
		{
			return this.teamMappingIndexArray.GetRef(teamId);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0001E676 File Offset: 0x0001C876
		public ref ClothParameters GetParametersRef(int teamId)
		{
			return this.parameterArray.GetRef(teamId);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0001E684 File Offset: 0x0001C884
		internal ref InertiaConstraint.CenterData GetCenterDataRef(int teamId)
		{
			return this.centerDataArray.GetRef(teamId);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0001E692 File Offset: 0x0001C892
		internal ref TeamManager.MappingData GetMappingDataRef(int mindex)
		{
			return this.mappingDataArray.GetRef(mindex);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0001E6A0 File Offset: 0x0001C8A0
		public ClothProcess GetClothProcess(int teamId)
		{
			if (this.clothProcessDict.ContainsKey(teamId))
			{
				return this.clothProcessDict[teamId];
			}
			return null;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0001E6C0 File Offset: 0x0001C8C0
		internal void CameraCullingPreProcess()
		{
			ClothManager cloth = MagicaManager.Cloth;
			this.cameraCullingClothSet.Clear();
			foreach (ClothProcess clothProcess in cloth.clothSet)
			{
				if (clothProcess.IsEnable && clothProcess.IsRunning())
				{
					MagicaCloth magicaCloth = ((clothProcess.SyncTopCloth != null) ? clothProcess.SyncTopCloth : clothProcess.cloth);
					CullingSettings cullingSettings = magicaCloth.SerializeData.cullingSettings;
					ClothProcess process = magicaCloth.Process;
					CullingSettings.CameraCullingMode cameraCullingMode = cullingSettings.cameraCullingMode;
					if (cameraCullingMode == CullingSettings.CameraCullingMode.AnimatorLinkage)
					{
						if (process.interlockingAnimator)
						{
							switch (process.interlockingAnimator.cullingMode)
							{
							case AnimatorCullingMode.AlwaysAnimate:
								cameraCullingMode = CullingSettings.CameraCullingMode.Off;
								break;
							case AnimatorCullingMode.CullUpdateTransforms:
								cameraCullingMode = CullingSettings.CameraCullingMode.Reset;
								break;
							case AnimatorCullingMode.CullCompletely:
								cameraCullingMode = CullingSettings.CameraCullingMode.Keep;
								break;
							}
						}
						else
						{
							cameraCullingMode = CullingSettings.CameraCullingMode.Off;
						}
					}
					if (magicaCloth == null || cameraCullingMode == CullingSettings.CameraCullingMode.Off)
					{
						clothProcess.cameraCullingAnimator = null;
						clothProcess.cameraCullingRenderers = null;
					}
					else
					{
						Animator animator = null;
						List<Renderer> list = cullingSettings.cameraCullingRenderers;
						if (cullingSettings.cameraCullingMethod == CullingSettings.CameraCullingMethod.AutomaticRenderer)
						{
							animator = magicaCloth.Process.interlockingAnimator;
							list = magicaCloth.Process.interlockingAnimatorRenderers;
						}
						clothProcess.cameraCullingAnimator = animator;
						clothProcess.cameraCullingRenderers = list;
					}
					clothProcess.cameraCullingMode = cameraCullingMode;
					this.cameraCullingClothSet.Add(clothProcess.cloth);
				}
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0001E83C File Offset: 0x0001CA3C
		internal void CameraCullingPostProcess()
		{
			ClothManager cloth = MagicaManager.Cloth;
			cloth.ClearVisibleDict();
			foreach (MagicaCloth magicaCloth in this.cameraCullingClothSet)
			{
				if (!(magicaCloth == null))
				{
					ClothProcess process = magicaCloth.Process;
					bool cameraCullingOldInvisible = process.cameraCullingOldInvisible;
					bool flag = (!(process.cameraCullingAnimator == null) || process.cameraCullingRenderers != null) && !cloth.CheckVisible(process.cameraCullingAnimator, process.cameraCullingRenderers);
					if (cameraCullingOldInvisible != flag)
					{
						int teamId = process.TeamId;
						ref TeamManager.TeamData teamDataRef = ref this.GetTeamDataRef(teamId);
						teamDataRef.flag.SetBits(11, flag);
						teamDataRef.flag.SetBits(12, false);
						process.SetState(8, flag);
						process.SetState(9, false);
						process.cameraCullingOldInvisible = flag;
						if (flag)
						{
							CullingSettings.CameraCullingMode cameraCullingMode = process.cameraCullingMode;
							if (cameraCullingMode != CullingSettings.CameraCullingMode.Off && cameraCullingMode != CullingSettings.CameraCullingMode.Reset)
							{
								if (cameraCullingMode == CullingSettings.CameraCullingMode.Keep)
								{
									teamDataRef.flag.SetBits(12, true);
									process.SetState(9, true);
								}
							}
							else
							{
								teamDataRef.flag.SetBits(2, true);
							}
						}
						process.UpdateRendererUse();
					}
				}
			}
			this.cameraCullingClothSet.Clear();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0001E99C File Offset: 0x0001CB9C
		internal void AlwaysTeamUpdate()
		{
			ClothManager cloth = MagicaManager.Cloth;
			TimeManager time = MagicaManager.Time;
			RenderManager render = MagicaManager.Render;
			TransformManager bone = MagicaManager.Bone;
			SimulationManager simulation = MagicaManager.Simulation;
			this.edgeColliderCollisionCount = 0;
			this.edgeColliderCollisionCountBuff.Value = 0;
			cloth.ClearVisibleDict();
			this.selfCollisionUpdateSet.Clear();
			this.teamAnchorTransformIndexArray.SetLength(this.TeamCount);
			this.teamDistanceTransformIndexArray.SetLength(this.TeamCount);
			this.transformPositionMap.Clear();
			this.transformRotationMap.Clear();
			this.cullingDirtyList.Clear();
			this.batchNormalClothTeamList.Clear();
			this.batchSplitClothTeamList.Clear();
			int i = 0;
			while (i < this.parameterDirtyList.Count)
			{
				ClothProcess clothProcess = this.parameterDirtyList[i];
				if (clothProcess == null)
				{
					this.parameterDirtyList.RemoveAt(i);
				}
				else if (!clothProcess.IsEnable)
				{
					i++;
				}
				else
				{
					MagicaManager.Collider.UpdateColliders(clothProcess);
					clothProcess.UpdateCullingAnimatorAndRenderers();
					int teamId = clothProcess.TeamId;
					ref TeamManager.TeamData teamDataRef = ref this.GetTeamDataRef(teamId);
					MagicaCloth cloth2 = clothProcess.cloth;
					teamDataRef.interlockingAnimatorId = ((clothProcess.interlockingAnimator != null) ? clothProcess.interlockingAnimator.GetInstanceID() : 0);
					clothProcess.SyncParameters();
					this.parameterArray[teamId] = clothProcess.parameters;
					teamDataRef.originalUpdateMode = cloth2.SerializeData.updateMode;
					teamDataRef.updateMode = cloth2.SerializeData.updateMode;
					teamDataRef.animationPoseRatio = cloth2.SerializeData.animationPoseRatio;
					teamDataRef.flag.SetBits(13, clothProcess.clothType == ClothProcess.ClothType.BoneSpring && clothProcess.parameters.springConstraint.springPower > 0f);
					this.selfCollisionUpdateSet.Add(teamId);
					teamDataRef.flag.SetBits(21, cloth2.SerializeData.meshWriteMode == ClothMeshWriteMode.PositionAndNormalTangent);
					clothProcess.SetState(14, teamDataRef.flag.IsSet(21));
					this.parameterDirtyList.RemoveAt(i);
				}
			}
			int j = 0;
			while (j < this.skipWritingDirtyList.Count)
			{
				ClothProcess clothProcess2 = this.skipWritingDirtyList[j];
				if (clothProcess2 == null)
				{
					this.skipWritingDirtyList.RemoveAt(j);
				}
				else if (!clothProcess2.IsEnable)
				{
					j++;
				}
				else
				{
					bool flag = clothProcess2.IsState(10);
					int teamId2 = clothProcess2.TeamId;
					this.GetTeamDataRef(teamId2).flag.SetBits(14, flag);
					foreach (ClothProcess.RenderMeshInfo renderMeshInfo in clothProcess2.renderMeshInfoList)
					{
						render.GetRendererData(renderMeshInfo.renderHandle).UpdateSkipWriting();
					}
					this.skipWritingDirtyList.RemoveAt(j);
				}
			}
			JobHandle jobHandle = new TeamManager.AlwaysTeamUpdatePreJob
			{
				teamDataArray = this.teamDataArray.GetNativeArray(),
				parameterArray = this.parameterArray.GetNativeArray(),
				comp2SuspendCounterMap = this.comp2SuspendCounterMap,
				comp2TeamIdMap = this.comp2TeamIdMap,
				comp2SyncPartnerCompMap = this.comp2SyncPartnerCompMap,
				comp2SyncTopCompMap = this.comp2SyncTopCompMap,
				selfCollisionUpdateSet = this.selfCollisionUpdateSet,
				edgeColliderCollisionCountBuff = this.edgeColliderCollisionCountBuff
			}.Schedule(default(JobHandle));
			JobHandle jobHandle2 = bone.ReadComponentTransform(default(JobHandle));
			JobHandle.ScheduleBatchedJobs();
			this.animatorUpdateModeMap.Clear();
			foreach (ClothProcess clothProcess3 in cloth.clothSet)
			{
				if (clothProcess3.TeamId != 0)
				{
					if (clothProcess3.interlockingAnimator)
					{
						int instanceID = clothProcess3.interlockingAnimator.GetInstanceID();
						if (!this.animatorUpdateModeMap.ContainsKey(instanceID))
						{
							this.animatorUpdateModeMap.Add(instanceID, (int)clothProcess3.interlockingAnimator.updateMode);
						}
					}
					ClothSerializeData serializeData = ((clothProcess3.SyncTopCloth != null) ? clothProcess3.SyncTopCloth : clothProcess3.cloth).SerializeData;
					Transform anchor = serializeData.inertiaConstraint.anchor;
					int num = ((anchor != null) ? anchor.GetInstanceID() : 0);
					if (num != 0 && !this.transformPositionMap.ContainsKey(num))
					{
						this.transformPositionMap.Add(num, anchor.position);
						this.transformRotationMap.Add(num, anchor.rotation);
					}
					if (clothProcess3.anchorTransformId != num)
					{
						clothProcess3.anchorTransformId = num;
						this.teamAnchorTransformIndexArray[clothProcess3.TeamId] = num;
					}
					int num2 = ((serializeData.cullingSettings.distanceCullingReferenceObject != null) ? serializeData.cullingSettings.distanceCullingReferenceObject.GetInstanceID() : 0);
					if (num2 != 0 && !this.transformPositionMap.ContainsKey(num2))
					{
						this.transformPositionMap.Add(num2, serializeData.cullingSettings.distanceCullingReferenceObject.transform.position);
					}
					if (clothProcess3.distanceReferenceObjectId != num2)
					{
						clothProcess3.distanceReferenceObjectId = num2;
						this.teamDistanceTransformIndexArray[clothProcess3.TeamId] = num2;
					}
				}
			}
			bool flag2 = Camera.main != null;
			float3 @float = (Camera.main ? Camera.main.transform.position : 0);
			this.transformPositionMap.Add(0, @float);
			jobHandle.Complete();
			jobHandle2.Complete();
			this.edgeColliderCollisionCount = this.edgeColliderCollisionCountBuff.Value;
			if (this.selfCollisionUpdateSet.Count() > 0)
			{
				foreach (int num3 in this.selfCollisionUpdateSet)
				{
					simulation.selfCollisionConstraint.UpdateTeam(num3);
				}
				this.selfCollisionUpdateSet.Clear();
			}
			if (this.ActiveTeamCount > 0)
			{
				float deltaTime = Time.deltaTime;
				float num4 = (float)time.FixedUpdateCount * Time.fixedDeltaTime;
				float unscaledDeltaTime = Time.unscaledDeltaTime;
				new TeamManager.AlwaysTeamUpdatePostJob
				{
					teamCount = this.TeamCount,
					unityFrameDeltaTime = deltaTime,
					unityFrameFixedDeltaTime = num4,
					unityFrameUnscaledDeltaTime = unscaledDeltaTime,
					globalTimeScale = time.GlobalTimeScale,
					simulationDeltaTime = time.SimulationDeltaTime,
					maxSimmulationCountPerFrame = time.maxSimulationCountPerFrame,
					splitProxyMeshVertexCount = simulation.splitProxyMeshVertexCount,
					teamStatus = this.teamStatus,
					teamDataArray = this.teamDataArray.GetNativeArray(),
					parameterArray = this.parameterArray.GetNativeArray(),
					centerDataArray = this.centerDataArray.GetNativeArray(),
					componentPositionArray = bone.componentPositionArray.GetNativeArray(),
					componentMinScaleArray = bone.componentMinScaleArray.GetNativeArray(),
					hasMainCamera = flag2,
					comp2TeamIdMap = this.comp2TeamIdMap,
					comp2SyncTopCompMap = this.comp2SyncTopCompMap,
					animatorUpdateModeMap = this.animatorUpdateModeMap,
					teamAnchorTransformIndexArray = this.teamAnchorTransformIndexArray.GetNativeArray(),
					teamDistanceTransformIndexArray = this.teamDistanceTransformIndexArray.GetNativeArray(),
					transformPositionMap = this.transformPositionMap,
					transformRotationMap = this.transformRotationMap,
					cullingDirtyList = this.cullingDirtyList,
					batchNormalClothTeamList = this.batchNormalClothTeamList,
					batchSplitClothTeamList = this.batchSplitClothTeamList
				}.Run<TeamManager.AlwaysTeamUpdatePostJob>();
				if (this.cullingDirtyList.Length > 0)
				{
					foreach (int num5 in this.cullingDirtyList)
					{
						ref TeamManager.TeamData teamDataRef2 = ref this.GetTeamDataRef(num5);
						ClothProcess clothProcess4 = this.GetClothProcess(num5);
						bool isDistanceCullingInvisible = teamDataRef2.IsDistanceCullingInvisible;
						clothProcess4.SetState(13, isDistanceCullingInvisible);
						clothProcess4.SetState(9, false);
						clothProcess4.UpdateRendererUse();
					}
				}
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0001F22C File Offset: 0x0001D42C
		internal void RemoveSyncParent(ref TeamManager.TeamData tdata, int parentTeamId)
		{
			(ref tdata.syncParentTeamId).MC2RemoveItemAtSwapBack(parentTeamId);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0001F23A File Offset: 0x0001D43A
		internal void AddMonitoringProcess(ClothProcess cprocess)
		{
			this.monitoringProcessSet.Add(cprocess);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0001F249 File Offset: 0x0001D449
		internal void RemoveMonitoringProcess(ClothProcess cprocess)
		{
			if (this.monitoringProcessSet.Contains(cprocess))
			{
				this.monitoringProcessSet.Remove(cprocess);
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0001F268 File Offset: 0x0001D468
		private void MonitoringProcess(bool force)
		{
			this.disposeProcessList.Clear();
			foreach (ClothProcess clothProcess in this.monitoringProcessSet)
			{
				if (clothProcess.cloth == null || force)
				{
					this.disposeProcessList.Add(clothProcess);
				}
			}
			if (this.disposeProcessList.Count > 0)
			{
				this.disposeProcessList.ForEach(delegate(ClothProcess cprocess)
				{
					cprocess.Dispose();
				});
				this.disposeProcessList.Clear();
			}
			if (force)
			{
				this.monitoringProcessSet.Clear();
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0001F32C File Offset: 0x0001D52C
		private void MonitoringProcessUpdate()
		{
			this.MonitoringProcess(false);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0001F338 File Offset: 0x0001D538
		internal static void SimulationCalcCenterAndInertiaAndWind(float simulationDeltaTime, int teamId, ref TeamManager.TeamData tdata, ref InertiaConstraint.CenterData cdata, ref TeamWindData windData, ref ClothParameters param, in NativeArray<float3> positions, in NativeArray<quaternion> rotations, in NativeArray<quaternion> vertexBindPoseRotations, in NativeArray<ushort> fixedArray, in NativeArray<float3> transformPositionArray, in NativeArray<quaternion> transformRotationArray, in NativeArray<float3> transformScaleArray, int windZoneCount, in NativeArray<WindManager.WindData> windDataArray)
		{
			int num = ((tdata.syncTeamId != 0 && tdata.flag.IsSet(6)) ? tdata.syncCenterTransformIndex : cdata.centerTransformIndex);
			NativeArray<float3> nativeArray = transformPositionArray;
			float3 @float = nativeArray[num];
			NativeArray<quaternion> nativeArray2 = transformRotationArray;
			quaternion quaternion = nativeArray2[num];
			nativeArray = transformScaleArray;
			float3 float2 = nativeArray[cdata.centerTransformIndex];
			cdata.componentWorldPosition = @float;
			cdata.componentWorldRotation = quaternion;
			cdata.componentWorldScale = float2;
			float num2 = math.length(float2) / math.length(tdata.initScale);
			float3 negativeScaleDirection = tdata.negativeScaleDirection;
			tdata.negativeScaleDirection = math.sign(float2);
			tdata.negativeScaleChange = negativeScaleDirection * tdata.negativeScaleDirection;
			if (float2.x < 0f || float2.y < 0f || float2.z < 0f)
			{
				tdata.negativeScaleSign = -1f;
				tdata.negativeScaleQuaternionValue = new float4(-math.sign(float2), 1f);
				tdata.negativeScaleTriangleSign.x = (float)((float2.x < 0f || float2.z < 0f) ? (-1) : 1);
				tdata.negativeScaleTriangleSign.y = (float)((float2.x < 0f) ? (-1) : 1);
				tdata.flag.SetBits(17, true);
			}
			else
			{
				tdata.negativeScaleSign = 1f;
				tdata.negativeScaleQuaternionValue = 1;
				tdata.negativeScaleTriangleSign = 1;
				tdata.flag.SetBits(17, false);
			}
			if (!negativeScaleDirection.Equals(tdata.negativeScaleDirection))
			{
				tdata.flag.SetBits(18, true);
				float4x4 float4x = float4x4.TRS(@float, quaternion, float2);
				float4x4 float4x2 = float4x4.TRS(cdata.oldComponentWorldPosition, cdata.oldComponentWorldRotation, cdata.oldComponentWorldScale);
				float4x4 float4x3 = math.mul(float4x, math.inverse(float4x2));
				cdata.oldComponentWorldPosition = MathUtility.TransformPoint(in cdata.oldComponentWorldPosition, in float4x3);
				cdata.oldComponentWorldScale = float2;
				cdata.oldAnchorPosition = MathUtility.TransformPoint(in cdata.oldAnchorPosition, in float4x3);
				cdata.smoothingVelocity = MathUtility.TransformVector(in cdata.smoothingVelocity, in float4x3);
			}
			float3 float3 = cdata.oldComponentWorldPosition;
			quaternion quaternion2 = cdata.oldComponentWorldRotation;
			float3 float4 = @float;
			quaternion quaternion3 = quaternion;
			if (tdata.fixedDataChunk.IsValid)
			{
				float3 float5 = 0;
				float3 float6 = 0;
				float3 float7 = 0;
				int startIndex = tdata.proxyCommonChunk.startIndex;
				int dataLength = tdata.fixedDataChunk.dataLength;
				int startIndex2 = tdata.fixedDataChunk.startIndex;
				float3 float11;
				float3 float12;
				for (int i = 0; i < dataLength; i++)
				{
					NativeArray<ushort> nativeArray3 = fixedArray;
					int num3 = (int)nativeArray3[startIndex2 + i] + startIndex;
					float3 float8 = float5;
					nativeArray = positions;
					float5 = float8 + nativeArray[num3];
					nativeArray2 = rotations;
					quaternion quaternion4 = nativeArray2[num3];
					if (tdata.negativeScaleSign < 0f)
					{
						float3 float9;
						float3 float10;
						MathUtility.ToNormalTangent(in quaternion4, out float9, out float10);
						float11 = -float9;
						float12 = -float10;
						quaternion4 = MathUtility.ToRotation(in float11, in float12);
					}
					quaternion quaternion5 = quaternion4;
					nativeArray2 = vertexBindPoseRotations;
					quaternion4 = math.mul(quaternion5, nativeArray2[num3]);
					float6 += MathUtility.ToNormal(in quaternion4);
					float7 += MathUtility.ToTangent(in quaternion4);
				}
				float6 *= (float)((tdata.negativeScaleDirection.x < 0f || tdata.negativeScaleDirection.z < 0f) ? (-1) : 1);
				float7 *= (float)((tdata.negativeScaleDirection.x < 0f || tdata.negativeScaleDirection.y < 0f) ? (-1) : 1);
				float4 = float5 / (float)dataLength;
				float11 = math.normalize(float6);
				float12 = math.normalize(float7);
				quaternion3 = MathUtility.ToRotation(in float11, in float12);
			}
			float4x4 float4x4 = MathUtility.WorldToLocalMatrix(in float4, in quaternion3, in float2);
			if (tdata.IsNegativeScaleTeleport)
			{
				float4x4 float4x5 = float4x4.TRS(float4, quaternion3, float2);
				float4x4 float4x6 = float4x4.TRS(cdata.oldFrameWorldPosition, cdata.oldFrameWorldRotation, cdata.oldFrameWorldScale);
				float4x4 float4x7 = math.mul(float4x5, math.inverse(float4x6));
				cdata.negativeScaleMatrix = float4x7;
			}
			float3 float13 = 0;
			quaternion quaternion6 = quaternion.identity;
			if (tdata.flag.IsSet(16) || tdata.IsReset)
			{
				cdata.oldAnchorPosition = cdata.anchorPosition;
				cdata.oldAnchorRotation = cdata.anchorRotation;
				float3 float11 = 1;
				cdata.anchorComponentLocalPosition = MathUtility.InverseTransformPoint(in @float, in cdata.anchorPosition, in cdata.anchorRotation, in float11);
			}
			if (tdata.flag.IsSet(15))
			{
				float3 float11 = 1;
				float13 = MathUtility.TransformPoint(in cdata.anchorComponentLocalPosition, in cdata.anchorPosition, in cdata.anchorRotation, in float11) - float3;
				quaternion6 = MathUtility.FromToRotation(in cdata.oldAnchorRotation, in cdata.anchorRotation);
				float num4 = 1f - param.inertiaConstraint.anchorInertia;
				float13 = math.lerp(float3.zero, float13, num4);
				quaternion6 = math.slerp(quaternion.identity, quaternion6, num4);
				float3 += float13;
				quaternion2 = math.mul(quaternion6, quaternion2);
				tdata.flag.SetBits(10, true);
			}
			float3 float14 = @float - float3;
			float num5 = MathUtility.Angle(in quaternion2, in quaternion);
			if (param.inertiaConstraint.teleportMode != InertiaConstraint.TeleportMode.None && !tdata.IsReset)
			{
				bool flag = false;
				flag = math.length(float14) >= param.inertiaConstraint.teleportDistance * num2 || flag;
				flag = math.degrees(num5) >= param.inertiaConstraint.teleportRotation || flag;
				if (flag)
				{
					InertiaConstraint.TeleportMode teleportMode = param.inertiaConstraint.teleportMode;
					if (teleportMode != InertiaConstraint.TeleportMode.Reset)
					{
						if (teleportMode == InertiaConstraint.TeleportMode.Keep)
						{
							tdata.flag.SetBits(9, true);
						}
					}
					else
					{
						tdata.flag.SetBits(2, true);
					}
				}
			}
			float3 float15 = 0;
			if (param.inertiaConstraint.movementInertiaSmoothing >= 1E-06f && !tdata.IsKeepReset && !tdata.IsReset)
			{
				if (tdata.IsRunning)
				{
					float3 float16 = ((tdata.frameDeltaTime > 0f) ? (float14 / tdata.frameDeltaTime) : 0);
					float num6 = param.inertiaConstraint.movementSpeedLimit * num2;
					if (num6 >= 0f)
					{
						float16 = MathUtility.ClampVector(float16, num6);
					}
					float num7 = math.saturate(math.pow(1f - param.inertiaConstraint.movementInertiaSmoothing, 3f) * 0.99f + 0.01f);
					cdata.smoothingVelocity = math.lerp(cdata.smoothingVelocity, float16, num7);
				}
				float3 float17 = @float - cdata.smoothingVelocity * tdata.frameDeltaTime;
				float15 = float17 - float3;
				float3 = float17;
				tdata.flag.SetBits(10, true);
			}
			cdata.frameWorldPosition = float4;
			cdata.frameWorldRotation = quaternion3;
			cdata.frameWorldScale = float2;
			if (tdata.IsReset)
			{
				cdata.oldComponentWorldPosition = @float;
				cdata.oldComponentWorldRotation = quaternion;
				cdata.oldComponentWorldScale = float2;
				float3 = @float;
				quaternion2 = quaternion;
				cdata.oldFrameWorldPosition = float4;
				cdata.oldFrameWorldRotation = quaternion3;
				cdata.oldFrameWorldScale = float2;
				cdata.nowWorldPosition = float4;
				cdata.nowWorldRotation = quaternion3;
				cdata.oldWorldPosition = float4;
				cdata.oldWorldRotation = quaternion3;
			}
			else if (tdata.IsNegativeScaleTeleport)
			{
				cdata.oldFrameWorldPosition = float4;
				cdata.oldFrameWorldRotation = quaternion3;
				cdata.oldFrameWorldScale = float2;
				cdata.nowWorldPosition = float4;
				cdata.nowWorldRotation = quaternion3;
				cdata.oldWorldPosition = float4;
				cdata.oldWorldRotation = quaternion3;
			}
			float3 float18 = float3;
			quaternion quaternion7 = quaternion2;
			if (tdata.IsReset)
			{
				cdata.frameComponentShiftVector = 0;
				cdata.frameComponentShiftRotation = quaternion.identity;
				cdata.smoothingVelocity = 0;
				float15 = 0;
			}
			else
			{
				cdata.frameComponentShiftVector = @float - float3;
				cdata.frameComponentShiftRotation = MathUtility.FromToRotation(in quaternion2, in quaternion);
				float num8 = 0f;
				float num9 = 0f;
				float num10 = 1f - param.inertiaConstraint.worldInertia;
				float num11 = 1f - param.inertiaConstraint.worldInertia;
				bool flag2 = tdata.IsKeepReset || tdata.IsCullingInvisible;
				num10 = (flag2 ? 1f : num10);
				num11 = (flag2 ? 1f : num11);
				if (num10 > 1E-08f || num11 > 1E-08f)
				{
					tdata.flag.SetBits(10, true);
					num8 = num10;
					num9 = num11;
					float18 = math.lerp(float18, @float, num10);
					quaternion7 = math.slerp(quaternion7, quaternion, num11);
				}
				float num12 = param.inertiaConstraint.movementSpeedLimit * num2;
				float rotationSpeedLimit = param.inertiaConstraint.rotationSpeedLimit;
				float3 float19 = @float - float18;
				float num13 = MathUtility.Angle(in quaternion7, in quaternion);
				float num14 = ((tdata.frameDeltaTime > 0f) ? (math.length(float19) / tdata.frameDeltaTime) : 0f);
				float num15 = ((tdata.frameDeltaTime > 0f) ? (math.degrees(num13) / tdata.frameDeltaTime) : 0f);
				if (num14 > num12 && num12 >= 0f)
				{
					tdata.flag.SetBits(10, true);
					float num16 = math.saturate(math.max(num14 - num12, 0f) / num14);
					num8 = math.lerp(num8, 1f, num16);
					float18 = math.lerp(float18, @float, num16);
				}
				if (num15 > rotationSpeedLimit && rotationSpeedLimit >= 0f)
				{
					tdata.flag.SetBits(10, true);
					float num17 = math.saturate(math.max(num15 - rotationSpeedLimit, 0f) / num15);
					num9 = math.lerp(num9, 1f, num17);
					quaternion7 = math.slerp(quaternion7, quaternion, num17);
				}
				float num18 = 0f;
				if (tdata.skipCount > 0)
				{
					num18 = math.lerp(num18, 1f, math.saturate((float)tdata.skipCount * simulationDeltaTime / (tdata.frameDeltaTime * tdata.nowTimeScale)));
				}
				if (tdata.velocityWeight < 1f)
				{
					num18 = math.lerp(num18, 1f, 1f - tdata.velocityWeight);
				}
				if (tdata.nowTimeScale < 1f)
				{
					num18 = math.lerp(num18, 1f, 1f - tdata.nowTimeScale);
				}
				if (num18 > 0f)
				{
					tdata.flag.SetBits(10, true);
					num8 = math.lerp(num8, 1f, num18);
					float18 = math.lerp(float18, @float, num18);
					num9 = math.lerp(num9, 1f, num18);
					quaternion7 = math.slerp(quaternion7, quaternion, num18);
				}
				if (tdata.IsInertiaShift)
				{
					cdata.frameComponentShiftVector *= num8;
					cdata.frameComponentShiftRotation = math.slerp(quaternion.identity, cdata.frameComponentShiftRotation, num9);
					cdata.frameComponentShiftVector += float13;
					cdata.frameComponentShiftRotation = math.mul(quaternion6, cdata.frameComponentShiftRotation);
					cdata.frameComponentShiftVector += float15;
					cdata.oldFrameWorldPosition = MathUtility.ShiftPosition(in cdata.oldFrameWorldPosition, in cdata.oldComponentWorldPosition, in cdata.frameComponentShiftVector, in cdata.frameComponentShiftRotation);
					cdata.oldFrameWorldRotation = math.mul(cdata.frameComponentShiftRotation, cdata.oldFrameWorldRotation);
					cdata.nowWorldPosition = MathUtility.ShiftPosition(in cdata.nowWorldPosition, in cdata.oldComponentWorldPosition, in cdata.frameComponentShiftVector, in cdata.frameComponentShiftRotation);
					cdata.nowWorldRotation = math.mul(cdata.frameComponentShiftRotation, cdata.nowWorldRotation);
				}
			}
			float3 float20 = @float - float18;
			float num19 = math.length(float20);
			cdata.frameMovingSpeed = ((tdata.frameDeltaTime > 0f) ? (num19 / tdata.frameDeltaTime) : 0f);
			cdata.frameMovingSpeed *= ((tdata.nowTimeScale > 1E-06f) ? (1f / tdata.nowTimeScale) : 0f);
			cdata.frameMovingDirection = ((num19 > 1E-06f) ? (float20 / num19) : 0);
			float3 float21 = MathUtility.InverseTransformPoint(in float4, in float4x4);
			cdata.frameLocalPosition = float21;
			if (tdata.flag.IsSet(2) || tdata.flag.IsSet(3))
			{
				tdata.velocityWeight = ((param.stablizationTimeAfterReset > 1E-06f) ? 0f : 1f);
				tdata.blendWeight = tdata.velocityWeight;
			}
			TeamWindData teamWindData = default(TeamWindData);
			if (windZoneCount > 0 && param.wind.IsValid())
			{
				float num20 = float.MaxValue;
				int num21 = 0;
				int num22 = -1;
				for (int j = 0; j < windZoneCount; j++)
				{
					NativeArray<WindManager.WindData> nativeArray4 = windDataArray;
					WindManager.WindData windData2 = nativeArray4[j];
					if (windData2.IsValid() && windData2.IsEnable())
					{
						bool flag3 = windData2.IsAddition();
						if (!flag3 || num21 < 3)
						{
							float3 float22 = math.transform(windData2.worldToLocalMatrix, float4);
							float num23 = math.length(float22);
							MagicaWindZone.Mode mode = windData2.mode;
							if (mode != MagicaWindZone.Mode.SphereDirection)
							{
								if (mode != MagicaWindZone.Mode.BoxDirection)
								{
									if (mode != MagicaWindZone.Mode.SphereRadial)
									{
										goto IL_0D5D;
									}
								}
								else
								{
									float3 float23 = math.abs(float22) * 2f;
									if (float23.x > windData2.size.x || float23.y > windData2.size.y)
									{
										goto IL_0E5C;
									}
									if (float23.z > windData2.size.z)
									{
										goto IL_0E5C;
									}
									goto IL_0D5D;
								}
							}
							if (num23 > windData2.size.x)
							{
								goto IL_0E5C;
							}
							IL_0D5D:
							if (flag3 || windData2.zoneVolume <= num20)
							{
								float3 float24 = windData2.worldWindDirection;
								if (windData2.mode == MagicaWindZone.Mode.SphereRadial)
								{
									if (num23 <= 1E-06f)
									{
										goto IL_0E5C;
									}
									float24 = math.normalize(float4 - windData2.worldPositin);
								}
								float num24 = windData2.main;
								if (windData2.mode == MagicaWindZone.Mode.SphereRadial)
								{
									if (num23 <= 1E-06f)
									{
										goto IL_0E5C;
									}
									float num25 = math.saturate(num23 / windData2.size.x);
									float num26 = (in windData2.attenuation).MC2EvaluateCurveClamp01(num25);
									num24 *= num26;
								}
								TeamWindInfo teamWindInfo = new TeamWindInfo
								{
									windId = j,
									time = -10000f,
									main = num24,
									direction = float24
								};
								if (flag3)
								{
									teamWindData.AddOrReplaceWindZone(teamWindInfo, in windData);
									num21++;
								}
								else
								{
									teamWindData.RemoveWindZone(num22);
									teamWindData.AddOrReplaceWindZone(teamWindInfo, in windData);
									num20 = windData2.zoneVolume;
									num22 = j;
								}
							}
						}
					}
					IL_0E5C:;
				}
			}
			teamWindData.movingWind = windData.movingWind;
			windData.CopyFrom(in teamWindData);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000201C8 File Offset: 0x0001E3C8
		internal static void SimulationStepTeamUpdate(int updateIndex, float simulationDeltaTime, int teamId, ref TeamManager.TeamData tdata, ref ClothParameters param, ref InertiaConstraint.CenterData cdata, ref TeamWindData wdata)
		{
			bool flag = updateIndex < tdata.updateCount;
			tdata.flag.SetBits(7, flag);
			tdata.nowUpdateTime += simulationDeltaTime;
			float num = tdata.time - tdata.frameOldTime;
			tdata.frameInterpolation = ((num > 0f) ? math.saturate((tdata.nowUpdateTime - tdata.frameOldTime) / num) : 1f);
			cdata.oldWorldPosition = cdata.nowWorldPosition;
			cdata.oldWorldRotation = cdata.nowWorldRotation;
			cdata.nowWorldPosition = math.lerp(cdata.oldFrameWorldPosition, cdata.frameWorldPosition, tdata.frameInterpolation);
			cdata.nowWorldRotation = math.slerp(cdata.oldFrameWorldRotation, cdata.frameWorldRotation, tdata.frameInterpolation);
			cdata.nowWorldRotation = math.normalize(cdata.nowWorldRotation);
			float3 @float = math.lerp(cdata.oldFrameWorldScale, cdata.frameWorldScale, tdata.frameInterpolation);
			cdata.stepVector = cdata.nowWorldPosition - cdata.oldWorldPosition;
			cdata.stepRotation = MathUtility.FromToRotation(in cdata.oldWorldRotation, in cdata.nowWorldRotation);
			float num2 = MathUtility.Angle(in cdata.oldWorldRotation, in cdata.nowWorldRotation);
			float num3 = 1f - param.inertiaConstraint.localInertia;
			float num4 = 1f - param.inertiaConstraint.localInertia;
			float num5 = math.length(cdata.stepVector * (1f - num3)) / simulationDeltaTime;
			if (num5 > param.inertiaConstraint.localMovementSpeedLimit && param.inertiaConstraint.localMovementSpeedLimit >= 0f)
			{
				float num6 = param.inertiaConstraint.localMovementSpeedLimit / num5;
				num3 = math.lerp(1f, num3, num6);
			}
			float num7 = math.degrees(num2 * (1f - num4) / simulationDeltaTime);
			if (num7 > param.inertiaConstraint.localRotationSpeedLimit && param.inertiaConstraint.localRotationSpeedLimit >= 0f)
			{
				float num8 = param.inertiaConstraint.localRotationSpeedLimit / num7;
				num4 = math.lerp(1f, num4, num8);
			}
			cdata.stepMoveInertiaRatio = num3;
			cdata.stepRotationInertiaRatio = num4;
			cdata.inertiaVector = math.lerp(float3.zero, cdata.stepVector, num3);
			cdata.inertiaRotation = math.slerp(quaternion.identity, cdata.stepRotation, num4);
			cdata.angularVelocity = num2 / simulationDeltaTime;
			if (cdata.angularVelocity > 1E-08f)
			{
				float num9;
				MathUtility.ToAngleAxis(in cdata.stepRotation, out num9, out cdata.rotationAxis);
			}
			else
			{
				cdata.rotationAxis = 0;
			}
			tdata.scaleRatio = math.max(math.length(@float) / math.length(tdata.initScale), 1E-06f);
			float num10 = 1f;
			if (math.lengthsq(param.worldGravityDirection) > 1E-08f)
			{
				float3 initLocalGravityDirection = cdata.initLocalGravityDirection;
				initLocalGravityDirection.y *= tdata.negativeScaleDirection.y;
				num10 = math.dot(math.mul(cdata.nowWorldRotation, initLocalGravityDirection), param.worldGravityDirection);
				num10 = math.saturate(num10 * 0.5f + 0.5f);
			}
			tdata.gravityDot = num10;
			float num11 = 1f;
			if (param.gravity > 1E-06f && param.gravityFalloff > 1E-06f)
			{
				num11 = math.lerp(math.saturate(1f - param.gravityFalloff), 1f, math.saturate(1f - num10));
			}
			tdata.gravityRatio = num11;
			if (tdata.velocityWeight < 1f)
			{
				float num12 = ((param.stablizationTimeAfterReset > 1E-06f) ? (simulationDeltaTime / param.stablizationTimeAfterReset) : 1f);
				tdata.velocityWeight = math.saturate(tdata.velocityWeight + num12);
			}
			tdata.blendWeight = math.saturate(tdata.velocityWeight * param.blendWeight * tdata.distanceWeight);
			TeamManager.UpdateWind(simulationDeltaTime, teamId, in tdata, in param.wind, in cdata, ref wdata);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000205C0 File Offset: 0x0001E7C0
		private static void UpdateWind(float simulationDeltaTime, int teamId, in TeamManager.TeamData tdata, in WindParams windParams, in InertiaConstraint.CenterData cdata, ref TeamWindData teamWindData)
		{
			WindParams windParams2 = windParams;
			if (!windParams2.IsValid())
			{
				return;
			}
			int zoneCount = teamWindData.ZoneCount;
			for (int i = 0; i < zoneCount; i++)
			{
				TeamWindInfo teamWindInfo = teamWindData.windZoneList[i];
				TeamManager.UpdateWindTime(ref teamWindInfo, windParams.frequency, simulationDeltaTime);
				teamWindData.windZoneList[i] = teamWindInfo;
			}
			TeamWindInfo movingWind = teamWindData.movingWind;
			movingWind.main = 0f;
			if (windParams.movingWind > 0.01f)
			{
				movingWind.main = cdata.frameMovingSpeed * windParams.movingWind / tdata.scaleRatio;
				movingWind.direction = -cdata.frameMovingDirection;
				TeamManager.UpdateWindTime(ref movingWind, windParams.frequency, simulationDeltaTime);
			}
			teamWindData.movingWind = movingWind;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00020684 File Offset: 0x0001E884
		private static void UpdateWindTime(ref TeamWindInfo windInfo, float frequency, float simulationDeltaTime)
		{
			float num = windInfo.main / 7.5f;
			float num2 = 0.2f + num * 0.5f;
			num2 *= frequency;
			num2 = math.min(num2, 1.5f);
			num2 *= simulationDeltaTime;
			windInfo.time += num2;
			if (windInfo.time > 10000f)
			{
				windInfo.time -= 20000f;
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x000206EC File Offset: 0x0001E8EC
		internal static void SimulationPostTeamUpdate(ref TeamManager.TeamData tdata, ref InertiaConstraint.CenterData cdata)
		{
			cdata.oldComponentWorldPosition = cdata.componentWorldPosition;
			cdata.oldComponentWorldRotation = cdata.componentWorldRotation;
			cdata.oldComponentWorldScale = cdata.componentWorldScale;
			if (tdata.IsRunning)
			{
				cdata.oldFrameWorldPosition = cdata.frameWorldPosition;
				cdata.oldFrameWorldRotation = cdata.frameWorldRotation;
				cdata.oldFrameWorldScale = cdata.frameWorldScale;
				tdata.forceMode = ClothForceMode.None;
				tdata.impactForce = 0;
				tdata.skipCount = 0;
			}
			cdata.oldAnchorPosition = cdata.anchorPosition;
			cdata.oldAnchorRotation = cdata.anchorRotation;
			float3 @float = 1;
			cdata.anchorComponentLocalPosition = MathUtility.InverseTransformPoint(in cdata.componentWorldPosition, in cdata.anchorPosition, in cdata.anchorRotation, in @float);
			tdata.flag.SetBits(2, false);
			tdata.flag.SetBits(3, false);
			tdata.flag.SetBits(5, false);
			tdata.flag.SetBits(7, false);
			tdata.flag.SetBits(9, false);
			tdata.flag.SetBits(10, false);
			tdata.flag.SetBits(18, false);
			if (tdata.time > 7200f)
			{
				tdata.time -= 3600f;
				tdata.oldTime -= 3600f;
				tdata.nowUpdateTime -= 3600f;
				tdata.oldUpdateTime -= 3600f;
				tdata.frameUpdateTime -= 3600f;
				tdata.frameOldTime -= 3600f;
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00020868 File Offset: 0x0001EA68
		public void InformationLog(StringBuilder allsb)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("========== Team Manager ==========");
			if (!this.IsValid())
			{
				stringBuilder.AppendLine("Team Manager. Invalid.");
				stringBuilder.AppendLine();
				Debug.Log(stringBuilder.ToString());
				allsb.Append(stringBuilder);
				return;
			}
			stringBuilder.AppendLine(string.Format("Team Manager. Team:{0}, Mapping:{1}, Monitoring:{2}", this.TeamCount, this.MappingCount, this.monitoringProcessSet.Count));
			stringBuilder.AppendLine("  -teamDataArray:" + this.teamDataArray.ToSummary());
			stringBuilder.AppendLine("  -teamWindArray:" + this.teamWindArray.ToSummary());
			stringBuilder.AppendLine("  -mappingDataArray:" + this.mappingDataArray.ToSummary());
			stringBuilder.AppendLine("  -parameterArray:" + this.parameterArray.ToSummary());
			stringBuilder.AppendLine("  -centerDataArray:" + this.centerDataArray.ToSummary());
			Debug.Log(stringBuilder.ToString());
			allsb.Append(stringBuilder);
			for (int i = 1; i < this.TeamCount; i++)
			{
				TeamManager.TeamData teamData = this.teamDataArray[i];
				if (teamData.IsValid)
				{
					stringBuilder.Clear();
					FixedList64Bytes<short> fixedList64Bytes = this.teamMappingIndexArray[i];
					ClothProcess clothProcess = this.GetClothProcess(i);
					if (clothProcess == null)
					{
						stringBuilder.AppendLine(string.Format("ID:{0} cprocess is null!", i));
						Debug.LogWarning(stringBuilder.ToString());
						allsb.Append(stringBuilder);
					}
					else
					{
						MagicaCloth cloth = clothProcess.cloth;
						if (cloth == null)
						{
							stringBuilder.AppendLine(string.Format("ID:{0} cloth is null!", i));
							Debug.LogWarning(stringBuilder.ToString());
							allsb.Append(stringBuilder);
						}
						else
						{
							stringBuilder.AppendLine(string.Format("ID:{0} [{1}] state:0x{2:X}, Flag:0x{3:X}, Particle:{4}, Collider:{5} Proxy:{6}, Mapping:{7}", new object[]
							{
								i,
								clothProcess.Name,
								clothProcess.GetStateFlag().Value,
								teamData.flag.Value,
								teamData.ParticleCount,
								clothProcess.colliderDict.Count,
								teamData.proxyMeshType,
								fixedList64Bytes.Length
							}));
							stringBuilder.AppendLine(string.Format("  -centerTransformIndex {0}", teamData.centerTransformIndex));
							stringBuilder.AppendLine(string.Format("  -initScale {0}", teamData.initScale));
							stringBuilder.AppendLine(string.Format("  -scaleRatio {0}", teamData.scaleRatio));
							stringBuilder.AppendLine(string.Format("  -animationPoseRatio {0}", teamData.animationPoseRatio));
							stringBuilder.AppendLine(string.Format("  -blendWeight {0}", teamData.blendWeight));
							stringBuilder.AppendLine(string.Format("  Sync:{0}, SyncParentCount:{1}", cloth.SyncPartnerCloth, teamData.syncParentTeamId.Length));
							stringBuilder.AppendLine(string.Format("  -ProxyTransformChunk {0}", teamData.proxyTransformChunk));
							stringBuilder.AppendLine(string.Format("  -ProxyCommonChunk {0}", teamData.proxyCommonChunk));
							stringBuilder.AppendLine(string.Format("  -ProxyMeshChunk {0}", teamData.proxyMeshChunk));
							stringBuilder.AppendLine(string.Format("  -ProxyBoneChunk {0}", teamData.proxyBoneChunk));
							stringBuilder.AppendLine(string.Format("  -ProxySkinBoneChunk {0}", teamData.proxySkinBoneChunk));
							stringBuilder.AppendLine(string.Format("  -ProxyTriangleChunk {0}", teamData.proxyTriangleChunk));
							stringBuilder.AppendLine(string.Format("  -ProxyEdgeChunk {0}", teamData.proxyEdgeChunk));
							stringBuilder.AppendLine(string.Format("  -BaseLineChunk {0}", teamData.baseLineChunk));
							stringBuilder.AppendLine(string.Format("  -BaseLineDataChunk {0}", teamData.baseLineDataChunk));
							stringBuilder.AppendLine(string.Format("  -ParticleChunk {0}", teamData.particleChunk));
							stringBuilder.AppendLine(string.Format("  -ColliderChunk {0}", teamData.colliderChunk));
							stringBuilder.AppendLine(string.Format("  -ColliderTrnasformChunk {0}", teamData.colliderTransformChunk));
							stringBuilder.AppendLine(string.Format("  -colliderCount {0}", teamData.colliderCount));
							stringBuilder.AppendLine(string.Format("  *Mapping Count {0}", fixedList64Bytes.Length));
							if (fixedList64Bytes.Length > 0)
							{
								for (int j = 0; j < fixedList64Bytes.Length; j++)
								{
									int num = (int)fixedList64Bytes[j];
									TeamManager.MappingData mappingData = this.mappingDataArray[num];
									stringBuilder.AppendLine(string.Format("  *Mapping Mid:{0}, Vertex:{1}", num, mappingData.VertexCount));
									stringBuilder.AppendLine(string.Format("    -teamId:{0}", mappingData.teamId));
									stringBuilder.AppendLine(string.Format("    -centerTransformIndex:{0}", mappingData.centerTransformIndex));
									stringBuilder.AppendLine(string.Format("    -mappingCommonChunk:{0}", mappingData.mappingCommonChunk));
									stringBuilder.AppendLine(string.Format("    -toProxyMatrix:{0}", mappingData.toProxyMatrix));
									stringBuilder.AppendLine(string.Format("    -toProxyRotation:{0}", mappingData.toProxyRotation));
									stringBuilder.AppendLine(string.Format("    -sameSpace:{0}", mappingData.sameSpace));
									stringBuilder.AppendLine(string.Format("    -toMappingMatrix:{0}", mappingData.toMappingMatrix));
									stringBuilder.AppendLine(string.Format("    -scaleRatio:{0}", mappingData.scaleRatio));
									stringBuilder.AppendLine(string.Format("    -renderDataWorkIndex:{0}", mappingData.renderDataWorkIndex));
								}
							}
							stringBuilder.AppendLine(string.Format("  +DistanceStartChunk {0}", teamData.distanceStartChunk));
							stringBuilder.AppendLine(string.Format("  +DistanceDataChunk {0}", teamData.distanceDataChunk));
							stringBuilder.AppendLine(string.Format("  +BendingPairChunk {0}", teamData.bendingPairChunk));
							stringBuilder.AppendLine(string.Format("  +selfPointChunk {0}", teamData.selfPointChunk));
							stringBuilder.AppendLine(string.Format("  +selfEdgeChunk {0}", teamData.selfEdgeChunk));
							stringBuilder.AppendLine(string.Format("  +selfTriangleChunk {0}", teamData.selfTriangleChunk));
							TeamWindData teamWindData = this.teamWindArray[i];
							stringBuilder.AppendLine(string.Format("  #Wind ZoneCount:{0}", teamWindData.ZoneCount));
							for (int k = 0; k < teamWindData.ZoneCount; k++)
							{
								stringBuilder.AppendLine(string.Format("    [{0}] {1}", k, teamWindData.windZoneList[k].ToString()));
							}
							stringBuilder.AppendLine("    [Move] " + teamWindData.movingWind.ToString());
							Debug.Log(stringBuilder.ToString());
							allsb.Append(stringBuilder);
						}
					}
				}
			}
			allsb.AppendLine();
			stringBuilder.Clear();
			int count = this.mappingDataArray.Count;
			stringBuilder.AppendLine(string.Format("#MappingData Count:{0}", count));
			for (int l = 0; l < count; l++)
			{
				TeamManager.MappingData mappingData2 = this.mappingDataArray[l];
				if (mappingData2.IsValid())
				{
					stringBuilder.AppendLine(string.Format("[{0}] teamId:{1}, renderDataWorkIndex:{2}", l, mappingData2.teamId, mappingData2.renderDataWorkIndex));
				}
			}
			Debug.Log(stringBuilder.ToString());
			allsb.Append(stringBuilder);
		}

		// Token: 0x0400064C RID: 1612
		public const int Flag_Valid = 0;

		// Token: 0x0400064D RID: 1613
		public const int Flag_Enable = 1;

		// Token: 0x0400064E RID: 1614
		public const int Flag_Reset = 2;

		// Token: 0x0400064F RID: 1615
		public const int Flag_TimeReset = 3;

		// Token: 0x04000650 RID: 1616
		public const int Flag_SyncSuspend = 4;

		// Token: 0x04000651 RID: 1617
		public const int Flag_Running = 5;

		// Token: 0x04000652 RID: 1618
		public const int Flag_Synchronization = 6;

		// Token: 0x04000653 RID: 1619
		public const int Flag_StepRunning = 7;

		// Token: 0x04000654 RID: 1620
		public const int Flag_Exit = 8;

		// Token: 0x04000655 RID: 1621
		public const int Flag_KeepTeleport = 9;

		// Token: 0x04000656 RID: 1622
		public const int Flag_InertiaShift = 10;

		// Token: 0x04000657 RID: 1623
		public const int Flag_CameraCullingInvisible = 11;

		// Token: 0x04000658 RID: 1624
		public const int Flag_CameraCullingKeep = 12;

		// Token: 0x04000659 RID: 1625
		public const int Flag_Spring = 13;

		// Token: 0x0400065A RID: 1626
		public const int Flag_SkipWriting = 14;

		// Token: 0x0400065B RID: 1627
		public const int Flag_Anchor = 15;

		// Token: 0x0400065C RID: 1628
		public const int Flag_AnchorReset = 16;

		// Token: 0x0400065D RID: 1629
		public const int Flag_NegativeScale = 17;

		// Token: 0x0400065E RID: 1630
		public const int Flag_NegativeScaleTeleport = 18;

		// Token: 0x0400065F RID: 1631
		public const int Flag_DistanceCullingInvisible = 19;

		// Token: 0x04000660 RID: 1632
		public const int Flag_RestoreTransformOnlyOnec = 20;

		// Token: 0x04000661 RID: 1633
		public const int Flag_Tangent = 21;

		// Token: 0x04000662 RID: 1634
		public const int Flag_ScaleSuspent = 22;

		// Token: 0x04000663 RID: 1635
		public const int Flag_Self_PointPrimitive = 32;

		// Token: 0x04000664 RID: 1636
		public const int Flag_Self_EdgePrimitive = 33;

		// Token: 0x04000665 RID: 1637
		public const int Flag_Self_TrianglePrimitive = 34;

		// Token: 0x04000666 RID: 1638
		public const int Flag_Self_EdgeEdge = 35;

		// Token: 0x04000667 RID: 1639
		public const int Flag_Sync_EdgeEdge = 36;

		// Token: 0x04000668 RID: 1640
		public const int Flag_PSync_EdgeEdge = 37;

		// Token: 0x04000669 RID: 1641
		public const int Flag_Self_PointTriangle = 38;

		// Token: 0x0400066A RID: 1642
		public const int Flag_Sync_PointTriangle = 39;

		// Token: 0x0400066B RID: 1643
		public const int Flag_PSync_PointTriangle = 40;

		// Token: 0x0400066C RID: 1644
		public const int Flag_Self_TrianglePoint = 41;

		// Token: 0x0400066D RID: 1645
		public const int Flag_Sync_TrianglePoint = 42;

		// Token: 0x0400066E RID: 1646
		public const int Flag_PSync_TrianglePoint = 43;

		// Token: 0x0400066F RID: 1647
		public const int Flag_Self_EdgeTriangleIntersect = 44;

		// Token: 0x04000670 RID: 1648
		public const int Flag_Sync_EdgeTriangleIntersect = 45;

		// Token: 0x04000671 RID: 1649
		public const int Flag_PSync_EdgeTriangleIntersect = 46;

		// Token: 0x04000672 RID: 1650
		public const int Flag_Self_TriangleEdgeIntersect = 47;

		// Token: 0x04000673 RID: 1651
		public const int Flag_Sync_TriangleEdgeIntersect = 48;

		// Token: 0x04000674 RID: 1652
		public const int Flag_PSync_TriangleEdgeIntersect = 49;

		// Token: 0x04000675 RID: 1653
		public ExNativeArray<TeamManager.TeamData> teamDataArray;

		// Token: 0x04000676 RID: 1654
		public ExNativeArray<TeamWindData> teamWindArray;

		// Token: 0x04000677 RID: 1655
		public const int MappingDataFlag_ChangePositionNormal = 0;

		// Token: 0x04000678 RID: 1656
		public const int MappingDataFlag_ChangeTangent = 1;

		// Token: 0x04000679 RID: 1657
		public const int MappingDataFlag_ChangeBoneWeight = 2;

		// Token: 0x0400067A RID: 1658
		public const int MappingDataFlag_ModifyBoneWeight = 3;

		// Token: 0x0400067B RID: 1659
		public ExNativeArray<TeamManager.MappingData> mappingDataArray;

		// Token: 0x0400067C RID: 1660
		public ExNativeArray<FixedList64Bytes<short>> teamMappingIndexArray;

		// Token: 0x0400067D RID: 1661
		public NativeReference<int4> teamStatus;

		// Token: 0x0400067E RID: 1662
		public ExNativeArray<ClothParameters> parameterArray;

		// Token: 0x0400067F RID: 1663
		public ExNativeArray<InertiaConstraint.CenterData> centerDataArray;

		// Token: 0x04000680 RID: 1664
		private HashSet<int> enableTeamSet = new HashSet<int>();

		// Token: 0x04000681 RID: 1665
		private Dictionary<int, ClothProcess> clothProcessDict = new Dictionary<int, ClothProcess>();

		// Token: 0x04000682 RID: 1666
		private bool isValid;

		// Token: 0x04000683 RID: 1667
		internal int edgeColliderCollisionCount;

		// Token: 0x04000684 RID: 1668
		internal NativeReference<int> edgeColliderCollisionCountBuff;

		// Token: 0x04000685 RID: 1669
		internal NativeParallelHashMap<int, int> comp2SuspendCounterMap;

		// Token: 0x04000686 RID: 1670
		internal NativeParallelHashMap<int, int> comp2TeamIdMap;

		// Token: 0x04000687 RID: 1671
		internal NativeParallelHashMap<int, int> comp2SyncPartnerCompMap;

		// Token: 0x04000688 RID: 1672
		internal NativeParallelHashMap<int, int> comp2SyncTopCompMap;

		// Token: 0x04000689 RID: 1673
		internal NativeList<int> batchNormalClothTeamList;

		// Token: 0x0400068A RID: 1674
		internal NativeList<int> batchSplitClothTeamList;

		// Token: 0x0400068B RID: 1675
		internal List<ClothProcess> parameterDirtyList;

		// Token: 0x0400068C RID: 1676
		internal List<ClothProcess> skipWritingDirtyList;

		// Token: 0x0400068D RID: 1677
		internal NativeList<int> cullingDirtyList;

		// Token: 0x0400068E RID: 1678
		internal NativeParallelHashSet<int> selfCollisionUpdateSet;

		// Token: 0x0400068F RID: 1679
		internal NativeParallelHashMap<int, int> animatorUpdateModeMap;

		// Token: 0x04000690 RID: 1680
		internal ExSimpleNativeArray<int> teamAnchorTransformIndexArray;

		// Token: 0x04000691 RID: 1681
		internal ExSimpleNativeArray<int> teamDistanceTransformIndexArray;

		// Token: 0x04000692 RID: 1682
		internal NativeParallelHashMap<int, float3> transformPositionMap;

		// Token: 0x04000693 RID: 1683
		internal NativeParallelHashMap<int, quaternion> transformRotationMap;

		// Token: 0x04000694 RID: 1684
		internal HashSet<MagicaCloth> cameraCullingClothSet = new HashSet<MagicaCloth>(256);

		// Token: 0x04000695 RID: 1685
		private static readonly ProfilerMarker teamCameraCullingPreProfiler = new ProfilerMarker("CameraCullingPre");

		// Token: 0x04000696 RID: 1686
		private static readonly ProfilerMarker teamCameraCullingProfiler = new ProfilerMarker("CameraCullingPost");

		// Token: 0x04000697 RID: 1687
		private static readonly ProfilerMarker startClothUpdateComponentProfiler = new ProfilerMarker("StartClothUpdate.Component");

		// Token: 0x04000698 RID: 1688
		private HashSet<ClothProcess> monitoringProcessSet = new HashSet<ClothProcess>();

		// Token: 0x04000699 RID: 1689
		private List<ClothProcess> disposeProcessList = new List<ClothProcess>();

		// Token: 0x020000B4 RID: 180
		public struct TeamData
		{
			// Token: 0x1700004A RID: 74
			// (get) Token: 0x06000310 RID: 784 RVA: 0x000210FA File Offset: 0x0001F2FA
			public bool IsFixedUpdate
			{
				get
				{
					return this.updateMode == ClothUpdateMode.UnityPhysics;
				}
			}

			// Token: 0x1700004B RID: 75
			// (get) Token: 0x06000311 RID: 785 RVA: 0x00021105 File Offset: 0x0001F305
			public bool IsUnscaled
			{
				get
				{
					return this.updateMode == ClothUpdateMode.Unscaled;
				}
			}

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x06000312 RID: 786 RVA: 0x00021110 File Offset: 0x0001F310
			public bool IsValid
			{
				get
				{
					return this.flag.IsSet(0);
				}
			}

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x06000313 RID: 787 RVA: 0x0002111E File Offset: 0x0001F31E
			public bool IsEnable
			{
				get
				{
					return this.flag.IsSet(1);
				}
			}

			// Token: 0x1700004E RID: 78
			// (get) Token: 0x06000314 RID: 788 RVA: 0x0002112C File Offset: 0x0001F32C
			public bool IsProcess
			{
				get
				{
					return this.IsEnable && !this.flag.IsSet(4) && !this.IsCullingInvisible && !this.IsScaleSuspend;
				}
			}

			// Token: 0x1700004F RID: 79
			// (get) Token: 0x06000315 RID: 789 RVA: 0x00021157 File Offset: 0x0001F357
			public bool IsReset
			{
				get
				{
					return this.flag.IsSet(2);
				}
			}

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x06000316 RID: 790 RVA: 0x00021165 File Offset: 0x0001F365
			public bool IsKeepReset
			{
				get
				{
					return this.flag.IsSet(9);
				}
			}

			// Token: 0x17000051 RID: 81
			// (get) Token: 0x06000317 RID: 791 RVA: 0x00021174 File Offset: 0x0001F374
			public bool IsInertiaShift
			{
				get
				{
					return this.flag.IsSet(10);
				}
			}

			// Token: 0x17000052 RID: 82
			// (get) Token: 0x06000318 RID: 792 RVA: 0x00021183 File Offset: 0x0001F383
			public bool IsRunning
			{
				get
				{
					return this.flag.IsSet(5);
				}
			}

			// Token: 0x17000053 RID: 83
			// (get) Token: 0x06000319 RID: 793 RVA: 0x00021191 File Offset: 0x0001F391
			public bool IsStepRunning
			{
				get
				{
					return this.flag.IsSet(7);
				}
			}

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x0600031A RID: 794 RVA: 0x0002119F File Offset: 0x0001F39F
			public bool IsCameraCullingInvisible
			{
				get
				{
					return this.flag.IsSet(11);
				}
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x0600031B RID: 795 RVA: 0x000211AE File Offset: 0x0001F3AE
			public bool IsCameraCullingKeep
			{
				get
				{
					return this.flag.IsSet(12);
				}
			}

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x0600031C RID: 796 RVA: 0x000211BD File Offset: 0x0001F3BD
			public bool IsDistanceCullingInvisible
			{
				get
				{
					return this.flag.IsSet(19);
				}
			}

			// Token: 0x17000057 RID: 87
			// (get) Token: 0x0600031D RID: 797 RVA: 0x000211CC File Offset: 0x0001F3CC
			public bool IsCullingInvisible
			{
				get
				{
					return this.IsCameraCullingInvisible || this.IsDistanceCullingInvisible;
				}
			}

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x0600031E RID: 798 RVA: 0x000211DE File Offset: 0x0001F3DE
			public bool IsSpring
			{
				get
				{
					return this.flag.IsSet(13);
				}
			}

			// Token: 0x17000059 RID: 89
			// (get) Token: 0x0600031F RID: 799 RVA: 0x000211ED File Offset: 0x0001F3ED
			public bool IsNegativeScale
			{
				get
				{
					return this.flag.IsSet(17);
				}
			}

			// Token: 0x1700005A RID: 90
			// (get) Token: 0x06000320 RID: 800 RVA: 0x000211FC File Offset: 0x0001F3FC
			public bool IsNegativeScaleTeleport
			{
				get
				{
					return this.flag.IsSet(18);
				}
			}

			// Token: 0x1700005B RID: 91
			// (get) Token: 0x06000321 RID: 801 RVA: 0x0002120B File Offset: 0x0001F40B
			public bool IsTangent
			{
				get
				{
					return this.flag.IsSet(21);
				}
			}

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x06000322 RID: 802 RVA: 0x0002121A File Offset: 0x0001F41A
			public bool IsScaleSuspend
			{
				get
				{
					return this.flag.IsSet(22);
				}
			}

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x06000323 RID: 803 RVA: 0x00021229 File Offset: 0x0001F429
			public int ParticleCount
			{
				get
				{
					return this.particleChunk.dataLength;
				}
			}

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x06000324 RID: 804 RVA: 0x00021236 File Offset: 0x0001F436
			public int UseColliderCount
			{
				get
				{
					return this.colliderCount;
				}
			}

			// Token: 0x1700005F RID: 95
			// (get) Token: 0x06000325 RID: 805 RVA: 0x0002123E File Offset: 0x0001F43E
			public int BaseLineCount
			{
				get
				{
					return this.baseLineChunk.dataLength;
				}
			}

			// Token: 0x17000060 RID: 96
			// (get) Token: 0x06000326 RID: 806 RVA: 0x0002124B File Offset: 0x0001F44B
			public int TriangleCount
			{
				get
				{
					return this.proxyTriangleChunk.dataLength;
				}
			}

			// Token: 0x17000061 RID: 97
			// (get) Token: 0x06000327 RID: 807 RVA: 0x00021258 File Offset: 0x0001F458
			public int EdgeCount
			{
				get
				{
					return this.proxyEdgeChunk.dataLength;
				}
			}

			// Token: 0x17000062 RID: 98
			// (get) Token: 0x06000328 RID: 808 RVA: 0x00021265 File Offset: 0x0001F465
			public float InitScale
			{
				get
				{
					return this.initScale.x;
				}
			}

			// Token: 0x0400069A RID: 1690
			public BitField64 flag;

			// Token: 0x0400069B RID: 1691
			public ClothUpdateMode originalUpdateMode;

			// Token: 0x0400069C RID: 1692
			public ClothUpdateMode updateMode;

			// Token: 0x0400069D RID: 1693
			public float frameDeltaTime;

			// Token: 0x0400069E RID: 1694
			public float time;

			// Token: 0x0400069F RID: 1695
			public float oldTime;

			// Token: 0x040006A0 RID: 1696
			public float nowUpdateTime;

			// Token: 0x040006A1 RID: 1697
			public float oldUpdateTime;

			// Token: 0x040006A2 RID: 1698
			public float frameUpdateTime;

			// Token: 0x040006A3 RID: 1699
			public float frameOldTime;

			// Token: 0x040006A4 RID: 1700
			public float timeScale;

			// Token: 0x040006A5 RID: 1701
			public float nowTimeScale;

			// Token: 0x040006A6 RID: 1702
			public int updateCount;

			// Token: 0x040006A7 RID: 1703
			public int skipCount;

			// Token: 0x040006A8 RID: 1704
			public float frameInterpolation;

			// Token: 0x040006A9 RID: 1705
			public float gravityRatio;

			// Token: 0x040006AA RID: 1706
			public float gravityDot;

			// Token: 0x040006AB RID: 1707
			public int centerTransformIndex;

			// Token: 0x040006AC RID: 1708
			public int distanceReferenceObjectId;

			// Token: 0x040006AD RID: 1709
			public int componentTransformIndex;

			// Token: 0x040006AE RID: 1710
			public float3 initScale;

			// Token: 0x040006AF RID: 1711
			public float scaleRatio;

			// Token: 0x040006B0 RID: 1712
			public float negativeScaleSign;

			// Token: 0x040006B1 RID: 1713
			public float3 negativeScaleDirection;

			// Token: 0x040006B2 RID: 1714
			public float3 negativeScaleChange;

			// Token: 0x040006B3 RID: 1715
			public float2 negativeScaleTriangleSign;

			// Token: 0x040006B4 RID: 1716
			public float4 negativeScaleQuaternionValue;

			// Token: 0x040006B5 RID: 1717
			public int componentId;

			// Token: 0x040006B6 RID: 1718
			public int syncTeamId;

			// Token: 0x040006B7 RID: 1719
			public FixedList32Bytes<int> syncParentTeamId;

			// Token: 0x040006B8 RID: 1720
			public int syncCenterTransformIndex;

			// Token: 0x040006B9 RID: 1721
			public int interlockingAnimatorId;

			// Token: 0x040006BA RID: 1722
			public float animationPoseRatio;

			// Token: 0x040006BB RID: 1723
			public float velocityWeight;

			// Token: 0x040006BC RID: 1724
			public float distanceWeight;

			// Token: 0x040006BD RID: 1725
			public float blendWeight;

			// Token: 0x040006BE RID: 1726
			public ClothForceMode forceMode;

			// Token: 0x040006BF RID: 1727
			public float3 impactForce;

			// Token: 0x040006C0 RID: 1728
			public VirtualMesh.MeshType proxyMeshType;

			// Token: 0x040006C1 RID: 1729
			public DataChunk proxyTransformChunk;

			// Token: 0x040006C2 RID: 1730
			public DataChunk proxyCommonChunk;

			// Token: 0x040006C3 RID: 1731
			public DataChunk proxyVertexChildDataChunk;

			// Token: 0x040006C4 RID: 1732
			public DataChunk proxyTriangleChunk;

			// Token: 0x040006C5 RID: 1733
			public DataChunk proxyEdgeChunk;

			// Token: 0x040006C6 RID: 1734
			public DataChunk proxyMeshChunk;

			// Token: 0x040006C7 RID: 1735
			public DataChunk proxyBoneChunk;

			// Token: 0x040006C8 RID: 1736
			public DataChunk proxySkinBoneChunk;

			// Token: 0x040006C9 RID: 1737
			public DataChunk baseLineChunk;

			// Token: 0x040006CA RID: 1738
			public DataChunk baseLineDataChunk;

			// Token: 0x040006CB RID: 1739
			public DataChunk fixedDataChunk;

			// Token: 0x040006CC RID: 1740
			public DataChunk particleChunk;

			// Token: 0x040006CD RID: 1741
			public DataChunk colliderChunk;

			// Token: 0x040006CE RID: 1742
			public DataChunk colliderTransformChunk;

			// Token: 0x040006CF RID: 1743
			public int colliderCount;

			// Token: 0x040006D0 RID: 1744
			public DataChunk distanceStartChunk;

			// Token: 0x040006D1 RID: 1745
			public DataChunk distanceDataChunk;

			// Token: 0x040006D2 RID: 1746
			public DataChunk bendingPairChunk;

			// Token: 0x040006D3 RID: 1747
			public DataChunk selfPointChunk;

			// Token: 0x040006D4 RID: 1748
			public DataChunk selfEdgeChunk;

			// Token: 0x040006D5 RID: 1749
			public DataChunk selfTriangleChunk;

			// Token: 0x040006D6 RID: 1750
			public float selfGridSize;

			// Token: 0x040006D7 RID: 1751
			public int selfPointGridCount;

			// Token: 0x040006D8 RID: 1752
			public int selfEdgeGridCount;

			// Token: 0x040006D9 RID: 1753
			public int selfTriangleGridCount;

			// Token: 0x040006DA RID: 1754
			public float selfMaxPrimitiveSize;
		}

		// Token: 0x020000B5 RID: 181
		public struct MappingData : IValid
		{
			// Token: 0x06000329 RID: 809 RVA: 0x00021272 File Offset: 0x0001F472
			public bool IsValid()
			{
				return this.teamId > 0;
			}

			// Token: 0x17000063 RID: 99
			// (get) Token: 0x0600032A RID: 810 RVA: 0x0002127D File Offset: 0x0001F47D
			public int VertexCount
			{
				get
				{
					return this.mappingCommonChunk.dataLength;
				}
			}

			// Token: 0x040006DB RID: 1755
			public int teamId;

			// Token: 0x040006DC RID: 1756
			public BitField32 flag;

			// Token: 0x040006DD RID: 1757
			public int centerTransformIndex;

			// Token: 0x040006DE RID: 1758
			public DataChunk mappingCommonChunk;

			// Token: 0x040006DF RID: 1759
			public float4x4 toProxyMatrix;

			// Token: 0x040006E0 RID: 1760
			public quaternion toProxyRotation;

			// Token: 0x040006E1 RID: 1761
			public bool sameSpace;

			// Token: 0x040006E2 RID: 1762
			public float4x4 toMappingMatrix;

			// Token: 0x040006E3 RID: 1763
			public quaternion toMappingRotation;

			// Token: 0x040006E4 RID: 1764
			public float scaleRatio;

			// Token: 0x040006E5 RID: 1765
			public int renderDataWorkIndex;
		}

		// Token: 0x020000B6 RID: 182
		[BurstCompile]
		private struct AlwaysTeamUpdatePreJob : IJob
		{
			// Token: 0x0600032B RID: 811 RVA: 0x0002128C File Offset: 0x0001F48C
			public unsafe void Execute()
			{
				int num = 0;
				TeamManager.TeamData* unsafePtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafePtr<TeamManager.TeamData>();
				foreach (KeyValue<int, int> keyValue in this.comp2TeamIdMap)
				{
					int num2 = *keyValue.Value;
					if (num2 != 0)
					{
						int key = keyValue.Key;
						ref TeamManager.TeamData ptr = ref unsafePtr[num2];
						ptr.flag.SetBits(20, false);
						if (ptr.flag.IsSet(1))
						{
							bool flag = true;
							if (!this.comp2SuspendCounterMap.ContainsKey(key) || this.comp2SuspendCounterMap[key] == 0)
							{
								flag = false;
							}
							if (this.comp2SyncPartnerCompMap.ContainsKey(key))
							{
								int num3 = this.comp2SyncPartnerCompMap[key];
								if (this.comp2TeamIdMap.ContainsKey(num3))
								{
									int num4 = this.comp2TeamIdMap[num3];
									if (num4 > 0)
									{
										ref TeamManager.TeamData ptr2 = ref unsafePtr[num4];
										int num5 = (this.comp2SuspendCounterMap.ContainsKey(num3) ? this.comp2SuspendCounterMap[num3] : 0);
										if (ptr2.IsEnable && num5 == 0)
										{
											flag = false;
										}
									}
								}
							}
							ptr.flag.SetBits(4, flag);
							if (!ptr.flag.IsSet(4))
							{
								int syncTeamId = ptr.syncTeamId;
								ptr.syncTeamId = 0;
								if (this.comp2SyncPartnerCompMap.ContainsKey(key))
								{
									int num6 = this.comp2SyncPartnerCompMap[key];
									if (this.comp2TeamIdMap.ContainsKey(num6))
									{
										int num7 = this.comp2TeamIdMap[num6];
										ptr.syncTeamId = num7;
									}
								}
								ptr.flag.SetBits(6, ptr.syncTeamId != 0);
								ptr.syncCenterTransformIndex = 0;
								if (syncTeamId != ptr.syncTeamId)
								{
									if (syncTeamId > 0)
									{
										(ref unsafePtr[syncTeamId].syncParentTeamId).MC2RemoveItemAtSwapBack(num2);
									}
									if (ptr.syncTeamId != 0)
									{
										ref TeamManager.TeamData ptr3 = ref unsafePtr[ptr.syncTeamId];
										if (ptr3.syncParentTeamId.Length != ptr3.syncParentTeamId.Capacity)
										{
											ptr3.syncParentTeamId.Add(in num2);
										}
										ptr.flag.SetBits(3, false);
									}
									this.selfCollisionUpdateSet.Add(num2);
								}
								ClothParameters clothParameters = this.parameterArray[num2];
								if (this.comp2SyncTopCompMap.ContainsKey(key))
								{
									int num8 = this.comp2SyncTopCompMap[key];
									if (this.comp2TeamIdMap.ContainsKey(num8))
									{
										int num9 = this.comp2TeamIdMap[num8];
										ref TeamManager.TeamData ptr4 = ref unsafePtr[num9];
										if (ptr4.IsValid)
										{
											ptr.originalUpdateMode = ptr4.originalUpdateMode;
											ptr.updateMode = ptr4.updateMode;
											ptr.time = ptr4.time;
											ptr.oldTime = ptr4.oldTime;
											ptr.nowUpdateTime = ptr4.nowUpdateTime;
											ptr.oldUpdateTime = ptr4.oldUpdateTime;
											ptr.frameUpdateTime = ptr4.frameUpdateTime;
											ptr.frameOldTime = ptr4.frameOldTime;
											ptr.timeScale = ptr4.timeScale;
											ptr.updateCount = ptr4.updateCount;
											ptr.frameInterpolation = ptr4.frameInterpolation;
											ptr.skipCount = ptr4.skipCount;
										}
										ClothParameters clothParameters2 = this.parameterArray[num9];
										clothParameters.inertiaConstraint.anchorInertia = clothParameters2.inertiaConstraint.anchorInertia;
										clothParameters.inertiaConstraint.worldInertia = clothParameters2.inertiaConstraint.worldInertia;
										clothParameters.inertiaConstraint.movementInertiaSmoothing = clothParameters2.inertiaConstraint.movementInertiaSmoothing;
										clothParameters.inertiaConstraint.movementSpeedLimit = clothParameters2.inertiaConstraint.movementSpeedLimit;
										clothParameters.inertiaConstraint.rotationSpeedLimit = clothParameters2.inertiaConstraint.rotationSpeedLimit;
										clothParameters.inertiaConstraint.teleportMode = clothParameters2.inertiaConstraint.teleportMode;
										clothParameters.inertiaConstraint.teleportDistance = clothParameters2.inertiaConstraint.teleportDistance;
										clothParameters.inertiaConstraint.teleportRotation = clothParameters2.inertiaConstraint.teleportRotation;
										this.parameterArray[num2] = clothParameters;
										ptr.syncCenterTransformIndex = ptr4.centerTransformIndex;
									}
								}
								if (clothParameters.colliderCollisionConstraint.mode == ColliderCollisionConstraint.Mode.Edge)
								{
									num += ptr.EdgeCount;
								}
							}
						}
					}
				}
				this.edgeColliderCollisionCountBuff.Value = num;
			}

			// Token: 0x040006E6 RID: 1766
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040006E7 RID: 1767
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x040006E8 RID: 1768
			public NativeParallelHashMap<int, int> comp2SuspendCounterMap;

			// Token: 0x040006E9 RID: 1769
			public NativeParallelHashMap<int, int> comp2TeamIdMap;

			// Token: 0x040006EA RID: 1770
			public NativeParallelHashMap<int, int> comp2SyncPartnerCompMap;

			// Token: 0x040006EB RID: 1771
			public NativeParallelHashMap<int, int> comp2SyncTopCompMap;

			// Token: 0x040006EC RID: 1772
			public NativeParallelHashSet<int> selfCollisionUpdateSet;

			// Token: 0x040006ED RID: 1773
			public NativeReference<int> edgeColliderCollisionCountBuff;
		}

		// Token: 0x020000B7 RID: 183
		[BurstCompile]
		private struct AlwaysTeamUpdatePostJob : IJob
		{
			// Token: 0x0600032C RID: 812 RVA: 0x00021728 File Offset: 0x0001F928
			public void Execute()
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				for (int i = 1; i < this.teamCount; i++)
				{
					TeamManager.TeamData teamData = this.teamDataArray[i];
					int componentId = teamData.componentId;
					if (teamData.IsEnable && !teamData.flag.IsSet(4))
					{
						ClothParameters clothParameters = this.parameterArray[i];
						teamData.flag.SetBits(22, this.componentMinScaleArray[teamData.componentTransformIndex] < 1E-06f);
						if (teamData.flag.IsSet(22))
						{
							this.teamDataArray[i] = teamData;
						}
						else
						{
							int num5 = this.teamAnchorTransformIndexArray[i];
							bool flag = teamData.flag.IsSet(15);
							bool flag2 = num5 != 0;
							teamData.flag.SetBits(15, flag2);
							teamData.flag.SetBits(16, flag != flag2);
							InertiaConstraint.CenterData centerData = this.centerDataArray[i];
							centerData.anchorPosition = ((num5 != 0) ? this.transformPositionMap[num5] : float3.zero);
							centerData.anchorRotation = ((num5 != 0) ? this.transformRotationMap[num5] : quaternion.identity);
							this.centerDataArray[i] = centerData;
							this.DistanceCullingUpdate(i, ref teamData, ref clothParameters);
							if (teamData.IsCullingInvisible)
							{
								this.teamDataArray[i] = teamData;
							}
							else
							{
								int num6 = 0;
								if (this.comp2SyncTopCompMap.ContainsKey(componentId))
								{
									int num7 = this.comp2SyncTopCompMap[componentId];
									if (this.comp2TeamIdMap.ContainsKey(num7))
									{
										num6 = this.comp2TeamIdMap[num7];
									}
								}
								if (teamData.originalUpdateMode == ClothUpdateMode.AnimatorLinkage || num6 > 0)
								{
									ClothUpdateMode clothUpdateMode = teamData.originalUpdateMode;
									int num8 = teamData.interlockingAnimatorId;
									if (num6 > 0)
									{
										TeamManager.TeamData teamData2 = this.teamDataArray[num6];
										clothUpdateMode = teamData2.originalUpdateMode;
										num8 = teamData2.interlockingAnimatorId;
									}
									if (clothUpdateMode > ClothUpdateMode.Unscaled)
									{
										if (clothUpdateMode != ClothUpdateMode.AnimatorLinkage)
										{
											teamData.updateMode = ClothUpdateMode.Normal;
										}
										else if (this.animatorUpdateModeMap.ContainsKey(num8))
										{
											switch (this.animatorUpdateModeMap[num8])
											{
											case 0:
												teamData.updateMode = ClothUpdateMode.Normal;
												break;
											case 1:
												teamData.updateMode = ClothUpdateMode.UnityPhysics;
												break;
											case 2:
												teamData.updateMode = ClothUpdateMode.Unscaled;
												break;
											default:
												teamData.updateMode = ClothUpdateMode.Normal;
												break;
											}
										}
										else
										{
											teamData.updateMode = ClothUpdateMode.Normal;
										}
									}
									else
									{
										teamData.updateMode = clothUpdateMode;
									}
								}
								if (teamData.flag.IsSet(3))
								{
									teamData.time = 0f;
									teamData.oldTime = 0f;
									teamData.nowUpdateTime = 0f;
									teamData.oldUpdateTime = 0f;
									teamData.frameUpdateTime = 0f;
									teamData.frameOldTime = 0f;
								}
								float num9 = (teamData.IsFixedUpdate ? this.unityFrameFixedDeltaTime : (teamData.IsUnscaled ? this.unityFrameUnscaledDeltaTime : this.unityFrameDeltaTime));
								teamData.frameDeltaTime = num9;
								float num10 = num9;
								float num11 = teamData.timeScale * (teamData.IsUnscaled ? 1f : this.globalTimeScale);
								num11 = (teamData.flag.IsSet(4) ? 0f : num11);
								teamData.nowTimeScale = num11;
								float num12 = num10 * num11;
								float num13 = teamData.time + num12;
								int num14 = (int)((num13 - teamData.nowUpdateTime) / this.simulationDeltaTime);
								teamData.updateCount = math.min(num14, this.maxSimmulationCountPerFrame);
								teamData.skipCount = num14 - teamData.updateCount;
								if (teamData.skipCount > 0)
								{
									num13 -= this.simulationDeltaTime * (float)teamData.skipCount;
								}
								if (teamData.updateCount > 0 && num12 == 0f)
								{
									teamData.updateCount = 0;
									teamData.skipCount = 0;
									teamData.nowUpdateTime = num13 - this.simulationDeltaTime + 0.0001f;
								}
								if (teamData.updateCount > 0)
								{
									teamData.frameOldTime = teamData.frameUpdateTime;
									teamData.frameUpdateTime = num13;
									teamData.oldUpdateTime = teamData.nowUpdateTime;
								}
								teamData.oldTime = teamData.time;
								teamData.time = num13;
								teamData.flag.SetBits(5, teamData.updateCount > 0);
								this.teamDataArray[i] = teamData;
								num = math.max(num, teamData.updateCount);
								bool flag3 = false;
								if (teamData.flag.IsSet(32) || teamData.flag.IsSet(33) || teamData.flag.IsSet(34))
								{
									this.batchSplitClothTeamList.Add(in i);
									flag3 = true;
									num4++;
								}
								else if (teamData.ParticleCount >= this.splitProxyMeshVertexCount)
								{
									this.batchSplitClothTeamList.Add(in i);
									flag3 = true;
								}
								else
								{
									this.batchNormalClothTeamList.Add(in i);
								}
								if (flag3 && teamData.UseColliderCount > 0)
								{
									if (clothParameters.colliderCollisionConstraint.mode == ColliderCollisionConstraint.Mode.Point)
									{
										num2++;
									}
									if (clothParameters.colliderCollisionConstraint.mode == ColliderCollisionConstraint.Mode.Edge)
									{
										num3++;
									}
								}
							}
						}
					}
				}
				this.teamStatus.Value = new int4(num, num2, num3, num4);
			}

			// Token: 0x0600032D RID: 813 RVA: 0x00021C70 File Offset: 0x0001FE70
			private void DistanceCullingUpdate(int teamId, ref TeamManager.TeamData tdata, ref ClothParameters param)
			{
				bool isDistanceCullingInvisible = tdata.IsDistanceCullingInvisible;
				bool flag;
				if (param.culling.useDistanceCulling)
				{
					int num = this.teamDistanceTransformIndexArray[teamId];
					if (this.transformPositionMap.ContainsKey(num))
					{
						float3 @float = this.componentPositionArray[tdata.componentTransformIndex];
						float3 float2 = this.transformPositionMap[num];
						float num2 = math.distance(@float, float2);
						float num3 = param.culling.distanceCullingLength;
						if (!this.hasMainCamera && tdata.componentTransformIndex == 0)
						{
							num3 = 1000000f;
						}
						flag = num2 >= num3;
						float num4 = math.saturate(param.culling.distanceCullingFadeRatio) * num3;
						tdata.distanceWeight = 1f - math.saturate(math.unlerp(num3 - num4, num3, num2));
					}
					else
					{
						flag = false;
						tdata.distanceWeight = 1f;
					}
				}
				else
				{
					flag = false;
					tdata.distanceWeight = 1f;
				}
				if (isDistanceCullingInvisible != flag)
				{
					tdata.flag.SetBits(19, flag);
					tdata.flag.SetBits(2, true);
					tdata.flag.SetBits(12, false);
					this.cullingDirtyList.Add(in teamId);
				}
			}

			// Token: 0x040006EE RID: 1774
			public int teamCount;

			// Token: 0x040006EF RID: 1775
			public float unityFrameDeltaTime;

			// Token: 0x040006F0 RID: 1776
			public float unityFrameFixedDeltaTime;

			// Token: 0x040006F1 RID: 1777
			public float unityFrameUnscaledDeltaTime;

			// Token: 0x040006F2 RID: 1778
			public float globalTimeScale;

			// Token: 0x040006F3 RID: 1779
			public float simulationDeltaTime;

			// Token: 0x040006F4 RID: 1780
			public int maxSimmulationCountPerFrame;

			// Token: 0x040006F5 RID: 1781
			public int splitProxyMeshVertexCount;

			// Token: 0x040006F6 RID: 1782
			public NativeReference<int4> teamStatus;

			// Token: 0x040006F7 RID: 1783
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040006F8 RID: 1784
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x040006F9 RID: 1785
			public NativeArray<InertiaConstraint.CenterData> centerDataArray;

			// Token: 0x040006FA RID: 1786
			public NativeArray<float3> componentPositionArray;

			// Token: 0x040006FB RID: 1787
			public NativeArray<float> componentMinScaleArray;

			// Token: 0x040006FC RID: 1788
			public bool hasMainCamera;

			// Token: 0x040006FD RID: 1789
			public NativeParallelHashMap<int, int> comp2TeamIdMap;

			// Token: 0x040006FE RID: 1790
			public NativeParallelHashMap<int, int> comp2SyncTopCompMap;

			// Token: 0x040006FF RID: 1791
			public NativeParallelHashMap<int, int> animatorUpdateModeMap;

			// Token: 0x04000700 RID: 1792
			public NativeArray<int> teamAnchorTransformIndexArray;

			// Token: 0x04000701 RID: 1793
			public NativeArray<int> teamDistanceTransformIndexArray;

			// Token: 0x04000702 RID: 1794
			public NativeParallelHashMap<int, float3> transformPositionMap;

			// Token: 0x04000703 RID: 1795
			public NativeParallelHashMap<int, quaternion> transformRotationMap;

			// Token: 0x04000704 RID: 1796
			public NativeList<int> cullingDirtyList;

			// Token: 0x04000705 RID: 1797
			public NativeList<int> batchNormalClothTeamList;

			// Token: 0x04000706 RID: 1798
			public NativeList<int> batchSplitClothTeamList;
		}
	}
}
