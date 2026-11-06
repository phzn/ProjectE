using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace UnityEngine.Rendering
{
	// Token: 0x02000097 RID: 151
	[Serializable]
	public class ProbeVolumeSceneData : ISerializationCallbackReceiver
	{
		// Token: 0x060004EA RID: 1258 RVA: 0x0001832F File Offset: 0x0001652F
		internal static string GetSceneGUID(Scene scene)
		{
			return (string)ProbeVolumeSceneData.s_SceneGUID.GetValue(scene);
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00018346 File Offset: 0x00016546
		internal string lightingScenario
		{
			get
			{
				return this.m_LightingScenario;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x0001834E File Offset: 0x0001654E
		internal string otherScenario
		{
			get
			{
				return this.m_OtherScenario;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x00018356 File Offset: 0x00016556
		internal float scenarioBlendingFactor
		{
			get
			{
				return this.m_ScenarioBlendingFactor;
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00018360 File Offset: 0x00016560
		internal void SetActiveScenario(string scenario)
		{
			if (this.m_LightingScenario == scenario && this.m_ScenarioBlendingFactor == 0f)
			{
				return;
			}
			this.m_LightingScenario = scenario;
			this.m_OtherScenario = null;
			this.m_ScenarioBlendingFactor = 0f;
			foreach (ProbeVolumePerSceneData probeVolumePerSceneData in ProbeReferenceVolume.instance.perSceneDataList)
			{
				probeVolumePerSceneData.UpdateActiveScenario(this.m_LightingScenario, this.m_OtherScenario);
			}
			if (ProbeReferenceVolume.instance.enableScenarioBlending)
			{
				ProbeReferenceVolume.instance.ScenarioBlendingChanged(true);
				return;
			}
			ProbeReferenceVolume.instance.UnloadAllCells();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00018418 File Offset: 0x00016618
		internal void BlendLightingScenario(string otherScenario, float blendingFactor)
		{
			if (!ProbeReferenceVolume.instance.enableScenarioBlending)
			{
				if (!ProbeBrickBlendingPool.isSupported)
				{
					Debug.LogError("Blending between lighting scenarios is not supported by this render pipeline.");
					return;
				}
				Debug.LogError("Blending between lighting scenarios is disabled in the render pipeline settings.");
				return;
			}
			else
			{
				blendingFactor = Mathf.Clamp01(blendingFactor);
				if (otherScenario == this.m_LightingScenario || string.IsNullOrEmpty(otherScenario))
				{
					otherScenario = null;
				}
				if (otherScenario == null)
				{
					blendingFactor = 0f;
				}
				if (otherScenario == this.m_OtherScenario && Mathf.Approximately(blendingFactor, this.m_ScenarioBlendingFactor))
				{
					return;
				}
				bool flag = otherScenario != this.m_OtherScenario;
				this.m_OtherScenario = otherScenario;
				this.m_ScenarioBlendingFactor = blendingFactor;
				if (flag)
				{
					foreach (ProbeVolumePerSceneData probeVolumePerSceneData in ProbeReferenceVolume.instance.perSceneDataList)
					{
						probeVolumePerSceneData.UpdateActiveScenario(this.m_LightingScenario, this.m_OtherScenario);
					}
				}
				ProbeReferenceVolume.instance.ScenarioBlendingChanged(flag);
				return;
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00018514 File Offset: 0x00016714
		public ProbeVolumeSceneData(Object parentAsset, string parentSceneDataPropertyName)
		{
			this.parentAsset = parentAsset;
			this.parentSceneDataPropertyName = parentSceneDataPropertyName;
			this.sceneBounds = new Dictionary<string, Bounds>();
			this.hasProbeVolumes = new Dictionary<string, bool>();
			this.sceneProfiles = new Dictionary<string, ProbeReferenceVolumeProfile>();
			this.sceneBakingSettings = new Dictionary<string, ProbeVolumeBakingProcessSettings>();
			this.bakingSets = new List<ProbeVolumeSceneData.BakingSet>();
			this.serializedBounds = new List<ProbeVolumeSceneData.SerializableBoundItem>();
			this.serializedHasVolumes = new List<ProbeVolumeSceneData.SerializableHasPVItem>();
			this.serializedProfiles = new List<ProbeVolumeSceneData.SerializablePVProfile>();
			this.serializedBakeSettings = new List<ProbeVolumeSceneData.SerializablePVBakeSettings>();
			this.UpdateBakingSets();
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x000185A9 File Offset: 0x000167A9
		public void SetParentObject(Object parent, string parentSceneDataPropertyName)
		{
			this.parentAsset = parent;
			this.parentSceneDataPropertyName = parentSceneDataPropertyName;
			this.UpdateBakingSets();
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000185C0 File Offset: 0x000167C0
		public void OnAfterDeserialize()
		{
			if (this.serializedBounds == null || this.serializedHasVolumes == null || this.serializedProfiles == null || this.serializedBakeSettings == null)
			{
				return;
			}
			this.sceneBounds = new Dictionary<string, Bounds>();
			this.hasProbeVolumes = new Dictionary<string, bool>();
			this.sceneProfiles = new Dictionary<string, ProbeReferenceVolumeProfile>();
			this.sceneBakingSettings = new Dictionary<string, ProbeVolumeBakingProcessSettings>();
			this.bakingSets = new List<ProbeVolumeSceneData.BakingSet>();
			foreach (ProbeVolumeSceneData.SerializableBoundItem serializableBoundItem in this.serializedBounds)
			{
				this.sceneBounds.Add(serializableBoundItem.sceneGUID, serializableBoundItem.bounds);
			}
			foreach (ProbeVolumeSceneData.SerializableHasPVItem serializableHasPVItem in this.serializedHasVolumes)
			{
				this.hasProbeVolumes.Add(serializableHasPVItem.sceneGUID, serializableHasPVItem.hasProbeVolumes);
			}
			foreach (ProbeVolumeSceneData.SerializablePVProfile serializablePVProfile in this.serializedProfiles)
			{
				this.sceneProfiles.Add(serializablePVProfile.sceneGUID, serializablePVProfile.profile);
			}
			foreach (ProbeVolumeSceneData.SerializablePVBakeSettings serializablePVBakeSettings in this.serializedBakeSettings)
			{
				this.sceneBakingSettings.Add(serializablePVBakeSettings.sceneGUID, serializablePVBakeSettings.settings);
			}
			if (string.IsNullOrEmpty(this.m_LightingScenario))
			{
				this.m_LightingScenario = ProbeReferenceVolume.defaultLightingScenario;
			}
			foreach (ProbeVolumeSceneData.BakingSet bakingSet in this.serializedBakingSets)
			{
				bakingSet.settings.Upgrade();
				this.bakingSets.Add(bakingSet);
			}
			if (this.m_OtherScenario == "")
			{
				this.m_OtherScenario = null;
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00018800 File Offset: 0x00016A00
		private void UpdateBakingSets()
		{
			foreach (ProbeVolumeSceneData.BakingSet bakingSet in this.serializedBakingSets)
			{
				if (bakingSet.profile == null)
				{
					this.InitializeBakingSet(bakingSet, bakingSet.name);
				}
				if (bakingSet.lightingScenarios.Count == 0)
				{
					this.InitializeScenarios(bakingSet);
				}
			}
			this.SyncBakingSetSettings();
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00018884 File Offset: 0x00016A84
		public void OnBeforeSerialize()
		{
			if (this.sceneBounds == null || this.hasProbeVolumes == null || this.sceneBakingSettings == null || this.sceneProfiles == null || this.serializedBounds == null || this.serializedHasVolumes == null || this.serializedBakeSettings == null || this.serializedProfiles == null || this.serializedBakingSets == null)
			{
				return;
			}
			this.serializedBounds.Clear();
			this.serializedHasVolumes.Clear();
			this.serializedProfiles.Clear();
			this.serializedBakeSettings.Clear();
			this.serializedBakingSets.Clear();
			foreach (string text in this.sceneBounds.Keys)
			{
				ProbeVolumeSceneData.SerializableBoundItem serializableBoundItem;
				serializableBoundItem.sceneGUID = text;
				serializableBoundItem.bounds = this.sceneBounds[text];
				this.serializedBounds.Add(serializableBoundItem);
			}
			foreach (string text2 in this.hasProbeVolumes.Keys)
			{
				ProbeVolumeSceneData.SerializableHasPVItem serializableHasPVItem;
				serializableHasPVItem.sceneGUID = text2;
				serializableHasPVItem.hasProbeVolumes = this.hasProbeVolumes[text2];
				this.serializedHasVolumes.Add(serializableHasPVItem);
			}
			foreach (string text3 in this.sceneBakingSettings.Keys)
			{
				ProbeVolumeSceneData.SerializablePVBakeSettings serializablePVBakeSettings;
				serializablePVBakeSettings.sceneGUID = text3;
				serializablePVBakeSettings.settings = this.sceneBakingSettings[text3];
				this.serializedBakeSettings.Add(serializablePVBakeSettings);
			}
			foreach (string text4 in this.sceneProfiles.Keys)
			{
				ProbeVolumeSceneData.SerializablePVProfile serializablePVProfile;
				serializablePVProfile.sceneGUID = text4;
				serializablePVProfile.profile = this.sceneProfiles[text4];
				this.serializedProfiles.Add(serializablePVProfile);
			}
			foreach (ProbeVolumeSceneData.BakingSet bakingSet in this.bakingSets)
			{
				this.serializedBakingSets.Add(bakingSet);
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00018B0C File Offset: 0x00016D0C
		internal ProbeVolumeSceneData.BakingSet CreateNewBakingSet(string name)
		{
			ProbeVolumeSceneData.BakingSet bakingSet = new ProbeVolumeSceneData.BakingSet();
			this.InitializeBakingSet(bakingSet, name);
			this.bakingSets.Add(bakingSet);
			return bakingSet;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00018B34 File Offset: 0x00016D34
		private void InitializeBakingSet(ProbeVolumeSceneData.BakingSet set, string name)
		{
			ProbeReferenceVolumeProfile probeReferenceVolumeProfile = ScriptableObject.CreateInstance<ProbeReferenceVolumeProfile>();
			set.name = probeReferenceVolumeProfile.name;
			set.profile = probeReferenceVolumeProfile;
			set.settings = ProbeVolumeBakingProcessSettings.Default;
			this.InitializeScenarios(set);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00018B6C File Offset: 0x00016D6C
		private void InitializeScenarios(ProbeVolumeSceneData.BakingSet set)
		{
			set.lightingScenarios = new List<string> { ProbeReferenceVolume.defaultLightingScenario };
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00018B84 File Offset: 0x00016D84
		internal void SyncBakingSetSettings()
		{
			foreach (ProbeVolumeSceneData.BakingSet bakingSet in this.bakingSets)
			{
				foreach (string text in bakingSet.sceneGUIDs)
				{
					this.sceneBakingSettings[text] = bakingSet.settings;
					this.sceneProfiles[text] = bakingSet.profile;
				}
			}
		}

		// Token: 0x0400033E RID: 830
		private static PropertyInfo s_SceneGUID = typeof(Scene).GetProperty("guid", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x0400033F RID: 831
		[SerializeField]
		private List<ProbeVolumeSceneData.SerializableBoundItem> serializedBounds;

		// Token: 0x04000340 RID: 832
		[SerializeField]
		private List<ProbeVolumeSceneData.SerializableHasPVItem> serializedHasVolumes;

		// Token: 0x04000341 RID: 833
		[SerializeField]
		private List<ProbeVolumeSceneData.SerializablePVProfile> serializedProfiles;

		// Token: 0x04000342 RID: 834
		[SerializeField]
		private List<ProbeVolumeSceneData.SerializablePVBakeSettings> serializedBakeSettings;

		// Token: 0x04000343 RID: 835
		[SerializeField]
		private List<ProbeVolumeSceneData.BakingSet> serializedBakingSets;

		// Token: 0x04000344 RID: 836
		internal Object parentAsset;

		// Token: 0x04000345 RID: 837
		internal string parentSceneDataPropertyName;

		// Token: 0x04000346 RID: 838
		public Dictionary<string, Bounds> sceneBounds;

		// Token: 0x04000347 RID: 839
		internal Dictionary<string, bool> hasProbeVolumes;

		// Token: 0x04000348 RID: 840
		internal Dictionary<string, ProbeReferenceVolumeProfile> sceneProfiles;

		// Token: 0x04000349 RID: 841
		internal Dictionary<string, ProbeVolumeBakingProcessSettings> sceneBakingSettings;

		// Token: 0x0400034A RID: 842
		internal List<ProbeVolumeSceneData.BakingSet> bakingSets;

		// Token: 0x0400034B RID: 843
		[SerializeField]
		private string m_LightingScenario = ProbeReferenceVolume.defaultLightingScenario;

		// Token: 0x0400034C RID: 844
		private string m_OtherScenario;

		// Token: 0x0400034D RID: 845
		private float m_ScenarioBlendingFactor;

		// Token: 0x020001B0 RID: 432
		[Serializable]
		private struct SerializableBoundItem
		{
			// Token: 0x04000723 RID: 1827
			[SerializeField]
			public string sceneGUID;

			// Token: 0x04000724 RID: 1828
			[SerializeField]
			public Bounds bounds;
		}

		// Token: 0x020001B1 RID: 433
		[Serializable]
		private struct SerializableHasPVItem
		{
			// Token: 0x04000725 RID: 1829
			[SerializeField]
			public string sceneGUID;

			// Token: 0x04000726 RID: 1830
			[SerializeField]
			public bool hasProbeVolumes;
		}

		// Token: 0x020001B2 RID: 434
		[Serializable]
		private struct SerializablePVProfile
		{
			// Token: 0x04000727 RID: 1831
			[SerializeField]
			public string sceneGUID;

			// Token: 0x04000728 RID: 1832
			[SerializeField]
			public ProbeReferenceVolumeProfile profile;
		}

		// Token: 0x020001B3 RID: 435
		[Serializable]
		private struct SerializablePVBakeSettings
		{
			// Token: 0x04000729 RID: 1833
			public string sceneGUID;

			// Token: 0x0400072A RID: 1834
			public ProbeVolumeBakingProcessSettings settings;
		}

		// Token: 0x020001B4 RID: 436
		[Serializable]
		internal class BakingSet
		{
			// Token: 0x06000B37 RID: 2871 RVA: 0x0002F2FC File Offset: 0x0002D4FC
			internal string CreateScenario(string name)
			{
				if (this.lightingScenarios.Contains(name))
				{
					int num = 1;
					string text;
					do
					{
						text = string.Format("{0} ({1})", name, num++);
					}
					while (this.lightingScenarios.Contains(text));
					name = text;
				}
				this.lightingScenarios.Add(name);
				return name;
			}

			// Token: 0x06000B38 RID: 2872 RVA: 0x0002F34D File Offset: 0x0002D54D
			internal bool RemoveScenario(string name)
			{
				return this.lightingScenarios.Remove(name);
			}

			// Token: 0x0400072B RID: 1835
			public string name;

			// Token: 0x0400072C RID: 1836
			public List<string> sceneGUIDs = new List<string>();

			// Token: 0x0400072D RID: 1837
			public ProbeVolumeBakingProcessSettings settings;

			// Token: 0x0400072E RID: 1838
			public ProbeReferenceVolumeProfile profile;

			// Token: 0x0400072F RID: 1839
			public List<string> lightingScenarios = new List<string>();
		}
	}
}
