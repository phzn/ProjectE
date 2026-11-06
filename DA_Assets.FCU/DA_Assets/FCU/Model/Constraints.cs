using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000057 RID: 87
	public struct Constraints
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000737F File Offset: 0x0000557F
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00007387 File Offset: 0x00005587
		[JsonProperty("vertical")]
		public string Vertical { readonly get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00007390 File Offset: 0x00005590
		// (set) Token: 0x0600027C RID: 636 RVA: 0x00007398 File Offset: 0x00005598
		[JsonProperty("horizontal")]
		public string Horizontal { readonly get; set; }
	}
}
