using System;
using System.Collections.Generic;
using System.IO;
using Bozo.AnimeCharacters;
using Features.RunModes;
using ProjectExhibitionist.CharacterCustomization.Core;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using ProjectExhibitionist.CharacterCustomization.Core.Storage;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EE8 RID: 3816
	public static class NPCRosterSession
	{
		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x06007239 RID: 29241 RVA: 0x002E7564 File Offset: 0x002E5764
		public static int MinimumEligible
		{
			get
			{
				NPCSpawnConfig npcspawnConfig = Resources.Load<NPCSpawnConfig>("NPCSpawnConfig");
				if (!(npcspawnConfig != null))
				{
					return 20;
				}
				return Mathf.Max(1, npcspawnConfig.maxTotalNpc);
			}
		}

		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x0600723A RID: 29242 RVA: 0x002E7594 File Offset: 0x002E5794
		public static NpcPopulationMode PopulationMode
		{
			get
			{
				if (!RunModeService.HasPreparedRun)
				{
					return NpcRosterPreferences.PopulationMode;
				}
				return NPCRosterSession.GetCurrentSnapshot().populationMode;
			}
		}

		// Token: 0x0600723B RID: 29243 RVA: 0x002E75AD File Offset: 0x002E57AD
		public static bool AllowsGender(Gender gender)
		{
			return NPCRosterSession.MatchesPopulation(NPCRosterSession.PopulationMode, gender);
		}

		// Token: 0x0600723C RID: 29244 RVA: 0x002E75BA File Offset: 0x002E57BA
		public static bool MatchesPopulation(NpcPopulationMode mode, Gender gender)
		{
			return mode == NpcPopulationMode.Mixed || (mode == NpcPopulationMode.MaleOnly && gender == Gender.Male) || (mode == NpcPopulationMode.FemaleOnly && gender == Gender.Female);
		}

		// Token: 0x0600723D RID: 29245 RVA: 0x002E75D4 File Offset: 0x002E57D4
		public static NPCRosterSnapshot GetCurrentSnapshot()
		{
			if (RunModeService.HasPreparedRun && RunModeService.CurrentNpcRoster != null)
			{
				return RunModeService.CurrentNpcRoster;
			}
			if (NPCRosterSession._directSceneSnapshot != null)
			{
				return NPCRosterSession._directSceneSnapshot;
			}
			string text;
			if (!NPCRosterSession.TryCapture(out NPCRosterSession._directSceneSnapshot, out text))
			{
				NPCRosterSession._directSceneSnapshot = new NPCRosterSnapshot
				{
					runId = Guid.NewGuid().ToString("N"),
					populationMode = NpcRosterPreferences.PopulationMode
				};
				Debug.LogWarning("[NPCRoster] Direct scene population unavailable: " + text);
			}
			if (RunModeService.HasPreparedRun)
			{
				RunModeService.SaveMigratedNpcRoster(NPCRosterSession._directSceneSnapshot);
			}
			return NPCRosterSession._directSceneSnapshot;
		}

		// Token: 0x0600723E RID: 29246 RVA: 0x002E7663 File Offset: 0x002E5863
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		public static void ResetFallback()
		{
			NPCRosterSession._directSceneSnapshot = null;
		}

		// Token: 0x0600723F RID: 29247 RVA: 0x002E766C File Offset: 0x002E586C
		public static bool TryCapture(out NPCRosterSnapshot snapshot, out string error)
		{
			snapshot = null;
			error = null;
			bool flag;
			try
			{
				flag = NPCRosterSession.TryBuildSnapshot(NPCRosterSession.LoadCatalog(), NpcRosterPreferences.PopulationMode, new Func<CustomizationPresetData, bool>(NpcRosterPreferences.IsEnabled), new Func<string, CharacterData>(NpcLookStore.LoadLook), out snapshot, out error, NPCRosterSession.MinimumEligible);
			}
			catch (Exception ex)
			{
				error = "Could not read the NPC roster. Open Characters and save your NPC selection again.";
				Debug.LogWarning("[NPCRoster] " + error + " " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06007240 RID: 29248 RVA: 0x002E76EC File Offset: 0x002E58EC
		public static List<CustomizationPresetData> LoadCatalog()
		{
			List<CustomizationPresetData> list = new List<CustomizationPresetData>();
			foreach (NPCSystemRosterMetadata.Entry entry in NPCSystemRosterMetadata.Entries)
			{
				if (!entry.isInactive)
				{
					list.Add(new CustomizationPresetData
					{
						presetId = "sys_" + entry.resourceName,
						characterId = entry.resourceName,
						presetName = entry.displayName,
						targetType = CustomizationTargetType.Npc,
						isSystemPreset = true,
						gender = entry.gender
					});
				}
			}
			string text = Path.Combine(GameDataPaths.CustomizationRoot, "customization_presets.json");
			if (!File.Exists(text))
			{
				return list;
			}
			CustomizationPresetCollection customizationPresetCollection = JsonUtility.FromJson<CustomizationPresetCollection>(File.ReadAllText(text));
			if (((customizationPresetCollection != null) ? customizationPresetCollection.presets : null) == null)
			{
				throw new InvalidDataException("Invalid preset collection.");
			}
			foreach (CustomizationPresetData customizationPresetData in customizationPresetCollection.presets)
			{
				if (customizationPresetData != null && !customizationPresetData.isSystemPreset && customizationPresetData.targetType == CustomizationTargetType.Npc && customizationPresetData.characterId == "CustomNPC")
				{
					list.Add(customizationPresetData);
				}
			}
			return list;
		}

		// Token: 0x06007241 RID: 29249 RVA: 0x002E7830 File Offset: 0x002E5A30
		public static bool TryBuildSnapshot(IReadOnlyList<CustomizationPresetData> catalog, NpcPopulationMode mode, Func<CustomizationPresetData, bool> isEnabled, Func<string, CharacterData> loadLook, out NPCRosterSnapshot snapshot, out string error, int minimumEligible = 20)
		{
			snapshot = null;
			error = null;
			minimumEligible = Math.Max(1, minimumEligible);
			if (catalog == null || isEnabled == null || loadLook == null || !Enum.IsDefined(typeof(NpcPopulationMode), mode))
			{
				error = "NPC roster is unavailable.";
				return false;
			}
			NPCRosterSnapshot npcrosterSnapshot = new NPCRosterSnapshot();
			Guid guid = Guid.NewGuid();
			npcrosterSnapshot.runId = guid.ToString("N");
			npcrosterSnapshot.populationMode = mode;
			NPCRosterSnapshot npcrosterSnapshot2 = npcrosterSnapshot;
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			int num = 0;
			foreach (CustomizationPresetData customizationPresetData in catalog)
			{
				if (customizationPresetData != null && customizationPresetData.targetType == CustomizationTargetType.Npc && !string.IsNullOrWhiteSpace(customizationPresetData.presetId) && hashSet.Add(customizationPresetData.presetId) && isEnabled(customizationPresetData))
				{
					if (!customizationPresetData.isSystemPreset && ++num > 4)
					{
						error = "Up to 4 custom NPCs can be enabled. Disable one in Characters before starting.";
						return false;
					}
					if (NPCRosterSession.MatchesPopulation(mode, customizationPresetData.gender))
					{
						NPCRosterEntry npcrosterEntry = new NPCRosterEntry
						{
							presetId = customizationPresetData.presetId,
							resourceName = (customizationPresetData.isSystemPreset ? customizationPresetData.characterId : null),
							displayName = customizationPresetData.presetName,
							gender = customizationPresetData.gender,
							isCustom = !customizationPresetData.isSystemPreset
						};
						if (npcrosterEntry.isCustom)
						{
							CharacterData characterData = loadLook(customizationPresetData.presetId);
							if (!Guid.TryParseExact(customizationPresetData.presetId, "N", out guid) || !NPCRosterSession.IsValidCustomLook(characterData, customizationPresetData.gender))
							{
								error = "The saved look for '" + customizationPresetData.presetName + "' is incomplete. Edit and save this NPC, or disable it in Characters.";
								return false;
							}
							npcrosterEntry.lookJson = JsonUtility.ToJson(characterData);
						}
						npcrosterSnapshot2.entries.Add(npcrosterEntry);
					}
				}
			}
			if (npcrosterSnapshot2.entries.Count < minimumEligible)
			{
				string text = ((mode == NpcPopulationMode.MaleOnly) ? "male " : ((mode == NpcPopulationMode.FemaleOnly) ? "female " : string.Empty));
				error = string.Format("Enable {0} more {1}NPCs in Characters, or change population. At least {2} are required.", minimumEligible - npcrosterSnapshot2.entries.Count, text, minimumEligible);
				return false;
			}
			snapshot = npcrosterSnapshot2;
			return true;
		}

		// Token: 0x06007242 RID: 29250 RVA: 0x002E7AA0 File Offset: 0x002E5CA0
		public static bool IsValidCustomLook(CharacterData look, Gender gender)
		{
			if (((look != null) ? look.generalInfo : null) == null || look.generalInfo.gender != gender || !Enum.IsDefined(typeof(Gender), gender) || look.bodyIDs == null || look.bodyShapes == null || look.bodyIDs.Count != look.bodyShapes.Count || look.faceIDs == null || look.faceShapes == null || look.faceIDs.Count != look.faceShapes.Count || look.bodyModsKeys == null || look.bodyMods == null || look.bodyModsKeys.Count != look.bodyMods.Count || look.outfitDatas == null || look.outfitDatas.Count == 0)
			{
				return false;
			}
			bool flag = false;
			foreach (OutfitData outfitData in look.outfitDatas)
			{
				if (!NPCRosterSession.IsValidOutfit(outfitData) || string.IsNullOrWhiteSpace(outfitData.outfit))
				{
					return false;
				}
				if (outfitData.outfit.StartsWith("Body/", StringComparison.Ordinal))
				{
					flag = true;
				}
			}
			return flag || NPCRosterSession.IsValidOutfit(look.bodyData);
		}

		// Token: 0x06007243 RID: 29251 RVA: 0x002E7BFC File Offset: 0x002E5DFC
		private static bool IsValidOutfit(OutfitData outfit)
		{
			return outfit != null && outfit.colors != null && outfit.decalColors != null && outfit.patternColors != null && (string.IsNullOrEmpty(outfit.decal) || outfit.decalColors.Count >= 2) && (string.IsNullOrEmpty(outfit.pattern) || outfit.patternColors.Count >= 2);
		}

		// Token: 0x04007558 RID: 30040
		public const int DefaultMinimumEligible = 20;

		// Token: 0x04007559 RID: 30041
		private static NPCRosterSnapshot _directSceneSnapshot;
	}
}
