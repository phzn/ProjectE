using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000844 RID: 2116
	[Serializable]
	public class ColorValue
	{
		// Token: 0x060037FF RID: 14335 RVA: 0x00147A13 File Offset: 0x00145C13
		public ColorValue(string category, string subcategory, string colorProperty, Color color)
		{
			this.category = category;
			this.subcategory = subcategory;
			this.colorProperty = colorProperty;
			this.color = color;
		}

		// Token: 0x040038FB RID: 14587
		public string category;

		// Token: 0x040038FC RID: 14588
		public string subcategory;

		// Token: 0x040038FD RID: 14589
		public string colorProperty;

		// Token: 0x040038FE RID: 14590
		public Color color;
	}
}
