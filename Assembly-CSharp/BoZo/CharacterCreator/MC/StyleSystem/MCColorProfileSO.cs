using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007D7 RID: 2007
	[CreateAssetMenu(fileName = "MCColorProfile", menuName = "Character Creator/MC/Style Color Profile")]
	public class MCColorProfileSO : ScriptableObject
	{
		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06003544 RID: 13636 RVA: 0x0012F693 File Offset: 0x0012D893
		public int ClampedChannelCount
		{
			get
			{
				return Mathf.Clamp(this.channelCount, 1, 2);
			}
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x0012F6A2 File Offset: 0x0012D8A2
		public bool TryGetPreset(int index, out MCColorProfileSO.ColorPresetEntry preset)
		{
			preset = null;
			if (index < 0 || index >= this.presets.Count)
			{
				return false;
			}
			preset = this.presets[index];
			return preset != null;
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x0012F6D0 File Offset: 0x0012D8D0
		public void OnValidate()
		{
			this.channelCount = Mathf.Clamp(this.channelCount, 1, 2);
			if (this.channels == null)
			{
				this.channels = new List<MCColorProfileSO.ColorChannelBinding>();
			}
			if (this.channels.Count > 2)
			{
				this.channels.RemoveRange(2, this.channels.Count - 2);
			}
			for (int i = 0; i < this.channels.Count; i++)
			{
				if (this.channels[i] == null)
				{
					this.channels[i] = new MCColorProfileSO.ColorChannelBinding();
				}
				this.channels[i].channelId = ((i == 0) ? MCColorChannelId.Primary : MCColorChannelId.Secondary);
			}
		}

		// Token: 0x0400357E RID: 13694
		[Header("Channels")]
		[Range(1f, 2f)]
		public int channelCount = 1;

		// Token: 0x0400357F RID: 13695
		public List<MCColorProfileSO.ColorChannelBinding> channels = new List<MCColorProfileSO.ColorChannelBinding>();

		// Token: 0x04003580 RID: 13696
		[Header("Selection")]
		public bool allowCustomPicker = true;

		// Token: 0x04003581 RID: 13697
		[Min(0f)]
		public int defaultPresetIndex;

		// Token: 0x04003582 RID: 13698
		public List<MCColorProfileSO.ColorPresetEntry> presets = new List<MCColorProfileSO.ColorPresetEntry>();

		// Token: 0x020007D8 RID: 2008
		[Serializable]
		public class ColorChannelBinding
		{
			// Token: 0x04003583 RID: 13699
			public MCColorChannelId channelId;

			// Token: 0x04003584 RID: 13700
			public MCColorCustomizationService.ColorLayer shaderLayer;

			// Token: 0x04003585 RID: 13701
			public string colorPropertyNameOverride = string.Empty;

			// Token: 0x04003586 RID: 13702
			public string blendModePropertyNameOverride = string.Empty;

			// Token: 0x04003587 RID: 13703
			public string enablePropertyNameOverride = string.Empty;

			// Token: 0x04003588 RID: 13704
			public string maskPropertyNameOverride = string.Empty;

			// Token: 0x04003589 RID: 13705
			public Texture maskTexture;
		}

		// Token: 0x020007D9 RID: 2009
		[Serializable]
		public class ColorPresetEntry
		{
			// Token: 0x0400358A RID: 13706
			public string presetId = "preset";

			// Token: 0x0400358B RID: 13707
			public string displayName = "Preset";

			// Token: 0x0400358C RID: 13708
			public Color primaryColor = Color.white;

			// Token: 0x0400358D RID: 13709
			public Color secondaryColor = Color.white;
		}
	}
}
