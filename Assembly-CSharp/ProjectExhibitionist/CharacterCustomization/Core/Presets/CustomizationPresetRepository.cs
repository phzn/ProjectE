using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Presets
{
	// Token: 0x02000B1A RID: 2842
	public class CustomizationPresetRepository : MonoBehaviour
	{
		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x06005071 RID: 20593 RVA: 0x001F21F8 File Offset: 0x001F03F8
		private string FilePath
		{
			get
			{
				return GameDataPaths.GetCustomizationFilePath(this.fileName);
			}
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x001F2208 File Offset: 0x001F0408
		public List<CustomizationPresetData> GetPresets(string characterId, CustomizationTargetType targetType, CharacterStateData defaultStateData = null)
		{
			object sync = this._sync;
			List<CustomizationPresetData> list2;
			lock (sync)
			{
				CustomizationPresetCollection customizationPresetCollection = this.LoadCollection();
				List<CustomizationPresetData> list = new List<CustomizationPresetData>();
				for (int i = 0; i < customizationPresetCollection.presets.Count; i++)
				{
					CustomizationPresetData customizationPresetData = customizationPresetCollection.presets[i];
					if (customizationPresetData != null && customizationPresetData.characterId == characterId && customizationPresetData.targetType == targetType)
					{
						list.Add(customizationPresetData);
					}
				}
				if (CustomizationPresetRepository.NormalizeInUseFlags(customizationPresetCollection, characterId, targetType))
				{
					this.SaveCollection(customizationPresetCollection);
				}
				if (this.autoCreateDefaultPreset && list.Count == 0)
				{
					CustomizationPresetData customizationPresetData2 = CustomizationPresetData.Create(this.defaultPresetName, characterId, targetType, true, CustomizationPresetRepository.CloneStateData(defaultStateData), this.markAutoDefaultAsSystemPreset);
					customizationPresetData2.isActive = false;
					customizationPresetCollection.presets.Add(customizationPresetData2);
					this.SaveCollection(customizationPresetCollection);
					list.Add(customizationPresetData2);
				}
				CustomizationPresetRepository.SortPresetsForDisplay(list);
				list2 = list;
			}
			return list2;
		}

		// Token: 0x06005073 RID: 20595 RVA: 0x001F230C File Offset: 0x001F050C
		public bool TryCreatePreset(string characterId, CustomizationTargetType targetType, string presetName, CharacterStateData stateData, out CustomizationPresetData createdPreset, bool isSystemPreset = false, string iconFileName = null)
		{
			createdPreset = null;
			if (string.IsNullOrWhiteSpace(characterId) || string.IsNullOrWhiteSpace(presetName))
			{
				return false;
			}
			object sync = this._sync;
			bool flag2;
			lock (sync)
			{
				CustomizationPresetCollection customizationPresetCollection = this.LoadCollection();
				if (CustomizationPresetRepository.HasPresetNameConflict(customizationPresetCollection, characterId, targetType, presetName, null))
				{
					flag2 = false;
				}
				else
				{
					CustomizationPresetData customizationPresetData = CustomizationPresetData.Create(presetName.Trim(), characterId, targetType, false, stateData, isSystemPreset);
					customizationPresetData.iconFileName = iconFileName;
					customizationPresetCollection.presets.Add(customizationPresetData);
					this.SaveCollection(customizationPresetCollection);
					createdPreset = customizationPresetData;
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x06005074 RID: 20596 RVA: 0x001F23AC File Offset: 0x001F05AC
		public bool TryEnsureSystemPreset(string presetId, string characterId, CustomizationTargetType targetType, string presetName, CharacterStateData stateData, string iconFileName = null)
		{
			if (string.IsNullOrWhiteSpace(presetId) || string.IsNullOrWhiteSpace(characterId) || targetType == CustomizationTargetType.None || string.IsNullOrWhiteSpace(presetName))
			{
				return false;
			}
			object sync = this._sync;
			bool flag3;
			lock (sync)
			{
				CustomizationPresetCollection customizationPresetCollection = this.LoadCollection();
				CustomizationPresetData customizationPresetData = CustomizationPresetRepository.FindPresetById(customizationPresetCollection, presetId);
				if (customizationPresetData != null)
				{
					bool flag2 = false;
					if (customizationPresetData.characterId != characterId)
					{
						customizationPresetData.characterId = characterId;
						flag2 = true;
					}
					if (customizationPresetData.targetType != targetType)
					{
						customizationPresetData.targetType = targetType;
						flag2 = true;
					}
					if (!string.Equals(customizationPresetData.presetName, presetName, StringComparison.Ordinal))
					{
						customizationPresetData.presetName = presetName;
						flag2 = true;
					}
					if (!customizationPresetData.isSystemPreset)
					{
						customizationPresetData.isSystemPreset = true;
						flag2 = true;
					}
					if (iconFileName != null && !string.Equals(customizationPresetData.iconFileName, iconFileName, StringComparison.Ordinal))
					{
						customizationPresetData.iconFileName = iconFileName;
						flag2 = true;
					}
					if (flag2)
					{
						customizationPresetData.Touch();
						this.SaveCollection(customizationPresetCollection);
					}
					flag3 = true;
				}
				else
				{
					CustomizationPresetData customizationPresetData2 = CustomizationPresetData.Create(presetName.Trim(), characterId, targetType, false, CustomizationPresetRepository.CloneStateData(stateData), true);
					customizationPresetData2.presetId = presetId;
					customizationPresetData2.isDefault = false;
					customizationPresetData2.isActive = false;
					customizationPresetData2.iconFileName = iconFileName;
					customizationPresetCollection.presets.Add(customizationPresetData2);
					this.SaveCollection(customizationPresetCollection);
					flag3 = true;
				}
			}
			return flag3;
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x001F2508 File Offset: 0x001F0708
		public bool TryUpdatePreset(string presetId, string presetName, CharacterStateData stateData, string iconFileName, out CustomizationPresetData updatedPreset, out string error)
		{
			updatedPreset = null;
			error = null;
			if (string.IsNullOrWhiteSpace(presetId) || string.IsNullOrWhiteSpace(presetName) || stateData == null)
			{
				error = "Invalid preset update payload.";
				return false;
			}
			object sync = this._sync;
			bool flag2;
			lock (sync)
			{
				CustomizationPresetCollection customizationPresetCollection = this.LoadCollection();
				CustomizationPresetData customizationPresetData = CustomizationPresetRepository.FindPresetById(customizationPresetCollection, presetId);
				if (customizationPresetData == null)
				{
					error = "Preset not found.";
					flag2 = false;
				}
				else if (customizationPresetData.isSystemPreset)
				{
					error = "System presets cannot be edited.";
					flag2 = false;
				}
				else if (CustomizationPresetRepository.HasPresetNameConflict(customizationPresetCollection, customizationPresetData.characterId, customizationPresetData.targetType, presetName, presetId))
				{
					error = "A preset with this name already exists.";
					flag2 = false;
				}
				else
				{
					customizationPresetData.presetName = presetName.Trim();
					customizationPresetData.stateData = CustomizationPresetRepository.CloneStateData(stateData);
					if (iconFileName != null)
					{
						CustomizationPresetRepository.DeleteIconFileIfExists(customizationPresetData.iconFileName, iconFileName);
						customizationPresetData.iconFileName = iconFileName;
					}
					customizationPresetData.Touch();
					this.SaveCollection(customizationPresetCollection);
					updatedPreset = customizationPresetData;
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x001F2608 File Offset: 0x001F0808
		public bool TryDuplicatePreset(string presetId, string duplicatedPresetName, out CustomizationPresetData duplicatedPreset)
		{
			duplicatedPreset = null;
			if (string.IsNullOrWhiteSpace(presetId) || string.IsNullOrWhiteSpace(duplicatedPresetName))
			{
				return false;
			}
			object sync = this._sync;
			bool flag2;
			lock (sync)
			{
				CustomizationPresetCollection customizationPresetCollection = this.LoadCollection();
				CustomizationPresetData customizationPresetData = CustomizationPresetRepository.FindPresetById(customizationPresetCollection, presetId);
				if (customizationPresetData == null)
				{
					flag2 = false;
				}
				else
				{
					CharacterStateData characterStateData = CustomizationPresetRepository.CloneStateData(customizationPresetData.stateData);
					CustomizationPresetData customizationPresetData2 = CustomizationPresetData.Create(duplicatedPresetName.Trim(), customizationPresetData.characterId, customizationPresetData.targetType, false, characterStateData, false);
					customizationPresetData2.iconFileName = CustomizationPresetRepository.DuplicateIconFile(customizationPresetData.iconFileName, duplicatedPresetName);
					customizationPresetData2.gender = customizationPresetData.gender;
					customizationPresetCollection.presets.Add(customizationPresetData2);
					this.SaveCollection(customizationPresetCollection);
					duplicatedPreset = customizationPresetData2;
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x06005077 RID: 20599 RVA: 0x001F26D8 File Offset: 0x001F08D8
		public bool TryDeletePreset(string presetId)
		{
			if (string.IsNullOrWhiteSpace(presetId))
			{
				return false;
			}
			object sync = this._sync;
			bool flag2;
			lock (sync)
			{
				CustomizationPresetCollection customizationPresetCollection = this.LoadCollection();
				int i = 0;
				while (i < customizationPresetCollection.presets.Count)
				{
					CustomizationPresetData customizationPresetData = customizationPresetCollection.presets[i];
					if (customizationPresetData != null && !(customizationPresetData.presetId != presetId))
					{
						if (CustomizationPresetRepository.IsDeletionProtected(customizationPresetData))
						{
							return false;
						}
						CustomizationPresetRepository.DeleteIconFileIfExists(customizationPresetData.iconFileName, null);
						customizationPresetCollection.presets.RemoveAt(i);
						this.SaveCollection(customizationPresetCollection);
						return true;
					}
					else
					{
						i++;
					}
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x001F2794 File Offset: 0x001F0994
		public bool TrySetActivePreset(string presetId)
		{
			if (string.IsNullOrWhiteSpace(presetId))
			{
				return false;
			}
			object sync = this._sync;
			bool flag2;
			lock (sync)
			{
				CustomizationPresetCollection customizationPresetCollection = this.LoadCollection();
				CustomizationPresetData customizationPresetData = CustomizationPresetRepository.FindPresetById(customizationPresetCollection, presetId);
				if (customizationPresetData == null)
				{
					flag2 = false;
				}
				else
				{
					for (int i = 0; i < customizationPresetCollection.presets.Count; i++)
					{
						CustomizationPresetData customizationPresetData2 = customizationPresetCollection.presets[i];
						if (customizationPresetData2 != null && (customizationPresetData2.characterId == customizationPresetData.characterId && customizationPresetData2.targetType == customizationPresetData.targetType))
						{
							bool flag3 = customizationPresetData2.presetId == customizationPresetData.presetId;
							if (customizationPresetData2.isDefault != flag3 || customizationPresetData2.isActive)
							{
								customizationPresetData2.isDefault = flag3;
								customizationPresetData2.isActive = false;
								customizationPresetData2.Touch();
							}
						}
					}
					this.SaveCollection(customizationPresetCollection);
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x001F289C File Offset: 0x001F0A9C
		public bool TryUpdatePresetState(string presetId, CharacterStateData stateData)
		{
			if (string.IsNullOrWhiteSpace(presetId) || stateData == null)
			{
				return false;
			}
			object sync = this._sync;
			bool flag2;
			lock (sync)
			{
				CustomizationPresetCollection customizationPresetCollection = this.LoadCollection();
				CustomizationPresetData customizationPresetData = CustomizationPresetRepository.FindPresetById(customizationPresetCollection, presetId);
				if (customizationPresetData == null)
				{
					flag2 = false;
				}
				else
				{
					customizationPresetData.stateData = CustomizationPresetRepository.CloneStateData(stateData);
					customizationPresetData.Touch();
					this.SaveCollection(customizationPresetCollection);
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x001F2918 File Offset: 0x001F0B18
		public bool TryUpdatePresetGender(string presetId, Gender gender)
		{
			if (string.IsNullOrWhiteSpace(presetId))
			{
				return false;
			}
			object sync = this._sync;
			bool flag2;
			lock (sync)
			{
				CustomizationPresetCollection customizationPresetCollection = this.LoadCollection();
				CustomizationPresetData customizationPresetData = CustomizationPresetRepository.FindPresetById(customizationPresetCollection, presetId);
				if (customizationPresetData == null)
				{
					flag2 = false;
				}
				else if (customizationPresetData.gender == gender)
				{
					flag2 = true;
				}
				else
				{
					customizationPresetData.gender = gender;
					customizationPresetData.Touch();
					this.SaveCollection(customizationPresetCollection);
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x0600507B RID: 20603 RVA: 0x001F299C File Offset: 0x001F0B9C
		private CustomizationPresetCollection LoadCollection()
		{
			string filePath = this.FilePath;
			if (!File.Exists(filePath))
			{
				return new CustomizationPresetCollection();
			}
			CustomizationPresetCollection customizationPresetCollection;
			try
			{
				string text = File.ReadAllText(filePath);
				if (string.IsNullOrWhiteSpace(text))
				{
					customizationPresetCollection = new CustomizationPresetCollection();
				}
				else
				{
					customizationPresetCollection = JsonUtility.FromJson<CustomizationPresetCollection>(text) ?? new CustomizationPresetCollection();
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CustomizationPresetRepository] Failed to read preset file. Recreating. Error: " + ex.Message);
				customizationPresetCollection = new CustomizationPresetCollection();
			}
			return customizationPresetCollection;
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x001F2A18 File Offset: 0x001F0C18
		private void SaveCollection(CustomizationPresetCollection collection)
		{
			string filePath = this.FilePath;
			string directoryName = Path.GetDirectoryName(filePath);
			if (!string.IsNullOrWhiteSpace(directoryName) && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			string text = JsonUtility.ToJson(collection, true);
			string text2 = filePath + ".tmp";
			File.WriteAllText(text2, text);
			if (File.Exists(filePath))
			{
				File.Replace(text2, filePath, null);
				return;
			}
			File.Move(text2, filePath);
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x001F2A80 File Offset: 0x001F0C80
		private static CustomizationPresetData FindPresetById(CustomizationPresetCollection collection, string presetId)
		{
			for (int i = 0; i < collection.presets.Count; i++)
			{
				CustomizationPresetData customizationPresetData = collection.presets[i];
				if (customizationPresetData != null && customizationPresetData.presetId == presetId)
				{
					return customizationPresetData;
				}
			}
			return null;
		}

		// Token: 0x0600507E RID: 20606 RVA: 0x001F2AC4 File Offset: 0x001F0CC4
		private static bool IsDeletionProtected(CustomizationPresetData preset)
		{
			return preset != null && (preset.isDefault || preset.isSystemPreset);
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x001F2ADC File Offset: 0x001F0CDC
		private static bool NormalizeInUseFlags(CustomizationPresetCollection collection, string characterId, CustomizationTargetType targetType)
		{
			if (collection == null || string.IsNullOrWhiteSpace(characterId) || targetType == CustomizationTargetType.None)
			{
				return false;
			}
			CustomizationPresetData customizationPresetData = null;
			for (int i = 0; i < collection.presets.Count; i++)
			{
				CustomizationPresetData customizationPresetData2 = collection.presets[i];
				if (customizationPresetData2 != null && !(customizationPresetData2.characterId != characterId) && customizationPresetData2.targetType == targetType)
				{
					if (customizationPresetData2.isDefault)
					{
						customizationPresetData = customizationPresetData2;
						break;
					}
					if (customizationPresetData == null && customizationPresetData2.isActive)
					{
						customizationPresetData = customizationPresetData2;
					}
				}
			}
			if (customizationPresetData == null)
			{
				for (int j = 0; j < collection.presets.Count; j++)
				{
					CustomizationPresetData customizationPresetData3 = collection.presets[j];
					if (customizationPresetData3 != null && customizationPresetData3.characterId == characterId && customizationPresetData3.targetType == targetType)
					{
						customizationPresetData = customizationPresetData3;
						break;
					}
				}
			}
			bool flag = false;
			for (int k = 0; k < collection.presets.Count; k++)
			{
				CustomizationPresetData customizationPresetData4 = collection.presets[k];
				if (customizationPresetData4 != null && !(customizationPresetData4.characterId != characterId) && customizationPresetData4.targetType == targetType)
				{
					bool flag2 = customizationPresetData4 == customizationPresetData;
					if (customizationPresetData4.isDefault != flag2 || customizationPresetData4.isActive)
					{
						customizationPresetData4.isDefault = flag2;
						customizationPresetData4.isActive = false;
						customizationPresetData4.Touch();
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x001F2C20 File Offset: 0x001F0E20
		public string GenerateUniquePresetName(string baseName, string characterId, CustomizationTargetType targetType)
		{
			if (string.IsNullOrWhiteSpace(baseName) || string.IsNullOrWhiteSpace(characterId))
			{
				return baseName;
			}
			object sync = this._sync;
			string text;
			lock (sync)
			{
				CustomizationPresetCollection customizationPresetCollection = this.LoadCollection();
				if (!CustomizationPresetRepository.HasPresetNameConflict(customizationPresetCollection, characterId, targetType, baseName, null))
				{
					text = baseName;
				}
				else
				{
					for (int i = 2; i <= 99; i++)
					{
						string text2 = string.Format("{0} ({1})", baseName, i);
						if (!CustomizationPresetRepository.HasPresetNameConflict(customizationPresetCollection, characterId, targetType, text2, null))
						{
							return text2;
						}
					}
					text = string.Format("{0} ({1:HHmmss})", baseName, DateTime.UtcNow);
				}
			}
			return text;
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x001F2CD4 File Offset: 0x001F0ED4
		private static bool HasPresetNameConflict(CustomizationPresetCollection collection, string characterId, CustomizationTargetType targetType, string presetName, string ignorePresetId)
		{
			if (collection == null || string.IsNullOrWhiteSpace(characterId) || string.IsNullOrWhiteSpace(presetName))
			{
				return false;
			}
			string text = presetName.Trim();
			for (int i = 0; i < collection.presets.Count; i++)
			{
				CustomizationPresetData customizationPresetData = collection.presets[i];
				if (customizationPresetData != null && (string.IsNullOrWhiteSpace(ignorePresetId) || !(customizationPresetData.presetId == ignorePresetId)) && !(customizationPresetData.characterId != characterId) && customizationPresetData.targetType == targetType)
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

		// Token: 0x06005082 RID: 20610 RVA: 0x001F2D70 File Offset: 0x001F0F70
		private static string DuplicateIconFile(string sourceIconFileName, string duplicatedPresetName)
		{
			if (string.IsNullOrWhiteSpace(sourceIconFileName))
			{
				return null;
			}
			string text = Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, sourceIconFileName);
			if (!File.Exists(text))
			{
				return sourceIconFileName;
			}
			string text5;
			try
			{
				GameDataPaths.EnsureCustomizationDirectories();
				string text2 = CustomizationPresetRepository.SanitizeFileName(string.IsNullOrWhiteSpace(duplicatedPresetName) ? "preset_copy" : duplicatedPresetName);
				string text3 = string.Format("{0}_{1:yyyyMMddHHmmss}.png", text2, DateTime.UtcNow);
				string text4 = Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, text3);
				File.Copy(text, text4, true);
				text5 = text3;
			}
			catch
			{
				text5 = sourceIconFileName;
			}
			return text5;
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x001F2E00 File Offset: 0x001F1000
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

		// Token: 0x06005084 RID: 20612 RVA: 0x001F2E34 File Offset: 0x001F1034
		private static string SanitizeFileName(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return "Preset";
			}
			foreach (char c in Path.GetInvalidFileNameChars())
			{
				value = value.Replace(c, '_');
			}
			return value;
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x001F2E74 File Offset: 0x001F1074
		private static void DeleteIconFileIfExists(string previousIconFileName, string newIconFileName)
		{
			if (string.IsNullOrWhiteSpace(previousIconFileName))
			{
				return;
			}
			if (!string.IsNullOrWhiteSpace(newIconFileName) && string.Equals(previousIconFileName, newIconFileName, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			string text = Path.Combine(GameDataPaths.CustomizationPresetIconsDirectory, previousIconFileName);
			if (!File.Exists(text))
			{
				return;
			}
			try
			{
				File.Delete(text);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CustomizationPresetRepository] Failed deleting icon '" + previousIconFileName + "': " + ex.Message);
			}
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x001F2EEC File Offset: 0x001F10EC
		private static CharacterStateData CloneStateData(CharacterStateData source)
		{
			return JsonUtility.FromJson<CharacterStateData>(JsonUtility.ToJson(source ?? new CharacterStateData())) ?? new CharacterStateData();
		}

		// Token: 0x0400512E RID: 20782
		[SerializeField]
		private string fileName = "customization_presets.json";

		// Token: 0x0400512F RID: 20783
		[SerializeField]
		private bool autoCreateDefaultPreset = true;

		// Token: 0x04005130 RID: 20784
		[SerializeField]
		private string defaultPresetName = "Default";

		// Token: 0x04005131 RID: 20785
		[SerializeField]
		private bool markAutoDefaultAsSystemPreset = true;

		// Token: 0x04005132 RID: 20786
		private readonly object _sync = new object();
	}
}
