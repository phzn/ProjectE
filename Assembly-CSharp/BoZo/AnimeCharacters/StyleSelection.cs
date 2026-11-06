using System;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000842 RID: 2114
	[Serializable]
	public class StyleSelection
	{
		// Token: 0x060037FD RID: 14333 RVA: 0x001479C9 File Offset: 0x00145BC9
		public StyleSelection(string category, string subcategory, string styleItemName, string part = "")
		{
			this.category = category;
			this.subcategory = subcategory;
			this.part = part;
			this.styleItemName = styleItemName;
		}

		// Token: 0x040038F2 RID: 14578
		public string category;

		// Token: 0x040038F3 RID: 14579
		public string subcategory;

		// Token: 0x040038F4 RID: 14580
		public string part;

		// Token: 0x040038F5 RID: 14581
		public string styleItemName;

		// Token: 0x040038F6 RID: 14582
		public string styleItemPath;
	}
}
