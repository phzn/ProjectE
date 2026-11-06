using System;
using System.Collections.Generic;
using System.IO;
using Bozo.AnimeCharacters;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;

namespace ProjectExhibitionist.CharacterCustomization.Core.Storage
{
	// Token: 0x02000ADE RID: 2782
	public static class NpcRosterPreferences
	{
		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06004EBC RID: 20156 RVA: 0x001E8391 File Offset: 0x001E6591
		// (remove) Token: 0x06004EBD RID: 20157 RVA: 0x001E839E File Offset: 0x001E659E
		public static event Action Changed
		{
			add
			{
				NpcRosterPreferences.Store.Changed += value;
			}
			remove
			{
				NpcRosterPreferences.Store.Changed -= value;
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06004EBE RID: 20158 RVA: 0x001E83AB File Offset: 0x001E65AB
		// (set) Token: 0x06004EBF RID: 20159 RVA: 0x001E83B7 File Offset: 0x001E65B7
		public static NpcPopulationMode PopulationMode
		{
			get
			{
				return NpcRosterPreferences.Store.PopulationMode;
			}
			set
			{
				NpcRosterPreferences.Store.PopulationMode = value;
			}
		}

		// Token: 0x06004EC0 RID: 20160 RVA: 0x001E83C4 File Offset: 0x001E65C4
		public static bool IsEnabled(CustomizationPresetData preset)
		{
			return NpcRosterPreferences.Store.IsEnabled(preset);
		}

		// Token: 0x06004EC1 RID: 20161 RVA: 0x001E83D1 File Offset: 0x001E65D1
		public static bool IsEligible(CustomizationPresetData preset)
		{
			return NpcRosterPreferences.Store.IsEligible(preset);
		}

		// Token: 0x06004EC2 RID: 20162 RVA: 0x001E83DE File Offset: 0x001E65DE
		public static int CountEnabled(IReadOnlyList<CustomizationPresetData> catalog)
		{
			return NpcRosterPreferences.Store.CountEnabled(catalog);
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x001E83EB File Offset: 0x001E65EB
		public static int CountEnabled(IReadOnlyList<CustomizationPresetData> catalog, Gender gender)
		{
			return NpcRosterPreferences.Store.CountEnabled(catalog, gender);
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x001E83F9 File Offset: 0x001E65F9
		public static int CountEnabledCustom(IReadOnlyList<CustomizationPresetData> catalog)
		{
			return NpcRosterPreferences.Store.CountEnabledCustom(catalog);
		}

		// Token: 0x06004EC5 RID: 20165 RVA: 0x001E8406 File Offset: 0x001E6606
		public static int CountEligible(IReadOnlyList<CustomizationPresetData> catalog)
		{
			return NpcRosterPreferences.Store.CountEligible(catalog);
		}

		// Token: 0x06004EC6 RID: 20166 RVA: 0x001E8413 File Offset: 0x001E6613
		public static bool TrySetEnabled(CustomizationPresetData preset, bool enabled, IReadOnlyList<CustomizationPresetData> catalog, out string reason)
		{
			return NpcRosterPreferences.Store.TrySetEnabled(preset, enabled, catalog, out reason);
		}

		// Token: 0x06004EC7 RID: 20167 RVA: 0x001E8423 File Offset: 0x001E6623
		public static bool TrySetEnabled(CustomizationPresetData preset, bool enabled, IReadOnlyList<CustomizationPresetData> catalog, int minimumPerGender, out string reason)
		{
			return NpcRosterPreferences.Store.TrySetEnabled(preset, enabled, catalog, minimumPerGender, out reason);
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x001E8435 File Offset: 0x001E6635
		public static int EnableSystemToMinimum(IReadOnlyList<CustomizationPresetData> catalog, int minimum)
		{
			return NpcRosterPreferences.Store.EnableSystemToMinimum(catalog, minimum);
		}

		// Token: 0x06004EC9 RID: 20169 RVA: 0x001E8443 File Offset: 0x001E6643
		public static int EnableSystemToMinimumPerGender(IReadOnlyList<CustomizationPresetData> catalog, int minimum)
		{
			return NpcRosterPreferences.Store.EnableSystemToMinimumPerGender(catalog, minimum);
		}

		// Token: 0x06004ECA RID: 20170 RVA: 0x001E8451 File Offset: 0x001E6651
		public static void Remove(string presetId)
		{
			NpcRosterPreferences.Store.Remove(presetId);
		}

		// Token: 0x04005013 RID: 20499
		public const int MaxEnabledCustom = 4;

		// Token: 0x04005014 RID: 20500
		private static readonly NpcRosterPreferenceStore Store = new NpcRosterPreferenceStore(Path.Combine(GameDataPaths.CustomizationRoot, "npc_roster_preferences.v1.json"));
	}
}
