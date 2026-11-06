using System;
using System.Collections.Generic;
using Features.NeckSigns;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000840 RID: 2112
	[Serializable]
	public class CharacterStateData
	{
		// Token: 0x060037FA RID: 14330 RVA: 0x00147718 File Offset: 0x00145918
		public CharacterStateData()
		{
			this.subcategoryStyles = new List<StyleSelection>();
			this.partStyles = new List<StyleSelection>();
			this.blendShapes = new List<BlendShapeValue>();
			this.styleMorphs = new List<StyleMorphValue>();
			this.colors = new List<ColorValue>();
			this.clothingToggles = new List<ClothingToggleValue>();
			this.skinDecals = new List<SkinDecalStateValue>();
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x001477DC File Offset: 0x001459DC
		public CharacterStateData(CharacterStateData original)
		{
			this.mainCharacterBio = original.mainCharacterBio;
			this.subcategoryStyles = new List<StyleSelection>(original.subcategoryStyles);
			this.partStyles = new List<StyleSelection>(original.partStyles);
			this.blendShapes = new List<BlendShapeValue>(original.blendShapes);
			this.styleMorphs = new List<StyleMorphValue>();
			if (original.styleMorphs != null)
			{
				foreach (StyleMorphValue styleMorphValue in original.styleMorphs)
				{
					if (styleMorphValue != null)
					{
						this.styleMorphs.Add(new StyleMorphValue(styleMorphValue.partId, styleMorphValue.presetId, styleMorphValue.controlId, styleMorphValue.value));
					}
				}
			}
			this.colors = new List<ColorValue>(original.colors);
			this.clothingToggles = new List<ClothingToggleValue>(original.clothingToggles);
			this.skinDecals = new List<SkinDecalStateValue>(original.skinDecals);
			this.neckSignDesigns = new List<NeckSignDesignData>();
			if (original.neckSignDesigns != null)
			{
				foreach (NeckSignDesignData neckSignDesignData in original.neckSignDesigns)
				{
					if (neckSignDesignData != null)
					{
						this.neckSignDesigns.Add(neckSignDesignData.Clone());
					}
				}
			}
		}

		// Token: 0x040038E5 RID: 14565
		public string mainCharacterBio = string.Empty;

		// Token: 0x040038E6 RID: 14566
		public List<StyleSelection> subcategoryStyles = new List<StyleSelection>();

		// Token: 0x040038E7 RID: 14567
		public List<StyleSelection> partStyles = new List<StyleSelection>();

		// Token: 0x040038E8 RID: 14568
		public List<BlendShapeValue> blendShapes = new List<BlendShapeValue>();

		// Token: 0x040038E9 RID: 14569
		public List<StyleMorphValue> styleMorphs = new List<StyleMorphValue>();

		// Token: 0x040038EA RID: 14570
		public List<ColorValue> colors = new List<ColorValue>();

		// Token: 0x040038EB RID: 14571
		public List<ClothingToggleValue> clothingToggles = new List<ClothingToggleValue>();

		// Token: 0x040038EC RID: 14572
		public List<SkinDecalStateValue> skinDecals = new List<SkinDecalStateValue>();

		// Token: 0x040038ED RID: 14573
		public List<NeckSignDesignData> neckSignDesigns = new List<NeckSignDesignData>();
	}
}
