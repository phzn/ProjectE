using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000047 RID: 71
	[AttributeUsage(AttributeTargets.Class)]
	internal class MenuCategoryAttribute : Attribute
	{
		// Token: 0x060002BD RID: 701 RVA: 0x00009AEF File Offset: 0x00007CEF
		public MenuCategoryAttribute(string category)
		{
			this.category = category ?? string.Empty;
		}

		// Token: 0x040000F4 RID: 244
		public readonly string category;
	}
}
