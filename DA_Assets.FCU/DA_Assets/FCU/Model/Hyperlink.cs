using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000064 RID: 100
	public struct Hyperlink
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060002EB RID: 747 RVA: 0x00007737 File Offset: 0x00005937
		// (set) Token: 0x060002EC RID: 748 RVA: 0x0000773F File Offset: 0x0000593F
		[JsonProperty("type")]
		public HyperlinkType Type { readonly get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00007748 File Offset: 0x00005948
		// (set) Token: 0x060002EE RID: 750 RVA: 0x00007750 File Offset: 0x00005950
		[JsonProperty("url")]
		public string Url { readonly get; set; }
	}
}
