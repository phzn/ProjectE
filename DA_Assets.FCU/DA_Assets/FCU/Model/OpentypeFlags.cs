using System;
using Newtonsoft.Json;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200006A RID: 106
	public struct OpentypeFlags
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000317 RID: 791 RVA: 0x000078AD File Offset: 0x00005AAD
		// (set) Token: 0x06000318 RID: 792 RVA: 0x000078B5 File Offset: 0x00005AB5
		[JsonProperty("CASE")]
		public int Case { readonly get; set; }
	}
}
