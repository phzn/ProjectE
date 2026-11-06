using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000096 RID: 150
	[ExecuteAlways]
	[AddComponentMenu("")]
	public class ProbeVolumePerSceneData : MonoBehaviour, ISerializationCallbackReceiver
	{
		// Token: 0x060004DB RID: 1243 RVA: 0x00017F8C File Offset: 0x0001618C
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.scenarios.Clear();
			foreach (ProbeVolumePerSceneData.SerializablePerScenarioDataItem serializablePerScenarioDataItem in this.serializedScenarios)
			{
				this.scenarios.Add(serializablePerScenarioDataItem.scenario, serializablePerScenarioDataItem.data);
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00017FFC File Offset: 0x000161FC
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.serializedScenarios.Clear();
			foreach (KeyValuePair<string, ProbeVolumePerSceneData.PerScenarioData> keyValuePair in this.scenarios)
			{
				this.serializedScenarios.Add(new ProbeVolumePerSceneData.SerializablePerScenarioDataItem
				{
					scenario = keyValuePair.Key,
					data = keyValuePair.Value
				});
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00018084 File Offset: 0x00016284
		internal void Clear()
		{
			this.QueueAssetRemoval();
			this.scenarios.Clear();
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00018097 File Offset: 0x00016297
		internal void RemoveScenario(string scenario)
		{
			this.scenarios.Remove(scenario);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000180A8 File Offset: 0x000162A8
		internal void RenameScenario(string scenario, string newName)
		{
			ProbeVolumePerSceneData.PerScenarioData perScenarioData;
			if (!this.scenarios.TryGetValue(scenario, out perScenarioData))
			{
				return;
			}
			this.scenarios.Remove(scenario);
			this.scenarios.Add(newName, perScenarioData);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000180E0 File Offset: 0x000162E0
		internal bool ResolveCells()
		{
			return this.ResolveSharedCellData() && this.ResolvePerScenarioCellData();
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000180F2 File Offset: 0x000162F2
		internal bool ResolveSharedCellData()
		{
			return this.asset != null && this.asset.ResolveSharedCellData(this.cellSharedDataAsset, this.cellSupportDataAsset);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001811C File Offset: 0x0001631C
		private bool ResolvePerScenarioCellData()
		{
			int num = 0;
			int num2 = ((this.otherScenario == null) ? 1 : 2);
			ProbeVolumePerSceneData.PerScenarioData perScenarioData;
			if (this.activeScenario != null && this.scenarios.TryGetValue(this.activeScenario, out perScenarioData) && this.asset.ResolvePerScenarioCellData(perScenarioData.cellDataAsset, perScenarioData.cellOptionalDataAsset, 0))
			{
				num++;
			}
			ProbeVolumePerSceneData.PerScenarioData perScenarioData2;
			if (this.otherScenario != null && this.scenarios.TryGetValue(this.otherScenario, out perScenarioData2) && this.asset.ResolvePerScenarioCellData(perScenarioData2.cellDataAsset, perScenarioData2.cellOptionalDataAsset, num))
			{
				num++;
			}
			for (int i = 0; i < this.asset.cells.Length; i++)
			{
				this.asset.cells[i].hasTwoScenarios = num == 2;
			}
			return num == num2;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x000181E5 File Offset: 0x000163E5
		internal void QueueAssetLoading()
		{
			if (this.asset == null || this.asset.IsInvalid() || !this.ResolvePerScenarioCellData())
			{
				return;
			}
			ProbeReferenceVolume.instance.AddPendingAssetLoading(this.asset);
			this.assetLoaded = true;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00018222 File Offset: 0x00016422
		internal void QueueAssetRemoval()
		{
			if (this.asset != null)
			{
				ProbeReferenceVolume.instance.AddPendingAssetRemoval(this.asset);
			}
			this.assetLoaded = false;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00018249 File Offset: 0x00016449
		private void OnEnable()
		{
			ProbeReferenceVolume.instance.RegisterPerSceneData(this);
			if (ProbeReferenceVolume.instance.sceneData != null)
			{
				this.Initialize();
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00018268 File Offset: 0x00016468
		private void OnDisable()
		{
			this.QueueAssetRemoval();
			this.activeScenario = (this.otherScenario = null);
			ProbeReferenceVolume.instance.UnregisterPerSceneData(this);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00018296 File Offset: 0x00016496
		internal void Initialize()
		{
			this.ResolveSharedCellData();
			this.QueueAssetRemoval();
			this.activeScenario = ProbeReferenceVolume.instance.sceneData.lightingScenario;
			this.otherScenario = ProbeReferenceVolume.instance.sceneData.otherScenario;
			this.QueueAssetLoading();
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000182D5 File Offset: 0x000164D5
		internal void UpdateActiveScenario(string activeScenario, string otherScenario)
		{
			if (this.asset == null)
			{
				return;
			}
			this.activeScenario = activeScenario;
			this.otherScenario = otherScenario;
			if (!this.assetLoaded)
			{
				this.QueueAssetLoading();
				return;
			}
			if (!this.ResolvePerScenarioCellData())
			{
				this.QueueAssetRemoval();
			}
		}

		// Token: 0x04000336 RID: 822
		[SerializeField]
		internal ProbeVolumeAsset asset;

		// Token: 0x04000337 RID: 823
		[SerializeField]
		internal TextAsset cellSharedDataAsset;

		// Token: 0x04000338 RID: 824
		[SerializeField]
		internal TextAsset cellSupportDataAsset;

		// Token: 0x04000339 RID: 825
		[SerializeField]
		private List<ProbeVolumePerSceneData.SerializablePerScenarioDataItem> serializedScenarios = new List<ProbeVolumePerSceneData.SerializablePerScenarioDataItem>();

		// Token: 0x0400033A RID: 826
		internal Dictionary<string, ProbeVolumePerSceneData.PerScenarioData> scenarios = new Dictionary<string, ProbeVolumePerSceneData.PerScenarioData>();

		// Token: 0x0400033B RID: 827
		private bool assetLoaded;

		// Token: 0x0400033C RID: 828
		private string activeScenario;

		// Token: 0x0400033D RID: 829
		private string otherScenario;

		// Token: 0x020001AE RID: 430
		[Serializable]
		internal struct PerScenarioData
		{
			// Token: 0x0400071E RID: 1822
			public int sceneHash;

			// Token: 0x0400071F RID: 1823
			public TextAsset cellDataAsset;

			// Token: 0x04000720 RID: 1824
			public TextAsset cellOptionalDataAsset;
		}

		// Token: 0x020001AF RID: 431
		[Serializable]
		private struct SerializablePerScenarioDataItem
		{
			// Token: 0x04000721 RID: 1825
			public string scenario;

			// Token: 0x04000722 RID: 1826
			public ProbeVolumePerSceneData.PerScenarioData data;
		}
	}
}
