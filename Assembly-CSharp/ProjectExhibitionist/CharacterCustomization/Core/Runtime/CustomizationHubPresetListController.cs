using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Bozo.AnimeCharacters;
using BoZo.CharacterCreator;
using BoZo.CharacterCreator.MC;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Detection;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using ProjectExhibitionist.CharacterCustomization.Core.Storage;
using ProjectExhibitionist.Futanari;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AF4 RID: 2804
	public class CustomizationHubPresetListController : MonoBehaviour
	{
		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06004F4D RID: 20301 RVA: 0x001EB51B File Offset: 0x001E971B
		public CustomizationPresetData SelectedPreset
		{
			get
			{
				return this._selectedPreset;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06004F4E RID: 20302 RVA: 0x001EB523 File Offset: 0x001E9723
		public IReadOnlyList<CustomizationPresetData> CurrentPresets
		{
			get
			{
				return this._currentPresets;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06004F4F RID: 20303 RVA: 0x001EB52B File Offset: 0x001E972B
		public CustomizationTargetType CurrentTargetType
		{
			get
			{
				return this._currentTargetType;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06004F50 RID: 20304 RVA: 0x001EB533 File Offset: 0x001E9733
		public string CurrentCharacterId
		{
			get
			{
				return this._currentCharacterId;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06004F51 RID: 20305 RVA: 0x001EB53B File Offset: 0x001E973B
		public CustomizationSystemPresetConfig SystemPresetConfig
		{
			get
			{
				return this.systemPresetConfig;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06004F52 RID: 20306 RVA: 0x001EB543 File Offset: 0x001E9743
		public bool IsPreviewLoading
		{
			get
			{
				return this._previewLoadOperations > 0;
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06004F53 RID: 20307 RVA: 0x001EB54E File Offset: 0x001E974E
		public CharactersManager CharactersManager
		{
			get
			{
				return this.charactersManager;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06004F54 RID: 20308 RVA: 0x001EB556 File Offset: 0x001E9756
		public GameObject NpcPreviewTarget
		{
			get
			{
				if (!(this.npcSpawnProvider != null))
				{
					return null;
				}
				return this.npcSpawnProvider.PreSpawnedTarget;
			}
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x001EB574 File Offset: 0x001E9774
		public List<string> GetActiveNpcSystemPresetIds()
		{
			List<string> list = new List<string>();
			if (this._cachedNpcSystemEntries == null)
			{
				return list;
			}
			for (int i = 0; i < this._cachedNpcSystemEntries.Count; i++)
			{
				SystemPresetEntry systemPresetEntry = this._cachedNpcSystemEntries[i];
				if (systemPresetEntry != null && !(systemPresetEntry.sourceAsset == null) && !systemPresetEntry.isInactive)
				{
					list.Add("sys_" + systemPresetEntry.sourceAsset.name);
				}
			}
			return list;
		}

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06004F56 RID: 20310 RVA: 0x001EB5EC File Offset: 0x001E97EC
		// (remove) Token: 0x06004F57 RID: 20311 RVA: 0x001EB624 File Offset: 0x001E9824
		public event Action<List<CustomizationPresetData>> OnPresetsLoaded;

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06004F58 RID: 20312 RVA: 0x001EB65C File Offset: 0x001E985C
		// (remove) Token: 0x06004F59 RID: 20313 RVA: 0x001EB694 File Offset: 0x001E9894
		public event Action<CustomizationPresetData> OnPresetSelected;

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06004F5A RID: 20314 RVA: 0x001EB6CC File Offset: 0x001E98CC
		// (remove) Token: 0x06004F5B RID: 20315 RVA: 0x001EB704 File Offset: 0x001E9904
		public event Action OnNewPresetDraftStarted;

		// Token: 0x06004F5C RID: 20316 RVA: 0x001EB739 File Offset: 0x001E9939
		private void Awake()
		{
			this.CacheSystemPresetEntries();
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x001EB741 File Offset: 0x001E9941
		private void OnEnable()
		{
			if (this.charactersManager != null)
			{
				CharactersManager charactersManager = this.charactersManager;
				charactersManager.OnPresetListUpdated = (Action<string, CustomizationTargetType, List<CustomizationPresetData>>)Delegate.Combine(charactersManager.OnPresetListUpdated, new Action<string, CustomizationTargetType, List<CustomizationPresetData>>(this.HandleCharactersManagerPresetListUpdated));
			}
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x001EB778 File Offset: 0x001E9978
		private void OnDisable()
		{
			if (this.charactersManager != null)
			{
				CharactersManager charactersManager = this.charactersManager;
				charactersManager.OnPresetListUpdated = (Action<string, CustomizationTargetType, List<CustomizationPresetData>>)Delegate.Remove(charactersManager.OnPresetListUpdated, new Action<string, CustomizationTargetType, List<CustomizationPresetData>>(this.HandleCharactersManagerPresetListUpdated));
			}
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x001EB7B0 File Offset: 0x001E99B0
		public void LoadPresetsForTarget(CustomizationTargetType targetType, string characterId = null, string preferredPresetId = null)
		{
			this._currentTargetType = targetType;
			this._currentCharacterId = this.ResolveCharacterId(targetType, characterId);
			if (targetType == CustomizationTargetType.None)
			{
				this._currentPresets.Clear();
				this._selectedPreset = null;
				this._previewRevision++;
				this.DeactivateAllPreviews();
				Action<List<CustomizationPresetData>> onPresetsLoaded = this.OnPresetsLoaded;
				if (onPresetsLoaded == null)
				{
					return;
				}
				onPresetsLoaded(this._currentPresets);
				return;
			}
			else
			{
				if (targetType == CustomizationTargetType.MainCharacter && this.charactersManager != null)
				{
					this.charactersManager.EnsureMainCharacterExists();
				}
				if (targetType == CustomizationTargetType.Npc && this.charactersManager != null)
				{
					this.charactersManager.EnsureCustomNpcOwnerExists();
				}
				this._currentPresets.Clear();
				List<CustomizationPresetData> list = this.LoadCustomPresets(targetType);
				this._currentPresets.AddRange(list);
				List<CustomizationPresetData> list2 = this.BuildSystemPresetDataList(targetType);
				this._currentPresets.AddRange(list2);
				CustomizationHubPresetListController.SortPresetsForDisplay(this._currentPresets);
				if (this.verboseLogs)
				{
					Debug.Log(string.Format("[HubPresetList] Loaded {0} custom + {1} system presets for {2}", list.Count, list2.Count, targetType));
				}
				Action<List<CustomizationPresetData>> onPresetsLoaded2 = this.OnPresetsLoaded;
				if (onPresetsLoaded2 != null)
				{
					onPresetsLoaded2(this._currentPresets);
				}
				CustomizationPresetData customizationPresetData = ((!string.IsNullOrWhiteSpace(preferredPresetId)) ? this.FindPresetById(preferredPresetId) : null);
				CustomizationPresetData customizationPresetData2 = CustomizationHubPresetListController.FindActivePreset(this._currentPresets);
				if (customizationPresetData != null)
				{
					this.SelectPreset(customizationPresetData.presetId, true, false);
					return;
				}
				if (customizationPresetData2 != null)
				{
					this.SelectPreset(customizationPresetData2.presetId, true, false);
					return;
				}
				if (this._currentPresets.Count > 0)
				{
					this.SelectPreset(this._currentPresets[0].presetId, true, false);
					return;
				}
				this._selectedPreset = null;
				this._previewRevision++;
				this.ActivatePreviewForTarget(targetType);
				return;
			}
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x001EB960 File Offset: 0x001E9B60
		public void RefreshCurrentPresets()
		{
			if (this._currentTargetType == CustomizationTargetType.None)
			{
				return;
			}
			this.LoadPresetsForTarget(this._currentTargetType, this._currentCharacterId, null);
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x001EB980 File Offset: 0x001E9B80
		public bool SelectPreset(string presetId, bool activatePreview = true, bool persistActive = true)
		{
			if (string.IsNullOrWhiteSpace(presetId))
			{
				return false;
			}
			CustomizationPresetData customizationPresetData = this.FindPresetById(presetId);
			if (customizationPresetData == null)
			{
				if (this.verboseLogs)
				{
					Debug.LogWarning("[HubPresetList] Preset not found: " + presetId);
				}
				return false;
			}
			this._selectedPreset = customizationPresetData;
			this._previewRevision++;
			bool flag = true;
			if (persistActive && this.presetCoordinator != null)
			{
				if (customizationPresetData.isSystemPreset && !this.EnsureSystemPresetPersisted(customizationPresetData))
				{
					flag = false;
				}
				if (flag)
				{
					flag = this.presetCoordinator.TrySetActivePresetForCharacter(this._currentCharacterId, this._currentTargetType, presetId);
				}
			}
			if (this.verboseLogs)
			{
				Debug.Log(string.Format("[HubPresetList] SelectPreset name='{0}' id={1} target={2} characterId='{3}' system={4} persistActive={5}", new object[] { customizationPresetData.presetName, customizationPresetData.presetId, this._currentTargetType, this._currentCharacterId, customizationPresetData.isSystemPreset, persistActive }));
			}
			if (activatePreview)
			{
				this.ActivatePreviewForTarget(this._currentTargetType);
				if (!this.IsPreviewLoading)
				{
					this.ApplyPresetDataToPreview(customizationPresetData);
				}
			}
			Action<CustomizationPresetData> onPresetSelected = this.OnPresetSelected;
			if (onPresetSelected != null)
			{
				onPresetSelected(this._selectedPreset);
			}
			return flag;
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x001480DB File Offset: 0x001462DB
		public bool CanDeletePreset(CustomizationPresetData preset)
		{
			return preset != null && !preset.isDefault && !preset.isSystemPreset;
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x001EBAA6 File Offset: 0x001E9CA6
		public bool TryApplySystemPresetAssetToPreview(CharacterObject systemPresetAsset, CustomizationTargetType targetType)
		{
			if (systemPresetAsset == null || systemPresetAsset.data == null)
			{
				return false;
			}
			this.TryApplySystemPresetAssetToPreviewAsync(systemPresetAsset, targetType);
			return true;
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x001EBAC8 File Offset: 0x001E9CC8
		public async Task<bool> TryApplySystemPresetAssetToPreviewAsync(CharacterObject systemPresetAsset, CustomizationTargetType targetType)
		{
			bool flag;
			if (systemPresetAsset == null || systemPresetAsset.data == null)
			{
				flag = false;
			}
			else
			{
				PrefabCustomizationSpawnProvider provider = ((targetType == CustomizationTargetType.MainCharacter) ? this.mainCharacterSpawnProvider : this.npcSpawnProvider);
				if (provider == null || provider.PreSpawnedTarget == null)
				{
					flag = false;
				}
				else
				{
					this._currentTargetType = targetType;
					this._selectedPreset = null;
					int num = this._previewRevision + 1;
					this._previewRevision = num;
					int revision = num;
					Action onNewPresetDraftStarted = this.OnNewPresetDraftStarted;
					if (onNewPresetDraftStarted != null)
					{
						onNewPresetDraftStarted();
					}
					while (this.IsPreviewLoading)
					{
						await Task.Yield();
					}
					if (revision != this._previewRevision)
					{
						flag = false;
					}
					else
					{
						this.ActivatePreviewForTarget(targetType);
						CustomizationHubPresetListController.ResetPreviewBlendShapes(provider.PreSpawnedTarget);
						if (targetType == CustomizationTargetType.MainCharacter)
						{
							this.ResetMainCharacterPreviewState(provider.PreSpawnedTarget, "DraftNewPreset");
							CustomizationHubPresetListController.ResetMainCharacterNewDraftVisualState(provider.PreSpawnedTarget);
						}
						OutfitSystem componentInChildren = provider.PreSpawnedTarget.GetComponentInChildren<OutfitSystem>(true);
						if (componentInChildren == null)
						{
							flag = false;
						}
						else
						{
							await this.LoadSystemPreviewAsync(componentInChildren, systemPresetAsset.data, provider.PreSpawnedTarget, null);
							if (revision != this._previewRevision)
							{
								flag = false;
							}
							else
							{
								if (targetType == CustomizationTargetType.MainCharacter)
								{
									CustomizationHubPresetListController.ResetMainCharacterNewDraftVisualState(provider.PreSpawnedTarget);
								}
								flag = true;
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x001EBB1C File Offset: 0x001E9D1C
		public void EnterCustomizationForSelectedPreset()
		{
			if (this.hubController == null || this._selectedPreset == null)
			{
				return;
			}
			if (this._selectedPreset.isSystemPreset && this._currentTargetType != CustomizationTargetType.Npc)
			{
				if (this.verboseLogs)
				{
					Debug.Log(string.Format("[HubPresetList] System preset selected (target={0}): customization entry blocked (preview-only). Duplicate to edit.", this._currentTargetType));
				}
				return;
			}
			string presetId = this._selectedPreset.presetId;
			if (this._currentTargetType == CustomizationTargetType.MainCharacter)
			{
				string text = ((!string.IsNullOrWhiteSpace(this._currentCharacterId)) ? this._currentCharacterId : ((this.systemPresetConfig != null) ? this.systemPresetConfig.MainCharacterId : "Ciel"));
				this.hubController.OpenMainCharacterCustomization(text, true, presetId, false);
				return;
			}
			if (this._currentTargetType == CustomizationTargetType.Npc)
			{
				this.hubController.OpenNpcCustomization("CustomNPC", false, presetId, false);
			}
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x001EBBF0 File Offset: 0x001E9DF0
		public bool TryDuplicateSystemNpcToCustom(string systemPresetId, string duplicatedName, out CustomizationPresetData createdCustom, out string error)
		{
			createdCustom = null;
			error = null;
			if (string.IsNullOrWhiteSpace(systemPresetId))
			{
				error = "System preset ID is missing.";
				return false;
			}
			if (this.charactersManager == null)
			{
				error = "CharactersManager not found.";
				return false;
			}
			SystemPresetEntry systemPresetEntry = this.FindSystemPresetEntry(systemPresetId);
			CharacterData characterData = ((systemPresetEntry != null && systemPresetEntry.sourceAsset != null) ? systemPresetEntry.sourceAsset.data : null);
			if (characterData == null)
			{
				error = "System NPC appearance not found.";
				return false;
			}
			Gender gender = ((systemPresetEntry.sourceAsset.data != null && systemPresetEntry.sourceAsset.data.generalInfo != null) ? systemPresetEntry.sourceAsset.data.generalInfo.gender : Gender.Male);
			this.charactersManager.EnsureCustomNpcOwnerExists();
			string text = "CustomNPC";
			if (!this.charactersManager.TryCreatePresetForCharacter(text, duplicatedName, out createdCustom, out error, null, null))
			{
				if (string.IsNullOrWhiteSpace(error))
				{
					error = "Failed to create the duplicate custom NPC.";
				}
				return false;
			}
			string presetId = createdCustom.presetId;
			string systemPresetIconFileName = CustomizationHubPresetListController.GetSystemPresetIconFileName(systemPresetId);
			CharacterData characterData2 = CustomizationHubPresetListController.CloneLook(characterData);
			if (characterData2 != null)
			{
				characterData2.stance = ((gender == Gender.Female) ? 1f : 0f);
			}
			NpcLookStore.Save(presetId, characterData2, duplicatedName, systemPresetIconFileName);
			CustomizationPresetData customizationPresetData;
			string text2;
			this.charactersManager.TryUpdatePresetForCharacter(text, presetId, duplicatedName, new CharacterStateData(), systemPresetIconFileName, out customizationPresetData, out text2);
			this.charactersManager.TryUpdatePresetGender(text, presetId, gender);
			this.LoadPresetsForTarget(CustomizationTargetType.Npc, null, presetId);
			return true;
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x001EBD4B File Offset: 0x001E9F4B
		private static CharacterData CloneLook(CharacterData source)
		{
			if (source == null)
			{
				return null;
			}
			return JsonUtility.FromJson<CharacterData>(JsonUtility.ToJson(source));
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x001EBD60 File Offset: 0x001E9F60
		private void CacheSystemPresetEntries()
		{
			if (this.systemPresetConfig == null)
			{
				this._cachedMcSystemEntries = new List<SystemPresetEntry>();
				this._cachedNpcSystemEntries = new List<SystemPresetEntry>();
				return;
			}
			SystemPresetEntry systemPresetEntry = this.systemPresetConfig.BuildMainCharacterSystemPreset();
			List<SystemPresetEntry> list;
			if (systemPresetEntry == null)
			{
				list = new List<SystemPresetEntry>();
			}
			else
			{
				(list = new List<SystemPresetEntry>()).Add(systemPresetEntry);
			}
			this._cachedMcSystemEntries = list;
			this._cachedNpcSystemEntries = this.systemPresetConfig.BuildNpcSystemPresets() ?? new List<SystemPresetEntry>();
			if (this.verboseLogs)
			{
				Debug.Log(string.Format("[HubPresetList] Cached {0} MC + {1} NPC system preset entries", this._cachedMcSystemEntries.Count, this._cachedNpcSystemEntries.Count));
			}
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x001EBE0C File Offset: 0x001EA00C
		private List<CustomizationPresetData> BuildSystemPresetDataList(CustomizationTargetType targetType)
		{
			List<SystemPresetEntry> list = ((targetType == CustomizationTargetType.MainCharacter) ? this._cachedMcSystemEntries : this._cachedNpcSystemEntries);
			List<CustomizationPresetData> list2 = null;
			if (this.presetCoordinator != null && !string.IsNullOrWhiteSpace(this._currentCharacterId))
			{
				list2 = this.presetCoordinator.GetPresets(this._currentCharacterId, targetType, null);
			}
			List<CustomizationPresetData> list3 = new List<CustomizationPresetData>();
			if (list == null)
			{
				return list3;
			}
			for (int i = 0; i < list.Count; i++)
			{
				SystemPresetEntry systemPresetEntry = list[i];
				if (systemPresetEntry != null && !(systemPresetEntry.sourceAsset == null) && !systemPresetEntry.isInactive)
				{
					string text = "sys_" + systemPresetEntry.sourceAsset.name;
					CustomizationPresetData customizationPresetData = CustomizationHubPresetListController.FindPersistedPresetById(list2, text);
					bool flag = customizationPresetData != null && (customizationPresetData.isDefault || customizationPresetData.isActive);
					NpcLookStore.NpcLookMeta npcLookMeta = ((targetType == CustomizationTargetType.Npc) ? NpcLookStore.LoadMeta(text) : null);
					string text2 = systemPresetEntry.presetName;
					if (npcLookMeta != null && !string.IsNullOrWhiteSpace(npcLookMeta.displayName))
					{
						text2 = npcLookMeta.displayName;
					}
					string text3;
					if (npcLookMeta != null && !string.IsNullOrWhiteSpace(npcLookMeta.iconFileName))
					{
						text3 = npcLookMeta.iconFileName;
					}
					else if (customizationPresetData != null && !string.IsNullOrWhiteSpace(customizationPresetData.iconFileName))
					{
						text3 = customizationPresetData.iconFileName;
					}
					else
					{
						text3 = CustomizationHubPresetListController.ResolveSystemPresetIconFileName(text);
					}
					Gender gender = ((systemPresetEntry.sourceAsset.data != null && systemPresetEntry.sourceAsset.data.generalInfo != null) ? systemPresetEntry.sourceAsset.data.generalInfo.gender : Gender.Male);
					CustomizationPresetData customizationPresetData2 = new CustomizationPresetData
					{
						presetId = text,
						presetName = text2,
						characterId = systemPresetEntry.characterId,
						targetType = systemPresetEntry.targetType,
						isDefault = flag,
						isSystemPreset = true,
						isActive = false,
						gender = gender,
						iconFileName = text3,
						stateData = new CharacterStateData()
					};
					list3.Add(customizationPresetData2);
				}
			}
			return list3;
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x001EC009 File Offset: 0x001EA209
		public static string GetSystemPresetIconFileName(string systemPresetId)
		{
			if (!string.IsNullOrWhiteSpace(systemPresetId))
			{
				return systemPresetId + ".png";
			}
			return null;
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x001EC020 File Offset: 0x001EA220
		private static string ResolveSystemPresetIconFileName(string systemPresetId)
		{
			string systemPresetIconFileName = CustomizationHubPresetListController.GetSystemPresetIconFileName(systemPresetId);
			if (string.IsNullOrWhiteSpace(systemPresetIconFileName))
			{
				return null;
			}
			if (File.Exists(Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, systemPresetIconFileName)))
			{
				return systemPresetIconFileName;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(systemPresetIconFileName);
			if (Resources.Load<Sprite>("NpcPresetIcons/" + fileNameWithoutExtension) != null)
			{
				return systemPresetIconFileName;
			}
			return null;
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x001EC074 File Offset: 0x001EA274
		public SystemPresetEntry FindSystemPresetEntry(string presetId)
		{
			if (string.IsNullOrWhiteSpace(presetId))
			{
				return null;
			}
			List<SystemPresetEntry> list = new List<SystemPresetEntry>();
			if (this._cachedMcSystemEntries != null)
			{
				list.AddRange(this._cachedMcSystemEntries);
			}
			if (this._cachedNpcSystemEntries != null)
			{
				list.AddRange(this._cachedNpcSystemEntries);
			}
			for (int i = 0; i < list.Count; i++)
			{
				SystemPresetEntry systemPresetEntry = list[i];
				if (systemPresetEntry != null && systemPresetEntry.sourceAsset != null && "sys_" + systemPresetEntry.sourceAsset.name == presetId)
				{
					return systemPresetEntry;
				}
			}
			return null;
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x001EC104 File Offset: 0x001EA304
		private List<CustomizationPresetData> LoadCustomPresets(CustomizationTargetType targetType)
		{
			if (this.presetCoordinator == null)
			{
				return new List<CustomizationPresetData>();
			}
			string currentCharacterId = this._currentCharacterId;
			if (string.IsNullOrWhiteSpace(currentCharacterId))
			{
				return new List<CustomizationPresetData>();
			}
			List<CustomizationPresetData> presets = this.presetCoordinator.GetPresets(currentCharacterId, targetType, null);
			List<CustomizationPresetData> list = new List<CustomizationPresetData>();
			for (int i = 0; i < presets.Count; i++)
			{
				if (presets[i] != null && !presets[i].isSystemPreset)
				{
					list.Add(presets[i]);
				}
			}
			return list;
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x001EC188 File Offset: 0x001EA388
		private void HandleCharactersManagerPresetListUpdated(string characterId, CustomizationTargetType targetType, List<CustomizationPresetData> presets)
		{
			if (targetType != this._currentTargetType)
			{
				return;
			}
			this._currentPresets.Clear();
			if (presets != null)
			{
				for (int i = 0; i < presets.Count; i++)
				{
					if (presets[i] != null && !presets[i].isSystemPreset)
					{
						this._currentPresets.Add(presets[i]);
					}
				}
			}
			List<CustomizationPresetData> list = this.BuildSystemPresetDataList(targetType);
			this._currentPresets.AddRange(list);
			CustomizationHubPresetListController.SortPresetsForDisplay(this._currentPresets);
			if (this._selectedPreset != null && this.FindPresetById(this._selectedPreset.presetId) == null)
			{
				CustomizationPresetData customizationPresetData = CustomizationHubPresetListController.FindActivePreset(this._currentPresets);
				this._selectedPreset = customizationPresetData ?? ((this._currentPresets.Count > 0) ? this._currentPresets[0] : null);
			}
			Action<List<CustomizationPresetData>> onPresetsLoaded = this.OnPresetsLoaded;
			if (onPresetsLoaded == null)
			{
				return;
			}
			onPresetsLoaded(this._currentPresets);
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x001EC26C File Offset: 0x001EA46C
		private void ActivatePreviewForTarget(CustomizationTargetType targetType)
		{
			if (targetType == CustomizationTargetType.MainCharacter)
			{
				CustomizationHubPresetListController.DeactivatePreview(this.npcSpawnProvider);
				CustomizationHubPresetListController.ActivatePreview(this.mainCharacterSpawnProvider);
				this.SyncHubSpinner(this.mainCharacterSpawnProvider);
				this.LogPreviewHairState("ActivatePreviewForTarget.MainCharacter", (this.mainCharacterSpawnProvider != null) ? this.mainCharacterSpawnProvider.PreSpawnedTarget : null, this._selectedPreset);
				return;
			}
			if (targetType == CustomizationTargetType.Npc)
			{
				CustomizationHubPresetListController.DeactivatePreview(this.mainCharacterSpawnProvider);
				CustomizationHubPresetListController.ActivatePreview(this.npcSpawnProvider);
				this.SyncHubSpinner(this.npcSpawnProvider);
				this.LogPreviewHairState("ActivatePreviewForTarget.Npc", (this.npcSpawnProvider != null) ? this.npcSpawnProvider.PreSpawnedTarget : null, this._selectedPreset);
			}
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x001EC320 File Offset: 0x001EA520
		private void SyncHubSpinner(PrefabCustomizationSpawnProvider provider)
		{
			if (this.hubSpinner == null)
			{
				return;
			}
			if (provider != null && provider.PreSpawnedTarget != null)
			{
				this.hubSpinner.SetCharacter(provider.PreSpawnedTarget.transform);
				this.hubSpinner.gameObject.SetActive(true);
				return;
			}
			this.hubSpinner.gameObject.SetActive(false);
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x001EC38C File Offset: 0x001EA58C
		private void DeactivateAllPreviews()
		{
			CustomizationHubPresetListController.DeactivatePreview(this.mainCharacterSpawnProvider);
			CustomizationHubPresetListController.DeactivatePreview(this.npcSpawnProvider);
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x001EC3A4 File Offset: 0x001EA5A4
		private static void ActivatePreview(PrefabCustomizationSpawnProvider provider)
		{
			if (provider == null || !provider.HasValidPrefab)
			{
				return;
			}
			CustomizationSessionContext customizationSessionContext = new CustomizationSessionContext
			{
				targetType = provider.TargetType
			};
			GameObject gameObject = provider.Spawn(customizationSessionContext, null);
			if (gameObject != null)
			{
				Animator componentInChildren = gameObject.GetComponentInChildren<Animator>(true);
				if (componentInChildren != null && componentInChildren.isActiveAndEnabled)
				{
					componentInChildren.Update(0f);
				}
			}
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x001EC40B File Offset: 0x001EA60B
		private static void DeactivatePreview(PrefabCustomizationSpawnProvider provider)
		{
			if (provider == null)
			{
				return;
			}
			provider.DeactivatePreview();
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x001EC420 File Offset: 0x001EA620
		private void ApplyPresetDataToPreview(CustomizationPresetData preset)
		{
			if (preset == null)
			{
				return;
			}
			if (!preset.isSystemPreset)
			{
				if (this._currentTargetType == CustomizationTargetType.Npc)
				{
					this.ApplyCustomNpcLookToPreview(preset);
					return;
				}
				this.ApplyCustomPresetToPreview(preset);
				return;
			}
			else
			{
				SystemPresetEntry systemPresetEntry = this.FindSystemPresetEntry(preset.presetId);
				if (systemPresetEntry == null || systemPresetEntry.sourceAsset == null)
				{
					return;
				}
				PrefabCustomizationSpawnProvider prefabCustomizationSpawnProvider = ((this._currentTargetType == CustomizationTargetType.MainCharacter) ? this.mainCharacterSpawnProvider : this.npcSpawnProvider);
				if (prefabCustomizationSpawnProvider == null || prefabCustomizationSpawnProvider.PreSpawnedTarget == null)
				{
					return;
				}
				CustomizationHubPresetListController.ResetPreviewBlendShapes(prefabCustomizationSpawnProvider.PreSpawnedTarget);
				if (this._currentTargetType == CustomizationTargetType.MainCharacter)
				{
					this.ResetMainCharacterPreviewState(prefabCustomizationSpawnProvider.PreSpawnedTarget, "SystemPreset");
					CustomizationHubPresetListController.ResetMainCharacterVisualDefaults(prefabCustomizationSpawnProvider.PreSpawnedTarget, true);
					SkinDecalController componentInChildren = prefabCustomizationSpawnProvider.PreSpawnedTarget.GetComponentInChildren<SkinDecalController>(true);
					if (componentInChildren != null)
					{
						componentInChildren.ResetToStartupDefaults();
					}
					CustomizationHubPresetListController.SyncAnimatorLayersForClothingState(prefabCustomizationSpawnProvider.PreSpawnedTarget);
					CustomizationHubPresetListController.EnsureAnimatorEvaluated(prefabCustomizationSpawnProvider.PreSpawnedTarget);
				}
				OutfitSystem componentInChildren2 = prefabCustomizationSpawnProvider.PreSpawnedTarget.GetComponentInChildren<OutfitSystem>(true);
				if (componentInChildren2 == null)
				{
					if (this.verboseLogs)
					{
						Debug.LogWarning(string.Format("[HubPresetList] No OutfitSystem found on {0} preview target for system preset apply.", this._currentTargetType));
					}
					return;
				}
				CharacterData data = systemPresetEntry.sourceAsset.data;
				if (this.verboseLogs)
				{
					Debug.Log(string.Format("[HubPresetList] Loading system preset '{0}' onto {1} ", preset.presetName, this._currentTargetType) + "preview via BMAC_SaveSystem (source appearance)");
				}
				this.LoadSystemPreviewAsync(componentInChildren2, data, prefabCustomizationSpawnProvider.PreSpawnedTarget, preset);
				return;
			}
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x001EC588 File Offset: 0x001EA788
		private void ApplyCustomNpcLookToPreview(CustomizationPresetData preset)
		{
			if (preset == null || this.npcSpawnProvider == null || this.npcSpawnProvider.PreSpawnedTarget == null)
			{
				return;
			}
			CustomizationHubPresetListController.ResetPreviewBlendShapes(this.npcSpawnProvider.PreSpawnedTarget);
			OutfitSystem componentInChildren = this.npcSpawnProvider.PreSpawnedTarget.GetComponentInChildren<OutfitSystem>(true);
			if (componentInChildren == null)
			{
				if (this.verboseLogs)
				{
					Debug.LogWarning("[HubPresetList] No OutfitSystem on NPC preview target for custom NPC apply.");
				}
				return;
			}
			CharacterData characterData = NpcLookStore.LoadLook(preset.presetId);
			if (characterData == null)
			{
				if (this.verboseLogs)
				{
					Debug.LogWarning(string.Concat(new string[] { "[HubPresetList] Custom NPC '", preset.presetName, "' (id=", preset.presetId, ") has no saved look on disk." }));
				}
				return;
			}
			if (this.verboseLogs)
			{
				Debug.Log(string.Concat(new string[] { "[HubPresetList] Loading CUSTOM NPC '", preset.presetName, "' (id=", preset.presetId, ") onto preview via BMAC_SaveSystem." }));
			}
			this.LoadSystemPreviewAsync(componentInChildren, characterData, this.npcSpawnProvider.PreSpawnedTarget, preset);
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x001EC6A4 File Offset: 0x001EA8A4
		private void ResetMainCharacterPreviewState(GameObject previewRoot, string context)
		{
			if (previewRoot == null)
			{
				return;
			}
			this.LogPreviewHairState("ResetMainCharacterPreviewState.Before." + context, previewRoot, this._selectedPreset);
			SkinDecalController componentInChildren = previewRoot.GetComponentInChildren<SkinDecalController>(true);
			if (componentInChildren != null)
			{
				componentInChildren.DeactivateAllDecals();
			}
			this.LogPreviewHairState("ResetMainCharacterPreviewState.After." + context, previewRoot, this._selectedPreset);
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x001EC704 File Offset: 0x001EA904
		private static void ResetMainCharacterVisualDefaults(GameObject previewRoot, bool systemPreset = false)
		{
			if (previewRoot == null)
			{
				return;
			}
			MCStyleCustomizationService componentInChildren = previewRoot.GetComponentInChildren<MCStyleCustomizationService>(true);
			if (componentInChildren != null)
			{
				componentInChildren.SetCustomizationPreview(false);
				if (systemPreset)
				{
					componentInChildren.ResetForNewPresetDraft();
				}
				else
				{
					componentInChildren.ResetAllPartsToDefaults();
					componentInChildren.SetCustomizationPreview(true);
				}
			}
			MCColorCustomizationService componentInChildren2 = previewRoot.GetComponentInChildren<MCColorCustomizationService>(true);
			if (componentInChildren2 != null)
			{
				componentInChildren2.ApplyDefaultPresetColors();
			}
			FutanariModeController componentInChildren3 = previewRoot.GetComponentInChildren<FutanariModeController>(true);
			if (componentInChildren3 != null)
			{
				componentInChildren3.SetFutanariMode(false);
			}
			CustomizationHubPresetListController.SyncBakedDefaultLookVisibility(previewRoot);
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x001EC780 File Offset: 0x001EA980
		private static void ResetMainCharacterNewDraftVisualState(GameObject previewRoot)
		{
			if (previewRoot == null)
			{
				return;
			}
			MCStyleCustomizationService componentInChildren = previewRoot.GetComponentInChildren<MCStyleCustomizationService>(true);
			if (componentInChildren != null)
			{
				componentInChildren.ResetForNewPresetDraft();
				componentInChildren.SetCustomizationPreview(true);
			}
			MCColorCustomizationService componentInChildren2 = previewRoot.GetComponentInChildren<MCColorCustomizationService>(true);
			if (componentInChildren2 != null)
			{
				componentInChildren2.ApplyDefaultPresetColors();
			}
			FutanariModeController componentInChildren3 = previewRoot.GetComponentInChildren<FutanariModeController>(true);
			if (componentInChildren3 != null)
			{
				componentInChildren3.SetFutanariMode(false);
			}
			CustomizationHubPresetListController.SyncBakedDefaultLookVisibility(previewRoot);
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x001EC7EC File Offset: 0x001EA9EC
		private static void SyncBakedDefaultLookVisibility(GameObject previewRoot)
		{
			if (previewRoot == null)
			{
				return;
			}
			MainCharacterAppearanceApplier componentInChildren = previewRoot.GetComponentInChildren<MainCharacterAppearanceApplier>(true);
			if (componentInChildren != null)
			{
				componentInChildren.SyncDefaultLookVisibility();
			}
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x001EC81C File Offset: 0x001EAA1C
		private void ApplyCustomPresetToPreview(CustomizationPresetData preset)
		{
			if (preset == null || preset.stateData == null)
			{
				return;
			}
			PrefabCustomizationSpawnProvider prefabCustomizationSpawnProvider = ((this._currentTargetType == CustomizationTargetType.MainCharacter) ? this.mainCharacterSpawnProvider : this.npcSpawnProvider);
			if (prefabCustomizationSpawnProvider == null || prefabCustomizationSpawnProvider.PreSpawnedTarget == null)
			{
				return;
			}
			if (this._currentTargetType == CustomizationTargetType.MainCharacter)
			{
				this.ResetMainCharacterPreviewState(prefabCustomizationSpawnProvider.PreSpawnedTarget, "CustomPreset");
				CustomizationHubPresetListController.ResetMainCharacterVisualDefaults(prefabCustomizationSpawnProvider.PreSpawnedTarget, false);
			}
			CustomizationHubPresetListController.ResetPreviewBlendShapes(prefabCustomizationSpawnProvider.PreSpawnedTarget);
			CharacterBlendShapeController componentInChildren = prefabCustomizationSpawnProvider.PreSpawnedTarget.GetComponentInChildren<CharacterBlendShapeController>(true);
			if (componentInChildren != null && componentInChildren.BlendShapeMap != null)
			{
				CustomizationHubPresetListController.ApplyBlendShapesToController(componentInChildren, preset.stateData.blendShapes);
			}
			if (componentInChildren == null || componentInChildren.BlendShapeMap == null)
			{
				OutfitSystem componentInChildren2 = prefabCustomizationSpawnProvider.PreSpawnedTarget.GetComponentInChildren<OutfitSystem>(true);
				if (componentInChildren2 != null && preset.stateData.blendShapes != null)
				{
					for (int i = 0; i < preset.stateData.blendShapes.Count; i++)
					{
						BlendShapeValue blendShapeValue = preset.stateData.blendShapes[i];
						if (blendShapeValue != null && !string.IsNullOrWhiteSpace(blendShapeValue.blendShapeName))
						{
							componentInChildren2.SetShape(blendShapeValue.blendShapeName, blendShapeValue.value);
						}
					}
				}
			}
			MCColorCustomizationService componentInChildren3 = prefabCustomizationSpawnProvider.PreSpawnedTarget.GetComponentInChildren<MCColorCustomizationService>(true);
			if (componentInChildren3 != null)
			{
				componentInChildren3.ApplyState(preset.stateData);
			}
			this.ApplySkinDecalPreview(prefabCustomizationSpawnProvider.PreSpawnedTarget, preset.stateData);
			MCStyleCustomizationService componentInChildren4 = prefabCustomizationSpawnProvider.PreSpawnedTarget.GetComponentInChildren<MCStyleCustomizationService>(true);
			if (componentInChildren4 != null)
			{
				componentInChildren4.ApplyState(preset.stateData, true);
			}
			CustomizationHubPresetListController.SyncBakedDefaultLookVisibility(prefabCustomizationSpawnProvider.PreSpawnedTarget);
			CustomizationHubPresetListController.ApplyClothingTogglesPreview(prefabCustomizationSpawnProvider.PreSpawnedTarget, preset.stateData);
			if (componentInChildren4 != null)
			{
				componentInChildren4.EnforceCustomizationWardrobe();
			}
			CustomizationHubPresetListController.SyncAnimatorLayersForClothingState(prefabCustomizationSpawnProvider.PreSpawnedTarget);
			CustomizationHubPresetListController.EnsureAnimatorEvaluated(prefabCustomizationSpawnProvider.PreSpawnedTarget);
			this.LogPreviewHairState("ApplyCustomPresetToPreview.AfterEnsureAnimator", prefabCustomizationSpawnProvider.PreSpawnedTarget, preset);
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x001ECA0C File Offset: 0x001EAC0C
		private static void ResetPreviewBlendShapes(GameObject previewRoot)
		{
			if (previewRoot == null)
			{
				return;
			}
			CharacterBlendShapeController componentInChildren = previewRoot.GetComponentInChildren<CharacterBlendShapeController>(true);
			if (componentInChildren == null || componentInChildren.BlendShapeMap == null)
			{
				return;
			}
			componentInChildren.RebindRenderersAndRecache();
			List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = componentInChildren.BlendShapeMap.blendShapeMappings;
			if (blendShapeMappings == null)
			{
				return;
			}
			for (int i = 0; i < blendShapeMappings.Count; i++)
			{
				BlendShapeMapSO.BlendShapeEntry blendShapeEntry = blendShapeMappings[i];
				if (blendShapeEntry != null)
				{
					componentInChildren.ApplyEntryValue(blendShapeEntry, blendShapeEntry.GetUiDefaultValue());
				}
			}
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x001ECA84 File Offset: 0x001EAC84
		private static void ApplyBlendShapesToController(CharacterBlendShapeController controller, List<BlendShapeValue> blendShapes)
		{
			if (controller == null || controller.BlendShapeMap == null || blendShapes == null)
			{
				return;
			}
			List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = controller.BlendShapeMap.blendShapeMappings;
			if (blendShapeMappings == null)
			{
				return;
			}
			for (int i = 0; i < blendShapes.Count; i++)
			{
				BlendShapeValue blendShapeValue = blendShapes[i];
				if (blendShapeValue != null && !string.IsNullOrWhiteSpace(blendShapeValue.blendShapeName))
				{
					BlendShapeMapSO.BlendShapeEntry blendShapeEntry = CustomizationHubPresetListController.FindBlendShapeEntry(blendShapeMappings, blendShapeValue);
					if (blendShapeEntry != null)
					{
						controller.ApplyEntryValue(blendShapeEntry, blendShapeValue.value);
					}
					else
					{
						controller.SetBlendShapeValue(blendShapeValue.blendShapeName, blendShapeValue.value);
					}
				}
			}
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x001ECB14 File Offset: 0x001EAD14
		private static BlendShapeMapSO.BlendShapeEntry FindBlendShapeEntry(List<BlendShapeMapSO.BlendShapeEntry> mappings, BlendShapeValue blendShape)
		{
			for (int i = 0; i < mappings.Count; i++)
			{
				BlendShapeMapSO.BlendShapeEntry blendShapeEntry = mappings[i];
				if (blendShapeEntry != null && string.Equals(blendShapeEntry.displayName, blendShape.blendShapeName, StringComparison.OrdinalIgnoreCase) && (string.IsNullOrWhiteSpace(blendShape.category) || string.Equals(blendShapeEntry.category, blendShape.category, StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrWhiteSpace(blendShape.subcategory) || string.Equals(blendShapeEntry.subcategory, blendShape.subcategory, StringComparison.OrdinalIgnoreCase)))
				{
					return blendShapeEntry;
				}
			}
			return null;
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x001ECB9C File Offset: 0x001EAD9C
		private static void ApplyClothingTogglesPreview(GameObject previewRoot, CharacterStateData stateData)
		{
			if (previewRoot == null || stateData == null || stateData.clothingToggles == null || stateData.clothingToggles.Count == 0)
			{
				return;
			}
			ClothingToggleController componentInChildren = previewRoot.GetComponentInChildren<ClothingToggleController>(true);
			if (componentInChildren == null)
			{
				return;
			}
			for (int i = 0; i < stateData.clothingToggles.Count; i++)
			{
				ClothingToggleValue clothingToggleValue = stateData.clothingToggles[i];
				if (clothingToggleValue != null && !string.IsNullOrWhiteSpace(clothingToggleValue.groupId))
				{
					componentInChildren.SetGroupState(clothingToggleValue.groupId, clothingToggleValue.isEnabled);
				}
			}
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x001ECC24 File Offset: 0x001EAE24
		private void ApplySkinDecalPreview(GameObject previewRoot, CharacterStateData stateData)
		{
			if (previewRoot == null || stateData == null)
			{
				return;
			}
			SkinDecalController componentInChildren = previewRoot.GetComponentInChildren<SkinDecalController>(true);
			if (componentInChildren == null)
			{
				return;
			}
			if (stateData.skinDecals == null || stateData.skinDecals.Count == 0)
			{
				if (this.diagnosticDecalLogs)
				{
					Debug.Log("[HubDecalDiag] ApplySkinDecalPreview: no saved active decals in stateData.");
				}
				return;
			}
			int num = 0;
			int num2 = 0;
			List<string> list = (this.diagnosticDecalLogs ? new List<string>() : null);
			for (int i = 0; i < stateData.skinDecals.Count; i++)
			{
				SkinDecalStateValue skinDecalStateValue = stateData.skinDecals[i];
				if (skinDecalStateValue != null && !string.IsNullOrWhiteSpace(skinDecalStateValue.decalId))
				{
					if (componentInChildren.TrySetDecalStateByName(skinDecalStateValue.decalId, skinDecalStateValue.isEnabled, skinDecalStateValue.opacity, skinDecalStateValue.tint))
					{
						num++;
					}
					else
					{
						num2++;
						if (list != null)
						{
							list.Add(skinDecalStateValue.decalId);
						}
					}
				}
			}
			if (this.diagnosticDecalLogs || num2 > 0)
			{
				SkinDecalSaveData saveData = componentInChildren.GetSaveData(false);
				int num3 = 0;
				int num4 = 0;
				if (saveData != null && saveData.entries != null)
				{
					num4 = saveData.entries.Count;
					for (int j = 0; j < saveData.entries.Count; j++)
					{
						if (saveData.entries[j] != null && saveData.entries[j].isActive)
						{
							num3++;
						}
					}
				}
				string text = ((list != null && list.Count > 0) ? string.Join(", ", list) : "none");
				Debug.Log(string.Format("[HubDecalDiag] ApplySkinDecalPreview requested={0}, applied={1}, missing={2}, controllerActive={3}/{4}, missingIds=[{5}]", new object[]
				{
					stateData.skinDecals.Count,
					num,
					num2,
					num3,
					num4,
					text
				}));
			}
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x001ECDF8 File Offset: 0x001EAFF8
		private static void EnsureAnimatorEvaluated(GameObject previewRoot)
		{
			if (previewRoot == null)
			{
				return;
			}
			Animator componentInChildren = previewRoot.GetComponentInChildren<Animator>(true);
			if (componentInChildren != null)
			{
				if (!componentInChildren.enabled)
				{
					componentInChildren.enabled = true;
				}
				if (componentInChildren.isActiveAndEnabled)
				{
					PlayerEmotionController componentInChildren2 = previewRoot.GetComponentInChildren<PlayerEmotionController>(true);
					if (componentInChildren2 != null)
					{
						componentInChildren2.ResetToNeutral();
					}
					componentInChildren.Update(0f);
				}
			}
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x001ECE5C File Offset: 0x001EB05C
		private static void SyncAnimatorLayersForClothingState(GameObject previewRoot)
		{
			if (previewRoot == null)
			{
				return;
			}
			PlayerEmotionController componentInChildren = previewRoot.GetComponentInChildren<PlayerEmotionController>(true);
			if (componentInChildren != null)
			{
				componentInChildren.ForceLayerWeightUpdate();
			}
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x001ECE8C File Offset: 0x001EB08C
		private async Task LoadSystemPreviewAsync(OutfitSystem outfitSystem, CharacterData data, GameObject previewRoot, CustomizationPresetData preset)
		{
			if (!(outfitSystem == null) && data != null)
			{
				int revision = this._previewRevision;
				this._previewLoadOperations++;
				try
				{
					this.LogPreviewHairState("LoadSystemPreviewAsync.BeforeLoad", previewRoot, preset);
					await BMAC_SaveSystem.LoadCharacter(outfitSystem, data, false, false);
					if (revision == this._previewRevision)
					{
						CustomizationHubPresetListController.SyncAnimatorLayersForClothingState(previewRoot);
						CustomizationHubPresetListController.EnsureAnimatorEvaluated(previewRoot);
						this.LogPreviewHairState("LoadSystemPreviewAsync.AfterEnsureAnimator", previewRoot, preset);
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[HubPresetList] Failed to load system preset preview: " + ex.Message);
				}
				finally
				{
					this._previewLoadOperations = Mathf.Max(0, this._previewLoadOperations - 1);
					if (this._previewLoadOperations == 0 && revision != this._previewRevision && this._selectedPreset != null)
					{
						this.ApplyPresetDataToPreview(this._selectedPreset);
					}
				}
			}
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x001ECEF0 File Offset: 0x001EB0F0
		private void LogPreviewHairState(string stage, GameObject previewRoot, CustomizationPresetData preset)
		{
			if (!this.diagnosticHairLogs || previewRoot == null)
			{
				return;
			}
			Animator componentInChildren = previewRoot.GetComponentInChildren<Animator>(true);
			Transform[] componentsInChildren = previewRoot.GetComponentsInChildren<Transform>(true);
			Transform transform = null;
			foreach (Transform transform2 in componentsInChildren)
			{
				if (transform2 != null && string.Equals(transform2.name, "Hair", StringComparison.OrdinalIgnoreCase))
				{
					transform = transform2;
					break;
				}
			}
			SkinDecalController componentInChildren2 = previewRoot.GetComponentInChildren<SkinDecalController>(true);
			int num = 0;
			int num2 = 0;
			if (componentInChildren2 != null)
			{
				SkinDecalSaveData saveData = componentInChildren2.GetSaveData(false);
				if (saveData != null && saveData.entries != null)
				{
					num2 = saveData.entries.Count;
					for (int j = 0; j < saveData.entries.Count; j++)
					{
						if (saveData.entries[j] != null && saveData.entries[j].isActive)
						{
							num++;
						}
					}
				}
			}
			string text = ((transform != null) ? transform.position.ToString("F4") : "<missing>");
			string text2 = ((transform != null) ? transform.localPosition.ToString("F4") : "<missing>");
			string text3 = ((transform != null && transform.parent != null) ? transform.parent.name : "<none>");
			string text4 = ((preset != null) ? preset.presetName : "<none>");
			Debug.Log(string.Format("[HubHairDiag] stage={0}, target={1}, preset={2}, animatorEnabled={3}, animatorActive={4}, hairParent={5}, hairPos={6}, hairLocalPos={7}, decalsActive={8}/{9}", new object[]
			{
				stage,
				this._currentTargetType,
				text4,
				componentInChildren != null && componentInChildren.enabled,
				componentInChildren != null && componentInChildren.isActiveAndEnabled,
				text3,
				text,
				text2,
				num,
				num2
			}));
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x001ED0EC File Offset: 0x001EB2EC
		private string ResolveCharacterId(CustomizationTargetType targetType, string explicitId)
		{
			if (!string.IsNullOrWhiteSpace(explicitId))
			{
				return explicitId;
			}
			if (targetType == CustomizationTargetType.MainCharacter)
			{
				if (this.charactersManager != null)
				{
					CharacterDataExtended mainCharacter = this.charactersManager.GetMainCharacter();
					if (mainCharacter != null)
					{
						return mainCharacter.characterId;
					}
				}
				if (this.systemPresetConfig != null)
				{
					return this.systemPresetConfig.MainCharacterId;
				}
			}
			if (targetType != CustomizationTargetType.MainCharacter)
			{
				return "CustomNPC";
			}
			return "Ciel";
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x001ED154 File Offset: 0x001EB354
		private CustomizationPresetData FindPresetById(string presetId)
		{
			if (string.IsNullOrWhiteSpace(presetId))
			{
				return null;
			}
			for (int i = 0; i < this._currentPresets.Count; i++)
			{
				if (this._currentPresets[i] != null && this._currentPresets[i].presetId == presetId)
				{
					return this._currentPresets[i];
				}
			}
			return null;
		}

		// Token: 0x06004F86 RID: 20358 RVA: 0x001ED1B8 File Offset: 0x001EB3B8
		private bool EnsureSystemPresetPersisted(CustomizationPresetData preset)
		{
			if (preset == null || !preset.isSystemPreset || this.presetCoordinator == null)
			{
				return false;
			}
			string text;
			if (this._currentTargetType == CustomizationTargetType.MainCharacter)
			{
				text = this.ResolveCharacterId(CustomizationTargetType.MainCharacter, null);
			}
			else
			{
				text = ((!string.IsNullOrWhiteSpace(this._currentCharacterId)) ? this._currentCharacterId : preset.characterId);
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}
			CustomizationTargetType customizationTargetType = ((this._currentTargetType != CustomizationTargetType.None) ? this._currentTargetType : preset.targetType);
			CharacterStateData characterStateData = preset.stateData ?? new CharacterStateData();
			if (this.verboseLogs)
			{
				Debug.Log(string.Format("[HubPresetList] EnsureSystemPresetPersisted presetId={0} ownerCharacterId='{1}' ownerTargetType={2}", preset.presetId, text, customizationTargetType));
			}
			return this.presetCoordinator.TryEnsureSystemPreset(preset.presetId, text, customizationTargetType, preset.presetName, characterStateData, preset.iconFileName);
		}

		// Token: 0x06004F87 RID: 20359 RVA: 0x001ED288 File Offset: 0x001EB488
		private static CustomizationPresetData FindPersistedPresetById(List<CustomizationPresetData> presets, string presetId)
		{
			if (presets == null || string.IsNullOrWhiteSpace(presetId))
			{
				return null;
			}
			for (int i = 0; i < presets.Count; i++)
			{
				CustomizationPresetData customizationPresetData = presets[i];
				if (customizationPresetData != null && customizationPresetData.presetId == presetId)
				{
					return customizationPresetData;
				}
			}
			return null;
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x001ED2CF File Offset: 0x001EB4CF
		private static void SortPresetsForDisplay(List<CustomizationPresetData> presets)
		{
			if (presets == null || presets.Count <= 1)
			{
				return;
			}
			presets.Sort(delegate(CustomizationPresetData a, CustomizationPresetData b)
			{
				if (a == b)
				{
					return 0;
				}
				if (a == null)
				{
					return 1;
				}
				if (b == null)
				{
					return -1;
				}
				int num = b.isDefault.CompareTo(a.isDefault);
				if (num != 0)
				{
					return num;
				}
				DateTime dateTime;
				bool flag = DateTime.TryParse(a.createdAtUtc, null, DateTimeStyles.RoundtripKind, out dateTime);
				DateTime dateTime2;
				bool flag2 = DateTime.TryParse(b.createdAtUtc, null, DateTimeStyles.RoundtripKind, out dateTime2);
				if (flag && flag2)
				{
					int num2 = dateTime.CompareTo(dateTime2);
					if (num2 != 0)
					{
						return num2;
					}
				}
				else
				{
					if (flag)
					{
						return -1;
					}
					if (flag2)
					{
						return 1;
					}
				}
				return string.Compare(a.presetName, b.presetName, StringComparison.OrdinalIgnoreCase);
			});
		}

		// Token: 0x06004F89 RID: 20361 RVA: 0x001ED304 File Offset: 0x001EB504
		private static CustomizationPresetData FindActivePreset(List<CustomizationPresetData> presets)
		{
			for (int i = 0; i < presets.Count; i++)
			{
				if (presets[i] != null && presets[i].isDefault)
				{
					return presets[i];
				}
			}
			for (int j = 0; j < presets.Count; j++)
			{
				if (presets[j] != null && presets[j].isActive)
				{
					return presets[j];
				}
			}
			return null;
		}

		// Token: 0x0400506C RID: 20588
		[Header("References")]
		[SerializeField]
		private CustomizationHubController hubController;

		// Token: 0x0400506D RID: 20589
		[SerializeField]
		private CustomizationPresetCoordinator presetCoordinator;

		// Token: 0x0400506E RID: 20590
		[SerializeField]
		private CharactersManager charactersManager;

		// Token: 0x0400506F RID: 20591
		[SerializeField]
		private CustomizationSystemPresetConfig systemPresetConfig;

		// Token: 0x04005070 RID: 20592
		[Header("Spawn Providers")]
		[SerializeField]
		private PrefabCustomizationSpawnProvider mainCharacterSpawnProvider;

		// Token: 0x04005071 RID: 20593
		[SerializeField]
		private PrefabCustomizationSpawnProvider npcSpawnProvider;

		// Token: 0x04005072 RID: 20594
		[Header("Hub Preview")]
		[Tooltip("Optional: CharacterSpinner UI element for rotating the preview model by drag.")]
		[SerializeField]
		private CharacterSpinner hubSpinner;

		// Token: 0x04005073 RID: 20595
		[Header("Debug")]
		[SerializeField]
		private bool verboseLogs;

		// Token: 0x04005074 RID: 20596
		[SerializeField]
		private bool diagnosticHairLogs;

		// Token: 0x04005075 RID: 20597
		[SerializeField]
		private bool diagnosticDecalLogs;

		// Token: 0x04005076 RID: 20598
		private CustomizationTargetType _currentTargetType;

		// Token: 0x04005077 RID: 20599
		private string _currentCharacterId;

		// Token: 0x04005078 RID: 20600
		private readonly List<CustomizationPresetData> _currentPresets = new List<CustomizationPresetData>();

		// Token: 0x04005079 RID: 20601
		private CustomizationPresetData _selectedPreset;

		// Token: 0x0400507A RID: 20602
		private List<SystemPresetEntry> _cachedMcSystemEntries;

		// Token: 0x0400507B RID: 20603
		private List<SystemPresetEntry> _cachedNpcSystemEntries;

		// Token: 0x0400507C RID: 20604
		private int _previewLoadOperations;

		// Token: 0x0400507D RID: 20605
		private int _previewRevision;
	}
}
