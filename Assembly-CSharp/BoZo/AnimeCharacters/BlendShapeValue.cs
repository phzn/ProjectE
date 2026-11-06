using System;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000843 RID: 2115
	[Serializable]
	public class BlendShapeValue
	{
		// Token: 0x060037FE RID: 14334 RVA: 0x001479EE File Offset: 0x00145BEE
		public BlendShapeValue(string category, string subcategory, string blendShapeName, float value)
		{
			this.category = category;
			this.subcategory = subcategory;
			this.blendShapeName = blendShapeName;
			this.value = value;
		}

		// Token: 0x040038F7 RID: 14583
		public string category;

		// Token: 0x040038F8 RID: 14584
		public string subcategory;

		// Token: 0x040038F9 RID: 14585
		public string blendShapeName;

		// Token: 0x040038FA RID: 14586
		public float value;
	}
}
