using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using Unity.Profiling;
using UnityEngine.SceneManagement;

namespace UnityEngine.Rendering
{
	// Token: 0x0200008B RID: 139
	public class ProbeReferenceVolume
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x00013B6F File Offset: 0x00011D6F
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x00013B77 File Offset: 0x00011D77
		internal Bounds globalBounds
		{
			get
			{
				return this.m_CurrGlobalBounds;
			}
			set
			{
				this.m_CurrGlobalBounds = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00013B80 File Offset: 0x00011D80
		public bool isInitialized
		{
			get
			{
				return this.m_ProbeReferenceVolumeInit;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00013B88 File Offset: 0x00011D88
		internal bool enabledBySRP
		{
			get
			{
				return this.m_EnabledBySRP;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00013B90 File Offset: 0x00011D90
		internal bool hasUnloadedCells
		{
			get
			{
				return this.m_ToBeLoadedCells.size != 0;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00013BA0 File Offset: 0x00011DA0
		internal bool enableScenarioBlending
		{
			get
			{
				return this.m_BlendingMemoryBudget != ProbeVolumeBlendingTextureMemoryBudget.None && ProbeBrickBlendingPool.isSupported;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00013BB1 File Offset: 0x00011DB1
		internal int numberOfCellsLoadedPerFrame
		{
			get
			{
				return this.m_NumberOfCellsLoadedPerFrame;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00013BB9 File Offset: 0x00011DB9
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x00013BC1 File Offset: 0x00011DC1
		public int numberOfCellsBlendedPerFrame
		{
			get
			{
				return this.m_NumberOfCellsBlendedPerFrame;
			}
			set
			{
				this.m_NumberOfCellsBlendedPerFrame = Mathf.Max(1, value);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x00013BD0 File Offset: 0x00011DD0
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x00013BD8 File Offset: 0x00011DD8
		public float turnoverRate
		{
			get
			{
				return this.m_TurnoverRate;
			}
			set
			{
				this.m_TurnoverRate = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00013BE6 File Offset: 0x00011DE6
		public ProbeVolumeSHBands shBands
		{
			get
			{
				return this.m_SHBands;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00013BEE File Offset: 0x00011DEE
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x00013BFB File Offset: 0x00011DFB
		public string lightingScenario
		{
			get
			{
				return this.sceneData.lightingScenario;
			}
			set
			{
				this.sceneData.SetActiveScenario(value);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00013C09 File Offset: 0x00011E09
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x00013C16 File Offset: 0x00011E16
		public float scenarioBlendingFactor
		{
			get
			{
				return this.sceneData.scenarioBlendingFactor;
			}
			set
			{
				this.sceneData.BlendLightingScenario(this.sceneData.otherScenario, value);
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00013C2F File Offset: 0x00011E2F
		public void BlendLightingScenario(string otherScenario, float blendingFactor)
		{
			this.sceneData.BlendLightingScenario(otherScenario, blendingFactor);
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00013C3E File Offset: 0x00011E3E
		public ProbeVolumeTextureMemoryBudget memoryBudget
		{
			get
			{
				return this.m_MemoryBudget;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00013C46 File Offset: 0x00011E46
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00013C4E File Offset: 0x00011E4E
		public float probeVolumesWeight
		{
			get
			{
				return this.m_ProbeVolumesWeight;
			}
			set
			{
				this.m_ProbeVolumesWeight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00013C5C File Offset: 0x00011E5C
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x00013C64 File Offset: 0x00011E64
		internal List<ProbeVolumePerSceneData> perSceneDataList { get; private set; } = new List<ProbeVolumePerSceneData>();

		// Token: 0x0600046A RID: 1130 RVA: 0x00013C6D File Offset: 0x00011E6D
		internal void RegisterPerSceneData(ProbeVolumePerSceneData data)
		{
			if (!this.perSceneDataList.Contains(data))
			{
				this.perSceneDataList.Add(data);
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00013C89 File Offset: 0x00011E89
		internal void UnregisterPerSceneData(ProbeVolumePerSceneData data)
		{
			this.perSceneDataList.Remove(data);
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00013C98 File Offset: 0x00011E98
		public static ProbeReferenceVolume instance
		{
			get
			{
				return ProbeReferenceVolume._instance;
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00013CA0 File Offset: 0x00011EA0
		public void Initialize(in ProbeVolumeSystemParameters parameters)
		{
			if (this.m_IsInitialized)
			{
				Debug.LogError("Probe Volume System has already been initialized.");
				return;
			}
			this.m_MemoryBudget = parameters.memoryBudget;
			this.m_BlendingMemoryBudget = parameters.blendingMemoryBudget;
			this.m_SHBands = parameters.shBands;
			this.m_ProbeVolumesWeight = 1f;
			this.InitializeDebug(in parameters);
			ProbeBrickBlendingPool.Initialize(in parameters);
			this.InitProbeReferenceVolume(this.m_MemoryBudget, this.m_BlendingMemoryBudget, this.m_SHBands);
			this.m_IsInitialized = true;
			this.m_NeedsIndexRebuild = true;
			this.sceneData = parameters.sceneData;
			this.m_SupportStreaming = parameters.supportStreaming;
			this.m_EnabledBySRP = true;
			if (this.sceneData != null)
			{
				foreach (ProbeVolumePerSceneData probeVolumePerSceneData in ProbeReferenceVolume.instance.perSceneDataList)
				{
					probeVolumePerSceneData.Initialize();
				}
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00013D90 File Offset: 0x00011F90
		public void SetEnableStateFromSRP(bool srpEnablesPV)
		{
			this.m_EnabledBySRP = srpEnablesPV;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00013D99 File Offset: 0x00011F99
		internal void ForceSHBand(ProbeVolumeSHBands shBands)
		{
			if (this.m_ProbeReferenceVolumeInit)
			{
				this.CleanupLoadedData();
			}
			this.m_SHBands = shBands;
			this.m_ProbeReferenceVolumeInit = false;
			this.InitProbeReferenceVolume(this.m_MemoryBudget, this.m_BlendingMemoryBudget, shBands);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00013DCA File Offset: 0x00011FCA
		public void Cleanup()
		{
			if (!this.m_ProbeReferenceVolumeInit)
			{
				return;
			}
			if (!this.m_IsInitialized)
			{
				Debug.LogError("Probe Volume System has not been initialized first before calling cleanup.");
				return;
			}
			this.CleanupLoadedData();
			this.CleanupDebug();
			this.m_IsInitialized = false;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00013DFC File Offset: 0x00011FFC
		public int GetVideoMemoryCost()
		{
			if (!this.m_ProbeReferenceVolumeInit)
			{
				return 0;
			}
			return this.m_Pool.estimatedVMemCost + this.m_Index.estimatedVMemCost + this.m_CellIndices.estimatedVMemCost + this.m_BlendingPool.estimatedVMemCost + this.m_TemporaryDataLocationMemCost;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00013E4C File Offset: 0x0001204C
		private void RemoveCell(ProbeReferenceVolume.Cell cell)
		{
			ProbeReferenceVolume.CellInfo cellInfo;
			if (this.cells.TryGetValue(cell.index, out cellInfo))
			{
				cellInfo.referenceCount--;
				if (cellInfo.referenceCount <= 0)
				{
					this.cells.Remove(cell.index);
					if (cellInfo.loaded)
					{
						this.m_LoadedCells.Remove(cellInfo);
						this.UnloadCell(cellInfo);
					}
					else
					{
						this.m_ToBeLoadedCells.Remove(cellInfo);
					}
					this.m_BlendingCellInfoPool.Release(cellInfo.blendingCell);
					this.m_CellInfoPool.Release(cellInfo);
				}
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00013EE0 File Offset: 0x000120E0
		internal void UnloadCell(ProbeReferenceVolume.CellInfo cellInfo)
		{
			if (cellInfo.loaded)
			{
				if (cellInfo.blendingCell.blending)
				{
					this.m_LoadedBlendingCells.Remove(cellInfo.blendingCell);
					this.UnloadBlendingCell(cellInfo.blendingCell);
				}
				else
				{
					this.m_ToBeLoadedBlendingCells.Remove(cellInfo.blendingCell);
				}
				if (cellInfo.flatIdxInCellIndices >= 0)
				{
					this.m_CellIndices.MarkCellAsUnloaded(cellInfo.flatIdxInCellIndices);
				}
				this.ReleaseBricks(cellInfo);
				cellInfo.loaded = false;
				cellInfo.debugProbes = null;
				cellInfo.updateInfo = default(ProbeBrickIndex.CellIndexUpdateInfo);
				this.ClearDebugData();
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00013F78 File Offset: 0x00012178
		internal void UnloadBlendingCell(ProbeReferenceVolume.BlendingCellInfo blendingCell)
		{
			if (blendingCell.blending)
			{
				this.m_BlendingPool.Deallocate(blendingCell.chunkList);
				blendingCell.chunkList.Clear();
				blendingCell.blending = false;
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00013FA8 File Offset: 0x000121A8
		internal unsafe void UnloadAllCells()
		{
			for (int i = 0; i < this.m_LoadedCells.size; i++)
			{
				this.UnloadCell(*this.m_LoadedCells[i]);
			}
			this.m_ToBeLoadedCells.AddRange(this.m_LoadedCells);
			this.m_LoadedCells.Clear();
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00013FFC File Offset: 0x000121FC
		internal unsafe void UnloadAllBlendingCells()
		{
			for (int i = 0; i < this.m_LoadedBlendingCells.size; i++)
			{
				this.UnloadBlendingCell(*this.m_LoadedBlendingCells[i]);
			}
			this.m_ToBeLoadedBlendingCells.AddRange(this.m_LoadedBlendingCells);
			this.m_LoadedBlendingCells.Clear();
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00014050 File Offset: 0x00012250
		private void AddCell(ProbeReferenceVolume.Cell cell, int assetInstanceID)
		{
			ProbeReferenceVolume.CellInfo cellInfo;
			if (!this.cells.TryGetValue(cell.index, out cellInfo))
			{
				cellInfo = this.m_CellInfoPool.Get();
				cellInfo.cell = cell;
				cellInfo.flatIdxInCellIndices = this.m_CellIndices.GetFlatIdxForCell(cell.position);
				cellInfo.sourceAssetInstanceID = assetInstanceID;
				cellInfo.referenceCount = 1;
				this.cells[cell.index] = cellInfo;
				ProbeReferenceVolume.BlendingCellInfo blendingCellInfo = this.m_BlendingCellInfoPool.Get();
				blendingCellInfo.cellInfo = cellInfo;
				cellInfo.blendingCell = blendingCellInfo;
				this.m_ToBeLoadedCells.Add(in cellInfo);
				return;
			}
			cellInfo.referenceCount++;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000140F4 File Offset: 0x000122F4
		internal bool LoadCell(ProbeReferenceVolume.CellInfo cellInfo, bool ignoreErrorLog = false)
		{
			ProbeBrickIndex.CellIndexUpdateInfo cellIndexUpdateInfo;
			if (this.GetCellIndexUpdate(cellInfo.cell, out cellIndexUpdateInfo, ignoreErrorLog))
			{
				this.minLoadedCellPos = Vector3Int.Min(this.minLoadedCellPos, cellInfo.cell.position);
				this.maxLoadedCellPos = Vector3Int.Max(this.maxLoadedCellPos, cellInfo.cell.position);
				return this.AddBricks(cellInfo, cellIndexUpdateInfo, ignoreErrorLog);
			}
			return false;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00014158 File Offset: 0x00012358
		internal unsafe void LoadAllCells()
		{
			int size = this.m_LoadedCells.size;
			for (int i = 0; i < this.m_ToBeLoadedCells.size; i++)
			{
				ProbeReferenceVolume.CellInfo cellInfo = *this.m_ToBeLoadedCells[i];
				if (this.LoadCell(cellInfo, true))
				{
					this.m_LoadedCells.Add(in cellInfo);
				}
			}
			for (int j = size; j < this.m_LoadedCells.size; j++)
			{
				this.m_ToBeLoadedCells.Remove(*this.m_LoadedCells[j]);
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000141DC File Offset: 0x000123DC
		private void RecomputeMinMaxLoadedCellPos()
		{
			this.minLoadedCellPos = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
			this.maxLoadedCellPos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
			foreach (ProbeReferenceVolume.CellInfo cellInfo in this.cells.Values)
			{
				if (cellInfo.loaded)
				{
					this.minLoadedCellPos = Vector3Int.Min(cellInfo.cell.position, this.minLoadedCellPos);
					this.maxLoadedCellPos = Vector3Int.Max(cellInfo.cell.position, this.maxLoadedCellPos);
				}
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000142A4 File Offset: 0x000124A4
		private bool CheckCompatibilityWithCollection(ProbeVolumeAsset asset, Dictionary<string, ProbeVolumeAsset> collection)
		{
			if (collection.Count > 0)
			{
				foreach (ProbeVolumeAsset probeVolumeAsset in collection.Values)
				{
					if (!this.m_PendingAssetsToBeUnloaded.ContainsKey(probeVolumeAsset.GetSerializedFullPath()))
					{
						return probeVolumeAsset.CompatibleWith(asset);
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001431C File Offset: 0x0001251C
		internal void AddPendingAssetLoading(ProbeVolumeAsset asset)
		{
			string serializedFullPath = asset.GetSerializedFullPath();
			if (this.m_PendingAssetsToBeLoaded.ContainsKey(serializedFullPath))
			{
				this.m_PendingAssetsToBeLoaded.Remove(serializedFullPath);
			}
			if (!this.CheckCompatibilityWithCollection(asset, this.m_ActiveAssets))
			{
				Debug.LogError("Trying to load Probe Volume data for a scene that has been baked with different settings than currently loaded ones. Please make sure all loaded scenes are in the same baking set.");
				return;
			}
			if (!this.CheckCompatibilityWithCollection(asset, this.m_PendingAssetsToBeLoaded))
			{
				Debug.LogError("Trying to load Probe Volume data for a scene that has been baked with different settings from other scenes that are being loaded. Please make sure all loaded scenes are in the same baking set.");
				return;
			}
			this.m_PendingAssetsToBeLoaded.Add(serializedFullPath, asset);
			this.m_NeedLoadAsset = true;
			Vector3Int zero = Vector3Int.zero;
			Vector3Int vector3Int = Vector3Int.one * 10000;
			Vector3Int vector3Int2 = Vector3Int.one * -10000;
			bool flag = true;
			foreach (ProbeVolumeAsset probeVolumeAsset in this.m_PendingAssetsToBeLoaded.Values)
			{
				vector3Int = Vector3Int.Min(vector3Int, probeVolumeAsset.minCellPosition);
				vector3Int2 = Vector3Int.Max(vector3Int2, probeVolumeAsset.maxCellPosition);
				if (flag)
				{
					this.m_CurrGlobalBounds = probeVolumeAsset.globalBounds;
					flag = false;
				}
				else
				{
					this.m_CurrGlobalBounds.Encapsulate(probeVolumeAsset.globalBounds);
				}
			}
			foreach (ProbeVolumeAsset probeVolumeAsset2 in this.m_ActiveAssets.Values)
			{
				vector3Int = Vector3Int.Min(vector3Int, probeVolumeAsset2.minCellPosition);
				vector3Int2 = Vector3Int.Max(vector3Int2, probeVolumeAsset2.maxCellPosition);
				if (flag)
				{
					this.m_CurrGlobalBounds = probeVolumeAsset2.globalBounds;
					flag = false;
				}
				else
				{
					this.m_CurrGlobalBounds.Encapsulate(probeVolumeAsset2.globalBounds);
				}
			}
			this.m_NeedsIndexRebuild |= this.m_Index == null || this.m_PendingInitInfo.pendingMinCellPosition != vector3Int || this.m_PendingInitInfo.pendingMaxCellPosition != vector3Int2;
			this.m_PendingInitInfo.pendingMinCellPosition = vector3Int;
			this.m_PendingInitInfo.pendingMaxCellPosition = vector3Int2;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001451C File Offset: 0x0001271C
		internal void AddPendingAssetRemoval(ProbeVolumeAsset asset)
		{
			string serializedFullPath = asset.GetSerializedFullPath();
			if (this.m_PendingAssetsToBeLoaded.ContainsKey(serializedFullPath))
			{
				this.m_PendingAssetsToBeLoaded.Remove(serializedFullPath);
			}
			if (this.m_ActiveAssets.ContainsKey(serializedFullPath))
			{
				this.m_PendingAssetsToBeUnloaded[serializedFullPath] = asset;
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00014568 File Offset: 0x00012768
		internal unsafe void RemovePendingAsset(ProbeVolumeAsset asset)
		{
			string serializedFullPath = asset.GetSerializedFullPath();
			if (this.m_ActiveAssets.ContainsKey(serializedFullPath))
			{
				this.m_ActiveAssets.Remove(serializedFullPath);
			}
			foreach (ProbeReferenceVolume.Cell cell in asset.cells)
			{
				this.RemoveCell(cell);
			}
			int instanceID = asset.GetInstanceID();
			for (int j = this.m_LoadedCells.size - 1; j >= 0; j--)
			{
				if (this.m_LoadedCells[j]->sourceAssetInstanceID == instanceID)
				{
					if (this.m_LoadedCells[j]->blendingCell.blending)
					{
						this.m_LoadedBlendingCells.Remove(this.m_LoadedCells[j]->blendingCell);
					}
					else
					{
						this.m_ToBeLoadedBlendingCells.Remove(this.m_LoadedCells[j]->blendingCell);
					}
					this.m_LoadedCells.RemoveAt(j);
				}
			}
			for (int k = this.m_ToBeLoadedCells.size - 1; k >= 0; k--)
			{
				if (this.m_ToBeLoadedCells[k]->sourceAssetInstanceID == instanceID)
				{
					this.m_ToBeLoadedCells.RemoveAt(k);
				}
			}
			this.ClearDebugData();
			this.RecomputeMinMaxLoadedCellPos();
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000146AA File Offset: 0x000128AA
		private void PerformPendingIndexChangeAndInit()
		{
			if (this.m_NeedsIndexRebuild)
			{
				this.CleanupLoadedData();
				this.InitProbeReferenceVolume(this.m_MemoryBudget, this.m_BlendingMemoryBudget, this.m_SHBands);
				this.m_HasChangedIndex = true;
				this.m_NeedsIndexRebuild = false;
				return;
			}
			this.m_HasChangedIndex = false;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x000146E8 File Offset: 0x000128E8
		internal void SetMinBrickAndMaxSubdiv(float minBrickSize, int maxSubdiv)
		{
			this.SetTRS(Vector3.zero, Quaternion.identity, minBrickSize);
			this.SetMaxSubdivision(maxSubdiv);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00014704 File Offset: 0x00012904
		private void LoadAsset(ProbeVolumeAsset asset)
		{
			if (asset.Version != 5)
			{
				Debug.LogWarning("Trying to load an asset " + asset.GetSerializedFullPath() + " that has been baked with a previous version of the system. Please re-bake the data.");
				return;
			}
			this.SetMinBrickAndMaxSubdiv(asset.minBrickSize, asset.maxSubdivision);
			if (asset.chunkSizeInBricks != this.m_CurrentProbeVolumeChunkSizeInBricks)
			{
				this.m_CurrentProbeVolumeChunkSizeInBricks = asset.chunkSizeInBricks;
				this.AllocateTemporaryDataLocation();
			}
			this.ClearDebugData();
			for (int i = 0; i < asset.cells.Length; i++)
			{
				this.AddCell(asset.cells[i], asset.GetInstanceID());
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00014794 File Offset: 0x00012994
		private void PerformPendingLoading()
		{
			if ((this.m_PendingAssetsToBeLoaded.Count == 0 && this.m_ActiveAssets.Count == 0) || !this.m_NeedLoadAsset || !this.m_ProbeReferenceVolumeInit)
			{
				return;
			}
			this.m_Pool.EnsureTextureValidity();
			this.m_BlendingPool.EnsureTextureValidity();
			if (this.m_HasChangedIndex)
			{
				foreach (ProbeVolumeAsset probeVolumeAsset in this.m_ActiveAssets.Values)
				{
					this.LoadAsset(probeVolumeAsset);
				}
			}
			foreach (ProbeVolumeAsset probeVolumeAsset2 in this.m_PendingAssetsToBeLoaded.Values)
			{
				this.LoadAsset(probeVolumeAsset2);
				if (!this.m_ActiveAssets.ContainsKey(probeVolumeAsset2.GetSerializedFullPath()))
				{
					this.m_ActiveAssets.Add(probeVolumeAsset2.GetSerializedFullPath(), probeVolumeAsset2);
				}
			}
			this.m_PendingAssetsToBeLoaded.Clear();
			this.m_NeedLoadAsset = false;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x000148B4 File Offset: 0x00012AB4
		private void PerformPendingDeletion()
		{
			if (!this.m_ProbeReferenceVolumeInit)
			{
				this.m_PendingAssetsToBeUnloaded.Clear();
			}
			foreach (ProbeVolumeAsset probeVolumeAsset in this.m_PendingAssetsToBeUnloaded.Values)
			{
				this.RemovePendingAsset(probeVolumeAsset);
			}
			this.m_PendingAssetsToBeUnloaded.Clear();
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001492C File Offset: 0x00012B2C
		internal int GetNumberOfBricksAtSubdiv(Vector3Int position, int minSubdiv, out Vector3Int minValidLocalIdxAtMaxRes, out Vector3Int sizeOfValidIndicesAtMaxRes)
		{
			minValidLocalIdxAtMaxRes = Vector3Int.zero;
			sizeOfValidIndicesAtMaxRes = Vector3Int.one;
			Vector3 vector = new Vector3((float)position.x * this.MaxBrickSize(), (float)position.y * this.MaxBrickSize(), (float)position.z * this.MaxBrickSize());
			Bounds bounds = default(Bounds);
			bounds.min = vector;
			bounds.max = vector + Vector3.one * this.MaxBrickSize();
			Bounds bounds2 = default(Bounds);
			bounds2.min = Vector3.Max(bounds.min, this.m_CurrGlobalBounds.min);
			bounds2.max = Vector3.Min(bounds.max, this.m_CurrGlobalBounds.max);
			Vector3 vector2 = bounds2.min - bounds.min;
			minValidLocalIdxAtMaxRes.x = Mathf.CeilToInt(vector2.x / this.MinBrickSize());
			minValidLocalIdxAtMaxRes.y = Mathf.CeilToInt(vector2.y / this.MinBrickSize());
			minValidLocalIdxAtMaxRes.z = Mathf.CeilToInt(vector2.z / this.MinBrickSize());
			Vector3 vector3 = bounds2.max - bounds.min;
			sizeOfValidIndicesAtMaxRes.x = Mathf.CeilToInt(vector3.x / this.MinBrickSize()) - minValidLocalIdxAtMaxRes.x + 1;
			sizeOfValidIndicesAtMaxRes.y = Mathf.CeilToInt(vector3.y / this.MinBrickSize()) - minValidLocalIdxAtMaxRes.y + 1;
			sizeOfValidIndicesAtMaxRes.z = Mathf.CeilToInt(vector3.z / this.MinBrickSize()) - minValidLocalIdxAtMaxRes.z + 1;
			Vector3Int vector3Int = default(Vector3Int);
			vector3Int = sizeOfValidIndicesAtMaxRes / ProbeReferenceVolume.CellSize(minSubdiv);
			return vector3Int.x * vector3Int.y * vector3Int.z;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00014B04 File Offset: 0x00012D04
		private bool GetCellIndexUpdate(ProbeReferenceVolume.Cell cell, out ProbeBrickIndex.CellIndexUpdateInfo cellUpdateInfo, bool ignoreErrorLog)
		{
			cellUpdateInfo = default(ProbeBrickIndex.CellIndexUpdateInfo);
			Vector3Int vector3Int;
			Vector3Int vector3Int2;
			int numberOfBricksAtSubdiv = this.GetNumberOfBricksAtSubdiv(cell.position, cell.minSubdiv, out vector3Int, out vector3Int2);
			cellUpdateInfo.cellPositionInBricksAtMaxRes = cell.position * ProbeReferenceVolume.CellSize(this.m_MaxSubdivision - 1);
			cellUpdateInfo.minSubdivInCell = cell.minSubdiv;
			cellUpdateInfo.minValidBrickIndexForCellAtMaxRes = vector3Int;
			cellUpdateInfo.maxValidBrickIndexForCellAtMaxResPlusOne = vector3Int2 + vector3Int;
			return this.m_Index.AssignIndexChunksToCell(numberOfBricksAtSubdiv, ref cellUpdateInfo, ignoreErrorLog);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00014B7B File Offset: 0x00012D7B
		public void PerformPendingOperations()
		{
			this.PerformPendingDeletion();
			this.PerformPendingIndexChangeAndInit();
			this.PerformPendingLoading();
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00014B90 File Offset: 0x00012D90
		private void InitProbeReferenceVolume(ProbeVolumeTextureMemoryBudget memoryBudget, ProbeVolumeBlendingTextureMemoryBudget blendingMemoryBudget, ProbeVolumeSHBands shBands)
		{
			Vector3Int pendingMinCellPosition = this.m_PendingInitInfo.pendingMinCellPosition;
			Vector3Int pendingMaxCellPosition = this.m_PendingInitInfo.pendingMaxCellPosition;
			if (!this.m_ProbeReferenceVolumeInit)
			{
				this.m_Pool = new ProbeBrickPool(memoryBudget, shBands, true);
				this.m_BlendingPool = new ProbeBrickBlendingPool(blendingMemoryBudget, shBands);
				this.m_Index = new ProbeBrickIndex(memoryBudget);
				this.m_CellIndices = new ProbeCellIndices(pendingMinCellPosition, pendingMaxCellPosition, (int)Mathf.Pow(3f, (float)(this.m_MaxSubdivision - 1)));
				if (this.m_CurrentProbeVolumeChunkSizeInBricks != 0)
				{
					this.AllocateTemporaryDataLocation();
				}
				this.m_PositionOffsets[0] = 0f;
				float num = 0.33333334f;
				for (int i = 1; i < 3; i++)
				{
					this.m_PositionOffsets[i] = (float)i * num;
				}
				this.m_PositionOffsets[this.m_PositionOffsets.Length - 1] = 1f;
				this.m_ProbeReferenceVolumeInit = true;
				this.ClearDebugData();
				this.m_NeedLoadAsset = true;
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00014C6B File Offset: 0x00012E6B
		private void AllocateTemporaryDataLocation()
		{
			this.m_TemporaryDataLocation.Cleanup();
			this.m_TemporaryDataLocation = ProbeBrickPool.CreateDataLocation(this.m_CurrentProbeVolumeChunkSizeInBricks * 64, false, this.m_SHBands, "APV_Intermediate", false, true, out this.m_TemporaryDataLocationMemCost);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00014CA0 File Offset: 0x00012EA0
		private ProbeReferenceVolume()
		{
			this.m_Transform.posWS = Vector3.zero;
			this.m_Transform.rot = Quaternion.identity;
			this.m_Transform.scale = 1f;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00014E88 File Offset: 0x00013088
		public ProbeReferenceVolume.RuntimeResources GetRuntimeResources()
		{
			if (!this.m_ProbeReferenceVolumeInit)
			{
				return default(ProbeReferenceVolume.RuntimeResources);
			}
			ProbeReferenceVolume.RuntimeResources runtimeResources = default(ProbeReferenceVolume.RuntimeResources);
			this.m_Index.GetRuntimeResources(ref runtimeResources);
			this.m_CellIndices.GetRuntimeResources(ref runtimeResources);
			this.m_Pool.GetRuntimeResources(ref runtimeResources);
			return runtimeResources;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00014ED7 File Offset: 0x000130D7
		internal void SetTRS(Vector3 position, Quaternion rotation, float minBrickSize)
		{
			this.m_Transform.posWS = position;
			this.m_Transform.rot = rotation;
			this.m_Transform.scale = minBrickSize;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00014EFD File Offset: 0x000130FD
		internal void SetMaxSubdivision(int maxSubdivision)
		{
			this.m_MaxSubdivision = Math.Min(maxSubdivision, 7);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00014F0C File Offset: 0x0001310C
		internal static int CellSize(int subdivisionLevel)
		{
			return (int)Mathf.Pow(3f, (float)subdivisionLevel);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00014F1B File Offset: 0x0001311B
		internal float BrickSize(int subdivisionLevel)
		{
			return this.m_Transform.scale * (float)ProbeReferenceVolume.CellSize(subdivisionLevel);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00014F30 File Offset: 0x00013130
		internal float MinBrickSize()
		{
			return this.m_Transform.scale;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00014F3D File Offset: 0x0001313D
		internal float MaxBrickSize()
		{
			return this.BrickSize(this.m_MaxSubdivision - 1);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00014F4D File Offset: 0x0001314D
		internal ProbeReferenceVolume.RefVolTransform GetTransform()
		{
			return this.m_Transform;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00014F55 File Offset: 0x00013155
		internal int GetMaxSubdivision()
		{
			return this.m_MaxSubdivision;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00014F5D File Offset: 0x0001315D
		internal int GetMaxSubdivision(float multiplier)
		{
			return Mathf.CeilToInt((float)this.m_MaxSubdivision * multiplier);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00014F6D File Offset: 0x0001316D
		internal float GetDistanceBetweenProbes(int subdivisionLevel)
		{
			return this.BrickSize(subdivisionLevel) / 3f;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00014F7C File Offset: 0x0001317C
		internal float MinDistanceBetweenProbes()
		{
			return this.GetDistanceBetweenProbes(0);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00014F85 File Offset: 0x00013185
		public bool DataHasBeenLoaded()
		{
			return this.m_BricksLoaded;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00014F90 File Offset: 0x00013190
		internal void Clear()
		{
			if (this.m_ProbeReferenceVolumeInit)
			{
				this.UnloadAllCells();
				this.m_Pool.Clear();
				this.m_BlendingPool.Clear();
				this.m_Index.Clear();
				this.cells.Clear();
			}
			if (this.clearAssetsOnVolumeClear)
			{
				this.m_PendingAssetsToBeLoaded.Clear();
				this.m_ActiveAssets.Clear();
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00014FF8 File Offset: 0x000131F8
		private List<ProbeBrickPool.BrickChunkAlloc> GetSourceLocations(int count, int chunkSize, ProbeBrickPool.DataLocation dataLoc)
		{
			ProbeBrickPool.BrickChunkAlloc brickChunkAlloc = default(ProbeBrickPool.BrickChunkAlloc);
			this.m_TmpSrcChunks.Clear();
			this.m_TmpSrcChunks.Add(brickChunkAlloc);
			for (int i = 1; i < count; i++)
			{
				brickChunkAlloc.x += chunkSize * 4;
				if (brickChunkAlloc.x >= dataLoc.width)
				{
					brickChunkAlloc.x = 0;
					brickChunkAlloc.y += 4;
					if (brickChunkAlloc.y >= dataLoc.height)
					{
						brickChunkAlloc.y = 0;
						brickChunkAlloc.z += 4;
					}
				}
				this.m_TmpSrcChunks.Add(brickChunkAlloc);
			}
			return this.m_TmpSrcChunks;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00015094 File Offset: 0x00013294
		private void UpdatePool(List<ProbeBrickPool.BrickChunkAlloc> chunkList, ProbeReferenceVolume.Cell.PerScenarioData data, NativeArray<byte> validityNeighMaskData, int chunkIndex, int poolIndex)
		{
			int num = this.m_CurrentProbeVolumeChunkSizeInBricks * 64;
			int num2 = chunkIndex * num;
			int num3 = num * 4;
			int num4 = chunkIndex * num3;
			(this.m_TemporaryDataLocation.TexL0_L1rx as Texture3D).SetPixelData<ushort>(data.shL0L1RxData.GetSubArray(num4, num3), 0, 0);
			(this.m_TemporaryDataLocation.TexL0_L1rx as Texture3D).Apply(false);
			(this.m_TemporaryDataLocation.TexL1_G_ry as Texture3D).SetPixelData<byte>(data.shL1GL1RyData.GetSubArray(num4, num3), 0, 0);
			(this.m_TemporaryDataLocation.TexL1_G_ry as Texture3D).Apply(false);
			(this.m_TemporaryDataLocation.TexL1_B_rz as Texture3D).SetPixelData<byte>(data.shL1BL1RzData.GetSubArray(num4, num3), 0, 0);
			(this.m_TemporaryDataLocation.TexL1_B_rz as Texture3D).Apply(false);
			if (poolIndex == -1)
			{
				this.m_TemporaryDataLocation.TexValidity.SetPixelData<byte>(validityNeighMaskData.GetSubArray(num2, num), 0, 0);
				this.m_TemporaryDataLocation.TexValidity.Apply(false);
			}
			if (this.m_SHBands == ProbeVolumeSHBands.SphericalHarmonicsL2)
			{
				(this.m_TemporaryDataLocation.TexL2_0 as Texture3D).SetPixelData<byte>(data.shL2Data_0.GetSubArray(num4, num3), 0, 0);
				(this.m_TemporaryDataLocation.TexL2_0 as Texture3D).Apply(false);
				(this.m_TemporaryDataLocation.TexL2_1 as Texture3D).SetPixelData<byte>(data.shL2Data_1.GetSubArray(num4, num3), 0, 0);
				(this.m_TemporaryDataLocation.TexL2_1 as Texture3D).Apply(false);
				(this.m_TemporaryDataLocation.TexL2_2 as Texture3D).SetPixelData<byte>(data.shL2Data_2.GetSubArray(num4, num3), 0, 0);
				(this.m_TemporaryDataLocation.TexL2_2 as Texture3D).Apply(false);
				(this.m_TemporaryDataLocation.TexL2_3 as Texture3D).SetPixelData<byte>(data.shL2Data_3.GetSubArray(num4, num3), 0, 0);
				(this.m_TemporaryDataLocation.TexL2_3 as Texture3D).Apply(false);
			}
			List<ProbeBrickPool.BrickChunkAlloc> sourceLocations = this.GetSourceLocations(1, this.m_CurrentProbeVolumeChunkSizeInBricks, this.m_TemporaryDataLocation);
			if (poolIndex == -1)
			{
				this.m_Pool.Update(this.m_TemporaryDataLocation, sourceLocations, chunkList, chunkIndex, this.m_SHBands);
				return;
			}
			this.m_BlendingPool.Update(this.m_TemporaryDataLocation, sourceLocations, chunkList, chunkIndex, this.m_SHBands, poolIndex);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00015304 File Offset: 0x00013504
		private void UpdatePoolValidity(List<ProbeBrickPool.BrickChunkAlloc> chunkList, ProbeReferenceVolume.Cell.PerScenarioData data, NativeArray<byte> validityNeighMaskData, int chunkIndex)
		{
			int num = this.m_CurrentProbeVolumeChunkSizeInBricks * 64;
			int num2 = chunkIndex * num;
			this.m_TemporaryDataLocation.TexValidity.SetPixelData<byte>(validityNeighMaskData.GetSubArray(num2, num), 0, 0);
			this.m_TemporaryDataLocation.TexValidity.Apply(false);
			List<ProbeBrickPool.BrickChunkAlloc> sourceLocations = this.GetSourceLocations(1, this.m_CurrentProbeVolumeChunkSizeInBricks, this.m_TemporaryDataLocation);
			this.m_Pool.UpdateValidity(this.m_TemporaryDataLocation, sourceLocations, chunkList, chunkIndex);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00015378 File Offset: 0x00013578
		private bool AddBlendingBricks(ProbeReferenceVolume.BlendingCellInfo blendingCell)
		{
			bool flag2;
			using (new ProfilerMarker("AddBlendingBricks").Auto())
			{
				ProbeReferenceVolume.Cell cell = blendingCell.cellInfo.cell;
				bool flag = this.sceneData.otherScenario == null || !cell.hasTwoScenarios;
				if (!flag && !this.m_BlendingPool.Allocate(cell.shChunkCount, blendingCell.chunkList))
				{
					flag2 = false;
				}
				else
				{
					List<ProbeBrickPool.BrickChunkAlloc> list = (flag ? blendingCell.cellInfo.chunkList : blendingCell.chunkList);
					int count = list.Count;
					if (!blendingCell.cellInfo.indexUpdated)
					{
						this.UpdateCellIndex(blendingCell.cellInfo);
						for (int i = 0; i < count; i++)
						{
							this.UpdatePoolValidity(list, cell.scenario0, cell.validityNeighMaskData, i);
						}
					}
					if (flag)
					{
						if (blendingCell.blendingFactor != this.scenarioBlendingFactor)
						{
							for (int j = 0; j < count; j++)
							{
								this.UpdatePool(list, cell.scenario0, cell.validityNeighMaskData, j, -1);
							}
						}
					}
					else
					{
						for (int k = 0; k < count; k++)
						{
							this.UpdatePool(list, cell.scenario0, cell.validityNeighMaskData, k, 0);
							this.UpdatePool(list, cell.scenario1, cell.validityNeighMaskData, k, 1);
						}
					}
					blendingCell.blending = true;
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000154F0 File Offset: 0x000136F0
		private bool AddBricks(ProbeReferenceVolume.CellInfo cellInfo, ProbeBrickIndex.CellIndexUpdateInfo cellUpdateInfo, bool ignoreErrorLog)
		{
			bool flag;
			using (new ProfilerMarker("AddBricks").Auto())
			{
				ProbeReferenceVolume.Cell cell = cellInfo.cell;
				int chunkCount = ProbeBrickPool.GetChunkCount(cell.bricks.Length, this.m_CurrentProbeVolumeChunkSizeInBricks);
				cellInfo.chunkList.Clear();
				if (!this.m_Pool.Allocate(chunkCount, cellInfo.chunkList, ignoreErrorLog))
				{
					flag = false;
				}
				else
				{
					if (this.enableScenarioBlending)
					{
						this.m_ToBeLoadedBlendingCells.Add(in cellInfo.blendingCell);
					}
					cellInfo.tempUpdateInfo = cellUpdateInfo;
					if (!this.enableScenarioBlending || this.scenarioBlendingFactor == 0f || !cell.hasTwoScenarios)
					{
						for (int i = 0; i < cellInfo.chunkList.Count; i++)
						{
							this.UpdatePool(cellInfo.chunkList, cell.scenario0, cell.validityNeighMaskData, i, -1);
						}
						this.UpdateCellIndex(cellInfo);
						cellInfo.blendingCell.blendingFactor = 0f;
					}
					else if (this.enableScenarioBlending)
					{
						cellInfo.blendingCell.Prioritize();
						this.m_HasRemainingCellsToBlend = true;
						cellInfo.indexUpdated = false;
					}
					cellInfo.loaded = true;
					this.ClearDebugData();
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00015648 File Offset: 0x00013848
		private void UpdateCellIndex(ProbeReferenceVolume.CellInfo cellInfo)
		{
			cellInfo.indexUpdated = true;
			this.m_BricksLoaded = true;
			NativeArray<ProbeBrickIndex.Brick> bricks = cellInfo.cell.bricks;
			ProbeBrickIndex.CellIndexUpdateInfo tempUpdateInfo = cellInfo.tempUpdateInfo;
			this.m_Index.AddBricks(cellInfo.cell, bricks, cellInfo.chunkList, this.m_CurrentProbeVolumeChunkSizeInBricks, this.m_Pool.GetPoolWidth(), this.m_Pool.GetPoolHeight(), tempUpdateInfo);
			cellInfo.updateInfo = tempUpdateInfo;
			this.m_CellIndices.UpdateCell(cellInfo.flatIdxInCellIndices, tempUpdateInfo);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000156C4 File Offset: 0x000138C4
		private void ReleaseBricks(ProbeReferenceVolume.CellInfo cellInfo)
		{
			if (cellInfo.chunkList.Count == 0)
			{
				Debug.Log("Tried to release bricks from an empty Cell.");
				return;
			}
			this.m_Index.RemoveBricks(cellInfo);
			this.m_Pool.Deallocate(cellInfo.chunkList);
			cellInfo.chunkList.Clear();
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00015714 File Offset: 0x00013914
		public void UpdateConstantBuffer(CommandBuffer cmd, ProbeVolumeShadingParameters parameters)
		{
			float num = parameters.normalBias;
			float num2 = parameters.viewBias;
			if (parameters.scaleBiasByMinDistanceBetweenProbes)
			{
				num *= this.MinDistanceBetweenProbes();
				num2 *= this.MinDistanceBetweenProbes();
			}
			Vector3Int cellMinPosition = this.m_CellIndices.GetCellMinPosition();
			Vector3Int cellIndexDimension = this.m_CellIndices.GetCellIndexDimension();
			Vector3Int poolDimensions = this.m_Pool.GetPoolDimensions();
			ShaderVariablesProbeVolumes shaderVariablesProbeVolumes;
			shaderVariablesProbeVolumes._Biases_CellInMinBrick_MinBrickSize = new Vector4(num, num2, (float)((int)Mathf.Pow(3f, (float)(this.m_MaxSubdivision - 1))), this.MinBrickSize());
			shaderVariablesProbeVolumes._IndicesDim_IndexChunkSize = new Vector4((float)cellIndexDimension.x, (float)cellIndexDimension.y, (float)cellIndexDimension.z, 243f);
			shaderVariablesProbeVolumes._MinCellPos_Noise = new Vector4((float)cellMinPosition.x, (float)cellMinPosition.y, (float)cellMinPosition.z, parameters.samplingNoise);
			shaderVariablesProbeVolumes._PoolDim_CellInMeters = new Vector4((float)poolDimensions.x, (float)poolDimensions.y, (float)poolDimensions.z, this.MaxBrickSize());
			shaderVariablesProbeVolumes._Weight_MinLoadedCell = new Vector4(parameters.weight, (float)this.minLoadedCellPos.x, (float)this.minLoadedCellPos.y, (float)this.minLoadedCellPos.z);
			shaderVariablesProbeVolumes._MaxLoadedCell_FrameIndex = new Vector4((float)this.maxLoadedCellPos.x, (float)this.maxLoadedCellPos.y, (float)this.maxLoadedCellPos.z, (float)parameters.frameIndexForNoise);
			shaderVariablesProbeVolumes._LeakReductionParams = new Vector4((float)parameters.leakReductionMode, parameters.occlusionWeightContribution, parameters.minValidNormalWeight, 0f);
			shaderVariablesProbeVolumes._NormalizationClamp_Padding12 = new Vector4(parameters.reflNormalizationLowerClamp, parameters.reflNormalizationUpperClamp, 0f, 0f);
			ConstantBuffer.PushGlobal<ShaderVariablesProbeVolumes>(cmd, in shaderVariablesProbeVolumes, this.m_CBShaderID);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000158D8 File Offset: 0x00013AD8
		private void CleanupLoadedData()
		{
			this.m_BricksLoaded = false;
			this.UnloadAllCells();
			if (this.m_ProbeReferenceVolumeInit)
			{
				this.m_Index.Cleanup();
				this.m_CellIndices.Cleanup();
				this.m_Pool.Cleanup();
				this.m_BlendingPool.Cleanup();
				this.m_TemporaryDataLocation.Cleanup();
			}
			this.m_ProbeReferenceVolumeInit = false;
			this.ClearDebugData();
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0001593E File Offset: 0x00013B3E
		internal ProbeVolumeDebug probeVolumeDebug { get; } = new ProbeVolumeDebug();

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00015946 File Offset: 0x00013B46
		public Color[] subdivisionDebugColors { get; } = new Color[7];

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001594E File Offset: 0x00013B4E
		public void RenderDebug(Camera camera)
		{
			if (camera.cameraType != CameraType.Reflection && camera.cameraType != CameraType.Preview)
			{
				this.DrawProbeDebug(camera);
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001596C File Offset: 0x00013B6C
		private void InitializeDebug(in ProbeVolumeSystemParameters parameters)
		{
			if (parameters.supportsRuntimeDebug)
			{
				this.m_DebugMesh = parameters.probeDebugMesh;
				this.m_DebugMaterial = CoreUtils.CreateEngineMaterial(parameters.probeDebugShader);
				this.m_DebugMaterial.enableInstancing = true;
				this.m_DebugOffsetMesh = parameters.offsetDebugMesh;
				this.m_DebugOffsetMaterial = CoreUtils.CreateEngineMaterial(parameters.offsetDebugShader);
				this.m_DebugOffsetMaterial.enableInstancing = true;
				this.subdivisionDebugColors[0] = new Color(1f, 0f, 0f);
				this.subdivisionDebugColors[1] = new Color(0f, 1f, 0f);
				this.subdivisionDebugColors[2] = new Color(0f, 0f, 1f);
				this.subdivisionDebugColors[3] = new Color(1f, 1f, 0f);
				this.subdivisionDebugColors[4] = new Color(1f, 0f, 1f);
				this.subdivisionDebugColors[5] = new Color(0f, 1f, 1f);
				this.subdivisionDebugColors[6] = new Color(0.5f, 0.5f, 0.5f);
			}
			this.RegisterDebug(parameters);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00015AC2 File Offset: 0x00013CC2
		private void CleanupDebug()
		{
			this.UnregisterDebug(true);
			CoreUtils.Destroy(this.m_DebugMaterial);
			CoreUtils.Destroy(this.m_DebugOffsetMaterial);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00015AE1 File Offset: 0x00013CE1
		private void DebugCellIndexChanged<T>(DebugUI.Field<T> field, T value)
		{
			this.ClearDebugData();
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00015AEC File Offset: 0x00013CEC
		private void RegisterDebug(ProbeVolumeSystemParameters parameters)
		{
			ProbeReferenceVolume.<>c__DisplayClass171_0 CS$<>8__locals1 = new ProbeReferenceVolume.<>c__DisplayClass171_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.parameters = parameters;
			List<DebugUI.Widget> list = new List<DebugUI.Widget>();
			DebugUI.Container container = new DebugUI.Container
			{
				displayName = "Subdivision Visualization"
			};
			container.children.Add(new DebugUI.BoolField
			{
				displayName = "Display Cells",
				getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.drawCells,
				setter = delegate(bool value)
				{
					CS$<>8__locals1.<>4__this.probeVolumeDebug.drawCells = value;
				},
				onValueChanged = new Action<DebugUI.Field<bool>, bool>(CS$<>8__locals1.<RegisterDebug>g__RefreshDebug|0<bool>)
			});
			container.children.Add(new DebugUI.BoolField
			{
				displayName = "Display Bricks",
				getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.drawBricks,
				setter = delegate(bool value)
				{
					CS$<>8__locals1.<>4__this.probeVolumeDebug.drawBricks = value;
				},
				onValueChanged = new Action<DebugUI.Field<bool>, bool>(CS$<>8__locals1.<RegisterDebug>g__RefreshDebug|0<bool>)
			});
			ObservableList<DebugUI.Widget> children = container.children;
			DebugUI.FloatField floatField = new DebugUI.FloatField();
			floatField.displayName = "Culling Distance";
			floatField.getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.subdivisionViewCullingDistance;
			floatField.setter = delegate(float value)
			{
				CS$<>8__locals1.<>4__this.probeVolumeDebug.subdivisionViewCullingDistance = value;
			};
			floatField.min = () => 0f;
			children.Add(floatField);
			DebugUI.Container container2 = new DebugUI.Container
			{
				displayName = "Probe Visualization"
			};
			container2.children.Add(new DebugUI.BoolField
			{
				displayName = "Display Probes",
				getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.drawProbes,
				setter = delegate(bool value)
				{
					CS$<>8__locals1.<>4__this.probeVolumeDebug.drawProbes = value;
				},
				onValueChanged = new Action<DebugUI.Field<bool>, bool>(CS$<>8__locals1.<RegisterDebug>g__RefreshDebug|0<bool>)
			});
			if (this.probeVolumeDebug.drawProbes)
			{
				DebugUI.Container container3 = new DebugUI.Container();
				container3.children.Add(new DebugUI.EnumField
				{
					displayName = "Probe Shading Mode",
					getter = () => (int)CS$<>8__locals1.<>4__this.probeVolumeDebug.probeShading,
					setter = delegate(int value)
					{
						CS$<>8__locals1.<>4__this.probeVolumeDebug.probeShading = (DebugProbeShadingMode)value;
					},
					autoEnum = typeof(DebugProbeShadingMode),
					getIndex = () => (int)CS$<>8__locals1.<>4__this.probeVolumeDebug.probeShading,
					setIndex = delegate(int value)
					{
						CS$<>8__locals1.<>4__this.probeVolumeDebug.probeShading = (DebugProbeShadingMode)value;
					},
					onValueChanged = new Action<DebugUI.Field<int>, int>(CS$<>8__locals1.<RegisterDebug>g__RefreshDebug|0<int>)
				});
				ObservableList<DebugUI.Widget> children2 = container3.children;
				DebugUI.FloatField floatField2 = new DebugUI.FloatField();
				floatField2.displayName = "Probe Size";
				floatField2.getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.probeSize;
				floatField2.setter = delegate(float value)
				{
					CS$<>8__locals1.<>4__this.probeVolumeDebug.probeSize = value;
				};
				floatField2.min = () => 0.05f;
				floatField2.max = () => 10f;
				children2.Add(floatField2);
				if (this.probeVolumeDebug.probeShading == DebugProbeShadingMode.SH || this.probeVolumeDebug.probeShading == DebugProbeShadingMode.SHL0 || this.probeVolumeDebug.probeShading == DebugProbeShadingMode.SHL0L1)
				{
					container3.children.Add(new DebugUI.FloatField
					{
						displayName = "Probe Exposure Compensation",
						getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.exposureCompensation,
						setter = delegate(float value)
						{
							CS$<>8__locals1.<>4__this.probeVolumeDebug.exposureCompensation = value;
						}
					});
				}
				ObservableList<DebugUI.Widget> children3 = container3.children;
				DebugUI.IntField intField = new DebugUI.IntField();
				intField.displayName = "Max subdivision displayed";
				intField.getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.maxSubdivToVisualize;
				intField.setter = delegate(int v)
				{
					CS$<>8__locals1.<>4__this.probeVolumeDebug.maxSubdivToVisualize = Mathf.Min(v, ProbeReferenceVolume.instance.GetMaxSubdivision() - 1);
				};
				intField.min = () => 0;
				intField.max = () => ProbeReferenceVolume.instance.GetMaxSubdivision() - 1;
				children3.Add(intField);
				ObservableList<DebugUI.Widget> children4 = container3.children;
				DebugUI.IntField intField2 = new DebugUI.IntField();
				intField2.displayName = "Min subdivision displayed";
				intField2.getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.minSubdivToVisualize;
				intField2.setter = delegate(int v)
				{
					CS$<>8__locals1.<>4__this.probeVolumeDebug.minSubdivToVisualize = Mathf.Max(v, 0);
				};
				intField2.min = () => 0;
				intField2.max = () => ProbeReferenceVolume.instance.GetMaxSubdivision() - 1;
				children4.Add(intField2);
				container2.children.Add(container3);
			}
			container2.children.Add(new DebugUI.BoolField
			{
				displayName = "Virtual Offset",
				getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.drawVirtualOffsetPush,
				setter = delegate(bool value)
				{
					CS$<>8__locals1.<>4__this.probeVolumeDebug.drawVirtualOffsetPush = value;
					if (CS$<>8__locals1.<>4__this.probeVolumeDebug.drawVirtualOffsetPush && CS$<>8__locals1.<>4__this.probeVolumeDebug.drawProbes)
					{
						float num = (float)ProbeReferenceVolume.CellSize(0) * CS$<>8__locals1.<>4__this.MinBrickSize() / 3f * CS$<>8__locals1.<>4__this.bakingProcessSettings.virtualOffsetSettings.searchMultiplier + CS$<>8__locals1.<>4__this.bakingProcessSettings.virtualOffsetSettings.outOfGeoOffset;
						CS$<>8__locals1.<>4__this.probeVolumeDebug.probeSize = Mathf.Min(CS$<>8__locals1.<>4__this.probeVolumeDebug.probeSize, Mathf.Clamp(num, 0.05f, 10f));
					}
				},
				onValueChanged = new Action<DebugUI.Field<bool>, bool>(CS$<>8__locals1.<RegisterDebug>g__RefreshDebug|0<bool>)
			});
			if (this.probeVolumeDebug.drawVirtualOffsetPush)
			{
				DebugUI.FloatField floatField3 = new DebugUI.FloatField();
				floatField3.displayName = "Offset Size";
				floatField3.getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.offsetSize;
				floatField3.setter = delegate(float value)
				{
					CS$<>8__locals1.<>4__this.probeVolumeDebug.offsetSize = value;
				};
				floatField3.min = () => 0.001f;
				floatField3.max = () => 0.1f;
				DebugUI.FloatField floatField4 = floatField3;
				container2.children.Add(new DebugUI.Container
				{
					children = { floatField4 }
				});
			}
			ObservableList<DebugUI.Widget> children5 = container2.children;
			DebugUI.FloatField floatField5 = new DebugUI.FloatField();
			floatField5.displayName = "Culling Distance";
			floatField5.getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.probeCullingDistance;
			floatField5.setter = delegate(float value)
			{
				CS$<>8__locals1.<>4__this.probeVolumeDebug.probeCullingDistance = value;
			};
			floatField5.min = () => 0f;
			children5.Add(floatField5);
			DebugUI.Container container4 = new DebugUI.Container
			{
				displayName = "Streaming"
			};
			container4.children.Add(new DebugUI.BoolField
			{
				displayName = "Freeze Streaming",
				getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.freezeStreaming,
				setter = delegate(bool value)
				{
					CS$<>8__locals1.<>4__this.probeVolumeDebug.freezeStreaming = value;
				}
			});
			ObservableList<DebugUI.Widget> children6 = container4.children;
			DebugUI.IntField intField3 = new DebugUI.IntField();
			intField3.displayName = "Number Of Cells Loaded Per Frame";
			intField3.getter = () => ProbeReferenceVolume.instance.numberOfCellsLoadedPerFrame;
			intField3.setter = delegate(int value)
			{
				ProbeReferenceVolume.instance.SetNumberOfCellsLoadedPerFrame(value);
			};
			intField3.min = () => 0;
			children6.Add(intField3);
			if (CS$<>8__locals1.parameters.supportsRuntimeDebug)
			{
				if (Application.isEditor)
				{
					list.Add(container);
				}
				list.Add(container2);
			}
			if (CS$<>8__locals1.parameters.supportStreaming)
			{
				list.Add(container4);
			}
			if (CS$<>8__locals1.parameters.scenarioBlendingShader != null && CS$<>8__locals1.parameters.blendingMemoryBudget != ProbeVolumeBlendingTextureMemoryBudget.None)
			{
				DebugUI.Container container5 = new DebugUI.Container
				{
					displayName = "Scenario Blending"
				};
				ObservableList<DebugUI.Widget> children7 = container5.children;
				DebugUI.IntField intField4 = new DebugUI.IntField();
				intField4.displayName = "Number Of Cells Blended Per Frame";
				intField4.getter = () => ProbeReferenceVolume.instance.numberOfCellsBlendedPerFrame;
				intField4.setter = delegate(int value)
				{
					ProbeReferenceVolume.instance.numberOfCellsBlendedPerFrame = value;
				};
				intField4.min = () => 0;
				children7.Add(intField4);
				ObservableList<DebugUI.Widget> children8 = container5.children;
				DebugUI.FloatField floatField6 = new DebugUI.FloatField();
				floatField6.displayName = "Turnover Rate";
				floatField6.getter = () => ProbeReferenceVolume.instance.turnoverRate;
				floatField6.setter = delegate(float value)
				{
					ProbeReferenceVolume.instance.turnoverRate = value;
				};
				floatField6.min = () => 0f;
				floatField6.max = () => 1f;
				children8.Add(floatField6);
				this.m_DebugScenarioField = new DebugUI.EnumField
				{
					displayName = "Scenario To Blend With",
					enumNames = this.m_DebugScenarioNames,
					enumValues = this.m_DebugScenarioValues,
					getIndex = delegate
					{
						base.<RegisterDebug>g__RefreshScenarioNames|42(ProbeVolumeSceneData.GetSceneGUID(SceneManager.GetActiveScene()));
						CS$<>8__locals1.<>4__this.probeVolumeDebug.otherStateIndex = 0;
						if (!string.IsNullOrEmpty(CS$<>8__locals1.<>4__this.sceneData.otherScenario))
						{
							for (int i = 1; i < CS$<>8__locals1.<>4__this.m_DebugScenarioNames.Length; i++)
							{
								if (CS$<>8__locals1.<>4__this.m_DebugScenarioNames[i].text == CS$<>8__locals1.<>4__this.sceneData.otherScenario)
								{
									CS$<>8__locals1.<>4__this.probeVolumeDebug.otherStateIndex = i;
									break;
								}
							}
						}
						return CS$<>8__locals1.<>4__this.probeVolumeDebug.otherStateIndex;
					},
					setIndex = delegate(int value)
					{
						string text = ((value == 0) ? null : CS$<>8__locals1.<>4__this.m_DebugScenarioNames[value].text);
						CS$<>8__locals1.<>4__this.sceneData.BlendLightingScenario(text, CS$<>8__locals1.<>4__this.sceneData.scenarioBlendingFactor);
						CS$<>8__locals1.<>4__this.probeVolumeDebug.otherStateIndex = value;
					},
					getter = () => CS$<>8__locals1.<>4__this.probeVolumeDebug.otherStateIndex,
					setter = delegate(int value)
					{
						CS$<>8__locals1.<>4__this.probeVolumeDebug.otherStateIndex = value;
					}
				};
				container5.children.Add(this.m_DebugScenarioField);
				ObservableList<DebugUI.Widget> children9 = container5.children;
				DebugUI.FloatField floatField7 = new DebugUI.FloatField();
				floatField7.displayName = "Scenario Blending Factor";
				floatField7.getter = () => ProbeReferenceVolume.instance.scenarioBlendingFactor;
				floatField7.setter = delegate(float value)
				{
					ProbeReferenceVolume.instance.scenarioBlendingFactor = value;
				};
				floatField7.min = () => 0f;
				floatField7.max = () => 1f;
				children9.Add(floatField7);
				list.Add(container5);
			}
			if (list.Count > 0)
			{
				this.m_DebugItems = list.ToArray();
				DebugManager.instance.GetPanel(ProbeReferenceVolume.k_DebugPanelName, true, 0, false).children.Add(this.m_DebugItems);
			}
			DebugManager.instance.RegisterData(this.probeVolumeDebug);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000164AD File Offset: 0x000146AD
		private void UnregisterDebug(bool destroyPanel)
		{
			if (destroyPanel)
			{
				DebugManager.instance.RemovePanel(ProbeReferenceVolume.k_DebugPanelName);
				return;
			}
			DebugManager.instance.GetPanel(ProbeReferenceVolume.k_DebugPanelName, false, 0, false).children.Remove(this.m_DebugItems);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x000164E8 File Offset: 0x000146E8
		private bool ShouldCullCell(Vector3 cellPosition, Transform cameraTransform, Plane[] frustumPlanes)
		{
			float num = this.MaxBrickSize();
			Vector3 posWS = this.GetTransform().posWS;
			Vector3 vector = cellPosition * num + posWS + Vector3.one * (num / 2f);
			float num2 = (float)Mathf.CeilToInt(this.probeVolumeDebug.probeCullingDistance / num) * num;
			if (Vector3.Distance(cameraTransform.position, vector) > num2)
			{
				return true;
			}
			Bounds bounds = new Bounds(vector, num * Vector3.one);
			return !GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00016574 File Offset: 0x00014774
		private void DrawProbeDebug(Camera camera)
		{
			if (!this.enabledBySRP || !this.isInitialized)
			{
				return;
			}
			if (!this.probeVolumeDebug.drawProbes && !this.probeVolumeDebug.drawVirtualOffsetPush)
			{
				return;
			}
			GeometryUtility.CalculateFrustumPlanes(camera, this.m_DebugFrustumPlanes);
			this.m_DebugMaterial.shaderKeywords = null;
			if (this.m_SHBands == ProbeVolumeSHBands.SphericalHarmonicsL1)
			{
				this.m_DebugMaterial.EnableKeyword("PROBE_VOLUMES_L1");
			}
			else if (this.m_SHBands == ProbeVolumeSHBands.SphericalHarmonicsL2)
			{
				this.m_DebugMaterial.EnableKeyword("PROBE_VOLUMES_L2");
			}
			this.m_DebugMaterial.renderQueue = 3000;
			this.m_DebugOffsetMaterial.renderQueue = 3000;
			int num = ((ProbeReferenceVolume.instance.cells.Count > 0) ? (ProbeReferenceVolume.instance.GetMaxSubdivision() - 1) : 0);
			foreach (ProbeReferenceVolume.CellInfo cellInfo in ProbeReferenceVolume.instance.cells.Values)
			{
				num = Mathf.Min(num, cellInfo.cell.minSubdiv);
			}
			this.probeVolumeDebug.maxSubdivToVisualize = Mathf.Min(this.probeVolumeDebug.maxSubdivToVisualize, ProbeReferenceVolume.instance.GetMaxSubdivision() - 1);
			this.m_MaxSubdivVisualizedIsMaxAvailable = this.probeVolumeDebug.maxSubdivToVisualize == ProbeReferenceVolume.instance.GetMaxSubdivision() - 1;
			this.probeVolumeDebug.minSubdivToVisualize = Mathf.Clamp(this.probeVolumeDebug.minSubdivToVisualize, num, this.probeVolumeDebug.maxSubdivToVisualize);
			foreach (ProbeReferenceVolume.CellInfo cellInfo2 in ProbeReferenceVolume.instance.cells.Values)
			{
				if (!this.ShouldCullCell(cellInfo2.cell.position, camera.transform, this.m_DebugFrustumPlanes))
				{
					ProbeReferenceVolume.CellInstancedDebugProbes cellInstancedDebugProbes = this.CreateInstancedProbes(cellInfo2);
					if (cellInstancedDebugProbes != null)
					{
						for (int i = 0; i < cellInstancedDebugProbes.probeBuffers.Count; i++)
						{
							MaterialPropertyBlock materialPropertyBlock = cellInstancedDebugProbes.props[i];
							materialPropertyBlock.SetInt("_ShadingMode", (int)this.probeVolumeDebug.probeShading);
							materialPropertyBlock.SetFloat("_ExposureCompensation", this.probeVolumeDebug.exposureCompensation);
							materialPropertyBlock.SetFloat("_ProbeSize", this.probeVolumeDebug.probeSize);
							materialPropertyBlock.SetFloat("_CullDistance", this.probeVolumeDebug.probeCullingDistance);
							materialPropertyBlock.SetInt("_MaxAllowedSubdiv", this.probeVolumeDebug.maxSubdivToVisualize);
							materialPropertyBlock.SetInt("_MinAllowedSubdiv", this.probeVolumeDebug.minSubdivToVisualize);
							materialPropertyBlock.SetFloat("_ValidityThreshold", this.bakingProcessSettings.dilationSettings.dilationValidityThreshold);
							materialPropertyBlock.SetFloat("_OffsetSize", this.probeVolumeDebug.offsetSize);
							if (this.probeVolumeDebug.drawProbes)
							{
								Matrix4x4[] array = cellInstancedDebugProbes.probeBuffers[i];
								Graphics.DrawMeshInstanced(this.m_DebugMesh, 0, this.m_DebugMaterial, array, array.Length, materialPropertyBlock, ShadowCastingMode.Off, false, 0, camera, LightProbeUsage.Off, null);
							}
							if (this.probeVolumeDebug.drawVirtualOffsetPush)
							{
								Matrix4x4[] array2 = cellInstancedDebugProbes.offsetBuffers[i];
								Graphics.DrawMeshInstanced(this.m_DebugOffsetMesh, 0, this.m_DebugOffsetMaterial, array2, array2.Length, materialPropertyBlock, ShadowCastingMode.Off, false, 0, camera, LightProbeUsage.Off, null);
							}
						}
					}
				}
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00016904 File Offset: 0x00014B04
		internal void ResetDebugViewToMaxSubdiv()
		{
			if (this.m_MaxSubdivVisualizedIsMaxAvailable)
			{
				this.probeVolumeDebug.maxSubdivToVisualize = ProbeReferenceVolume.instance.GetMaxSubdivision() - 1;
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00016925 File Offset: 0x00014B25
		private void ClearDebugData()
		{
			this.realtimeSubdivisionInfo.Clear();
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00016934 File Offset: 0x00014B34
		private ProbeReferenceVolume.CellInstancedDebugProbes CreateInstancedProbes(ProbeReferenceVolume.CellInfo cellInfo)
		{
			if (cellInfo.debugProbes != null)
			{
				return cellInfo.debugProbes;
			}
			int num = ProbeReferenceVolume.instance.GetMaxSubdivision() - 1;
			ProbeReferenceVolume.Cell cell = cellInfo.cell;
			if (!cell.bricks.IsCreated || cell.bricks.Length == 0 || !cellInfo.loaded)
			{
				return null;
			}
			List<Matrix4x4[]> list = new List<Matrix4x4[]>();
			List<Matrix4x4[]> list2 = new List<Matrix4x4[]>();
			List<MaterialPropertyBlock> list3 = new List<MaterialPropertyBlock>();
			List<ProbeBrickPool.BrickChunkAlloc> chunkList = cellInfo.chunkList;
			Vector4[] array = new Vector4[511];
			float[] array2 = new float[511];
			float[] array3 = new float[511];
			float[] array4 = ((cell.touchupVolumeInteraction.Length > 0) ? new float[511] : null);
			Vector4[] array5 = ((cell.offsetVectors.Length > 0) ? new Vector4[511] : null);
			List<Matrix4x4> list4 = new List<Matrix4x4>();
			List<Matrix4x4> list5 = new List<Matrix4x4>();
			ProbeReferenceVolume.CellInstancedDebugProbes cellInstancedDebugProbes = new ProbeReferenceVolume.CellInstancedDebugProbes();
			cellInstancedDebugProbes.probeBuffers = list;
			cellInstancedDebugProbes.offsetBuffers = list2;
			cellInstancedDebugProbes.props = list3;
			int num2 = this.m_CurrentProbeVolumeChunkSizeInBricks * 64;
			Vector3Int vector3Int = ProbeBrickPool.ProbeCountToDataLocSize(num2);
			int num3 = 0;
			int num4 = 0;
			int num5 = cell.probeCount / 64;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			for (int i = 0; i < num5; i++)
			{
				int subdivisionLevel = cell.bricks[i].subdivisionLevel;
				int num9 = i / this.m_CurrentProbeVolumeChunkSizeInBricks;
				ProbeBrickPool.BrickChunkAlloc brickChunkAlloc = chunkList[num9];
				Vector3Int vector3Int2 = new Vector3Int(brickChunkAlloc.x + num6, brickChunkAlloc.y + num7, brickChunkAlloc.z + num8);
				for (int j = 0; j < 4; j++)
				{
					for (int k = 0; k < 4; k++)
					{
						for (int l = 0; l < 4; l++)
						{
							Vector3Int vector3Int3 = new Vector3Int(vector3Int2.x + l, vector3Int2.y + k, vector3Int2.z + j);
							int num10 = num9 * num2 + (num6 + l) + vector3Int.x * (num7 + k + vector3Int.y * (num8 + j));
							list4.Add(Matrix4x4.TRS(cell.probePositions[num10], Quaternion.identity, Vector3.one * (0.3f * (float)(subdivisionLevel + 1))));
							array2[num3] = cell.validity[num10];
							array[num3] = new Vector4((float)vector3Int3.x, (float)vector3Int3.y, (float)vector3Int3.z, (float)subdivisionLevel);
							array3[num3] = (float)subdivisionLevel / (float)num;
							if (array4 != null)
							{
								array4[num3] = cell.touchupVolumeInteraction[num10];
							}
							if (array5 != null)
							{
								Vector3 vector = cell.offsetVectors[num10];
								array5[num3] = vector;
								if (vector.sqrMagnitude < 1E-06f)
								{
									list5.Add(Matrix4x4.identity);
								}
								else
								{
									Vector3 vector2 = cell.probePositions[num10] + vector;
									Quaternion quaternion = Quaternion.LookRotation(-vector);
									Vector3 vector3 = new Vector3(0.5f, 0.5f, vector.magnitude);
									list5.Add(Matrix4x4.TRS(vector2, quaternion, vector3));
								}
							}
							num3++;
							if (list4.Count >= 511 || num4 == cell.probeCount - 1)
							{
								num3 = 0;
								MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
								materialPropertyBlock.SetFloatArray("_Validity", array2);
								materialPropertyBlock.SetFloatArray("_TouchupedByVolume", array4);
								materialPropertyBlock.SetFloatArray("_RelativeSize", array3);
								materialPropertyBlock.SetVectorArray("_IndexInAtlas", array);
								if (array5 != null)
								{
									materialPropertyBlock.SetVectorArray("_Offset", array5);
								}
								list3.Add(materialPropertyBlock);
								list.Add(list4.ToArray());
								list4 = new List<Matrix4x4>();
								list4.Clear();
								list2.Add(list5.ToArray());
								list5.Clear();
							}
							num4++;
						}
					}
				}
				num6 += 4;
				if (num6 >= vector3Int.x)
				{
					num6 = 0;
					num7 += 4;
					if (num7 >= vector3Int.y)
					{
						num7 = 0;
						num8 += 4;
						if (num8 >= vector3Int.z)
						{
							num6 = 0;
							num7 = 0;
							num8 = 0;
						}
					}
				}
			}
			cellInfo.debugProbes = cellInstancedDebugProbes;
			return cellInstancedDebugProbes;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00016D9B File Offset: 0x00014F9B
		private void OnClearLightingdata()
		{
			this.ClearDebugData();
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00016DA4 File Offset: 0x00014FA4
		internal unsafe void ScenarioBlendingChanged(bool scenarioChanged)
		{
			this.m_HasRemainingCellsToBlend = true;
			if (scenarioChanged)
			{
				this.UnloadAllBlendingCells();
				for (int i = 0; i < this.m_ToBeLoadedBlendingCells.size; i++)
				{
					this.m_ToBeLoadedBlendingCells[i]->ForceReupload();
				}
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00016DE9 File Offset: 0x00014FE9
		public void SetNumberOfCellsLoadedPerFrame(int numberOfCells)
		{
			this.m_NumberOfCellsLoadedPerFrame = Mathf.Max(1, numberOfCells);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00016DF8 File Offset: 0x00014FF8
		private unsafe void ComputeCellCameraDistance(Vector3 cameraPosition, DynamicArray<ProbeReferenceVolume.CellInfo> cells)
		{
			for (int i = 0; i < cells.size; i++)
			{
				ProbeReferenceVolume.CellInfo cellInfo = *cells[i];
				cellInfo.streamingScore = Vector3.Distance(cameraPosition, cellInfo.cell.position);
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00016E3C File Offset: 0x0001503C
		private unsafe void ComputeStreamingScoreForBlending(DynamicArray<ProbeReferenceVolume.BlendingCellInfo> cells, float worstScore)
		{
			float scenarioBlendingFactor = this.scenarioBlendingFactor;
			for (int i = 0; i < cells.size; i++)
			{
				ProbeReferenceVolume.BlendingCellInfo blendingCellInfo = *cells[i];
				if (scenarioBlendingFactor == blendingCellInfo.blendingFactor)
				{
					blendingCellInfo.MarkUpToDate();
				}
				else
				{
					blendingCellInfo.streamingScore = blendingCellInfo.cellInfo.streamingScore;
					if (blendingCellInfo.ShouldPrioritize())
					{
						blendingCellInfo.streamingScore -= worstScore;
					}
				}
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00016EA4 File Offset: 0x000150A4
		private bool TryLoadCell(ProbeReferenceVolume.CellInfo cellInfo, ref int shBudget, ref int indexBudget, DynamicArray<ProbeReferenceVolume.CellInfo> loadedCells)
		{
			if (cellInfo.cell.shChunkCount <= shBudget && cellInfo.cell.indexChunkCount <= indexBudget && this.LoadCell(cellInfo, false))
			{
				loadedCells.Add(in cellInfo);
				shBudget -= cellInfo.cell.shChunkCount;
				indexBudget -= cellInfo.cell.indexChunkCount;
				return true;
			}
			return false;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00016F06 File Offset: 0x00015106
		private void UnloadBlendingCell(ProbeReferenceVolume.BlendingCellInfo blendingCell, DynamicArray<ProbeReferenceVolume.BlendingCellInfo> unloadedCells)
		{
			this.UnloadBlendingCell(blendingCell);
			unloadedCells.Add(in blendingCell);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00016F18 File Offset: 0x00015118
		private bool TryLoadBlendingCell(ProbeReferenceVolume.BlendingCellInfo blendingCell, DynamicArray<ProbeReferenceVolume.BlendingCellInfo> loadedCells)
		{
			if (!this.AddBlendingBricks(blendingCell))
			{
				return false;
			}
			loadedCells.Add(in blendingCell);
			return true;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00016F30 File Offset: 0x00015130
		public unsafe void UpdateCellStreaming(CommandBuffer cmd, Camera camera)
		{
			if (!this.isInitialized)
			{
				return;
			}
			using (new ProfilingScope(null, ProfilingSampler.Get<CoreProfileId>(CoreProfileId.APVCellStreamingUpdate)))
			{
				Vector3 position = camera.transform.position;
				if (!this.probeVolumeDebug.freezeStreaming)
				{
					this.m_FrozenCameraPosition = position;
				}
				Vector3 vector = (this.m_FrozenCameraPosition - this.m_Transform.posWS) / this.MaxBrickSize() - Vector3.one * 0.5f;
				this.ComputeCellCameraDistance(vector, this.m_ToBeLoadedCells);
				this.ComputeCellCameraDistance(vector, this.m_LoadedCells);
				this.m_ToBeLoadedCells.QuickSort<ProbeReferenceVolume.CellInfo>();
				this.m_LoadedCells.QuickSort<ProbeReferenceVolume.CellInfo>();
				int num = this.m_Index.GetRemainingChunkCount();
				int num2 = this.m_Pool.GetRemainingChunkCount();
				int num3 = Mathf.Min(this.m_NumberOfCellsLoadedPerFrame, this.m_ToBeLoadedCells.size);
				if (this.m_SupportStreaming)
				{
					while (this.m_TempCellToLoadList.size < num3)
					{
						ProbeReferenceVolume.CellInfo cellInfo = *this.m_ToBeLoadedCells[this.m_TempCellToLoadList.size];
						if (!this.TryLoadCell(cellInfo, ref num2, ref num, this.m_TempCellToLoadList))
						{
							break;
						}
					}
					if (this.m_TempCellToLoadList.size != num3)
					{
						int num4 = 0;
						while (this.m_TempCellToLoadList.size < num3 && this.m_LoadedCells.size - num4 != 0)
						{
							ProbeReferenceVolume.CellInfo cellInfo2 = *this.m_LoadedCells[this.m_LoadedCells.size - num4 - 1];
							ProbeReferenceVolume.CellInfo cellInfo3 = *this.m_ToBeLoadedCells[this.m_TempCellToLoadList.size];
							if (cellInfo2.streamingScore <= cellInfo3.streamingScore)
							{
								break;
							}
							num4++;
							this.UnloadCell(cellInfo2);
							num2 += cellInfo2.cell.shChunkCount;
							num += cellInfo2.cell.indexChunkCount;
							this.m_TempCellToUnloadList.Add(in cellInfo2);
							this.TryLoadCell(cellInfo3, ref num2, ref num, this.m_TempCellToLoadList);
						}
						if (num4 > 0)
						{
							this.m_LoadedCells.RemoveRange(this.m_LoadedCells.size - num4, num4);
							this.RecomputeMinMaxLoadedCellPos();
						}
					}
				}
				else
				{
					for (int i = 0; i < num3; i++)
					{
						ProbeReferenceVolume.CellInfo cellInfo4 = *this.m_ToBeLoadedCells[this.m_TempCellToLoadList.size];
						this.TryLoadCell(cellInfo4, ref num2, ref num, this.m_TempCellToLoadList);
					}
				}
				this.m_ToBeLoadedCells.RemoveRange(0, this.m_TempCellToLoadList.size);
				this.m_LoadedCells.AddRange(this.m_TempCellToLoadList);
				this.m_ToBeLoadedCells.AddRange(this.m_TempCellToUnloadList);
				this.m_TempCellToLoadList.Clear();
				this.m_TempCellToUnloadList.Clear();
			}
			if (this.enableScenarioBlending)
			{
				using (new ProfilingScope(cmd, ProfilingSampler.Get<CoreProfileId>(CoreProfileId.APVScenarioBlendingUpdate)))
				{
					this.UpdateBlendingCellStreaming(cmd);
				}
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00017250 File Offset: 0x00015450
		private unsafe int FindWorstBlendingCellToBeLoaded()
		{
			int num = -1;
			float num2 = -1f;
			float scenarioBlendingFactor = this.scenarioBlendingFactor;
			for (int i = this.m_TempBlendingCellToLoadList.size; i < this.m_ToBeLoadedBlendingCells.size; i++)
			{
				float num3 = Mathf.Abs(this.m_ToBeLoadedBlendingCells[i]->blendingFactor - scenarioBlendingFactor);
				if (num3 > num2)
				{
					num = i;
					if (this.m_ToBeLoadedBlendingCells[i]->ShouldReupload())
					{
						break;
					}
					num2 = num3;
				}
			}
			return num;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000172C8 File Offset: 0x000154C8
		private unsafe void UpdateBlendingCellStreaming(CommandBuffer cmd)
		{
			if (!this.m_HasRemainingCellsToBlend)
			{
				return;
			}
			float num = ((this.m_LoadedCells.size != 0) ? this.m_LoadedCells[this.m_LoadedCells.size - 1]->streamingScore : 0f);
			float num2 = ((this.m_ToBeLoadedCells.size != 0) ? this.m_ToBeLoadedCells[this.m_ToBeLoadedCells.size - 1]->streamingScore : 0f);
			float num3 = Mathf.Max(num, num2);
			this.ComputeStreamingScoreForBlending(this.m_ToBeLoadedBlendingCells, num3);
			this.ComputeStreamingScoreForBlending(this.m_LoadedBlendingCells, num3);
			this.m_ToBeLoadedBlendingCells.QuickSort<ProbeReferenceVolume.BlendingCellInfo>();
			this.m_LoadedBlendingCells.QuickSort<ProbeReferenceVolume.BlendingCellInfo>();
			int num4 = Mathf.Min(this.m_NumberOfCellsLoadedPerFrame, this.m_ToBeLoadedBlendingCells.size);
			while (this.m_TempBlendingCellToLoadList.size < num4)
			{
				ProbeReferenceVolume.BlendingCellInfo blendingCellInfo = *this.m_ToBeLoadedBlendingCells[this.m_TempBlendingCellToLoadList.size];
				if (!this.TryLoadBlendingCell(blendingCellInfo, this.m_TempBlendingCellToLoadList))
				{
					break;
				}
			}
			if (this.m_TempBlendingCellToLoadList.size != num4)
			{
				int num5 = -1;
				int num6 = (int)((float)this.m_LoadedBlendingCells.size * (1f - this.turnoverRate));
				ProbeReferenceVolume.BlendingCellInfo blendingCellInfo2 = ((num6 < this.m_LoadedBlendingCells.size) ? (*this.m_LoadedBlendingCells[num6]) : null);
				while (this.m_TempBlendingCellToLoadList.size < num4 && this.m_LoadedBlendingCells.size - this.m_TempBlendingCellToUnloadList.size != 0)
				{
					ProbeReferenceVolume.BlendingCellInfo blendingCellInfo3 = *this.m_LoadedBlendingCells[this.m_LoadedBlendingCells.size - this.m_TempBlendingCellToUnloadList.size - 1];
					ProbeReferenceVolume.BlendingCellInfo blendingCellInfo4 = *this.m_ToBeLoadedBlendingCells[this.m_TempBlendingCellToLoadList.size];
					if (blendingCellInfo4.streamingScore >= (blendingCellInfo2 ?? blendingCellInfo3).streamingScore)
					{
						if (blendingCellInfo2 == null)
						{
							break;
						}
						if (num5 == -1)
						{
							num5 = this.FindWorstBlendingCellToBeLoaded();
						}
						blendingCellInfo4 = *this.m_ToBeLoadedBlendingCells[num5];
						if (blendingCellInfo4.IsUpToDate())
						{
							break;
						}
					}
					this.UnloadBlendingCell(blendingCellInfo3, this.m_TempBlendingCellToUnloadList);
					if (this.TryLoadBlendingCell(blendingCellInfo4, this.m_TempBlendingCellToLoadList) && num5 != -1)
					{
						*this.m_ToBeLoadedBlendingCells[num5] = *this.m_ToBeLoadedBlendingCells[this.m_TempBlendingCellToLoadList.size - 1];
						*this.m_ToBeLoadedBlendingCells[this.m_TempBlendingCellToLoadList.size - 1] = blendingCellInfo4;
						if (++num5 >= this.m_ToBeLoadedBlendingCells.size)
						{
							num5 = this.m_TempBlendingCellToLoadList.size;
						}
					}
				}
				this.m_LoadedBlendingCells.RemoveRange(this.m_LoadedBlendingCells.size - this.m_TempBlendingCellToUnloadList.size, this.m_TempBlendingCellToUnloadList.size);
			}
			this.m_ToBeLoadedBlendingCells.RemoveRange(0, this.m_TempBlendingCellToLoadList.size);
			this.m_LoadedBlendingCells.AddRange(this.m_TempBlendingCellToLoadList);
			this.m_TempBlendingCellToLoadList.Clear();
			this.m_ToBeLoadedBlendingCells.AddRange(this.m_TempBlendingCellToUnloadList);
			this.m_TempBlendingCellToUnloadList.Clear();
			if (this.m_LoadedBlendingCells.size != 0)
			{
				float scenarioBlendingFactor = this.scenarioBlendingFactor;
				int num7 = Mathf.Min(this.numberOfCellsBlendedPerFrame, this.m_LoadedBlendingCells.size);
				for (int i = 0; i < num7; i++)
				{
					this.m_LoadedBlendingCells[i]->blendingFactor = scenarioBlendingFactor;
					this.m_BlendingPool.BlendChunks(*this.m_LoadedBlendingCells[i], this.m_Pool);
				}
				this.m_BlendingPool.PerformBlending(cmd, scenarioBlendingFactor, this.m_Pool);
			}
			if (this.m_ToBeLoadedBlendingCells.size == 0)
			{
				this.m_HasRemainingCellsToBlend = false;
			}
		}

		// Token: 0x040002A1 RID: 673
		private bool m_IsInitialized;

		// Token: 0x040002A2 RID: 674
		private bool m_SupportStreaming;

		// Token: 0x040002A3 RID: 675
		private ProbeReferenceVolume.RefVolTransform m_Transform;

		// Token: 0x040002A4 RID: 676
		private int m_MaxSubdivision;

		// Token: 0x040002A5 RID: 677
		private ProbeBrickPool m_Pool;

		// Token: 0x040002A6 RID: 678
		private ProbeBrickIndex m_Index;

		// Token: 0x040002A7 RID: 679
		private ProbeCellIndices m_CellIndices;

		// Token: 0x040002A8 RID: 680
		private ProbeBrickBlendingPool m_BlendingPool;

		// Token: 0x040002A9 RID: 681
		private List<ProbeBrickPool.BrickChunkAlloc> m_TmpSrcChunks = new List<ProbeBrickPool.BrickChunkAlloc>();

		// Token: 0x040002AA RID: 682
		private float[] m_PositionOffsets = new float[4];

		// Token: 0x040002AB RID: 683
		private Bounds m_CurrGlobalBounds;

		// Token: 0x040002AC RID: 684
		internal Dictionary<int, ProbeReferenceVolume.CellInfo> cells = new Dictionary<int, ProbeReferenceVolume.CellInfo>();

		// Token: 0x040002AD RID: 685
		private ObjectPool<ProbeReferenceVolume.CellInfo> m_CellInfoPool = new ObjectPool<ProbeReferenceVolume.CellInfo>(delegate(ProbeReferenceVolume.CellInfo x)
		{
			x.Clear();
		}, null, false);

		// Token: 0x040002AE RID: 686
		private ObjectPool<ProbeReferenceVolume.BlendingCellInfo> m_BlendingCellInfoPool = new ObjectPool<ProbeReferenceVolume.BlendingCellInfo>(delegate(ProbeReferenceVolume.BlendingCellInfo x)
		{
			x.Clear();
		}, null, false);

		// Token: 0x040002AF RID: 687
		private ProbeBrickPool.DataLocation m_TemporaryDataLocation;

		// Token: 0x040002B0 RID: 688
		private int m_TemporaryDataLocationMemCost;

		// Token: 0x040002B1 RID: 689
		private int m_CurrentProbeVolumeChunkSizeInBricks;

		// Token: 0x040002B2 RID: 690
		internal ProbeVolumeSceneData sceneData;

		// Token: 0x040002B3 RID: 691
		private Vector3Int minLoadedCellPos = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);

		// Token: 0x040002B4 RID: 692
		private Vector3Int maxLoadedCellPos = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

		// Token: 0x040002B5 RID: 693
		public Action<ProbeReferenceVolume.ExtraDataActionInput> retrieveExtraDataAction;

		// Token: 0x040002B6 RID: 694
		public Action checksDuringBakeAction;

		// Token: 0x040002B7 RID: 695
		private bool m_BricksLoaded;

		// Token: 0x040002B8 RID: 696
		private Dictionary<string, ProbeVolumeAsset> m_PendingAssetsToBeLoaded = new Dictionary<string, ProbeVolumeAsset>();

		// Token: 0x040002B9 RID: 697
		private Dictionary<string, ProbeVolumeAsset> m_PendingAssetsToBeUnloaded = new Dictionary<string, ProbeVolumeAsset>();

		// Token: 0x040002BA RID: 698
		private Dictionary<string, ProbeVolumeAsset> m_ActiveAssets = new Dictionary<string, ProbeVolumeAsset>();

		// Token: 0x040002BB RID: 699
		private bool m_NeedLoadAsset;

		// Token: 0x040002BC RID: 700
		private bool m_ProbeReferenceVolumeInit;

		// Token: 0x040002BD RID: 701
		private bool m_EnabledBySRP;

		// Token: 0x040002BE RID: 702
		private ProbeReferenceVolume.InitInfo m_PendingInitInfo;

		// Token: 0x040002BF RID: 703
		private bool m_NeedsIndexRebuild;

		// Token: 0x040002C0 RID: 704
		private bool m_HasChangedIndex;

		// Token: 0x040002C1 RID: 705
		private int m_CBShaderID = Shader.PropertyToID("ShaderVariablesProbeVolumes");

		// Token: 0x040002C2 RID: 706
		private int m_NumberOfCellsLoadedPerFrame = 2;

		// Token: 0x040002C3 RID: 707
		private int m_NumberOfCellsBlendedPerFrame = 10000;

		// Token: 0x040002C4 RID: 708
		private float m_TurnoverRate = 0.1f;

		// Token: 0x040002C5 RID: 709
		private ProbeVolumeTextureMemoryBudget m_MemoryBudget;

		// Token: 0x040002C6 RID: 710
		private ProbeVolumeBlendingTextureMemoryBudget m_BlendingMemoryBudget;

		// Token: 0x040002C7 RID: 711
		private ProbeVolumeSHBands m_SHBands;

		// Token: 0x040002C8 RID: 712
		private float m_ProbeVolumesWeight;

		// Token: 0x040002C9 RID: 713
		internal bool clearAssetsOnVolumeClear;

		// Token: 0x040002CA RID: 714
		internal static string defaultLightingScenario = "Default";

		// Token: 0x040002CB RID: 715
		private static ProbeReferenceVolume _instance = new ProbeReferenceVolume();

		// Token: 0x040002CD RID: 717
		private const int kProbesPerBatch = 511;

		// Token: 0x040002CE RID: 718
		public static readonly string k_DebugPanelName = "Probe Volume";

		// Token: 0x040002D1 RID: 721
		private DebugUI.Widget[] m_DebugItems;

		// Token: 0x040002D2 RID: 722
		private Mesh m_DebugMesh;

		// Token: 0x040002D3 RID: 723
		private Material m_DebugMaterial;

		// Token: 0x040002D4 RID: 724
		private Mesh m_DebugOffsetMesh;

		// Token: 0x040002D5 RID: 725
		private Material m_DebugOffsetMaterial;

		// Token: 0x040002D6 RID: 726
		private Plane[] m_DebugFrustumPlanes = new Plane[6];

		// Token: 0x040002D7 RID: 727
		private GUIContent[] m_DebugScenarioNames = new GUIContent[0];

		// Token: 0x040002D8 RID: 728
		private int[] m_DebugScenarioValues = new int[0];

		// Token: 0x040002D9 RID: 729
		private string m_DebugActiveSceneGUID;

		// Token: 0x040002DA RID: 730
		private string m_DebugActiveScenario;

		// Token: 0x040002DB RID: 731
		private DebugUI.EnumField m_DebugScenarioField;

		// Token: 0x040002DC RID: 732
		internal ProbeVolumeBakingProcessSettings bakingProcessSettings;

		// Token: 0x040002DD RID: 733
		internal Dictionary<Bounds, ProbeBrickIndex.Brick[]> realtimeSubdivisionInfo = new Dictionary<Bounds, ProbeBrickIndex.Brick[]>();

		// Token: 0x040002DE RID: 734
		private bool m_MaxSubdivVisualizedIsMaxAvailable;

		// Token: 0x040002DF RID: 735
		private DynamicArray<ProbeReferenceVolume.CellInfo> m_LoadedCells = new DynamicArray<ProbeReferenceVolume.CellInfo>();

		// Token: 0x040002E0 RID: 736
		private DynamicArray<ProbeReferenceVolume.CellInfo> m_ToBeLoadedCells = new DynamicArray<ProbeReferenceVolume.CellInfo>();

		// Token: 0x040002E1 RID: 737
		private DynamicArray<ProbeReferenceVolume.CellInfo> m_TempCellToLoadList = new DynamicArray<ProbeReferenceVolume.CellInfo>();

		// Token: 0x040002E2 RID: 738
		private DynamicArray<ProbeReferenceVolume.CellInfo> m_TempCellToUnloadList = new DynamicArray<ProbeReferenceVolume.CellInfo>();

		// Token: 0x040002E3 RID: 739
		private DynamicArray<ProbeReferenceVolume.BlendingCellInfo> m_LoadedBlendingCells = new DynamicArray<ProbeReferenceVolume.BlendingCellInfo>();

		// Token: 0x040002E4 RID: 740
		private DynamicArray<ProbeReferenceVolume.BlendingCellInfo> m_ToBeLoadedBlendingCells = new DynamicArray<ProbeReferenceVolume.BlendingCellInfo>();

		// Token: 0x040002E5 RID: 741
		private DynamicArray<ProbeReferenceVolume.BlendingCellInfo> m_TempBlendingCellToLoadList = new DynamicArray<ProbeReferenceVolume.BlendingCellInfo>();

		// Token: 0x040002E6 RID: 742
		private DynamicArray<ProbeReferenceVolume.BlendingCellInfo> m_TempBlendingCellToUnloadList = new DynamicArray<ProbeReferenceVolume.BlendingCellInfo>();

		// Token: 0x040002E7 RID: 743
		private Vector3 m_FrozenCameraPosition;

		// Token: 0x040002E8 RID: 744
		private bool m_HasRemainingCellsToBlend;

		// Token: 0x0200019D RID: 413
		[DebuggerDisplay("Index = {index} position = {position}")]
		[Serializable]
		internal class Cell
		{
			// Token: 0x17000182 RID: 386
			// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x0002E4DF File Offset: 0x0002C6DF
			// (set) Token: 0x06000AD4 RID: 2772 RVA: 0x0002E4E7 File Offset: 0x0002C6E7
			public NativeArray<ProbeBrickIndex.Brick> bricks { get; internal set; }

			// Token: 0x17000183 RID: 387
			// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0002E4F0 File Offset: 0x0002C6F0
			// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x0002E4F8 File Offset: 0x0002C6F8
			public NativeArray<byte> validityNeighMaskData { get; internal set; }

			// Token: 0x17000184 RID: 388
			// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0002E501 File Offset: 0x0002C701
			// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x0002E509 File Offset: 0x0002C709
			public NativeArray<Vector3> probePositions { get; internal set; }

			// Token: 0x17000185 RID: 389
			// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x0002E512 File Offset: 0x0002C712
			// (set) Token: 0x06000ADA RID: 2778 RVA: 0x0002E51A File Offset: 0x0002C71A
			public NativeArray<float> touchupVolumeInteraction { get; internal set; }

			// Token: 0x17000186 RID: 390
			// (get) Token: 0x06000ADB RID: 2779 RVA: 0x0002E523 File Offset: 0x0002C723
			// (set) Token: 0x06000ADC RID: 2780 RVA: 0x0002E52B File Offset: 0x0002C72B
			public NativeArray<Vector3> offsetVectors { get; internal set; }

			// Token: 0x17000187 RID: 391
			// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0002E534 File Offset: 0x0002C734
			// (set) Token: 0x06000ADE RID: 2782 RVA: 0x0002E53C File Offset: 0x0002C73C
			public NativeArray<float> validity { get; internal set; }

			// Token: 0x17000188 RID: 392
			// (get) Token: 0x06000ADF RID: 2783 RVA: 0x0002E545 File Offset: 0x0002C745
			public ProbeReferenceVolume.Cell.PerScenarioData bakingScenario
			{
				get
				{
					return this.scenario0;
				}
			}

			// Token: 0x040006AB RID: 1707
			public Vector3Int position;

			// Token: 0x040006AC RID: 1708
			public int index;

			// Token: 0x040006AD RID: 1709
			public int probeCount;

			// Token: 0x040006AE RID: 1710
			public int minSubdiv;

			// Token: 0x040006AF RID: 1711
			public int maxSubdiv;

			// Token: 0x040006B0 RID: 1712
			public int indexChunkCount;

			// Token: 0x040006B1 RID: 1713
			public int shChunkCount;

			// Token: 0x040006B2 RID: 1714
			public bool hasTwoScenarios;

			// Token: 0x040006B3 RID: 1715
			public ProbeVolumeSHBands shBands;

			// Token: 0x040006BA RID: 1722
			[NonSerialized]
			public ProbeReferenceVolume.Cell.PerScenarioData scenario0;

			// Token: 0x040006BB RID: 1723
			[NonSerialized]
			public ProbeReferenceVolume.Cell.PerScenarioData scenario1;

			// Token: 0x020001FA RID: 506
			public struct PerScenarioData
			{
				// Token: 0x17000195 RID: 405
				// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x000307C5 File Offset: 0x0002E9C5
				// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x000307CD File Offset: 0x0002E9CD
				public NativeArray<ushort> shL0L1RxData { readonly get; internal set; }

				// Token: 0x17000196 RID: 406
				// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x000307D6 File Offset: 0x0002E9D6
				// (set) Token: 0x06000BBA RID: 3002 RVA: 0x000307DE File Offset: 0x0002E9DE
				public NativeArray<byte> shL1GL1RyData { readonly get; internal set; }

				// Token: 0x17000197 RID: 407
				// (get) Token: 0x06000BBB RID: 3003 RVA: 0x000307E7 File Offset: 0x0002E9E7
				// (set) Token: 0x06000BBC RID: 3004 RVA: 0x000307EF File Offset: 0x0002E9EF
				public NativeArray<byte> shL1BL1RzData { readonly get; internal set; }

				// Token: 0x17000198 RID: 408
				// (get) Token: 0x06000BBD RID: 3005 RVA: 0x000307F8 File Offset: 0x0002E9F8
				// (set) Token: 0x06000BBE RID: 3006 RVA: 0x00030800 File Offset: 0x0002EA00
				public NativeArray<byte> shL2Data_0 { readonly get; internal set; }

				// Token: 0x17000199 RID: 409
				// (get) Token: 0x06000BBF RID: 3007 RVA: 0x00030809 File Offset: 0x0002EA09
				// (set) Token: 0x06000BC0 RID: 3008 RVA: 0x00030811 File Offset: 0x0002EA11
				public NativeArray<byte> shL2Data_1 { readonly get; internal set; }

				// Token: 0x1700019A RID: 410
				// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0003081A File Offset: 0x0002EA1A
				// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x00030822 File Offset: 0x0002EA22
				public NativeArray<byte> shL2Data_2 { readonly get; internal set; }

				// Token: 0x1700019B RID: 411
				// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x0003082B File Offset: 0x0002EA2B
				// (set) Token: 0x06000BC4 RID: 3012 RVA: 0x00030833 File Offset: 0x0002EA33
				public NativeArray<byte> shL2Data_3 { readonly get; internal set; }
			}
		}

		// Token: 0x0200019E RID: 414
		[DebuggerDisplay("Index = {cell.index} Loaded = {loaded}")]
		internal class CellInfo : IComparable<ProbeReferenceVolume.CellInfo>
		{
			// Token: 0x06000AE1 RID: 2785 RVA: 0x0002E555 File Offset: 0x0002C755
			public int CompareTo(ProbeReferenceVolume.CellInfo other)
			{
				if (this.streamingScore < other.streamingScore)
				{
					return -1;
				}
				if (this.streamingScore > other.streamingScore)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06000AE2 RID: 2786 RVA: 0x0002E578 File Offset: 0x0002C778
			public void Clear()
			{
				this.cell = null;
				this.blendingCell = null;
				this.chunkList.Clear();
				this.flatIdxInCellIndices = -1;
				this.loaded = false;
				this.updateInfo = default(ProbeBrickIndex.CellIndexUpdateInfo);
				this.sourceAssetInstanceID = -1;
				this.streamingScore = 0f;
				this.referenceCount = 0;
				this.debugProbes = null;
			}

			// Token: 0x040006BC RID: 1724
			public ProbeReferenceVolume.Cell cell;

			// Token: 0x040006BD RID: 1725
			public ProbeReferenceVolume.BlendingCellInfo blendingCell;

			// Token: 0x040006BE RID: 1726
			public List<ProbeBrickPool.BrickChunkAlloc> chunkList = new List<ProbeBrickPool.BrickChunkAlloc>();

			// Token: 0x040006BF RID: 1727
			public int flatIdxInCellIndices = -1;

			// Token: 0x040006C0 RID: 1728
			public bool loaded;

			// Token: 0x040006C1 RID: 1729
			public ProbeBrickIndex.CellIndexUpdateInfo updateInfo;

			// Token: 0x040006C2 RID: 1730
			public bool indexUpdated;

			// Token: 0x040006C3 RID: 1731
			public ProbeBrickIndex.CellIndexUpdateInfo tempUpdateInfo;

			// Token: 0x040006C4 RID: 1732
			public int sourceAssetInstanceID;

			// Token: 0x040006C5 RID: 1733
			public float streamingScore;

			// Token: 0x040006C6 RID: 1734
			public int referenceCount;

			// Token: 0x040006C7 RID: 1735
			public ProbeReferenceVolume.CellInstancedDebugProbes debugProbes;
		}

		// Token: 0x0200019F RID: 415
		[DebuggerDisplay("Index = {cellInfo.cell.index} Factor = {blendingFactor} Score = {streamingScore}")]
		internal class BlendingCellInfo : IComparable<ProbeReferenceVolume.BlendingCellInfo>
		{
			// Token: 0x06000AE4 RID: 2788 RVA: 0x0002E5F2 File Offset: 0x0002C7F2
			public int CompareTo(ProbeReferenceVolume.BlendingCellInfo other)
			{
				if (this.streamingScore < other.streamingScore)
				{
					return -1;
				}
				if (this.streamingScore > other.streamingScore)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06000AE5 RID: 2789 RVA: 0x0002E615 File Offset: 0x0002C815
			public void Clear()
			{
				this.cellInfo = null;
				this.chunkList.Clear();
				this.blendingFactor = 0f;
				this.streamingScore = 0f;
				this.blending = false;
			}

			// Token: 0x06000AE6 RID: 2790 RVA: 0x0002E646 File Offset: 0x0002C846
			public void MarkUpToDate()
			{
				this.streamingScore = float.MaxValue;
			}

			// Token: 0x06000AE7 RID: 2791 RVA: 0x0002E653 File Offset: 0x0002C853
			public bool IsUpToDate()
			{
				return this.streamingScore == float.MaxValue;
			}

			// Token: 0x06000AE8 RID: 2792 RVA: 0x0002E662 File Offset: 0x0002C862
			public void ForceReupload()
			{
				this.blendingFactor = -1f;
			}

			// Token: 0x06000AE9 RID: 2793 RVA: 0x0002E66F File Offset: 0x0002C86F
			public bool ShouldReupload()
			{
				return this.blendingFactor == -1f;
			}

			// Token: 0x06000AEA RID: 2794 RVA: 0x0002E67E File Offset: 0x0002C87E
			public void Prioritize()
			{
				this.blendingFactor = -2f;
			}

			// Token: 0x06000AEB RID: 2795 RVA: 0x0002E68B File Offset: 0x0002C88B
			public bool ShouldPrioritize()
			{
				return this.blendingFactor == -2f;
			}

			// Token: 0x040006C8 RID: 1736
			public ProbeReferenceVolume.CellInfo cellInfo;

			// Token: 0x040006C9 RID: 1737
			public List<ProbeBrickPool.BrickChunkAlloc> chunkList = new List<ProbeBrickPool.BrickChunkAlloc>();

			// Token: 0x040006CA RID: 1738
			public float streamingScore;

			// Token: 0x040006CB RID: 1739
			public float blendingFactor;

			// Token: 0x040006CC RID: 1740
			public bool blending;
		}

		// Token: 0x020001A0 RID: 416
		internal struct Volume : IEquatable<ProbeReferenceVolume.Volume>
		{
			// Token: 0x06000AED RID: 2797 RVA: 0x0002E6B0 File Offset: 0x0002C8B0
			public Volume(Matrix4x4 trs, float maxSubdivision, float minSubdivision)
			{
				this.X = trs.GetColumn(0);
				this.Y = trs.GetColumn(1);
				this.Z = trs.GetColumn(2);
				this.corner = trs.GetColumn(3) - this.X * 0.5f - this.Y * 0.5f - this.Z * 0.5f;
				this.maxSubdivisionMultiplier = maxSubdivision;
				this.minSubdivisionMultiplier = minSubdivision;
			}

			// Token: 0x06000AEE RID: 2798 RVA: 0x0002E756 File Offset: 0x0002C956
			public Volume(Vector3 corner, Vector3 X, Vector3 Y, Vector3 Z, float maxSubdivision = 1f, float minSubdivision = 0f)
			{
				this.corner = corner;
				this.X = X;
				this.Y = Y;
				this.Z = Z;
				this.maxSubdivisionMultiplier = maxSubdivision;
				this.minSubdivisionMultiplier = minSubdivision;
			}

			// Token: 0x06000AEF RID: 2799 RVA: 0x0002E788 File Offset: 0x0002C988
			public Volume(ProbeReferenceVolume.Volume copy)
			{
				this.X = copy.X;
				this.Y = copy.Y;
				this.Z = copy.Z;
				this.corner = copy.corner;
				this.maxSubdivisionMultiplier = copy.maxSubdivisionMultiplier;
				this.minSubdivisionMultiplier = copy.minSubdivisionMultiplier;
			}

			// Token: 0x06000AF0 RID: 2800 RVA: 0x0002E7E0 File Offset: 0x0002C9E0
			public Volume(Bounds bounds)
			{
				Vector3 size = bounds.size;
				this.corner = bounds.center - size * 0.5f;
				this.X = new Vector3(size.x, 0f, 0f);
				this.Y = new Vector3(0f, size.y, 0f);
				this.Z = new Vector3(0f, 0f, size.z);
				this.maxSubdivisionMultiplier = (this.minSubdivisionMultiplier = 0f);
			}

			// Token: 0x06000AF1 RID: 2801 RVA: 0x0002E878 File Offset: 0x0002CA78
			public Bounds CalculateAABB()
			{
				Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
				Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < 2; j++)
					{
						for (int k = 0; k < 2; k++)
						{
							Vector3 vector3 = new Vector3((float)i, (float)j, (float)k);
							Vector3 vector4 = this.corner + this.X * vector3.x + this.Y * vector3.y + this.Z * vector3.z;
							vector = Vector3.Min(vector, vector4);
							vector2 = Vector3.Max(vector2, vector4);
						}
					}
				}
				return new Bounds((vector + vector2) / 2f, vector2 - vector);
			}

			// Token: 0x06000AF2 RID: 2802 RVA: 0x0002E970 File Offset: 0x0002CB70
			public void CalculateCenterAndSize(out Vector3 center, out Vector3 size)
			{
				size = new Vector3(this.X.magnitude, this.Y.magnitude, this.Z.magnitude);
				center = this.corner + this.X * 0.5f + this.Y * 0.5f + this.Z * 0.5f;
			}

			// Token: 0x06000AF3 RID: 2803 RVA: 0x0002E9F4 File Offset: 0x0002CBF4
			public void Transform(Matrix4x4 trs)
			{
				this.corner = trs.MultiplyPoint(this.corner);
				this.X = trs.MultiplyVector(this.X);
				this.Y = trs.MultiplyVector(this.Y);
				this.Z = trs.MultiplyVector(this.Z);
			}

			// Token: 0x06000AF4 RID: 2804 RVA: 0x0002EA50 File Offset: 0x0002CC50
			public override string ToString()
			{
				return string.Format("Corner: {0}, X: {1}, Y: {2}, Z: {3}, MaxSubdiv: {4}", new object[] { this.corner, this.X, this.Y, this.Z, this.maxSubdivisionMultiplier });
			}

			// Token: 0x06000AF5 RID: 2805 RVA: 0x0002EAB4 File Offset: 0x0002CCB4
			public bool Equals(ProbeReferenceVolume.Volume other)
			{
				return this.corner == other.corner && this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.minSubdivisionMultiplier == other.minSubdivisionMultiplier && this.maxSubdivisionMultiplier == other.maxSubdivisionMultiplier;
			}

			// Token: 0x040006CD RID: 1741
			internal Vector3 corner;

			// Token: 0x040006CE RID: 1742
			internal Vector3 X;

			// Token: 0x040006CF RID: 1743
			internal Vector3 Y;

			// Token: 0x040006D0 RID: 1744
			internal Vector3 Z;

			// Token: 0x040006D1 RID: 1745
			internal float maxSubdivisionMultiplier;

			// Token: 0x040006D2 RID: 1746
			internal float minSubdivisionMultiplier;
		}

		// Token: 0x020001A1 RID: 417
		internal struct RefVolTransform
		{
			// Token: 0x040006D3 RID: 1747
			public Vector3 posWS;

			// Token: 0x040006D4 RID: 1748
			public Quaternion rot;

			// Token: 0x040006D5 RID: 1749
			public float scale;
		}

		// Token: 0x020001A2 RID: 418
		public struct RuntimeResources
		{
			// Token: 0x040006D6 RID: 1750
			public ComputeBuffer index;

			// Token: 0x040006D7 RID: 1751
			public ComputeBuffer cellIndices;

			// Token: 0x040006D8 RID: 1752
			public RenderTexture L0_L1rx;

			// Token: 0x040006D9 RID: 1753
			public RenderTexture L1_G_ry;

			// Token: 0x040006DA RID: 1754
			public RenderTexture L1_B_rz;

			// Token: 0x040006DB RID: 1755
			public RenderTexture L2_0;

			// Token: 0x040006DC RID: 1756
			public RenderTexture L2_1;

			// Token: 0x040006DD RID: 1757
			public RenderTexture L2_2;

			// Token: 0x040006DE RID: 1758
			public RenderTexture L2_3;

			// Token: 0x040006DF RID: 1759
			public Texture3D Validity;
		}

		// Token: 0x020001A3 RID: 419
		public struct ExtraDataActionInput
		{
		}

		// Token: 0x020001A4 RID: 420
		private struct InitInfo
		{
			// Token: 0x040006E0 RID: 1760
			public Vector3Int pendingMinCellPosition;

			// Token: 0x040006E1 RID: 1761
			public Vector3Int pendingMaxCellPosition;
		}

		// Token: 0x020001A5 RID: 421
		internal class CellInstancedDebugProbes
		{
			// Token: 0x040006E2 RID: 1762
			public List<Matrix4x4[]> probeBuffers;

			// Token: 0x040006E3 RID: 1763
			public List<Matrix4x4[]> offsetBuffers;

			// Token: 0x040006E4 RID: 1764
			public List<MaterialPropertyBlock> props;
		}
	}
}
