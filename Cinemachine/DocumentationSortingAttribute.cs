using System;

namespace Cinemachine
{
	// Token: 0x02000044 RID: 68
	[DocumentationSorting(DocumentationSortingAttribute.Level.Undoc)]
	public sealed class DocumentationSortingAttribute : Attribute
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002CD RID: 717 RVA: 0x00012DF5 File Offset: 0x00010FF5
		// (set) Token: 0x060002CE RID: 718 RVA: 0x00012DFD File Offset: 0x00010FFD
		public DocumentationSortingAttribute.Level Category { get; private set; }

		// Token: 0x060002CF RID: 719 RVA: 0x00012E06 File Offset: 0x00011006
		public DocumentationSortingAttribute(DocumentationSortingAttribute.Level category)
		{
			this.Category = category;
		}

		// Token: 0x020000AF RID: 175
		public enum Level
		{
			// Token: 0x0400038A RID: 906
			Undoc,
			// Token: 0x0400038B RID: 907
			API,
			// Token: 0x0400038C RID: 908
			UserRef
		}
	}
}
