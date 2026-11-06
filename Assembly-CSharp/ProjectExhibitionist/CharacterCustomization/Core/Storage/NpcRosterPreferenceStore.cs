using System;
using System.Collections.Generic;
using System.IO;
using Bozo.AnimeCharacters;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Storage
{
	// Token: 0x02000ADF RID: 2783
	public sealed class NpcRosterPreferenceStore
	{
		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06004ECC RID: 20172 RVA: 0x001E847C File Offset: 0x001E667C
		// (remove) Token: 0x06004ECD RID: 20173 RVA: 0x001E84B4 File Offset: 0x001E66B4
		public event Action Changed;

		// Token: 0x06004ECE RID: 20174 RVA: 0x001E84E9 File Offset: 0x001E66E9
		public NpcRosterPreferenceStore(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException("A roster preferences file path is required.", "filePath");
			}
			this._filePath = filePath;
			this.Load();
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06004ECF RID: 20175 RVA: 0x001E8526 File Offset: 0x001E6726
		// (set) Token: 0x06004ED0 RID: 20176 RVA: 0x001E8530 File Offset: 0x001E6730
		public NpcPopulationMode PopulationMode
		{
			get
			{
				return this._populationMode;
			}
			set
			{
				if (!Enum.IsDefined(typeof(NpcPopulationMode), value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this._populationMode != value)
				{
					string text;
					this.Commit(value, this._overrides, out text);
				}
			}
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x001E8578 File Offset: 0x001E6778
		public bool IsEnabled(CustomizationPresetData preset)
		{
			if (!NpcRosterPreferenceStore.IsValidNpc(preset))
			{
				return false;
			}
			bool flag;
			if (!this._overrides.TryGetValue(preset.presetId, out flag))
			{
				return preset.isSystemPreset;
			}
			return flag;
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x001E85AC File Offset: 0x001E67AC
		public bool IsEligible(CustomizationPresetData preset)
		{
			return this.IsEnabled(preset) && this.MatchesPopulation(preset);
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x001E85C0 File Offset: 0x001E67C0
		public int CountEnabled(IReadOnlyList<CustomizationPresetData> catalog)
		{
			return this.CountEnabled(catalog, null);
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x001E85DD File Offset: 0x001E67DD
		public int CountEnabled(IReadOnlyList<CustomizationPresetData> catalog, Gender gender)
		{
			return this.CountEnabled(catalog, new Gender?(gender));
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x001E85EC File Offset: 0x001E67EC
		private int CountEnabled(IReadOnlyList<CustomizationPresetData> catalog, Gender? gender)
		{
			int num = 0;
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			if (catalog == null)
			{
				return num;
			}
			for (int i = 0; i < catalog.Count; i++)
			{
				CustomizationPresetData customizationPresetData = catalog[i];
				if (NpcRosterPreferenceStore.IsValidNpc(customizationPresetData) && hashSet.Add(customizationPresetData.presetId) && this.IsEnabled(customizationPresetData) && (gender == null || customizationPresetData.gender == gender.Value))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x001E8660 File Offset: 0x001E6860
		public int CountEnabledCustom(IReadOnlyList<CustomizationPresetData> catalog)
		{
			int num = 0;
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			if (catalog == null)
			{
				return num;
			}
			for (int i = 0; i < catalog.Count; i++)
			{
				CustomizationPresetData customizationPresetData = catalog[i];
				if (NpcRosterPreferenceStore.IsValidNpc(customizationPresetData) && hashSet.Add(customizationPresetData.presetId) && !customizationPresetData.isSystemPreset && this.IsEnabled(customizationPresetData))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x001E86C4 File Offset: 0x001E68C4
		public int CountEligible(IReadOnlyList<CustomizationPresetData> catalog)
		{
			int num = 0;
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			if (catalog == null)
			{
				return num;
			}
			for (int i = 0; i < catalog.Count; i++)
			{
				CustomizationPresetData customizationPresetData = catalog[i];
				if (NpcRosterPreferenceStore.IsValidNpc(customizationPresetData) && hashSet.Add(customizationPresetData.presetId) && this.IsEligible(customizationPresetData))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x001E8720 File Offset: 0x001E6920
		public bool TrySetEnabled(CustomizationPresetData preset, bool enabled, IReadOnlyList<CustomizationPresetData> catalog, int minimumPerGender, out string reason)
		{
			if (!enabled && minimumPerGender > 0 && this.IsEnabled(preset) && this.CountEnabled(catalog, preset.gender) <= minimumPerGender)
			{
				string text = ((preset.gender == Gender.Male) ? "male" : "female");
				reason = string.Format("Keep at least {0} {1} NPCs active.", minimumPerGender, text);
				return false;
			}
			return this.TrySetEnabled(preset, enabled, catalog, out reason);
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x001E8788 File Offset: 0x001E6988
		public bool TrySetEnabled(CustomizationPresetData preset, bool enabled, IReadOnlyList<CustomizationPresetData> catalog, out string reason)
		{
			reason = null;
			if (!NpcRosterPreferenceStore.IsValidNpc(preset))
			{
				reason = "This NPC preset is not available.";
				return false;
			}
			if (this.IsEnabled(preset) == enabled)
			{
				return true;
			}
			if (enabled && !preset.isSystemPreset)
			{
				bool flag = false;
				if (catalog != null)
				{
					for (int i = 0; i < catalog.Count; i++)
					{
						if (NpcRosterPreferenceStore.IsValidNpc(catalog[i]) && catalog[i].presetId == preset.presetId)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					reason = "Refresh the NPC list before enabling this preset.";
					return false;
				}
				if (this.CountEnabledCustom(catalog) >= 4)
				{
					reason = "Up to 4 custom NPCs can be enabled. Disable one to make room.";
					return false;
				}
			}
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>(this._overrides, StringComparer.Ordinal);
			if (enabled == preset.isSystemPreset)
			{
				dictionary.Remove(preset.presetId);
			}
			else
			{
				dictionary[preset.presetId] = enabled;
			}
			return this.Commit(this._populationMode, dictionary, out reason);
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x001E886C File Offset: 0x001E6A6C
		public int EnableSystemToMinimum(IReadOnlyList<CustomizationPresetData> catalog, int minimum)
		{
			int num = Math.Max(0, minimum - this.CountEligible(catalog));
			if (catalog == null || num == 0)
			{
				return 0;
			}
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>(this._overrides, StringComparer.Ordinal);
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			int num2 = 0;
			int num3 = 0;
			while (num3 < catalog.Count && num2 < num)
			{
				CustomizationPresetData customizationPresetData = catalog[num3];
				if (NpcRosterPreferenceStore.IsValidNpc(customizationPresetData) && hashSet.Add(customizationPresetData.presetId) && customizationPresetData.isSystemPreset && this.MatchesPopulation(customizationPresetData) && !this.IsEnabled(customizationPresetData))
				{
					dictionary.Remove(customizationPresetData.presetId);
					num2++;
				}
				num3++;
			}
			string text;
			if (num2 <= 0 || !this.Commit(this._populationMode, dictionary, out text))
			{
				return 0;
			}
			return num2;
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x001E8934 File Offset: 0x001E6B34
		public int EnableSystemToMinimumPerGender(IReadOnlyList<CustomizationPresetData> catalog, int minimum)
		{
			if (catalog == null || minimum <= 0)
			{
				return 0;
			}
			int num = Math.Max(0, minimum - this.CountEnabled(catalog, Gender.Male));
			int num2 = Math.Max(0, minimum - this.CountEnabled(catalog, Gender.Female));
			if (num == 0 && num2 == 0)
			{
				return 0;
			}
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>(this._overrides, StringComparer.Ordinal);
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			int num3 = 0;
			int num4 = 0;
			while (num4 < catalog.Count && (num > 0 || num2 > 0))
			{
				CustomizationPresetData customizationPresetData = catalog[num4];
				if (NpcRosterPreferenceStore.IsValidNpc(customizationPresetData) && hashSet.Add(customizationPresetData.presetId) && customizationPresetData.isSystemPreset && !this.IsEnabled(customizationPresetData))
				{
					if (customizationPresetData.gender == Gender.Male && num > 0)
					{
						num--;
					}
					else
					{
						if (customizationPresetData.gender != Gender.Female || num2 <= 0)
						{
							goto IL_00C5;
						}
						num2--;
					}
					dictionary.Remove(customizationPresetData.presetId);
					num3++;
				}
				IL_00C5:
				num4++;
			}
			string text;
			if (num3 <= 0 || !this.Commit(this._populationMode, dictionary, out text))
			{
				return 0;
			}
			return num3;
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x001E8A40 File Offset: 0x001E6C40
		public void Remove(string presetId)
		{
			if (string.IsNullOrWhiteSpace(presetId) || !this._overrides.ContainsKey(presetId))
			{
				return;
			}
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>(this._overrides, StringComparer.Ordinal);
			dictionary.Remove(presetId);
			string text;
			this.Commit(this._populationMode, dictionary, out text);
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x001E8A8D File Offset: 0x001E6C8D
		private bool MatchesPopulation(CustomizationPresetData preset)
		{
			return this._populationMode == NpcPopulationMode.Mixed || (this._populationMode == NpcPopulationMode.MaleOnly && preset.gender == Gender.Male) || (this._populationMode == NpcPopulationMode.FemaleOnly && preset.gender == Gender.Female);
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x001E8ABE File Offset: 0x001E6CBE
		private static bool IsValidNpc(CustomizationPresetData preset)
		{
			return preset != null && preset.targetType == CustomizationTargetType.Npc && !string.IsNullOrWhiteSpace(preset.presetId);
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x001E8ADC File Offset: 0x001E6CDC
		private void Load()
		{
			try
			{
				if (File.Exists(this._filePath))
				{
					string text = File.ReadAllText(this._filePath);
					if (!string.IsNullOrWhiteSpace(text))
					{
						NpcRosterPreferenceStore.Document document = JsonUtility.FromJson<NpcRosterPreferenceStore.Document>(text);
						if (document != null && document.version == 1)
						{
							this._populationMode = (Enum.IsDefined(typeof(NpcPopulationMode), document.populationMode) ? document.populationMode : NpcPopulationMode.Mixed);
							if (document.entries != null)
							{
								foreach (NpcRosterPreferenceStore.Entry entry in document.entries)
								{
									if (entry != null && !string.IsNullOrWhiteSpace(entry.presetId))
									{
										this._overrides[entry.presetId] = entry.enabled;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NpcRosterPreferences] Could not read roster preferences: " + ex.Message);
			}
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x001E8BF0 File Offset: 0x001E6DF0
		private bool Commit(NpcPopulationMode mode, Dictionary<string, bool> overrides, out string reason)
		{
			reason = null;
			string text = this._filePath + ".tmp";
			try
			{
				string directoryName = Path.GetDirectoryName(this._filePath);
				if (!string.IsNullOrEmpty(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				NpcRosterPreferenceStore.Document document = new NpcRosterPreferenceStore.Document
				{
					version = 1,
					populationMode = mode,
					entries = new List<NpcRosterPreferenceStore.Entry>()
				};
				foreach (KeyValuePair<string, bool> keyValuePair in overrides)
				{
					document.entries.Add(new NpcRosterPreferenceStore.Entry
					{
						presetId = keyValuePair.Key,
						enabled = keyValuePair.Value
					});
				}
				document.entries.Sort((NpcRosterPreferenceStore.Entry left, NpcRosterPreferenceStore.Entry right) => string.CompareOrdinal(left.presetId, right.presetId));
				File.WriteAllText(text, JsonUtility.ToJson(document, true));
				if (File.Exists(this._filePath))
				{
					File.Replace(text, this._filePath, null);
				}
				else
				{
					File.Move(text, this._filePath);
				}
			}
			catch (Exception ex)
			{
				reason = "Could not save NPC preferences. Please try again.";
				Debug.LogWarning("[NpcRosterPreferences] " + reason + " " + ex.Message);
				return false;
			}
			this._populationMode = mode;
			this._overrides = overrides;
			Action changed = this.Changed;
			if (changed != null)
			{
				changed();
			}
			return true;
		}

		// Token: 0x04005015 RID: 20501
		private readonly string _filePath;

		// Token: 0x04005016 RID: 20502
		private Dictionary<string, bool> _overrides = new Dictionary<string, bool>(StringComparer.Ordinal);

		// Token: 0x04005017 RID: 20503
		private NpcPopulationMode _populationMode;

		// Token: 0x02000AE0 RID: 2784
		[Serializable]
		private sealed class Entry
		{
			// Token: 0x04005019 RID: 20505
			public string presetId;

			// Token: 0x0400501A RID: 20506
			public bool enabled;
		}

		// Token: 0x02000AE1 RID: 2785
		[Serializable]
		private sealed class Document
		{
			// Token: 0x0400501B RID: 20507
			public int version;

			// Token: 0x0400501C RID: 20508
			public NpcPopulationMode populationMode;

			// Token: 0x0400501D RID: 20509
			public List<NpcRosterPreferenceStore.Entry> entries;
		}
	}
}
