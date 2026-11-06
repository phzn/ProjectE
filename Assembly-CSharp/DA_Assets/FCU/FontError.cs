using System;

namespace DA_Assets.FCU
{
	// Token: 0x02000305 RID: 773
	internal struct FontError
	{
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x0008A8E6 File Offset: 0x00088AE6
		// (set) Token: 0x06001559 RID: 5465 RVA: 0x0008A8EE File Offset: 0x00088AEE
		public FontStruct FontStruct { readonly get; set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x0008A8F7 File Offset: 0x00088AF7
		// (set) Token: 0x0600155B RID: 5467 RVA: 0x0008A8FF File Offset: 0x00088AFF
		public WebError Error { readonly get; set; }
	}
}
