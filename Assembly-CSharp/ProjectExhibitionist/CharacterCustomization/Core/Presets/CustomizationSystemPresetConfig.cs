using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Presets
{
	// Token: 0x02000B1C RID: 2844
	[CreateAssetMenu(fileName = "CustomizationSystemPresetConfig", menuName = "Customization/System Preset Config")]
	public class CustomizationSystemPresetConfig : ScriptableObject
	{
		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x0600508B RID: 20619 RVA: 0x001F2FDF File Offset: 0x001F11DF
		public string MainCharacterId
		{
			get
			{
				return this.mainCharacterId;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x0600508C RID: 20620 RVA: 0x001F2FE7 File Offset: 0x001F11E7
		public string MainCharacterPresetName
		{
			get
			{
				return this.mainCharacterPresetName;
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x0600508D RID: 20621 RVA: 0x001F2FEF File Offset: 0x001F11EF
		public CharacterObject MainCharacterBasePreset
		{
			get
			{
				return this.mainCharacterBasePreset;
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x0600508E RID: 20622 RVA: 0x001F2FF7 File Offset: 0x001F11F7
		public bool HasMainCharacterBasePreset
		{
			get
			{
				return this.mainCharacterBasePreset != null;
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x0600508F RID: 20623 RVA: 0x001F3005 File Offset: 0x001F1205
		public CharacterObject NpcNewPresetBaseAsset
		{
			get
			{
				return this.npcNewPresetBaseAsset;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06005090 RID: 20624 RVA: 0x001F300D File Offset: 0x001F120D
		public bool HasNpcNewPresetBaseAsset
		{
			get
			{
				return this.npcNewPresetBaseAsset != null;
			}
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x001F301C File Offset: 0x001F121C
		public SystemPresetEntry BuildMainCharacterSystemPreset()
		{
			if (this.mainCharacterBasePreset == null)
			{
				return null;
			}
			return new SystemPresetEntry
			{
				sourceAsset = this.mainCharacterBasePreset,
				presetName = this.mainCharacterPresetName,
				characterId = this.mainCharacterId,
				targetType = CustomizationTargetType.MainCharacter,
				isInactive = false
			};
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x001F3070 File Offset: 0x001F1270
		public List<SystemPresetEntry> BuildNpcSystemPresets()
		{
			List<SystemPresetEntry> list = new List<SystemPresetEntry>();
			if (this.npcSystemPresets == null || this.npcSystemPresets.Count == 0)
			{
				return list;
			}
			HashSet<CharacterObject> hashSet = new HashSet<CharacterObject>();
			for (int i = 0; i < this.inactiveNpcPresets.Count; i++)
			{
				if (this.inactiveNpcPresets[i] != null)
				{
					hashSet.Add(this.inactiveNpcPresets[i]);
				}
			}
			for (int j = 0; j < this.npcSystemPresets.Count; j++)
			{
				CharacterObject characterObject = this.npcSystemPresets[j];
				if (!(characterObject == null))
				{
					string text = CustomizationSystemPresetConfig.BuildNpcPresetName(characterObject);
					if (string.IsNullOrWhiteSpace(text))
					{
						text = characterObject.name;
					}
					list.Add(new SystemPresetEntry
					{
						sourceAsset = characterObject,
						presetName = text,
						characterId = characterObject.name,
						targetType = CustomizationTargetType.Npc,
						isInactive = hashSet.Contains(characterObject)
					});
				}
			}
			return list;
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x001F3168 File Offset: 0x001F1368
		private static string BuildNpcPresetName(CharacterObject asset)
		{
			if (asset.data == null || asset.data.generalInfo == null)
			{
				if (asset.data == null)
				{
					return asset.name;
				}
				return asset.data.characterName;
			}
			else
			{
				CharacterGeneralInfo generalInfo = asset.data.generalInfo;
				string text = generalInfo.characterName ?? "";
				string text2 = generalInfo.surname ?? "";
				if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(text2))
				{
					return text.Trim() + " " + text2.Trim();
				}
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text.Trim();
				}
				return asset.data.characterName ?? asset.name;
			}
		}

		// Token: 0x04005135 RID: 20789
		[Header("Main Character")]
		[Tooltip("The base CharacterObject SO that defines the MC default system preset.")]
		[SerializeField]
		private CharacterObject mainCharacterBasePreset;

		// Token: 0x04005136 RID: 20790
		[Tooltip("Display name for the MC system preset in the hub.")]
		[SerializeField]
		private string mainCharacterPresetName = "Ciel (Default)";

		// Token: 0x04005137 RID: 20791
		[Tooltip("Character ID used for MC presets.")]
		[SerializeField]
		private string mainCharacterId = "Ciel";

		// Token: 0x04005138 RID: 20792
		[Header("NPC System Presets")]
		[Tooltip("Explicit list of NPC CharacterObject SOs to include as system presets.")]
		[SerializeField]
		private List<CharacterObject> npcSystemPresets = new List<CharacterObject>();

		// Token: 0x04005139 RID: 20793
		[Tooltip("NPC CharacterObject SOs to mark as inactive (hidden by default, player-toggled).")]
		[SerializeField]
		private List<CharacterObject> inactiveNpcPresets = new List<CharacterObject>();

		// Token: 0x0400513A RID: 20794
		[Tooltip("Base CharacterObject usato come look di partenza quando si crea un NUOVO NPC (bottone Add). Analogo a mainCharacterBasePreset per il MC.")]
		[SerializeField]
		private CharacterObject npcNewPresetBaseAsset;
	}
}
