using System;
using System.Collections.Generic;
using System.IO;
using Bozo.AnimeCharacters;
using Features.NeckSigns;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B0E RID: 2830
	public static class MainCharacterAppearanceService
	{
		// Token: 0x06005010 RID: 20496 RVA: 0x001EF7BC File Offset: 0x001ED9BC
		public static bool TrySaveNeckSignDesign(NeckSignDesignData design)
		{
			CustomizationPresetData inUse;
			if (design == null || string.IsNullOrWhiteSpace(design.partId) || !MainCharacterAppearanceService.TryGetInUseMainCharacterPreset(out inUse))
			{
				return false;
			}
			string text = Path.Combine(GameDataPaths.CustomizationRoot, "customization_presets.json");
			bool flag;
			try
			{
				CustomizationPresetCollection customizationPresetCollection = JsonUtility.FromJson<CustomizationPresetCollection>(File.ReadAllText(text));
				CustomizationPresetData customizationPresetData;
				if (customizationPresetCollection == null)
				{
					customizationPresetData = null;
				}
				else
				{
					List<CustomizationPresetData> presets = customizationPresetCollection.presets;
					customizationPresetData = ((presets != null) ? presets.Find((CustomizationPresetData item) => item != null && item.presetId == inUse.presetId && item.characterId == inUse.characterId && item.targetType == CustomizationTargetType.MainCharacter) : null);
				}
				CustomizationPresetData customizationPresetData2 = customizationPresetData;
				if (customizationPresetData2 == null)
				{
					flag = false;
				}
				else
				{
					if (customizationPresetData2.stateData == null)
					{
						customizationPresetData2.stateData = new CharacterStateData();
					}
					if (customizationPresetData2.stateData.neckSignDesigns == null)
					{
						customizationPresetData2.stateData.neckSignDesigns = new List<NeckSignDesignData>();
					}
					customizationPresetData2.stateData.neckSignDesigns.RemoveAll((NeckSignDesignData item) => item != null && string.Equals(item.partId, design.partId, StringComparison.OrdinalIgnoreCase));
					customizationPresetData2.stateData.neckSignDesigns.Add(design.Clone());
					customizationPresetData2.Touch();
					string text2 = text + ".necksign.tmp";
					File.WriteAllText(text2, JsonUtility.ToJson(customizationPresetCollection, true));
					File.Replace(text2, text, null);
					flag = true;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NeckSigns] Could not save the drawing: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x001EF90C File Offset: 0x001EDB0C
		public static bool TryGetMainCharacterId(out string characterId)
		{
			characterId = null;
			string characterProfilesDirectory = GameDataPaths.CharacterProfilesDirectory;
			if (!Directory.Exists(characterProfilesDirectory))
			{
				return false;
			}
			foreach (string text in Directory.GetFiles(characterProfilesDirectory, "*.json"))
			{
				try
				{
					CharacterDataExtended characterDataExtended = JsonUtility.FromJson<CharacterDataExtended>(File.ReadAllText(text));
					if (characterDataExtended != null && characterDataExtended.isMainCharacter && !string.IsNullOrWhiteSpace(characterDataExtended.characterId))
					{
						characterId = characterDataExtended.characterId;
						return true;
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[MCAppearance] Profilo non leggibile '" + text + "': " + ex.Message);
				}
			}
			return false;
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x001EF9B8 File Offset: 0x001EDBB8
		public static bool TryGetInUseMainCharacterStateData(out CharacterStateData stateData)
		{
			stateData = null;
			CustomizationPresetData customizationPresetData;
			if (!MainCharacterAppearanceService.TryGetInUseMainCharacterPreset(out customizationPresetData))
			{
				return false;
			}
			stateData = customizationPresetData.stateData;
			return true;
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x001EF9DC File Offset: 0x001EDBDC
		public static bool TryGetInUseMainCharacterPreset(out CustomizationPresetData presetData)
		{
			presetData = null;
			string text;
			if (!MainCharacterAppearanceService.TryGetMainCharacterId(out text))
			{
				return false;
			}
			string text2 = Path.Combine(GameDataPaths.CustomizationRoot, "customization_presets.json");
			if (!File.Exists(text2))
			{
				return false;
			}
			CustomizationPresetCollection customizationPresetCollection;
			try
			{
				customizationPresetCollection = JsonUtility.FromJson<CustomizationPresetCollection>(File.ReadAllText(text2));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[MCAppearance] File preset non leggibile '" + text2 + "': " + ex.Message);
				return false;
			}
			if (customizationPresetCollection == null || customizationPresetCollection.presets == null)
			{
				return false;
			}
			CustomizationPresetData customizationPresetData = null;
			CustomizationPresetData customizationPresetData2 = null;
			for (int i = 0; i < customizationPresetCollection.presets.Count; i++)
			{
				CustomizationPresetData customizationPresetData3 = customizationPresetCollection.presets[i];
				if (customizationPresetData3 != null && !(customizationPresetData3.characterId != text) && customizationPresetData3.targetType == CustomizationTargetType.MainCharacter)
				{
					if (customizationPresetData2 == null)
					{
						customizationPresetData2 = customizationPresetData3;
					}
					if (customizationPresetData3.isDefault)
					{
						customizationPresetData = customizationPresetData3;
						break;
					}
					if (customizationPresetData == null && customizationPresetData3.isActive)
					{
						customizationPresetData = customizationPresetData3;
					}
				}
			}
			if (customizationPresetData == null)
			{
				customizationPresetData = customizationPresetData2;
			}
			if (customizationPresetData == null || customizationPresetData.stateData == null)
			{
				return false;
			}
			presetData = customizationPresetData;
			return true;
		}

		// Token: 0x040050E7 RID: 20711
		private const string PresetsFileName = "customization_presets.json";
	}
}
