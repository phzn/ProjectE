using System;
using Bozo.AnimeCharacters;

namespace ProjectExhibitionist.CharacterCustomization.Core.Presets
{
	// Token: 0x02000B19 RID: 2841
	[Serializable]
	public class CustomizationPresetData
	{
		// Token: 0x0600506E RID: 20590 RVA: 0x001F2140 File Offset: 0x001F0340
		public static CustomizationPresetData Create(string presetName, string characterId, CustomizationTargetType targetType, bool isDefault, CharacterStateData stateData, bool isSystemPreset = false)
		{
			string text = DateTime.UtcNow.ToString("O");
			return new CustomizationPresetData
			{
				presetId = Guid.NewGuid().ToString("N"),
				presetName = presetName,
				characterId = characterId,
				targetType = targetType,
				isDefault = isDefault,
				isSystemPreset = isSystemPreset,
				createdAtUtc = text,
				updatedAtUtc = text,
				stateData = (stateData ?? new CharacterStateData())
			};
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x001F21C0 File Offset: 0x001F03C0
		public void Touch()
		{
			this.updatedAtUtc = DateTime.UtcNow.ToString("O");
		}

		// Token: 0x04005122 RID: 20770
		public string presetId;

		// Token: 0x04005123 RID: 20771
		public string presetName;

		// Token: 0x04005124 RID: 20772
		public string characterId;

		// Token: 0x04005125 RID: 20773
		public CustomizationTargetType targetType;

		// Token: 0x04005126 RID: 20774
		public bool isDefault;

		// Token: 0x04005127 RID: 20775
		public bool isSystemPreset;

		// Token: 0x04005128 RID: 20776
		public bool isActive;

		// Token: 0x04005129 RID: 20777
		public Gender gender;

		// Token: 0x0400512A RID: 20778
		public string iconFileName;

		// Token: 0x0400512B RID: 20779
		public string createdAtUtc;

		// Token: 0x0400512C RID: 20780
		public string updatedAtUtc;

		// Token: 0x0400512D RID: 20781
		public CharacterStateData stateData = new CharacterStateData();
	}
}
