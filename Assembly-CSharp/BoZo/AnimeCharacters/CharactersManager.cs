using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectExhibitionist.CharacterCustomization.Core;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000847 RID: 2119
	public class CharactersManager : MonoBehaviour
	{
		// Token: 0x06003802 RID: 14338 RVA: 0x00147A73 File Offset: 0x00145C73
		private void Awake()
		{
			this.InitializePaths();
			this.LoadAllCharacters();
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x00147A84 File Offset: 0x00145C84
		private void InitializePaths()
		{
			this.characterDataPath = GameDataPaths.CharacterProfilesDirectory;
			this.characterIconsPath = GameDataPaths.CharacterProfileIconsDirectory;
			if (!Directory.Exists(this.characterDataPath))
			{
				Directory.CreateDirectory(this.characterDataPath);
			}
			if (!Directory.Exists(this.characterIconsPath))
			{
				Directory.CreateDirectory(this.characterIconsPath);
			}
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x00147AD9 File Offset: 0x00145CD9
		public void SetSelectedCharacterId(string characterId, bool refreshPresets = true)
		{
			if (this.selectedCharacterId == characterId)
			{
				return;
			}
			this.selectedCharacterId = characterId;
			Action<string> onSelectedCharacterChanged = this.OnSelectedCharacterChanged;
			if (onSelectedCharacterChanged != null)
			{
				onSelectedCharacterChanged(this.selectedCharacterId);
			}
			if (refreshPresets)
			{
				this.RefreshSelectedCharacterPresets();
			}
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x00147B11 File Offset: 0x00145D11
		public string GetSelectedCharacterId()
		{
			return this.selectedCharacterId;
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x00147B19 File Offset: 0x00145D19
		public List<CharacterDataExtended> GetAllCharacters()
		{
			return new List<CharacterDataExtended>(this.allCharacters);
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x00147B26 File Offset: 0x00145D26
		public List<CharacterDataExtended> GetNPCs()
		{
			return this.allCharacters.Where<CharacterDataExtended>((CharacterDataExtended c) => !c.isMainCharacter).ToList<CharacterDataExtended>();
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x00147B57 File Offset: 0x00145D57
		public CharacterDataExtended GetMainCharacter()
		{
			return this.allCharacters.FirstOrDefault<CharacterDataExtended>((CharacterDataExtended c) => c.isMainCharacter);
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x00147B84 File Offset: 0x00145D84
		public CharacterDataExtended GetCharacterById(string characterId)
		{
			return this.allCharacters.FirstOrDefault<CharacterDataExtended>((CharacterDataExtended c) => c.characterId == characterId);
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x00147BB8 File Offset: 0x00145DB8
		public CharacterDataExtended CreateNewCharacter(string name = "New Character", bool isMainCharacter = false)
		{
			CharacterDataExtended characterDataExtended = new CharacterDataExtended
			{
				characterName = name,
				description = "A new character",
				createdFrom = "base",
				icon = this.defaultIcon,
				isMainCharacter = isMainCharacter
			};
			characterDataExtended.generalInfo.characterName = name;
			this.allCharacters.Add(characterDataExtended);
			this.SaveCharacter(characterDataExtended);
			Action<CharacterDataExtended> onCharacterCreated = this.OnCharacterCreated;
			if (onCharacterCreated != null)
			{
				onCharacterCreated(characterDataExtended);
			}
			Action<List<CharacterDataExtended>> onCharacterListUpdated = this.OnCharacterListUpdated;
			if (onCharacterListUpdated != null)
			{
				onCharacterListUpdated(this.allCharacters);
			}
			this.RefreshPresetsForCharacter(characterDataExtended.characterId);
			return characterDataExtended;
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x00147C50 File Offset: 0x00145E50
		public CharacterDataExtended DuplicateCharacter(CharacterDataExtended original)
		{
			if (original == null)
			{
				Debug.LogError("Cannot duplicate null character");
				return null;
			}
			CharacterDataExtended characterDataExtended = new CharacterDataExtended(original);
			characterDataExtended.isMainCharacter = false;
			this.allCharacters.Add(characterDataExtended);
			this.SaveCharacter(characterDataExtended);
			Action<CharacterDataExtended> onCharacterCreated = this.OnCharacterCreated;
			if (onCharacterCreated != null)
			{
				onCharacterCreated(characterDataExtended);
			}
			Action<List<CharacterDataExtended>> onCharacterListUpdated = this.OnCharacterListUpdated;
			if (onCharacterListUpdated != null)
			{
				onCharacterListUpdated(this.allCharacters);
			}
			this.RefreshPresetsForCharacter(characterDataExtended.characterId);
			return characterDataExtended;
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x00147CC4 File Offset: 0x00145EC4
		public void UpdateCharacter(CharacterDataExtended characterData)
		{
			if (characterData == null)
			{
				return;
			}
			CharacterDataExtended characterById = this.GetCharacterById(characterData.characterId);
			if (characterById != null)
			{
				int num = this.allCharacters.IndexOf(characterById);
				if (num >= 0)
				{
					this.allCharacters[num] = characterData;
				}
				characterData.UpdateLastModified();
				this.SaveCharacter(characterData);
				Action<CharacterDataExtended> onCharacterUpdated = this.OnCharacterUpdated;
				if (onCharacterUpdated != null)
				{
					onCharacterUpdated(characterData);
				}
				Action<List<CharacterDataExtended>> onCharacterListUpdated = this.OnCharacterListUpdated;
				if (onCharacterListUpdated == null)
				{
					return;
				}
				onCharacterListUpdated(this.allCharacters);
			}
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x00147D38 File Offset: 0x00145F38
		public void DeleteCharacter(string characterId)
		{
			CharacterDataExtended characterById = this.GetCharacterById(characterId);
			if (characterById != null)
			{
				if (characterById.isMainCharacter)
				{
					Debug.LogWarning("Cannot delete main character!");
					return;
				}
				this.allCharacters.Remove(characterById);
				this.DeleteCharacterFiles(characterId);
				Action<string> onCharacterDeleted = this.OnCharacterDeleted;
				if (onCharacterDeleted != null)
				{
					onCharacterDeleted(characterId);
				}
				Action<List<CharacterDataExtended>> onCharacterListUpdated = this.OnCharacterListUpdated;
				if (onCharacterListUpdated != null)
				{
					onCharacterListUpdated(this.allCharacters);
				}
				if (this.selectedCharacterId == characterId)
				{
					this.SetSelectedCharacterId(null, false);
				}
			}
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x00147DB8 File Offset: 0x00145FB8
		public List<CustomizationPresetData> GetPresetsForCharacter(string characterId)
		{
			CharacterDataExtended characterById = this.GetCharacterById(characterId);
			if (characterById == null || this.presetCoordinator == null)
			{
				return new List<CustomizationPresetData>();
			}
			return this.presetCoordinator.GetPresets(characterById.characterId, CharactersManager.GetTargetType(characterById), characterById.stateData);
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x00147E04 File Offset: 0x00146004
		public bool TrySetActivePresetForCharacter(string characterId, string presetId)
		{
			CharacterDataExtended characterById = this.GetCharacterById(characterId);
			return characterById != null && !(this.presetCoordinator == null) && !string.IsNullOrWhiteSpace(presetId) && this.presetCoordinator.TrySetActivePresetForCharacter(characterById.characterId, CharactersManager.GetTargetType(characterById), presetId);
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x00147E4C File Offset: 0x0014604C
		public bool TryCreatePresetForCharacter(string characterId, string presetName, out CustomizationPresetData createdPreset, CharacterStateData explicitStateData = null, string iconFileName = null)
		{
			string text;
			return this.TryCreatePresetForCharacter(characterId, presetName, out createdPreset, out text, explicitStateData, iconFileName);
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x00147E68 File Offset: 0x00146068
		public bool TryCreatePresetForCharacter(string characterId, string presetName, out CustomizationPresetData createdPreset, out string error, CharacterStateData explicitStateData = null, string iconFileName = null)
		{
			createdPreset = null;
			error = null;
			CharacterDataExtended characterById = this.GetCharacterById(characterId);
			if (characterById == null || this.presetCoordinator == null)
			{
				error = "Character or preset coordinator not found.";
				return false;
			}
			if (this.HasPresetNameConflict(characterById.characterId, CharactersManager.GetTargetType(characterById), presetName, null))
			{
				error = "A preset with this name already exists.";
				return false;
			}
			bool flag = this.presetCoordinator.TryCreatePreset(characterById.characterId, CharactersManager.GetTargetType(characterById), presetName, explicitStateData ?? characterById.stateData, out createdPreset, false, iconFileName);
			if (!flag)
			{
				error = "Failed to create preset.";
			}
			if (flag)
			{
				this.RefreshPresetsForCharacter(characterById.characterId);
			}
			return flag;
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x00147F04 File Offset: 0x00146104
		public bool TryUpdatePresetForCharacter(string characterId, string presetId, string presetName, CharacterStateData stateData, string iconFileName, out CustomizationPresetData updatedPreset, out string error)
		{
			updatedPreset = null;
			error = null;
			CharacterDataExtended characterById = this.GetCharacterById(characterId);
			if (characterById == null || this.presetCoordinator == null)
			{
				error = "Character or preset coordinator not found.";
				return false;
			}
			if (this.HasPresetNameConflict(characterById.characterId, CharactersManager.GetTargetType(characterById), presetName, presetId))
			{
				error = "A preset with this name already exists.";
				return false;
			}
			bool flag = this.presetCoordinator.TryUpdatePreset(presetId, presetName, stateData ?? characterById.stateData, iconFileName, out updatedPreset, out error);
			if (flag)
			{
				this.RefreshPresetsForCharacter(characterById.characterId);
			}
			return flag;
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x00147F8C File Offset: 0x0014618C
		public bool TryUpdatePresetGender(string characterId, string presetId, Gender gender)
		{
			CharacterDataExtended characterById = this.GetCharacterById(characterId);
			if (characterById == null || this.presetCoordinator == null || string.IsNullOrWhiteSpace(presetId))
			{
				return false;
			}
			bool flag = this.presetCoordinator.TryUpdatePresetGender(presetId, gender);
			if (flag)
			{
				this.RefreshPresetsForCharacter(characterById.characterId);
			}
			return flag;
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x00147FD8 File Offset: 0x001461D8
		public bool TryCreateSystemPresetForCharacter(string characterId, string presetName, out CustomizationPresetData createdPreset)
		{
			createdPreset = null;
			CharacterDataExtended characterById = this.GetCharacterById(characterId);
			if (characterById == null || this.presetCoordinator == null)
			{
				return false;
			}
			bool flag = this.presetCoordinator.TryCreatePreset(characterById.characterId, CharactersManager.GetTargetType(characterById), presetName, characterById.stateData, out createdPreset, true, null);
			if (flag)
			{
				this.RefreshPresetsForCharacter(characterById.characterId);
			}
			return flag;
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x00148034 File Offset: 0x00146234
		public bool TryDuplicatePresetForCharacter(string characterId, string sourcePresetId, string duplicatedPresetName, out CustomizationPresetData duplicatedPreset)
		{
			duplicatedPreset = null;
			CharacterDataExtended characterById = this.GetCharacterById(characterId);
			if (characterById == null || this.presetCoordinator == null)
			{
				return false;
			}
			bool flag = this.presetCoordinator.TryDuplicatePreset(characterById.characterId, CharactersManager.GetTargetType(characterById), sourcePresetId, duplicatedPresetName, out duplicatedPreset);
			if (flag)
			{
				this.RefreshPresetsForCharacter(characterById.characterId);
			}
			return flag;
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x0014808C File Offset: 0x0014628C
		public bool TryDeletePresetForCharacter(string characterId, string presetId)
		{
			CharacterDataExtended characterById = this.GetCharacterById(characterId);
			if (characterById == null || this.presetCoordinator == null)
			{
				return false;
			}
			bool flag = this.presetCoordinator.TryDeletePreset(characterById.characterId, CharactersManager.GetTargetType(characterById), presetId);
			if (flag)
			{
				this.RefreshPresetsForCharacter(characterById.characterId);
			}
			return flag;
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x001480DB File Offset: 0x001462DB
		public bool CanDeletePreset(CustomizationPresetData preset)
		{
			return preset != null && !preset.isDefault && !preset.isSystemPreset;
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x001480F3 File Offset: 0x001462F3
		public void RefreshSelectedCharacterPresets()
		{
			if (string.IsNullOrWhiteSpace(this.selectedCharacterId))
			{
				return;
			}
			this.RefreshPresetsForCharacter(this.selectedCharacterId);
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x00148110 File Offset: 0x00146310
		public void RefreshPresetsForCharacter(string characterId)
		{
			CharacterDataExtended characterById = this.GetCharacterById(characterId);
			if (characterById == null || this.presetCoordinator == null)
			{
				return;
			}
			CustomizationTargetType targetType = CharactersManager.GetTargetType(characterById);
			List<CustomizationPresetData> presets = this.presetCoordinator.GetPresets(characterById.characterId, targetType, characterById.stateData);
			Action<string, CustomizationTargetType, List<CustomizationPresetData>> onPresetListUpdated = this.OnPresetListUpdated;
			if (onPresetListUpdated == null)
			{
				return;
			}
			onPresetListUpdated(characterById.characterId, targetType, presets);
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x0014816F File Offset: 0x0014636F
		private static CustomizationTargetType GetTargetType(CharacterDataExtended character)
		{
			if (character == null)
			{
				return CustomizationTargetType.None;
			}
			if (!character.isMainCharacter)
			{
				return CustomizationTargetType.Npc;
			}
			return CustomizationTargetType.MainCharacter;
		}

		// Token: 0x0600381B RID: 14363 RVA: 0x00148184 File Offset: 0x00146384
		private bool HasPresetNameConflict(string characterId, CustomizationTargetType targetType, string presetName, string ignorePresetId)
		{
			if (string.IsNullOrWhiteSpace(characterId) || string.IsNullOrWhiteSpace(presetName) || this.presetCoordinator == null)
			{
				return false;
			}
			string text = presetName.Trim();
			List<CustomizationPresetData> presets = this.presetCoordinator.GetPresets(characterId, targetType, null);
			for (int i = 0; i < presets.Count; i++)
			{
				CustomizationPresetData customizationPresetData = presets[i];
				if (customizationPresetData != null && (string.IsNullOrWhiteSpace(ignorePresetId) || !(customizationPresetData.presetId == ignorePresetId)))
				{
					string presetName2 = customizationPresetData.presetName;
					if (string.Equals((presetName2 != null) ? presetName2.Trim() : null, text, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x0014821C File Offset: 0x0014641C
		private void LoadAllCharacters()
		{
			this.allCharacters.Clear();
			this.LoadCharactersFromLocalFiles();
			if (this.allCharacters.Count == 0)
			{
				this.EnsureMainCharacterExists();
			}
			CharacterDataExtended characterDataExtended = this.GetMainCharacter() ?? this.allCharacters.FirstOrDefault<CharacterDataExtended>();
			this.SetSelectedCharacterId((characterDataExtended != null) ? characterDataExtended.characterId : null, false);
			Action<List<CharacterDataExtended>> onCharacterListUpdated = this.OnCharacterListUpdated;
			if (onCharacterListUpdated == null)
			{
				return;
			}
			onCharacterListUpdated(this.allCharacters);
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x00148290 File Offset: 0x00146490
		private void LoadCharactersFromLocalFiles()
		{
			if (!Directory.Exists(this.characterDataPath))
			{
				return;
			}
			foreach (string text in Directory.GetFiles(this.characterDataPath, "*.json"))
			{
				try
				{
					CharacterDataExtended characterDataExtended = JsonUtility.FromJson<CharacterDataExtended>(File.ReadAllText(text));
					this.LoadCharacterIcon(characterDataExtended);
					this.allCharacters.Add(characterDataExtended);
				}
				catch (Exception ex)
				{
					Debug.LogError("Failed to load character from " + text + ": " + ex.Message);
				}
			}
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x00148320 File Offset: 0x00146520
		private void SaveCharacter(CharacterDataExtended characterData)
		{
			try
			{
				string text = JsonUtility.ToJson(characterData, true);
				File.WriteAllText(Path.Combine(this.characterDataPath, characterData.characterId + ".json"), text);
				this.SaveCharacterIcon(characterData);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to save character " + characterData.characterName + ": " + ex.Message);
			}
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x00148394 File Offset: 0x00146594
		private void DeleteCharacterFiles(string characterId)
		{
			try
			{
				string text = Path.Combine(this.characterDataPath, characterId + ".json");
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				string text2 = Path.Combine(this.characterIconsPath, characterId + ".png");
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to delete character files for " + characterId + ": " + ex.Message);
			}
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x0014841C File Offset: 0x0014661C
		private void SaveCharacterIcon(CharacterDataExtended characterData)
		{
			if (characterData.icon == null)
			{
				return;
			}
			try
			{
				byte[] array = characterData.icon.texture.EncodeToPNG();
				File.WriteAllBytes(Path.Combine(this.characterIconsPath, characterData.characterId + ".png"), array);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to save character icon for " + characterData.characterId + ": " + ex.Message);
			}
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x001484A0 File Offset: 0x001466A0
		private void LoadCharacterIconFromPng(CharacterDataExtended characterData)
		{
			this.LoadCharacterIcon(characterData);
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x001484AC File Offset: 0x001466AC
		private void LoadCharacterIcon(CharacterDataExtended characterData)
		{
			string text = Path.Combine(this.characterIconsPath, characterData.characterId + ".png");
			if (File.Exists(text))
			{
				byte[] array = File.ReadAllBytes(text);
				Texture2D texture2D = new Texture2D(2, 2);
				texture2D.LoadImage(array);
				characterData.icon = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.one * 0.5f);
			}
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x0014852C File Offset: 0x0014672C
		public List<CharacterDataExtended> SearchCharacters(string searchTerm, bool includeMainCharacter = true)
		{
			if (!string.IsNullOrEmpty(searchTerm))
			{
				searchTerm = searchTerm.ToLower();
				IEnumerable<CharacterDataExtended> enumerable = this.allCharacters.Where<CharacterDataExtended>((CharacterDataExtended c) => c.characterName.ToLower().Contains(searchTerm) || c.description.ToLower().Contains(searchTerm) || c.location.ToLower().Contains(searchTerm));
				if (!includeMainCharacter)
				{
					enumerable = enumerable.Where<CharacterDataExtended>((CharacterDataExtended c) => !c.isMainCharacter);
				}
				return enumerable.ToList<CharacterDataExtended>();
			}
			if (!includeMainCharacter)
			{
				return this.GetNPCs();
			}
			return this.GetAllCharacters();
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x001485BC File Offset: 0x001467BC
		public CharacterDataExtended EnsureMainCharacterExists()
		{
			CharacterDataExtended characterDataExtended = this.GetMainCharacter();
			if (characterDataExtended == null)
			{
				characterDataExtended = this.CreateNewCharacter("Main Character", true);
			}
			return characterDataExtended;
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x001485E4 File Offset: 0x001467E4
		public CharacterDataExtended EnsureCustomNpcOwnerExists()
		{
			CharacterDataExtended characterDataExtended = this.GetCharacterById("CustomNPC");
			if (characterDataExtended != null)
			{
				return characterDataExtended;
			}
			characterDataExtended = new CharacterDataExtended
			{
				characterId = "CustomNPC",
				characterName = "Custom NPCs",
				description = "Owner profile for all custom NPCs",
				createdFrom = "system",
				isMainCharacter = false
			};
			characterDataExtended.generalInfo.characterName = "Custom NPCs";
			this.allCharacters.Add(characterDataExtended);
			this.SaveCharacter(characterDataExtended);
			Action<List<CharacterDataExtended>> onCharacterListUpdated = this.OnCharacterListUpdated;
			if (onCharacterListUpdated != null)
			{
				onCharacterListUpdated(this.allCharacters);
			}
			return characterDataExtended;
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x00148676 File Offset: 0x00146876
		[ContextMenu("Refresh Character List")]
		public void RefreshCharacterList()
		{
			this.LoadAllCharacters();
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x0014867E File Offset: 0x0014687E
		public void OnCharacterSavedExternal()
		{
			this.RefreshCharacterList();
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x00148688 File Offset: 0x00146888
		[ContextMenu("Debug Character List")]
		public void DebugCharacterList()
		{
			Debug.Log("=== CHARACTER LIST DEBUG ===");
			Debug.Log(string.Format("Total characters: {0}", this.allCharacters.Count));
			Debug.Log("Selected character id: " + (this.selectedCharacterId ?? "None"));
			foreach (CharacterDataExtended characterDataExtended in this.allCharacters)
			{
				Debug.Log(string.Format("- {0} (Main: {1}, Icon: {2})", characterDataExtended.characterName, characterDataExtended.isMainCharacter, characterDataExtended.icon != null));
			}
			Debug.Log("=== DEBUG COMPLETE ===");
		}

		// Token: 0x04003905 RID: 14597
		[Header("Configuration")]
		public Sprite defaultIcon;

		// Token: 0x04003906 RID: 14598
		[Header("File Paths")]
		private string characterDataPath;

		// Token: 0x04003907 RID: 14599
		private string characterIconsPath;

		// Token: 0x04003908 RID: 14600
		[Header("Preset System")]
		[SerializeField]
		private CustomizationPresetCoordinator presetCoordinator;

		// Token: 0x04003909 RID: 14601
		private string selectedCharacterId;

		// Token: 0x0400390A RID: 14602
		private List<CharacterDataExtended> allCharacters = new List<CharacterDataExtended>();

		// Token: 0x0400390B RID: 14603
		public Action<List<CharacterDataExtended>> OnCharacterListUpdated;

		// Token: 0x0400390C RID: 14604
		public Action<CharacterDataExtended> OnCharacterCreated;

		// Token: 0x0400390D RID: 14605
		public Action<CharacterDataExtended> OnCharacterUpdated;

		// Token: 0x0400390E RID: 14606
		public Action<string> OnCharacterDeleted;

		// Token: 0x0400390F RID: 14607
		public Action<string, CustomizationTargetType, List<CustomizationPresetData>> OnPresetListUpdated;

		// Token: 0x04003910 RID: 14608
		public Action<string> OnSelectedCharacterChanged;
	}
}
