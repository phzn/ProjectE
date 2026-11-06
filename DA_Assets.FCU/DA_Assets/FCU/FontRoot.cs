using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DA_Assets.FCU
{
	// Token: 0x0200002F RID: 47
	public struct FontRoot
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000BB RID: 187 RVA: 0x0000377E File Offset: 0x0000197E
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00003786 File Offset: 0x00001986
		[JsonProperty("kind")]
		public string Kind { readonly get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000BD RID: 189 RVA: 0x0000378F File Offset: 0x0000198F
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00003797 File Offset: 0x00001997
		[JsonProperty("items")]
		public List<FontItem> Items { readonly get; set; }
	}
}
