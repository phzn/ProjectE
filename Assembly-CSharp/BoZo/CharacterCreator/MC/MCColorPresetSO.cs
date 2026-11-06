using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoZo.CharacterCreator.MC
{
	// Token: 0x020007D2 RID: 2002
	[CreateAssetMenu(fileName = "MCColorPreset", menuName = "Character Creator/MC Color Preset")]
	public class MCColorPresetSO : ScriptableObject
	{
		// Token: 0x06003526 RID: 13606 RVA: 0x0012EB80 File Offset: 0x0012CD80
		public bool TryGetPreset(int index, out MCColorPresetSO.ColorPresetEntry preset)
		{
			preset = null;
			if (index < 0 || index >= this.presets.Count)
			{
				return false;
			}
			preset = this.presets[index];
			return preset != null;
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x0012EBAC File Offset: 0x0012CDAC
		public Color GetDefaultColor()
		{
			MCColorPresetSO.ColorPresetEntry colorPresetEntry;
			if (this.TryGetPreset(this.defaultPresetIndex, out colorPresetEntry))
			{
				return colorPresetEntry.color;
			}
			if (this.presets != null && this.presets.Count > 0 && this.presets[0] != null)
			{
				return this.presets[0].color;
			}
			return Color.white;
		}

		// Token: 0x0400356B RID: 13675
		[Header("Part")]
		public MCColorPart targetPart;

		// Token: 0x0400356C RID: 13676
		public bool allowCustomPicker = true;

		// Token: 0x0400356D RID: 13677
		[Min(0f)]
		public int defaultPresetIndex;

		// Token: 0x0400356E RID: 13678
		[Header("Color Presets")]
		public List<MCColorPresetSO.ColorPresetEntry> presets = new List<MCColorPresetSO.ColorPresetEntry>();

		// Token: 0x020007D3 RID: 2003
		[Serializable]
		public class ColorPresetEntry
		{
			// Token: 0x0400356F RID: 13679
			public string presetId = "preset";

			// Token: 0x04003570 RID: 13680
			public string displayName = "Preset";

			// Token: 0x04003571 RID: 13681
			public Color color = Color.white;
		}
	}
}
