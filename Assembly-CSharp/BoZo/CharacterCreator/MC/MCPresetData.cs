using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoZo.CharacterCreator.MC
{
	// Token: 0x020007CC RID: 1996
	[Serializable]
	public class MCPresetData
	{
		// Token: 0x04003555 RID: 13653
		public string presetName = "My Character";

		// Token: 0x04003556 RID: 13654
		public string bio = "";

		// Token: 0x04003557 RID: 13655
		[Tooltip("Uniform scale applied to the 3D Model transform.")]
		public float heightScale = 1f;

		// Token: 0x04003558 RID: 13656
		[Tooltip("Per-blend-shape UI values keyed by displayName.")]
		public List<MCPresetData.BlendShapeValue> blendShapeValues = new List<MCPresetData.BlendShapeValue>();

		// Token: 0x04003559 RID: 13657
		[Tooltip("Per-material color values keyed by groupLabel or propertyName.")]
		public List<MCPresetData.ColorValue> colorValues = new List<MCPresetData.ColorValue>();

		// Token: 0x0400355A RID: 13658
		[Tooltip("Clothing toggle states keyed by group name.")]
		public List<MCPresetData.ClothingToggleValue> clothingToggles = new List<MCPresetData.ClothingToggleValue>();

		// Token: 0x0400355B RID: 13659
		[Tooltip("Decal states keyed by decal id.")]
		public List<MCPresetData.DecalValue> decalValues = new List<MCPresetData.DecalValue>();

		// Token: 0x020007CD RID: 1997
		[Serializable]
		public class BlendShapeValue
		{
			// Token: 0x0400355C RID: 13660
			public string displayName;

			// Token: 0x0400355D RID: 13661
			public float value;
		}

		// Token: 0x020007CE RID: 1998
		[Serializable]
		public class ColorValue
		{
			// Token: 0x0400355E RID: 13662
			public string key;

			// Token: 0x0400355F RID: 13663
			public Color color;
		}

		// Token: 0x020007CF RID: 1999
		[Serializable]
		public class ClothingToggleValue
		{
			// Token: 0x04003560 RID: 13664
			public string groupName;

			// Token: 0x04003561 RID: 13665
			public int activeIndex;
		}

		// Token: 0x020007D0 RID: 2000
		[Serializable]
		public class DecalValue
		{
			// Token: 0x04003562 RID: 13666
			public string decalId;

			// Token: 0x04003563 RID: 13667
			public bool enabled;

			// Token: 0x04003564 RID: 13668
			public float opacity;
		}
	}
}
