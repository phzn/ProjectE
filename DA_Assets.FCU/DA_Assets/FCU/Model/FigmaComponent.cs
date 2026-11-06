using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000055 RID: 85
	public struct FigmaComponent
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00007319 File Offset: 0x00005519
		// (set) Token: 0x0600026E RID: 622 RVA: 0x00007321 File Offset: 0x00005521
		[JsonProperty("key")]
		public string Key { readonly get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000732A File Offset: 0x0000552A
		// (set) Token: 0x06000270 RID: 624 RVA: 0x00007332 File Offset: 0x00005532
		[JsonProperty("name")]
		public string Name { readonly get; set; }
	}
}
