using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008AA RID: 2218
	[CreateAssetMenu(fileName = "ColorVariants_", menuName = "Character Creator/Outfit Color Variant Set")]
	public class OutfitColorVariantSetSO : ScriptableObject
	{
		// Token: 0x04003BC3 RID: 15299
		public List<OutfitColorVariantSetSO.Variant> variants = new List<OutfitColorVariantSetSO.Variant>();

		// Token: 0x020008AB RID: 2219
		[Serializable]
		public class Variant
		{
			// Token: 0x04003BC4 RID: 15300
			public string variantName;

			// Token: 0x04003BC5 RID: 15301
			public List<Color> channelColors = new List<Color>();
		}
	}
}
